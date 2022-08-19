using CSV.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSV.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HourlyController : Controller
{
    private readonly ICsvReadFile service;
    private readonly IJupiterService jupiterService;
    private readonly IUpdateTerminalsService terminalService;
    public HourlyController(ICsvReadFile service, IJupiterService jupiterService, IUpdateTerminalsService terminalService)
    {
        this.service = service;
        this.jupiterService = jupiterService;
        this.terminalService = terminalService;
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
    
    [HttpGet]
    public async Task<IActionResult> getAll()
    {
        return Ok(await jupiterService.getDataFromJupiterOld());
    }
    
    [HttpPost("/updateTerminals")]
    public async Task<IActionResult> updateTerminals()
    {
        return Ok(await terminalService.UpdateTerminals());
    }
    
    [HttpPost("/updateTerminalsappend")]
    public async Task<IActionResult> updateTerminalsAppend()
    {
        return Ok(await terminalService.UpdateTerminalsAppend());
    }
    
    [HttpPost("/allinonejob")]
    public async Task<IActionResult> allinonejob()
    {
        await terminalService.AllInOneJob();
        return Ok();
    }
}