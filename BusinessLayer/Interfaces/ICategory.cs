using DataLayer.Models;
using DTO_s_Layer.DTO_Model;
using DTO_s_Layer.DTO_Model.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ICategory
    {
        Task<List<Category_db>> List();

        Task<string> Create(Category_DTO cat_Db);

        Task<string> Edit(EditCatDTO edit_Dto);

        string DeleteCat(Guid id);

    }
}
