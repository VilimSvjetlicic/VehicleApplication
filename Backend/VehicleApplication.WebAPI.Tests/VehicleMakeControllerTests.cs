using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using VehicleApplication.Model;
using VehicleApplication.WebAPI.Controllers;
using VehicleApplication.Service.Common;
using Microsoft.AspNetCore.Mvc;
using VehicleApplication.WebAPI.Models;

namespace VehicleApplication.WebAPI.Tests
{
    public class VehicleMakeControllerTests
    {
        private readonly Mock<IVehicleService<VehicleMake>> mockService;
        private readonly Mock<IMapper> mockMapper;
        private readonly VehicleMakeController controller;

        public VehicleMakeControllerTests()
        {
            mockService = new Mock<IVehicleService<VehicleMake>>();
            mockMapper = new Mock<IMapper>();
            controller = new VehicleMakeController(mockService.Object, mockMapper.Object);
        }

        [Fact]
        public async Task GetVehicleMakes_OkWithMappedDto()
        {
            var makes = new List<VehicleMake>
            {
                new VehicleMake { Id = 1, Name = "Alfa Romeo", Abrv = "ALFA" }
            };
            var dtos = new List<VehicleMakeDto>
            {
                new VehicleMakeDto { Id = 1, Name = "Alfa Romeo", Abrv = "ALFA" }
            };

            mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(makes);
            mockMapper.Setup(m => m.Map<IEnumerable<VehicleMakeDto>>(makes)).Returns(dtos);

            var result = await controller.GetVehicleMakes();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<VehicleMakeDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetVehicleMake_NotFound()
        {
            mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((VehicleMake)null);

            var result = await controller.GetVehicleMake(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetVehicleMake_OkWithMappedDto()
        {
            var make = new VehicleMake { Id = 1, Name = "Alfa Romeo", Abrv = "ALFA" };
            var dto = new VehicleMakeDto { Id = 1, Name = "Alfa Romeo", Abrv = "ALFA" };

            mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(make);
            mockMapper.Setup(m => m.Map<VehicleMakeDto>(make)).Returns(dto);

            var result = await controller.GetVehicleMake(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dto, okResult.Value);
        }

        [Fact]
        public async Task PostVehicleMake_BadRequest_ModelInvalid()
        {
            controller.ModelState.AddModelError("Name", "Required");

            var result = await controller.PostVehicleMake(new VehicleMakeDto());

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostVehicleMake_Created_Successful()
        {
            var make = new VehicleMake { Id = 1, Name = "Alfa Romeo", Abrv = "ALFA" };
            var dto = new VehicleMakeDto { Id = 1, Name = "Alfa Romeo", Abrv = "ALFA" };

            mockMapper.Setup(m => m.Map<VehicleMake>(dto)).Returns(make);
            mockService.Setup(s => s.AddAsync(make)).ReturnsAsync(make);
            mockService.Setup(s => s.CommitAsync()).ReturnsAsync(true);

            var result = await controller.PostVehicleMake(dto);

            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(VehicleMakeController.GetVehicleMake), createdAtResult.ActionName);
        }

        [Fact]
        public async Task PostVehicleMake_BadRequest_CommitFail()
        {
            var make = new VehicleMake { Id = 1, Name = "Alfa Romeo", Abrv = "ALFA" };
            var dto = new VehicleMakeDto { Id = 1, Name = "Alfa Romeo", Abrv = "ALFA" };

            mockMapper.Setup(m => m.Map<VehicleMake>(dto)).Returns(make);
            mockService.Setup(s => s.AddAsync(make)).ReturnsAsync(make);
            mockService.Setup(s => s.CommitAsync()).ReturnsAsync(false);

            var result = await controller.PostVehicleMake(dto);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PutVehicleMake_BadRequest_IdMismatch()
        {
            var dto = new VehicleMakeDto { Id = 2, Name = "UpdateMake" };

            var result = await controller.PutVehicleMake(1, dto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutVehicleMake_BadRequest_ModelInvalid()
        {
            controller.ModelState.AddModelError("Name", "Required");
            var dto = new VehicleMakeDto { Id = 1 };

            var result = await controller.PutVehicleMake(1, dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PutVehicleMake_NotFound_UpdateFails()
        {
            var dto = new VehicleMakeDto { Id = 1, Name = "UpdateMake" };
            var make = new VehicleMake { Id = 1, Name = "UpdateMake" };

            mockMapper.Setup(m => m.Map<VehicleMake>(dto)).Returns(make);
            mockService.Setup(s => s.UpdateAsync(make)).ReturnsAsync(false);
            mockService.Setup(s => s.CommitAsync()).ReturnsAsync(true);

            var result = await controller.PutVehicleMake(1, dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutVehicleMake_NoContent_Successful()
        {
            var dto = new VehicleMakeDto { Id = 1, Name = "UpdateMake" };
            var make = new VehicleMake { Id = 1, Name = "UpdateMake" };

            mockMapper.Setup(m => m.Map<VehicleMake>(dto)).Returns(make);
            mockService.Setup(s => s.UpdateAsync(make)).ReturnsAsync(true);
            mockService.Setup(s => s.CommitAsync()).ReturnsAsync(true);

            var result = await controller.PutVehicleMake(1, dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteVehicleMake_NotFound()
        {
            mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);
            mockService.Setup(s => s.CommitAsync()).ReturnsAsync(true);

            var result = await controller.DeleteVehicleMake(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteVehicleMake_NoContent_Successful()
        {
            mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);
            mockService.Setup(s => s.CommitAsync()).ReturnsAsync(true);

            var result = await controller.DeleteVehicleMake(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteVehicleMake_NotFound_CommitFail()
        {
            mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);
            mockService.Setup(s => s.CommitAsync()).ReturnsAsync(false);

            var result = await controller.DeleteVehicleMake(1);

            Assert.IsType<NotFoundResult>(result);
        }

    }
}
