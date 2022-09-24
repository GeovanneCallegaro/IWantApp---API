using Flunt.Notifications;
using IWantApp.Domain.Orders;
using IWantApp.Domain.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IWantApp.Infra.Data;

public class ApplicationDBContext : IdentityDbContext<IdentityUser>
{

	public DbSet<Product> Products { get; set; }

	public DbSet<Category> Categories { get; set; }

	public DbSet<Order> Orders { get; set; }

	public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder builder) {
		base.OnModelCreating(builder);	
		
		builder.Ignore<Notification>();

		builder.Entity<Product>()
			.Property(p => p.Name).IsRequired();
		builder.Entity<Product>()
			.Property(p => p.Description).HasMaxLength(500);
		builder.Entity<Product>()
			.Property(p => p.Price).HasColumnType("decimal(10,2)").IsRequired();

		builder.Entity<Category>()
			.Property(c => c.Name).IsRequired();

		builder.Entity<Order>()
			.Property(o => o.ClientId).IsRequired();
		builder.Entity<Order>()
			.Property(o => o.DeliveryAddress).IsRequired();
		builder.Entity<Order>()
			.HasMany(o => o.Products)
			.WithMany(p => p.Orders)
			.UsingEntity(x => x.ToTable("OrderProducts"));
	}

	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		configurationBuilder.Properties<string>()
			.HaveMaxLength(100);
	}
}
