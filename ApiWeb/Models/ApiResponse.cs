using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Payload { get; set; }
        public Notify Notify { get; set; }

    }

    public class Notify
    {
        public Guid NotifyId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

    }
}
