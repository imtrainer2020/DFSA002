using DBO.IServices;
using DBO.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBO.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly AppDbContext context;
        public UserRoleService(AppDbContext _context)
        {
            context = _context;
        }

        public async Task<Result<int>> AddRoleAsync(string roleName)
        {
            try
            {
                await context.UserRoles.AddAsync(new UserRole
                {
                    RoleName = roleName
                });
                return Result<int>.Success(await context.SaveChangesAsync(), "Role saved successfully.");
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Error: " + ex.Message);
            }
        }

        public async Task<Result<int>> DeleteRoleAsync(string roleName)
        {
            try
            {
                var role = await context.UserRoles.Where(e => e.RoleName == roleName).FirstOrDefaultAsync();
                if (role != null)
                {
                    context.UserRoles.Remove(role);
                    return Result<int>.Success(await context.SaveChangesAsync(), "Role deleted successfully.");
                }
                else
                {
                    return Result<int>.Failure("Role not found.");
                }
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Error: " + ex.Message);
            }
        }

        public async Task<Result<List<UserRole>>> GetAllRolesAsync()
        {
            try
            {
                var list = await context.UserRoles.ToListAsync();
                return Result<List<UserRole>>.Success(list);
            }
            catch (Exception ex)
            {
                return Result<List<UserRole>>.Failure("Error: " + ex.Message);
            }

        }

        public async Task<Result<UserRole>> GetRoleByNameAsync(string roleName)
        {
            try
            {
                var role = await context.UserRoles.Where(e => e.RoleName == roleName).FirstOrDefaultAsync();
                return Result<UserRole>.Success(role);
            }
            catch (Exception ex)
            {
                return Result<UserRole>.Failure("Error: " + ex.Message);
            }
        }

        public async Task<Result<int>> UpdateRoleAsync(string oldRoleName, string newRoleName)
        {
            try
            {
                var role = await context.UserRoles.Where(e => e.RoleName == oldRoleName).FirstOrDefaultAsync();
                role.RoleName = newRoleName;
                return Result<int>.Success(await context.SaveChangesAsync(), "Role updated successfully.");
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Error: " + ex.Message);
            }
        }
    }
}
