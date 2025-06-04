namespace API.DTOs;

public class SubmitDraftRequest
{
    public IList<Request> Results { get; set; } = [];
}

public class Request
{
    public int TeamId { get; set; }
    public required IList<int> Athletes { get; set; }
}