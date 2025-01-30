using BusinessLogic.Services;
using BusinessLogic.Services.Interfaces;
using DataAccess;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CmcConnectionString"));
});
builder.Services.AddAuthentication(builder.Configuration["CookieName"]).AddCookie(builder.Configuration["CookieName"],options =>
{
    options.Cookie.Name = builder.Configuration["CookieName"];
    options.LoginPath = "/Home/Login";
});

//REPOSITORIES
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INewProjectRepository, NewProjectRepository>();
builder.Services.AddScoped<ICandleRepository, CandleRepository>();
builder.Services.AddScoped<IMarketDepthRepository, MarketDepthRepository>();

//SERVICES
builder.Services.AddSingleton<IWebSocketService, RecentTradesService>();
builder.Services.AddHostedService<RecentTradesService>();

builder.Services.AddSingleton<IWebSocketService, CandleWebSocketService>();
builder.Services.AddHostedService<CandleWebSocketService>();

builder.Services.AddSingleton<IWebSocketService, MarketDepthService>();
builder.Services.AddHostedService<MarketDepthService>();

builder.Services.AddScoped<IBinanceService, BinanceService>();
builder.Services.AddHttpClient<IBinanceService, BinanceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
