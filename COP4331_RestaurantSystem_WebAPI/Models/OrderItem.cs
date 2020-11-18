using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace COP4331_RestaurantSystem_WebAPI.Models
{
    public class OrderItem
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("MenuItem")]
        public int MenuItemID { get; set; }

        [ForeignKey("Order")]
        public int OrderID { get; set; }

        public virtual MenuItem MenuItem { get; set; }

        public virtual Order Order { get; set; }
    }
}