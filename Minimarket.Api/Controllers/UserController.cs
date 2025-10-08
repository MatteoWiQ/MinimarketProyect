using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Minimarket.Api.Responses;
//using Minimarket.Core.CustomEntities;
//using Minimarket.Core.Entities;
//using Minimarket.Core.Interfaces;
//using Minimarket.Infrastructure.DTOs;
//using Minimarket.Infrastructure.Validators;
namespace Minimarket.Api.Controllers
{
    [Route("api/[controller]")]
    private readonly MinimarketContext _ctx;
    public TestController(MinimarketContext ctx) => _ctx = ctx;

    [HttpGet("ping")]
    public async Task<IActionResult> Ping()
    {
        if (!await _ctx.Database.CanConnectAsync())
            return StatusCode(500, "No se pudo conectar a la DB.");

        var products = await _ctx.Products.Take(5).ToListAsync();
        return Ok(new { db = "ok", sampleProducts = products });
    }
}