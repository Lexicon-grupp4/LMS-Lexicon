using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Shared.DTOs.UserDtos;
using LMS.Shared.Enum;
using LMS.Shared.Paging;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;

namespace LMS.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(
            IUnitOfWork uow,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _uow = uow;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<CreateUserResponseDto> CreateUserAsync(
    CreateUserDto dto)
        {
            ValidateRole(dto.Role);

            var user = _mapper.Map<ApplicationUser>(dto);

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                ThrowIdentityErrors(result);
            }

            var roleResult =
                await _userManager.AddToRoleAsync(
                    user,
                    dto.Role!);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                ThrowIdentityErrors(roleResult);
            }

            var token =
                await _userManager.GeneratePasswordResetTokenAsync(user);

            var userDto =
                _mapper.Map<UserDto>(user);

            userDto.Role = dto.Role;

            return new CreateUserResponseDto
            {
                User = userDto,
                PasswordSetupToken = token
            };
        }
        public async Task SetPasswordAsync(SetPasswordDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                throw new ArgumentException(
                    "Passwords do not match.");
            }

            var user =
                await _userManager.FindByIdAsync(dto.UserId);

            if (user is null)
            {
                throw new KeyNotFoundException(
                    "User not found.");
            }

            var result =
                await _userManager.ResetPasswordAsync(
                    user,
                    dto.Token,
                    dto.Password);

            if (!result.Succeeded)
            {
                ThrowIdentityErrors(result);
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            var user =
                await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new Exception(
                    $"{id} not found");
            }

            var result =
                await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                ThrowIdentityErrors(result);
            }
        }

        public async Task<UserDto> GetUserAsync(
            string id,
            bool includeCourse = false,
            bool trackChanges = false)
        {
            var user =
                await _uow.UserRepsoitory.GetUserByIdAsync(
                    id,
                    includeCourse,
                    trackChanges);

            if (user == null)
            {
                throw new Exception(
                    $"{id} not found");
            }

            var userDto =
                _mapper.Map<UserDto>(user);

            var roles =
                await _userManager.GetRolesAsync(user);

            userDto.Role =
                roles.FirstOrDefault() ?? string.Empty;

            return userDto;
        }

        public async Task<PagedResponse<UserDto>> GetUsersAsync(
     QueryParameters query,
     bool trackChanges = false)
        {
            var users =
                await _uow.UserRepsoitory
                    .GetAllUsersWithRolesAsync(query);

            var userDtos = users.Items
                .Select(x =>
                {
                    var dto = _mapper.Map<UserDto>(x.User);
                    dto.Role = x.Role ?? string.Empty;
                    return dto;
                })
                .ToList();

            return new PagedResponse<UserDto>(
                userDtos,
                query.PageNumber,
                query.PageSize,
                users.TotalCount);
        }

        public async Task<UserDto> UpdateUserAsync(
            string id,
            UpdateUserDto dto)
        {
            var user =
                await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new Exception(
                    $"{id} not found");
            }

            // Validate role before changing anything
            if (!string.IsNullOrWhiteSpace(dto.Role))
            {
                ValidateRole(dto.Role);
            }

            // Update normal user properties
            _mapper.Map(dto, user);

            var result =
                await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                ThrowIdentityErrors(result);
            }

            // Update role
            if (!string.IsNullOrWhiteSpace(dto.Role))
            {
                var currentRoles =
                    await _userManager.GetRolesAsync(user);

                if (currentRoles.Any())
                {
                    var removeResult =
                        await _userManager.RemoveFromRolesAsync(
                            user,
                            currentRoles);

                    if (!removeResult.Succeeded)
                    {
                        ThrowIdentityErrors(removeResult);
                    }
                }

                var addResult =
                    await _userManager.AddToRoleAsync(
                        user,
                        dto.Role);

                if (!addResult.Succeeded)
                {
                    ThrowIdentityErrors(addResult);
                }
            }

            var userDto =
                _mapper.Map<UserDto>(user);

            var roles =
                await _userManager.GetRolesAsync(user);

            userDto.Role =
                roles.FirstOrDefault() ?? string.Empty;

            return userDto;
        }

        private static void ValidateRole(string? role)
        {
            if (role != UserRole.Teacher.ToString() &&
                role != UserRole.Student.ToString())
            {
                throw new ArgumentException(
                    "Role must be either Teacher or Student.");
            }
        }

        private static void ThrowIdentityErrors(
            IdentityResult result)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(e => e.Description));

            throw new Exception(errors);
        }
    }
}