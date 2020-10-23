namespace COP4331_RestaurantSystem_DavidGreen.Models
{
    using System.Collections.Generic;

    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int ID { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsEmployee { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
