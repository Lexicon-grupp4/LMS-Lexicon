using LMS.Shared.DTOs.UserDtos;
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
    [Route("api/users")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Authorize]

    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<PagedResponse<UserDto>>> GetUsers(
            [FromQuery] QueryParameters query)
        {
            var users =
                await _userService.GetUsersAsync(query);

            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(
            string id)
        {
            var user =
                await _userService.GetUserAsync(id);

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(
            [FromBody] CreateUserDto dto)
        {
            var user =
                await _userService.CreateUserAsync(dto);

            return CreatedAtAction(
                nameof(GetUser),
                new { id = user.Id },
                user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(
            string id,
            [FromBody] UpdateUserDto dto)
        {
            var user =
                await _userService.UpdateUserAsync(
                    id,
                    dto);

            return Ok(user);
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(
            string id)
        {
            await _userService.DeleteUserAsync(id);

            return NoContent();
        }
    }
}
