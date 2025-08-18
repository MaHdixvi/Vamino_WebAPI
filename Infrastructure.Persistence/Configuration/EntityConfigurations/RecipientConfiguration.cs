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
    public class RecipientConfiguration : IEntityTypeConfiguration<Recipient>
    {
        public void Configure(EntityTypeBuilder<Recipient> builder)
        {
            // نام جدول (اختیاری)
            builder.ToTable("Recipients");

            // کلید اصلی
            builder.HasKey(r => r.Id);

            // تنظیم طول و الزامی بودن فیلدها
            builder.Property(r => r.Id)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(r => r.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(r => r.Mobile)
                   .HasMaxLength(20);

            builder.Property(r => r.Email)
                   .HasMaxLength(100);

            // می‌تونی ایندکس‌ها یا محدودیت‌های یکتا هم اضافه کنی
            builder.HasIndex(r => r.Email).IsUnique();
            builder.HasIndex(r => r.Mobile).IsUnique();
        }
    }
}
