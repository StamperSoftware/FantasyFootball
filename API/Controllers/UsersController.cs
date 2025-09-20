using API.DTOs;
using API.Extensions;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UsersController(IUserService service) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<IList<AppUserDto>>> GetUsers()
    {
        var users = await service.GetConfirmedAppUsers();
        return Ok(users.Select(u => u.Convert()));
    }
    
    
    [HttpGet("{userId}")]
    public async Task<ActionResult<AppUserDto>> GetUser(string userId)
    {
        try
        {
            var user = await service.GetUser(userId);
            return Ok(user.Convert());
        }
        catch (Exception ex)
        {
            return BadRequest($"Error getting user, {ex.Message}");
        }
    }
}