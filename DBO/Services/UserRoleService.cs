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
    }
}
