using EShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EShop.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        public float? Price { get; set; }
        public Product Product { get; set; }
    }
}