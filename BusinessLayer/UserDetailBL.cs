using DBO;
using DBO.Models;
using DBO.Services;

namespace BusinessLayer
{
    public class UserDetailBL
    {
        UserDetailService service;
        public async Task AddUserDetailAsync(object userDetailVM)
        {
            service = new UserDetailService();
            UserDetail userDetail = new UserDetail
            {
                UserId = (int)userDetailVM.GetType().GetProperty("UserId").GetValue(userDetailVM),
                //Photo = (byte[])userDetailVM.GetType().GetProperty("Photo").GetValue(userDetailVM),

            };
        }
    }
}
