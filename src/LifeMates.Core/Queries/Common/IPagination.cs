using FluentValidation;

namespace LifeMates.Core.Queries.Common;

public interface IPagination
{
    int Offset { get; }
    int Limit { get; }
}

public class PaginationValidator : AbstractValidator<IPagination>
{
    public PaginationValidator()
    {
        RuleFor(x => x.Offset)
            .GreaterThan(-1)
            .LessThan(100);
        
        RuleFor(x => x.Limit)
            .GreaterThan(0)
            .LessThan(100);
    }
}