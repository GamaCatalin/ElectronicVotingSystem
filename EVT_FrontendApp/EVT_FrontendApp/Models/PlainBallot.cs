using System;
using System.Collections.Generic;

namespace EVT_FrontendApp.Models
{
    public class PlainBallot : IIdentifiable
    {
        public string Id { get; set; }
        public string ElectionName { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public string OrganizationId { get; set; }


        public static explicit operator Ballot(PlainBallot ballot) => new Ballot
        {
            Id = ballot.Id,
            ElectionName = ballot.ElectionName,
            OrganizationId = ballot.OrganizationId,
            EndDate = ballot.EndDate,
            StartDate = ballot.StartDate,
            Options = new List<CandidateOption>()
        };
        
    }
}