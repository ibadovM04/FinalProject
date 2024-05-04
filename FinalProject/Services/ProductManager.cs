using FinalProject.Data;
using FinalProject.DTOs;
using FinalProject.Interfaces;
using FinalProject.ServiceModels;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace FinalProject.Services
{
    public class ProductManager : IProductManager
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;

        private readonly HttpContext _httpContext;

        private readonly string _productImageBasePath;
        private readonly string _userId;
        public ProductManager(IHttpContextAccessor httpContextAccessor,
                             IConfiguration configuration,
                             ApplicationDbContext context)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _configuration = configuration;
            _context = context;
            _productImageBasePath = _configuration["Folders:Products"];
            _userId = GetUserId();

        }

        

        public async Task<ProductListVm> GetFilteredProducts(ProductListQueryModel request)
        
        {
            var takeNumber = request.Take ?? Convert.ToInt32(_configuration["List:Products"]);


            var query = _context.Products.Where(c => (request.CategoryId == null || c.CategoryId == request.CategoryId));
            var count = await query.CountAsync();

            query = query.OrderByDescending(c => c.Created);

            query = query.Skip((request.Page - 1) * takeNumber).Take(takeNumber);

            var productCount = await query.CountAsync();

            query = query.Include(c => c.ProductPhotos)
                         .Include(c => c.UserWishlists);

            var products = await query.Select(c => new ProductDto
            {
                ProductId = c.Id,

                Slug = c.Slug,

                Description = c.Description,

                ImageURL = _productImageBasePath + c.ProductPhotos.Where(a => a.IsMain).Select(a => a.ImageUrl).FirstOrDefault(),

                Name = c.Name,

                Price = c.SellAmount,

                IsWishlist = _userId == null ? false : c.UserWishlists.Any(a => a.UserId.ToString() == _userId),

                Discount = c.Discount,

                DiscountedPrice = c.Discount != null ? c.SellAmount - (c.SellAmount * (double)c.Discount / 100) : null
            }).ToListAsync();

            var totalPage = (int)Math.Ceiling(count / (decimal)takeNumber);

            var vm = new ProductListVm
            {
                CurrentPage = request.Page,
                TotalPage = totalPage,
                Products = products,
                ProductCount = productCount
            };

            return vm;

        }
        private string GetUserId()
        {
            string userId = null;

            if (_httpContext.User.Identity.IsAuthenticated)
            {
                userId = _httpContext.User.Claims.Where(c => c.Type == "Id").FirstOrDefault().Value;
            }

            return userId;
        }
    }

    
}
