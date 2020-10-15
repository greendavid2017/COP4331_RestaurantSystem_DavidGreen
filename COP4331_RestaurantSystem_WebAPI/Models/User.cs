namespace COP4331_RestaurantSystem_WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(512)]
        public string Password { get; set; }

        [Required]
        public bool IsEmployee { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
