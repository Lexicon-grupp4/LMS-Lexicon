using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Shared.DTOs.UserDtos;
using LMS.Shared.Enum;
using LMS.Shared.Paging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;


namespace LMS.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(
            IUnitOfWork uow,
            IMapper mapper,
            IEmailService emailService,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {
            _uow = uow;
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
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
            await SendSetPasswordEmailAsync(user);

            //var token =
            //    await _userManager.GeneratePasswordResetTokenAsync(user);

            var userDto =
                _mapper.Map<UserDto>(user);

            userDto.Role = dto.Role;

            return new CreateUserResponseDto
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email!
                }
            };
        }
        public async Task SetPasswordAsync(SetPasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId) ||
                string.IsNullOrWhiteSpace(dto.Token))
            {
                throw new ArgumentException(
                    "The password setup link is invalid.");
            }

            if (dto.Password != dto.ConfirmPassword)
            {
                throw new ArgumentException(
                    "Passwords do not match.");
            }

            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user is null)
            {
                throw new KeyNotFoundException(
                    "User not found.");
            }

            string decodedToken;

            try
            {
                decodedToken = Encoding.UTF8.GetString(
                    WebEncoders.Base64UrlDecode(dto.Token));
            }
            catch (FormatException)
            {
                throw new ArgumentException(
                    "The password setup link is invalid or expired.");
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                decodedToken,
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
        private async Task SendSetPasswordEmailAsync(
    ApplicationUser user)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new InvalidOperationException(
                    "The user does not have an email address.");
            }

            var token =
                await _userManager.GeneratePasswordResetTokenAsync(user);

            // Convert the Identity token to URL-safe Base64.
            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token));

            var clientUrl = _configuration[
                "AppUrls:BlazorClient"];

            if (string.IsNullOrWhiteSpace(clientUrl))
            {
                throw new InvalidOperationException(
                    "The Blazor Client URL is not configured.");
            }

            var link =
                $"{clientUrl.TrimEnd('/')}/users/set-password" +
                $"?userId={Uri.EscapeDataString(user.Id)}" +
                $"&token={Uri.EscapeDataString(encodedToken)}";

            var html = $"""
        <h2>Welcome to LMS</h2>
        <p>Your LMS account has been created.</p>
        <p>Use the link below to set your password:</p>
        <p><a href="{link}">Set your password</a></p>
        <p>If you did not expect this email, you can ignore it.</p>
        """;

            await _emailService.SendEmailAsync(
                user.Email,
                "Set your LMS password",
                html);
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