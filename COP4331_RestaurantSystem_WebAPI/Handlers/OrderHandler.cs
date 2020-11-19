using COP4331_RestaurantSystem_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace COP4331_RestaurantSystem_WebAPI.Handlers
{
    public class OrderHandler
    {
        public List<Order> GetUserOrdersDb(String email)
        {
            using (var db = new RestaurantSystemDataContext())
            {
                var id = db.Users.Where(u => u.Email == email).FirstOrDefault().ID;
                return db.Orders.Where(o => o.UserID == id).Include(o => o.OrderItems).Include(o => o.OrderItems.Select(i => i.MenuItem)).ToList().Select(o => { o.User = null; o.OrderItems = null; return o; }).ToList();
            }
        }

        public List<Order> GetOrdersDb()
        {
            using (var db = new RestaurantSystemDataContext())
            {
                return db.Orders.Include(o => o.User).Include(o => o.OrderItems).Include(o => o.OrderItems.Select(i => i.MenuItem)).ToList();
            }
        }

        public bool UpdateOrderStatusDb(int orderId, int orderStatus)
        {
            using (var db = new RestaurantSystemDataContext())
            {
                var order = db.Orders.Where(o => o.ID == orderId).FirstOrDefault();
                if (order == null)
                    return false;
                order.Status = orderStatus;
                db.SaveChanges();
                return true;
            }
        }

        public bool CreateOrderDb(String email, List<MenuItem> orderItems)
        {
            using(var db = new RestaurantSystemDataContext())
            {
                var user = db.Users.Where(u => u.Email == email).FirstOrDefault();

                if (user == null)
                    return false;

                decimal totalPrice = 0;
                foreach(MenuItem item in orderItems)
                {
                    totalPrice += item.Price;
                }

                totalPrice *= (decimal)1.06;

                Math.Round(totalPrice, 2);

                Order newOrder = new Order()
                {
                    Price = totalPrice,
                    Status = 0,
                    UserID = user.ID,
                    Submitted = DateTime.Now
                };

                db.Orders.Add(newOrder);

                foreach(MenuItem item in orderItems)
                {
                    OrderItem orderItem = new OrderItem()
                    {
                        MenuItemID = item.ID,
                        Order = newOrder
                    };
                    db.OrderItems.Add(orderItem);
                }

                db.SaveChanges();
                return true;

            }
        }
    }
}