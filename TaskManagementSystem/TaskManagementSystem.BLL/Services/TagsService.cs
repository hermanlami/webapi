using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.BLL.Services
{
    internal class TagsService : ITagsService
    {
        private readonly ITagsRepository _repository;
        private readonly ILogger<TagsService> _logger;
        public TagsService(ITagsRepository repository, ILogger<TagsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<Tag> AddTag(Tag model)
        {
            try
            {
                var dalTag = new DAL.Entities.Tag()
                {
                    Name = model.Name,
                };
                var addedTag = await _repository.AddTag(dalTag);
                model.Id = addedTag.Id;
                if (addedTag.Id > 0)
                {
                    _logger.LogInformation("Tag added successfully");
                    return model;
                }
                else
                {
                    _logger.LogError("Tag could not be added");

                }

            }
            catch (Exception ex)
            {

            }
            return new DTO.Tag();
        }

        public async Task<Tag> DeleteTag(Tag model)
        {
            try
            {
                var tag = await _repository.GetTagById(model.Id);
                if (tag != null)
                {
                    tag.IsDeleted = true;

                    var deletedTag = await _repository.DeleteTag(tag);
                    if (deletedTag != null)
                    {
                        _logger.LogInformation("Tag deleted successfully");

                        return new DTO.Tag()
                        {
                            Name = deletedTag.Name,
                        };

                    }
                    else
                    {
                        _logger.LogError("Tag could not be deleted");

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return new DTO.Tag();
        }

        public async Task<Tag> GetTagById(int id)
        {
            try
            {
                var tag = await _repository.GetTagById(id);
                if (tag != null)
                {
                    _logger.LogInformation("Tag retrieved successfully");

                    return new DTO.Tag()
                    {
                        Id = tag.Id,
                        Name = tag.Name,

                    };
                }
                else
                {
                    _logger.LogError("Tag could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new DTO.Tag();
        }

        public async Task<List<Tag>> GetTags()
        {
            try
            {
                var tags = await _repository.GetTags();
                var dtoTags = new List<Tag>();
                if (tags != null)
                {
                    _logger.LogInformation("Tags retrieved successfully");
                    tags.ForEach(x => dtoTags.Add(new Tag
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }));
                    return dtoTags;
                }
                else
                {
                    _logger.LogError("Tags could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new List<Tag>();
        }
    }
}
