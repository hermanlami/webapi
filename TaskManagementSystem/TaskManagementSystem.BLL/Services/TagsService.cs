using AutoMapper;
using Serilog;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
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
            try
            {
                var dalTag =_mapper.Map<DAL.Entities.Tag>(model);
                var addedTag = await _repository.AddTag(dalTag);
                if (addedTag.Id > 0)
                {
                    Log.Information("Tag added successfully");
                    return _mapper.Map<Tag>(addedTag);
                }
                else
                {
                    Log.Error("Tag could not be added");

                }

            }
            catch (Exception ex)
            {

            }
            return new DTO.Tag();
        }

        public async Task<Tag> DeleteTag(int id)
        {
            try
            {
                var tag = await _repository.GetTagById(id);
                if (tag != null)
                {
                    tag.IsDeleted = true;

                    var deletedTag = await _repository.DeleteTag(tag);
                    if (deletedTag != null)
                    {
                        Log.Information("Tag deleted successfully");

                        return _mapper.Map<Tag>(deletedTag);

                    }
                    else
                    {
                        Log.Error("Tag could not be deleted");

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
                    Log.Information("Tag retrieved successfully");

                    return _mapper.Map<Tag>(tag);
                }
                else
                {
                    Log.Error("Tag could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new DTO.Tag();
        }

        public async Task<Tag> GetTagByName(string name)
        {
            try
            {
                var tag = await _repository.GetTagByName(name);
                if (tag != null)
                {
                    Log.Information("Tag retrieved successfully");

                    return _mapper.Map<Tag>(tag);
                }
                else
                {
                    Log.Error("Tag could not be retrieved");

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
                    Log.Information("Tags retrieved successfully");
                    return _mapper.Map<List<Tag>>(tags);
                }
                else
                {
                    Log.Error("Tags could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new List<Tag>();
        }
    }
}
