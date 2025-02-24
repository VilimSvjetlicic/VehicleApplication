using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using VehicleApplication.Model;
using VehicleApplication.Service.Common;
using VehicleApplication.WebAPI.Models;

namespace VehicleApplication.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleModelController: ControllerBase
    {
        private readonly IVehicleService<VehicleModel> vehicleModelService;
        private readonly IMapper mapper;

        public VehicleModelController(IVehicleService<VehicleModel> vehicleModelService, IMapper mapper)
        {
            this.vehicleModelService = vehicleModelService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleModelDto>>> GetVehicleModels()
        {
            var vehicleModels = await vehicleModelService.GetAllAsync();
            var vehicleModelDtos = mapper.Map<IEnumerable<VehicleModelDto>>(vehicleModels);
            return Ok(vehicleModelDtos); // 200
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleModelDto>> GetVehicleModel(int id)
        {
            var vehicleModel = await vehicleModelService.GetByIdAsync(id);

            if (vehicleModel == null)
            {
                return NotFound(); // 404
            }

            var vehicleModelDto = mapper.Map<VehicleModelDto>(vehicleModel);
            return Ok(vehicleModelDto); // 200
        }

        [HttpPost]
        public async Task<ActionResult<VehicleModelDto>> PostVehicleModel(VehicleModelDto vehicleModelDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400
            }

            var vehicleModel = mapper.Map<VehicleModel>(vehicleModelDto);
            var addedVehicleModel = await vehicleModelService.AddAsync(vehicleModel);
            var committed = await vehicleModelService.CommitAsync();

            if (addedVehicleModel != vehicleModel || !committed)
            {
                return BadRequest(ModelState); //400
            }
            vehicleModelDto.Id = vehicleModel.Id;
            return CreatedAtAction(nameof(GetVehicleModel), new { id = vehicleModelDto.Id }, vehicleModelDto); // 201
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicleModel(int id, VehicleModelDto vehicleModelDto)
        {
            if (id != vehicleModelDto.Id)
            {
                return BadRequest(); // 400
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400
            }

            var vehicleModel = mapper.Map<VehicleModel>(vehicleModelDto);
            var updated = await vehicleModelService.UpdateAsync(vehicleModel);
            var committed = await vehicleModelService.CommitAsync();

            if (!updated || !committed)
            {
                return NotFound(); // 404
            }

            return NoContent(); // 204
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleModel(int id)
        {
            var deleted = await vehicleModelService.DeleteAsync(id);
            var commited = await vehicleModelService.CommitAsync();

            if (!deleted || !commited)
            {
                return NotFound(); // 404
            }

            return NoContent(); // 204
        }
    }
}
