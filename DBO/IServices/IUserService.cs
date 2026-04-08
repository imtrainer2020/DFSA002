using System;
using System.Collections.Generic;
using System.Text;
using DBO.Models;
using System.Linq;

namespace DBO.IServices
{
    public interface IUserService
    {
        public Task<Result<IList<User>>> GetAllUsersAsync();
        public Task<Result<User>> GetUserByEmailAsync(string email);
        public Task<Result<User>> GetUserByUserIdAsync(int userId);
        public Task<Result<int>> AddUserAsync(User user);
        public Task<Result<int>> UpdateUserAsync(User user);
        public Task<Result<int>> DeleteUserAsync(int userId);
        public Task<Result<bool>> IsEmailExistAsync(string email);
        public Task<Result<int>> ResetPasswordAsync(string newPassword, string email);
        public Task<Result<bool>> ValidateLoginCredentials(string email, string password);

    }
}
