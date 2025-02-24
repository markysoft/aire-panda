using PandaAPI.Dtos;
using PandaAPI.Repositories;

namespace PandaAPI.Test.Integration.Repositories
{
    public class AppointmentRepositoryTests : IDisposable
    {
        private readonly AppointmentRepository repository;
        private DataSeeder seeder;

        public AppointmentRepositoryTests()
        {
            // Seed data
            seeder = new DataSeeder("appointment-test");
            seeder.SeedPatients();
            repository = new AppointmentRepository(seeder.Context);
        }

        [Fact]
        public async Task Save_Appointment_Should_Persist_Data()
        {
            // Arrange
            var appointment = new AppointmentDto
            {
                Id = Guid.NewGuid(),
                Patient = seeder.Patients[0].NhsNumber,
                Status = "active",
                Time = DateTime.Parse("2021-03-26T09:00:00+00:00"),
               Duration = "30m",
                Clinician = "Joseph Savage",
                Department = "gastroentology",
                Postcode = "E91 9AE"
            };
            // Act
            await repository.Save(appointment);
            var result = await repository.Get(appointment.Id);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(appointment.Id, result.Id);
            Assert.Equal("1585672939", result.Patient);
            Assert.Equal(DateTime.Parse("2021-03-26T09:00:00+00:00"), result.Time);
            Assert.Equal("30m", result.Duration);
            Assert.Equal("Joseph Savage", result.Clinician);
            Assert.Equal("gastroentology", result.Department);
            Assert.Equal("E91 9AE", result.Postcode);
        }

        [Fact]
        public async Task Update_Appointment_Should_Modify_Existing_Appointment()
        {
            // Arrange
            var appointment = new AppointmentDto
            {
                Id = Guid.NewGuid(),
                Patient = seeder.Patients[0].NhsNumber,
                Status = "active",
                Time = DateTime.Parse("2021-03-26T09:00:00+00:00"),
                Duration = "30m",
                Clinician = "Joseph Savage",
                Department = "gastroentology",
                Postcode = "E91 9AE"
            };
            await repository.Save(appointment);

            var updatedAppointment = new AppointmentDto
            {
                Id = appointment.Id,
                Patient = appointment.Patient,
                Status = "attended",
                Time = DateTime.Parse("2021-03-26T10:00:00+00:00"),
                Duration = "45m",
                Clinician = "Jane Doe",
                Department = "cardiology",
                Postcode = "E92 9AE"
            };

            // Act
            await repository.Update(updatedAppointment);
            var result = await repository.Get(updatedAppointment.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedAppointment.Id, result.Id);
            Assert.Equal(updatedAppointment.Patient, result.Patient);
            Assert.Equal(updatedAppointment.Status, result.Status);
            Assert.Equal(updatedAppointment.Time, result.Time);
            Assert.Equal(updatedAppointment.Duration, result.Duration);
            Assert.Equal(updatedAppointment.Clinician, result.Clinician);
            Assert.Equal(updatedAppointment.Department, result.Department);
            Assert.Equal(updatedAppointment.Postcode, result.Postcode);
        }

        public void Dispose()
        {
            // Drop the database
            seeder.Context.Database.EnsureDeleted();
            seeder.Context.Dispose();
        }
    }
}