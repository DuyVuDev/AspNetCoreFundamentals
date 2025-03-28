using System.Text;

namespace R2EDuy.AspNetCoreFundamentals.Loggin.middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var schema = context.Request.Scheme;
            var host = context.Request.Host.Value;
            var path = context.Request.Path;
            var queryString = context.Request.QueryString.ToString();
            string requestBody = "";

            // Get data from request body
            if (context.Request.ContentLength > 0 && context.Request.Body.CanRead)
            {
                context.Request.EnableBuffering(); // Allow multiple reads
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    requestBody = await reader.ReadToEndAsync();
                }
                context.Request.Body.Position = 0; // Reset position for the request pipeline to continue processing
            }

            // Capture the response body
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                string logMessage = $"[{DateTime.Now}] Schema: {schema}, Host: {host}, Path: {path}, QueryString: {queryString}, Body: {requestBody}, Response: {responseBodyText}\n";

                try
                {
                    // Ensure the logs directory exists
                    var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }

                    await File.AppendAllTextAsync(Path.Combine(logDirectory, "http_logs.txt"), logMessage);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during logging
                    Console.WriteLine($"Failed to write log: {ex.Message}");
                }

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }

    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
