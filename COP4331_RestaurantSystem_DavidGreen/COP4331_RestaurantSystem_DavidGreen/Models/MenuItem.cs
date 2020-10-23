namespace COP4331_RestaurantSystem_DavidGreen.Models
{
    using System.Collections.Generic;

    public partial class MenuItem
    {

        public MenuItem()
        {
            Orders = new HashSet<Order>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public int Category { get; set; }

        public decimal Price { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
