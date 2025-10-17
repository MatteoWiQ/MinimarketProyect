using FluentValidation;
using Minimarket.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Infrastructure.Validations
{
    public class SaleDtoValidator : AbstractValidator<SaleDto>
    {
        public SaleDtoValidator()
        {
            
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("El ID del cliente es obligatorio")
                .GreaterThan(0)
                .WithMessage("El ID del cliente debe ser mayor que cero")
          
                ;
            RuleFor(x => x.CustomerName)
                .NotEmpty()
                .WithMessage("El nombre del cliente es obligatorio")
                .MaximumLength(200)
                .WithMessage("El nombre del cliente no puede tener más de 200 caracteres")
                ;
            
            RuleFor(x => x.Date)
                .Must(BeValidDate)
                .WithMessage("La fecha de venta no es válida")
                ;
        }

        private bool BeValidDate(DateTime fecha)
        {
            return fecha != default(DateTime) &&
                   fecha >= new DateTime(2000, 1, 1) &&
                   fecha <= DateTime.Now.AddDays(1);
        }

    }
}
