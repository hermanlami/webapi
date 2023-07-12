using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagementSystem.BLL;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Middlewares;
using Serilog;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    public class DevelopersController : BaseController
    {
        private readonly IDevelopersService _developersService;
        public DevelopersController(IDevelopersService developersService)
        {
            _developersService = developersService;
        }

        [HttpPost, Authorize]
        [Route("api/developers")]
        public async Task<IActionResult> AddDeveloper([FromBody] Developer model)
        {
            return await HandleExceptionAsync(async () =>
            {
                var developer = await _developersService.AddDeveloper(model);
                return Ok(developer);
            });
        }

        [HttpGet]
        [Route("api/developers/{id}")]
        public async Task<IActionResult> GetDeveloper(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var developer = await _developersService.GetDeveloperById(id);

                return Ok(developer);

            });
        }

        [HttpGet, Authorize]
        [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin", "Developer" } })]
        [Route("api/developers")]
        public async Task<IActionResult> GetDevelopers()
        {
            return await HandleExceptionAsync(async () =>
            {
                var developers = await _developersService.GetDevelopers();

                return Ok(developers);
            });
        }


        [HttpPut]
        [Route("api/developers/{id}")]
        public async Task<IActionResult> UpdateDeveloper(int id, [FromBody] Developer model)
        {
            return await HandleExceptionAsync(async () =>
            {
                var developer = await _developersService.UpdateDeveloper(id, model);
                return Ok(developer);

            });
        }

        [HttpDelete]
        [Route("api/developers/{id}")]
        public async Task<IActionResult> DeleteDeveloper(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var deleted = await _developersService.DeleteDeveloper(id);

                return Ok(deleted);   
            });
        }

        [HttpPost]
        [Route("api/developers/login")]
        public async Task<ActionResult<AuthenticationResponse>> Authenticate([FromBody] AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _developersService.Authenticate(request);
            return Ok(response);
        }
    }
}
