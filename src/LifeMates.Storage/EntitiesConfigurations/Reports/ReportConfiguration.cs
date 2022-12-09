using LifeMates.Domain.Models.Reports;
using LifeMates.Domain.Shared.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeMates.Storage.EntitiesConfigurations.Reports;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.ToTable(nameof(Report));
        
        builder
            .HasKey(u => u.Id);
        
        builder
            .Property(u => u.Id)
            .UseIdentityColumn();
        
        builder
            .Property(u => u.Type)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<ReportType>(v))
            .IsRequired();
    }
}