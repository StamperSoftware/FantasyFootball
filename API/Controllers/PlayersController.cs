using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PlayersController(IPlayerService playerService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Player>>> GetPlayersAsync()
    {
        var players = await playerService.GetPlayers();
        return Ok(players);
    }

    [HttpGet("{playerId:int}")]
    public async Task<ActionResult<Player>> GetPlayerAsync(int playerId)
    {
        try
        {
            var player = await playerService.GetPlayer(playerId);
            return Ok(player);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}