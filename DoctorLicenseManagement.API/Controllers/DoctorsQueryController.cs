using DoctorLicenseManagement.Application.Commands.CreateDoctorCommand;
using DoctorLicenseManagement.Application.Queries;
using DoctorLicenseManagement.Application.Queries.GetAllDoctors;
using DoctorLicenseManagement.Application.Queries.GetDoctorsById;
using DoctorLicenseManagement.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] LicenseStatus? licenseStatus,
             [FromQuery] int page = 1,[FromQuery] int pageSize = 10)
        {
            var response = await _mediator.Send(new GetAllDoctorsQuery 
            { 
                Search = search,
                LicenseStatus = licenseStatus,
                Page = page,
                PageSize = pageSize,
            });
            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _mediator.Send(new GetDoctorsByIdQuery
             {
                 Id = id
             });
             
            return Ok(response);
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
