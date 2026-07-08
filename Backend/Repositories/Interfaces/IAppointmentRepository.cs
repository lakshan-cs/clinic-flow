using System.Collections.Generic;
using ClinicFlow.Models;

namespace ClinicFlow.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        void AddAppointment(Appointment appointment);

        IEnumerable<Appointment> GetAllAppointments();

        Appointment GetAppointmentById(int id);

        IEnumerable<Appointment> GetAppointmentsByPatientId(int patientId);

        IEnumerable<Appointment> GetAppointmentsByProviderId(int providerId);

        IEnumerable<Appointment> GetAppointmentsByClinicId(int clinicId);
    }
}
