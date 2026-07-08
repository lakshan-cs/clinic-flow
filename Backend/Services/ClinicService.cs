using ClinicFlow.Repositories;
using ClinicFlow.Models;
using ClinicFlow.Services.Interfaces;
using ClinicFlow.Exceptions;

namespace ClinicFlow.Services
{
    public class ClinicService : IClinicService
    {
        private readonly IClinicRepository clinicRepository;
        public ClinicService(IClinicRepository clinicRepository)
        {
            this.clinicRepository = clinicRepository;
        }

        public void AddClinic(Clinic clinic)
        {
            clinicRepository.AddClinic(clinic);
        }

        public IEnumerable<Clinic> GetClinics()
        {
            return clinicRepository.GetClinics();
        }

        public Clinic GetClinic(int id)
        {
             var clinic = clinicRepository.GetClinicById(id);
             if (clinic == null)
             {
                 throw new NotFoundException("Clinic not found with ID: " + id);
             }
             return clinic;
        }

        public void UpdateClinic(Clinic clinic)
        {
            var existingClinic = clinicRepository.GetClinicById(clinic.Id);
            if (existingClinic == null)
            {
                throw new NotFoundException("Clinic not found with ID: " + clinic.Id);
            }
            
            existingClinic.Name = clinic.Name;
            existingClinic.Location = clinic.Location;

            clinicRepository.UpdateClinic(existingClinic);
        }

        public void DeleteClinic(int id)
        {
            if (clinicRepository.GetClinicById(id) != null)
                clinicRepository.DeleteClinic(id);
            else
                throw new NotFoundException("Clinic not found with ID: " + id);
        }
    }
}
