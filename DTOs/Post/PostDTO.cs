﻿namespace shareme_backend.DTOs.Post;

using shareme_backend.DTOs.Category;
using shareme_backend.DTOs.User;

public class PostDTO
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string ImageSrc { get; set; }

    public Guid PostedById { get; set; }

    public UserDTO User { get; set; }

    public List<CommentDTO> Comments { get; set; }

    public List<CategoryDTO> Categories { get; set; }

    public int? LikesCount { get; set; }

    public bool UserLikedPost { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
