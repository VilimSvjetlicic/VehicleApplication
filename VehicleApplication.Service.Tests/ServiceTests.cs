using Autofac.Core;
using FluentAssertions;
using Moq;
using VehicleApplication.Common;
using VehicleApplication.DAL;
using VehicleApplication.Model;
using VehicleApplication.Model.Common;
using VehicleApplication.Repository;
using VehicleApplication.Service.Common;

namespace VehicleApplication.Service.Tests
{
    public class ServiceTests
    {

        private Mock<IGenericRepository<VehicleModel>> mockRepository;
        private VehicleService<VehicleModel> vehicleService;

        public ServiceTests()
        {
            mockRepository = new Mock<IGenericRepository<VehicleModel>>();

            vehicleService = new VehicleService<VehicleModel>(mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnAllVehicles()
        {
            var mockData = new List<VehicleModel>
        {
            new VehicleModel { Id = 1, Name = "Model X", Abrv = "MX", MakeId = 1 },
            new VehicleModel { Id = 2, Name = "Model Y", Abrv = "MY", MakeId = 2 }
        };
            mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(mockData);

            var result = await vehicleService.GetAllAsync();

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnVehicle()
        {
            var mockData = new VehicleModel { Id = 1, Name = "Model X", Abrv = "MX", MakeId = 1 };
            mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(mockData);

            var result = await vehicleService.GetByIdAsync(1);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task AddAsync_InvokeRepositoryAdd()
        {
            var newVehicle = new VehicleModel { Id = 3, Name = "Polo", Abrv = "PLO", MakeId = 3 };

            await vehicleService.AddAsync(newVehicle);

            mockRepository.Verify(repo => repo.AddAsync(newVehicle), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_InvokeRepositoryUpdate()
        {
            var existingVehicle = new VehicleModel { Id = 2, Name = "Focus", Abrv = "FOCUS", MakeId = 2 };

            await vehicleService.UpdateAsync(existingVehicle);

            mockRepository.Verify(repo => repo.UpdateAsync(existingVehicle), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_InvokeRepositoryDelete()
        {
            await vehicleService.DeleteAsync(1);

            mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task CommitAsync_InvokeRepositoryCommit()
        {
            await vehicleService.CommitAsync();

            mockRepository.Verify(repo => repo.CommitAsync(), Times.Once);
        }

        private IEnumerable<VehicleModel> getMockData()
        {
            return new List<VehicleModel>
        {
            new VehicleModel { Id = 1, Name = "Model X", Abrv = "MX", MakeId = 1 },
            new VehicleModel { Id = 2, Name = "Model Y", Abrv = "MY", MakeId = 2 }
        };
        }
    }
}