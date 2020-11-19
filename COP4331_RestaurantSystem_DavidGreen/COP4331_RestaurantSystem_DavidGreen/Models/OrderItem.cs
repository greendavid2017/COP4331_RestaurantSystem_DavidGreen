using System;
using System.Collections.Generic;

namespace COP4331_RestaurantSystem_DavidGreen.Models
{
    public class OrderItem
    {
        public int ID { get; set; }

        public int MenuItemID { get; set; }

        public int OrderID { get; set; }

        public MenuItem MenuItem { get; set; }

        public Order Order { get; set; }
    }
}