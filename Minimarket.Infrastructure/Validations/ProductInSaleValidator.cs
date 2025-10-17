using FluentValidation;
using Minimarket.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Infrastructure.Validations
{
    public class ProductInSaleValidator : AbstractValidator<ProductInSale>
    {
        public ProductInSaleValidator() { }

        public void ValidateProductInSale(ProductInSale productInSale)
        {
            RuleFor(pis => pis.IdSale).NotEmpty().WithMessage("El Id de la venta no puede estar vacío.");
            RuleFor(pis => pis.IdProduct).NotEmpty().WithMessage("El Id del producto no puede estar vacío.");
            RuleFor(pis => pis.Quantity).GreaterThan(0).WithMessage("La cantidad debe ser mayor que cero.");
            RuleFor(pis => pis.UnitPrice).GreaterThan(0).WithMessage("El precio debe ser mayor que cero.");
        }
    }
}
