using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class TeamsController(IGenericRepository<Team> teams):BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<TeamDto>> GetTeams()
    {
        var response = await teams.ListAllAsync();
        return Ok(response.Select(t => t?.Convert()));
    }
}