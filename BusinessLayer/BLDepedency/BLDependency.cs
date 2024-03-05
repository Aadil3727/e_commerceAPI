using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using BusinessLayer.Mapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DataFirstApproach.Utility.Image;
using BusinessLayer.Services.Category;
using BusinessLayer.Services.Product;

namespace BusinessLayer.BLDepedency
{
    public static class BLDependency
    {
        public static void AddBLDependency(this IServiceCollection services,IConfiguration configuration) 
        {
            services.AddScoped<IProduct, ProductService>();
            services.AddScoped<IUser, Userservices>();
            services.AddScoped<ICategory, CategoryServices>();
           
            services.AddScoped<ImageServicesUtility>();
            services.AddScoped<MultipleImageUtility>();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(UserProfile));
        }
    }
}
