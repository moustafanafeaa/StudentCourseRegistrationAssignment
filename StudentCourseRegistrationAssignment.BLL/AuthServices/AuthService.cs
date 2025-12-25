using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using StudentCourseRegistrationAssignment.BLL.EmailServices;
using StudentCourseRegistrationAssignment.BLL.ViewModels;
using StudentCourseRegistrationAssignment.DAL.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCourseRegistrationAssignment.BLL.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IEmailService _emailService;

        public AuthService(UserManager<ApplicationUser> userManager, IEmailService emailService, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _emailService = emailService;
            _signInManager = signInManager;
        }


        public async Task<GeneralResponse> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
                return new GeneralResponse
                {
                    Message = "Invalid username or password"
                };

            if (!user.EmailConfirmed)
                return new GeneralResponse
                {
                    Message = "Please confirm your email first"
                };

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
                return new GeneralResponse
                {
                    Message = "Invalid username or password"
                };

            return new GeneralResponse
            {
                Success = true,
                Message = "Logged in successfully"
            };
        }

        public async Task<GeneralResponse> RegisterAsync(RegisterViewModel model)
        {
            var response = new GeneralResponse();

            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                response.Success = false;
                response.Message = "Username already exists";
                return response;
            }

            if (await _userManager.FindByEmailAsync(model.Email) != null)

            {
                response.Success = false;
                response.Message = "Email already exists";
                return response;
            }

            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                AcademicYear = model.AcademicYear,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                response.Success = false;
                response.Message = string.Join(",", result.Errors.Select(e => e.Description));

                return response;
            }

            await _userManager.AddToRoleAsync(user, "Student");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmLink =
                $"https://localhost:7054/Account/ConfirmEmail?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            await _emailService.SendAsync(
                user.Email,
                "Confirm your email",
                $"Please confirm your email by clicking <a href='{confirmLink}'>here</a>");

            response.Success = true;
            response.Message = "Registration successful. Please check your email.";
            return response;

        }
        public async Task<GeneralResponse> ForgetPasswordAsync(string email, string resetLinkBase)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) { 
                return new GeneralResponse
                {
                    Message = "Invalid Email",
                    Success = false
                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink =
                $"{resetLinkBase}?token={Uri.EscapeDataString(token)}&email={email}";

            await _emailService.SendAsync(
                email,
                "Reset Password",
                $"<a href='{resetLink}'>Reset Password</a>"
            );

            return new GeneralResponse
            {
                Success = true,
                Message = "Reset password link sent to your email."
            };
        }

        public async Task<GeneralResponse> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                return new GeneralResponse
                {
                    Message = "Invalid Request",
                    Success = false,
                    StatusCode = 400
                };
            }

            var result = await _userManager.ResetPasswordAsync(
               user,
               model.Token,
               model.Password
           );

           if (!result.Succeeded)
           {
                return new GeneralResponse
                {
                    Message = "Password reset failed.",
                    Success = false,
                    StatusCode = 400,
                    Errors = result.Errors
                        .Select(e => e.Description)
                        .ToList()
                };
           }


            return new GeneralResponse
            {
                Message = "Password reset successfully.",
                Success = true,
                StatusCode = 200
          
            };

        }

        public async Task<GeneralResponse> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Invalid confirmation request.",
                    StatusCode = 404
                };
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Email confirmation failed.",
                    StatusCode = 404,
                    Errors = result.Errors
                    .Select(e => e.Description)
                    .ToList()
                };
           
            }
            return new GeneralResponse
            {
                Success = true,
                Message = "Email confirmed successfully.",
                StatusCode =200
            };
       
        }
    }
}
