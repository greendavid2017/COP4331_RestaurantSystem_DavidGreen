using COP4331_RestaurantSystem_WebAPI.Handlers;
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

        // GET: RestaurantSystem
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CheckToken()
        {
            AccountHandler handler = new AccountHandler();
            var apiToken = Request.Headers.Get("apiToken");
            var email = Request.Headers.Get("email");
            var tokenValid = handler.isTokenValid(apiToken, email);
            return (tokenValid ? new HttpStatusCodeResult(HttpStatusCode.OK) : new HttpStatusCodeResult(HttpStatusCode.Unauthorized));
        }

        // GET: RestaurantSystem/GetMenuItems
        [HttpGet]
        public ActionResult GetMenuItems()
        {
            MenuHandler handler = new MenuHandler();
            
            return Json(handler.GetMenuItemsDb(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateMenuItem(MenuItem item)
        {
            MenuHandler handler = new MenuHandler();
            handler.CreateMenuItemDb(item);
            return Json(item);
        }

        [HttpPost]
        public ActionResult Login(String email, String password)
        {
            AccountHandler handler = new AccountHandler();
            bool loginSuccess = handler.LoginDb(email, password);
            // If a user was found, we can log them in
            if (loginSuccess)
            {
                // Create a token which will be good for three days from time of login
                var ciphered = EncryptionHelper.aesCipher(DateTime.Now.AddDays(3).ToString(), email);
                // Return the token
                return Json(ciphered);
            }

            // Return 401 (Unauthorized) if user data not found or incorrect
            return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public ActionResult Register(String email, String password)
        {
            AccountHandler handler = new AccountHandler();
            var registrationSuccess = handler.RegisterDb(email, password);
            // If a user with the given username does not already exist, we can proceed with registration
            if(registrationSuccess)
            {
                return Login(email, password);
            }

            // If a user with this email already exists, return a conflict code
            return new HttpStatusCodeResult(HttpStatusCode.Conflict);

        }

        [HttpPost]
        public ActionResult CreateOrder(String email, List<MenuItem> orderItems)
        {
            OrderHandler handler = new OrderHandler();
            var orderCreated = handler.CreateOrderDb(email, orderItems);
            // If a user with the given username does not already exist, we can proceed with registration
            if (orderCreated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            // If a user with this email already exists, return a conflict code
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

        }

        [HttpGet]
        public ActionResult GetUserOrders(String email)
        {
            OrderHandler handler = new OrderHandler();
            var user = handler.GetUserOrdersDb(email);

            var result = JsonConvert.SerializeObject(user, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Content(result);

        }

        // GET: RestaurantSystem/GetOrders
        public ActionResult GetOrders()
        {
            OrderHandler handler = new OrderHandler();
            return Json(handler.GetOrdersDb(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateOrderStatus(int orderId, int orderStatus)
        {
            OrderHandler handler = new OrderHandler();
            bool orderUpdated = handler.UpdateOrderStatusDb(orderId, orderStatus);
            if (orderUpdated)
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}
