using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_s_Layer.DTO_Model.Product
{
    public class Product_DTO
    {
       
        public string Name { get; set; }
        public string Description { get; set; }
        public string CatId { get; set; }
        public string Images { get; set; }
        public bool IsOffer { get; set; }
        public decimal offerprice { get; set; }
        public decimal Price { get; set; }

    }

    public class ProductEditDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CatId { get; set; }
        public string Images { get; set; }
        public bool IsOffer { get; set; }
        public decimal offerprice { get; set; }
        public decimal Price { get; set; }

    }
}
