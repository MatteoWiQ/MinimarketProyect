using AutoMapper;
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
        [HttpGet("dto/mapper")]
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
        [HttpGet("dto/mapper/{id}")]
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
        [HttpPost("dto/mapper/")]
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
        [HttpPut("dto/mapper")]
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