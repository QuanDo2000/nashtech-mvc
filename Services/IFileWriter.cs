
namespace MyWebApp.Services;

public interface IFileWriter
{
  Task WriteToFileAsync(string filePath, string content);
}