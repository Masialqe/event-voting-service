namespace EVS.App.Application.UseCases.Events.ListEvents;

public sealed record LoadEventPageByNameRequest(string? EventName, int PageSize, int PageNumber)
{
    public static LoadEventPageByNameRequest Create(string? eventName, int pageSize, int pageNumber)
     => new( eventName, pageSize, pageNumber);
}
