using System.Collections.Generic;

namespace TicketSystem.Models
{
    public class ApiResult<T>
    {
        public List<T> Data { get; set; }
    }
}