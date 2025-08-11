using Microsoft.AspNetCore.Mvc;
using ProductionEquipmentLeasing.Application.DTOs;
using ProductionEquipmentLeasing.Application.Interfaces.Services;

namespace ProductionEquipmentLeasing.API.Controllers;

[ApiController]
[Route("api/production-facilities")]
public class ProductionFacilitiesController : ControllerBase
{
    private readonly IProductionFacilityService _service;

    public ProductionFacilitiesController(IProductionFacilityService service)
        => _service = service;

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductionFacilityDto>> GetById(Guid id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductionFacilityDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateProductionFacilityDto dto)
        => Ok(await _service.CreateAsync(dto));

    [HttpPut]
    public async Task<IActionResult> Update(UpdateProductionFacilityDto dto)
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
