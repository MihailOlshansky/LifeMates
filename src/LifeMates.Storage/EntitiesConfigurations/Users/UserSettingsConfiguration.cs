using LifeMates.Domain.Models.Users;
using LifeMates.Domain.Shared.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeMates.Storage.EntitiesConfigurations.Users;

public class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettings>
{
    public void Configure(EntityTypeBuilder<UserSettings> builder)
    {
        builder
            .ToTable(nameof(UserSettings));

        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .UseIdentityColumn();
        
        builder
            .Property(x => x.ShowingGender)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<UserGender>(v))
            .IsRequired();

        builder
            .HasOne(s => s.User)
            .WithOne(s => s.Settings)
            .HasForeignKey<UserSettings>(s => s.UserId);
    }
}