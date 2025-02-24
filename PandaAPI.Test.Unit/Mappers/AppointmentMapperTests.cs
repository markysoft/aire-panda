using PandaAPI.Dtos;
using PandaAPI.Exceptions;
using PandaAPI.Mappers;
using PandaAPI.Models;
using PandaAPI.Utils;

namespace PandaAPI.Test.Unit.Mappers
{
    public class AppointmentMapperTests
    {
        [Fact]
        public void DtoToAppointment_Should_Map_Fields_Correctly()
        {
            // Arrange
            var appointmentDto = new AppointmentDto
            {
                Id = Guid.NewGuid(),
                Patient = "1373645350",
                Status = "active",
                Time = new DateTime(2025, 2, 23, 10, 0, 0),
                Duration = "1h30m",
                Clinician = "Dr. John Doe",
                Department = "Cardiology",
                Postcode = "N6 2FA"
            };

            var patient = new Patient
            {
                NhsNumber = "1373645350",
                Name = "Dr Glenn Clark",
                DateOfBirth = new DateOnly(1996, 2, 1),
                Postcode = "N6 2FA"
            };

            // Act
            var appointment = AppointmentMapper.DtoToAppointment(appointmentDto, patient);

            // Assert
            Assert.Equal(appointmentDto.Id, appointment.AppointmentId);
            Assert.Equal(appointmentDto.Status, appointment.Status);
            Assert.Equal(appointmentDto.Time, appointment.Time);
            Assert.Equal(appointmentDto.Time + DateUtils.GetDuration(appointmentDto.Duration), appointment.EndTime);
            Assert.Equal(appointmentDto.Clinician, appointment.Clinician);
            Assert.Equal(appointmentDto.Department, appointment.Department);
            Assert.Equal(appointmentDto.Postcode, appointment.Postcode);
            Assert.Equal(patient, appointment.Patient);
        }

        [Fact]
        public void AppointmentToDto_Should_Map_Fields_Correctly()
        {
            // Arrange
            var patient = new Patient
            {
                NhsNumber = "1373645350",
                Name = "Dr Glenn Clark",
                DateOfBirth = new DateOnly(1996, 2, 1),
                Postcode = "N6 2FA"
            };

            var appointment = new Appointment
            {
                AppointmentId = Guid.NewGuid(),
                Status = "attended",
                Time = new DateTime(2025, 2, 23, 10, 0, 0),
                EndTime = new DateTime(2025, 2, 23, 11, 30, 0),
                Clinician = "Dr. John Doe",
                Department = "Cardiology",
                Postcode = "N6 2FA",
                Patient = patient
            };

            // Act
            var appointmentDto = AppointmentMapper.AppointmentToDto(appointment);

            // Assert
            Assert.Equal(appointment.AppointmentId, appointmentDto.Id);
            Assert.Equal(appointment.Patient.NhsNumber, appointmentDto.Patient);
            Assert.Equal(appointment.Status, appointmentDto.Status);
            Assert.Equal(appointment.Time, appointmentDto.Time);
            Assert.Equal("1h30m", appointmentDto.Duration);
            Assert.Equal(appointment.Clinician, appointmentDto.Clinician);
            Assert.Equal(appointment.Department, appointmentDto.Department);
            Assert.Equal(appointment.Postcode, appointmentDto.Postcode);
        }



        [Fact]
        public void AppointmentToDto_Should_Set_Status_To_Missed_For_Active_Appointments_In_The_Past()
        {
            // Arrange
            var patient = new Patient
            {
                NhsNumber = "1373645350",
                Name = "Dr Glenn Clark",
                DateOfBirth = new DateOnly(1996, 2, 1),
                Postcode = "N6 2FA"
            };

            var appointment = new Appointment
            {
                AppointmentId = Guid.NewGuid(),
                Status = "active",
                Time = new DateTime(2025, 2, 23, 10, 0, 0),
                EndTime = new DateTime(2025, 2, 23, 11, 30, 0),
                Clinician = "Dr. John Doe",
                Department = "Cardiology",
                Postcode = "N6 2FA",
                Patient = patient
            };

            // Act
            var appointmentDto = AppointmentMapper.AppointmentToDto(appointment);

            // Assert
            Assert.Equal(appointment.AppointmentId, appointmentDto.Id);
            Assert.Equal(appointment.Patient.NhsNumber, appointmentDto.Patient);
            Assert.Equal("missed", appointmentDto.Status);
            Assert.Equal(appointment.Time, appointmentDto.Time);
            Assert.Equal("1h30m", appointmentDto.Duration);
            Assert.Equal(appointment.Clinician, appointmentDto.Clinician);
            Assert.Equal(appointment.Department, appointmentDto.Department);
            Assert.Equal(appointment.Postcode, appointmentDto.Postcode);
        }

        [Fact]
        public void UpdateAppointmentFromDto_Should_Update_Fields_Correctly()
        {
            // Arrange
            var appointmentDto = new AppointmentDto
            {
                Id = Guid.NewGuid(),
                Patient = "1373645350",
                Status = "attended",
                Time = new DateTime(2025, 2, 23, 10, 0, 0),
                Duration = "1h30m",
                Clinician = "Dr. John Doe",
                Department = "Cardiology",
                Postcode = "N6 2FA"
            };

            var appointment = new Appointment
            {
                AppointmentId = appointmentDto.Id,
                Status = "active",
                Time = new DateTime(2025, 2, 23, 9, 0, 0),
                EndTime = new DateTime(2025, 2, 23, 10, 0, 0),
                Clinician = "Dr. Jane Doe",
                Department = "Neurology",
                Postcode = "E1 6AN",
                Patient = new Patient { NhsNumber = "1373645350" }
            };

            // Act
            AppointmentMapper.UpdateAppointmentFromDto(appointmentDto, appointment);

            // Assert
            Assert.Equal(appointmentDto.Status, appointment.Status);
            Assert.Equal(appointmentDto.Time, appointment.Time);
            Assert.Equal(appointmentDto.Time + DateUtils.GetDuration(appointmentDto.Duration), appointment.EndTime);
            Assert.Equal(appointmentDto.Clinician, appointment.Clinician);
            Assert.Equal(appointmentDto.Department, appointment.Department);
            Assert.Equal(appointmentDto.Postcode, appointment.Postcode);
        }

        [Fact]
        public void UpdateAppointmentFromDto_Should_Allow_Cancellation()
        {
            // Arrange
            var appointmentDto = new AppointmentDto
            {
                Id = Guid.NewGuid(),
                Patient = "1373645350",
                Status = "cancelled",
                Time = new DateTime(2025, 2, 23, 10, 0, 0),
                Duration = "1h30m",
                Clinician = "Dr. John Doe",
                Department = "Cardiology",
                Postcode = "N6 2FA"
            };

            var appointment = new Appointment
            {
                AppointmentId = appointmentDto.Id,
                Status = "active",
                Time = new DateTime(2025, 2, 23, 9, 0, 0),
                EndTime = new DateTime(2025, 2, 23, 10, 0, 0),
                Clinician = "Dr. Jane Doe",
                Department = "Neurology",
                Postcode = "E1 6AN",
                Patient = new Patient { NhsNumber = "1373645350" }
            };

            // Act
            AppointmentMapper.UpdateAppointmentFromDto(appointmentDto, appointment);

            // Assert
            Assert.Equal(appointmentDto.Status, appointment.Status);
            Assert.Equal(appointmentDto.Time, appointment.Time);
            Assert.Equal(appointmentDto.Time + DateUtils.GetDuration(appointmentDto.Duration), appointment.EndTime);
            Assert.Equal(appointmentDto.Clinician, appointment.Clinician);
            Assert.Equal(appointmentDto.Department, appointment.Department);
            Assert.Equal(appointmentDto.Postcode, appointment.Postcode);
        }

        [Fact]
        public void UpdateAppointmentFromDto_Should_Not_Allow_Cancelled_Appointments_To_Be_Reinstated()
        {
            // Arrange
            var appointmentDto = new AppointmentDto
            {
                Id = Guid.NewGuid(),
                Patient = "1373645350",
                Status = "active",
                Time = new DateTime(2025, 2, 23, 10, 0, 0),
                Duration = "1h30m",
                Clinician = "Dr. John Doe",
                Department = "Cardiology",
                Postcode = "N6 2FA"
            };

            var appointment = new Appointment
            {
                AppointmentId = appointmentDto.Id,
                Status = "cancelled",
                Time = new DateTime(2025, 2, 23, 9, 0, 0),
                EndTime = new DateTime(2025, 2, 23, 10, 0, 0),
                Clinician = "Dr. Jane Doe",
                Department = "Neurology",
                Postcode = "E1 6AN",
                Patient = new Patient { NhsNumber = "1373645350" }
            };

            // Act & Assert
            var exception = Assert.Throws<AppointmentUpdateException>(
                () => AppointmentMapper.UpdateAppointmentFromDto(appointmentDto, appointment));
            Assert.Equal("Cannot reinstate a cancelled appointment", exception.Message);

        }
    }
}
