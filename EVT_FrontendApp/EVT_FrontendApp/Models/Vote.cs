namespace EVT_FrontendApp.Models
{
    public class Vote : IIdentifiable
    {
        public string Id { get; set; }
        
        public string CandidateId { get; set; }
        
    }
}