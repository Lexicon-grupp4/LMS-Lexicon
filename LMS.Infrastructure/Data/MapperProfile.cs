using AutoMapper;
using Domain.Models.Entities;
using LMS.Infrastructure.Paging;
using LMS.Shared.DTOs.ActionType;
using LMS.Shared.DTOs.Activity;
using LMS.Shared.DTOs.Module;
using LMS.Shared.DTOs.UserDtos;
using LMS.Shared.Paging;
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

            // ApplicationUser->CourseParticipantDto
            CreateMap<ApplicationUser, CourseParticipantDto>();

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

            CreateMap<Module, ModuleDto>();
            CreateMap<Module, ModuleListDto>();

            CreateMap<CreateModuleDto, Module>();
            CreateMap<UpdateModuleDto, Module>();

            CreateMap<PagedList<Module>, PagedResponse<ModuleDto>>()
                .ForCtorParam("Items", opt =>
                    opt.MapFrom(src => src.Items))
                .ForCtorParam("PageNumber", opt =>
                    opt.MapFrom(src => 1))
                .ForCtorParam("PageSize", opt =>
                    opt.MapFrom(src => src.Items.Count))
                .ForCtorParam("TotalCount", opt =>
                    opt.MapFrom(src => src.TotalCount));

            // Activity -> ActivityDto
            CreateMap<Activity, ActivityDto>()
                .ForMember(
                    dest => dest.ActionTypeName,
                    opt => opt.MapFrom(src => src.ActionType.Name));

            // CreateActivityDto -> Activity
            CreateMap<CreateActivityDto, Activity>();

            // UpdateActivityDto -> Activity
            CreateMap<UpdateActivityDto, Activity>();

            // PagedList<Activity> -> PagedResponse<ActivityDto>
            CreateMap<PagedList<Activity>, PagedResponse<ActivityDto>>()
                .ForCtorParam("Items", opt =>
                    opt.MapFrom(src => src.Items))
                .ForCtorParam("PageNumber", opt =>
                    opt.MapFrom(src => 1))
                .ForCtorParam("PageSize", opt =>
                    opt.MapFrom(src => src.Items.Count))
                .ForCtorParam("TotalCount", opt =>
                    opt.MapFrom(src => src.TotalCount));


            CreateMap<ActionType, ActionTypeDto>();
            // PagedList<ActionType> -> PagedResponse<ActionTypeDto>
            CreateMap<PagedList<ActionType>, PagedResponse<ActionTypeDto>>()
                .ForCtorParam("Items", opt =>
                    opt.MapFrom(src => src.Items))
                .ForCtorParam("PageNumber", opt =>
                    opt.MapFrom(src => 1))
                .ForCtorParam("PageSize", opt =>
                    opt.MapFrom(src => src.Items.Count))
                .ForCtorParam("TotalCount", opt =>
                    opt.MapFrom(src => src.TotalCount));

        }
    }
}
