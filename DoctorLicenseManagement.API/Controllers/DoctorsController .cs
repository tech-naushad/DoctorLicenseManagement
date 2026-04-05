using DoctorLicenseManagement.Application.DTOs;
using DoctorLicenseManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DoctorLicenseManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _service;

        public DoctorsController(IDoctorService service)
        {
            _service = service;
        }

        // 🔹 1. Create Doctor
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DoctorDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        // 🔹 2. Get All Doctors
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        // 🔹 3. Get Doctor By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
                return NotFound($"Doctor with Id {id} not found");

            return Ok(data);
        }

        // 🔹 4. Update Doctor
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DoctorDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id mismatch");

            var result = await _service.UpdateAsync(dto);

            if (!result)
                return NotFound($"Doctor with Id {id} not found");

            return NoContent();
        }

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
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
                return NotFound($"Doctor with Id {id} not found");

            return NoContent();
        }
    }
}
