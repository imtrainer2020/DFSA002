using DBO.IServices;
using DBO.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace DBO.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext context;
        private readonly IUserDetailService userDetailService;
        public UserService(AppDbContext _context, IUserDetailService _userDetailService)
        {
            this.context = _context;
            this.userDetailService = _userDetailService;
        }

        public async Task<Result<int>> AddUserAsync(User user)
        {
            try
            {
                await context.Users.AddAsync(user);
                return Result<int>.Success(
                    await context.SaveChangesAsync(),
                    "User added successfully.");
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Error: " + ex.Message);
            }
        }

        public async Task<Result<int>> DeleteUserAsync(int userId)
        {
            // 1. Start a transaction
            using var transaction = await this.context.Database.BeginTransactionAsync();

            try
            {
                var user = await GetUserByIdOrEmail(userId);
                if (user != null)
                {
                    // 2. Delete the details first
                    var detailResult = await userDetailService.DeleteUserDetailbyUserIdAsync(userId);

                    // If details couldn't be deleted, we stop and roll back
                    if (!detailResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return Result<int>.Failure("Failed to delete user details: " + detailResult.Message);
                    }

                    context.Users.Remove(user);
                    int rowsAffected = await context.SaveChangesAsync();

                    // 4. If everything reached here without crashing, save it permanently
                    await transaction.CommitAsync();

                    return Result<int>.Success(rowsAffected, "User and details deleted successfully.");
                }
                else
                {
                    return Result<int>.Failure("User not found.");
                }
            }
            catch (Exception ex)
            {
                // 5. If anything goes wrong anywhere, undo everything!
                await transaction.RollbackAsync();
                return Result<int>.Failure("Error: " + ex.Message);
            }

        }

        public async Task<Result<IList<User>>> GetAllUsersAsync()
        {
            try
            {
                return Result<IList<User>>.Success(await context.Users.ToListAsync());
            }
            catch (Exception ex)
            {
                return Result<IList<User>>.Failure("Error: " + ex.Message);
            }
        }

        public async Task<Result<User>> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await GetUserByIdOrEmail(0, email);
                if (user == null)
                    return Result<User>.Failure("User not found.");
                return Result<User>.Success(user);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure("Error: " + ex.Message);
            }
        }

        public async Task<Result<User>> GetUserByUserIdAsync(int userId)
        {
            try
            {
                var user = await GetUserByIdOrEmail(userId);
                if (user == null)
                    return Result<User>.Failure("User not found.");
                return Result<User>.Success(user);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure("Error: " + ex.Message);
            }
        }

        public async Task<Result<bool>> IsEmailExistAsync(string email)
        {
            try
            {
                var res = await GetUserByIdOrEmail(0, email);
                if (res != null)
                    return Result<bool>.Success(true, "User with the same Email already Exist");
                else
                    return Result<bool>.Failure("User Not Available");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Error: " + ex.Message);
            }
        }

        public async Task<Result<int>> ResetPasswordAsync(string newPassword, string email)
        {
            try
            {
                var user = await GetUserByIdOrEmail(0, email);
                if (user == null)
                    return Result<int>.Failure("User not found.");

                user.Password = newPassword;

                context.Users.Update(user);
                return Result<int>.Success(
                    await context.SaveChangesAsync(),
                    "Password changed successfully.");
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Error: " + ex.Message);
            }
        }

        public async Task<Result<int>> UpdateUserAsync(User user)
        {
            try
            {
                var existingUser = await GetUserByIdOrEmail(user.UserId, user.Email);
                if (existingUser == null)
                    return Result<int>.Failure("User not found.");
                existingUser.Role = user.Role;
                existingUser.ModifiedDate = DateTime.UtcNow;

                context.Users.Update(existingUser);
                return Result<int>.Success(
                    await context.SaveChangesAsync(),
                    "User updated successfully.");

            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Error: " + ex.Message);
            }
        }

        public async Task<Result<bool>> ValidateLoginCredentials(string email, string password)
        {
            try
            {
                var existingUser = await GetUserByIdOrEmail(0, email);
                if (existingUser == null)
                    return Result<bool>.Failure("User not found: Email not exist");
                else
                {
                    if (existingUser.Password == password)
                        return Result<bool>.Success(true);
                    else
                        return Result<bool>.Failure("Invalid password.");
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Error: " + ex.Message);
            }

        }

        private async Task<User?> GetUserByIdOrEmail(int userId, string email = null)
        {
            if (email != null && email.Length > 0)
                return await context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            else if (userId > 0)
                return await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            else
                return null;
        }
    }
}
