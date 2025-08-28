public static class Logger
{
    private static readonly string LogFilePath = Path.Combine("C:\\Users\\iriku\\source\\repos\\WeaTherForecast\\WeaTherForecast", "app.log");

    public static async Task LogAsync(string message)
    {
        try
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var fullMessage = $"{timestamp} - {message}{Environment.NewLine}";

            await File.AppendAllTextAsync(LogFilePath, fullMessage);
        }
        catch (Exception ex)
        {
            // Optional: fallback if logging fails
            System.Diagnostics.Debug.WriteLine($"Logging failed: {ex.Message}");
        }
    }

    public static string GetLogFilePath() => LogFilePath;
}
