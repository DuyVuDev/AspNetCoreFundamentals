using R2EDuy.AspNetCoreFundamentals.Loggin.middleware;

namespace R2EDuy.AspNetCoreFundamentals.Loggin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.UseLoggingMiddleware();

            app.MapEndpoints();

            app.Run();
        }
    }
}
