using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.EntityConfigurations
{
    /// <summary>
    /// کانفیگ مدل برای موجودیت TransactionLog
    /// </summary>
    public class TransactionLogConfiguration : IEntityTypeConfiguration<TransactionLog>
    {
        public void Configure(EntityTypeBuilder<TransactionLog> builder)
        {
            builder.ToTable("TransactionLogs");
            builder.HasKey(tl => tl.Id);

            builder.Property(tl => tl.Id)
                .IsRequired()
                .HasMaxLength(36);

            builder.Property(tl => tl.Timestamp)
                .IsRequired();

            builder.Property(tl => tl.Action)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(tl => tl.RelatedEntity)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(tl => tl.Details)
                .HasMaxLength(500);

            // ارتباط با User (اختیاری)
            builder.HasOne(tl => tl.User)
                .WithMany(u => u.TransactionLogs)
                .HasForeignKey(tl => tl.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}