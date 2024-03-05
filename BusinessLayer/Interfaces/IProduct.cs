using DataLayer.Models;
using DTO_s_Layer.DTO_Model.Category;
using DTO_s_Layer.DTO_Model.Product;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IProduct
    {
        Task<string> ProductCreate(Product_DTO product);
        Task<List<Product_db>> List();
        Task<string> Edit(ProductEditDTO edit_Dto);
        string DeletePro(Guid id);

        Task<Product_DTO> GetProductById(Guid id);

    }
}
