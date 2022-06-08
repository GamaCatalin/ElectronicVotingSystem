namespace EVT_FrontendApp.Models
{
    public class Organization : IIdentifiable
    {
        public string Id { get; set; }
        
        public string OrganizationName { get; set; }
        
        public string Founder { get; set; }
    }
}