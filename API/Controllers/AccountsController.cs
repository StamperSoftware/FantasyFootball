using System.Net;
using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountsController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IGenericRepository<Player> playerRepo, IEmailSender<AppUser> userEmailSender) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult> CreateUser(RegisterDto registerDto)
    {
        
        AppUser user = new(registerDto.Email, registerDto.UserName);

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            Player newPlayer = new(registerDto.FirstName, registerDto.LastName, user);
            playerRepo.Add(newPlayer);
            
            if (!await playerRepo.SaveAllAsync()) return BadRequest("App User was created but could not create Player");

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmLink = Url.Action("ConfirmEmail", "accounts", new { email = user.Email, token=WebUtility.UrlEncode(token) },
                Request.Scheme) ?? throw new Exception("Could not create confirm link");
            
            await userEmailSender.SendConfirmationLinkAsync(user, registerDto.Email,confirmLink);
            return Ok();
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem();
    }
    
    [HttpGet("{email}/confirm/{token}")]
    public async Task ConfirmEmail(string email, string token)
    {
        var user = await userManager.FindByEmailAsync(email) ?? throw new Exception("Could not get user");
        await userManager.ConfirmEmailAsync(user, WebUtility.UrlDecode(token));
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return NoContent();
    }

    [HttpGet("user-info")]
    public async Task<ActionResult<AppUser>> GetUserInfo()
    {
        if (User.Identity?.IsAuthenticated == false) return NoContent();
        var user = await signInManager.UserManager.GetUserByEmail(User);

        return Ok(new AppUserDto(user));
    }

    [HttpGet("auth-status")]
    public ActionResult GetAuthStatus()
    {
        return Ok(new {isAuthenticated = User?.Identity?.IsAuthenticated ?? false});
    }
}