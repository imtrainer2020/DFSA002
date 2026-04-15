using System;
using System.Collections.Generic;
using System.Text;
using DBO.Models;

namespace DBO.IServices
{
    public interface IUserDetailService
    {
        public Task<Result<UserDetail>> GetUserDetailByUserIdAsync(int userId);
        public Task<Result<int>> AddUserDetailAsync(UserDetail userDetail);
        public Task<Result<int>> UpdateUserDetailAsync(UserDetail userDetail);
        public Task<Result<int>> DeleteUserDetailAsync(int userDetailId);
        public Task<Result<int>> DeleteUserDetailbyUserIdAsync(int userId);
    }
}
