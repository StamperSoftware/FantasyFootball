using Core.Entities;

namespace Core.Interfaces;

public interface IUserService
{
    public Task<AppUser> GetUser(string userId);
    public Task<IList<AppUser>> GetUsers();
}