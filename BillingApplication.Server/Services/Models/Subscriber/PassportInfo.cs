using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Services.Models.Subscriber
{
    public class PassportInfo
    {
        public int? Id { get; set; }
        public string? PassportNumber { get; set; }
        public string? FullName { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? IssuedBy { get; set; }
        public string? Registration { get; set; }
    }
}
