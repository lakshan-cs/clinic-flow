using Microsoft.AspNetCore.Mvc;
using ClinicFlow.Services.Interfaces;
using ClinicFlow.Dto;
using ClinicFlow.Models;

namespace ClinicFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        // Create a new appointment
        [HttpPost]
        public ActionResult<AppointmentResponse> CreateAppointment([FromBody] AppointmentRequest appointmentRequest)
        {
            var appointment = new Appointment
            {
                PatientId = appointmentRequest.PatientId,
                ProviderId = appointmentRequest.ProviderId,
                ClinicId = appointmentRequest.ClinicId,
                DateTime = appointmentRequest.DateTime,
                Reason = appointmentRequest.Reason
            };

            appointmentService.AddAppointment(appointment);

            var appointmentResponse = new AppointmentResponse
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                ClinicId = appointment.ClinicId,
                ProviderId = appointment.ProviderId,
                DateTime = appointment.DateTime,
                Reason = appointment.Reason
            };

            return CreatedAtAction(
                nameof(GetAppointment), 
                new { id = appointment.Id }, 
                appointmentResponse
                );
        }

        // Get a specific appointment by ID
        [HttpGet("{id}")]
        public ActionResult<Appointment> GetAppointment(int id)
        {
            var appointment = appointmentService.GetAppointment(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return appointment;
        }

        // Get all appointments
        [HttpGet]
        public IEnumerable<Appointment> GetAllAppointments()
        {
            return appointmentService.GetAppointments();
        }

        // Get appointments by patient ID
        [HttpGet("patient/{patientId}")]
        public IEnumerable<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            return appointmentService.GetAppointmentsByPatientId(patientId);
        }

        // Get appointments by provider ID
        [HttpGet("provider/{providerId}")]
        public IEnumerable<Appointment> GetAppointmentsByProviderId(int providerId)
        {
            return appointmentService.GetAppointmentsByProviderId(providerId);
        }

        // Get appointments by clinic ID
        [HttpGet("clinic/{clinicId}")]
        public IEnumerable<Appointment> GetAppointmentsByClinicId(int clinicId)
        {
            return appointmentService.GetAppointmentsByClinicId(clinicId);
        }
    }
}
