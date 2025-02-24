using PandaAPI.Dtos;
using PandaAPI.Mappers;
using PandaAPI.Models;

namespace PandaAPI.Test.Unit.Mappers
{
    public class PatientMapperTests
    {
        [Fact]
        public void DtoToPatient_Should_Map_Fields_Correctly()
        {
            // Arrange
            var patientDto = new PatientDto
            {
                NhsNumber = "1373645350",
                Name = "Dr Glenn Clark",
                DateOfBirth = "1996-02-01",
                Postcode = "N6 2FA"
            };

            // Act
            var patient = PatientMapper.DtoToPatient(patientDto);

            // Assert
            Assert.Equal(patientDto.NhsNumber, patient.NhsNumber);
            Assert.Equal(patientDto.Name, patient.Name);
            Assert.Equal(new DateOnly(1996, 2, 1), patient.DateOfBirth);
            Assert.Equal(patientDto.Postcode, patient.Postcode);
        }

        [Fact]
        public void PatientToDto_Should_Map_Fields_Correctly()
        {
            // Arrange
            var patient = new Patient
            {
                NhsNumber = "1373645350",
                Name = "Dr Glenn Clark",
                DateOfBirth = new DateOnly(1997, 11, 3),
                Postcode = "N6 2FA"
            };

            // Act
            var patientDto = PatientMapper.PatientToDto(patient);

            // Assert
            Assert.Equal(patient.NhsNumber, patientDto.NhsNumber);
            Assert.Equal(patient.Name, patientDto.Name);
            Assert.Equal("1997-11-03", patientDto.DateOfBirth);
            Assert.Equal(patient.Postcode, patientDto.Postcode);
        }

        [Fact]
        public void UpdatePatientFromDto_Should_Update_Fields_Correctly()
        {
            // Arrange
            var patientDto = new PatientDto
            {
                NhsNumber = "1373645350",
                Name = "Dr Glenn Clark",
                DateOfBirth = "1996-02-01",
                Postcode = "N6 2FA"
            };

            var patient = new Patient
            {
                NhsNumber = "1234567890",
                Name = "John Doe",
                DateOfBirth = new DateOnly(1990, 1, 1),
                Postcode = "E1 6AN"
            };

            // Act
            PatientMapper.UpdatePatientFromDto(patientDto, patient);

            // Assert
            Assert.Equal(patientDto.NhsNumber, patient.NhsNumber);
            Assert.Equal(patientDto.Name, patient.Name);
            Assert.Equal(new DateOnly(1996, 2, 1), patient.DateOfBirth);
            Assert.Equal(patientDto.Postcode, patient.Postcode);
        }
    }
}
