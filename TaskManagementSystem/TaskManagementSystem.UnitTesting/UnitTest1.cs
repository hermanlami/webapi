using Moq;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.BLL.Services;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.UnitTesting
{
    [TestFixture]
    public class Tests
    {
        private IProjectsService _projectsService;
        private Mock<IProjectsRepository> _projectsRepository;

        [SetUp]
        public void Setup()
        {
            _projectsRepository = new Mock<IProjectsRepository>();
            _projectsService=new ProjectsService(_projectsRepository.Object);
        }

        [Test]
        public async System.Threading.Tasks.Task AddProject_ReturnsEmptyObject_WhenIdIsZero() 
        {
            var project = new DAL.Entities.Project
            {
            };

            var dtoProject = new Project() { };
            _projectsRepository.Setup(x => x.AddProject(project))
                        .ReturnsAsync(new DAL.Entities.Project());

            var result = await _projectsService.AddProject(dtoProject);

            Assert.True(result.Id==0);
        }

        [Test]
        public async System.Threading.Tasks.Task GetProjectById_ReturnsEmptyObject_WhenObjectDoesNotExist()
        {
            int id = 0;
            _projectsRepository.Setup(x => x.GetProjectById(id))
                        .ReturnsAsync(new DAL.Entities.Project());

            var result = await _projectsService.GetProjectById(id);
            Assert.True(result.Id==0);
        }

        [Test]
        public async System.Threading.Tasks.Task DeleteProject_ReturnsEmptyObject_WhenObjectDoesNotExist()
        {
            var project = new Project
            {
                Id = 0,
                Name = "Project",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                ProjectManagerId = 12
            };

            _projectsRepository.Setup(x => x.GetProjectById(project.Id))
                         .ReturnsAsync(new DAL.Entities.Project());

            var result = await _projectsService.DeleteProject(project);
            Assert.True(result.Id == 0);
        }

        [Test]
        public async System.Threading.Tasks.Task UpdateProject_ReturnsEmptyObject_WhenObjectDoesNotExist()
        {
            var project = new Project
            {
                Id = 0,
                Name = "Project",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                ProjectManagerId = 12
            };

            _projectsRepository.Setup(x => x.GetProjectById(project.Id))
                         .ReturnsAsync(new DAL.Entities.Project());

            var result = await _projectsService.UpdateProject(project);
            Assert.True(result.Id == 0);
        }
    }
}