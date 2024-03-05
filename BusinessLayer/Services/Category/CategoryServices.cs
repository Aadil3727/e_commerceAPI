using AutoMapper;
using BusinessLayer.Interfaces;
using DataFirstApproach.Utility.Image;
using DataLayer.Models;
using Demoproject.Models;
using DTO_s_Layer.DTO_Model;
using DTO_s_Layer.DTO_Model.Category;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Category
{
    public class CategoryServices : ICategory
    {
        private readonly ApplicationContext context;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ImageServicesUtility fileUpload;

        public CategoryServices(ApplicationContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment, ImageServicesUtility fileUpload)
        {
            this.context = context;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
            this.fileUpload = fileUpload;
        }
        public async Task<List<Category_db>> List()
        {
            var data = await context.Category.ToListAsync();
            return mapper.Map<List<Category_db>>(data);
        }


        public async Task<string> Create(Category_DTO cat_Db)
        {
            try
            {
                var validator = new AddCatValidator();

                var validationResult = validator.Validate(cat_Db);

                if (!validationResult.IsValid)
                {
                    var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return $"Validation failed: {errorMessage}";
                }

                var filepath = fileUpload.SaveBase64Image(cat_Db.CatImg);
                cat_Db.CatImg = filepath;
                var name = cat_Db.Name.Trim();
                var des = cat_Db.Description;
                var authUserEntity = mapper.Map<Category_db>(cat_Db);
                context.Category.Add(authUserEntity);
                await context.SaveChangesAsync();

                return "Success";
            }
            catch (Exception ex)
            {
                return $"Failed: {ex.Message}";
            }
        }
        public async Task<string> Edit(EditCatDTO edit_Dto)
        {
            try
            {
                var validator = new EditCatValidator();

                var validationResult = validator.Validate(edit_Dto);

                if (!validationResult.IsValid)
                {
                    var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return $"Validation failed: {errorMessage}";
                }

                var categoryToUpdate = await context.Category.FindAsync(edit_Dto.Id);
                if (categoryToUpdate == null)
                {
                    return "Category not found";
                }

                // If a new image is provided, process it. Otherwise, keep the existing image.
                if (!string.IsNullOrEmpty(edit_Dto.CatImg))
                {
                    var filepath = fileUpload.SaveBase64Image(edit_Dto.CatImg);
                    edit_Dto.CatImg = filepath; // Update the DTO with the new image path
                }
                else
                {
                    edit_Dto.CatImg = categoryToUpdate.CatImg; // Retain the existing image if a new one isn't provided
                }

                // Update properties
                categoryToUpdate.Name = edit_Dto.Name.Trim();
                categoryToUpdate.Description = edit_Dto.Description;
                categoryToUpdate.CatImg = edit_Dto.CatImg; // This will either be a new image path or the existing one
                categoryToUpdate.IsActive = edit_Dto.IsActive; // Update the IsActive property

                // No need to add, just save changes as the entity is already being tracked
                await context.SaveChangesAsync();

                return "Success";
            }
            catch (Exception ex)
            {
                return $"Failed: {ex.Message}";
            }
        }

        public string DeleteCat(Guid id)
        {
            var user = context.Category.Where(x => x.Id == id).FirstOrDefault();
            if (user != null)
            {   
                context.Category.Remove(user);
                context.SaveChanges();
                return "Success";
            }
            return "User not found";
        }
    }
}
