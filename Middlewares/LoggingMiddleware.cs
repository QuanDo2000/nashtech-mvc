using System.Globalization;

using MyWebApp.Interfaces;

namespace MyWebApp.Middleware;

public class LoggingMiddleware
{
  private readonly RequestDelegate _next;

  public LoggingMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context, IFileWriter fileWriter)
  {
    await _next(context);
    string currentDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    string logPath = $"./Logs/{currentDate}.txt";
    string content = $"""
    Request {context.Request.Method} {context.Request.Path} was processed at {DateTime.Now.ToString()}
      Schema: {context.Request.Scheme}
      Host: {context.Request.Host}
      Path: {context.Request.Path}
      QueryString: {context.Request.QueryString}
      Request Body: {await new StreamReader(context.Request.Body).ReadToEndAsync()}

    """;
    await fileWriter.WriteToFileAsync(logPath, content);
  }
}

public static class LoggingMiddlewareExtensions
{
  public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<LoggingMiddleware>();
  }
}