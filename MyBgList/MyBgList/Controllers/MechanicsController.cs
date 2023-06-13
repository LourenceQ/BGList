using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBgList.DTO;
using MyBgList.Models;
using System.Linq.Dynamic.Core;

namespace MyBgList.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MechanicsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<MechanicsController> _logger;

    public MechanicsController(ILogger<MechanicsController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet(Name = "GetMechanics")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
    public async Task<RestDto<Mechanic[]>> Get([FromQuery] RequestDto<MechanicDto> input)
    {
        var query = _context.Mechanics.AsQueryable();
        if (!string.IsNullOrEmpty(input.FilterQuery))
            query = query.Where(b => b.Name.Contains(input.FilterQuery));
        var recordCount = await query.CountAsync();
        query = query
                .OrderBy($"{input.SortColumn} {input.SortOrder}")
                .Skip(input.PageIndex * input.PageSize)
                .Take(input.PageSize);

        return new RestDto<Mechanic[]>()
        {
            Data = await query.ToArrayAsync(),
            PageIndex = input.PageIndex,
            PageSize = input.PageSize,
            RecordCount = recordCount,
            Links = new List<LinkDto> {
                    new LinkDto(
                        Url.Action(
                            null,
                            "Mechanics",
                            new { input.PageIndex, input.PageSize },
                            Request.Scheme)!,
                        "self",
                        "GET"),
                }
        };
    }

    [HttpPost(Name = "UpdateMechanic")]
    [ResponseCache(NoStore = true)]
    public async Task<RestDto<Mechanic?>> Post(MechanicDto model)
    {
        var mechanic = await _context.Mechanics.Where(b => b.Id == model.Id)
            .FirstOrDefaultAsync();
        if (mechanic != null)
        {
            if (!string.IsNullOrEmpty(model.Name))
                mechanic.Name = model.Name;

            mechanic.LastModifiedDate = DateTime.Now;
            _context.Mechanics.Update(mechanic);

            await _context.SaveChangesAsync();
        }

        return new RestDto<Mechanic?>()
        {
            Data = mechanic,
            Links = new List<LinkDto>
            {
                new LinkDto(Url.Action(null, "Mechanics", model, Request.Scheme)!, "self", "POST")
            }
        };
    }

    [HttpDelete(Name = "DeleteMechanic")]
    [ResponseCache(NoStore = true)]
    public async Task<RestDto<Mechanic?>> Delete(int id)
    {
        var mechanic = await _context.Mechanics.Where(b => b.Id == id)
            .FirstOrDefaultAsync();

        if (mechanic != null)
        {
            _context.Mechanics.Remove(mechanic);
            await _context.SaveChangesAsync();
        }

        return new RestDto<Mechanic?>()
        {
            Data = mechanic,
            Links = new List<LinkDto>
            {
                new LinkDto(Url.Action(null, "Mechanics", id, Request.Scheme)!, "self", "DELETE")
            }
        };
    }
}
