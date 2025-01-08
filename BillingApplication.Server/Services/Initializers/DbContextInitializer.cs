using BillingApplication.DataLayer.Entities;
using BillingApplication.Entities;
using BillingApplication.Services.Auth;
using BillingApplication.Services.Models.Subscriber;
using BillingApplication.Services.Models.Utilites;
using Microsoft.EntityFrameworkCore;

namespace BillingApplication.Server.Services.Initializers
{
    public class DbContextInitializer
    {
        public static void InitializeDbContext(BillingAppDbContext dbContext, IEncrypt encrypt)
        {
            dbContext.Database.Migrate();
            var existingOperators = dbContext.Operators.ToArray();
            var existingTariffs = dbContext.Tariffs.ToArray();
            var existingExtras = dbContext.Extras.ToArray();
            var existingBundles = dbContext.Bundles.ToArray();
            var existingUsers = dbContext.Subscribers.ToArray();
            var existingPassports = dbContext.PassportInfos.ToArray();

            AddOperatorIfNotExist(email: "admin@billing.ru", nickname: "Admin", password: "admin123!", isAdmin: true);
            AddOperatorIfNotExist(email: "operator@billing.ru", nickname: "Operator", password: "operator123!", isAdmin: false);

            AddBundleIfNotExist(id: 1, messages: 30);
            AddBundleIfNotExist(id: 2, messages: 100);
            AddBundleIfNotExist(id: 3, messages: 150);
            AddBundleIfNotExist(id: 4, calltime: 50);
            AddBundleIfNotExist(id: 5, calltime: 100);
            AddBundleIfNotExist(id: 6, calltime: 150);
            AddBundleIfNotExist(id: 7, calltime: 200);
            AddBundleIfNotExist(id: 8, calltime: 300);
            AddBundleIfNotExist(id: 9, internet: 5 * 1024);
            AddBundleIfNotExist(id: 10, internet: 10 * 1024);
            AddBundleIfNotExist(id: 11, internet: 15 * 1024);
            AddBundleIfNotExist(id: 12, internet: 20 * 1024);
            AddBundleIfNotExist(id: 13, internet: 25 * 1024);
            AddBundleIfNotExist(id: 14, calltime: 100, internet: 10 * 1024, messages: 100);
            AddBundleIfNotExist(id: 15, calltime: 300, internet: 25 * 1024, messages: 150);
            AddBundleIfNotExist(id: 16, calltime: 200, internet: 15 * 1024, messages: 100);
            AddBundleIfNotExist(id: 17, calltime: 150, messages: 150);
            AddBundleIfNotExist(id: 18, calltime: 50, internet: 2 * 1024, messages: 50);
            AddBundleIfNotExist(id: 19, calltime: 0, internet: 0, messages: 0);

            AddExtrasIfNotExist(id: 1, title: "100 на связи", price: 100, bundleId: 5);
            AddExtrasIfNotExist(id: 2, title: "Сеть на максимум", price: 100, bundleId: 10);
            AddExtrasIfNotExist(id: 3, title: "SMS Pro", price: 100, bundleId: 2);
            AddExtrasIfNotExist(id: 4, title: "Мини-разговор", price: 70, bundleId: 4);
            AddExtrasIfNotExist(id: 5, title: "Сетевой мини", price: 70, bundleId: 9);
            AddExtrasIfNotExist(id: 6, title: "SMS мини", price: 70, bundleId: 1);

            AddTariffIfNotExist(id: 1, title: "Стандартный", price: 0, bundleId: 19);
            AddTariffIfNotExist(id: 2, title: "Оптимум", price: 300, bundleId: 14);
            AddTariffIfNotExist(id: 3, title: "Всё и сразу", price: 800, bundleId: 15);
            AddTariffIfNotExist(id: 4, title: "Золотая середина", price: 450, bundleId: 16);
            AddTariffIfNotExist(id: 5, title: "Чистый онлайн", price: 200, bundleId: 10);
            AddTariffIfNotExist(id: 6, title: "Только общение", price: 200, bundleId: 17);

            AddUserIfNotExist(
                email: "nneketaa@yandex.ru", 
                number: "+79089260075", 
                balance: 9999999, 
                password: "Qwerty123!", 
                passport: new PassportInfoEntity
                {
                    Id = 1,
                    ExpiryDate = DateTime.UtcNow,
                    FullName ="Беликов Никита Васильевич",
                    IssueDate = DateTime.UtcNow,
                    IssuedBy = "МВД России",
                    PassportNumber = "6666 666666",
                    Registration = "Екакебибург"
                }
                );
            AddUserIfNotExist(
                email: "zensot1@yandex.ru",
                number: "89512723576",
                balance: 9999999,
                password: "bb228dmm",
                passport: new PassportInfoEntity
                {
                    Id = 2,
                    ExpiryDate = DateTime.UtcNow,
                    FullName = "Егоров Евгений Андреевич",
                    IssueDate = DateTime.UtcNow,
                    IssuedBy = "МВД России",
                    PassportNumber = "6666 666665",
                    Registration = "Екакебибург"
                }
                );
            AddUserIfNotExist(
                email: "kontyrtime@mail.ru",
                number: "89000326386",
                balance: 9999999,
                password: "Qwerty123!",
                passport: new PassportInfoEntity
                {
                    Id = 3,
                    ExpiryDate = DateTime.UtcNow,
                    FullName = "Верхотуров Виталий Сергеевич",
                    IssueDate = DateTime.UtcNow,
                    IssuedBy = "МВД России",
                    PassportNumber = "6666 666664",
                    Registration = "Екакебибург"
                }
                );

            dbContext.SaveChanges();

            void AddOperatorIfNotExist(string email, string nickname, string password, bool isAdmin)
            {
                if (existingOperators.Any(eb => eb.Email == email)) return;
                
                string salt = Guid.NewGuid().ToString();
                dbContext.Operators.Add(new OperatorEntity
                {
                    Email = email,
                    Nickname = nickname,
                    Salt = salt,
                    Password = encrypt.HashPassword(password, salt),
                    IsAdmin = isAdmin
                });
            }

            async void AddUserIfNotExist(string email, string number, string password, PassportInfoEntity passport, int balance = 200)
            {
                if (existingUsers.Any(eb => eb.Email == email)) return;

                string salt = Guid.NewGuid().ToString();
                var tariff = await dbContext.Tariffs.FindAsync(1);

                AddPassportIfNotExist(passport);

                dbContext.Subscribers.Add(new SubscriberEntity
                {
                    Id = passport.Id,
                    Email = email,
                    Salt = salt,
                    Number = number,
                    Balance = balance,
                    PaymentDate = DateTime.UtcNow,
                    PassportId = passport.Id,
                    PassportInfo = passport,
                    TariffId = 1,
                    Tariff = tariff!,
                    InternetAmount = tariff!.Bundle.Internet,
                    MessagesCount = tariff!.Bundle.Messages,
                    CallTime = tariff!.Bundle.CallTIme,
                    Password = encrypt.HashPassword(password, salt),
                    CreationDate = DateTime.UtcNow
                });

                
            }

            async void AddPassportIfNotExist(PassportInfoEntity passport)
            {
                if (existingPassports.Any(eb => eb.Id == passport.Id)) return;

                dbContext.PassportInfos.Add(passport);
            }

            void AddBundleIfNotExist(int id, int calltime = 0, long internet = 0, int messages = 0)
            {
                if (existingBundles.Any(eb => eb.Id == id)) return;
                
                dbContext.Bundles.Add(new BundleEntity
                {
                    Id = id,
                    CallTIme = TimeSpan.FromMinutes(calltime),
                    Internet = internet,
                    Messages = messages
                });
            }

            async void AddTariffIfNotExist(int id, string title, int price, int bundleId)
            {
                if (existingTariffs.Any(eb => eb.Id == id)) return;

                var bundle = await dbContext.Bundles.FindAsync(bundleId);
                dbContext.Tariffs.Add(new TariffEntity
                {
                    Id = id,
                    Title = title,
                    Price = price,
                    Description = "",
                    TariffPlan = bundleId,
                    Bundle = bundle!
                });
            }

            async void AddExtrasIfNotExist(int id, string title, int price, int bundleId)
            {
                if (existingExtras.Any(eb => eb.Id == id)) return;

                var bundle = await dbContext.Bundles.FindAsync(bundleId);
                dbContext.Extras.Add(new ExtrasEntity
                {
                    Id = id,
                    Title = title,
                    Price= price,
                    Description = "",
                    Package = bundleId,
                    Bundle = bundle!
                });
            }

        }
    }
}
