using ClinicFlow.Models;

namespace ClinicFlow.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        User GetUserByRole(string role);
    }
}
