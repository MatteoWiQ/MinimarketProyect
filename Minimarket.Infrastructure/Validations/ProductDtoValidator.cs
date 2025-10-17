using FluentValidation;
using Minimarket.Infraestructure.Dtos;
namespace Minimarket.Infrastructure.Validations
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Nombre del producto es requerido.")
                .Length(2, 100).WithMessage("El nombre del producto debe tener entre 2 y 100 caracteres.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("El precio del producto debe ser positivo.");

            RuleFor(p => p.ProductBrand)
                .NotEmpty().WithMessage("La marca del producto es requerida.");
            // Validar que el stock no sea negativo
            RuleFor(p => p.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock del producto no puede ser negativo.");
            // Validar que la descripción no esté vacía y tenga una longitud adecuada
            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("La descripción del producto es requerida.")
                .MaximumLength(500).WithMessage("La descripción del producto no puede exceder los 500 caracteres.");
            // Validar que CreatedAt no sea una fecha futura
            RuleFor(p => p.CreatedAt)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de creación no puede ser futura.");
        }
    }
}
