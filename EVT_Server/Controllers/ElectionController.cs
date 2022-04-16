using EVT_Server.Models.Election;
using EVT_Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace EVT_Server.Controllers;

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
    public async Task GetElections()
    {
        ElectionService electionService = new ElectionService();

        await electionService.GetAll();
    }

    [HttpPut]
    public async Task AddElection()
    {
        await _electionService.InsertElection("new new election", new CandidateOption[]
        {
            new CandidateOption()
            {
                Id = Guid.NewGuid(),
                Title = "candidate 1"
            },
            new CandidateOption()
            {
                Id = Guid.NewGuid(),
                Title = "candidate 2"
            },
            new CandidateOption()
            {
                Id = Guid.NewGuid(),
                Title = "candidate 3"
            },
            new CandidateOption()
            {
                Id = Guid.NewGuid(),
                Title = "candidate 4"
            },
            new CandidateOption()
            {
                Id = Guid.NewGuid(),
                Title = "candidate 5"
            }
        });
    }

    [HttpPut("{candidateVote}")]
    public async Task Vote(Guid candidateVote)
    {
        var voteStatus = await _votingService.DoVote(candidateVote);
        
        Console.WriteLine($"Vote status for candidate {candidateVote} :   {voteStatus}");
        
    }
    
}