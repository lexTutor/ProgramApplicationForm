
namespace Application.Infrastructure.Logger
{
    public interface ILogManager<T> where T : class
    {
        void LogError(Exception exception, string message = "", object? data = null);
        void LogError(string message, object? data = null);
        void LogInformation(string message, object? data = null);
        void LogInformation(string message, object? data0, object? data1);
        void LogWarning(string message, object? data = null);
    }
}