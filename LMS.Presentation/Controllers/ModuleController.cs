using LMS.Shared.DTOs.Module;
using LMS.Shared.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/Module")]
    public class ModuleController : ApiControllerBase
    {
        private readonly IModuleService _moduleService;

        public ModuleController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        // GET: api/Module
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
        {
            var modules = await _moduleService.GetAllAsync(parameters);

            return Ok(modules);
        }

        // GET: api/Module/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var module = await _moduleService.GetByIdAsync(id);

            return Ok(module);
        }

        // GET: api/Module/{id}/course
        [HttpGet("{id:int}/course")]
        public async Task<IActionResult> GetByIdWithCourse(int id)
        {
            var module = await _moduleService.GetByIdWithCourseAsync(id);

            return Ok(module);
        }

        // GET: api/Module/course/{courseId}
        [HttpGet("course/{courseId:int}")]
        public async Task<IActionResult> GetByCourseId(int courseId)
        {
            var modules = await _moduleService.GetByCourseIdAsync(courseId);

            return Ok(modules);
        }

        // GET: api/Module/{courseId}/overlapping-dates
        [HttpGet("{courseId:int}/overlapping-dates")]
        public async Task<IActionResult> HasOverlappingDates(
            int courseId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int? excludeModuleId = null)
        {
            var isOverlapping =
                await _moduleService.HasOverlappingDatesAsync(courseId, startDate, endDate, excludeModuleId);

            return Ok(new { isOverlapping });
        }

        // POST: api/Module
        [HttpPost]
        [Authorize(Roles = "Teacher")]

        public async Task<IActionResult> Create([FromBody] CreateModuleDto createModuleDto)
        {
            var module = await _moduleService.CreateModuleAsync(createModuleDto);

            return CreatedAtAction(nameof(GetById), new { id = module.Id }, module);
        }

        // PUT: api/Module/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Update( int id,[FromBody] UpdateModuleDto updateModuleDto)
        {
            var module = await _moduleService.UpdateModuleAsync(id,updateModuleDto);
      
            return Ok(module);
        }

        // DELETE: api/Module/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int id)
        {
            await _moduleService.DeleteModuleAsync(id);

            return NoContent();
        }
    }
}
