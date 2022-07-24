using MyPortfolio.Middleware;
using PostgreSQLIntegration.Context;
using MyPortfolio.Services.MapUserService;
using MyPortfolio.Database.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<PostgreSQLContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("ProductionDB")), ServiceLifetime.Singleton);
builder.Services.AddSingleton<IMapUserService, MapUserService>();
builder.Services.AddSingleton(typeof(IBaseRepository<>), typeof(BaseRepository<>));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithRedirects("/Home/Error");
    app.UseReverseProxyHttpsEnforcer();
    app.UseHsts();
}
else
{
    app.UseStatusCodePagesWithRedirects("/Home/Error");
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=About}/{id?}");

app.Run();