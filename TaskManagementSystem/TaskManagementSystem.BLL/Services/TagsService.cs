using AutoMapper;
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
        private readonly IMapper _mapper;
        public TagsService(ITagsRepository repository, ILogger<TagsService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Tag> AddTag(Tag model)
        {
            try
            {
                var dalTag =_mapper.Map<DAL.Entities.Tag>(model);
                var addedTag = await _repository.AddTag(dalTag);
                if (addedTag.Id > 0)
                {
                    _logger.LogInformation("Tag added successfully");
                    return _mapper.Map<Tag>(addedTag);
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

                        return _mapper.Map<Tag>(deletedTag);

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

                    return _mapper.Map<Tag>(tag);
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
                if (tags != null)
                {
                    _logger.LogInformation("Tags retrieved successfully");
                    return _mapper.Map<List<Tag>>(tags);
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
