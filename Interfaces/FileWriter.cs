namespace MyWebApp.Interfaces;

public interface IFileWriter
{
  Task WriteToFileAsync(string filePath, string content);
}

public class LoggingFileWriter : IFileWriter
{
  private readonly ILogger<LoggingFileWriter> _logger;

  public LoggingFileWriter(ILogger<LoggingFileWriter> logger)
  {
    _logger = logger;
  }

  public async Task WriteToFileAsync(string filePath, string content)
  {
    await File.AppendAllTextAsync(filePath, content);
    _logger.LogInformation($"File {filePath} was written at {DateTime.Now.ToString()}");
  }
}