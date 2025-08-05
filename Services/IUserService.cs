using BookLibrarySystem.Models;

namespace BookLibrarySystem.Services
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string email, string password);
        Task<User> RegisterAsync(User user, string password);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
    }
}