using ClinicFlow.Exceptions;
using ClinicFlow.Models;
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
            var appointments = appointmentRepository.GetAppointmentsByProviderId(appointment.ProviderId);
            if (appointments != null && appointments.Any(a => a.DateTime == appointment.DateTime))
            {
                throw new DuplicateResourceException("Provider already has an appointment at this time.");
            }
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
                throw new NotFoundException("Appointment not found with ID: " + id);
            }
            return appointment;
        }

        public IEnumerable<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            var appointments = appointmentRepository.GetAppointmentsByPatientId(patientId);
            if (appointments == null || !appointments.Any())
            {
                throw new NotFoundException("No appointments found for patient with ID: " + patientId);
            }
            return appointments;
        }

        public IEnumerable<Appointment> GetAppointmentsByProviderId(int providerId)
        {
            var appointments = appointmentRepository.GetAppointmentsByProviderId(providerId);
            if (appointments == null || !appointments.Any())
            {
                throw new NotFoundException("No appointments found for provider with ID: " + providerId);
            }
            return appointments;
        }

        public IEnumerable<Appointment> GetAppointmentsByClinicId(int clinicId)
        {
            var appointments = appointmentRepository.GetAppointmentsByClinicId(clinicId);
            if (appointments == null || !appointments.Any())
            {
                throw new NotFoundException("No appointments found for clinic with ID: " + clinicId);
            }
            return appointments;
        }
    }
}
