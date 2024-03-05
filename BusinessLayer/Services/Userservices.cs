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
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Userservices(ApplicationContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment, ImageServicesUtility fileUpload)
        {
            this.context = context;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
            this.fileUpload = fileUpload;
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


    }
}
