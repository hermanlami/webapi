using AutoMapper;
using Moq;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.BLL.Services;
using TaskManagementSystem.Common.CustomExceptions;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.UnitTesting
{
    [TestFixture]
    public class Tests
    {
        private IProjectsService _projectsService;
        private Mock<IProjectsRepository> _projectsRepository;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            _projectsRepository = new Mock<IProjectsRepository>();
            _mapperMock = new Mock<IMapper>();
            _projectsService = new ProjectsService(_projectsRepository.Object, _mapperMock.Object);
        }
        /// <summary>
        /// Kontrollon nese metoda AddProject hston nje modle bosh.
        /// </summary>
        [Test]
        public async System.Threading.Tasks.Task AddProject_ThrowsCustomException_WhenModelIsEmpty()
        {
            var project = new DAL.Entities.Project
            {

            };

            var dtoProject = new Project { };

            _mapperMock.Setup(x => x.Map<DAL.Entities.Project>(It.IsAny<Project>()))
                       .Returns(project);

            _projectsRepository.Setup(x => x.AddProject(project))
                               .ReturnsAsync(project);

            Assert.ThrowsAsync<CustomException>(async () => await _projectsService.AddProject(dtoProject));

        }

        /// <summary>
        /// Kontrollon cfare kthen metoda GetProjectById nese projekti me ate id nuk ekziston
        /// </summary>
        [Test]
        public async System.Threading.Tasks.Task GetProjectByName_ThrowsNotFoundException_WhenObjectDoesNotExist()
        {
            string name = "ProjectName";

            _projectsRepository.Setup(x => x.GetProjectByName(name, 0, 0))
                               .ReturnsAsync((DAL.Entities.Project)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _projectsService.GetProjectByName(name, " ", 0));

        }
        /// <summary>
        /// Kontrollon cfare kthen metoda DeleteProject nese projekti nuk ekziston
        /// </summary>
        [Test]
        public async System.Threading.Tasks.Task DeleteProject_ThrowsNotFoundException_WhenObjectDoesNotExist()
        {
            string projectName = "ProjectName";

            _projectsRepository.Setup(x => x.GetProjectByName(projectName, 0, 0))
                               .ReturnsAsync((DAL.Entities.Project)null);

            NotFoundException notFoundException = Assert.ThrowsAsync<NotFoundException>(async () => await _projectsService.DeleteProject(projectName));
            Assert.AreEqual("Project not found", notFoundException.Message);

            _projectsRepository.Setup(x => x.DeleteProject(It.IsAny<DAL.Entities.Project>()))
                               .ReturnsAsync(new DAL.Entities.Project());

            Assert.ThrowsAsync<NotFoundException>(async () => await _projectsService.DeleteProject(projectName));
        }

        /// <summary>
        /// Kontrollon cfare kthen metoda UpdateProject kur projekti nuk ekziston
        /// </summary>
        [Test]
        public async System.Threading.Tasks.Task UpdateProject_ThrowsNotFoundException_WhenProjectDoesNotExist()
        {
            string projectName = "ProjectName";
            Project model = new Project
            {
                Name = projectName,
            };

            _projectsRepository.Setup(x => x.GetProjectByName(projectName, 0, 0))
                               .ReturnsAsync((DAL.Entities.Project)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _projectsService.UpdateProject(projectName, model));
        }
    }
}