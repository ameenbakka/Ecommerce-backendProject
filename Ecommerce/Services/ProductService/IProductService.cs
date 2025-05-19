using Ecommerce.DTO;

namespace Ecommerce.Services.ProductService
{
    public interface IProductService
    {
        Task<List<Productviewdto>> GetAllProducts();
        Task<Productviewdto> GetProductsById(int id);
        Task<List<Productviewdto>> GetProductsByCategory(string catogaryname);
        Task<List<Productviewdto>> SearchProduct(string Search);
        Task<bool> AddProduct(AddProductDto addProduct, IFormFile image);
        Task<bool> DeleteProduct(int id);
        Task<bool> EditProduct(int id, AddProductDto editproduct, IFormFile image);
    }
}