namespace shareme_backend.DTOs.User;

using System.ComponentModel.DataAnnotations;

public class ListUsernamesQuery
{
    [Required]
    public string username { get; set; }
}
