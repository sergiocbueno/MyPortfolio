using MyPortfolio.Middleware;
using PostgreSQLIntegration.Context;
using MyPortfolio.Services.MapUserService;
using MyPortfolio.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

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
    app.UseStatusCodePagesWithRedirects("/Error/Handler/{0}");
    app.UseReverseProxyHttpsEnforcer();
    app.UseHsts();
}
else
{
    app.UseStatusCodePagesWithRedirects("/Error/Handler/{0}");
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=About}/{id?}");

app.Run();