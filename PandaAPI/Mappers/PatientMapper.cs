using PandaAPI.Dtos;
using PandaAPI.Models;

namespace PandaAPI.Mappers
{
    public static class PatientMapper
    {
        public static Patient DtoToPatient(PatientDto dto)
        {
            return new Patient
            {
                NhsNumber = dto.NhsNumber,
                Name = dto.Name,
                DateOfBirth = DateOnly.ParseExact(dto.DateOfBirth, "yyyy-MM-dd"),
                Postcode = dto.Postcode
            };
        }

        public static PatientDto PatientToDto(Patient patient)
        {
            return new PatientDto
            {
                NhsNumber = patient.NhsNumber,
                Name = patient.Name,
                DateOfBirth = patient.DateOfBirth.ToString("yyyy-MM-dd"),
                Postcode = patient.Postcode,
            };
        }

        public static void UpdatePatientFromDto(PatientDto dto, Patient patient)
        {
            patient.NhsNumber = dto.NhsNumber;
            patient.Name = dto.Name;
            patient.DateOfBirth = DateOnly.ParseExact(dto.DateOfBirth, "yyyy-MM-dd");
            patient.Postcode = dto.Postcode;
        }
    }
}
