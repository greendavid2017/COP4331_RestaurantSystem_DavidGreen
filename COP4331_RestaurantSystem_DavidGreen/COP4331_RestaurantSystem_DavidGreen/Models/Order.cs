namespace COP4331_RestaurantSystem_DavidGreen.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public int ID { get; set; }

        public decimal Price { get; set; }

        public int Status { get; set; }

        public int UserID { get; set; }

        public DateTime Submitted { get; set; }

        public User User { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public String StatusText
        {
            get
            {
                switch (Status)
                {
                    case 0:
                        return "Received Order";
                    case 1:
                        return "Preparing Order";
                    case 2:
                        return "Order Done";
                    case 3:
                        return "Picked Up";
                    default:
                        return "Invalid Status";
                }
            }
        }
    }
}
