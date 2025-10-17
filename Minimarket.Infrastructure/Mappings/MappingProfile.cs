using AutoMapper;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.Dtos;
using Minimarket.Infraestructure.Dtos;
using Minimarket.Infrastructure.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Infraestructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<SaleDto, Sale>();
            CreateMap<Sale, SaleDto>();
            CreateMap<ProductInSale, ProductInSaleDto>();
            CreateMap<ProductInSaleDto, ProductInSale>();
        }
    }
}
