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

        public async Task<Result<int>> AddUserDetailAsync(UserDetail userDetail)
        {
            try
            {
                await context.UserDetails.AddAsync(userDetail);
                await context.SaveChangesAsync();

                // Return the primary key (Udid) on success
                return Result<int>.Success(userDetail.Udid, "User details and photo saved successfully.");
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Error: " + ex.Message);
            }
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
