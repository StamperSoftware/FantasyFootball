using Core.Entities;

namespace Core.Interfaces;

public interface IUserTeamService
{
    public Task<UserTeam?> GetUserTeamWithPlayer(int id);
}