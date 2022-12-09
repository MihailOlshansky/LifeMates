using LifeMates.Domain.Models.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeMates.Storage.EntitiesConfigurations.Chats;

public class RequestHistoryConfigurations : IEntityTypeConfiguration<RequestHistory>
{
    public void Configure(EntityTypeBuilder<RequestHistory> builder)
    {
        builder.ToTable(nameof(RequestHistory));
        
        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .UseIdentityColumn();

        builder
            .HasOne(x => x.User)
            .WithOne(x => x.RequestHistory)
            .HasForeignKey<RequestHistory>(x => x.UserId);
    }
}