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
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallets");

            builder.HasKey(w => w.UserId);

            builder.Property(w => w.UserId)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(w => w.Balance)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(w => w.LastUpdated)
                   .HasColumnType("datetime2")
                   .HasDefaultValueSql("GETUTCDATE()")
                   .ValueGeneratedOnAddOrUpdate()
                   .IsRequired();

            builder.HasIndex(w => w.UserId)
                   .IsUnique();
        }
    }
}
