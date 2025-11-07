    using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minimarket.Api.Responses;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.Dtos;
using Minimarket.Core.Interface;
using Minimarket.Core.Services;
using Minimarket.Infraestructure.Dtos;
using Minimarket.Infraestructure.Validations;
using System.Net;

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
        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> getProductByIdDtoMapper(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            var productDto = _mapper.Map<ProductDto>(product);
            var response = new ApiResponse<ProductDto>(productDto);
            return Ok(response);
        }
        [HttpPost("dto/mapper")]
        public async Task<IActionResult> insertProductDtoMapper(ProductDto productDto)
        {
            try
            {
                var validationResult = await _validatorService.ValidateAsync(productDto);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }

                var product = _mapper.Map<Product>(productDto);
                await _productService.InsertAsync(product);
                productDto.Id = product.Id;
                var response = new ApiResponse<ProductDto>(productDto);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> updateProductDtoMapper(int id, ProductDto productDto)
        {
            if (id != productDto.Id)
                return BadRequest("El ID del producto no coincide");
            else
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                    return NotFound("Usuario no encontrado");
                _mapper.Map(productDto, product);
                await _productService.UpdateAsync(product);

                var response = new ApiResponse<Product>(product);
                return Ok(response);
            }
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> deleteProductDtoMapper(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}