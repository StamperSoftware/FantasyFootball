using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PlayersController(IGenericRepository<Player> repo) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Player>>> GetPlayersAsync()
    {
        var players = await repo.ListAllAsync();
        return Ok(players);
    }

    [HttpGet("{playerId:int}")]
    public async Task<ActionResult<Player>> GetPlayerAsync(int playerId)
    {
        var player = await repo.GetByIdAsync(playerId);
        if (player == null) return BadRequest($"Could not get player with id {playerId}");
        return Ok(player);
    }
}