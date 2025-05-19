using AutoMapper;
using Ecommerce.DTO;
using Ecommerce.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
namespace Ecommerce.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly AppdbContext _context;
        private IMapper _mapper;
        public CategoryService(AppdbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> AddCategory(CategoryViewDto categoryViewDto)
        {
            var isExist = await _context.Category.AnyAsync(x => x.Name.ToLower() == categoryViewDto.Name.ToLower());
            if (!isExist)
            {
                var d = _mapper.Map<Category>(categoryViewDto);
                await _context.Category.AddAsync(d);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> RemoveCategory(int id)
        {
            var res = await _context.Category.FirstOrDefaultAsync(x => x.Id == id);
            if (res == null)
            {
                return false;
            }
            else
            {
                _context.Category.Remove(res);
                await _context.SaveChangesAsync();
                return true;
            }
        }
        public async Task<List<CategoryViewDto>> ViewCategory()
        {
            var categories = await _context.Category.ToListAsync();
            if (categories.Count == 0)
            {
                return new List<CategoryViewDto>();
            }
            return _mapper.Map<List<CategoryViewDto>>(categories);
        }
    }
}
