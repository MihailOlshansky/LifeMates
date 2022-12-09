using LifeMates.Domain.Models.Interests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeMates.Storage.EntitiesConfigurations.Interests;

internal class InterestConfiguration : IEntityTypeConfiguration<Interest>
{
    public void Configure(EntityTypeBuilder<Interest> builder)
    {
        builder
            .ToTable(nameof(Interest));

        builder
            .HasKey(i => i.Id);
        
        builder
            .Property(i => i.Id)
            .UseIdentityColumn();

        builder
            .Property(i => i.Value)
            .IsRequired();

        builder
            .HasMany(i => i.Users)
            .WithOne(u => u.Interest);
    }
}