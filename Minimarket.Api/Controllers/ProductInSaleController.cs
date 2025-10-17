using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minimarket.Api.Responses;
using Minimarket.Core.Interfaces;
using Minimarket.Infraestructure.Validations;
using Minimarket.Infrastructure.Dtos;

namespace Minimarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductInSaleController : ControllerBase
    {
        private readonly IProductInSaleService _productInSaleService;
        public readonly IMapper _mapper;
        private readonly IValidatorService _validatorService;
        public ProductInSaleController(IProductInSaleService productInSaleService, IMapper mapper, IValidatorService validatorService)
        {
            _productInSaleService = productInSaleService;
            _mapper = mapper;
            _validatorService = validatorService;
        }
        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> getProductsInSaleDtoMapper(int idSale)
        {
            var productsInSale = await _productInSaleService.GetAllAsync(idSale);

            var productsInSaleDto = _mapper.Map<IEnumerable<ProductInSaleDto>>(productsInSale);
            var response = new ApiResponse<IEnumerable<ProductInSaleDto>>(productsInSaleDto);
            return Ok(response);
        }
    }
}
