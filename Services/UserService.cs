using ClinicFlow.Exceptions;
using ClinicFlow.Models;
using ClinicFlow.Repositories.Interfaces;
using ClinicFlow.Services.Interfaces;

namespace ClinicFlow.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public User GetUserByEmail(string email)
        {
            var user = userRepository.GetUserByEmail(email);
            if (user == null)
            {
                throw new UserNotFoundException("User not found with email: " + email);
            }

            return user;
        }

        public User GetUserByRole(string role)
        {
            var user = userRepository.GetUserByRole(role);
            if (user == null)
            {
                throw new InvalidRoleException("Invalid role: " + role);
            }

            return user;
        }
    }
}

    
