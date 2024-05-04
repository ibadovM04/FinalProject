using FinalProject.Areas.Admin.DTOs;
using FinalProject.Areas.Admin.ViewModels;
using FinalProject.Data;
using FinalProject.Enums;
using FinalProject.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminCookies")]
    public class UserController : Controller
    {

        private ICompositeViewEngine _viewEngine;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context,
            ICompositeViewEngine viewEngine,
            IConfiguration configuration)
        {
            _context = context;
            _viewEngine = viewEngine;
            _configuration = configuration;

        }
        public async Task<IActionResult> List(int page = 1)
        {
            var query = _context.Users.Where(u => true);

            var result = await SelectUsers(query, page);

            var vm = new UserListVm
            {
                CurrentPage = page,
                TotalPage = result.Item2,
                Users = result.Item1
            };

            return View(vm);
        }

        private async Task<(List<UserDto>, int)> SelectUsers(IQueryable<User> query, int page)
        {
            var takeNumber = Convert.ToInt32(_configuration["List:AdminUsers"]);

            query = query.OrderByDescending(u => u.Created);

            var count = await query.CountAsync();

            var totalPage = (int)Math.Ceiling(count / (decimal)takeNumber);

            var users = await query.Include(u => u.UserRole)
                                 .Select(u => new UserDto
                                 {
                                     UserId = u.Id,
                                     Name = u.Name,
                                     Surname = u.Surname,
                                     Email = u.Email,
                                     Gender = u.Gender,
                                     GenderText = u.Gender != null ? ((bool)u.Gender ? "Kişi" : "Qadın") : "Qeyd olunmayıb",
                                     UserStatusId = u.UserStatusId,
                                     StatusText = u.UserStatusId == (byte)UserStatusEnum.Active ? "Aktiv" : "Deaktiv",
                                     Registered = u.Created,
                                     Role = u.UserRole.Name,
                                     RoleId = u.UserRoleId
                                 })
                                 .Skip((page - 1) * takeNumber)
                                 .Take(takeNumber)
                                 .ToListAsync();


            return (users, totalPage);
        }
    }
}
