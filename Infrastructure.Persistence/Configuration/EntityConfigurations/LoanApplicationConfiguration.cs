using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.EntityConfigurations
{
    public class LoanApplicationConfiguration : IEntityTypeConfiguration<LoanApplication>
    {
        public void Configure(EntityTypeBuilder<LoanApplication> builder)
        {
            builder.ToTable("LoanApplications");
            builder.HasKey(la => la.Id);

            builder.Property(la => la.Id)
                .IsRequired()
                .HasMaxLength(36);

            builder.Property(la => la.UserId)
                .IsRequired()
                .HasMaxLength(36);

            builder.Property(la => la.SubmitDate)
                .IsRequired();

            builder.Property(la => la.RequestedAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(la => la.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            builder.Property(la => la.ReasonForRejection)
                .HasMaxLength(500);

            // ارتباط با کاربر
            builder.HasOne(la => la.User)
                .WithMany(u => u.LoanApplications)
                .HasForeignKey(la => la.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ارتباط یک به یک با Loan
            builder.HasOne(la => la.Loan)
                .WithOne(l => l.LoanApplication)
                .HasForeignKey<Loan>(l => l.LoanApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}