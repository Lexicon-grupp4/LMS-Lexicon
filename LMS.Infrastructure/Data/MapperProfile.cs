using AutoMapper;
using Domain.Models.Entities;
using LMS.Shared.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LMS.Infrastructure.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<CreateUserDto, UserDto>();
            CreateMap<UpdateUserDto, ApplicationUser>().ReverseMap();

        }
    }
}
