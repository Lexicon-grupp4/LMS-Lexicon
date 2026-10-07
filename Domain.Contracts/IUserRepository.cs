using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Domain.Contracts
{
    public interface IUserRepository : IRepositoryBase<ApplicationUser>
    {
       Task<ApplicationUser?> GetUserByEmailAsync(bool includeCourses = false, string email=null!);
       Task<ApplicationUser?> GetUserByIdAsync(bool includeCourses = false, string id = null!);

    }
}
