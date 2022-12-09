using LifeMates.Domain.Models.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeMates.Storage.EntitiesConfigurations.Chats;

public class ChatConfigurations : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder
            .ToTable(nameof(Chat));

        builder
            .HasKey(c => c.Id);
        
        builder
            .Property(c => c.Id)
            .UseIdentityColumn();
        
        builder
            .Property(c => c.CreatedAt)
            .HasDefaultValue(DateTime.UtcNow);

        builder
            .HasMany(c => c.ChatUsers)
            .WithOne(cu => cu.Chat);
    }
}