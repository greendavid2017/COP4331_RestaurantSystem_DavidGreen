using COP4331_RestaurantSystem_WebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
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

        // GET: RestaurantSystem/GetAllMenuItems
        public ActionResult GetMenuItems()
        {
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
