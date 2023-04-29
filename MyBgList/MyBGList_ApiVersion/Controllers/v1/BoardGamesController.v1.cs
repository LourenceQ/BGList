using Microsoft.AspNetCore.Mvc;
using MyBGList_ApiVersion.DTO.v1;

namespace MyBGList_ApiVersion.Controllers.v1;

[Route("[controller]")]
[ApiController]
public class BoardGamesController.v1 : ControllerBase
{
    private readonly ILogger<BoardGamesController.v1> _logger;

public BoardGamesControllerv1(ILogger<BoardGamesControllerv1> logger)
{
    _logger = logger;
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
public RestDto<BoardGame[]> Get()
{
    return new RestDto<BoardGame[]>()
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
    };
}
}
