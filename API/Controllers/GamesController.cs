using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class GamesController(IGameService gameService):BaseApiController
{
    [HttpGet("{gameId:int}")]
    public async Task<ActionResult<GameDto>> GetGame(int gameId)
    {
        var game = await gameService.GetGame(gameId);
        
        if (game == null) return BadRequest("Could not get game");
        return Ok(game.Convert());
    }


    [HttpPut("{gameId:int}/finalize")]
    public async Task FinalizeGame(int gameId)
    {
        await gameService.FinalizeGameAsync(gameId);
    }
    [HttpPut("finalize")]
    public async Task FinalizeGames()
    {
        await gameService.FinalizeGamesAsync();
    }

    [HttpPut("{gameId:int}/score")]
    public async Task UpdateScores(int gameId)
    {
        await gameService.UpdateScoreAsync(gameId);
    }
}