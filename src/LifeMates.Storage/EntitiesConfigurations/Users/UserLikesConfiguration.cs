using LifeMates.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeMates.Storage.EntitiesConfigurations.Users;

public class UserLikesConfiguration : IEntityTypeConfiguration<UserLikes>
{
    public void Configure(EntityTypeBuilder<UserLikes> builder)
    {
        builder
            .ToTable(nameof(UserLikes));

        builder
            .HasKey(i => i.Id);
        
        builder
            .Property(i => i.Id)
            .UseIdentityColumn();

        builder
            .HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId);
    }
}

public class UserDislikesConfiguration : IEntityTypeConfiguration<UserDislikes>
{
    public void Configure(EntityTypeBuilder<UserDislikes> builder)
    {
        builder
            .ToTable(nameof(UserDislikes));

        builder
            .HasKey(i => i.Id);
        
        builder
            .Property(i => i.Id)
            .UseIdentityColumn();

        builder
            .HasOne(l => l.User)
            .WithMany(u => u.Dislikes)
            .HasForeignKey(l => l.UserId);
    }
}