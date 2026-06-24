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
    }
}
