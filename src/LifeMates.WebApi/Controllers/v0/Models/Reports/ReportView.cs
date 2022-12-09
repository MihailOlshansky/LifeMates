namespace LifeMates.WebApi.Controllers.v0.Models.Reports;

public class ReportView
{
    public long UserId { get; set; }
    public long ComplainerId { get; set; }
    public ReportType Type { get; set; }
    public string Value { get; set; } = string.Empty;
}