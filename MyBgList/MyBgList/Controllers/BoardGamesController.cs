using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBgList.Constants;
using MyBgList.DTO;
using MyBgList.Models;
using System.Linq.Dynamic.Core;

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
    public async Task<RestDto<BoardGame[]>> Get([FromQuery] RequestDto<BoardGameDto> input)
    {
        _logger.LogInformation(CustomLogEvents.BoardGamesController_Get, "Get method started.");

        var query = _context.BoardGames.AsQueryable();

        if (!string.IsNullOrEmpty(input.FilterQuery))
            query = query.Where(b => b.Name.Contains(input.FilterQuery));

        query = query.OrderBy($"{input.SortColumn} {input.SortOrder}").Skip(input.PageIndex * input.PageSize).Take(input.PageSize);

        return new MyBgList.DTO.RestDto<BoardGame[]>()
        {
            Data = await query.ToArrayAsync(),
            PageIndex = input.PageIndex,
            PageSize = input.PageSize,
            RecordCount = await _context.BoardGames.CountAsync(),
            Links = new List<LinkDto>
            {
                new LinkDto(Url.Action(null, "BoardGames", new { input.PageIndex, input.PageSize}, Request.Scheme)!, "self", "GET")
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

    [HttpPost(Name = "UpdateBoardGame")]
    [ResponseCache(NoStore = true)]
    public async Task<RestDto<BoardGame?>> Post(BoardGameDto model)
    {
        var boardGame = await _context.BoardGames.Where(b => b.Id == model.Id)
            .FirstOrDefaultAsync();

        if (boardGame != null)
        {
            if (!string.IsNullOrEmpty(model.Name))
                boardGame.Name = model.Name;
            if (model.Year.HasValue && model.Year.Value > 0)
                boardGame.Year = model.Year.Value;

            boardGame.LastModifiedDate = DateTime.Now;
            _context.BoardGames.Update(boardGame);
            await _context.SaveChangesAsync();
        }

        return new RestDto<BoardGame?>()
        {
            Data = boardGame,
            Links = new List<LinkDto>
            {
                new LinkDto(Url.Action(null, "BoardGames", model, Request.Scheme)!, "self", "POST")
            }
        };
    }

    [HttpDelete(Name = "DeleteBoardGame")]
    [ResponseCache(NoStore = true)]
    public async Task<RestDto<BoardGame?>> Delete(int id)
    {
        var boardGame = await _context.BoardGames
            .Where(b => b.Id == id)
            .FirstOrDefaultAsync();
        if (boardGame != null)
        {
            _context.BoardGames.Remove(boardGame);
            await _context.SaveChangesAsync();
        }

        return new RestDto<BoardGame?>()
        {
            Data = boardGame,
            Links = new List<LinkDto>
            {
                new LinkDto(Url.Action(null, "BoardGames", id, Request.Scheme)!, "self", "DELETE")
            }
        };
    }
}
