using DBO.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using DBO.Models;

namespace DBO.Services
{
    public class UserDetailService : IUserDetailService
    {
        private readonly AppDbContext context;

        public UserDetailService(AppDbContext _context)
        {
            this.context = _context;
        }

        public async Task<Result<UserDetail>> GetUserDetailByUserIdAsync(int userId)
        {
            try
            {
                var userDetail = await context.UserDetails.FindAsync(userId);
                return userDetail != null ? Result<UserDetail>.Success(userDetail)
                    : Result<UserDetail>.Failure("User detail not found.");
            }
            catch (Exception ex)
            {
                return Result<UserDetail>.Failure("Error: " + ex.Message);
            }

        }
    }
}
