﻿using AutoMapper;
using Serilog;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Common;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.BLL.Services
{
    internal class TagsService : ITagsService
    {
        private readonly ITagsRepository _repository;
        private readonly IMapper _mapper;
        public TagsService(ITagsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Tag> AddTag(Tag model)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                if (await _repository.GetTagByName(model.Name) != null)
                {
                    Log.Error($"Tag {model.Name} already exists");
                    throw new CustomException($"Tag {model.Name} already exists");
                }
                var addedTag = await _repository.AddTag(_mapper.Map<DAL.Entities.Tag>(model));
                if (addedTag.Id > 0)
                {
                    Log.Information($"Tag {addedTag.Name} added successfully");
                    return _mapper.Map<Tag>(addedTag);
                }

                Log.Error("Tag could not be added");
                throw new CustomException("Tag could not be added");

            });
        }

        public async Task<Tag> DeleteTag(int id)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var tag = await _repository.GetTagById(id);
                if (tag == null)
                {
                    Log.Error("Tag not found");
                    throw new CustomException("Tag not found");

                }

                tag.IsDeleted = true;
                
                var deletedTag = await _repository.DeleteTag(tag);
                if (deletedTag != null)
                {
                    Log.Information($"Tag {deletedTag.Name} deleted successfully");
                    return _mapper.Map<Tag>(deletedTag);
                }

                Log.Error("Tag could not be deleted");
                throw new CustomException("Tag could not be deleted");
            });
        }

        public async Task<Tag> GetTagById(int id)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var tag = await _repository.GetTagById(id);
                if (tag == null)
                {
                    Log.Error("Tag not found");
                    throw new CustomException("Tag not found");

                }
                Log.Information($"Tag {tag.Name} retrieved successfully");
                return _mapper.Map<Tag>(tag);
            });
        }

        public async Task<Tag> GetTagByName(string name)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var tag = await _repository.GetTagByName(name);
                if (tag != null)
                {
                    Log.Information($"Tag {name} retrieved successfully");
                    return _mapper.Map<Tag>(tag);
                }
                Log.Error("Tag not found");
                throw new CustomException("Tag not found");
            });
        }

        public async Task<List<Tag>> GetTags()
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var tags = await _repository.GetTags();
                if (tags != null)
                {
                    Log.Information("Tags retrieved successfully");
                    return _mapper.Map<List<Tag>>(tags);
                }

                Log.Error("Tags could not be retrieved");
                throw new CustomException("Tag could not be retrieved");

            });
        }
    }
}
