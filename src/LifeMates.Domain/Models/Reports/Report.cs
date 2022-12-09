using LifeMates.Domain.Shared.Reports;

namespace LifeMates.Domain.Models.Reports;

public class Report
{
    public long Id { get; private set; }
    public long UserId { get; private set; }
    public long ComplainerId { get; private set; }
    public ReportType Type { get; private set; }
    public string Value { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public Report(ReportType type, string value, long userId, long complainerId)
    {
        Type = type;
        Value = value;
        UserId = userId;
        ComplainerId = complainerId;
        CreatedAt = DateTime.UtcNow;
    }

    protected Report()
    {
        
    }
}