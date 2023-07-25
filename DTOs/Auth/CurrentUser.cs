namespace shareme_backend.DTOs.Auth;

public class CurrentUser
{
    public Guid UserId { get; set; }

    public string Email { get; set; }

    public string Username { get; set; }
}
