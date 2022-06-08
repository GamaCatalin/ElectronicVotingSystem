using EVT_Server.Models;
using EVT_Server.Models.Election;
using EVT_Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace EVT_Server.Controllers{

[ApiController]
[Route("[controller]")]
public class ElectionController : ControllerBase
{
    private readonly ElectionService _electionService;
    private readonly VotingService _votingService;

    public ElectionController()
    {
        _electionService = new ElectionService();
        _votingService = new VotingService();
    }


    [HttpGet]
    public IEnumerable<ElectionInfo> GetElections()
    {
        ElectionService electionService = new ElectionService();

        return electionService.GetAll()!;
    }

    [HttpPut]
    public bool AddElection([FromBody] ElectionInfo electionInfo)
    {
        var addStatus = _electionService.InsertElection(electionInfo.ElectionName, electionInfo.Options);
        return addStatus;
    }

    [HttpPut("{candidateVote}")]
    public bool Vote(Guid candidateVote)
    {
        var voteStatus = _votingService.DoVote(candidateVote);
        return voteStatus;
        // Console.WriteLine($"Vote status for candidate {candidateVote}:   {voteStatus}");       
    }
}

}