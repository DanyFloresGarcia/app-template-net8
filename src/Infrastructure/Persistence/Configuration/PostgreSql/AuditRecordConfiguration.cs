using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Domain.ValueObjects;

namespace Infrastructure.Persistence.Configuration.PostgreSql;

public static class AuditRecordConfiguration
{
    public static void ConfigureAuditRecord<T>(OwnedNavigationBuilder<T, AuditRecord> auditRecordBuilder) where T : class
    {
        var dateTimeToUtcConverter = new ValueConverter<DateTime, DateTime>(
            v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
        );
        
        auditRecordBuilder.Property(a => a.Asset).IsRequired().HasColumnName("Asset");
        auditRecordBuilder.Property(a => a.UserCreator).IsRequired().HasMaxLength(30).HasColumnName("UserCreator");
        auditRecordBuilder.Property(a => a.UserUpdater).IsRequired().HasMaxLength(30).HasColumnName("UserUpdater");
        auditRecordBuilder.Property(a => a.HostCreator).IsRequired().HasMaxLength(100).HasColumnName("HostCreator");
        auditRecordBuilder.Property(a => a.HostUpdater).IsRequired().HasMaxLength(100).HasColumnName("HostUpdater");
        auditRecordBuilder.Property(a => a.DateUpdate).IsRequired().HasColumnName("DateUpdate");
        auditRecordBuilder.Property(a => a.AppCreator).IsRequired().HasMaxLength(50).HasColumnName("AppCreator");
        auditRecordBuilder.Property(a => a.AppUpdater).IsRequired().HasMaxLength(50).HasColumnName("AppUpdater");

        auditRecordBuilder.Property(a => a.DateCreated)
          .IsRequired()
          .HasColumnName("DateCreated")
          .HasConversion(dateTimeToUtcConverter);
        auditRecordBuilder.Property(a => a.DateUpdate)
          .IsRequired()
          .HasColumnName("DateUpdate")
          .HasConversion(dateTimeToUtcConverter);
    }
}