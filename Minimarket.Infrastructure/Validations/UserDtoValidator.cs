using FluentValidation;
using Minimarket.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.Validator
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.UserType)
                .NotEmpty()
                .WithMessage("El tipo de usuario es obligatorio")
                ;
            RuleFor(x => x.FirstName).NotEmpty()
                .WithMessage("El nombre es obligatorio")
                .MaximumLength(100)
                .WithMessage("El nombre no puede tener más de 100 caracteres")
                ;
            RuleFor(x => x.LastName).NotEmpty()
                .WithMessage("El apellido es obligatorio")
                .MaximumLength(100)
                .WithMessage("El apellido no puede tener más de 100 caracteres")
                ;
            RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("El correo electrónico no es válido")
                ;
            RuleFor(x => x.Telephone).MaximumLength(50).When(x => !string.IsNullOrWhiteSpace(x.Telephone))
                .WithMessage("El teléfono no puede tener más de 50 caracteres")
                ;

            // Validar dateOfBirth
            RuleFor(x => x.DateOfBirth)
                .Must(BeValidDate).WithMessage("La fecha de nacimiento no es válida")
                ;
        }

        private bool BeValidDate(DateOnly? fecha)
        {
            if (fecha.HasValue)
            {
                return fecha.Value != default(DateOnly) &&
                       fecha.Value >= new DateOnly(1900, 1, 1) &&
                       fecha.Value <= new DateOnly(2100, 12, 31);
            }
            return false;
        }

    }
}



