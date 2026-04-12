using DoctorLicenseManagement.Application.Commands.CreateDoctorCommand;
using DoctorLicenseManagement.Application.Commands.DeleteDoctorCommand;
using DoctorLicenseManagement.Application.Commands.UpdateDoctorCommand;
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
               
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDoctorCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);            
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch");

            var result = await _mediator.Send(command);
             

           // if (!result.Success)
                //return BadRequest(result);

            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteDoctorCommand { Id = id });

            if (!result.Success)
            {
                return BadRequest(result); 
            }

            return Ok(result); 
        }
    }
}
