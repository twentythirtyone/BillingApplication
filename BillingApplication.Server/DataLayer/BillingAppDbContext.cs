using BillingApplication.DataLayer.Entities;
using BillingApplication.Entities;
using BillingApplication.Server.DataLayer.Entities;
using BillingApplication.Services.Models.Subscriber.Stats;
using BillingApplication.Services.Models.Utilites;
using Microsoft.EntityFrameworkCore;

namespace BillingApplication
{
    public class BillingAppDbContext : DbContext
    {
        public BillingAppDbContext(DbContextOptions<BillingAppDbContext> options)
            :base(options)
        {
            
        }

        public DbSet<PassportInfoEntity> PassportInfos { get; set; }
        public DbSet<OperatorEntity> Operators { get; set; }
        public DbSet<SubscriberEntity> Subscribers { get; set; }
        public DbSet<BundleEntity> Bundles { get; set; }
        public DbSet<TariffEntity> Tariffs { get; set; }
        public DbSet<ExtrasEntity> Extras { get; set; }
        public DbSet<PaymentEntity> Payments { get; set; }
        public DbSet<TopUpsEntity> TopUps { get; set; }
        public DbSet<CallsEntity> Calls { get; set; }
        public DbSet<MessagesEntity> Messages { get; set; }
        public DbSet<InternetEntity> Internet { get; set; }
        public DbSet<OwnerChangeEntity> OwnerChanges { get; set; }
        public DbSet<TariffChangeEntity> TariffChanges { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            AutoIncrementAdd(modelBuilder);

            // Связь между PassportInfo и Subscriber
            modelBuilder.Entity<PassportInfoEntity>()
                .HasMany(p => p.Subscribers) // Один паспорт имеет много подписчиков
                .WithOne(s => s.PassportInfo) // У подписчика есть один паспорт
                .HasForeignKey(s => s.PassportId)// Внешний ключ в таблице подписчиков
                .OnDelete(DeleteBehavior.NoAction); 

            // Связь между Subscriber и Payment
            modelBuilder.Entity<SubscriberEntity>()
                .HasMany(s => s.Payments) // Один подписчик имеет много платежей
                .WithOne(p => p.Subscriber) // У платежа есть один подписчик
                .HasForeignKey(p => p.PhoneId)
                .OnDelete(DeleteBehavior.NoAction); // Внешний ключ в таблице платежей

            // Связь между Subscriber и TopUps
            modelBuilder.Entity<SubscriberEntity>()
                .HasMany(s => s.TopUps) // Один подписчик имеет много пополнений
                .WithOne(t => t.Subscriber) // У пополнения есть один подписчик
                .HasForeignKey(t => t.PhoneId)// Внешний ключ в таблице пополнений
                .OnDelete(DeleteBehavior.NoAction); 

            // Связь между Subscriber и Calls
            modelBuilder.Entity<SubscriberEntity>()
                .HasMany(s => s.Calls) // Один подписчик имеет много звонков
                .WithOne(c => c.Subscriber) // У звонка есть один подписчик
                .HasForeignKey(c => c.FromSubscriberId); // Внешний ключ в таблице звонков

            // Связь между Subscriber и Messages
            modelBuilder.Entity<SubscriberEntity>()
                .HasMany(s => s.Messages) // Один подписчик имеет много сообщений
                .WithOne(m => m.Subscriber) // У сообщения есть один подписчик
                .HasForeignKey(m => m.FromPhoneId); // Внешний ключ в таблице сообщений

            // Связь между Subscriber и Internet
            modelBuilder.Entity<SubscriberEntity>()
                .HasMany(s => s.Internet) // Один подписчик имеет много трафика
                .WithOne(i => i.Subscriber) // У потраченного трафика есть один подписчик
                .HasForeignKey(i => i.PhoneId); // Внешний ключ в таблице трафика

            // Связь между Subscriber и OwnerChange
            modelBuilder.Entity<SubscriberEntity>()
                .HasMany(s => s.OwnerChanges) // Один подписчик имеет много изменений владельца
                .WithOne(oc => oc.Subscriber) // У изменения владельца есть один подписчик
                .HasForeignKey(oc => oc.PhoneId); // Внешний ключ в таблице изменений владельца

            // Связь между Subscriber и TariffChange
            modelBuilder.Entity<SubscriberEntity>()
                .HasMany(s => s.TariffChanges) // Один подписчик имеет много изменений тарифа
                .WithOne(tc => tc.Subscriber) // У изменения тарифа есть один подписчик
                .HasForeignKey(tc => tc.PhoneId)// Внешний ключ в таблице изменений тарифа
                .OnDelete(DeleteBehavior.NoAction); 

            // Связь между Tariff и Subscriber
            modelBuilder.Entity<TariffEntity>()
                .HasMany(t => t.Subscribers) // Один тариф имеет много подписчиков
                .WithOne(s => s.Tariff) // У подписчика есть один тариф
                .HasForeignKey(s => s.TariffId)// Внешний ключ в таблице подписчиков
                .OnDelete(DeleteBehavior.NoAction); 


            // Связь между Bundle и Tariff
            modelBuilder.Entity<BundleEntity>()
                .HasMany(b => b.Tariffs) // Один бандл имеет много тарифов
                .WithOne(t => t.Bundle) // У тарифа есть один бандл
                .HasForeignKey(t => t.TariffPlan)// Внешний ключ в тарифе
                .OnDelete(DeleteBehavior.Cascade);


            // Связь между Bundle и Extras
            modelBuilder.Entity<BundleEntity>()
                .HasMany(b => b.Extras) // Один пакет имеет много дополнительных услуг
                .WithOne(e => e.Bundle) // У дополнительной услуги есть один пакет
                .HasForeignKey(e => e.Package)// Внешний ключ в таблице дополнительных услуг
                .OnDelete(DeleteBehavior.Cascade); 

            // Связь между TariffChange и Tariff
            modelBuilder.Entity<TariffChangeEntity>()
                .HasOne(tc => tc.LastTariff) // У изменения тарифа есть последний тариф
                .WithMany() // Один тариф может не иметь связанных изменений
                .HasForeignKey(tc => tc.LastTariffId)// Внешний ключ для последнего тарифа
                .OnDelete(DeleteBehavior.SetNull); 

            modelBuilder.Entity<TariffChangeEntity>()
                .HasOne(tc => tc.NewTariff) // У изменения тарифа есть новый тариф
                .WithMany() // Один тариф может не иметь связанных изменений
                .HasForeignKey(tc => tc.NewTariffId) // Внешний ключ для нового тарифа
                .OnDelete(DeleteBehavior.SetNull);

            // Связь между OwnerChange и PassportInfo
            modelBuilder.Entity<OwnerChangeEntity>()
                .HasOne(oc => oc.LastUser) // У изменения владельца есть последний пользователь
                .WithMany() // Один пользователь может не иметь связанных изменений
                .HasForeignKey(oc => oc.LastUserId) // Внешний ключ для последнего пользователя
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OwnerChangeEntity>()
                .HasOne(oc => oc.NewUser) // У изменения владельца есть новый пользователь
                .WithMany() // Один пользователь может не иметь связанных изменений
                .HasForeignKey(oc => oc.NewUserId)
                .OnDelete(DeleteBehavior.SetNull); // Внешний ключ для нового пользователя
        }

        void AutoIncrementAdd(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;
                var idProperty = clrType.GetProperty("Id");

                if (idProperty != null && idProperty.PropertyType == typeof(int))
                {
                    modelBuilder.Entity(clrType).Property("Id").ValueGeneratedOnAdd();
                }
            }

        }
    }
}
