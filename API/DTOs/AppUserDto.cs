using System.Diagnostics.CodeAnalysis;
using Core.Entities;

namespace API.DTOs;

public class AppUserDto
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
}