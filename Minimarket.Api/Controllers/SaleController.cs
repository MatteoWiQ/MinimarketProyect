using AutoMapper;
using Minimarket.Api.Responses;
using Minimarket.Infrastructure.Dtos;
using Microsoft.AspNetCore.Mvc;
using Minimarket.Core.Interfaces;
using Minimarket.Infraestructure.Validations;
using Minimarket.Core.Data.Entities;

namespace Minimarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> getSalesDtoMapper()
        {
            var sales = await _saleService.GetAllAsync();
            var salesDto = _mapper.Map<IEnumerable<SaleDto>>(sales);
            var response = new ApiResponse<IEnumerable<SaleDto>>(salesDto);
            return Ok(response);
        }
        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> getSaleById(int id)
        {
            var sale = await _saleService.GetByIdAsync(id);
            var saleDto = _mapper.Map<SaleDto>(sale);
            var response = new ApiResponse<SaleDto>(saleDto);
            if (saleDto == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpPost("dto/mapper/")]
        public async Task<IActionResult> InsertSale([FromBody] SaleDto saleDto)
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