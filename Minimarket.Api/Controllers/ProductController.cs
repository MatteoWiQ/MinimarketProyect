    using AutoMapper;
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
    [Produces("application/json")]
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


        /// <summary>
        /// Recupera una lista paginada de productos como objetos de transferencia de datos (DTO) utilizando AutoMapper.
        /// </summary>
        /// <remarks>
        /// Este metodo se utiliza para obtener una lista de productos en formato DTO, lo que facilita la transferencia de datos entre el cliente y el servidor.
        /// </remarks>
        /// <param name="ProductQueryFilter">
        /// Los filtros se aplican a la consulta para limitar los resultados devueltos.
        /// </param>
        /// <returns>Retorna coleccion o liata de productos en formato DTO.
        /// </returns>
        /// <response code="200">Operacion exitosa.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> getProductsDtoMapper([FromQuery] ProductQueryFilter filter)
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
        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> getProductByIdDtoMapper(int id)
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
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, ex);
            }
        }

        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> updateProductDtoMapper(int id, ProductDto productDto)
        {
            try
            {

                if (id != productDto.Id)
                    throw new BussinesException("El ID del producto no coincide", (int)HttpStatusCode.BadRequest);
                else
                {
                    var product = await _productService.GetByIdAsync(id);
                    if (product == null)
                        throw new BussinesException("Usuario no encontrado", (int)HttpStatusCode.NotFound);
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
        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> deleteProductDtoMapper(int id)
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
        [HttpGet("dto/mapper/products-order-by-quantity-sold")]
        public async Task<IActionResult> getProductsOrderByQuantitySoldDtoMapper([FromQuery] ProductSoldQueryPaginationResponse filter)
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

        [HttpGet("dto/mapper/most-expensive-product")]
        public async Task<IActionResult> getMostExpensiveProductDtoMapper([FromQuery] ProductQueryFilter filter)
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


        [HttpGet("dto/mapper/products-that-never-sold")]
        public async Task<IActionResult> getProductsThatNeverSoldDtoMapper([FromQuery] ProductQueryFilter filter)
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
    }
}
