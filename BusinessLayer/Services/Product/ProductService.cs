using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.RegisterValidator.Product;
using DataFirstApproach.Utility.Image;
using DataLayer.Models;
using DTO_s_Layer.DTO_Model.Category;
using DTO_s_Layer.DTO_Model.Product;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Product
{
    public class ProductService : IProduct
    {
        private readonly ApplicationContext context;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ImageServicesUtility fileUpload;
        private readonly MultipleImageUtility multipleImageUtility;

        public ProductService(ApplicationContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment, ImageServicesUtility fileUpload, MultipleImageUtility multipleImageUtility)
        {
            this.context = context;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
            this.fileUpload = fileUpload;
            this.multipleImageUtility = multipleImageUtility;
        }
        public async Task<List<Product_db>> List()
        {
            var data = await context.Product.ToListAsync();
            return mapper.Map<List<Product_db>>(data);
        }
        public async Task<string> ProductCreate(Product_DTO product)
        {
            try
            {
                var validator = new AddProValidator();

                var validationResult = validator.Validate(product);

                if (!validationResult.IsValid)
                {
                    var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return $"Validation failed: {errorMessage}";
                }
                // Check if product and images are provided
                if (product == null || string.IsNullOrEmpty(product.Images))
                {
                    return "Failed";
                }

                // Save base64 images and get file paths
                var imageFileNames = multipleImageUtility.SaveBase64Images(product.Images);

                // Map DTO to Entity
                var productEntity = mapper.Map<Product_db>(product);

                // Generate a new Guid for the product
                productEntity.Id = Guid.NewGuid();

                // Assign concatenated file names to the product entity's Image property
                productEntity.Images = string.Join(",", imageFileNames);

                // Add the product entity to the database context and save changes
                context.Product.Add(productEntity);
                context.SaveChanges(); // Use SaveChanges method

                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating product: {ex.Message}");
                return "Failed";
            }
        }
        public string DeletePro(Guid id)
        {
            var user = context.Product.Where(x => x.Id == id).FirstOrDefault();
            if (user != null)
            {
                context.Product.Remove(user);
                context.SaveChanges();
                return "Success";
            }
            return "User not found";
        }
        public async Task<string> Edit(ProductEditDTO edit_Dto)
        {
            try
            {
                var validator = new EditProductValidator();

                var validationResult = validator.Validate(edit_Dto);

                if (!validationResult.IsValid)
                {
                    var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return $"Validation failed: {errorMessage}";
                }

                var productToUpdate = await context.Product.FindAsync(edit_Dto.Id);
                if (productToUpdate == null)
                {
                    return "Category not found";
                }

                // If a new image is provided, process it. Otherwise, keep the existing image.
                if (!string.IsNullOrEmpty(edit_Dto.Images))
                {
                    var filePaths = multipleImageUtility.SaveBase64Images(edit_Dto.Images);

                    // Assuming you want to store the file paths as a single string, delimited by some character (e.g., '|')
                    edit_Dto.Images = string.Join("|", filePaths.Where(path => path != null));
                }
                else
                {
                    // Retain the existing images if new ones aren't provided
                    edit_Dto.Images = productToUpdate.Images;
                }

                // Update properties
                productToUpdate.Name = edit_Dto.Name.Trim();
                productToUpdate.Description = edit_Dto.Description;
                productToUpdate.Images = edit_Dto.Images; // This will either be a new image path or the existing one
                productToUpdate.IsOffer = edit_Dto.IsOffer;
                productToUpdate.offerprice = edit_Dto.offerprice;// Update the IsActive property

                await context.SaveChangesAsync();

                return "Success";
            }
            catch (Exception ex)
            {
                return $"Failed: {ex.Message}";
            }
        }
        public async Task<Product_DTO> GetProductById(Guid id)
        {
            var user = await context.Product.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return null;
            }

            return mapper.Map<Product_DTO>(user);
        }

    }
}
