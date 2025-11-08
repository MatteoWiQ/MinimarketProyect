using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minimarket.Api.Responses;
using Minimarket.Core.CustomEntities;
using Minimarket.Core.Exceptions;
using Minimarket.Core.Interfaces;
using Minimarket.Core.QueryFilters;
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
        [HttpGet("dto/mapper/")]
        public async Task<IActionResult> getProductsInSaleDtoMapper([FromQuery] ProductInSaleQueryFilter filter )
        {
            try
            {
                var productsInSale = await _productInSaleService.GetAllAsync(filter);
                var productsInSaleDto = _mapper.Map<IEnumerable<ProductInSaleDto>>(productsInSale.Pagination);

                var pagination = new Pagination
                {
                    TotalCount = productsInSale.Pagination.TotalCount,
                    PageSize = productsInSale.Pagination.PageSize,
                    CurrentPage = productsInSale.Pagination.CurrentPage,
                    TotalPages = productsInSale.Pagination.TotalPages,
                    HasNextPage = productsInSale.Pagination.HasNextPage,
                    HasPreviousPage = productsInSale.Pagination.HasPreviousPage
                };
                var response = new ApiResponse<IEnumerable<ProductInSaleDto>>(productsInSaleDto)
                {
                    Pagination = pagination,
                    Messages = productsInSale.Messages
                };
                return StatusCode((int)productsInSale.StatusCode, response);
            }
            catch (Exception ex)
            {
                var responseProductInSale = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Error", Description = ex.Message } },
                };
                return StatusCode(400, responseProductInSale);
            }

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
            catch (BussinesException ex)
            {
                return StatusCode((int)ex.StatusCode, ex);
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
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, ex);
            }
        }
        [HttpDelete("dto/mapper/{idSale}/{idProduct}")]
        public async Task<IActionResult> DeleteProductInSale(int idSale, int idProduct)
        {
            
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
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, ex);
            }
        }
        #region Dapper Queries

        [HttpGet("all-details/")]
        public async Task<IActionResult> GetProductsInSaleDetailsDtoMapper([FromQuery] ProductInSaleDetailsPagination filter)
        {
            try
            {
                var productsInSaleDetails = await _productInSaleService.GetDeailsProductInSale(filter);
                var productsInSaleDetailsDto = _mapper.Map<IEnumerable<GetProductsInSaleDetailsResponse>>(productsInSaleDetails.Pagination);
                var pagination = new Pagination
                {
                    TotalCount = productsInSaleDetails.Pagination.TotalCount,
                    PageSize = productsInSaleDetails.Pagination.PageSize,
                    CurrentPage = productsInSaleDetails.Pagination.CurrentPage,
                    TotalPages = productsInSaleDetails.Pagination.TotalPages,
                    HasNextPage = productsInSaleDetails.Pagination.HasNextPage,
                    HasPreviousPage = productsInSaleDetails.Pagination.HasPreviousPage
                };
                var response = new ApiResponse<IEnumerable<GetProductsInSaleDetailsResponse>>(productsInSaleDetailsDto)
                {
                    Pagination = pagination,
                    Messages = productsInSaleDetails.Messages
                };
                return StatusCode((int)productsInSaleDetails.StatusCode, response);
            }
            catch (Exception ex)
            {
                var responseProductInSaleDetails = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Error", Description = ex.Message } },
                };
                return StatusCode(400, responseProductInSaleDetails);
            }
        }

        [HttpGet("product-quantity-by-sale/")]
        public async Task<IActionResult> GetProductQuantityBySaleDtoMapper([FromQuery] ProductQuantityBySalePagination filter)
        {
            try
            {
                var productQuantityBySale = await _productInSaleService.GetProductQuantityBySale(filter);
                var productQuantityBySaleDto = _mapper.Map<IEnumerable<ProductQuantityBySaleResponse>>(productQuantityBySale.Pagination);
                var pagination = new Pagination
                {
                    TotalCount = productQuantityBySale.Pagination.TotalCount,
                    PageSize = productQuantityBySale.Pagination.PageSize,
                    CurrentPage = productQuantityBySale.Pagination.CurrentPage,
                    TotalPages = productQuantityBySale.Pagination.TotalPages,
                    HasNextPage = productQuantityBySale.Pagination.HasNextPage,
                    HasPreviousPage = productQuantityBySale.Pagination.HasPreviousPage
                };
                var response = new ApiResponse<IEnumerable<ProductQuantityBySaleResponse>>(productQuantityBySaleDto)
                {
                    Pagination = pagination,
                    Messages = productQuantityBySale.Messages
                };
                return StatusCode((int)productQuantityBySale.StatusCode, response);
            }
            catch (Exception ex)
            {
                var responseProductQuantityBySale = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Error", Description = ex.Message } },
                };
                return StatusCode(400, responseProductQuantityBySale);
            }
        }

        [HttpGet("product-with-highest-quantity/")]
        public async Task<IActionResult> GetProductWithHighestQuantityInSaleDtoMapper()
        {
            try
            {
                var productWithHighestQuantityInSale = await _productInSaleService.GetProductWithHighestQuantityInSaleAsync();
                var productWithHighestQuantityInSaleDto = _mapper.Map<ProductWithHighestQuantityInSaleResponse>(productWithHighestQuantityInSale);
                var response = new ApiResponse<ProductWithHighestQuantityInSaleResponse>(productWithHighestQuantityInSaleDto);
                response.Messages = new Message[] { new() { Type = "Information", Description = "Producto con mayor cantidad en una venta recuperado correctamente" } };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var responseProductWithHighestQuantityInSale = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Error", Description = ex.Message } },
                };
                return StatusCode(400, responseProductWithHighestQuantityInSale);
            }
        }

        #endregion
    }
}
