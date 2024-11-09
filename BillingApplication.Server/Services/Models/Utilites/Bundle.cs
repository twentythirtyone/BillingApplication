using NpgsqlTypes;

namespace BillingApplication.Services.Models.Utilites
{
    public class Bundle
    {
        public int? Id { get; set; }
        public TimeSpan CallTime { get; set; }
        public int Messages { get; set; }
        public long Internet { get; set; }
    }
}
