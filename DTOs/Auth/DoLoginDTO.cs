﻿namespace shareme_backend.DTOs.Auth;

using System.ComponentModel.DataAnnotations;
using System.Text.Json;

public class DoLoginDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}