using System;

namespace EVT_FrontendApp.Models
{
    public class AuthInfo
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public AuthInfo()
        {
        }

        public AuthInfo(string username, string password)
        {
            Id = Guid.NewGuid().ToString();
            Username = username;
            Password = password;
        }
    }
}