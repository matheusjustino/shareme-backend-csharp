namespace shareme_backend.Models;

using System.ComponentModel.DataAnnotations;

public class User : Entity
{
    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public string? Avatar { get; set; }

    public string? ProfileImg { get; set; }
}