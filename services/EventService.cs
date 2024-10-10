public class EventService
{
    private List<Event> _events = new();

    public async Task<Event?> GetEventAsync(Guid id) => await Task.FromResult(_events.FirstOrDefault(e => e.id == id));
    public async Task<IEnumerable<Event>> GetAllEventsAsync() => await Task.FromResult(_events);
    public async Task<Event> CreateEventAsync(Event eventItem) { _events.Add(eventItem); return await Task.FromResult(eventItem); }
    public async Task<Event?> UpdateEventAsync(Guid id, Event eventItem)
    {
        var index = _events.FindIndex(e => e.id == id);
        if (index == -1) return null;
        _events[index] = eventItem with { Id = id };
        return await Task.FromResult(_events[index]);
    }
    public async Task<bool> DeleteEventAsync(Guid id)
    {
        var eventItem = _events.FirstOrDefault(e => e.id == id);
        if (eventItem == null) return false;
        _events.Remove(eventItem);
        return await Task.FromResult(true);
    }
}