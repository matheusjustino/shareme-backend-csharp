namespace shareme_backend.Models;

public class Comment : Entity
{
    public string Content { get; set; }

    public Guid CommentedById { get; set; }

    public User User { get; set; }

    public Guid PostId { get; set; }

    public Post Post { get; set; }
}
