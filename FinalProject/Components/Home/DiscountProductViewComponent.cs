using FinalProject.Data;
using FinalProject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Components.Home
{
    public class DiscountProductViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public DiscountProductViewComponent(ApplicationDbContext context,
                                         IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
    }
}
