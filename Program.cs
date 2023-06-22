using MyWebApp.Middleware;
using MyWebApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyWebApp.Data;
using MyWebApp.Models;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
  builder.Services.AddDbContext<PersonContext>(options =>
      options.UseSqlite(builder.Configuration.GetConnectionString("PersonContext") ?? throw new InvalidOperationException("Connection string 'PersonContext' not found.")));
}
else
{
  builder.Services.AddDbContext<PersonContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("PersonContext") ?? throw new InvalidOperationException("Connection string 'PersonContext' not found.")));
}

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IFileWriter, LoggingFileWriter>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  SeedData.Initialize(services);
}

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

app.UseAuthorization();

app.UseLoggingMiddleware();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "rookie",
    pattern: "/NashTech/{controller=Rookie}/{action=Index}/{id?}");

app.Run();
