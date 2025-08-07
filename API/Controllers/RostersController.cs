using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class RostersController(IRosterService rosterService):BaseApiController
{
    [HttpGet("{id}")]
    public async Task<Roster?> GetRoster(string id)
    {
        return await rosterService.GetRoster(id);
    }
}