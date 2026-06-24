using ClinicFlow.Models;

namespace ClinicFlow.Services.Interfaces;

public interface IAppointmentService
{
    void AddAppointment(Appointment appointment);

    IEnumerable<Appointment> GetAppointments();

    Appointment GetAppointment(int id);
}
