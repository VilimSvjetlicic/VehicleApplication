using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleApplication.Model;
using VehicleApplication.Service.Common;
using VehicleApplication.WebAPI.Controllers;
using VehicleApplication.WebAPI.Models;

namespace VehicleApplication.WebAPI.Tests
{
    public class VehicleModelControllerTests
    {
        private readonly Mock<IVehicleService<VehicleModel>> mockService;
        private readonly Mock<IMapper> mockMapper;
        private readonly VehicleModelController controller;

        public VehicleModelControllerTests()
        {
            mockService = new Mock<IVehicleService<VehicleModel>>();
            mockMapper = new Mock<IMapper>();
            controller = new VehicleModelController(mockService.Object, mockMapper.Object);
        }

        [Fact]
        public async Task GetVehicleModels_OkWithMappedDtos()
        {
            var models = new List<VehicleModel>
            {
                new VehicleModel { Id = 1, Name = "Giulia", Abrv = "GLA" }
            };
            var dtos = new List<VehicleModelDto>
            {
                new VehicleModelDto { Id = 1, Name = "Giulia", Abrv = "GLA" }
            };

            mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(models);
            mockMapper.Setup(m => m.Map<IEnumerable<VehicleModelDto>>(models)).Returns(dtos);

            var result = await controller.GetVehicleModels();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<VehicleModelDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetVehicleModel_NotFound()
        {
            mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((VehicleModel)null);

            var result = await controller.GetVehicleModel(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetVehicleModel_OkWithMappedDto_ModelExists()
        {
            var model = new VehicleModel { Id = 1, Name = "Giulia", Abrv = "GLA" };
            var dto = new VehicleModelDto { Id = 1, Name = "Giulia", Abrv = "GLA" };

            mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(model);
            mockMapper.Setup(m => m.Map<VehicleModelDto>(model)).Returns(dto);

            var result = await controller.GetVehicleModel(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dto, okResult.Value);
        }

        [Fact]
        public async Task PostVehicleModel_BadRequest_ModelInvalid()
        {
            controller.ModelState.AddModelError("Name", "Required");

            var result = await controller.PostVehicleModel(new VehicleModelDto());

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostVehicleModel_Created_Successful()
        {
            var dto = new VehicleModelDto { Name = "Giulia", Abrv = "GLA" };
            var model = new VehicleModel { Id = 1, Name = "Giulia", Abrv = "GLA" };

            mockMapper.Setup(m => m.Map<VehicleModel>(dto)).Returns(model);
            mockService.Setup(s => s.AddAsync(model)).ReturnsAsync(model);
            mockService.Setup(s => s.CommitAsync()).ReturnsAsync(true);

            var result = await controller.PostVehicleModel(dto);

            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(VehicleModelController.GetVehicleModel), createdAtResult.ActionName);
        }

        [Fact]
        public async Task PostVehicleModel_BadRequest_CommitFails()
        {
            var dto = new VehicleModelDto { Name = "Giulia", Abrv = "GLA" };
            var model = new VehicleModel { Id = 1, Name = "Giulia", Abrv = "GLA" };

            mockMapper.Setup(m => m.Map<VehicleModel>(dto)).Returns(model);
            mockService.Setup(s => s.AddAsync(model)).ReturnsAsync(model);
            mockService.Setup(s => s.CommitAsync()).ReturnsAsync(false);

            var result = await controller.PostVehicleModel(dto);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PutVehicleModel_BadRequest_IdMismatch()
        {
            var dto = new VehicleModelDto { Id = 2, Name = "UpdateModel" };

            var result = await controller.PutVehicleModel(1, dto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutVehicleModel_BadRequest_ModelInvalid()
        {
            controller.ModelState.AddModelError("Name", "Required");
            var dto = new VehicleModelDto { Id = 1 };

            var result = await controller.PutVehicleModel(1, dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PutVehicleModel_NotFound_UpdateFails()
        {
            var dto = new VehicleModelDto { Id = 1, Name = "UpdateModel" };
            var model = new VehicleModel { Id = 1, Name = "UpdateModel" };

            mockMapper.Setup(m => m.Map<VehicleModel>(dto)).Returns(model);
            mockService.Setup(s => s.UpdateAsync(model)).ReturnsAsync(false);
            mockService.Setup(s => s.CommitAsync()).ReturnsAsync(true);

            var result = await controller.PutVehicleModel(1, dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutVehicleModel_NoContent_Successful()
        {
            var dto = new VehicleModelDto { Id = 1, Name = "UpdateModel" };
            var model = new VehicleModel { Id = 1, Name = "UpdateModel" };

            mockMapper.Setup(m => m.Map<VehicleModel>(dto)).Returns(model);
            mockService.Setup(s => s.UpdateAsync(model)).ReturnsAsync(true);
            mockService.Setup(s => s.CommitAsync()).ReturnsAsync(true);

            var result = await controller.PutVehicleModel(1, dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteVehicleModel_NotFound_DeleteFails()
        {
            mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);
            mockService.Setup(s => s.CommitAsync()).ReturnsAsync(true);

            var result = await controller.DeleteVehicleModel(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteVehicleModel_NoContent_Successful()
        {
            mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);
            mockService.Setup(s => s.CommitAsync()).ReturnsAsync(true);

            var result = await controller.DeleteVehicleModel(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteVehicleModel_NotFound_CommitFails()
        {
            mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);
            mockService.Setup(s => s.CommitAsync()).ReturnsAsync(false);

            var result = await controller.DeleteVehicleModel(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
