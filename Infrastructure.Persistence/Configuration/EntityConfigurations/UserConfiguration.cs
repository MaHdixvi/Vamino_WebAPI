using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.EntityConfigurations
{
    /// <summary>
    /// کانفیگ مدل برای موجودیت User
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .IsRequired()
                .HasMaxLength(36);

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.NationalId)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(u => u.PhoneNumber)
                .IsRequired()
                .HasMaxLength(11);

            builder.Property(u => u.Email)
                .HasMaxLength(150);

            builder.Property(u => u.BankAccountNumber)
                .HasMaxLength(26);

            // ارتباط یک به چند با Loan
            builder.HasMany(u => u.Loans)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ارتباط یک به چند با LoanApplication
            builder.HasMany(u => u.LoanApplications)
                .WithOne(la => la.User)
                .HasForeignKey(la => la.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ارتباط یک به چند با TransactionLog
            builder.HasMany(u => u.TransactionLogs)
                .WithOne(tl => tl.User)
                .HasForeignKey(tl => tl.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // ارتباط یک به چند با CreditScore
            builder.HasMany(u => u.CreditScores)
                .WithOne(cs => cs.User)
                .HasForeignKey(cs => cs.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}