namespace shareme_backend.Services;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using shareme_backend.Data;
using shareme_backend.DTOs.Category;

public class CategoryService : ICategoryService
{
    private readonly ILogger<CategoryService> _logger;

    private readonly AppDbContext _context;

    private readonly IMapper _mapper;

    public CategoryService(ILogger<CategoryService> logger, AppDbContext context, IMapper mapper)
    {
        this._logger = logger;
        this._context = context;
        this._mapper = mapper;
    }

    public async Task<CategoryDTO> CreateCategory(CreateCategoryDTO data)
    {
        var newCategory = new Models.Category { Name = data.Name, };

        await this._context.Category.AddAsync(newCategory);
        await this._context.SaveChangesAsync();

        return this._mapper.Map<CategoryDTO>(newCategory);
    }

    public async Task<List<CategoryDTO>> ListCategories()
    {
        return await this._context.Category.OrderBy(c => c.Name).Select(c => this._mapper.Map<CategoryDTO>(c)).ToListAsync();
    }
}
