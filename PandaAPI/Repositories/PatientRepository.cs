using Microsoft.EntityFrameworkCore;
using PandaAPI.Data;
using PandaAPI.Dtos;
using PandaAPI.Exceptions;
using PandaAPI.Mappers;

namespace PandaAPI.Repositories
{
    public class PatientRepository(ApplicationDbContext context) : IPatientRepository
    {

        public async Task Save(PatientDto patientDto)
        {
            var patient = PatientMapper.DtoToPatient(patientDto);
            context.Patients.Add(patient);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: Patients.NhsNumber") == true)
                {
                    throw new PatientExistsException($"Patient with NHS number '{patientDto.NhsNumber}' already exists");
                }
                throw;
            }
        }

        public async Task<PatientDto?> GetByNhsNumber(string nhsNumber)
        {
            return await context.Patients
                .Where(p => p.NhsNumber == nhsNumber)
                .Select(p => PatientMapper.PatientToDto(p))
                .FirstOrDefaultAsync();
        }

        public async Task Update(PatientDto patientDto)
        {
            var existingPatient = await context.Patients.FirstOrDefaultAsync(p => p.NhsNumber == patientDto.NhsNumber);
            if (existingPatient == null)
            {
                throw new PatientNotFoundException("Patient not found");
            }

            // Be explicit about updated fields to avoid over posting
            PatientMapper.UpdatePatientFromDto(patientDto, existingPatient);
            await context.SaveChangesAsync();
        }

        public async Task Delete(string nhsNumber)
        {
            var patient = await context.Patients.FirstOrDefaultAsync(p => p.NhsNumber == nhsNumber);
            // silently fail if the patient does not exist
            if (patient != null)
            {
                context.Patients.Remove(patient);
                await context.SaveChangesAsync();
            }
        }
    }
}
