using PandaAPI.Dtos;
using PandaAPI.Exceptions;
using PandaAPI.Models;
using PandaAPI.Utils;

namespace PandaAPI.Mappers
{
    public static class AppointmentMapper
    {
        public static Appointment DtoToAppointment(AppointmentDto dto, Patient patient)
        {
            return new Appointment
            {
                AppointmentId = dto.Id,
                Status = dto.Status,
                Time = dto.Time,
                EndTime = dto.Time + DateUtils.GetDuration(dto.Duration),
                Clinician = dto.Clinician,
                Department = dto.Department,
                Postcode = dto.Postcode,
                Patient = patient
            };
        }    
        
        public static void UpdateAppointmentFromDto(AppointmentDto dto, Appointment appointment)
        {
            if (appointment.Status == Constants.Cancelled && dto.Status != Constants.Cancelled)
            {
                throw new AppointmentUpdateException("Cannot reinstate a cancelled appointment");
            }
            else
            {
                appointment.Status = dto.Status;
            }
            appointment.Time = dto.Time;
            appointment.EndTime = dto.Time + DateUtils.GetDuration(dto.Duration);
            appointment.Clinician = dto.Clinician;
            appointment.Department = dto.Department;
            appointment.Postcode = dto.Postcode;
        }

        public static AppointmentDto AppointmentToDto(Appointment appointment)
        {
           return new AppointmentDto
           {
               Id = appointment.AppointmentId,
               Patient = appointment.Patient.NhsNumber,
               Status = appointment.Status == Constants.Active && appointment.EndTime < DateTime.Now ? Constants.Missed : appointment.Status,
               Time = appointment.Time,
               Duration = DateUtils.FormatDuration(appointment.EndTime - appointment.Time),
               Clinician = appointment.Clinician,
               Department = appointment.Department,
               Postcode = appointment.Postcode
           };
        }
    }
}
