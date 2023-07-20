using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using Serilog;
using Serilog.Configuration;
using TaskManagementSystem.BLL;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [TypeFilter(typeof(RoleActionFilter), Arguments = new object[] { new string[] { "Admin" } })]
    public class TagsController : BaseController
    {
        private readonly ITagsService _tagsService;
        public TagsController(ITagsService tagsService)
        {
            _tagsService = tagsService;
        }
        /// <summary>
        /// Krijon nje tag.
        /// </summary>
        /// <param name="model">Modeli ne baze te te cilit behet krijimi i tag-ut.</param>
        [HttpPost]
        [Route("api/tags")]
        public async Task<IActionResult> AddTag([FromBody] Tag model)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var tag = await _tagsService.AddTag(model);
                return Ok(tag);

            });
        }
        /// <summary>
        /// Merr nje tag ne baze te id se tij.
        /// </summary>
        /// <param name="id">Id qe identifikon tag-un.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/tags/{id}")]
        public async Task<IActionResult> GetTag(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var tag = await _tagsService.GetTagById(id);
                return Ok(tag);

            });
        }
        /// <summary>
        /// Merr te githa tag-et.
        /// </summary>
        [HttpGet]
        [Route("api/tags")]
        public async Task<IActionResult> GetTags()
        {
            return await HandleExceptionAsync(async () =>
            {
                var tags = await _tagsService.GetTags();
                return Ok(tags);
            });
        }
        /// <summary>
        /// Fshin nje tag.
        /// </summary>
        /// <param name="id">Id qe identifikon tag-un.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/tags/{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var deleted = await _tagsService.DeleteTag(id);
                return Ok(deleted);

            });
        }

    }
}
