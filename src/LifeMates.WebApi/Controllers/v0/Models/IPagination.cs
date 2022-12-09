namespace LifeMates.WebApi.Controllers.v0.Models;

public interface IPagination
{
    int Offset { get; }
    int Limit { get; }
}