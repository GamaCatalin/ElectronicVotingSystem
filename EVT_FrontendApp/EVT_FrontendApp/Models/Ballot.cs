using System;
using System.Collections;
using System.Collections.Generic;

namespace EVT_FrontendApp.Models
{
    public class Ballot : IIdentifiable
    {
        public string Id { get; set; }
        public string ElectionName { get; set; }
        
        public IEnumerable<CandidateOption> Options { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public string OrganizationId { get; set; }
       
        
        public static explicit operator PlainBallot(Ballot ballot) => new PlainBallot
        {
            Id = ballot.Id,
            ElectionName = ballot.ElectionName,
            OrganizationId = ballot.OrganizationId,
            EndDate = ballot.EndDate,
            StartDate = ballot.StartDate
        };
    }
}