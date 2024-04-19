using FinalProject.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);





builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["Database:Connection"]);
});




builder.Services.AddControllersWithViews();

var app = builder.Build();


app.UseStaticFiles();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}");

app.Run();
