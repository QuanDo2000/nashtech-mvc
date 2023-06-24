using Microsoft.EntityFrameworkCore;

using MyWebApp.Models;

namespace MyWebApp.Data;

public class TaskContext : DbContext
{
  public TaskContext(DbContextOptions<TaskContext> options) : base(options)
  {
  }

  public DbSet<TaskItem> TaskItems { get; set; } = null!;
}