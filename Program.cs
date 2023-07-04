using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System.Text.Json.Serialization;

using MyWebApp.Middleware;
using MyWebApp.Services;
using MyWebApp.Data;
using MyWebApp.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<StudentContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentContext") ?? throw new InvalidOperationException("Connection string 'StudentContext' not found.")));
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreContext") ?? throw new InvalidOperationException("Connection string 'StoreContext' not found.")));

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
builder.Services.AddDbContext<TaskContext>(options =>
  options.UseInMemoryDatabase("TaskList"));

builder.Services.AddScoped<IFileWriter, LoggingFileWriter>();
builder.Services.AddScoped<IDbOperations<Person>, PersonOperations>();

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(
  options =>
  {
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
  }
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
else
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "rookie",
    pattern: "/NashTech/{controller=Rookie}/{action=Index}/{id?}");

app.UseLoggingMiddleware();

app.Run();
