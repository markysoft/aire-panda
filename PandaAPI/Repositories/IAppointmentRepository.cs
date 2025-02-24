using PandaAPI.Dtos;

namespace PandaAPI.Repositories
{
    public interface IAppointmentRepository
    {
        Task<AppointmentDto?> Get(Guid id);
        // IQueryable<Appointment> GetAll();
        Task Save(AppointmentDto appointment);
        Task Update(AppointmentDto dto);
    }
}