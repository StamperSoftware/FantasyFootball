using Core.Entities;

namespace Core.Specifications;

public class LeagueSpecification : BaseSpecification<League>
{
    public LeagueSpecification(LeagueSpecParams specParams) : base(l =>
        string.IsNullOrWhiteSpace(specParams.Search) || l.Name.Contains(specParams.Search, StringComparison.CurrentCultureIgnoreCase))
    {
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        switch (specParams.Sort)
        {
            default:
                AddOrderBy(l => l.Name);
                break;
        }
    }
}