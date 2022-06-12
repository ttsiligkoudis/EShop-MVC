using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using EShop.Models;
using EShop.ViewModels;

namespace EShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Order
        public ActionResult Index()
        {
            var orders = _context.Orders.Include(c => c.Customer).ToList();
            var user = (User) Session["User"];
            var customer = (Customer) Session["Customer"];
            if (customer == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (user.UserType != UserType.Admin)
            {
                orders = orders.Where(o => o.CustomerId == customer.Id).ToList();
            }
            return View(orders);
        }

        public ActionResult Edit(int id)
        {
            var viewModel = new OrderViewModel
            {
                Order = _context.Orders.Include(c => c.Customer).SingleOrDefault(c => c.Id == id),
                OrderProducts = _context.OrderProducts.Include(p => p.Product).Where(o => o.OrderId == id).ToList()
            };
            return View("OrderForm", viewModel);
        }

        public ActionResult Create()
        {
            var viewModel = new OrderViewModel
            {
                Order = new Order(),
                OrderProducts = new List<OrderProducts>(),
                Customers = _context.Customers.ToList(),
                Products = _context.Products.Where(p => p.Quantity > 0).ToList()
            };

            var customer = (Customer)Session["Customer"];
            if (customer != null)
            {
                viewModel.Order.Customer = customer;
                viewModel.Order.CustomerId = customer.Id;
            }

            return View(viewModel);
        }

        public ActionResult Save(OrderViewModel viewModel)
        {
            if (viewModel.Order.CustomerId == 0)
            {
                viewModel.Order.Customer.RegDate = DateTime.Now;
                _context.Customers.Add(viewModel.Order.Customer);
                _context.SaveChanges();
                viewModel.Order.CustomerId = viewModel.Order.Customer.Id;
            }
            viewModel.Order.OrderDate = DateTime.Now;
            viewModel.Products = _context.Products.Where(p => viewModel.ProductList.Contains(p.Id)).ToList();
            _context.Orders.Add(viewModel.Order);
            _context.SaveChanges();
            var finalPrice = 0f;
            foreach (var product in viewModel.Products)
            {
                finalPrice += product.Price;
                product.Quantity--;
                _context.OrderProducts.Add(new OrderProducts
                    {OrderId = viewModel.Order.Id, ProductId = product.Id, ProductPrice = product.Price});
            }

            viewModel.Order.FinalPrice = finalPrice;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var order = _context.Orders.SingleOrDefault(o => o.Id == id);
            if (order == null) return RedirectToAction("Index");
            var user = (User) Session["User"];
            if (user != null && order.Completed && user.UserType != UserType.Admin)
            {
                ModelState.AddModelError("", @"Cannot delete a completed order");
                var customer = (Customer) Session["Customer"];
                return View("Index", _context.Orders.Where(o => o.CustomerId == customer.Id).Include(c => c.Customer).ToList());
            }
            var orderProducts = _context.OrderProducts.Where(p => p.OrderId == order.Id).ToList();
            if (orderProducts.Any())
            {
                foreach (var product in orderProducts)
                {
                    _context.OrderProducts.Remove(product);
                }
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CompleteOrder(int id)
        {
            var order = _context.Orders.SingleOrDefault(o => o.Id == id);
            if (order != null)
            {
                order.Completed = true;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}