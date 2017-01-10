using System.Collections.Generic;

namespace TicketSystem.Models.ApiResults
{
    public class TobitAccessToken
    {
        public int LocationId { get; set; }
        public List<object> Permissions { get; set; }
        public int UserId { get; set; }
        public string PersonId { get; set; }
        public string FacebookUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Expires { get; set; }
        public TokenType TokenType { get; set; }
    }
    public class TokenType
    {
        public int Type { get; set; }
        public string Name { get; set; }
    }
}