using BillingApplication.Exceptions;
using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Server.Mapper;
using BillingApplication.Server.Services.Manager.PaymentsManager;
using BillingApplication.Server.Services.Models.Subscriber.Stats;
using BillingApplication.Server.Services.Models.Utilites;
using BillingApplication.Services.Models.Subscriber.Stats;
using Microsoft.EntityFrameworkCore;

namespace BillingApplication.Server.DataLayer.Repositories.Implementations
{
    public class InternetRepository : IInternetRepository
    {
        private readonly BillingAppDbContext context;
        private readonly IPaymentsManager paymentsManager;

        public InternetRepository(BillingAppDbContext context, IPaymentsManager paymentsManager)
        {
            this.context = context;
            this.paymentsManager = paymentsManager;
        }

        public async Task<int?> AddTraffic(Internet traffic)
        {
            var user = await context.Subscribers.FindAsync(traffic.PhoneId) ?? throw new UserNotFoundException();

            if (user.InternetAmount >= traffic.SpentInternet)
            {
                user.InternetAmount -= traffic.SpentInternet;
                traffic.Price = 0;
            }
            else
            {
                await paymentsManager.AddPayment(
                    new Payment()
                    {
                        Name = "Плата за ГБ",
                        Date = DateTime.UtcNow,
                        Amount = traffic.Price,
                        PhoneId = (int)user.Id!
                    }
                 ); ;
            }

            var trafficEntity = InternetMapper.InternetModelToInternetEntity(traffic);
            await context.Internet.AddAsync(trafficEntity!);
            await context.SaveChangesAsync();

            return trafficEntity?.Id;
        }

        public async Task<IEnumerable<Internet>> Get()
        {
            var traffic = await context.Internet
                .AsNoTracking()
                .ToListAsync();

            return traffic.Select(InternetMapper.InternetEntityToInternetModel)!;
        }

        public async Task<IEnumerable<Internet>> GetAllTrafficByUserId(int? id)
        {
            var traffic = await context.Internet
               .AsNoTracking()
               .ToListAsync();

            return traffic.Where(x => x.PhoneId == id).Select(InternetMapper.InternetEntityToInternetModel)!;
        }

        public async Task<Internet> GetTrafficById(int id)
        {
            var traffic = await context.Internet.FindAsync(id);

            return InternetMapper.InternetEntityToInternetModel(traffic)!;
        }
    }
}
