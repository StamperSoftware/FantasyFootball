using Core.Entities;

namespace Core.Interfaces;

public interface IScheduleService
{
    public Task CreateSchedule(League league);
}