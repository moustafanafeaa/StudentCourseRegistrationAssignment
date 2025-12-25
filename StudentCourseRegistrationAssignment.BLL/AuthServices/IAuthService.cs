using StudentCourseRegistrationAssignment.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCourseRegistrationAssignment.BLL.AuthServices
{
    public interface IAuthService
    {
        Task<GeneralResponse> RegisterAsync(RegisterViewModel model);
        Task<GeneralResponse> LoginAsync(LoginViewModel model);
        Task<GeneralResponse> ConfirmEmailAsync(string userId, string token);
        Task<GeneralResponse> ForgetPasswordAsync(string email, string resetLinkBase);
        Task<GeneralResponse> ResetPasswordAsync(ResetPasswordViewModel model);
    }
}
