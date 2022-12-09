using LifeMates.Domain.Models.Interests;
using LifeMates.Domain.Models.Users; 
using LifeMates.Domain.Models.Chats;
using LifeMates.Domain.Models.Reports;
using LifeMates.Storage.EntitiesConfigurations.Chats;
using LifeMates.Storage.EntitiesConfigurations.Interests;
using LifeMates.Storage.EntitiesConfigurations.Reports;
using LifeMates.Storage.EntitiesConfigurations.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LifeMates.Storage;

// Add-migration MigrationName (from package console for this project)
public sealed class LifematesDbContext : IdentityDbContext<UserCredentials>
{
    internal DbSet<User> Users { get; set; }
    internal DbSet<UserLikes> UsersLikes { get; set; }
    internal DbSet<UserDislikes> UserDislikes { get; set; }
    internal DbSet<UserLocation> UserLocations { get; set; }
    internal DbSet<UserSettings> UserSettings { get; set; }
    internal DbSet<UserCredentials> UsersCredentials { get; set; }
    internal DbSet<UserInterest> UsersInterests { get; set; }
    internal DbSet<Interest> Interests { get; set; }
    internal DbSet<Chat> Chats { get; set; }
    internal DbSet<ChatUser> ChatUsers { get; set; }
    internal DbSet<Report> Reports { get; set; }
    internal DbSet<Message> Messages { get; set; }

    public LifematesDbContext(DbContextOptions<LifematesDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserConfigurations());
        modelBuilder.ApplyConfiguration(new UserContactConfigurations());
        modelBuilder.ApplyConfiguration(new UserImageConfiguration());
        modelBuilder.ApplyConfiguration(new UserInterestConfiguration());
        modelBuilder.ApplyConfiguration(new UserLocationConfiguration());
        modelBuilder.ApplyConfiguration(new UserSettingsConfiguration());
        modelBuilder.ApplyConfiguration(new UserLikesConfiguration());
        modelBuilder.ApplyConfiguration(new UserDislikesConfiguration());
        modelBuilder.ApplyConfiguration(new InterestConfiguration());
        modelBuilder.ApplyConfiguration(new ChatConfigurations());
        modelBuilder.ApplyConfiguration(new ChatUserConfigurations());
        modelBuilder.ApplyConfiguration(new ReportConfiguration());
        modelBuilder.ApplyConfiguration(new MessageConfiguration());
    }
}