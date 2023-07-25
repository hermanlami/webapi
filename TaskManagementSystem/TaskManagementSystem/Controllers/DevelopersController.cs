using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.BLL;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Middlewares;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
   // [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin", "ProjectManager" } })]

    public class DevelopersController : BaseController
    {
        private readonly IDevelopersService _developersService;
        public DevelopersController(IDevelopersService developersService)
        {
            _developersService = developersService;
        }
        /// <summary>
        /// Krijon nje developer te ri.
        /// </summary>
        /// <param name="model">Modeli ne baze te te cilit do te krijohet developer i ri</param>
        [HttpPost]
        [AllowAnonymous]
        [Route("api/developers/add")]
        public async Task<IActionResult> AddDeveloper([FromBody] Developer model)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var developer = await _developersService.AddDeveloper(model);
                return Ok(developer);
            });
        }
        /// <summary>
        /// Merr nje developer ne baze te Id se tij.
        /// </summary>
        /// <param name="id">Id qe identifikon developer-in.</param>
        [HttpGet]
        [AllowAnonymous]
        [Route("api/developers/{id}")]
        public async Task<IActionResult> GetDeveloper(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var developer = await _developersService.GetDeveloperById(id);
                return Ok(developer);

            });
        }
        /// <summary>
        /// Merr te gjithe developers.
        /// </summary>
        [HttpGet]
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

        /// <summary>
        /// Perditeson te dhenat e nje developer.
        /// </summary>
        /// <param name="id">Id qe identifikon developer-in.</param>
        /// <param name="model">Modeli ne baze te te cilit do te behet perditesimi.</param>
        [HttpPut]
        [Route("api/developers/{id}")]
        public async Task<IActionResult> UpdateDeveloper(int id, [FromBody] Developer model)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var developer = await _developersService.UpdateDeveloper(id, model);
                return Ok(developer);

            });
        }
        /// <summary>
        /// Fshin nje developer.
        /// </summary>
        /// <param name="id">Id qe identifikon developer-in.</param>
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
    }
}
