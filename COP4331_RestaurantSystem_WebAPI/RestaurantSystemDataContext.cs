namespace COP4331_RestaurantSystem_WebAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RestaurantSystemDataContext : DbContext
    {
        public RestaurantSystemDataContext()
            : base("name=RestaurantSystemDataContext")
        {
            base.Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<MenuItem> MenuItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Order>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany<MenuItem>(e => e.MenuItems)
                .WithMany(e => e.Orders)
                .Map(e =>
                {
                    e.MapLeftKey("OrderID");
                    e.MapRightKey("MenuItemID");
                    e.ToTable("OrderItems");
                });
        }
    }
}
