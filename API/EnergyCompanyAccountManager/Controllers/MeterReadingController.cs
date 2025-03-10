using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using POCOs;

namespace EnergyCompanyAccountManager.Controllers;

[ApiController]
[Route("meter-reading-uploads")]
public class MeterReadingController : ControllerBase
{
    private readonly IMeterReadingService _service;

    public MeterReadingController(IMeterReadingService service)
    {
        _service = service;
    }


    [HttpGet(Name = "")]
    public IActionResult Alive()
    {

        return new OkObjectResult("Alive");
    }

    [HttpPost(Name = "")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return new BadRequestObjectResult("The file upload is empty");
        }

        try
        {
            using (var stream = file.OpenReadStream())
            {
                UploadResult result = await _service.Upload(stream);
                return new OkObjectResult(result);
            }
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
    }
}
