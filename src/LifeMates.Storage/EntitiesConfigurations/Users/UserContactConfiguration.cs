using LifeMates.Domain.Models.Users;
using LifeMates.Domain.Shared.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeMates.Storage.EntitiesConfigurations.Users;

internal class UserContactConfigurations : IEntityTypeConfiguration<UserContact>
{
    public void Configure(EntityTypeBuilder<UserContact> builder)
    {
        builder
            .ToTable(nameof(UserContact));

        builder
            .HasKey(c => c.Id);
        
        builder
            .Property(c => c.Id)
            .UseIdentityColumn();
        
        builder
            .Property(c => c.UserId)
            .IsRequired();
        
        builder
            .Property(c => c.Type)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<ContactType>(v))
            .IsRequired();
        
        builder
            .Property(c => c.Value)
            .IsRequired();

        builder
            .HasOne(c => c.User)
            .WithMany(u => u.Contacts)
            .HasForeignKey(c => c.UserId);
    }
}