using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public record UserPointsModel
{
    [Key]
    public int AllTimePoints { get; set; } = 0;
    public int PointAmount { get; set; } = 0;
}
