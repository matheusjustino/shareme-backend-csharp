namespace shareme_backend.Models;

public class Post : Entity
{
    public string Title { get; set; }

    public string Description { get; set; }

    public string ImageSrc { get; set; }

    public Guid PostedById { get; set; }

    public User User { get; set; }
}
