using Microsoft.AspNetCore.Mvc;
using MyBgList.Models;

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
}
