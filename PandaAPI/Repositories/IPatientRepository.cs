using PandaAPI.Dtos;

namespace PandaAPI.Repositories
{
    public interface IPatientRepository
    {
        Task Delete(string nhsNumber);
        Task<PatientDto?> GetByNhsNumber(string nhsNumber);
        Task Save(PatientDto patient);
        Task Update(PatientDto patient);
    }
}