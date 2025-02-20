using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment1.Data.Context;
using PRN222.Assignment1.Data.Models;

namespace PRN222.Assignment1.Data.Repositories.Implements
{
    public class UserRepository : IUserRepository
    {
        private readonly Prn222asm1Context _context;

        public UserRepository(Prn222asm1Context context)
        {
            _context = context;
        }
        public async Task<User> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> DeleteUserAsync(int userId)
        {
            User? user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            User? user = await _context.Users.FindAsync(userId);
            return user;
        }

        public async Task<User> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            User? userInDb = await _context.Users.FindAsync(user.UserId);
            if (userInDb != null)
            {
                userInDb.Username = user.Username;
                userInDb.Password = user.Password;
                userInDb.FullName = user.FullName;
                userInDb.Email = user.Email;
                userInDb.Role = user.Role;
                await _context.SaveChangesAsync();
            }
            return userInDb;
        }
    }
}
