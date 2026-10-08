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
            // ApplicationUser->UserDto
        CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.Ignore());

            // UserDto -> ApplicationUser
            CreateMap<UserDto, ApplicationUser>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedUserName,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Course,
                    opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken,
                    opt => opt.Ignore())
                .ForMember(dest => dest.RefreshTokenExpireTime,
                    opt => opt.Ignore());

            // CreateUserDto -> ApplicationUser
            CreateMap<CreateUserDto, ApplicationUser>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedUserName,
                    opt => opt.Ignore());

            // UpdateUserDto -> ApplicationUser
            CreateMap<UpdateUserDto, ApplicationUser>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedUserName,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Course,
                    opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken,
                    opt => opt.Ignore())
                .ForMember(dest => dest.RefreshTokenExpireTime,
                    opt => opt.Ignore());
        }
    }
}
