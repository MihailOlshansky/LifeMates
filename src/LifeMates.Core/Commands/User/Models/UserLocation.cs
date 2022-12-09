using FluentValidation;

namespace LifeMates.Core.Commands.User.Models;

public record UserLocation(double Latitude, double Longitude);

public class UserLocationValidator : AbstractValidator<UserLocation>
{
    public UserLocationValidator()
    {
        RuleFor(x => x.Latitude)
            .LessThan(90)
            .GreaterThan(-90);
        
        RuleFor(x => x.Longitude)
            .LessThan(90)
            .GreaterThan(-90);
    }
}