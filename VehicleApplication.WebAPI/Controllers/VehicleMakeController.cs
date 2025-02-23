using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using VehicleApplication.Service.Common;
using VehicleApplication.Model;
using VehicleApplication.WebAPI.Models;

namespace VehicleApplication.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleMakeController: ControllerBase
    {
        private readonly IVehicleService<VehicleMake> vehicleMakeService;
        private readonly IMapper mapper;

        public VehicleMakeController(IVehicleService<VehicleMake> vehicleMakeService, IMapper mapper)
        {
            this.vehicleMakeService = vehicleMakeService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleMakeDto>>> GetVehicleMakes()
        {
            var vehicleMakes = await vehicleMakeService.GetAllAsync();
            var vehicleMakeDtos = mapper.Map<IEnumerable<VehicleMakeDto>>(vehicleMakes);
            return Ok(vehicleMakeDtos); // 200
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleMakeDto>> GetVehicleMake(int id)
        {
            var vehicleMake = await vehicleMakeService.GetByIdAsync(id);

            if (vehicleMake == null)
            {
                return NotFound(); // 404
            }

            var vehicleMakeDto = mapper.Map<VehicleMakeDto>(vehicleMake);
            return Ok(vehicleMakeDto); // 200
        }

        [HttpPost]
        public async Task<ActionResult<VehicleMakeDto>> PostVehicleMake(VehicleMakeDto vehicleMakeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400
            }

            var vehicleMake = mapper.Map<VehicleMake>(vehicleMakeDto);
            var addedVehicleMake = await vehicleMakeService.AddAsync(vehicleMake);
            var committed = await vehicleMakeService.CommitAsync();
            if (vehicleMake != addedVehicleMake || !committed)
            {
                return BadRequest(ModelState); //400
            }
            vehicleMakeDto.Id = vehicleMake.Id;
            return CreatedAtAction(nameof(GetVehicleMake), new { id = vehicleMakeDto.Id }, vehicleMakeDto); // 201
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicleMake(int id, VehicleMakeDto vehicleMakeDto)
        {
            if (id != vehicleMakeDto.Id)
            {
                return BadRequest(); // 400
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400
            }

            var vehicleMake = mapper.Map<VehicleMake>(vehicleMakeDto);
            var updated = await vehicleMakeService.UpdateAsync(vehicleMake);
            var committed = await vehicleMakeService.CommitAsync();

            if (!updated || !committed)
            {
                return NotFound(); // 404
            }

            return NoContent(); // 204
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleMake(int id)
        {
            var deleted = await vehicleMakeService.DeleteAsync(id);
            var committed = await vehicleMakeService.CommitAsync();

            if (!deleted || !committed)
            {
                return NotFound(); // 404
            }

            return NoContent(); // 204
        }
    }
}
