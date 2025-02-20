using Microsoft.EntityFrameworkCore;
using Moq;
using VehicleApplication.Common;
using VehicleApplication.DAL;
using VehicleApplication.Model;
using VehicleApplication.Model.Common;
using VehicleApplication.Repository;
using Xunit;
using FluentAssertions;


namespace VehicleApplication.Repository.Tests
{
    public class RepositoryTests: IDisposable
    {
        private GenericRepository<VehicleModel> repository;
        private UnitOfWork mockUnitOfWork;
        private VehicleContext context;

        public RepositoryTests()
        {
            var options = new DbContextOptionsBuilder<VehicleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            context = new VehicleContext(options);

            mockUnitOfWork = new UnitOfWork(context);

            repository = new GenericRepository<VehicleModel>(mockUnitOfWork);
        }

        // GetAllAsync
        [Fact]
        public async Task GetAllAsync_returnsAllModels()
        {
            var testData = new List<VehicleModel>
            {
                new VehicleModel { Id = 1, Name = "X5", Abrv = "X5", MakeId = 1 },
                new VehicleModel { Id = 2, Name = "Focus", Abrv = "FCS", MakeId = 2 }
            };

            await context.VehicleModels.AddRangeAsync(testData);
            await context.SaveChangesAsync();

            var result = await repository.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(v => v.Name == "X5");
            result.Should().Contain(v => v.Name == "Focus");
        }

        // GetByIdAsync
        [Fact]
        public async Task GetByIdAsync_ReturnModel()
        {
            var testData = new VehicleModel { Id = 1, Name = "X5", Abrv = "X5", MakeId = 1 };
            await context.VehicleModels.AddAsync(testData);
            await context.SaveChangesAsync();

            var result = await repository.GetByIdAsync(1);

            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("X5");
        }

        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnNull()
        {
            var result = await repository.GetByIdAsync(-1);

            result.Should().BeNull();
        }

        // AddAsync
        [Fact]
        public async Task AddAsync_ValidModel()
        {
            var newModel = new VehicleModel { Id = 3, Name = "Golf", Abrv = "GLF", MakeId = 3 };

            await repository.AddAsync(newModel);
            await repository.CommitAsync();

            var addedModel = await context.VehicleModels.FindAsync(3);
            addedModel.Should().NotBeNull();
            addedModel.Name.Should().Be("Golf");
        }

        // Update
        [Fact]
        public async Task Update_ValidModel()
        {
            var modelToUpdate = new VehicleModel { Id = 1, Name = "X5", Abrv = "X5", MakeId = 1 };
            await context.VehicleModels.AddAsync(modelToUpdate);
            await context.SaveChangesAsync();

            modelToUpdate.Name = "UpdatedName";

            await repository.UpdateAsync(modelToUpdate);
            await repository.CommitAsync();

            var updatedModel = await context.VehicleModels.FindAsync(1);
            updatedModel.Should().NotBeNull();
            updatedModel.Name.Should().Be("UpdatedName");
        }

        // Delete
        [Fact]
        public async Task Delete_ValidModel()
        {
            var testData = new VehicleModel { Id = 1, Name = "X5", Abrv = "X5", MakeId = 1 };
            await context.VehicleModels.AddAsync(testData);
            await context.SaveChangesAsync();

            var result = await repository.DeleteAsync(1);
            await repository.CommitAsync();

            result.Should().BeTrue();
            var deletedModel = await context.VehicleModels.FindAsync(1);
            deletedModel.Should().BeNull();
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }

}
