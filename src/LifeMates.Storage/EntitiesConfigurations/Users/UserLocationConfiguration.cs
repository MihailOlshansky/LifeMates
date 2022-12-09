using LifeMates.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeMates.Storage.EntitiesConfigurations.Users;

internal class UserLocationConfiguration : IEntityTypeConfiguration<UserLocation>
{
    public void Configure(EntityTypeBuilder<UserLocation> builder)
    {
        builder
            .ToTable(nameof(UserLocation));

        builder
            .HasKey(l => l.Id);

        builder
            .Property(l => l.Id)
            .UseIdentityColumn();

        builder
            .Property(l => l.Latitude)
            .IsRequired();
        
        builder
            .Property(l => l.Longitude)
            .IsRequired();

        builder
            .HasOne(l => l.User)
            .WithOne(u => u.Location)
            .HasForeignKey<UserLocation>(l => l.UserId);
    }
}