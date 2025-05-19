using AutoMapper;
using CloudinaryDotNet;
using Ecommerce.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Services.CloudinaryService;
using Ecommerce.DTO;
using Ecommerce.Models;
using Ecommerce.Services.ProductService;

namespace Ecommerce.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly AppdbContext _context;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;
        public ProductService(AppdbContext context, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }
        public async Task<List<Productviewdto>> GetAllProducts()
        {
                try
                {
                    var products = await _context.products.Include(x => x.Category).ToListAsync();
                    if (products.Count > 0)
                    {
                        var productall = products.Select(x => new Productviewdto
                        {
                            Title = x.Title,
                            Description = x.Description,
                            Price = x.Price,
                            stock = x.Stock,
                            Image = x.Image
                        }).ToList();
                        return productall;
                    }
                    return new List<Productviewdto>();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
        }
        public async Task<Productviewdto> GetProductsById(int id)
        {
            try
            {
                var products = await _context.products.FirstOrDefaultAsync(x => x.Id == id);
                if (products == null)
                {
                    return null;
                }
                return new Productviewdto()
                {
                    Title = products.Title,
                    Description = products.Description,
                    Price = products.Price,
                    stock = products.Stock,
                    Image = products.Image
                };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Productviewdto>> GetProductsByCategory(string categoryname)
        {
            var products = await _context.products.Include(x => x.Category)
                .Where(x => x.Category.Name == categoryname)
                .Select(x => new Productviewdto
                {
                    Title = x.Title,
                    Description = x.Description,
                    Price = x.Price,
                    stock = x.Stock,
                    Image = x.Image

                }).ToListAsync();
            if (!products.Any())
            {
                return new List<Productviewdto>();
            }
            return products;
        }
        public async Task<List<Productviewdto>> SearchProduct(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return new List<Productviewdto>();
            }
            var products = await _context.products.Include(x => x.Category)
                .Where(p => p.Title.ToLower().Contains(search.ToLower()))
                .ToListAsync();
            return products.Select(s => new Productviewdto
            {
                Title = s.Title,
                Description = s.Description,
                Price = s.Price,
                Image = s.Image,
            }).ToList();
        }
        
        public async Task<bool> AddProduct(AddProductDto addProduct, IFormFile image)
        {
            try
            {

                if (addProduct == null)
                {
                    return false;
                }
                var category = await _context.Category.FirstOrDefaultAsync(x => x.Id == addProduct.CategoryId);
                if (category == null)
                {
                    throw new Exception("there is no  category in this id");
                }
                if (image == null)
                {
                    throw new InvalidOperationException("Image is Not Uploaded");
                }

                string imageUrl = await _cloudinaryService.UploadImage(image);
                var product = _mapper.Map<Product>(addProduct);
                product.Image = imageUrl;
                await _context.products.AddAsync(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return false;
            }
            _context.products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> EditProduct(int id, AddProductDto editproduct, IFormFile image)
        {
            var exproduct = await _context.products.FirstOrDefaultAsync(x => x.Id == id);
            var catexist = await _context.Category.FirstOrDefaultAsync(x => x.Id == editproduct.CategoryId);
            if (catexist == null)
            {
                throw new Exception("There is no category in this id");
            }
            if (exproduct == null)
            {
                return false;
            }
            try
            {
                exproduct.Title = editproduct.Title;
                exproduct.Description = editproduct.Description;
                exproduct.Price = editproduct.Price;
                exproduct.Stock = editproduct.Stock;
                exproduct.CategoryId = editproduct.CategoryId;
                if (image != null && image.Length > 0)
                {
                    string imageUrl = await _cloudinaryService.UploadImage(image);
                    exproduct.Image = imageUrl;
                }
                _context.products.Update(exproduct);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
    }


}
