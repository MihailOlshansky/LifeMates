using LifeMates.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeMates.Storage.EntitiesConfigurations.Users;

internal class UserInterestConfiguration : IEntityTypeConfiguration<UserInterest>
{
    public void Configure(EntityTypeBuilder<UserInterest> builder)
    {
        builder
            .ToTable(nameof(UserInterest));

        builder
            .HasKey(i => i.Id);
        
        builder
            .Property(i => i.Id)
            .UseIdentityColumn();

        builder
            .Property(i => i.UserId)
            .IsRequired();
        
        builder
            .Property(i => i.InterestId)
            .IsRequired();
        
        builder
            .HasOne(i => i.User)
            .WithMany(u => u.Interests)
            .HasForeignKey(i => i.UserId);
        
        builder
            .HasOne(i => i.Interest)
            .WithMany(u => u.Users)
            .HasForeignKey(i => i.InterestId);
    }
}