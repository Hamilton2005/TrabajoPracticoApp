using TrabajoPracticoApp.Models;
using TrabajoPracticoApp.Data;
using Microsoft.EntityFrameworkCore;

namespace TrabajoPracticoApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        { 
            _context = context;
        }



        public async Task<User> AddAsync(User user)
        {
           var entry= await _context.Users.AddAsync(user);
            return entry.Entity;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u =>u.Email==email);

        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users
                .FirstAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<User?> GetUserByUserName(string userName)
        {
            return await _context.Users
               .Include(u => u.Role)
               .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool ValidatePassWord(User user, string passWord)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(passWord, user.Password);
        }
    }
}
