namespace shareme_backend.Models;

public class Like : Entity
{
    public Guid LikedById { get; set; }

    public User User { get; set; }

    public Guid PostId { get; set; }

    public Post Post { get; set; }
}
