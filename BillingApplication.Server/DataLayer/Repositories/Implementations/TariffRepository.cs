﻿using BillingApplication.Entities;
using BillingApplication.Mapper;
using BillingApplication.Exceptions;
using BillingApplication.Services.Models.Roles;
using BillingApplication.Services.Models.Utilites.Tariff;
using Microsoft.EntityFrameworkCore;
using BillingApplication.Services.Models.Utilites;
using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Server.Mapper;
using BillingApplication.Server.Services.Models.Utilites;
using BillingApplication.Server.Quartz.Workers;

namespace BillingApplication.Server.DataLayer.Repositories.Implementations
{
    public class TariffRepository : ITariffRepository
    {
        private readonly BillingAppDbContext context;
        private readonly IEmailSender emailSender;
        public TariffRepository(BillingAppDbContext context, IEmailSender emailSender)
        {
            this.context = context;
            this.emailSender = emailSender;
        }
        public async Task<int?> Create(Tariffs? tariff, int? bundleId)
        {
            var existingBundle = await context.Bundles.FindAsync(bundleId);
            if (existingBundle == null)
            {
                existingBundle = BundleMapper.BundleModelToBundleEntity(tariff!.Bundle);
                await context.Bundles.AddAsync(existingBundle!);
                await context.SaveChangesAsync();
            }

            bundleId = existingBundle.Id;
            existingBundle = await context.Bundles.FindAsync(bundleId);

            var tariffEntity = TariffMapper.TariftModelToTarifEntity(tariff, existingBundle!);

            if (context.Tariffs.Any(x => x.Title == tariff.Title))
                throw new Exception("Такое название тарифа уже есть");

            await context.Tariffs.AddAsync(tariffEntity);
            await context.SaveChangesAsync();

            return tariffEntity?.Id;
        }

        public async Task<int?> Delete(int? id)
        {
            if (id == null)
                return null;

            var tariff = await context.Tariffs
                                      .Include(t => t.Subscribers) 
                                      .FirstOrDefaultAsync(t => t.Id == id);

            if (tariff == null)
                throw new TariffNotFoundException();

            foreach (var subscriber in tariff.Subscribers)
            {
                subscriber.TariffId = Constants.DEFAULT_TARIFF_ID;
                await emailSender.SendEmailAsync(subscriber.Email, "Уведомление", "Ваш тариф устарел и больше не функционирует. " +
                                                                                  "Ваши остатки пакетов сохранены, но по их окончанию условия тарифа" +
                                                                                  "прекращают своё действие. ");
                context.TariffChanges.Add(
                    new TariffChangeEntity
                    {
                        LastTariffId = null,
                        NewTariffId = Constants.DEFAULT_TARIFF_ID,
                        Date = DateTime.UtcNow,
                        PhoneId = (int)subscriber.Id!
                    }) ;
            }
            context.Tariffs.Remove(tariff);

            await context.SaveChangesAsync();

            return tariff.Id;
        }

        public async Task<IEnumerable<Tariffs?>> Get()
        {
            var tariffEntities = await context.Tariffs
                .Include(x => x.Bundle)
                .AsNoTracking()
                .ToListAsync();
            return tariffEntities.Select(TariffMapper.TariftEntityToTarifModel);
        }

        public async Task<Tariffs?> GetById(int? id)
        {
            var tariffEntity = await context.Tariffs.FindAsync(id);
            var tariff = TariffMapper.TariftEntityToTarifModel(tariffEntity);
            return tariff;
        }

        public async Task<Tariffs?> GetByTitle(string Title)
        {
            var tariffEntity = await context.Tariffs.Where(t => t.Title == Title).FirstOrDefaultAsync();
            var tariff = TariffMapper.TariftEntityToTarifModel(tariffEntity);
            return tariff;
        }

        public async Task<Tariffs?> GetBySubscriber(int id)
        {
            var existingSubscriber = await context.Subscribers.FindAsync(id);
            if (existingSubscriber == null)
            {
                throw new UserNotFoundException();
            }

            var tariffEntity = await context.Tariffs.Where(t => t.Id == existingSubscriber.TariffId).FirstOrDefaultAsync();
            var tariff = TariffMapper.TariftEntityToTarifModel(tariffEntity);
            return tariff;
        }

        public async Task<int?> Update(Tariffs? tariff, int? bundleId)
        {
            var existingBundle = await context.Bundles.FindAsync(bundleId);
            if (existingBundle == null)
            {
                existingBundle = BundleMapper.BundleModelToBundleEntity(tariff!.Bundle);
                await context.Bundles.AddAsync(
                    existingBundle!
                 );
            }
            var currentTariff = await context.Tariffs.FindAsync(tariff.Id);
            if (currentTariff.Id is not null)
            {
                TariffMapper.UpdateTariffEntity(currentTariff, tariff, existingBundle);
            }
            await context.SaveChangesAsync();
            return currentTariff.Id;
        }

        public async Task<Bundle> GetBundleByTariffId(int? tariffId)
        {
            var bundleEntities = context.Tariffs.Select(x => x.Bundle);
            var bundle = new Bundle();
            foreach (var bundleEntity in bundleEntities)
            {
                bundle.CallTime += bundleEntity.CallTIme;
                bundle.Messages += bundleEntity.Messages;
                bundle.Internet += bundleEntity.Internet;
            }
            return bundle;
        }

        public async Task<Dictionary<string, int>> GetTariffsByUserCount()
        {
            var tariffs = await context.Tariffs.Select(x => x.Title).ToListAsync();
            var result = new Dictionary<string, int>();
            foreach (var tariff in tariffs)
            {
                var users = await context.Subscribers
                    .Include(x => x.Tariff)
                    .Where(x => x.Tariff.Title == tariff)
                    .ToListAsync();
                result.Add(tariff, users.Count);
            }
            return result.OrderByDescending(x=>x.Value).ToDictionary();
        }
    }
}
