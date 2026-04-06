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
        //public Task<Result<User>> GetUserByEmailAsync(string email);
        //public Task<Result<User>> GetUserByUserIdAsync(int userId);
        //public Task<Result<int>> AddUserAsync();
        //public Task<Result<int>> UpdateUserAsync();
        //public Task<Result<int>> DeleteUserAsync();
        //public Task<Result<bool>> IsEmailExistAsync();
        //public Task<Result<int>> ResetPasswordAsync();

    }
}
