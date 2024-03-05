using static BusinessLayer.Mapper.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DTO_s_Layer.DTO_Model;
using DataLayer.Models;
using DTO_s_Layer.DTO_Model.Category;
using DTO_s_Layer.DTO_Model.Product;

namespace BusinessLayer.Mapper
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<User_db, UserDTO>().ReverseMap();

            CreateMap<User_db, LoginDTO>();

            CreateMap<User_db, EditDTO>();

            CreateMap<Category_DTO, Category_db>();

            CreateMap<Category_db, EditCatDTO>();

            CreateMap<Product_DTO, Product_db>();
            CreateMap<Product_db,Product_DTO>();


            CreateMap<ProductEditDTO, Product_db>();
            CreateMap<Product_db,ProductEditDTO >();


        }
    }
}
