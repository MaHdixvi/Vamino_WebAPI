using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.EntityConfigurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.ToTable("Loans");
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Id)
                .IsRequired()
                .HasMaxLength(36);

            builder.Property(l => l.LoanApplicationId)
                .IsRequired()
                .HasMaxLength(36);

            builder.Property(l => l.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(l => l.DisbursementDate)
                .IsRequired();

            builder.Property(l => l.DueDate)
                .IsRequired();

            builder.Property(l => l.Status)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(l => l.InterestRate)
                .HasMaxLength(10);

            builder.Property(l => l.UserId)
                .IsRequired()
                .HasMaxLength(36);

            // ارتباط با کاربر
            builder.HasOne(l => l.User)
                .WithMany(u => u.Loans)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ارتباط یک به یک با LoanApplication
            builder.HasOne(l => l.LoanApplication)
                .WithOne(la => la.Loan)
                .HasForeignKey<Loan>(l => l.LoanApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            // ارتباط با Installment
            builder.HasMany(l => l.Installments)
                .WithOne(i => i.Loan)
                .HasForeignKey(i => i.LoanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}