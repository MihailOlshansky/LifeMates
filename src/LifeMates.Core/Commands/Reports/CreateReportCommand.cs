using FluentResults;
using FluentValidation;
using LifeMates.Auth.Services.Interfaces;
using LifeMates.Domain.Errors.Reports;
using LifeMates.Domain.Models.Reports;
using LifeMates.Domain.Shared.Reports;
using LifeMates.Storage.SharedKernel;
using LifeMates.Storage.SharedKernel.Repositories;
using MediatR;

namespace LifeMates.Core.Commands.Reports;

public record CreateReportCommand(long UserId, ReportType Type, string Value) : IRequest<Result<Report>>;

public class CreateReportCommandValidator : AbstractValidator<CreateReportCommand>
{
    public CreateReportCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.Value)
            .Must(x => !string.IsNullOrWhiteSpace(x));
    }
}

public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, Result<Report>>
{
    private readonly IReportRepository _reportRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public CreateReportCommandHandler(IReportRepository reportRepository, IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _reportRepository = reportRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<Report>> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var complainerId = _currentUser.GetUserId();

        if (await _reportRepository.Exists(complainerId, request.Type, cancellationToken))
        {
            return Result.Fail(new ReportAlreadyExistsError(request.Type));
        }

        var report = new Report(request.Type, request.Value, request.UserId, complainerId);
        _reportRepository.Create(report, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return Result.Ok(report);
    }
}