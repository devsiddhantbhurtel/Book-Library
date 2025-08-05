using BookLibrarySystem.Models;
using BookLibrarySystem.Repositories;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BookLibrarySystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private static readonly Random _random = new Random();

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private string GenerateMembershipId()
        {
            // Generate a random 8-digit number
            int membershipNumber;
            do
            {
                membershipNumber = _random.Next(10000000, 99999999);
            } while (IsMembershipIdTaken(membershipNumber.ToString()));

            return membershipNumber.ToString();
        }

        private bool IsMembershipIdTaken(string membershipId)
        {
            return _userRepository.GetUserByMembershipIdAsync(membershipId).Result != null;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                Console.WriteLine($"User not found for email: {email}");
                return null;
            }
            if (!VerifyPasswordHash(password, user.PasswordHash))
            {
                Console.WriteLine($"Password mismatch for user: {email}");
                return null;
            }
            Console.WriteLine($"User authenticated: {email}");
            return user;
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required");

            user.PasswordHash = HashPassword(password);
            user.CreatedAt = DateTime.UtcNow;

            // Only assign membership ID to regular users
            if (user.Role == "User")
            {
                user.MembershipID = GenerateMembershipId();
            }

            return await _userRepository.CreateUserAsync(user);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            var hashOfInput = HashPassword(password);
            return storedHash == hashOfInput;
        }
    }
}