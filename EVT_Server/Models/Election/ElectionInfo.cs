using EVT_Server.Models.Election;

namespace EVT_Server.Models;

public class ElectionInfo : BaseEntity
{
    public string ElectionName { get; set; } = "";
    public IEnumerable<CandidateOption> Options { get; set; } = Array.Empty<CandidateOption>();
    
    
    
    public override string ToString()
    {
        return $"Election{{Id={Id}, ElectionName={ElectionName}}}";
    }
}