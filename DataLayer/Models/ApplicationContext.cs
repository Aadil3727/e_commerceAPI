using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Models
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        { }
        public DbSet<User_db> AuthUsers { get; set; }

        public DbSet<Category_db> Category { get; set; }

        public DbSet<Product_db> Product { get; set; }

    }
}
