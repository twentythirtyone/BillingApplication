using BillingApplication.Server.DataLayer.Entities;
using System.ComponentModel.DataAnnotations.Schema;


namespace BillingApplication.Entities
{
    public class SubscriberEntity
    {
        public int? Id { get; set; }
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Salt { get; set; } = "";
        public int? PassportId { get; set; }
        public int? TariffId { get; set; }
        public required string Number { get; set; }
        public decimal Balance { get; set; }
        public DateTime PaymentDate { get; set; }
        public TimeSpan CallTime { get; set; }
        public int MessagesCount { get; set; }
        public long InternetAmount { get; set; }
        public DateTime CreationDate { get; set; }


        public virtual PassportInfoEntity PassportInfo { get; set; }
        public virtual TariffEntity Tariff { get; set; }
        public virtual ICollection<PaymentEntity> Payments { get; set; }
        public virtual ICollection<TopUpsEntity> TopUps { get; set; }
        public virtual ICollection<CallsEntity> Calls { get; set; }
        public virtual ICollection<MessagesEntity> Messages { get; set; }
        public virtual ICollection<InternetEntity> Internet { get; set; }
        public virtual ICollection<OwnerChangeEntity> OwnerChanges { get; set; }
        public virtual ICollection<TariffChangeEntity> TariffChanges { get; set; }
    }
}
