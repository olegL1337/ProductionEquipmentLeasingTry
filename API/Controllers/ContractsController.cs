using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductionEquipmentLeasing.Application.DTOs;
using ProductionEquipmentLeasing.Application.Exceptions;
using ProductionEquipmentLeasing.Application.Interfaces.Services;

namespace ProductionEquipmentLeasing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;
    private readonly IMapper _mapper;

    public ContractsController(
        IContractService contractService,
        IMapper mapper)
    {
        _contractService = contractService;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContractDto>> GetById(Guid id)
    {
        var contract = await _contractService.GetByIdAsync(id);
        return Ok(contract);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContractDto>>> GetAll()
    {
        var contracts = await _contractService.GetAllAsync();
        return Ok(contracts);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContractDto dto)
    {
        var id = await _contractService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] UpdateContractDto dto)
    {
        await _contractService.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _contractService.DeleteAsync(id);
        return NoContent();
    }
}