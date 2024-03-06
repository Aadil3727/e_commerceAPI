using AutoMapper;
using BusinessLayer.Interfaces;
using DataFirstApproach.Utility.Image;
using DataLayer.Models;
using Demoproject.Models;
using DTO_s_Layer.DTO_Model;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BusinessLayer.Services
{
    public class Userservices : IUser
    {
        private readonly ApplicationContext context;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ImageServicesUtility fileUpload;
        private readonly IConfiguration configuration;

        public Userservices(ApplicationContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment, ImageServicesUtility fileUpload, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
            this.fileUpload = fileUpload;
            this.configuration = configuration;

        }
        public async Task<List<User_db>> List()
        {
            var data = await context.AuthUsers.ToListAsync();
            return mapper.Map<List<User_db>>(data);
        }

        public async Task<User_db> GetUserById(Guid id)
        {
            var user = await context.AuthUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return null;
            }

            return mapper.Map<User_db>(user);
        }

        public async Task<string> Create(UserDTO user_Db)
        {
            try
            {
                var validator = new UserDTOValidator();

                var validationResult = validator.Validate(user_Db);

                if (!validationResult.IsValid)
                {
                    var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return $"Validation failed: {errorMessage}";
                }

                var filepath = fileUpload.SaveBase64Image(user_Db.ProfileImg);
                user_Db.ProfileImg = filepath;
                user_Db.Email = user_Db.Email.ToLower().Trim();
                var name = user_Db.Name.Trim();
                user_Db.Password = Encryption.Encrypt(user_Db.Password);
                var authUserEntity = mapper.Map<User_db>(user_Db);
                context.AuthUsers.Add(authUserEntity);
                await context.SaveChangesAsync();

                return "Success";
            }
            catch (Exception ex)
            {
                return $"Failed: {ex.Message}";
            }
        }

        public async Task<string> Login(LoginDTO user_Db)
        {
            try
            {
                user_Db.Email = user_Db.Email.ToLower().Trim();
                user_Db.Password = Encryption.Encrypt(user_Db.Password);
                var user = context.AuthUsers.Where(x => x.Email == user_Db.Email && x.Password == user_Db.Password).FirstOrDefault();
                if (user == null)
                {
                    return "User not found";
                }

                return "Success";
            }
            catch (Exception ex)
            {
                return "An error occurred";
            }
        }

        public string DeleteUser(Guid id)
        {
            var user = context.AuthUsers.Where(x => x.Id == id).FirstOrDefault();
            if (user != null)
            {
                context.AuthUsers.Remove(user);
                context.SaveChanges();
                return "Success";
            }
            return "User not found";
        }

        public async Task<string> Edit(EditDTO edit_Dto)
        {
            var user = await context.AuthUsers.Where(x => x.Id == edit_Dto.Id).FirstOrDefaultAsync();
            if (user != null)
            {
                // Validate the input DTO first, before making any changes to the user
                var validator = new EditDTOValidator();
                var validationResult = validator.Validate(edit_Dto);

                if (!validationResult.IsValid)
                {
                    var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return $"Validation failed: {errorMessage}";
                }

                user.Email = edit_Dto.Email.ToLower().Trim();
                user.Name = edit_Dto.Name.Trim();

                // Check if a new profile image is provided and update if necessary
                if (!string.IsNullOrWhiteSpace(edit_Dto.ProfileImg))
                {
                    // Assuming `fileUpload` and method `SaveBase64Image` are accessible here
                    // Save the new image and update the user's ProfileImg path
                    var filepath = fileUpload.SaveBase64Image(edit_Dto.ProfileImg);
                    user.ProfileImg = filepath; // Update the ProfileImg property
                }

                // Update user in the database
                context.AuthUsers.Update(user);
                await context.SaveChangesAsync(); // Ensure you await the async call
                return "Success";
            }
            return "User not found";
        }
        public async Task<bool> UserExists(string Email)
        {
            return await context.AuthUsers.AnyAsync(u => u.Email == Email);
        }
        public async Task SendPasswordResetEmail(string toEmail, string resetToken)
        {

            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("test25943026@gmail.com", "inotbtixswttcxlm"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("test25943026@gmail.com"),
                    Subject = "Password Reset",
                    Body = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: 'Arial', sans-serif;
                    background-color: #f4f4f4;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #ffffff;
                    border: 1px solid #dddddd;
                    border-radius: 5px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }}
                .reset-link {{
                    display: block;
                    padding: 10px;
                    background-color: blue;
                    color: #f4f4f4;
                    text-align: center;
                    text-decoration: none;
                    border-radius: 5px;
                }}

            </style>
        </head>
        <body>
            <div class='container'>
                <p>Hello,</p>
                 <p>Click the following link to reset your password:</p>
<a href='https://www.example.com/password-reset?token={resetToken}'><button  class='reset-link'>Reset Password</button></a>
                
            </div>
                <p style='Color:#D3D3D3'>This link will expire within 1 hour</p>
        </body>
        </html>",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);


                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Handle exception (log, etc.)
                throw new Exception("Failed to send email", ex);
            }
        }
        public async Task<string> ForgotPassword(string email)
        {
            var response = "";
            try
            {
                var user = await context.AuthUsers.SingleOrDefaultAsync(x => x.Email == email);

                if (user != null)
                {
                    string resetToken = Guid.NewGuid().ToString();
                    user.ResetToken = resetToken;
                    user.ResetTokenExpiration = DateTime.UtcNow.AddSeconds(5);
                    await context.SaveChangesAsync();
                    await SendPasswordResetEmail(email, resetToken);

                    return response = "Password reset link sent successfully";
                }
                else
                {
                    return response = "User not found";
                }
            }
            catch (Exception ex)
            {
                return "Failed to send password reset link";
            }
        }
        public async Task<string> ResetPassword(string token, string newPassword)
        {
            try
            {
                var user = await context.AuthUsers.SingleOrDefaultAsync(x => x.ResetToken == token);

                if (user != null)
                {
                    if (user.ResetTokenExpiration <= DateTime.UtcNow)
                    {
                        return "Token expired";
                    }

                    user.Password = Encryption.Encrypt(newPassword);
                    user.ResetToken = "-";
                    user.ResetTokenExpiration = null;

                    await context.SaveChangesAsync();

                    return "Password reset successfully";
                }
                else
                {
                    return "Invalid token or user not found";
                }
            }
            catch (Exception ex)
            {
                return "Failed to reset password";
            }
        }
    }
}
