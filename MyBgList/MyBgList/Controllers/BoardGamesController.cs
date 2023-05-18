using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBgList.DTO;
using MyBgList.Models;

namespace MyBgList.Controllers;

[Route("[controller]")]
[ApiController]
public class BoardGamesController : ControllerBase
{
    private readonly ILogger<BoardGamesController> _logger;
    private readonly ApplicationDbContext _context;

    public BoardGamesController(ILogger<BoardGamesController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    /*[HttpGet(Name = "GetBoardGames")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 6)]
    public IEnumerable<BoardGame> Get()
    {
        return new[]
        {
            new BoardGame()
            {
                Id = 1, Name = "Axis & Allies", Year = 1981
            },
            new BoardGame()
            {
                Id = 2, Name = "Citadels", Year = 2000
            },
            new BoardGame()
            {
                Id = 3, Name = "Terraforming Mars", Year = 2016
            }
        };
    }*/

    [HttpGet(Name = "GetBoardGames")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 6)]
    public async Task<RestDto<BoardGame[]>> Get(int pageIndex = 0, int pageSize = 10)
    {
        var query = _context.BoardGames.Skip(pageIndex * pageSize).Take(pageSize);
        return new MyBgList.DTO.RestDto<BoardGame[]>()
        {
            Data = await query.ToArrayAsync(),
            PageIndex = pageIndex,
            PageSize = pageSize,
            RecordCount = await _context.BoardGames.CountAsync(),
            Links = new List<LinkDto>
            {
                new LinkDto(Url.Action(null, "BoardGames", null, Request.Scheme)!, "self", "GET")
            }
        };

        #region old
        /*return new RestDto<BoardGame[]>()
        {

        
            Data = new BoardGame[]
            {
                new BoardGame()
                {
                    Id = 1,
                    Name = "Axis & Allies",
                    Year = 1981
                },
                new BoardGame()
                {
                    Id = 2,
                    Name = "Citadels",
                    Year = 2000
                },
                new BoardGame()
                {
                    Id = 3,
                    Name = "Terraforming Mars",
                    Year = 2016
                }
            },
            Links = new List<LinkDto>
            {
                new LinkDto(Url.Action(null, "BoardGames", null, Request.Scheme)!,
                    "self",
                    "GET"),
            }
        };*/
        #endregion

    }
}
