using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minimarket.Api.Responses;
using Minimarket.Core.CustomEntities;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.Dtos;
using Minimarket.Core.Enum;
using Minimarket.Core.Exceptions;
using Minimarket.Core.Interfaces;
using Minimarket.Core.QueryFilters;
using Minimarket.Infraestructure.Validations;
using Minimarket.Infrastructure.Dtos;

namespace Minimarket.Api.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar las ventas (Sale).
    /// Requiere autenticación con JWT.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly IMapper _mapper;
        private readonly IValidatorService _validatorService;

        public SaleController(ISaleService saleService, IMapper mapper, IValidatorService validatorService)
        {
            _saleService = saleService;
            _mapper = mapper;
            _validatorService = validatorService;
        }

        /// <summary>
        /// Obtiene todas las ventas paginadas, aplicando filtros.
        /// </summary>
        /// <param name="filters">Filtros de búsqueda para ventas.</param>
        /// <returns>Listado paginado de ventas.</returns>
        /// <response code="200">Listado obtenido correctamente.</response>
        /// <response code="400">Error en filtros o datos enviados.</response>
        /// <response code="500">Error interno inesperado.</response>
        [HttpGet("dto/mapper")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SaleDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getSalesDtoMapper([FromQuery] SaleQueryFilter filters)
        {
            try
            {
                var sales = await _saleService.GetAllAsync(filters);

                var salesDto = _mapper.Map<IEnumerable<SaleDto>>(sales.Pagination);

                var pagination = new Pagination
                {
                    TotalCount = sales.Pagination.TotalCount,
                    PageSize = sales.Pagination.PageSize,
                    CurrentPage = sales.Pagination.CurrentPage,
                    TotalPages = sales.Pagination.TotalPages,
                    HasNextPage = sales.Pagination.HasNextPage,
                    HasPreviousPage = sales.Pagination.HasPreviousPage
                };

                var response = new ApiResponse<IEnumerable<SaleDto>>(salesDto)
                {
                    Pagination = pagination,
                    Messages = sales.Messages
                };

                return StatusCode((int)sales.StatusCode, response);
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
        /// Obtiene una venta por su ID.
        /// </summary>
        /// <param name="id">ID de la venta.</param>
        /// <returns>La venta solicitada.</returns>
        /// <response code="200">Venta encontrada.</response>
        /// <response code="404">La venta no existe.</response>
        /// <response code="500">Error interno.</response>
        [HttpGet("dto/mapper/{id}")]
        [ProducesResponseType(typeof(ApiResponse<SaleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getSaleById(int id)
        {
            try
            {
                var sale = await _saleService.GetByIdAsync(id);
                var saleDto = _mapper.Map<SaleDto>(sale);
                var response = new ApiResponse<SaleDto>(saleDto);

                return Ok(response);
            }
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, new { Errors = ex.Message });
            }
        }

        /// <summary>
        /// Inserta una nueva venta.
        /// </summary>
        /// <param name="saleDto">Datos de la venta.</param>
        /// <returns>Venta registrada.</returns>
        /// <response code="200">Venta creada exitosamente.</response>
        /// <response code="400">Error de validación.</response>
        /// <response code="500">Error interno.</response>
        [HttpPost("dto/mapper/")]
        [ProducesResponseType(typeof(ApiResponse<SaleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InsertSale([FromBody] SaleDto saleDto)
        {
            try
            {
                var validationResult = await _validatorService.ValidateAsync(saleDto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }

                var sale = _mapper.Map<Sale>(saleDto);
                await _saleService.InsertAsync(sale);

                saleDto.Id = sale.Id;
                var response = new ApiResponse<SaleDto>(saleDto);

                return Ok(response);
            }
            catch (BussinesException ex)
            {
                return StatusCode(ex.StatusCode, new { Errors = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza una venta existente.
        /// </summary>
        /// <param name="saleDto">Datos actualizados.</param>
        /// <returns>Venta modificada.</returns>
        /// <response code="200">Venta actualizada correctamente.</response>
        /// <response code="400">Error de validación.</response>
        /// <response code="500">Error interno.</response>
        [HttpPut("dto/mapper")]
        [ProducesResponseType(typeof(ApiResponse<SaleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSale([FromBody] SaleDto saleDto)
        {
            var validationResult = await _validatorService.ValidateAsync(saleDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors });
            }

            var sale = _mapper.Map<Sale>(saleDto);
            await _saleService.UpdateAsync(sale);

            var response = new ApiResponse<SaleDto>(saleDto);
            return Ok(response);
        }
    }
}
