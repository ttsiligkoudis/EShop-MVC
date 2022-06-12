using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EShop.ViewModels;

namespace EShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product Description")]
        public string Name { get; set; }
        [Required]
        public Category Category { get; set; }
        public string Color { get; set; }
        [Required]
        public float Price { get; set; }
        public int? Quantity { get; set; }
    }
}