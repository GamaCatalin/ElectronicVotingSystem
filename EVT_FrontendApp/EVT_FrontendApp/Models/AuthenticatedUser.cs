namespace EVT_FrontendApp.Models
{
    public class AuthenticatedUser : IIdentifiable
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string OrganizationId { get; set; }
        public string Email { get; set; }
        public string ProfilePictureURL { get; set; }
        
    }
}