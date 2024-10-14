using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApi.Models
{
    public class ErrorLog
    {
        public string? Path { get; set; }
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
        public DateTime Timestamp { get; set; }
    }
}