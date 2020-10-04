using Blazor.Learner.Server.Models;
using Blazor.Learner.Shared.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blazor.Learner.Server.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Balance>()
            .HasIndex(p => new { p.BalanceDate, p.AccountNumber }).IsUnique();

            modelBuilder.Entity<Balance>().HasOne(a => a.Account).WithMany(b => b.Balances)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<BalanceTransaction>()
        .HasKey(bc => new { bc.BalanceId, bc.TransactionId });

            modelBuilder.Entity<BalanceTransaction>()
            .HasOne<Balance>(s => s.Balance)
            .WithMany().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BalanceTransaction>()
           .HasOne<Transaction>(s => s.Transaction)
           .WithMany().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BalanceTransaction>()
                .HasOne(bc => bc.Transaction)
                .WithMany(b => b.BalanceTransactions).OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(bc => bc.TransactionId);

            modelBuilder.Entity<BalanceTransaction>()
                .HasOne(bc => bc.Balance)
                .WithMany(c => c.BalanceTransactions).OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(bc => bc.BalanceId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Developer> Developers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<BalanceTransaction> BalanceTransactions { get; set; }

    }
}
