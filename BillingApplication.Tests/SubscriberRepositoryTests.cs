using BillingApplication;
using BillingApplication.Entities;
using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Server.DataLayer.Repositories.Implementations;
using BillingApplication.Server.Services.Models.Utilites;
using BillingApplication.Services.Models.Roles;
using BillingApplication.Services.Models.Subscriber;
using BillingApplication.Services.Models.Utilites.Tariff;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BillingApplication.Tests;

public class SubscriberRepositoryTests
{
    private readonly DbContextOptions<BillingAppDbContext> _dbContextOptions;

    public SubscriberRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<BillingAppDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Уникальная база для каждого теста
            .Options;
    }

    [Fact]
    public async Task Create_SubscriberWithValidTariff_ReturnsSubscriberId()
    {
        // Arrange
        await using var context = new BillingAppDbContext(_dbContextOptions);
        context.Tariffs.Add(new TariffEntity { Id = 1, Title = "Test Tariff", Bundle = new BillingApplication.DataLayer.Entities.BundleEntity() });
        await context.SaveChangesAsync();

        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var repository = new SubscriberRepository(context, paymentRepositoryMock.Object);

        var subscriber = new Subscriber {  Email = "test@test.com", Number = "1234567890" };
        var passportInfo = new PassportInfo { PassportNumber = "12345678" };

        // Act
        var result = await repository.Create(subscriber, passportInfo, 1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result > 0);
    }

    [Fact]
    public async Task Create_SubscriberWithInvalidTariff_SetsDefaultTariff()
    {
        // Arrange
        await using var context = new BillingAppDbContext(_dbContextOptions);
        await context.Tariffs.AddAsync(new TariffEntity { Id = Constants.DEFAULT_TARIFF_ID, Title = "Default Tariff", Bundle = new DataLayer.Entities.BundleEntity() });
        await context.SaveChangesAsync();

        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var repository = new SubscriberRepository(context, paymentRepositoryMock.Object);

        var subscriber = new Subscriber { Email = "test@test.com", Number = "1234567890" };
        var passportInfo = new PassportInfo { PassportNumber = "12345678" };

        // Act
        await repository.Create(subscriber, passportInfo, 999); // Несуществующий тариф

        // Assert
        var savedSubscriber = await context.Subscribers.FirstOrDefaultAsync(s => s.Email == "test@test.com");
        Assert.NotNull(savedSubscriber); // Проверяем, что подписчик сохранен
        Assert.Equal(Constants.DEFAULT_TARIFF_ID, savedSubscriber.TariffId); // Проверяем, что тариф по умолчанию установлен
    }



    [Fact]
    public async Task GetSubscriberById_ExistingSubscriber_ReturnsSubscriber()
    {
        // Arrange
        await using var context = new BillingAppDbContext(_dbContextOptions);
        var subscriber = new SubscriberEntity { Id = 1, Email = "test@test.com", Number = "1234567890" };
        context.Subscribers.Add(subscriber);
        await context.SaveChangesAsync();

        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var repository = new SubscriberRepository(context, paymentRepositoryMock.Object);

        // Act
        var result = await repository.GetSubscriberById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@test.com", result.Email);
    }

    [Fact]
    public async Task GetSubscriberById_NonExistingSubscriber_ReturnsNull()
    {
        // Arrange
        await using var context = new BillingAppDbContext(_dbContextOptions);
        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var repository = new SubscriberRepository(context, paymentRepositoryMock.Object);

        // Act
        var result = await repository.GetSubscriberById(999); // Несуществующий ID

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteSubscriber_ExistingSubscriber_RemovesSubscriber()
    {
        // Arrange
        await using var context = new BillingAppDbContext(_dbContextOptions);
        var subscriber = new SubscriberEntity { Id = 1, Email = "test@test.com", Number = "1234567890" };
        context.Subscribers.Add(subscriber);
        await context.SaveChangesAsync();

        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var repository = new SubscriberRepository(context, paymentRepositoryMock.Object);

        // Act
        await repository.Delete(1);
        var result = await context.Subscribers.FindAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteSubscriber_NonExistingSubscriber_DoesNothing()
    {
        // Arrange
        await using var context = new BillingAppDbContext(_dbContextOptions);
        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var repository = new SubscriberRepository(context, paymentRepositoryMock.Object);

        // Act
        await repository.Delete(999); // Несуществующий ID

        // Assert
        // Проверяем, что база данных осталась пустой
        Assert.Empty(context.Subscribers);
    }
}
