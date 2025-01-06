public class EventService
{
    private List<Event> _events = new();

    public async Task<Event?> GetEventAsync(Guid id) => await Task.FromResult(_events.FirstOrDefault(e => e.Id == id));
    public async Task<IEnumerable<Event>> GetAllEventsAsync() => await Task.FromResult(_events);
    public async Task<Event> CreateEventAsync(Event eventItem) { _events.Add(eventItem); return await Task.FromResult(eventItem); }
    public async Task<Event?> UpdateEventAsync(Guid id, Event eventItem)
    {
        var index = _events.FindIndex(e => e.Id == id);
        if (index == -1) return null;
        _events[index] = eventItem with { Id = id };
        return await Task.FromResult(_events[index]);
    }
    public async Task<bool> DeleteEventAsync(Guid id)
    {
        var eventItem = _events.FirstOrDefault(e => e.Id == id);
        if (eventItem == null) return false;
        _events.Remove(eventItem);
        return await Task.FromResult(true);
    }
}
public interface IEventService
{
    Task<Event?> GetEventAsync(Guid id);
    Task<IEnumerable<Event>> GetAllEventsAsync();
    Task<Event> CreateEventAsync(Event eventItem);
    Task<Event?> UpdateEventAsync(Guid id, Event eventItem);
    Task<bool> DeleteEventAsync(Guid id);
}