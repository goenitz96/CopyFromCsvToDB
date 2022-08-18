using CSV.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSV.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HourlyController : Controller
{
    private readonly ICsvReadFile service;
    public HourlyController(ICsvReadFile service)
    {
        this.service = service;
    }
    
    [HttpPost]
    public async Task<IActionResult> insertAll()
    {
        return Ok(await service.saveTheData());
    }
    
    [HttpPost("/insertData")]
    public async Task<IActionResult> insertDataIntoHourlyBilling()
    {
        return Ok(service.insertIntoHourlyBilling());
    }
}