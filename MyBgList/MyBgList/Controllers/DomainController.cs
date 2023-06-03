using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBgList.DTO;
using MyBgList.Models;
using System.Diagnostics;
using System.Linq.Dynamic.Core;

namespace MyBgList.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DomainController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<DomainController> _logger;

    [HttpGet(Name = "GetDomains")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
    //[ManualValidationFilter]
    public async Task<ActionResult<RestDto<Domain[]>>> Get([FromQuery] RequestDto<DomainDto> input)
    {
        if (!ModelState.IsValid)
        {
            var details = new ValidationProblemDetails(ModelState);
            details.Extensions["traceId"] =
                Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            if (ModelState.Keys.Any(k => k == "PageSize"))
            {
                details.Type =
                    "https://tools.ietf.org/html/rfc7231#section-6.6.2";
                details.Status = StatusCodes.Status501NotImplemented;
                return new ObjectResult(details)
                {
                    StatusCode = StatusCodes.Status501NotImplemented
                };
            }
            else
            {
                details.Type =
                    "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                details.Status = StatusCodes.Status400BadRequest;
                return new BadRequestObjectResult(details);
            }
        }

        var query = _context.Domains.AsQueryable();
        if (!string.IsNullOrEmpty(input.FilterQuery))
            query = query.Where(b => b.Name.Contains(input.FilterQuery));
        var recordCount = await query.CountAsync();
        query = query
                .OrderBy($"{input.SortColumn} {input.SortOrder}")
                .Skip(input.PageIndex * input.PageSize)
                .Take(input.PageSize);

        return new RestDto<Domain[]>()
        {
            Data = await query.ToArrayAsync(),
            PageIndex = input.PageIndex,
            PageSize = input.PageSize,
            RecordCount = recordCount,
            Links = new List<LinkDto> {
                    new LinkDto(
                        Url.Action(
                            null,
                            "Domains",
                            new { input.PageIndex, input.PageSize },
                            Request.Scheme)!,
                        "self",
                        "GET"),
                }
        };
    }
}
