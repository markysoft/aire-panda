using PandaAPI.Dtos;
using PandaAPI.Repositories;

namespace PandaAPI.Test.Integration.Repositories
{
    public class PatientRepositoryTests : IDisposable
    {
        private const string UnknownPatientId = "9999999999";
        private readonly PatientRepository repository;
        private readonly DataSeeder seeder;

        public PatientRepositoryTests()
        {
            seeder = new DataSeeder("patient-test");
            repository = new PatientRepository(seeder.Context);
        }

        [Fact]
        public async Task Get_Patients_Should_Return_Patient_For_Valid_NhsNumber()
        {
            // Arrange
            string nhsNumber = "0987654321";
            string name = "Paul Pott";
            string postcode = "YO17 9RD";
            var newPatient = new PatientDto
            {
                NhsNumber = nhsNumber,
                Name = name,
                DateOfBirth = "1989-03-03",
                Postcode = postcode
            };
            await repository.Save(newPatient);

            // Act
            var patient = await repository.GetByNhsNumber(nhsNumber);

            // Assert
            Assert.NotNull(patient);
            Assert.Equal(name, patient.Name);
            Assert.Equal(nhsNumber, patient.NhsNumber);
            Assert.Equal("1989-03-03", patient.DateOfBirth);
            Assert.Equal(postcode, patient.Postcode);
        }

        [Fact]
        public async Task Get_Patients_Should_Return_Null_For_Unknown_NhsNumber()
        {
            // Arrange
            var nhsNumber = "ere";

            // Act
            var patient = await repository.GetByNhsNumber(nhsNumber);

            // Assert
            Assert.Null(patient);
        }

        [Fact]
        public async Task Save_Patients_Should_Add_New_Patients()
        {
            // Arrange
            string nhsNumber = "1234567890";
            string name = "John Wick";
            string postcode = "YO24 1AB";
            string dateOfBirth = "1988-01-01";
            var newPatient = new PatientDto
            {
                NhsNumber = nhsNumber,
                Name = name,
                DateOfBirth = dateOfBirth,
                Postcode = postcode
            };

            // Act
            await repository.Save(newPatient);

            // Assert
            // No assertions needed as we are just testing the save functionality
        }

        [Fact]
        public async Task Update_Patients_Should_Update_ExistingPatient()
        {
            // Arrange
            string nhsNumber = "3234567890";
            string name = "Mr Manhatten";
            string postcode = "Y017 6PL";
            string dateOfBirth = "1980-01-01";
            var patient = new PatientDto
            {
                NhsNumber = nhsNumber,
                Name = name,
                DateOfBirth = dateOfBirth,
                Postcode = postcode
            };
            await repository.Save(patient);
            string updatedName = "Dr Manhatten";
            string updatedPostcode = "Y019 5RH";
            string updatedDateOfBirth = "1980-01-01";
            patient.DateOfBirth = updatedDateOfBirth;
            patient.Name = updatedName;
            patient.Postcode = updatedPostcode;

            // Act
            await repository.Update(patient);
            var updatedPatient = await repository.GetByNhsNumber(nhsNumber);

            // Assert
            Assert.NotNull(updatedPatient);
            Assert.Equal(updatedPostcode, updatedPatient.Postcode);
            Assert.Equal(updatedDateOfBirth, updatedPatient.DateOfBirth);
            Assert.Equal(updatedName, updatedPatient.Name);
        }

        [Fact]
        public async Task Get_Patients_Should_Return_Null_For_Unknown_Patient()
        {
            // Arrange
            var nhsNumber = UnknownPatientId;

            // Act
            var patient = await repository.GetByNhsNumber(nhsNumber);

            // Assert
            Assert.Null(patient);
        }

        [Fact]
        public async Task Delete_Patients_Should_Silently_Fail_For_Unknown_ID()
        {
            // Arrange
            var nhsNumber = UnknownPatientId;

            // Act
            await repository.Delete(nhsNumber);

            // Assert
            // No assertions needed as we are just testing no exception is thrown
        }

        [Fact]
        public async Task Delete_Patients_Should_Delete_Existing_Patient()
        {
            // Arrange
            string nhsNumber = "7987654321";
            string name = "Tony Stark";
            string postcode = "YO17 9RD";
            var newPatient = new PatientDto
            {
                NhsNumber = nhsNumber,
                Name = name,
                DateOfBirth = "1995-04-05",
                Postcode = postcode
            };
            await repository.Save(newPatient);

            // Act
            await repository.Delete(nhsNumber);
            var patient = await repository.GetByNhsNumber(nhsNumber);

            // Assert
            Assert.Null(patient);
        }

        public void Dispose()
        {
            // Drop the database
            seeder.Context.Database.EnsureDeleted();
            seeder.Context.Dispose();
        }
    }
}
