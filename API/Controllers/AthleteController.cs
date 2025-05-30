﻿using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AthleteController(IGenericRepository<Athlete> repo, IAthleteService service):BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AthleteDto>>> GetAthletes()
    {
        var athletes = await service.GetAthletesWithTeams();
        
        return Ok(athletes.Select(athlete => new AthleteDto(athlete)));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AthleteDto>> GetAthlete(int id)
    {
        return Ok(await repo.GetByIdAsync(id));
    }
    
}