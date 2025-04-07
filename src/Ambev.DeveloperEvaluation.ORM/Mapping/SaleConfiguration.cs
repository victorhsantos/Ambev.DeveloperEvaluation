using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.PostgreSQL.Configurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(s => s.SaleNumber);

            builder.Property(s => s.SaleNumber)
                .IsRequired();

            builder.Property(s => s.SaleDate)
                .IsRequired();

            builder.Property(s => s.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.OwnsOne(s => s.Customer, customer =>
            {
                customer.Property(c => c.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                customer.Property(c => c.Email)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            builder.OwnsOne(s => s.PaymentAddress, address =>
            {
                address.Property(a => a.Street)
                    .IsRequired()
                    .HasMaxLength(200);

                address.Property(a => a.Number)
                    .IsRequired()
                    .HasMaxLength(10);

                address.Property(a => a.City)
                    .IsRequired()
                    .HasMaxLength(100);

                address.Property(a => a.State)
                    .IsRequired()
                    .HasMaxLength(50);

                address.Property(a => a.PostalCode)
                    .IsRequired()
                    .HasMaxLength(20);

                address.Property(a => a.Country)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            builder.HasMany(s => s.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
