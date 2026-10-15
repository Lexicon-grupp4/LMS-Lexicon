using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.Module;
using LMS.Shared.Paging;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ModuleService(
            IUnitOfWork uow,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<PagedResponse<ModuleDto>?> GetAllAsync(QueryParameters parameters, bool trackChanges = false)
        {
            var modules = await _uow.ModuleRepository.GetAllAsync(parameters, trackChanges);
            return modules != null ? _mapper.Map<PagedResponse<ModuleDto>>(modules) : null;
        }

        public async Task<PagedResponse<ModuleDto>?> GetByCourseIdAsync(int courseId, bool trackChanges = false)
        {
            // Todo: Check if the course exsists; 
            var modules = await _uow.ModuleRepository.GetByCourseIdAsync(courseId, trackChanges);
            return modules != null ? _mapper.Map<PagedResponse<ModuleDto>>(modules) : null;
        }

        public async Task<ModuleDto?> GetByIdAsync(int id, bool trackChanges = false)
        {
            var module = await _uow.ModuleRepository.GetByIdAsync(id, trackChanges);

            if (module is null)
            {
                throw new NotFoundException($"Module with id {id} not found.");
            }

            return _mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDto?> GetByIdWithCourseAsync(int moduleId, bool trackChanges = false)
        {
            var module = await _uow.ModuleRepository.GetByIdWithCourseAsync(moduleId, trackChanges);

            if (module is null)
            {
                throw new NotFoundException($"Module with id {moduleId} not found.");
            }

            return _mapper.Map<ModuleDto>(module);
        }

        public async Task<bool> HasOverlappingDatesAsync(int courseId, DateTime startDate, DateTime endDate, int? excludeModuleId = null)
        {
            //check if the course exists
            //var course = await _uow.CourseRepository.GetByIdAsync(courseId, false);
            //if (course is null)
            //{
            //    throw new NotFoundException($"Course with id {courseId} not found.");
            //}

            return await _uow.ModuleRepository.HasOverlappingDatesAsync(courseId, startDate, endDate, excludeModuleId);
        }
        public async Task<ModuleDto> CreateModuleAsync(CreateModuleDto createModuleDto)
        {
            // Check if the course exists
            //var course = await _uow.CourseRepository.GetByIdAsync(createModuleDto.CourseId, false);
            //if (course is null)
            //{
            //    throw new NotFoundException($"Course with id {createModuleDto.CourseId} not found.");
            //}
            // Check for overlapping dates
            var isOverlapping = await _uow.ModuleRepository.HasOverlappingDatesAsync(createModuleDto.CourseId, createModuleDto.StartDate, createModuleDto.EndDate);
            if (isOverlapping)
            {
                throw new ModuleOverlappingException($"The module dates overlap with an existing module in the course with id {createModuleDto.CourseId}.");
            }

            var moduleEntity = _mapper.Map<Module>(createModuleDto);
            _uow.ModuleRepository.Create(moduleEntity);

            await _uow.CompleteAsync();
            return _mapper.Map<ModuleDto>(moduleEntity);
        }
        public async Task<ModuleDto> UpdateModuleAsync(int moduleId, UpdateModuleDto updateModuleDto)
        {
            var moduleEntity = await _uow.ModuleRepository.GetByIdAsync(moduleId, true);
            if (moduleEntity is null)
            {
                throw new NotFoundException($"Module with id {moduleId} not found.");
            }
            // Check for overlapping dates
            var isOverlapping = await _uow.ModuleRepository.HasOverlappingDatesAsync(moduleEntity.CourseId, updateModuleDto.StartDate, updateModuleDto.EndDate, moduleId);
            if (isOverlapping)
            {
                throw new ModuleOverlappingException($"The module dates overlap with an existing module in the course with id {moduleEntity.CourseId}.");
            }
            _mapper.Map(updateModuleDto, moduleEntity);
            await _uow.CompleteAsync();
            return _mapper.Map<ModuleDto>(moduleEntity);
        }
        public async Task DeleteModuleAsync(int moduleId)
        {
            var moduleEntity = await _uow.ModuleRepository.GetByIdAsync(moduleId, true);
            if (moduleEntity is null)
            {
                throw new NotFoundException($"Module with id {moduleId} not found.");
            }
            _uow.ModuleRepository.Delete(moduleEntity);
            await _uow.CompleteAsync();
        }
    }
}
