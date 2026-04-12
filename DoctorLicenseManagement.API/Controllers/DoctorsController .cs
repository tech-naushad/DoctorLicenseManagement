using DoctorLicenseManagement.Application.Commands.CreateDoctorCommand;
using DoctorLicenseManagement.Application.Commands.DeleteDoctorCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DoctorLicenseManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly ILogger<DoctorsController> _logger;
        private readonly IMediator _mediator;
        public DoctorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 🔹 1. Create Doctor
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDoctorCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);            
        }



        //// 🔹 4. Update Doctor
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, [FromBody] DoctorDto dto)
        //{
        //    if (id != dto.Id)
        //        return BadRequest("Id mismatch");

        //    var result = await _service.UpdateAsync(dto);

        //    if (!result)
        //        return NotFound($"Doctor with Id {id} not found");

        //    return NoContent();
        //}

        //// 🔹 5. Update Doctor Status
        //[HttpPatch("{id}/status")]
        //public async Task<IActionResult> UpdateStatus(int id, [FromBody] int status)
        //{
        //    var result = await _service.UpdateStatusAsync(id, status);

        //    if (!result)
        //        return NotFound($"Doctor with Id {id} not found");

        //    return NoContent();
        //}


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteDoctorCommand { Id = id });

            if (!result.Success)
            {
                return BadRequest(result); // 400 with message
            }

            return Ok(result); // 200 with success response
        }
    }
}
