using LifeMates.Domain.Models.Reports;
using LifeMates.Domain.Shared.Reports;

namespace LifeMates.Storage.SharedKernel.Repositories;

public interface IReportRepository
{
    Task<bool> Exists(long complainerId, ReportType type, CancellationToken cancellationToken);

    void Create(Report report, CancellationToken cancellationToken);
}