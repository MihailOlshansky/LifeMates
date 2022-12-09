using FluentResults;
using FluentValidation;
using LifeMates.Domain.Errors;
using MediatR;

namespace LifeMates.Core.Mediatr.Behaviors
{
    public class FluentResultsPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : ResultBase, new()
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public FluentResultsPipelineBehavior
        (
            IEnumerable<IValidator<TRequest>> validators
        )
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancel,
            RequestHandlerDelegate<TResponse> next)
        {
            var validationResult = Validate(request);

            if (validationResult.IsFailed)
            {
                return validationResult;
            }

            return await next();
        }

        private TResponse Validate(TRequest request)
        {
            var errors = _validators
                .Select(v => v.Validate(request))
                .SelectMany(vr => vr.Errors)
                .Where(f => f != null)
                .Select(x => new PropertyValidationError(x.PropertyName, x.ErrorMessage, x.ErrorCode))
                .ToList();

            var result = new TResponse();
            if (errors.Any())
            {
                var validationError = new ValidationError();
                validationError.Reasons.AddRange(errors);

                result.Reasons.Add(validationError);
            }

            return result;
        }
    }
}