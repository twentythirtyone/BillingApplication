using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Nickname { get; set; }
        public required string Password { get; set; }
    }
}
