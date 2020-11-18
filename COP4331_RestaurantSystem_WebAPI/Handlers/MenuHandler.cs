using COP4331_RestaurantSystem_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COP4331_RestaurantSystem_WebAPI.Handlers
{
    public class MenuHandler
    {
        public List<Models.MenuItem> GetMenuItemsDb()
        {
            using (var db = new RestaurantSystemDataContext())
            {
                return db.MenuItems.ToList();
            }
        }

        public void CreateMenuItemDb(MenuItem item)
        {
            using (var db = new RestaurantSystemDataContext())
            {
                db.MenuItems.Add(item);
                db.SaveChanges();
            }
        }
    }
}