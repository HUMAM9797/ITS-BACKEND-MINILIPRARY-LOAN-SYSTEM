using LoanSystem.Domain.Entities;
using LoanSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoanSystem.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Global Query Filters for Soft Delete
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Book>().HasQueryFilter(b => !b.IsDeleted);
        }

        public override int SaveChanges()
        {
            ApplyAuditInfo();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInfo()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditable || e.Entity is ISoftDeletable);

            foreach (var entry in entries)
            {
                if (entry.Entity is IAuditable auditable && (entry.State == EntityState.Added || entry.State == EntityState.Modified))
                {
                    auditable.UpdatedAt = DateTime.UtcNow;
                    if (entry.State == EntityState.Modified && string.IsNullOrEmpty(auditable.Changes))
                    {
                        auditable.Changes = "Modified"; 
                    }
                }

                if (entry.Entity is ISoftDeletable softDeletable && entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    softDeletable.IsDeleted = true;
                    softDeletable.DeletedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
