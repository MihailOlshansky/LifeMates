namespace LifeMates.WebApi.Controllers.v0.Models.Reports;

public class CreateReportRequest
{
    public long UserId { get; set; }
    public ReportType Type { get; set; }
    public string Value { get; set; } = string.Empty;
}