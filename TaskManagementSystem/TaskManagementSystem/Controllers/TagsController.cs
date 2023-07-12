using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using Serilog;
using Serilog.Configuration;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    public class TagsController : BaseController
    {
        private readonly ITagsService _tagsService;
        public TagsController(ITagsService tagsService)
        {
            _tagsService = tagsService;
        }

        [HttpPost]
        [Route("api/tags")]
        public async Task<IActionResult> AddTag([FromBody] Tag model)
        {
            return await HandleExceptionAsync(async () =>
            {
                var tag = await _tagsService.AddTag(model);
                return Ok(tag);

            });

        }
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
