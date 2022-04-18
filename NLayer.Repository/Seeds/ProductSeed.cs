using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Seeds
{
    internal class ProductSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product() { Id = 1, Name = "Pilot Kalem", CategoryId = 1, Price = 100, Stock = 10 },
                new Product() { Id = 2, Name = "Matematik Kitabı", CategoryId = 2, Price = 100, Stock = 5 },
                new Product() { Id = 3, Name = "Dolma Kalem", CategoryId = 1, Price = 200, Stock = 20 },
                new Product() { Id = 4, Name = "Türkçe Kitabı", CategoryId = 2, Price = 150, Stock = 100 },
                new Product() { Id = 5, Name = "Düz Defter", CategoryId = 3, Price = 100, Stock = 50 },
                new Product() { Id = 6, Name = "Çizgili Defter ", CategoryId = 3, Price = 80, Stock = 250 },
                new Product() { Id = 7, Name = "Fen Kitabı", CategoryId = 2, Price = 300, Stock = 230 }
                );
        }
    }
}
