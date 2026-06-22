using ClinicFlow.Data;
using ClinicFlow.Models;
using ClinicFlow.Repositories.Interfaces;

namespace ClinicFlow.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ClinicDbContext context;

        public UserRepository(ClinicDbContext context)
        {
            this.context = context;
        }

        public User GetUserByEmail(string email)
        {
            return context.Users.FirstOrDefault(u => u.Email == email);

        }

        public User GetUserByRole(string role)
        {
            return context.Users.FirstOrDefault(u => u.Role == role);
        }
    }
}
