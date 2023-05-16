using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBgList.Models;
using MyBgList.Models.Csv;
using System.Globalization;

namespace MyBgList.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeedController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<SeedController> _logger;

    private readonly IWebHostEnvironment _env;

    public SeedController(ApplicationDbContext context
        , ILogger<SeedController> logger
        , IWebHostEnvironment env)
    {
        _context = context;
        _logger = logger;
        _env = env;
    }

    [HttpPut(Name = "Seed")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> Put()
    {
        var config = new CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR"))
        {
            HasHeaderRecord = true,
            Delimiter = ";"
        };

        using var reader = new StreamReader(System.IO.Path.Combine(_env.ContentRootPath, "Data/bgg_dataset.csv"));
        using var csv = new CsvReader(reader, config);

        var existingBoardGames = await _context.BoardGames.ToDictionaryAsync(bg => bg.Id);
        var existingDomains = await _context.Domains.ToDictionaryAsync(d => d.Name);
        var existingMechanics = await _context.Mechanics.ToDictionaryAsync(m => m.Name);

        var now = DateTime.Now;

        var records = csv.GetRecords<BggRecord>();
        var skippedRows = 0;

        foreach (var record in records)
        {
            if (!record.ID.HasValue
                    || string.IsNullOrEmpty(record.Name)
                    || existingBoardGames.ContainsKey(record.ID.Value))
            {
                skippedRows++;
                continue;
            }
        }


        return new JsonResult(new
        {
            BoardGames = _context.BoardGames.Count(),
            Domains = _context.Domains.Count(),
            Mechanics = _context.Mechanics.Count(),
            SkippedRows = skippedRows
        });
    }
}