using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minimarket.Api.Responses;
using Minimarket.Core.Interface;
using Minimarket.Infraestructure.Dtos;
using Minimarket.Infraestructure.Validations;

namespace Minimarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IValidatorService _validatorService;
        public ProductController(IProductService productService, IMapper mapper, IValidatorService validatorService)
        {
            _productService = productService;
            _mapper = mapper;
            _validatorService = validatorService;
        }
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> getProductsDtoMapper()
        {
            var products = await _productService.GetAllAsync();
            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
            var response = new ApiResponse<IEnumerable<ProductDto>>(productsDto);
            return Ok(response);
        }
    }
}
