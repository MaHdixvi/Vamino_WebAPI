using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.EntityConfigurations
{
    public class CreditScoreConfiguration : IEntityTypeConfiguration<CreditScore>
    {
        public void Configure(EntityTypeBuilder<CreditScore> builder)
        {
            builder.ToTable("CreditScores");  // نام جدول در دیتابیس

            builder.HasKey(cs => cs.Id);

            builder.Property(cs => cs.UserId)
                .IsRequired()
                .HasMaxLength(36);

            builder.Property(cs => cs.Score)
                .IsRequired();

            builder.Property(cs => cs.RiskLevel)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(cs => cs.EvaluationDate)
                .IsRequired();

            builder.Property(cs => cs.ReasonForScore)
                .HasMaxLength(500);

            builder.Property(cs => cs.EvaluatedBy)
                .HasMaxLength(100);
             builder.HasOne(cs => cs.User)
                .WithMany(u => u.CreditScores) 
               .HasForeignKey(cs => cs.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
