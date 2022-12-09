using LifeMates.Domain.Models.Users;
using LifeMates.Domain.Shared.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeMates.Storage.EntitiesConfigurations.Users;

internal class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable(nameof(User));

        builder
            .HasKey(u => u.Id);
        
        builder
            .Property(u => u.Id)
            .UseIdentityColumn();

        builder
            .Property(u => u.Name)
            .IsRequired();
        
        builder
            .Property(u => u.Gender)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<UserGender>(v))
            .IsRequired();
        
        builder
            .Property(u => u.Status)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<UserStatus>(v))
            .IsRequired();

        builder
            .Property(u => u.CreatedAt)
            .IsRequired();
        
        builder
            .HasOne(u => u.Location)
            .WithOne(l => l.User);
        
        builder
            .HasOne(u => u.Settings)
            .WithOne(s => s.User);

        builder
            .HasMany(u => u.Images)
            .WithOne(i => i.User);
        
        builder
            .HasMany(u => u.Contacts)
            .WithOne(i => i.User);

        builder
            .HasMany(u => u.Interests)
            .WithOne(i => i.User);
            
        builder
            .HasMany(u => u.Likes)
            .WithOne(i => i.User);
        
        builder
            .HasMany(u => u.Dislikes)
            .WithOne(i => i.User);
        
        builder
            .HasMany(u => u.Chats)
            .WithOne(i => i.User);
    }
}