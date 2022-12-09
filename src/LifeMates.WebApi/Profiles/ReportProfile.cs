using AutoMapper;
using LifeMates.Core.Commands.Reports;
using LifeMates.WebApi.Controllers.v0.Models.Reports;

namespace LifeMates.WebApi.Profiles;

public class ReportProfile : Profile
{
    public ReportProfile()
    {
        DomainMap();
        CreateReportMap();
    }

    private void CreateReportMap()
    {
        CreateMap<CreateReportRequest, CreateReportCommand>();
    }

    private void DomainMap()
    {
        CreateMap<ReportType, Domain.Shared.Reports.ReportType>();
        CreateMap<Domain.Models.Reports.Report, ReportView>();
    }
}