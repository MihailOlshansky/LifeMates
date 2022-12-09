using LifeMates.Domain.Models.Reports;
using LifeMates.Domain.Shared.Reports;
using LifeMates.Storage.SharedKernel.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeMates.Storage.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly LifematesDbContext _dbContext;

    public ReportRepository(LifematesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> Exists(long complainerId, ReportType type, CancellationToken cancellationToken)
    {
        return _dbContext.Reports.AnyAsync(x => x.ComplainerId == complainerId && x.Type == type, cancellationToken);
    }

    public void Create(Report report, CancellationToken cancellationToken)
    {
        _dbContext.Reports.Add(report);
    }
}