using Microsoft.AspNetCore.Mvc;
using PandaAPI.Dtos;
using PandaAPI.Models;
using PandaAPI.Repositories;

namespace PandaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController(IPatientRepository repository) : ControllerBase
    {
        /// <summary>
        /// Gets a patient by NHS number.
        /// </summary>
        /// <param name="nhsNumber">The NHS number of the patient to retrieve.</param>
        /// <returns>The patient with the specified NHS number.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Patient/1373645350
        ///
        /// </remarks>
        /// <response code="200">Returns the patient with the specified NHS number.</response>
        /// <response code="404">If the patient is not found.</response>
        /// <response code="500">If there is a server error.</response>

        [HttpGet("{nhsNumber}")]
        public async Task<ActionResult<PatientDto>> GetPatientById(string nhsNumber)
        {
            var patient = await repository.GetByNhsNumber(nhsNumber);
            return (patient == null) ? NotFound() : patient;
        }
        /// <summary>
        /// Creates a new patient.
        /// </summary>
        /// <param name="patientDto">the details of the patient to create.</param>
        /// <returns>The newly created patient.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Patient
        ///     {
        ///        "nhsNumber": "1373645350",
        ///        "name": "Dr Glenn Clark",
        ///        "dateOfBirth": "1996-02-01",
        ///        "postcode": "N6 2FA"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created patient.</response>
        /// <response code="400">If the patient data is invalid.</response>
        /// <response code="500">If there is a server error.</response>
        [HttpPost]
        public async Task<ActionResult<Patient>> CreatePatient(PatientDto patientDto)
        {
            await repository.Save(patientDto);
            return CreatedAtAction(nameof(GetPatientById), new { nhsNumber = patientDto.NhsNumber }, patientDto);
        }
        /// <summary>
        /// Updates an existing patient.
        /// </summary>
        /// <param name="nhsNumber">The NHS number of the patient to update.</param>
        /// <param name="patientDto">The details to be updated for the patient.</param>
        /// <returns>No content.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/Patient/1373645350
        ///     {
        ///        "nhsNumber": "1373645350",
        ///        "name": "Dr Glenn Clark",
        ///        "dateOfBirth": "1996-02-01",
        ///        "postcode": "N6 2FA"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">If the patient was successfully updated.</response>
        /// <response code="400">If the patient data is invalid or the NHS number does not match.</response>
        /// <response code="404">If the patient is not found.</response>
        /// <response code="500">If there is a server error.</response>
        [HttpPut("{nhsNumber}")]
        public async Task<IActionResult> UpdatePatient(string nhsNumber, PatientDto patientDto)
        {
            var patient = await repository.GetByNhsNumber(nhsNumber);

            if (patient == null)
            {
                return NotFound();
            }
            if (patient.NhsNumber != patientDto.NhsNumber)
            {
                return BadRequest("NHS Number in body does not match NHS Number in URL");
            }

            await repository.Update(patientDto);

            return NoContent();
        }
        /// <summary>
        /// Deletes a patient by NHS number.
        /// </summary>
        /// <param name="nhsNumber">The NHS number of the patient to delete.</param>
        /// <returns>No content.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/Patient/1373645350
        ///
        /// </remarks>
        /// <response code="204">If the patient was successfully deleted.</response>
        /// <response code="404">If the patient is not found.</response>
        /// <response code="500">If there is a server error.</response>
        [HttpDelete("{nhsNumber}")]
        public async Task<IActionResult> DeletePatient(string nhsNumber)
        {
            var patient = await repository.GetByNhsNumber(nhsNumber);
            if (patient != null)
            {
                await repository.Delete(patient.NhsNumber);
            }
            return NoContent();
        }
    }
}