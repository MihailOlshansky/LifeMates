using LifeMates.Domain.Models.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeMates.Storage.EntitiesConfigurations.Chats;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable(nameof(Message));
        
        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .UseIdentityColumn();

        builder
            .Property(x => x.Content)
            .IsRequired();

        builder
            .HasOne(x => x.Chat)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ChatId);
        
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.UserId);
    }
}