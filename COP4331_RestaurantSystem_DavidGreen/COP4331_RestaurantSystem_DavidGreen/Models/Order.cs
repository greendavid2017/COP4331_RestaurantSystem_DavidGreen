namespace COP4331_RestaurantSystem_WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [Key]
        public int ID { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Required]
        public int Status { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [Required]
        public DateTime Submitted { get; set; }

        public virtual User User { get; set; }
    }
}
