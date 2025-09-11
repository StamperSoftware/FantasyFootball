using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PlayersController(IPlayerService playerService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PlayerDto>>> GetPlayersAsync()
    {
        var players = await playerService.GetPlayers();
        return Ok(players.Select(p=>p.Convert()));
    }

    [HttpGet("{playerId:int}")]
    public async Task<ActionResult<PlayerDto>> GetPlayerAsync(int playerId)
    {
        try
        {
            var player = await playerService.GetPlayer(playerId);
            return Ok(player.Convert());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}