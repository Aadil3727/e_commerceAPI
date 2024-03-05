using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Product_db
    {
        public Guid Id { get; set; }
        public string CatId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public bool IsOffer { get; set; }
        public decimal offerprice { get; set; }
        public string Images { get; set; } 

    }

}

