﻿using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountsController(SignInManager<AppUser> signInManager, IGenericRepository<Player> playerRepo) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult> CreateUser(RegisterDto registerDto)
    {
        
        AppUser user = new(registerDto.Email, registerDto.UserName);

        var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            Player newPlayer = new(registerDto.FirstName, registerDto.LastName, user);
            playerRepo.Add(newPlayer);
            
            if (!await playerRepo.SaveAllAsync()) return BadRequest("App User was created but could not create Player");
            return Ok();
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem();
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