public class EventService
{
    private readonly List<Event> _events = new();

    public async Task<Event?> GetEventAsync(Guid id)
    {
        return await Task.FromResult(_events.FirstOrDefault(e => e.Id == id));
    }

    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        return await Task.FromResult(_events.AsReadOnly());
    }

    public async Task<Event> CreateEventAsync(Event eventItem)
    {
        if (eventItem == null) throw new ArgumentNullException(nameof(eventItem));

        // Optionally: Validate eventItem properties here
        _events.Add(eventItem);
        return await Task.FromResult(eventItem);
    }

    public async Task<Event?> UpdateEventAsync(Guid id, Event eventItem)
    {
        if (eventItem == null) throw new ArgumentNullException(nameof(eventItem));

        var index = _events.FindIndex(e => e.Id == id);
        if (index == -1) return null;

        // Update the existing event, keeping the original ID
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
