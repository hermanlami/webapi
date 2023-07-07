using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    public class TagsController : BaseController
    {
        private readonly ITagsService _tagsService;
        private readonly ILogger<TagsController> _logger;
        public TagsController(ITagsService tagsService, ILogger<TagsController> logger)
        {
            _tagsService = tagsService;
            _logger = logger;
        }
        [HttpPost]
        [Route("api/tags")]
        public async Task<IActionResult> AddTag([FromBody] Tag model)
        {
            return await HandleExceptionAsync(async () =>
            {
                var tag = await _tagsService.AddTag(model);

                if (tag.Id != null)
                {
                    _logger.LogInformation("Tag added successfully");
                    return Ok(tag);
                }
                else
                {
                    _logger.LogError("Tag could not be added");
                    return BadRequest("Failed to add tag.");
                }
            });

        }
        [HttpGet]
        [Route("api/tags/{id}")]
        public async Task<IActionResult> GetTag(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var tag = await _tagsService.GetTagById(id);

                if (tag != null)
                {
                    _logger.LogInformation("Tag retrieved successfully");
                    return Ok(tag);
                }
                else
                {
                    _logger.LogError("Tag could not be retrieved");

                    return NotFound();
                }
            });
        }
        [HttpGet]
        [Route("api/tags")]
        public async Task<IActionResult> GetTags()
        {
            return await HandleExceptionAsync(async () =>
            {
                var tags = await _tagsService.GetTags();
                _logger.LogInformation("Tags retrieved successfully");

                return Ok(tags);
            });
        }
        [HttpDelete]
        [Route("api/tags/{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var tag = await _tagsService.GetTagById(id);
                var deleted = await _tagsService.DeleteTag(tag);

                if (deleted.Id != 0)
                {
                    _logger.LogInformation("Tag deleted successfully");

                    return Ok();
                }
                else
                {
                    _logger.LogError("Tag could not be deleted");

                    return NotFound();
                }
            });
        }

    }
}
