using Ecommerce.DTO;
namespace Ecommerce.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<bool> AddCategory(CategoryViewDto categoryViewDto);
        Task<bool> RemoveCategory(int id);
        Task<List<CategoryViewDto>> ViewCategory();
    }
}
