﻿namespace shareme_backend.DTOs.Category;

using System.ComponentModel.DataAnnotations;

public class CreateCategoryDTO
{
    [Required]
    public string Name { get; set; }
}
