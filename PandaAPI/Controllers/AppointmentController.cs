using Microsoft.AspNetCore.Mvc;
using PandaAPI.Dtos;
using PandaAPI.Models;
using PandaAPI.Repositories;

namespace PandaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController(IAppointmentRepository repository, IPatientRepository patientRepository) : Controller
    {
        /// <summary>
        /// Gets an appointment by ID.
        /// </summary>
        /// <param name="id">The ID of the appointment to retrieve.</param>
        /// <returns>The appointment with the specified ID.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Appointment/d290f1ee-6c54-4b01-90e6-d701748f085
        ///
        /// </remarks>        
        /// <response code="200">Returns the appointment with the specified ID.</response>
        /// <response code="404">If the appointment is not found.</response>
        /// <response code="500">If there is a server error.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetAppointment(Guid id)
        {
            var appointment = await repository.Get(id);
            return (appointment == null) ? NotFound() : appointment;
        }

        /// <summary>
        /// Creates a new appointment.
        /// </summary>
        /// <param name="appointmentDto">The details of the appointment to create.</param>
        /// <returns>The newly created appointment.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Appointment
        ///     {
        ///        "id": "d290f1ee-6c54-4b01-90e6-d701748f0851",
        ///        "patient": "1373645350",
        ///        "status": "active",
        ///        "time": "2025-02-23T10:00:00",
        ///        "duration": "1h30m",
        ///        "clinician": "Dr. John Doe",
        ///        "department": "Cardiology",
        ///        "postcode": "N6 2FA"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created appointment.</response>
        /// <response code="400">If the appointment data is invalid.</response>
        /// <response code="404">If the patient is not found.</response>
        /// <response code="500">If there is a server error.</response>
        [HttpPost]
        public async Task<ActionResult<Appointment>> CreateAppointment(AppointmentDto appointmentDto)
        {
            var patient = await patientRepository.GetByNhsNumber(appointmentDto.Patient);
            if (patient == null)
            {
                return NotFound("Patient not found");
            }

            await repository.Save(appointmentDto);
            return CreatedAtAction(nameof(GetAppointment), new { id = appointmentDto.Id }, appointmentDto);
        }

        /// <summary>
        /// Updates an existing appointment.
        /// </summary>
        /// <param name="id">The ID of the appointment to update.</param>
        /// <param name="appointmentDto">The details to be updated for the appointment.</param>
        /// <returns>No content.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/Appointment/d290f1ee-6c54-4b01-90e6-d701748f0851
        ///     {
        ///        "id": "d290f1ee-6c54-4b01-90e6-d701748f0851",
        ///        "patient": "1373645350",
        ///        "status": "active",
        ///        "time": "2025-02-23T10:00:00",
        ///        "duration": "1h30m",
        ///        "clinician": "Dr. John Doe",
        ///        "department": "Cardiology",
        ///        "postcode": "N6 2FA"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">If the appointment was successfully updated.</response>
        /// <response code="400">If the appointment data is invalid or the ID does not match.</response>
        /// <response code="404">If the appointment is not found.</response>
        /// <response code="500">If there is a server error.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(Guid id, AppointmentDto appointmentDto)
        {
            if (id != appointmentDto.Id)
            {
                return BadRequest("Appointment in bod does not match URL");
            }
            var appointment = await repository.Get(id);

            if (appointment == null)
            {
                return NotFound();
            }

            await repository.Update(appointmentDto);

            return NoContent();
        }
    }
}
