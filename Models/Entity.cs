namespace shareme_backend.Models;

public class Entity
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; }
}
