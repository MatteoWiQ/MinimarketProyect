using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minimarket.Api.Responses;
using Minimarket.Core.CustomEntities;
using Minimarket.Core.Exceptions;
using Minimarket.Core.Interfaces;
using Minimarket.Core.QueryFilters;
using Minimarket.Infraestructure.Validations;
using Minimarket.Infrastructure.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace Minimarket.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    public class ProductInSaleController : ControllerBase
    {
        private readonly IProductInSaleService _productInSaleService;
        private readonly IMapper _mapper;
        private readonly IValidatorService _validatorService;

        public ProductInSaleController(
            IProductInSaleService productInSaleService,
            IMapper mapper,
            IValidatorService validatorService)
        {
            _productInSaleService = productInSaleService;
            _mapper = mapper;
            _validatorService = validatorService;
        }

        // --------------------------------------------------------------------
        // GET: Obtener productos en ventas con paginación
        // --------------------------------------------------------------------
        [HttpGet("dto/mapper/")]
        [SwaggerOperation(
            Summary = "Obtener productos en ventas",
            Description = "Retorna un listado paginado de productos asociados a ventas usando Dto + AutoMapper."
        )]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductInSaleDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseData), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductsInSaleDtoMapper([FromQuery] ProductInSaleQueryFilter filter)
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
                return BadRequest(new ResponseData
                {
                    Messages = new[] { new Message { Type = "Error", Description = ex.Message } }
                });
            }
        }

        // --------------------------------------------------------------------
        // POST: Insertar producto en venta
        // --------------------------------------------------------------------
        [HttpPost("dto/mapper/")]
        [SwaggerOperation(
            Summary = "Insertar un producto dentro de una venta",
            Description = "Crea un registro producto-venta, valida datos y mapea con AutoMapper."
        )]
        [ProducesResponseType(typeof(ApiResponse<ProductInSaleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InsertProductInSale([FromBody] ProductInSaleDto productInSaleDto)
        {
            try
            {
                var validationResult = await _validatorService.ValidateAsync(productInSaleDto);
                if (!validationResult.IsValid)
                    return BadRequest(new { Errors = validationResult.Errors });

                var productInSale = _mapper.Map<Core.Data.Entities.ProductInSale>(productInSaleDto);
                await _productInSaleService.CreateAsync(productInSale);

                productInSaleDto.IdProduct = productInSale.IdProduct;

                return Ok(new ApiResponse<ProductInSaleDto>(productInSaleDto));
            }
            catch (BussinesException ex)
            {
                return StatusCode((int)ex.StatusCode, ex);
            }
        }

        // --------------------------------------------------------------------
        // PUT: Actualizar producto en venta
        // --------------------------------------------------------------------
        [HttpPut("dto/mapper")]
        [SwaggerOperation(
            Summary = "Actualizar un producto en una venta",
            Description = "Actualiza la información de un producto dentro de una venta específica."
        )]
        [ProducesResponseType(typeof(ApiResponse<ProductInSaleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProductInSale([FromBody] ProductInSaleDto productInSaleDto)
        {
            try
            {
                var validationResult = await _validatorService.ValidateAsync(productInSaleDto);
                if (!validationResult.IsValid)
                    return BadRequest(new { Errors = validationResult.Errors });

                var productInSale = _mapper.Map<Core.Data.Entities.ProductInSale>(productInSaleDto);
                var result = await _productInSaleService.UpdateAsync(productInSale);

                if (!result)
                    return NotFound();

                return Ok(new ApiResponse<ProductInSaleDto>(productInSaleDto));
            }
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, ex);
            }
        }

        // --------------------------------------------------------------------
        // DELETE: Borrar producto en venta
        // --------------------------------------------------------------------
        [HttpDelete("dto/mapper/{idSale}/{idProduct}")]
        [SwaggerOperation(
            Summary = "Eliminar un producto de una venta",
            Description = "Elimina la relación entre un producto y una venta usando sus identificadores."
        )]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProductInSale(int idSale, int idProduct)
        {
            try
            {
                var result = await _productInSaleService.DeleteAsync(idSale, idProduct);

                if (!result)
                    return NotFound();

                return Ok(new ApiResponse<bool>(result));
            }
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, ex);
            }
        }

        // ==================== DAPPER QUERIES ====================

        // --------------------------------------------------------------------
        // GET: Detalles de productos en ventas
        // --------------------------------------------------------------------
        [HttpGet("all-details/")]
        [SwaggerOperation(
            Summary = "Obtener detalles de productos en ventas",
            Description = "Consulta avanzada con Dapper que retorna información detallada de los productos vendidos."
        )]
        public async Task<IActionResult> GetProductsInSaleDetailsDtoMapper([FromQuery] ProductInSaleDetailsPagination filter)
        {
            try
            {
                var result = await _productInSaleService.GetDeailsProductInSale(filter);
                var dto = _mapper.Map<IEnumerable<GetProductsInSaleDetailsResponse>>(result.Pagination);

                var pagination = new Pagination
                {
                    TotalCount = result.Pagination.TotalCount,
                    PageSize = result.Pagination.PageSize,
                    CurrentPage = result.Pagination.CurrentPage,
                    TotalPages = result.Pagination.TotalPages,
                    HasNextPage = result.Pagination.HasNextPage,
                    HasPreviousPage = result.Pagination.HasPreviousPage
                };

                return StatusCode((int)result.StatusCode, new ApiResponse<IEnumerable<GetProductsInSaleDetailsResponse>>(dto)
                {
                    Pagination = pagination,
                    Messages = result.Messages
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseData
                {
                    Messages = new[] { new Message { Type = "Error", Description = ex.Message } }
                });
            }
        }

        // --------------------------------------------------------------------
        // GET: Cantidad de productos por venta
        // --------------------------------------------------------------------
        [HttpGet("product-quantity-by-sale/")]
        [SwaggerOperation(
            Summary = "Obtener cantidad de productos por venta",
            Description = "Devuelve cuántos productos hay en cada venta."
        )]
        public async Task<IActionResult> GetProductQuantityBySaleDtoMapper([FromQuery] ProductQuantityBySalePagination filter)
        {
            try
            {
                var result = await _productInSaleService.GetProductQuantityBySale(filter);
                var dto = _mapper.Map<IEnumerable<ProductQuantityBySaleResponse>>(result.Pagination);

                var pagination = new Pagination
                {
                    TotalCount = result.Pagination.TotalCount,
                    PageSize = result.Pagination.PageSize,
                    CurrentPage = result.Pagination.CurrentPage,
                    TotalPages = result.Pagination.TotalPages,
                    HasNextPage = result.Pagination.HasNextPage,
                    HasPreviousPage = result.Pagination.HasPreviousPage
                };

                return StatusCode((int)result.StatusCode, new ApiResponse<IEnumerable<ProductQuantityBySaleResponse>>(dto)
                {
                    Pagination = pagination,
                    Messages = result.Messages
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseData
                {
                    Messages = new[] { new Message { Type = "Error", Description = ex.Message } }
                });
            }
        }

        // --------------------------------------------------------------------
        // GET: Producto con mayor cantidad vendida
        // --------------------------------------------------------------------
        [HttpGet("product-with-highest-quantity/")]
        [SwaggerOperation(
            Summary = "Obtener producto con mayor cantidad en una venta",
            Description = "Retorna el producto que alcanzó la mayor cantidad dentro de una sola venta."
        )]
        public async Task<IActionResult> GetProductWithHighestQuantityInSaleDtoMapper()
        {
            try
            {
                var result = await _productInSaleService.GetProductWithHighestQuantityInSaleAsync();
                var dto = _mapper.Map<ProductWithHighestQuantityInSaleResponse>(result);

                return Ok(new ApiResponse<ProductWithHighestQuantityInSaleResponse>(dto)
                {
                    Messages = new[]
                    {
                        new Message
                        {
                            Type = "Information",
                            Description = "Producto con mayor cantidad recuperado correctamente."
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseData
                {
                    Messages = new[] { new Message { Type = "Error", Description = ex.Message } }
                });
            }
        }
    }
}
