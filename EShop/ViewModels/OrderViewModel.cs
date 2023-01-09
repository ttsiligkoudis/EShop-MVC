using EShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EShop.ViewModels
{
    public class OrderViewModel : Controller
    {
        [Required]
        public Order Order { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public List<OrderProducts> OrderProducts { get; set; }
        [Required]
        public int[] ProductList { get; set; }
    }
}