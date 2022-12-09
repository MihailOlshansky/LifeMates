using LifeMates.Domain.Shared.Users;

namespace LifeMates.Core.Commands.User.Models;

public record UserContact(ContactType Type, string Value);