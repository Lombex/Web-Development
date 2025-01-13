using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public interface IEventService
{
    Task<Event?> GetEventAsync(Guid id);
    Task<IEnumerable<Event>> GetAllEventsAsync();
    Task<Event> CreateEventAsync(Event eventItem);
    Task<Event?> UpdateEventAsync(Guid id, Event eventItem);
    Task<bool> DeleteEventAsync(Guid id);
    // New method
    Task<Event> CompleteEventAsync(Guid id);
}

public class EventService : IEventService
{
    private readonly AppDbContext _context;

    public EventService(AppDbContext context)
    {
        _context = context;
    }

    // Haal een event op via ID
    public async Task<Event?> GetEventAsync(Guid id)
    {
        return await _context.Events.FindAsync(id); 
    }

    // Haal alle events op
    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        return await _context.Events.ToListAsync(); 
    }

    // Maak een nieuw event aan
    public async Task<Event> CreateEventAsync(Event eventItem)
    {
        await _context.Events.AddAsync(eventItem); 
        await _context.SaveChangesAsync(); 
        return eventItem; 
    }

    // Update een bestaand event
    public async Task<Event?> UpdateEventAsync(Guid id, Event eventItem)
    {
        var existingEvent = await _context.Events.FindAsync(id); 
        if (existingEvent == null) return null;
        _context.Events.Remove(existingEvent); 
        await _context.Events.AddAsync(new Event(id, eventItem.Title, eventItem.Description, eventItem.StartTime, eventItem.EndTime, eventItem.Location, eventItem.Approval)); // Voeg het bijgewerkte event opnieuw toe
        await _context.SaveChangesAsync();

        return eventItem; 
    }

    // Verwijder een event
    public async Task<bool> DeleteEventAsync(Guid id)
    {
        var eventItem = await _context.Events.FindAsync(id); 
        if (eventItem == null) return false;

        _context.Events.Remove(eventItem);
        await _context.SaveChangesAsync();
        return true; 
    }
    public async Task<Event> CompleteEventAsync(Guid id)
    {
        var @event = await _context.Events
            .Include(e => e.EventAttendances)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (@event == null) return null;

        // Create a new event with updated completion status since Event is a record
        var completedEvent = new Event(
            @event.Id,
            @event.Title,
            @event.Description,
            @event.StartTime,
            @event.EndTime,
            @event.Location,
            @event.Approval,
            @event.PointsReward,
            @event.BonusPoints
        );

        _context.Events.Remove(@event);
        await _context.Events.AddAsync(completedEvent);
        await _context.SaveChangesAsync();

        return completedEvent;
    }
}