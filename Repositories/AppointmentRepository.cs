using ClinicFlow.Models;
using ClinicFlow.Repositories.Interfaces;
using ClinicFlow.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ClinicDbContext context;

        public AppointmentRepository(ClinicDbContext context)
        {
            this.context = context;
        }

        public void AddAppointment(Appointment appointment)
        {
            context.Appointments.Add(appointment);
            context.SaveChanges();
        }

        public IEnumerable<Appointment> GetAllAppointments()
        {
            return context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Clinic)
                .Include(a => a.Provider)
                .ToList();
        }

        public Appointment GetAppointmentById(int id)
        {
            return context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Clinic)
                .Include(a => a.Provider)
                .FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            return context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Clinic)
                .Include(a => a.Provider)
                .Where(a => a.PatientId == patientId)
                .ToList();
        }

        public IEnumerable<Appointment> GetAppointmentsByProviderId(int providerId)
        {
            return context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Clinic)
                .Include(a => a.Provider)
                .Where(a => a.ProviderId == providerId)
                .ToList();
        }

        public IEnumerable<Appointment> GetAppointmentsByClinicId(int clinicId)
        {
            return context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Clinic)
                .Include(a => a.Provider)
                .Where(a => a.ClinicId == clinicId)
                .ToList();
        }
    }
}
