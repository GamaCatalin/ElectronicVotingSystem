namespace EVT_Server.Models.Election{

public class CandidateOption : BaseEntity
{
    public string Title { get; set; } = "";
    public string Address { get; set; } = "";
    public string Picture { get; set; } = "";
    public string Party { get; set; } = "";
    public int VoteCount { get; set; } = 0;
    public Guid ElectionId { get; set; } = Guid.Empty;

    public override string ToString()
    {
        return $"Candidate{{Title: {Title}, Address: {Address}, Picture: {Picture}, Party: {Party}, ElectionId: {ElectionId}, VoteCount: {VoteCount}}}";
    }
}
}