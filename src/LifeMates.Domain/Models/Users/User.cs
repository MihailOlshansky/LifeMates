using FluentResults;
using LifeMates.Domain.Models.Chats;
using LifeMates.Domain.Shared.Users;

namespace LifeMates.Domain.Models.Users;

public class User
{
    public long Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public UserGender Gender { get; private set; }
    public UserStatus Status { get; private set; }
    public DateTime? Birthday { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public UserCredentials Credentials { get; private set; }
    public UserLocation? Location { get; private set; }
    public UserSettings Settings { get; private set; }
    public ICollection<UserImage>? Images { get; private set; }
    public ICollection<UserContact>? Contacts { get; private set; }
    public ICollection<UserInterest>? Interests { get; private set; }
    public ICollection<UserLikes>? Likes { get; private set; }
    public ICollection<UserDislikes>? Dislikes { get; private set; }
    public ICollection<ChatUser>? Chats { get; private set; }
    public ICollection<Message>? Messages { get; private set; }
    public RequestHistory RequestHistory { get; private set; }
    
    public User(
        long id, 
        string name, 
        string? description, 
        UserGender gender,
        UserStatus status,
        DateTime? birthday,
        UserLocation? location,
        UserSettings settings,
        ICollection<UserImage>? images,
        ICollection<UserContact>? contacts,
        ICollection<UserInterest>? interests)
    {
        Id = id;
        Name = name;
        Description = description;
        Gender = gender;
        Status = status;
        Birthday = birthday;
        Location = location;
        Settings = settings;
        Images = images;
        Contacts = contacts;
        Interests = interests;
    }
    
    public User(
        string name, 
        string? description, 
        UserGender gender,
        UserStatus status,
        DateTime? birthday,
        UserLocation? location,
        UserSettings settings,
        ICollection<UserImage>? images,
        ICollection<UserContact>? contacts,
        ICollection<UserInterest>? interests)
    {
        Name = name;
        Description = description;
        Gender = gender;
        Status = status;
        Birthday = birthday;
        Location = location;
        Settings = settings;
        Images = images;
        Contacts = contacts;
        Interests = interests;
        CreatedAt = DateTime.UtcNow;
    }

    public Result<bool> Update(
        string name, 
        string? description, 
        UserGender gender,
        DateTime? birthday,
        UserSettings settings,
        ICollection<UserImage> images,
        ICollection<UserContact> contacts,
        ICollection<UserInterest> interests)
    {
        var hasChanges = false;

        if (Name != name
            || Description != description
            || Gender != gender
            || Birthday != birthday)
        {
            hasChanges = true;

            Name = name;
            Description = description;
            Gender = gender;
            Birthday = birthday;
        }

        var updateResult = Settings.Update(settings);
        hasChanges = hasChanges || updateResult.Value;

        updateResult = UpdateImages(images);
        hasChanges = hasChanges || updateResult.Value;
        
        updateResult = UpdateContacts(contacts);
        hasChanges = hasChanges || updateResult.Value;
        
        updateResult = UpdateInterests(interests);
        hasChanges = hasChanges || updateResult.Value;

        return Result.Ok(hasChanges);
    }

    private Result<bool> UpdateInterests(ICollection<UserInterest> interests)
    {
        var hasChanges = false;
        
        var newInterests = new List<UserInterest>();

        if (Interests is not null)
        {
            foreach (var interest in Interests)
            {
                if (interests.Any(i => i.InterestId == interest.InterestId))
                {
                    newInterests.Add(interest);
                }
                else
                {
                    hasChanges = true;
                }
            }
        }
        
        newInterests.AddRange(interests);

        var previousCount = Interests?.Count ?? 0;
        
        Interests = newInterests.DistinctBy(i => i.InterestId).ToList();

        hasChanges = hasChanges || previousCount != Interests.Count;
        
        return Result.Ok(hasChanges);
    }

    private Result<bool> UpdateContacts(ICollection<UserContact> contacts)
    {
        var hasChanges = false;
        
        var newContacts = new List<UserContact>();

        if (Contacts is not null)
        {
            foreach (var contact in Contacts)
            {
                if (contacts.Any(i => i.Type == contact.Type && i.Value == contact.Value))
                {
                    newContacts.Add(contact);
                }
                else
                {
                    hasChanges = true;
                }
            }
        }
        
        newContacts.AddRange(contacts);

        var previousCount = Contacts?.Count ?? 0;
        
        Contacts = newContacts.DistinctBy(i => (i.Type, i.Value)).ToList();

        hasChanges = hasChanges || previousCount != Contacts.Count;
        
        return Result.Ok(hasChanges);
    }

    private Result<bool> UpdateImages(ICollection<UserImage> images)
    {
        var hasChanges = false;
        
        var newImagesList = new List<UserImage>();

        if (Images is not null)
        {
            foreach (var image in Images)
            {
                if (images.Any(i => i.Url == image.Url))
                {
                    newImagesList.Add(image);
                }
                else
                {
                    hasChanges = true;
                }
            }
        }
        
        newImagesList.AddRange(images);

        var previousCount = Images?.Count ?? 0;
        
        Images = newImagesList.DistinctBy(i => i.Url).ToList();

        hasChanges = hasChanges || previousCount != Images.Count;
        
        return Result.Ok(hasChanges);
    }

    public Result<bool> Update(UserStatus newStatus)
    {
        var hasChanged = Status != newStatus;
        if (hasChanged)
        {
            Status = newStatus;
        }
        return Result.Ok(hasChanged);
    }
    
    public Result<bool> Update(UserLocation? userLocation)
    {
        const double tolerance = 0.000001;

        if (userLocation == Location ||
            (Location is not null && userLocation is not null &&
            Math.Abs(Location.Latitude - userLocation.Latitude) < tolerance &&
            Math.Abs(Location.Longitude - userLocation.Longitude) < tolerance))
        {
            return Result.Ok(false);
        }
        
        Location = userLocation;

        return Result.Ok(true);
    }

    protected User()
    {
        
    }
}