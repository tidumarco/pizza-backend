using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Data;

public class PizzaContext : DbContext
{
    public DbSet<Pizza> Pizzas { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Sauce> Sauces { get; set; }
    public DbSet<Topping> Toppings { get; set; }
    public DbSet<PizzaOrder> PizzaOrders { get; set; }

    public PizzaContext(DbContextOptions<PizzaContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PizzaOrder>()
            .HasKey(po => new { po.PizzaId, po.OrderId });

        modelBuilder.Entity<PizzaOrder>()
            .HasOne(po => po.Pizza)
            .WithMany(p => p.PizzaOrders)
            .HasForeignKey(po => po.PizzaId);

        modelBuilder.Entity<PizzaOrder>()
            .HasOne(po => po.Order)
            .WithMany(o => o.PizzaOrders)
            .HasForeignKey(po => po.OrderId);

        modelBuilder.Entity<Pizza>()
            .HasOne(p => p.Sauce)
            .WithMany()
            .HasForeignKey(p => p.SauceId);

        modelBuilder.Entity<Pizza>()
            .HasMany(p => p.Toppings)
            .WithMany(t => t.Pizzas);
    }
}