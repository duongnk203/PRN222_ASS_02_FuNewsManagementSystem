using FUNewsManagementSystem;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;
using FUNewsManagementSystem.Service;
using FUNewsManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var connectionStrings = builder.Configuration.GetConnectionString("FuNewsSystemDB");
builder.Services.AddDbContext<FunewsManagementContext>(options => options.UseSqlServer(connectionStrings));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Authentication/Login";  // Trang login
        options.LogoutPath = "/Authentication/Logout"; // Trang logout
        options.AccessDeniedPath = "/Authentication/AccessDenied"; // Trang bị từ chối
    });

//Cau hinh signalr
builder.Services.AddSignalR();
builder.Services.AddSingleton<SignalRServer>();
builder.Services.AddRazorPages().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<RoleService>();
builder.Services.AddAuthorization();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<INewsArticalRepository, NewsArticalRepository>();
builder.Services.AddTransient<IReportRepository, ReportRepository>();
builder.Services.AddTransient<ISystemAccountRepository, SystemAccountRepository>();
builder.Services.AddTransient<ITagRepository, TagRepository>();
builder.Services.AddTransient<ICommentRepository, CommentRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<AccountLogin>(
    option =>
    {
        var admin = builder.Configuration.GetSection("AccountAdmin");
        return new AccountLogin()
        {
            AccountEmail = admin["Email"],
            Password = admin["Password"]
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<SignalRServer>("/signalRServer");

app.Run();
