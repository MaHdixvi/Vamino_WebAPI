using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.builderConfigurations
{
    /// <summary>
    /// کانفیگ مدل برای موجودیت TransactionLog
    /// </summary>
    public class TransactionLogConfiguration : IEntityTypeConfiguration<TransactionLog>
    {
        public void Configure(EntityTypeBuilder<TransactionLog> builder)
        {
            // نام جدول در دیتابیس
            builder.ToTable("TransactionLogs");

            // تنظیمات فیلدها
            builder.Property(e => e.Timestamp)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()"); // مقدار پیش‌فرض زمان فعلی

            builder.Property(e => e.Action)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.RelatedEntity)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.EntityId)
                .HasMaxLength(100);

            builder.Property(e => e.Details)
                .HasMaxLength(500);

            builder.Property(e => e.UserId)
               .IsRequired(false); // حتماً اضافه شود

            builder.Property(e => e.TrackingCode)
                .HasMaxLength(50);

            builder.Property(e => e.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.PaymentMethod)
                .HasMaxLength(50);

            builder.Property(e => e.IPAddress)
                .HasMaxLength(20);

            // تنظیمات رابطه با کاربر
            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull); // در صورت حذف کاربر، مقدار null شود

            // ایندکس‌گذاری برای بهبود کارایی
            builder.HasIndex(e => e.Timestamp);
            builder.HasIndex(e => e.Action);
            builder.HasIndex(e => new { e.RelatedEntity, e.EntityId });
            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => e.TrackingCode).IsUnique();
        }
     }
}
