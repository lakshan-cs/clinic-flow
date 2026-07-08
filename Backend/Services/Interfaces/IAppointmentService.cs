using ClinicFlow.Models;

namespace ClinicFlow.Services.Interfaces;

public interface IAppointmentService
{
    void AddAppointment(Appointment appointment);

    IEnumerable<Appointment> GetAppointments();

    Appointment GetAppointment(int id);

    IEnumerable<Appointment> GetAppointmentsByPatientId(int patientId);

    IEnumerable<Appointment> GetAppointmentsByProviderId(int providerId);

    IEnumerable<Appointment> GetAppointmentsByClinicId(int clinicId);
}
