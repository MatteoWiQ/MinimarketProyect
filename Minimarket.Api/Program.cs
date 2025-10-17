using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Minimarket.Core.Dtos;
using Minimarket.Infraestructure.Repositories;
using Minimarket.Core.Interface;
using Minimarket.Core.Services;
using Minimarket.Core.Validator;
using Minimarket.Infraestructure.Dtos;
using Minimarket.Infraestructure.Filters;
using Minimarket.Infraestructure.Mappings;

using Minimarket.Infraestructure.Validations;
using Minimarket.Infrastructure.Data.Context;
using Minimarket.Infrastructure.Validations;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddScoped<UserDtoValidator>();
        builder.Services.AddScoped<ProductDtoValidator>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IUserRespository, UserRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IValidatorService, ValidationService>();
        builder.Services.AddScoped<ValidationFilter>();
        
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
        });
        builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<ProductDtoValidator>();
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