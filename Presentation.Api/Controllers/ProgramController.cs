using Application.Infrastructure.Filters;
using Application.Infrastructure.Models.Request;
using Application.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProgramController(IProgramFormService programFormService) : ControllerBase
{
    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<CreateProgramForm>))]
    public async Task<IActionResult> CreateProgram([FromBody] CreateProgramForm request)
    {
        var response = await programFormService.CreateProgram(request);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPut]
    [ServiceFilter(typeof(ValidationFilter<UpdateProgramForm>))]
    public async Task<IActionResult> UpdateProgram([FromBody] UpdateProgramForm request)
    {
        var response = await programFormService.UpdateProgram(request);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPost("submit")]
    [ServiceFilter(typeof(ValidationFilter<CreateProgramFormSubmission>))]
    public async Task<IActionResult> SubmitProgramApplication([FromBody] CreateProgramFormSubmission request)
    {
        var response = await programFormService.SubmitApplication(request);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet("{formId}")]
    public async Task<IActionResult> GetProgram(string formId)
    {
        var response = await programFormService.GetProgram(formId);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPrograms()
    {
        var response = await programFormService.GetAllPrograms();
        return StatusCode((int)response.StatusCode, response);
    }
}
