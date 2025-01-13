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
    public bool IsCompleted { get; init; }
    public int PointsReward { get; init; }
    public int BonusPoints { get; init; }
    public ICollection<EventAttendance> EventAttendances { get; init; } = new List<EventAttendance>();
    
    public Event(Guid id, string title, string description, DateTime startTime, 
                DateTime endTime, string location, bool approval, int pointsReward = 100, 
                int bonusPoints = 50)
    {
        Id = id;
        Title = title;
        Description = description;
        StartTime = startTime;
        EndTime = endTime;
        Location = location;
        Approval = approval;
        PointsReward = pointsReward;
        BonusPoints = bonusPoints;
        IsCompleted = false;
    }
}


