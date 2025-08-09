using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class SiteSettings:BaseEntity
{
    public int CurrentWeek { get; set; }
    public int CurrentSeason { get; set; }
}