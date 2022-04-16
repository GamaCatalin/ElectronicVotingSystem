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

namespace EVT_Server.Services;



public class ElectionService
{
    private ElectionRepository repo;
    
    
    public ElectionService()
    {

        repo = new ElectionRepository();



    }



    public async Task<IEnumerable<ElectionInfo>?> GetAll()
    {
        return await repo.GetAll();
        
    }


    public async Task<bool> InsertElection(string electionName, IEnumerable<CandidateOption> candidates)
    {
        ElectionInfo election = new ElectionInfo() {Id = Guid.NewGuid(), ElectionName = electionName, Options = candidates};

        var insertResult = await repo.InsertElection(election);

        return insertResult;
    }




}