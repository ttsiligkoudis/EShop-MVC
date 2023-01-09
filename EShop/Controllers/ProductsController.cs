using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using EShop.Models;
using EShop.ViewModels;

namespace EShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context = ApplicationDbContext.Create();

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            var viewModel = new ProductViewModel
            {
                Product = new Product()
            };
            return View("ProductForm", viewModel);
        }

        public ActionResult Edit(int id)
        {
            var product = _context.Products.SingleOrDefault(c => c.Id == id);
            if (product == null) return RedirectToAction("Index", "Products");

            var viewModel = new ProductViewModel
            {
                Product = product,
                Price = product.Price
            };
            return View("ProductForm", viewModel);
        }

        [HttpPost]
        public ActionResult Save(ProductViewModel viewModel)
        {
            if (viewModel.Price != null) viewModel.Product.Price = (float) viewModel.Price;
            if (!ModelState.IsValid) return View("ProductForm", viewModel);
            if (viewModel.Product.Id == 0)
            {
                _context.Products.Add(viewModel.Product);
            }
            else
            {
                var productInDb = _context.Products.Single(c => c.Id == viewModel.Product.Id);
                productInDb.Name = viewModel.Product.Name;
                productInDb.Category = viewModel.Product.Category;
                productInDb.Color = viewModel.Product.Color;
                productInDb.Quantity = viewModel.Product.Quantity;
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }
    }
}