using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Infrastructure.Context
{
    public class EstatesConfig : IEntityTypeConfiguration<Estates>
    {
        public void Configure(EntityTypeBuilder<Estates> builder)
        {
            builder.ToTable("Estates");

            builder.HasKey(e => e.Id);

            // سایر تنظیمات مربوط به Estates
            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(50);

         
        }
    }

    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.Id);

            // سایر تنظیمات مربوط به Category
        }
    }
}
