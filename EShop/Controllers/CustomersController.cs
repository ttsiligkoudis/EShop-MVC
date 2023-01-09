using System.Linq;
using System.Web.Mvc;
using System.Web;
using EShop.Models;

namespace EShop.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context = ApplicationDbContext.Create();

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            var customer = _context.Customers.ToList();
            return View(customer);
        }

        public ActionResult Create()
        {
            var customer = new Customer
            {
                RegDate = System.DateTime.Now
            };
            return View("CustomerForm", customer);
        }

        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return RedirectToAction("Index");
            }
            return View("CustomerForm", customer);
        }

        [HttpPost]
        public ActionResult Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View("CustomerForm", customer);
            }
            if (customer.Id == 0)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                var userInDb = _context.Customers.Single(c => c.Id == customer.Id);
                userInDb.Name = customer.Name;
                userInDb.Email = customer.Email;
                userInDb.City = customer.City;
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var customer = _context.Customers.Single(c => c.Id == id);
            var loggedUser = (User) Session["User"];
            if (customer != null)
            {
                DeleteCustomerDependencies(customer);
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
            return loggedUser != null ? RedirectToAction("Logout", "User", loggedUser) : RedirectToAction("Index");
        }

        public void DeleteCustomerDependencies(Customer customer)
        {
            if (customer.UserId != null)
            {
                DeleteUserDependencies((int) customer.UserId);
            }
            var customerOrders = _context.Orders.Where(c => c.CustomerId == customer.Id).ToList();
            if (customerOrders.Any())
            {
                foreach (var order in customerOrders)
                {
                    DeleteOrderDependencies(order);
                    _context.Orders.Remove(order);
                }
                _context.SaveChanges();
            }
        }

        public void DeleteOrderDependencies(Order order)
        {
            var orderProducts = _context.OrderProducts.Where(c => c.OrderId == order.Id).ToList();
            if (orderProducts.Any())
            {
                foreach (var product in orderProducts)
                {
                    _context.OrderProducts.Remove(product);
                }
                _context.SaveChanges();
            }
        }

        public void DeleteUserDependencies(int id)
        {
            var user = _context.Users.Single(c => c.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}