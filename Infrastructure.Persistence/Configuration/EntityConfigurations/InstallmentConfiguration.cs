using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.EntityConfigurations
{
    /// <summary>
    /// کانفیگ مدل برای موجودیت Installment
    /// </summary>
    public class InstallmentConfiguration : IEntityTypeConfiguration<Installment>
    {
        public void Configure(EntityTypeBuilder<Installment> builder)
        {
            builder.ToTable("Installments");
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .IsRequired()
                .HasMaxLength(36);

            builder.Property(i => i.LoanId)
                .IsRequired()
                .HasMaxLength(36); // ✅ باید 36 کاراکتر باشد

            builder.Property(i => i.Number)
                .IsRequired();

            builder.Property(i => i.DueDate)
                .IsRequired();

            builder.Property(i => i.PrincipalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.InterestAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            builder.Property(i => i.PaymentDate)
                .IsRequired(false);

            // ارتباط با Loan
            builder.HasOne(i => i.Loan)
                .WithMany(l => l.Installments)
                .HasForeignKey(i => i.LoanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}