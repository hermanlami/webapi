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

            CreateMap<Project, DAL.Entities.Project>().ReverseMap();

            CreateMap<Tag, DAL.Entities.Tag>().ReverseMap();

            CreateMap<DTO.Task, DAL.Entities.Task>().ReverseMap();

            CreateMap<TaskTag, DAL.Entities.TaskTag>().ReverseMap();


            CreateMap<DAL.Entities.Admin, Person>();
            CreateMap<DAL.Entities.Developer, Person>();
            CreateMap<DAL.Entities.ProjectManager, Person>();

        }
    }
}
