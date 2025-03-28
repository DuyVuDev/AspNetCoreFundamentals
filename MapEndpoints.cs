using R2EDuy.AspNetCoreFundamentals.Loggin.Endpoints;

namespace R2EDuy.AspNetCoreFundamentals.Loggin
{
    public static class MapEndpointDI
    {
        public static void MapEndpoints(this WebApplication app)
        {
            app.MapStudentEndpoints();
        }
    }
}
