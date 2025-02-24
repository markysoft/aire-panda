using Microsoft.EntityFrameworkCore;
using PandaAPI.Data;
using PandaAPI.Dtos;
using PandaAPI.Exceptions;
using PandaAPI.Mappers;

namespace PandaAPI.Repositories
{
    public class AppointmentRepository(ApplicationDbContext context) : IAppointmentRepository
    {
        public async Task Save(AppointmentDto dto)
        {
            var patient = await context.Patients.FirstOrDefaultAsync(p => p.NhsNumber == dto.Patient);
            if (patient == null)
            {
                throw new AppointmentNotFoundException($"patient not found");
            }

            var appointment = AppointmentMapper.DtoToAppointment(dto, patient);
            context.Appointments.Add(appointment);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: Appointments.AppointmentId") == true)
                {
                    throw new AppointmentExistsException($"Appointment with ID '{dto.Id}' already exists");
                }
                throw;
            }
        }

        public async Task<AppointmentDto?> Get(Guid id)
        {
            var appointment = await context.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            return appointment == null ? null : AppointmentMapper.AppointmentToDto(appointment);
        }

        public async Task Update(AppointmentDto dto)
        {
            var existingAppointment = await context.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.AppointmentId == dto.Id);

            if (existingAppointment == null)
            {
                throw new AppointmentNotFoundException($"Appointment {dto.Id}not found");
            }

            if (dto.Patient != existingAppointment.Patient.NhsNumber)
            {
                var patient = await context.Patients.FirstOrDefaultAsync(p => p.NhsNumber == dto.Patient);
                if (patient == null)
                {
                    throw new PatientNotFoundException($"patient not found");
                }
                existingAppointment.Patient = patient;
            }

            AppointmentMapper.UpdateAppointmentFromDto(dto, existingAppointment);

            await context.SaveChangesAsync();
        }
    }
}
