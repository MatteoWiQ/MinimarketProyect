using Microsoft.EntityFrameworkCore;
using Minimarket.Core.Interface;
using Minimarket.Core.Services;
using Minimarket.Core.Validator;
using Minimarket.Infraestructure.Mappings;
using Minimarket.Infraestructure.Data;
using Minimarket.Infraestructure.Repositories;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddScoped<CreateUserValidator>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUserRespository, UserRepository>();
        // Add services to the container.
        builder.Services.AddDbContext<MinimarketContext>(options =>
    options.UseSqlServer("Server=MATEOQAYLAS;Database=MinimarketDB;Trusted_Connection=True;TrustServerCertificate=True;"));

        builder.Services.AddControllers();
        var app = builder.Build();


        

        

        app.MapControllers();
        app.Run();


        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}