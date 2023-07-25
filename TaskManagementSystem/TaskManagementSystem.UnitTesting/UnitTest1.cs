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
        /// <summary>
        /// Kontrollon nese metoda AddProject hston nje modle bosh.
        /// </summary>
        [Test]
        public async System.Threading.Tasks.Task AddProject_ReturnsEmptyObject_WhenModelIsEmpty() 
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
        /// <summary>
        /// Kontrollon cfare kthen metoda GetProjectById nese projekti me ate id nuk ekziston
        /// </summary>
        [Test]
        public async System.Threading.Tasks.Task GetProjectById_ReturnsEmptyObject_WhenObjectDoesNotExist()
        {
            int id = 0;
            _projectsRepository.Setup(x => x.GetProjectById(id))
                        .ReturnsAsync(new DAL.Entities.Project());

            var result = await _projectsService.GetProjectById(id);
            Assert.True(result.Id==0);
        }
        /// <summary>
        /// Kontrollon cfare kthen metoda DeleteProject nese projekti nuk ekziston
        /// </summary>
        [Test]
        public async System.Threading.Tasks.Task DeleteProject_ReturnsEmptyObject_WhenObjectDoesNotExist()
        {
            var project = new Project
            {
                Id = 50,
                Name = "Project",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                ProjectManagerId = 12
            };

            _projectsRepository.Setup(x => x.GetProjectById(project.Id))
                         .ReturnsAsync(new DAL.Entities.Project());

            var result = await _projectsService.DeleteProject(project.Id);
            Assert.True(result.Id == 0);
        }
        /// <summary>
        /// Kontrollon cfare kthen metoda UpdateProject kur projekti nuk ekziston
        /// </summary>
        [Test]
        public async System.Threading.Tasks.Task UpdateProject_ReturnsEmptyObject_WhenObjectDoesNotExist()
        {
            var project = new Project
            {
                Id = 50,
                Name = "Project",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                ProjectManagerId = 12
            };

            _projectsRepository.Setup(x => x.GetProjectById(project.Id))
                         .ReturnsAsync(new DAL.Entities.Project());

            var result = await _projectsService.UpdateProject(project.Id, project);
            Assert.True(result.Id == 0);
        }
    }
}