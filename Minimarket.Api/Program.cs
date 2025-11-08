using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Minimarket.Core.Interface;
using Minimarket.Core.Interfaces;
using Minimarket.Core.Services;
using Minimarket.Core.Validator;
using Minimarket.Infraestructure.Filters;
using Minimarket.Infraestructure.Mappings;
using Minimarket.Infraestructure.Repositories;
using Minimarket.Infraestructure.Validations;
using Minimarket.Infrastructure.Data.Context;
using Minimarket.Infrastructure.Filters;
using Minimarket.Infrastructure.Repositories;
using Minimarket.Infrastructure.Validations;
using SocialMedia.Infrastructure.Data;

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
        
        builder.Services.AddTransient<IUserRepository, UserRepository>();


        builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        builder.Services.AddScoped<IDapperContext, DapperContext>();

        //builder.Services.AddTransient<IUserRepository, UserRepository>();
        //builder.Services.AddTransient<IProductRepository, ProductRepository>();
        //builder.Services.AddTransient<ISaleRepository, SaleRepository>();
        //builder.Services.AddTransient<IProductInSaleRepository, ProductInSaleRepository>();

        // IBaseRepository
        builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();


        builder.Services.AddScoped<IValidatorService, ValidationService>();
        builder.Services.AddScoped<ValidationFilter>();

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<GlobalExceptionFilter>();
        }).AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }).ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });


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


        #region Swagger Config
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "Backend Minimarket API",
                Version = "v1",
                Description = "Documentación de la API de Minimarket - .NET 9",
                Contact = new()
                {
                    Name = "Mateo Wilson Quispe Aylas",
                    Email = "mateo.quispe@ucb.edu.bo"
                }
            });
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            options.EnableAnnotations();
;

        });

        #endregion

        var app = builder.Build();

        // uso del swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Minimarket API V1");
                options.RoutePrefix = string.Empty;
            });
        }
        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}