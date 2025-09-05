using Core.Entities;

namespace Infrastructure.Factories;

public static class GameFactory
{
    public static Game CreateGame(UserTeam home, UserTeam away, int week, int season, int leagueId)
    {
        return Game.CreateGame(home, away, week, season, leagueId);
    }
}