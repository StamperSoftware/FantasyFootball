using API.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "SiteAdmin")]
public class UsersController(IUserService service) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<IList<AppUserDto>>> GetUsers()
    {
        var users = await service.GetUsers();
        return Ok(users);
    }
    
    
    [HttpGet("{userId}")]
    public async Task<ActionResult<AppUserDto>> GetUser(string userId)
    {
        try
        {
            var user = await service.GetUser(userId);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error getting user, {ex.Message}");
        }
    }
}