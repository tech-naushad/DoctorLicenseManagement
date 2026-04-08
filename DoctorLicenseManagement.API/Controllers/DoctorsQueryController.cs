using DoctorLicenseManagement.Application.Commands.CreateDoctorCommand;
using DoctorLicenseManagement.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DoctorLicenseManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsQueryController : ControllerBase
    {
        private readonly ILogger<DoctorsController> _logger;
        private readonly IMediator _mediator;
        public DoctorsQueryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _mediator.Send(new GetAllDoctorsQuery());
            return Ok(data);
        }

        //// 🔹 3. Get Doctor By Id
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var data = await _service.GetByIdAsync(id);

        //    if (data == null)
        //        return NotFound($"Doctor with Id {id} not found");

        //    return Ok(data);
        //}

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

        // 🔹 6. Soft Delete Doctor
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var result = await _service.DeleteAsync(id);

        //    if (!result)
        //        return NotFound($"Doctor with Id {id} not found");

        //    return NoContent();
        //}
    }
}
