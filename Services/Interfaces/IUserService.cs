using ClinicFlow.Models;

namespace ClinicFlow.Services.Interfaces
{
    public interface IUserService
    {
        User GetUserByEmail(string email);

        User GetUserByRole(string role);
    }
}
