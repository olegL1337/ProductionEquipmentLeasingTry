using Microsoft.AspNetCore.Mvc;
using ProductionEquipmentLeasing.Application.DTOs;
using ProductionEquipmentLeasing.Application.Interfaces.Services;

namespace ProductionEquipmentLeasing.API.Controllers;

[ApiController]
[Route("api/equipment-types")]
public class EquipmentTypesController : ControllerBase
{
    private readonly IEquipmentTypeService _service;

    public EquipmentTypesController(IEquipmentTypeService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EquipmentTypeDto>> GetById(Guid id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EquipmentTypeDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateEquipmentTypeDto dto)
        => Ok(await _service.CreateAsync(dto));

    [HttpPut]
    public async Task<IActionResult> Update(UpdateEquipmentTypeDto dto)
    {
        await _service.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
