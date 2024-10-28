using BillingApplication.Services.Models.Utilites;

namespace BillingApplication.Server.Services.Models.Utilites
{
    public class AddExtraToUserModel
    {
        public Extras Extra { get; set; }
        public int UserId { get; set; }
    }
}
