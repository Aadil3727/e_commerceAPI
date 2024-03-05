using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_s_Layer.DTO_Model.Category
{
    public class EditCatDTO
    {

        public Guid Id { get; set; }    
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string CatImg { get; set; }
    }
}
