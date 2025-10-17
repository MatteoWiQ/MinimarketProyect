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
        public async Task<IActionResult> getProductsInSaleDtoMapper(int id)
        {
            var productsInSale = await _productInSaleService.GetAllAsync(id);

            var productsInSaleDto = _mapper.Map<IEnumerable<ProductInSaleDto>>(productsInSale);
            var response = new ApiResponse<IEnumerable<ProductInSaleDto>>(productsInSaleDto);
            return Ok(response);
        }
        [HttpPost("dto/mapper/")]
        public async Task<IActionResult> InsertProductInSale([FromBody] ProductInSaleDto productInSaleDto)
        {
            try
            {

                var validationResult = await _validatorService.ValidateAsync(productInSaleDto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }
                var productInSale = _mapper.Map<Core.Data.Entities.ProductInSale>(productInSaleDto);
                await _productInSaleService.CreateAsync(productInSale);
                productInSaleDto.IdProduct = productInSale.IdProduct;
                var response = new ApiResponse<ProductInSaleDto>(productInSaleDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Errors = ex.Message });
            }
        }

        [HttpPut("dto/mapper")]
        public async Task<IActionResult> UpdateProductInSale([FromBody] ProductInSaleDto productInSaleDto)
        {
            try
            {

                var validationResult = await _validatorService.ValidateAsync(productInSaleDto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }
                var productInSale = _mapper.Map<Core.Data.Entities.ProductInSale>(productInSaleDto);
                var result = await _productInSaleService.UpdateAsync(productInSale);
                if (!result)
                {
                    return NotFound();
                }
                var response = new ApiResponse<ProductInSaleDto>(productInSaleDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Errors = ex.Message });
            }
        }
        [HttpDelete("dto/mapper/{idSale}/{idProduct}")]
        public async Task<IActionResult> DeleteProductInSale(int idSale, int idProduct)
        {
            // eliminar por id sale y id product
            try
            {
                var result = await _productInSaleService.DeleteAsync(idSale, idProduct);
                if (!result)
                {
                    return NotFound();
                }
                var response = new ApiResponse<bool>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Errors = ex.Message });
            }
        }
    }
}
