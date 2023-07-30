namespace shareme_backend.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shareme_backend.DTOs.Category;
using shareme_backend.Services;

[Route("api/categories")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        this._categoryService = categoryService;
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CreateCategoryDTO body)
    {
        var newCategory = await this._categoryService.CreateCategory(body);
        return Ok(newCategory);
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDTO>>> ListCategories()
    {
        var categories = await this._categoryService.ListCategories();
        return Ok(categories);
    }
}
