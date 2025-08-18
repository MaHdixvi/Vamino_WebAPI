using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configuration.EntityConfigurations
{
    public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
    {
        public void Configure(EntityTypeBuilder<WalletTransaction> builder)
        {
            // نام جدول
            builder.ToTable("WalletTransactions");

            // کلید اصلی
            builder.HasKey(wt => wt.TransactionId);

            // ویژگی‌ها
            builder.Property(wt => wt.TransactionId)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(wt => wt.UserId)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(wt => wt.Type)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(wt => wt.Amount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(wt => wt.BalanceAfter)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(wt => wt.TransactionDate)
                   .HasColumnType("datetime2")
                   .IsRequired();

            builder.Property(wt => wt.Status)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(wt => wt.Description)
                   .HasMaxLength(500);

            builder.Property(wt => wt.ReferenceTransactionId)
                   .HasMaxLength(50);

            builder.Property(wt => wt.TrackingCode)
                   .HasMaxLength(100);

            builder.Property(wt => wt.WalletId)
                   .HasMaxLength(50)
                   .IsRequired();

            // ارتباط با Wallet (کلید خارجی)
            builder.HasOne(wt => wt.Wallet)
                   .WithMany(w => w.Transactions)
                   .HasForeignKey(wt => wt.WalletId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ایندکس‌ها
            builder.HasIndex(wt => wt.UserId);
            builder.HasIndex(wt => wt.TrackingCode).IsUnique(false);
        }
    }
}
