using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.ReadModels;
using LMS.Infrastructure.Data;
using LMS.Infrastructure.Extensions;
using LMS.Infrastructure.Paging;
using LMS.Shared.DTOs.UserDtos;
using LMS.Shared.Paging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LMS.Infrastructure.Repositories
{

    public class UserRepository : RepositoryBase<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(
            string email,
            bool includeCourse = false,
            bool trackChanges = false)
        {
            IQueryable<ApplicationUser> query = _context.Users;

            if (includeCourse)
            {
                query = query.Include(u => u.Course);
            }

            return await query
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(
            string id,
            bool includeCourse = false,
            bool trackChanges = false)
        {
            IQueryable<ApplicationUser> query = _context.Users;

            if (includeCourse)
            {
                query = query.Include(u => u.Course);
            }

            return await query
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IPagedList<ApplicationUser>> GetAllUsersAsync(
            QueryParameters parameters,
            bool includeCourse = false,
            bool trackChanges = false)
        {
            IQueryable<ApplicationUser> query = GetAllUsersQuery(includeCourse, trackChanges);

            return await query.ToPagedListAsync(parameters);
        }
        private IQueryable<ApplicationUser> GetAllUsersQuery(bool includeCourse, bool trackChanges)
        {
            return includeCourse ? FindAll(trackChanges)
                                        .Include(u => u.Course)
                                        :
                                       FindAll(trackChanges);
        }
        public async Task<IPagedList<UserWithRole>> GetAllUsersWithRolesAsync(
        QueryParameters parameters)
        {
            var query =
                from user in _context.Users
                join userRole in _context.UserRoles
                    on user.Id equals userRole.UserId
                join role in _context.Roles
                    on userRole.RoleId equals role.Id
                select new UserWithRole
                {
                    User = user,
                    Role = role.Name
                };

            return await query.ToPagedListAsync(parameters);
        }
    }

}
