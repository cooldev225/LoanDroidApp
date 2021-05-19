// =============================
// Email: bluestar1027@hotmail.com

// =============================
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Models.data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.data.Interfaces;
using LoanDroidApp.Models;

namespace DBSetup
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public string CurrentUserId { get; set; }
        public DbSet<ApplicationPage> ApplicationPage { get; set; }
        public DbSet<AccountPayment> AccountPayment { get; set; }
        public DbSet<LoanRequest> LoanRequest { get; set; }
        public DbSet<LoanRequestStatus> LoanRequestStatus { get; set; }
        public DbSet<Investment> Investment { get; set; }
        public DbSet<InvestmentStatus> InvestmentStatus { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<NotificationReading> NotificationReading { get; set; }
        public DbSet<TransactionHistory> TransactionHistory { get; set; }
        public DbSet<LoanInterestPayment> LoanInterestPayment { get; set; }
        public DbSet<Nationality> Nationality { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Option> Option { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().HasMany(u => u.Claims).WithOne().HasForeignKey(c => c.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationUser>().HasMany(u => u.Roles).WithOne().HasForeignKey(r => r.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationRole>().HasMany(r => r.Claims).WithOne().HasForeignKey(c => c.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationRole>().HasMany(r => r.Users).WithOne().HasForeignKey(r => r.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationPage>().HasIndex(p => p.Id);
            builder.Entity<ApplicationPage>().Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<AccountPayment>().HasIndex(p => p.Id);
            builder.Entity<AccountPayment>().Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<LoanRequest>().HasIndex(p => p.Id);
            builder.Entity<LoanRequest>().Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<LoanRequestStatus>().HasIndex(p => p.Id);
            builder.Entity<LoanRequestStatus>().Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<Investment>().HasIndex(p => p.Id);
            builder.Entity<Investment>().Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<InvestmentStatus>().HasIndex(p => p.Id);
            builder.Entity<InvestmentStatus>().Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<Notification>().HasIndex(p => p.Id);
            builder.Entity<Notification>().Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<NotificationReading>().HasIndex(p => p.Id);
            builder.Entity<NotificationReading>().Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<TransactionHistory>().HasIndex(p => p.Id);
            builder.Entity<TransactionHistory>().Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<LoanInterestPayment>().HasIndex(p => p.Id);
            builder.Entity<LoanInterestPayment>().Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<Nationality>().HasIndex(p => p.Id);
            builder.Entity<Nationality>().Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<Province>().HasIndex(p => p.Id);
            builder.Entity<Province>().Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<Company>().HasIndex(p => p.Id);
            builder.Entity<Company>().Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<Option>().HasKey(u => u.Key);
        }
        public override int SaveChanges()
        {
            UpdateAuditEntities();
            return base.SaveChanges();
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        private void UpdateAuditEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));


            foreach (var entry in modifiedEntries)
            {
                var entity = (IAuditableEntity)entry.Entity;
                DateTime now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = CurrentUserId;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }

                entity.UpdatedDate = now;
                entity.UpdatedBy = CurrentUserId;
            }
        }
    }
}
