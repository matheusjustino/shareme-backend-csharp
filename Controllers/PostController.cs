namespace shareme_backend.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shareme_backend.DTOs.Auth;
using shareme_backend.DTOs.Post;
using shareme_backend.Services;

[Route("api/posts")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        this._postService = postService;
    }

    [Authorize]
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<PostDTO>> CreatePost([FromForm] CreatePostDTO body)
    {
        var user = (CurrentUser)HttpContext.Items["User"];
        var newPost = await this._postService.CreatePost(user.UserId, body);

        return Ok(newPost);
    }

    [HttpGet]
    public async Task<ActionResult<List<ListPostsResponseDTO>>> ListPosts([FromQuery] int? skip, [FromQuery] int? limit)
    {
        var posts = await this._postService.ListPosts(skip ?? 0, limit ?? 10);
        return Ok(posts);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PostDTO>> GetPost([FromRoute] Guid id)
    {
        var user = (CurrentUser)HttpContext.Items["User"];
        PostDTO post;
        if (user != null)
        {
            post = await this._postService.GetPost(id, user.UserId);
        }
        else
        {
            post = await this._postService.GetPost(id, null);
        }

        return Ok(post);
    }

    [Authorize]
    [HttpPost("add/comment")]
    public async Task<ActionResult<CommentDTO>> CreateComment([FromBody] CreateCommentDTO body)
    {
        var user = (CurrentUser)HttpContext.Items["User"];
        var comment = await this._postService.CreateComment(user.UserId, body);

        return Ok(comment);
    }

    [Authorize]
    [HttpPost("{id:guid}/like")]
    public async Task<ActionResult> LikeDislikePost([FromRoute] Guid id)
    {
        var user = (CurrentUser)HttpContext.Items["User"];
        await this._postService.LikeDislikePost(user.UserId, id);

        return Ok();
    }

    [HttpGet("image/{filename}")]
    public ActionResult GetFile([FromRoute] string filename)
    {
        var filePath = this._postService.GetFile(filename);
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var contentType = GetContentType(filePath);
        return File(fileStream, contentType, filename);
    }

    private static string GetContentType(string filePath)
    {
        var ext = Path.GetExtension(filePath).ToLowerInvariant();
        return ext switch
        {
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream",
        };
    }
}
