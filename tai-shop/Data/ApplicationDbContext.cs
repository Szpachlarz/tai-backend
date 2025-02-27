﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using tai_shop.Enums;
using tai_shop.Models;

namespace tai_shop.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {

        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Return> Returns { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ItemOrder> ItemOrders { get; set; }
        public DbSet<ItemReturn> ItemReturns { get; set; }
        public DbSet<ItemTag> ItemTags { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<CustomerComplaint> Complaints { get; set; }

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

            builder.Entity<AppUser>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId);

            builder.Entity<Return>()
                .HasOne(r => r.Order)
                .WithMany(o => o.Returns)
                .HasForeignKey(r => r.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ItemReturn>()
                .HasOne(ir => ir.ItemOrder)
                .WithMany()
                .HasForeignKey(ir => ir.ItemOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ItemReturn>()
                .HasOne(ri => ri.Return)
                .WithMany(r => r.ItemReturns)
                .HasForeignKey(ri => ri.ReturnId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

            builder.Entity<Order>()
                .Property(o => o.ShippingMethod)
                .HasConversion<string>();

            builder.Entity<Cart>()
                .Property(o => o.Status)
                .HasConversion<string>();

            builder.Entity<Return>()
                .Property(o => o.Status)
                .HasConversion<string>();

            builder.Entity<CustomerComplaint>()
                .Property(o => o.Status)
                .HasConversion<string>();
        }
    }
}
