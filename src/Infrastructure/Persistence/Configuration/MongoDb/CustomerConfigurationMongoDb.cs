using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Domain.Customers;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Infrastructure.Persistence.Configuration.MongoDb;

public class CustomerConfigurationMongoDb : IEntityTypeConfiguration<Customer>{
    public void Configure(EntityTypeBuilder<Customer> builder){
        builder.ToCollection("Customer");
        builder.HasKey(c=> c.Id);
        builder.HasIndex(e => e.Id);
        builder.Property(d => d.Id).HasColumnName("_id");

        builder.Property(c=> c.Name).HasMaxLength(100).IsRequired().HasColumnName("Name");
        builder.Property(c=> c.LastName).HasMaxLength(150).IsRequired().HasColumnName("LastName");

        builder.HasIndex(c=> c.Email).IsUnique();
        builder.Property(c=> c.Email).HasMaxLength(100).IsRequired().HasColumnName("Email");

        builder.OwnsOne(c => c.AuditRecord, auditRecordBuilder =>
        {
            AuditRecordConfiguration.ConfigureAuditRecord(auditRecordBuilder);
        });
    }
}