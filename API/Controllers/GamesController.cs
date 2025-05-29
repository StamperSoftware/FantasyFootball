using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class GamesController(IGenericRepository<Game> repo, IGameService gameService):BaseApiController
{
    [HttpGet("{gameId:int}")]
    public async Task<ActionResult<GameDto>> GetGame(int gameId)
    {
        var game = await gameService.GetFullDetailAsync(gameId);
        
        if (game == null) return BadRequest("Could not get game");
        return Ok(new GameDto(game));
    }
}