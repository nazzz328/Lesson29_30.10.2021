using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal WeightKg { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        [ForeignKey("ProductCategoryId")]
        public int ProductCategoryId { get; set; }
    }
}
