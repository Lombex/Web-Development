using System;


public record Event
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string Location { get; init; }
    public bool Approval { get; init; }

    // You may also add a constructor for ease of use
    public Event(Guid id, string title, string description, DateTime startTime, DateTime endTime, string location, bool approval)
    {
        Id = id;
        Title = title;
        Description = description;
        StartTime = startTime;
        EndTime = endTime;
        Location = location;
        Approval = approval;
    }
}


