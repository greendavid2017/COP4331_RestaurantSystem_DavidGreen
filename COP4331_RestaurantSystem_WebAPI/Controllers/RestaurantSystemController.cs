using COP4331_RestaurantSystem_WebAPI.Helpers;
using COP4331_RestaurantSystem_WebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace COP4331_RestaurantSystem_WebAPI.Controllers
{
    public class RestaurantSystemController : Controller
    {
        // API calls which require a user to be logged in call this method to determine if the user has a valid token
        private bool isTokenValid()
        {
            // Get the token and email address (to identify user) from the HTTPRequest header
            var apiToken = Request.Headers.Get("apiToken");
            var email = Request.Headers.Get("email");

            // Check if a token and email address was provided in the header
            if (apiToken != null && email != null)
            {
                // Decipher the token using the agreed upon deciphering method
                var decipheredToken = EncryptionHelper.aesDecipher(apiToken, email);
                
                // The deciphered token should be a DateTime if it is valid,
                // If it does parse as a DateTime and the DateTime is in the future the user is still authorized
                DateTime expires;
                if(DateTime.TryParse(decipheredToken, out expires))
                {
                    if(DateTime.Now < expires)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // GET: RestaurantSystem
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CheckToken()
        {
            return (isTokenValid() ? new HttpStatusCodeResult(HttpStatusCode.OK) : new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        // GET: RestaurantSystem/GetMenuItems
        [HttpGet]
        public ActionResult GetMenuItems()
        {
           // if (!isTokenValid())
           //     return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            using(var db = new RestaurantSystemDataContext())
            {
                var menuItems = db.MenuItems.ToList();
                return Json(menuItems, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CreateMenuItem(MenuItem item)
        {
            using(var db = new RestaurantSystemDataContext())
            {
                db.MenuItems.Add(item);
                db.SaveChanges();
            }
            return Json(item);
        }

        [HttpPost]
        public ActionResult Login(String email, String password)
        {
            using (var db = new RestaurantSystemDataContext())
            {
                // Search the database for a registered user with the corresponding email and password
                var encryptedPassword = EncryptionHelper.sha256(password);
                var user = db.Users.Where(u => u.Email == email && u.Password == encryptedPassword).FirstOrDefault();

                // If a user was found, we can log them in
                if (user != null)
                {
                    // Create a token which will be good for three days from time of login
                    var ciphered = EncryptionHelper.aesCipher(DateTime.Now.AddDays(3).ToString(), email);
                    var deciphered = EncryptionHelper.aesDecipher(ciphered, email);
                    // Return the token
                    return Json(ciphered);

                    //var deciphered = EncryptionHelper.aesDecipher(ciphered, email);
                }

                // Return 401 (Unauthorized) if user data not found or incorrect
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost]
        public ActionResult Register(String email, String password)
        {
            using(var db = new RestaurantSystemDataContext())
            {
                // Check and make sure this email doesn't already exist
                var user = db.Users.Where(u => u.Email == email).FirstOrDefault();

                // If a user with the given username does not already exist, we can proceed with registration
                if(user == null)
                {
                    db.Users.Add(new User()
                    {
                        Email = email,
                        Password = EncryptionHelper.sha256(password),
                        IsEmployee = false
                    });

                    db.SaveChanges();

                    // Immediately log the user in upon successfully registering
                    return Login(email, password);
                }

                // If a user with this email already exists, return a conflict code
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);

            }
        }

        [HttpPost]
        public ActionResult CreateUser(User user)
        {
            using (var db = new RestaurantSystemDataContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            return Json(user);
        }

        // GET: RestaurantSystem/GetOrdersByUser?id=
        public ActionResult GetUserAndOrders(int id)
        {
            using (var db = new RestaurantSystemDataContext())
            {
                var user = db.Users.Where(u => u.ID == id).Include(o => o.Orders).FirstOrDefault();

                var result = JsonConvert.SerializeObject(user, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            
        }

        // GET: RestaurantSystem/GetOrders
        public ActionResult GetOrders()
        {
            using (var db = new RestaurantSystemDataContext())
            {
                var orders = db.Orders.ToList();
                return Json(orders, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
