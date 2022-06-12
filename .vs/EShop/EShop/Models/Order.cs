using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public float FinalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Completed { get; set; }
    }
}