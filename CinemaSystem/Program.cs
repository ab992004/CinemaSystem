using CinemaSystem.Data;
using CinemaSystem.Models;
using CinemaSystem.Repositories;
using CinemaSystem.Repositories.IRepositories;
using ECommerce521.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options=>
{     
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IRepository<ApplicationUserOTP>, Repository<ApplicationUserOTP>>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddIdentity<AppUser, IdentityRole>(confi =>
{
    confi.User.RequireUniqueEmail = true;
    confi.Password.RequiredLength = 15;
    confi.Password.RequireNonAlphanumeric = false;
    confi.Lockout.MaxFailedAccessAttempts = 3;
    confi.SignIn.RequireConfirmedEmail = false;
})
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
