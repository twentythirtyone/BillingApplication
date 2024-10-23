using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BillingApplication.Services.Models.Roles
{
    public class Operator : IUser
    {
        public int? Id { get; set; }
        public required string Email { get; set; }
        public required string Nickname { get; set; }
        public required string Password { get; set; }
        public required bool IsAdmin { get; set; }
        public string UniqueId => Id.ToString();
    }
}
