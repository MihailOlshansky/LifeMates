using LifeMates.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeMates.Storage.EntitiesConfigurations.Users;

internal class UserImageConfiguration : IEntityTypeConfiguration<UserImage>
{
    public void Configure(EntityTypeBuilder<UserImage> builder)
    {
        builder
            .ToTable(nameof(UserImage));

        builder
            .HasKey(i => i.Id);
        
        builder
            .Property(i => i.Id)
            .UseIdentityColumn();

        builder
            .Property(i => i.Url)
            .IsRequired();
        
        builder
            .Property(i => i.UserId)
            .IsRequired();

        builder
            .HasOne(i => i.User)
            .WithMany(u => u.Images)
            .HasForeignKey(i => i.UserId);
    }
}
