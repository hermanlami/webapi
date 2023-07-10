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
            CreateMap<Developer, DAL.Entities.Developer>();
            CreateMap<DAL.Entities.Developer, Developer>();

            CreateMap<ProjectManager, DAL.Entities.ProjectManager>();
            CreateMap<DAL.Entities.ProjectManager, ProjectManager>();
            
            CreateMap<Project, DAL.Entities.Project>();
            CreateMap<DAL.Entities.Project, Project>();
            
            CreateMap<Tag, DAL.Entities.Tag>();
            CreateMap<DAL.Entities.Tag, Tag>();
            
            CreateMap<DTO.Task, DAL.Entities.Task>();
            CreateMap<DAL.Entities.Task, DTO.Task>();

        }
    }
}
