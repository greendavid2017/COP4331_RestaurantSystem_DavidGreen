namespace COP4331_RestaurantSystem_DavidGreen.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Order
    {
        public Order()
        {
            MenuItems = new HashSet<MenuItem>();
        }

        public int ID { get; set; }

        public decimal Price { get; set; }

        public int Status { get; set; }

        public int UserID { get; set; }

        public DateTime Submitted { get; set; }

        public User User { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; }
    }
}
