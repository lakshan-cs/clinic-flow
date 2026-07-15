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
            var startTime = appointment.DateTime;
            var endTime = startTime.AddMinutes(20);

            var openingTime = startTime.Date.AddHours(8);
            var closingTime = startTime.Date.AddHours(16);

            if (startTime < openingTime || endTime > closingTime)
            {
                throw new InvalidAppointmentException(
                    "Appointment must be between 08:00 and 16:00.");
            }

            if (appointment.DateTime < DateTime.Now)
            {
                throw new InvalidAppointmentException("Appointment cannot be scheduled in the past. Please select a future date and time.");
            }

            var appointments = appointmentRepository.GetAppointmentsByProviderId(appointment.ProviderId);
            if (appointments != null && appointments.Any(a => a.DateTime == appointment.DateTime))
            {
                throw new AppointmentConflictException("Provider already has an appointment at this time.");
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
            return appointmentRepository.GetAppointmentsByPatientId(patientId);
        }

        public IEnumerable<Appointment> GetAppointmentsByProviderId(int providerId)
        {
            return appointmentRepository.GetAppointmentsByProviderId(providerId);
        }

        public IEnumerable<Appointment> GetAppointmentsByClinicId(int clinicId)
        {
            return appointmentRepository.GetAppointmentsByClinicId(clinicId);
        }
    }
}
