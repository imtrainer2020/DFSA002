using DBO.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBO.IServices
{
    public interface IUserRoleService
    {
        public Task<Result<List<UserRole>>> GetAllRolesAsync();
        public Task<Result<UserRole>> GetRoleByNameAsync(string roleName);
        public Task<Result<int>> AddRoleAsync(string roleName);
        public Task<Result<int>> UpdateRoleAsync(string oldRoleName, string newRoleName);
        public Task<Result<int>> DeleteRoleAsync(string roleName);
    }
}
