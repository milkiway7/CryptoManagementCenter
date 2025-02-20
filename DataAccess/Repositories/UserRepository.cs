﻿using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) :base(context)
        {
            _context = context;
        }

        public async Task CreateUser(UserModel user)
        {
            _context.Users.Add(user);
            await SaveAsync();
        }
            
        public async Task<UserModel> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == email); 
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }

        }
    }
}
