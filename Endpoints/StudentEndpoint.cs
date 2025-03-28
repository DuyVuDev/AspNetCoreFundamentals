namespace R2EDuy.AspNetCoreFundamentals.Loggin.Endpoints
{
    public static class StudentEndpoint
    {
        public static void MapStudentEndpoints(this WebApplication app)
        {
            app.MapGet("/api/Student", () => "Hello World!");

            app.MapGet("/api/Student/{id:int}", (int id) => $"Hello student {id}");

            app.MapPut("/api/Student", async (HttpContext context) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                string body = await reader.ReadToEndAsync();

                return Results.Ok(new { Message = "Student created", Data = body });
            });

            app.MapPost("/api/Student/{id:int}", async (HttpContext context, int id) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                string body = await reader.ReadToEndAsync();

                return Results.Ok(new { Message = $"Student {id} updated", Data = body });
            });

            app.MapDelete("/api/Student/{id:int}", async (HttpContext context, int id) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                string body = await reader.ReadToEndAsync();

                return Results.Ok(new { Message = $"Student {id} deleted", Data = body });
            });

            app.MapDelete("/api/Student", () => "All students deleted");
        }
    }
}
