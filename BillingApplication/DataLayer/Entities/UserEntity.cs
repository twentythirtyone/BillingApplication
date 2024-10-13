using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillingApplication.Entities
{
    public class UserEntity
    {
        [Column("id")]
        public int? Id { get; set; }
        [Column("email")]
        public string Email { get; set; } = "";
        [Column("password")]
        public string Password { get; set; } = "";
        [Column("salt")]
        public string Salt { get; set; } = "";
    }
}
