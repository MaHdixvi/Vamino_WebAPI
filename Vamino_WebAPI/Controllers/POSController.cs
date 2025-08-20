using Core.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Vamino_WebAPI.Controllers;
public class POSController : SiteBaseController
{
    private readonly IPOSService _posService;

    public POSController(IPOSService posService)
    {
        _posService = posService;
    }

    [HttpPost("payment")]
    public async Task<IActionResult> ProcessPayment([FromBody] POSPaymentRequest request)
    {
        var result = await _posService.ProcessPayment(request);
        return Ok(result);
    }
}