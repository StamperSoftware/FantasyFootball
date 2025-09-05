using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class GamesController(IGameService gameService):BaseApiController
{
    [HttpGet("{gameId}")]
    public async Task<ActionResult<GameDto>> GetGame(string gameId)
    {
        var game = await gameService.GetGame(gameId);
        
        if (game == null) return BadRequest("Could not get game");
        return Ok(game.Convert());
    }


    [HttpPut("{gameId}/finalize")]
    public async Task FinalizeGame(string gameId)
    {
        await gameService.FinalizeGameAsync(gameId);
    }
    [HttpPut("finalize")]
    public async Task FinalizeGames()
    {
        await gameService.FinalizeGamesAsync();
    }

    [HttpPut("{gameId}/score")]
    public async Task UpdateScores(string gameId)
    {
        await gameService.UpdateScoreAsync(gameId);
    }
}