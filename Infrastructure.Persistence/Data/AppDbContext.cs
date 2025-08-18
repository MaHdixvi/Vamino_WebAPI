using Domain.Entities;
using Infrastructure.Persistence.Configuration;
using Infrastructure.Persistence.Configuration.builderConfigurations;
using Infrastructure.Persistence.Configuration.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence.Data
{
    /// <summary>
    /// متن اصلی دیتابیس برای سیستم وامینو
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Installment> Installments { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<CreditScore> CreditScores { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<Recipient> Recipients { get; set; } 
        public DbSet<CurrencyInfo> Currencies { get; set; }



        private readonly DatabaseConfig _config;

        public AppDbContext(DbContextOptions<AppDbContext> options, IOptions<DatabaseConfig> config)
            : base(options)
        {
            _config = config.Value;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // اعمال کانفیگ‌های موجودیت‌ها
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new LoanApplicationConfiguration());
            modelBuilder.ApplyConfiguration(new LoanConfiguration());
            modelBuilder.ApplyConfiguration(new InstallmentConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionLogConfiguration());
            modelBuilder.ApplyConfiguration(new CreditScoreConfiguration());
            modelBuilder.ApplyConfiguration(new WalletConfiguration());
            modelBuilder.ApplyConfiguration(new WalletTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new RecipientConfiguration());

            // تنظیمات کلی
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName != null)
                {
                    entityType.SetTableName(tableName);
                }
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}