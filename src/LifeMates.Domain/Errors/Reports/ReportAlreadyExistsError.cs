using LifeMates.Domain.Shared.Reports;

namespace LifeMates.Domain.Errors.Reports;

public class ReportAlreadyExistsError : ApplicationError
{
    public ReportAlreadyExistsError(ReportType type)
    {
        Message = "Уже есть ваша жалоба данного типа на выбранного пользователя";
    }
}