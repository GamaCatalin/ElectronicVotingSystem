using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using Azure;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Security.ConfidentialLedger;
using EVT_Server.Models;
using EVT_Server.Models.Election;
using EVT_Server.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EVT_Server.Services{



public class ElectionService
{
    private ElectionRepository repo;
    
    
    public ElectionService()
    {

        repo = new ElectionRepository();



    }



    public List<ElectionInfo>? GetAll()
    {
        var elections = repo.GetAllElections();
        
        foreach (var election in elections)
        {
            var matchingCandidates = GetMatchingCandidates(election.Id);
            foreach(var candidate in matchingCandidates)
            {
                Console.WriteLine(candidate.ToString());
                election.Options.Add(candidate);
            }
        }

        return elections;
    }

    private List<CandidateOption> GetMatchingCandidates(Guid electionId)
    {
        CandidateService candidateService = new CandidateService();        
        List<CandidateOption> candidates = candidateService.GetAll();

        var matchingCandidates = candidates.FindAll(candidate => candidate.ElectionId == electionId);

        return matchingCandidates;
    }


    public bool InsertElection(string electionName, IEnumerable<CandidateOption> candidates)
    {
        ElectionInfo election = new ElectionInfo() {Id = Guid.NewGuid(), ElectionName = electionName, Options = new List<CandidateOption>(candidates)};

        var insertResult = repo.InsertElection(election);

        return insertResult;
    }




}

}