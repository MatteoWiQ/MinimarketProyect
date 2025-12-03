using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minimarket.Api.Responses;
using Minimarket.Core.CustomEntities;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.Dtos;
using Minimarket.Core.Enum;
using Minimarket.Core.Exceptions;
using Minimarket.Core.Interface;
using Minimarket.Core.QueryFilters;
using Minimarket.Core.Services;
using Minimarket.Infraestructure.Dtos;
using Minimarket.Infraestructure.Validations;
using System.Net;

namespace Minimarket.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IValidatorService _validatorService;
        public ProductController(IProductService productService, IMapper mapper, IValidatorService validatorService, IConfiguration configuration)
        {
            _productService = productService;
            _mapper = mapper;
            _validatorService = validatorService;
            _configuration = configuration;
        }

        /// <summary>
        /// Recupera una lista paginada de productos (DTO) usando AutoMapper.
        /// </summary>
        /// <remarks>
        /// Aplicar filtros de consulta para paginar, ordenar o filtrar por campos. 
        /// Devuelve un objeto ApiResponse con la lista y metadatos de paginación.
        /// </remarks>
        /// <param name="filter">Filtros de consulta (page, pageSize, name, brand, etc.).</param>
        /// <returns>ApiResponse con IEnumerable&lt;ProductDto&gt; y paginación.</returns>
        /// <response code="200">Operación exitosa, devuelve lista paginada.</response>
        /// <response code="400">Solicitud inválida o error en la entrada.</response>
        /// <response code="401">No autorizado. Token JWT inválido o expirado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("dto/mapper")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<ProductDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetProductsDtoMapper([FromQuery] ProductQueryFilter filter)
        {
            try
            {
                var products = await _productService.GetAllAsync(filter);

                var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products.Pagination);

                var pagination = new Pagination
                {
                    TotalCount = products.Pagination.TotalCount,
                    PageSize = products.Pagination.PageSize,
                    CurrentPage = products.Pagination.CurrentPage,
                    TotalPages = products.Pagination.TotalPages,
                    HasNextPage = products.Pagination.HasNextPage,
                    HasPreviousPage = products.Pagination.HasPreviousPage
                };
                var response = new ApiResponse<IEnumerable<ProductDto>>(productsDto)
                {
                    Pagination = pagination,
                    Messages = products.Messages
                };
                return StatusCode((int)products.StatusCode, response);
            }
            catch (Exception err)
            {
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = err.Message } },
                };
                return StatusCode(400, responsePost);
            }
        }

        /// <summary>
        /// Recupera un producto por su Id (DTO).
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>ApiResponse con ProductDto.</returns>
        /// <response code="200">Operación exitosa.</response>
        /// <response code="401">No autorizado. Token JWT inválido o expirado.</response>
        /// <response code="404">Producto no encontrado.</response>
        [HttpGet("dto/mapper/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<ProductDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetProductByIdDtoMapper(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                var productDto = _mapper.Map<ProductDto>(product);
                var response = new ApiResponse<ProductDto>(productDto);
                return Ok(response);

            }
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, ex);
            }
        }

        /// <summary>
        /// Inserta un nuevo producto (usando DTO).
        /// </summary>
        /// <param name="productDto">Objeto ProductDto con los datos del producto.</param>
        /// <returns>ApiResponse con el ProductDto insertado (Id ya asignado).</returns>
        /// <response code="200">Inserción exitosa.</response>
        /// <response code="400">Validación fallida.</response>
        /// <response code="401">No autorizado. Token JWT inválido o expirado.</response>
        /// <response code="409">Conflicto (ej. producto ya existe) si se lanza BussinesException.</response>
        [HttpPost("dto/mapper")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<ProductDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> InsertProductDtoMapper(ProductDto productDto)
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
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, ex);
            }
        }

        /// <summary>
        /// Actualiza un producto por Id (usando DTO).
        /// </summary>
        /// <param name="id">Id del producto a actualizar.</param>
        /// <param name="productDto">DTO con los datos a actualizar (debe contener el mismo Id).</param>
        /// <returns>ApiResponse con el producto actualizado.</returns>
        /// <response code="200">Actualización exitosa.</response>
        /// <response code="400">Id no coincide o solicitud inválida.</response>
        /// <response code="401">No autorizado. Token JWT inválido o expirado.</response>
        /// <response code="404">Producto no encontrado.</response>
        [HttpPut("dto/mapper/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<Product>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> UpdateProductDtoMapper(int id, ProductDto productDto)
        {
            try
            {
                if (id != productDto.Id)
                    throw new BussinesException("El ID del producto no coincide", (int)HttpStatusCode.BadRequest);
                else
                {
                    var product = await _productService.GetByIdAsync(id);
                    if (product == null)
                        throw new BussinesException("Producto no encontrado", (int)HttpStatusCode.NotFound);
                    _mapper.Map(productDto, product);
                    await _productService.UpdateAsync(product);

                    var response = new ApiResponse<Product>(product);
                    return Ok(response);
                }
            }
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, ex);
            }
        }

        /// <summary>
        /// Elimina un producto por Id.
        /// </summary>
        /// <param name="id">Id del producto a eliminar.</param>
        /// <response code="204">Eliminación exitosa (No Content).</response>
        /// <response code="401">No autorizado. Token JWT inválido o expirado.</response>
        /// <response code="404">Producto no encontrado.</response>
        [HttpDelete("dto/mapper/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> DeleteProductDtoMapper(int id)
        {
            try
            {
                await _productService.DeleteAsync(id);
                return NoContent();
            }
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, ex);
            }
        }

        #region Sql Queries

        /// <summary>
        /// Recupera productos ordenados por cantidad vendida (SQL query).
        /// </summary>
        /// <param name="filter">Filtros de paginación y búsqueda.</param>
        /// <returns>ApiResponse con IEnumerable&lt;GetProductsOrderByQuantitySoldResponse&gt;.</returns>
        /// <response code="200">Operación exitosa.</response>
        /// <response code="400">Solicitud inválida o error en la entrada.</response>
        /// <response code="401">No autorizado. Token JWT inválido o expirado.</response>
        [HttpGet("dto/mapper/products-order-by-quantity-sold")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<GetProductsOrderByQuantitySoldResponse>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetProductsOrderByQuantitySoldDtoMapper([FromQuery] ProductSoldQueryPaginationResponse filter)
        {
            try
            {
                var products = await _productService.GetProductsOrderByQuantitySoldAsync(filter);

                var productsDto = _mapper.Map<IEnumerable<GetProductsOrderByQuantitySoldResponse>>(products.Pagination);
                var pagination = new Pagination
                {
                    TotalCount = products.Pagination.TotalCount,
                    PageSize = products.Pagination.PageSize,
                    CurrentPage = products.Pagination.CurrentPage,
                    TotalPages = products.Pagination.TotalPages,
                    HasNextPage = products.Pagination.HasNextPage,
                    HasPreviousPage = products.Pagination.HasPreviousPage
                };
                var response = new ApiResponse<IEnumerable<GetProductsOrderByQuantitySoldResponse>>(productsDto)
                {
                    Pagination = pagination,
                    Messages = products.Messages
                };

                return StatusCode((int)products.StatusCode, response);
            }
            catch (Exception err)
            {
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = err.Message } },
                };
                return StatusCode(400, responsePost);
            }
        }

        /// <summary>
        /// Recupera el producto más caro.
        /// </summary>
        /// <param name="filter">Filtros opcionales.</param>
        /// <returns>ApiResponse con ProductQueriesResponse.</returns>
        /// <response code="200">Operacion exitosa.</response>
        /// <response code="400">Solicitud inválida o error en la entrada.</response>
        /// <response code="401">No autorizado. Token JWT inválido o expirado.</response>
        [HttpGet("dto/mapper/most-expensive-product")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<ProductQueriesResponse>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetMostExpensiveProductDtoMapper([FromQuery] ProductQueryFilter filter)
        {
            try
            {
                var product = await _productService.GetMostExpensiveProductAsync(filter);
                var productDto = _mapper.Map<ProductQueriesResponse>(product);
                var response = new ApiResponse<ProductQueriesResponse>(productDto);
                response.Messages = new Message[] { new() { Type = TypeMessage.information.ToString(), Description = "Producto más caro recuperado correctamente" } };
                return Ok(response);
            }
            catch (Exception err)
            {
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = err.Message } },
                };
                return StatusCode(400, responsePost);
            }
        }

        /// <summary>
        /// Recupera los productos que nunca se vendieron.
        /// </summary>
        /// <param name="filter">Filtros de consulta/paginación.</param>
        /// <returns>ApiResponse con IEnumerable&lt;ProductQueriesResponse&gt;.</returns>
        /// <response code="200">Operacion exitosa.</response>
        /// <response code="400">Solicitud inválida o error en la entrada.</response>
        /// <response code="401">No autorizado. Token JWT inválido o expirado.</response>
        [HttpGet("dto/mapper/products-that-never-sold")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<ProductQueriesResponse>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetProductsThatNeverSoldDtoMapper([FromQuery] ProductQueryFilter filter)
        {
            try
            {
                var products = await _productService.GetProductsThatNeverSoldAsync(filter);
                var productsDto = _mapper.Map<IEnumerable<ProductQueriesResponse>>(products.Pagination);
                var pagination = new Pagination
                {
                    TotalCount = products.Pagination.TotalCount,
                    PageSize = products.Pagination.PageSize,
                    CurrentPage = products.Pagination.CurrentPage,
                    TotalPages = products.Pagination.TotalPages,
                    HasNextPage = products.Pagination.HasNextPage,
                    HasPreviousPage = products.Pagination.HasPreviousPage
                };
                var response = new ApiResponse<IEnumerable<ProductQueriesResponse>>(productsDto)
                {
                    Pagination = pagination,
                    Messages = products.Messages
                };
                return StatusCode((int)products.StatusCode, response);
            }
            catch (Exception err)
            {
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = err.Message } },
                };
                return StatusCode(400, responsePost);
            }
        }
        #endregion

        [HttpGet("Test")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Test()
        {
            var result = new
            {
                connectionSqlServer = _configuration.GetConnectionString("SqlServerConnection") != null,
            };
            return Ok(result);
        }
    }
}
