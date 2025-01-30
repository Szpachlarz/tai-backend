using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using tai_shop.Models;

namespace tai_shop.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
        : base(dbContextOptions)
        {

        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Return> Returns { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public Review Reviews { get; set; }
        public ItemOrder ItemOrders { get; set; }
        public ItemReturn ItemReturns { get; set; }
        public ItemTag ItemTags { get; set; }
        public Photo Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ItemOrder>()
                .HasOne(b => b.Order)
                .WithMany(ba => ba.ItemOrders)
                .HasForeignKey(bi => bi.OrderId);

            builder.Entity<ItemReturn>()
                .HasOne(b => b.Return)
                .WithMany(ba => ba.ItemReturns)
                .HasForeignKey(bi => bi.ReturnId);

            builder.Entity<ItemTag>()
                .HasOne(b => b.Tag)
                .WithMany(ba => ba.ItemTags)
                .HasForeignKey(bi => bi.TagId);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
