using LifeMates.Domain.Models.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeMates.Storage.EntitiesConfigurations.Chats;

public class ChatUserConfigurations : IEntityTypeConfiguration<ChatUser>
{
    public void Configure(EntityTypeBuilder<ChatUser> builder)
    {
        builder
            .ToTable(nameof(ChatUser));

        builder
            .HasKey(c => c.Id);
        
        builder
            .Property(c => c.Id)
            .UseIdentityColumn();

        builder
            .Property(c => c.IsSeen)
            .HasDefaultValue(false);

        builder
            .HasOne(cu => cu.User)
            .WithMany(c => c.Chats)
            .HasForeignKey(cu => cu.UserId);
        
        builder
            .HasOne(cu => cu.Chat)
            .WithMany(c => c.ChatUsers)
            .HasForeignKey(cu => cu.ChatId);
    }
}