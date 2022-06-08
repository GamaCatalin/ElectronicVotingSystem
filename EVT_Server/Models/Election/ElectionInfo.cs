namespace EVT_Server.Models.Election{

public class ElectionInfo : BaseEntity
{
    public string ElectionName { get; set; } = "";
    public List<CandidateOption> Options { get; set; } = new List<CandidateOption>(Array.Empty<CandidateOption>());
    
    
    
    public override string ToString()
    {
        return $"Election{{Id={Id}, ElectionName={ElectionName}}}";
    }
}
}