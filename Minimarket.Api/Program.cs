using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Minimarket.Core.Dtos;
using Minimarket.Core.Interface;
using Minimarket.Core.Services;
using Minimarket.Core.Validator;
using Minimarket.Infraestructure.Dtos;
using Minimarket.Infraestructure.Filters;
using Minimarket.Infraestructure.Mappings;

using Minimarket.Infraestructure.Validations;
using Minimarket.Infrastructure.Data.Context;
using Minimarket.Infrastructure.Validations;
using Minimarket.Core.Interfaces;
using Minimarket.Infrastructure.Repositories;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAutoMapper(typeof(MappingProfile));


        builder.Services.AddScoped<UserDtoValidator>();
        builder.Services.AddScoped<ProductDtoValidator>();
        builder.Services.AddScoped<SaleDtoValidator>();
        builder.Services.AddScoped<ProductInSaleDtoValidator>();

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<ISaleService, SaleService>();
        builder.Services.AddScoped<IProductInSaleService, ProductInSaleService>();
        //builder.Services.AddTransient<IPostRepository, PostRepository>();
        //builder.Services.AddTransient<IUserRepository, UserRepository>();

        //builder.Services.AddTransient<IUserRepository, UserRepository>();
        //builder.Services.AddTransient<IProductRepository, ProductRepository>();
        //builder.Services.AddTransient<ISaleRepository, SaleRepository>();
        builder.Services.AddTransient<IProductInSaleRepository, ProductInSaleRepository>();

        // IBaseRepository
        builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));


        builder.Services.AddScoped<IValidatorService, ValidationService>();
        builder.Services.AddScoped<ValidationFilter>();
        
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
        });
        builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<ProductDtoValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<SaleDtoValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<ProductInSaleDtoValidator>();
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