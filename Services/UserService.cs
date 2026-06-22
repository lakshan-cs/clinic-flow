using ClinicFlow.Dto;
using ClinicFlow.Exceptions;
using ClinicFlow.Models;
using ClinicFlow.Repositories.Interfaces;
using ClinicFlow.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public LoginResponse Login(LoginRequest loginRequest)
        {
            var user = GetUserByEmail(loginRequest.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                throw new InvalidCredentialsException("Invalid email or password.");
            }

            var loginResponse = new LoginResponse
            {
                Id = user.Id.ToString(),
                Username = user.Username,
                Email = user.Email
            };

            return loginResponse;
        }

        private User GetUserByEmail(string email)
        {
            return userRepository.GetUserByEmail(email);
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

    
