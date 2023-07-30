namespace shareme_backend.Services;

using shareme_backend.DTOs.Category;

public interface ICategoryService
{
    Task<CategoryDTO> CreateCategory(CreateCategoryDTO data);

    Task<List<CategoryDTO>> ListCategories();
}
