using DBO.IServices;
using DBO.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBO.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext context;
        public UserService(AppDbContext _context)
        {
            this.context = _context;
        }
        public async Task<Result<IList<User>>> GetAllUsersAsync()
        {
            try
            {
                var list = await context.Users.ToListAsync();
                return Result<IList<User>>.Success(list);
            }
            catch (Exception ex)
            {
                return Result<IList<User>>.Failure("Error: " + ex.Message);
            }
        }
    }
}
