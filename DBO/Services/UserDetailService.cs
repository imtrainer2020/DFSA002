using DBO.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using DBO.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Result<int>> DeleteUserDetailAsync(int userDetailId)
        {
            try
            {
                var ud = await context.UserDetails.Where(e => e.Udid == userDetailId).FirstOrDefaultAsync();
                if (ud == null)
                    return Result<int>.Failure("User detail not found.");
                else
                {
                    context.UserDetails.Remove(ud);
                    return Result<int>.Success(await context.SaveChangesAsync(),
                        "User details deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Error: " + ex.Message);
            }
        }

        public async Task<Result<int>> DeleteUserDetailbyUserIdAsync(int userId)
        {
            try
            {
                var ud = await context.UserDetails.Where(e => e.UserId == userId).FirstOrDefaultAsync();
                if (ud == null)
                    return Result<int>.Failure("User detail not found.");
                else
                {
                    context.UserDetails.Remove(ud);
                    return Result<int>.Success(await context.SaveChangesAsync(),
                        "User details deleted successfully.");
                }
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
                var userDetail = await context.UserDetails.Where(u => u.UserId == userId).FirstOrDefaultAsync();
                return userDetail != null ? Result<UserDetail>.Success(userDetail)
                    : Result<UserDetail>.Failure("User detail not found.");
            }
            catch (Exception ex)
            {
                return Result<UserDetail>.Failure("Error: " + ex.Message);
            }

        }

        public async Task<Result<int>> UpdateUserDetailAsync(UserDetail userDetail)
        {
            try
            {
                var ud = await context.UserDetails.Where(e => e.Udid == userDetail.Udid || e.UserId == userDetail.UserId).FirstOrDefaultAsync();
                if (ud == null)
                    return Result<int>.Failure("User detail not found.");
                else
                {
                    ud.FullName = userDetail.FullName;
                    ud.Address = userDetail.Address;
                    ud.City = userDetail.City;
                    ud.PostCode = userDetail.PostCode;
                    ud.Phone = userDetail.Phone;
                    if (userDetail.Photo != null && userDetail.Photo != ud.Photo)
                        ud.Photo = userDetail.Photo;
                    context.UserDetails.Update(ud);
                    await context.SaveChangesAsync();
                    return Result<int>.Success(ud.Udid, "User details updated successfully.");
                }
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Error: " + ex.Message);
            }
        }
    }
}
