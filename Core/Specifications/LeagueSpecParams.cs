namespace Core.Specifications;

public class LeagueSpecParams
{
    private const int MAX_PAGE_SIZE = 50;
    public int PageIndex { get; set; } = 1;
    private int _pageSize { get; set; } = 6;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value;
    }
    public string? Sort { get; set; }
    private string? _search { get; set; }

    public string Search
    {
        get => _search ?? "";
        set => _search = value.ToLower();
    }
}