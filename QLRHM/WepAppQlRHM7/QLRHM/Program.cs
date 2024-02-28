using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using QLRHM.Models;
using QLRHM7.Middlewares;
using Rotativa.AspNetCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DatnqlrhmContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionString")));
builder.Services.AddDbContext<MasterNcvCvDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionString")));
//cau hinh de null van chay
builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
//cau hinh de null van chay

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                  .AddCookie(options =>
                  {
                      options.LoginPath = "/login";
                      options.Cookie.Name = "my_app_auth_cookie";
                      options.AccessDeniedPath = "/_404/AccessDenied";
                      options.LogoutPath = "/Login/Logout";
                  });

var app = builder.Build();
app.UseCustMiddleware();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
var cultureInfo = new CultureInfo("vi-VN");
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("vi-VN");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("vi-VN");
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
Thread.CurrentThread.CurrentUICulture = cultureInfo;
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");
IWebHostEnvironment env = app.Environment;
RotativaConfiguration.Setup(env.WebRootPath, "../wk/Windows");

app.Run();
