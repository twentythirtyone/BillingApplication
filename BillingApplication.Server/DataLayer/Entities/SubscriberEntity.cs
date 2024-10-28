using System.ComponentModel.DataAnnotations.Schema;


namespace BillingApplication.Entities
{
    public class SubscriberEntity
    {
        [Column("id")]
        public int? Id { get; set; }
        [Column("email")]
        public string Email { get; set; } = "";
        [Column("password")]
        public string Password { get; set; } = "";
        [Column("salt")]
        public string Salt { get; set; } = "";
        [Column("passport_id")]
        public int PassportId { get; set; }
        [Column("tariff_id")]
        public int TariffId { get; set; }
        [Column("phone_number")]
        public required string Number { get; set; }
        [Column("balance")]
        public decimal Balance { get; set; }
        [Column("payment_date")]
        public DateTime PaymentDate { get; set; }
        [Column("call_time")]
        public TimeSpan CallTime { get; set; }
        [Column("messages")]
        public int MessagesCount { get; set; }
        [Column("internet")]
        public long Internet { get; set; }

        public virtual PassportInfoEntity PassportInfo { get; set; }
        public virtual TariffEntity Tariff { get; set; }
        public virtual ICollection<PaymentEntity> Payments { get; set; }
        public virtual ICollection<TopUpsEntity> TopUps { get; set; }
        public virtual ICollection<CallsEntity> Calls { get; set; }
        public virtual ICollection<MessagesEntity> Messages { get; set; }
        public virtual ICollection<OwnerChangeEntity> OwnerChanges { get; set; }
        public virtual ICollection<TariffChangeEntity> TariffChanges { get; set; }
    }
}
