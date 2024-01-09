using Microsoft.EntityFrameworkCore;
using Task_Management.DAL;
using Task_Management.Model;


namespace Task_Management.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);
    }

    public class UserService : IUserService
    {
        private readonly Contexto _contexto;

        public UserService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            var user = await _contexto.User
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            return user;
        }
    }
}