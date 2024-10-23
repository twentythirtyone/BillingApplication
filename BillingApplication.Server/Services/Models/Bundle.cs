using NpgsqlTypes;

namespace BillingApplication.Services.Models
{
    public class Bundle
    {
        public int Id { get; set; }
        public NpgsqlInterval Interval { get; set; }
        public int Messages { get; set; }
        public long Internet { get; set; }
    }
}
