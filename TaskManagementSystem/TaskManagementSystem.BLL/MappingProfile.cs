using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.BLL.DTO;

namespace TaskManagementSystem.BLL
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Developer, DAL.Entities.Developer>().ReverseMap();

            CreateMap<ProjectManager, DAL.Entities.ProjectManager>().ReverseMap();

            CreateMap<DAL.Entities.Project, Project>().ForMember(dest => dest.ProjectManager, opt => opt.MapFrom(src => src.ProjectManager.FirstName + " " + src.ProjectManager.LastName));

            CreateMap<Project, DAL.Entities.Project>().ForMember(dest => dest.ProjectManager, opt => opt.Ignore());

            CreateMap<Tag, DAL.Entities.Tag>().ReverseMap();

            CreateMap<DAL.Entities.Task,DTO.Task>().ForMember(dest => dest.Developer, opt => opt.MapFrom(src => src.Developer.FirstName + " " + src.Developer.LastName));

            CreateMap<DTO.Task, DAL.Entities.Task>().ForMember(dest => dest.Developer, opt => opt.Ignore());

            CreateMap<TaskTag, DAL.Entities.TaskTag>().ReverseMap();


            CreateMap<DAL.Entities.Admin, Person>();
            CreateMap<DAL.Entities.Developer, Person>();
            CreateMap<DAL.Entities.ProjectManager, Person>();

        }
    }
}
