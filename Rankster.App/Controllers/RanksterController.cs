using Microsoft.AspNetCore.Mvc;
using Rankster.Data.Enums;

namespace Rankster.App.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RanksterController : ControllerBase
{
    private readonly ILogger<RanksterController> _logger;
    private readonly IRanksterService _ranksterService;

    public RanksterController(ILogger<RanksterController> logger, IRanksterService rankerService)
    {
        _logger = logger;
        _ranksterService = rankerService;
    }

    [HttpGet("{code}")]
    public async Task<RanksterModel?> GetRankster(string code)
    {
        var temp = await _ranksterService.GetRankster(code);
        return temp;
    }
}
