using EVS.App.Presentation.Configuration.Events;

namespace EVS.App.Presentation.Models.Events;

public class EventSearchBarModel
{
    public string EventName { get; set; } = string.Empty;
    public int PageSize { get; set; } = EventListOptions.MinimalPageSize;
    public static EventSearchBarModel Empty => new();
}