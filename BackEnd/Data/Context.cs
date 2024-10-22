using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Data
{
    public class Context : DbContext
    {
        private IHttpContextAccessor _httpContextAccessor;
        public Context(DbContextOptions<Context> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            SavingChanges += UpdateLogs;
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            DbSeed.Run(builder);

            builder.Entity<Motorcycle>().HasKey(x => x.Id);
            builder.Entity<Motorcycle>().HasMany(x => x.Rentals).WithOne().HasForeignKey(x => x.MotorcycleId);

            builder.Entity<User>().HasKey(x => x.Id);
            builder.Entity<User>().HasMany(x => x.Rentals).WithOne().HasForeignKey(x => x.UserId);

            builder.Entity<RentalPlan>().HasKey(x => x.Days);
            builder.Entity<RentalPlan>().HasMany(x => x.Rentals).WithOne(x => x.RentalPlan).HasForeignKey(x => x.RentalPlanId);

            builder.Entity<Rental>().HasKey(x => x.Id);
            builder.Entity<Rental>().HasOne(x => x.RentalPlan).WithMany(x => x.Rentals).HasForeignKey(x => x.RentalPlanId);
        }

        private void UpdateLogs(object? sender, SavingChangesEventArgs e)
        {
            string id = GetUserId(_httpContextAccessor);
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                string table = entry.Metadata.GetTableName();
                if (table == "Notifications")
                {
                    continue;
                }

                entry.CurrentValues.TryGetValue("Version", out int v);
                entry.CurrentValues["Version"] = v + 1;
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["CreatedById"] = id;
                        entry.CurrentValues["CreatedDate"] = DateTime.UtcNow;
                        entry.CurrentValues["Deleted"] = null;
                        break;
                    case EntityState.Modified:
                        entry.CurrentValues["LastModifiedById"] = id;
                        entry.CurrentValues["LastModifiedDate"] = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["LastModifiedById"] = id;
                        entry.CurrentValues["LastModifiedDate"] = DateTime.UtcNow;
                        entry.CurrentValues["Deleted"] = DateTime.UtcNow;
                        break;
                }
            }
        }
        
        public static string GetUserId(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return string.Empty;
            }

            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim?.Value ?? string.Empty;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalPlan> RentalPlans { get; set; }
        public DbSet<Notification> Notifications { get; set; }

    }
}
