using Microsoft.EntityFrameworkCore;

using Minimarket.Core.Services;
internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        var app = builder.Build();


        

        // Test de conexión al arrancar (opcional, para debugging)
        using (var scope = app.Services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<MinimarketContext>();
            var canConnect = await ctx.Database.CanConnectAsync();
            Console.WriteLine($"Database.CanConnect: {canConnect}");

            if (canConnect)
            {
                // Ejemplo: contar productos
                var count = await ctx.Products.CountAsync();
                Console.WriteLine($"Productos en DB: {count}");
            }
            else
            {
                Console.WriteLine("No fue posible conectar a la base de datos.");
            }
        }

        app.MapControllers();
        app.Run();


        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}