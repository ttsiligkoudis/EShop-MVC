using System;
using System.Linq;
using System.Web.Mvc;
using EShop.Models;
using EShop.ViewModels;

namespace EShop.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context = ApplicationDbContext.Create();

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: User
        public ActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public ActionResult Register()
        {
            return View("Register", new RegisterViewModel());
        }
        [HttpPost]
        public ActionResult RegisterViewResult(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid) return View("Register", viewModel);
            var user = new User
            {
                Email = viewModel.Email,
                Password = viewModel.Password,
                RegDate = DateTime.Now,
                UserType = UserType.User
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            viewModel.Customer.Email = user.Email;
            viewModel.Customer.RegDate = user.RegDate;
            viewModel.Customer.UserId = user.Id;
            _context.Customers.Add(viewModel.Customer);
            _context.SaveChanges();
            return RedirectToAction("LoginViewResult", user);
        }
        public ActionResult Login()
        {
            return View("Login");
        }
        public ActionResult LoginViewResult(User user)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", user);
            }
            var userInDb = _context.Users.SingleOrDefault(u =>
                u.Email == user.Email && u.Password == user.Password);
            if (userInDb != null)
            {
                Session["User"] = userInDb;
                Session["UserType"] = userInDb.UserType;
                Session["UserId"] = userInDb.Id;
                var customer = _context.Customers.SingleOrDefault(c => c.UserId == userInDb.Id);
                if (customer != null)
                {
                    Session["Customer"] = customer;
                    Session["CustomerName"] = customer.Name;
                }
                userInDb.LoginDate = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("",@"Invalid email or password");
            return View("Login", user);

        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            Session["UserType"] = null;
            Session["UserId"] = null;
            Session["Customer"] = null;
            Session["CustomerName"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Details(int id)
        {
            var viewModel = new UserViewModel
            {
                User = _context.Users.SingleOrDefault( u => u.Id == id),
                Customer = _context.Customers.SingleOrDefault( c => c.UserId == id)
            };
            return viewModel.User == null || viewModel.Customer == null
                ? (ActionResult) RedirectToAction("Index", "Home")
                : View("Details", viewModel);
        }

        public ActionResult ConvertToAdmin(int id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.UserType = UserType.Admin;
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "User");
        }
    }
}