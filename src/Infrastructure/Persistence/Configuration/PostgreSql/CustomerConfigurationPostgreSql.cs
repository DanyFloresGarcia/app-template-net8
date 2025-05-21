
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Domain.Customers;

namespace Infrastructure.Persistence.Configuration.PostgreSql;

public class CustomerConfigurationPostgreSql : IEntityTypeConfiguration<Customer>{
    public void Configure(EntityTypeBuilder<Customer> builder){
        builder.ToTable("Customer", "public");
        builder.HasKey(c=> c.Id);
        builder.HasIndex(e => e.Id);
        builder.Property(d => d.Id).HasColumnName("IdCustomer")
           .ValueGeneratedOnAdd();

        builder.Property(c=> c.Name).HasMaxLength(50).IsRequired().HasColumnName("Name");
        builder.Property(c=> c.LastName).HasMaxLength(50).IsRequired().HasColumnName("LastName");
        builder.Property(c=> c.Address).HasMaxLength(200).IsRequired().HasColumnName("Address");

        builder.HasIndex(c=> c.Email).IsUnique();
        builder.Property(c=> c.Email).HasMaxLength(200).IsRequired().HasColumnName("Email");

        builder.OwnsOne(c => c.AuditRecord, auditRecordBuilder =>
        {
            AuditRecordConfiguration.ConfigureAuditRecord(auditRecordBuilder);
        });
    }
}