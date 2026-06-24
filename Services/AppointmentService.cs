using ClinicFlow.Models;
using ClinicFlow.Repositories;
using ClinicFlow.Repositories.Interfaces;
using ClinicFlow.Services.Interfaces;

namespace ClinicFlow.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository appointmentRepository;

        public AppointmentService(IAppointmentRepository repository)
        {
            appointmentRepository = repository;
        }

        public void AddAppointment(Appointment appointment)
        {
            appointmentRepository.AddAppointment(appointment);
        }

        public IEnumerable<Appointment> GetAppointments()
        {
            return appointmentRepository.GetAllAppointments();
        }

        public Appointment GetAppointment(int id)
        {
            var appointment = appointmentRepository.GetAppointmentById(id);
            if (appointment == null)
            {
                throw new ArgumentException("Appointment not found with ID: " + id);
            }
            return appointment;
        }
    }
}
