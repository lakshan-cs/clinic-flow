using ClinicFlow.Dto;
using ClinicFlow.Models;

namespace ClinicFlow.Services.Interfaces
{
    public interface IUserService
    {
        LoginResponse Login(LoginRequest loginRequest);

        User GetUserByRole(string role);
    }
}
