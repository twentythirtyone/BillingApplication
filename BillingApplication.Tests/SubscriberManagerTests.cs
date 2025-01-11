using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Services.Models.Subscriber;
using BillingApplication.Services.Auth;
using BillingApplication.Exceptions;
using BillingApplication.Server.Services.Models.Subscriber;
using BillingApplication.Services.Models.Roles;
using BillingApplication.Mapper;
using BillingApplication.Server.Exceptions;
using BillingApplication.Services.Models.Subscriber.Stats;
using BillingApplication.Services.Models.Utilites.Tariff;

namespace BillingApplication.Tests
{
    public class SubscriberManagerTests
    {
        private readonly Mock<ISubscriberRepository> _subscriberRepositoryMock;
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
        private readonly Mock<IEncrypt> _encryptMock;
        private readonly SubscriberManager _subscriberManager;

        public SubscriberManagerTests()
        {
            _subscriberRepositoryMock = new Mock<ISubscriberRepository>();
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _encryptMock = new Mock<IEncrypt>();

            _subscriberManager = new SubscriberManager(
                _encryptMock.Object,
                _subscriberRepositoryMock.Object,
                _paymentRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateSubscriber_ShouldThrowException_WhenPhoneExists()
        {
            // Arrange
            var existingSubscriber = new SubscriberViewModel { Id = 1, Number = "1234567890" };
            _subscriberRepositoryMock
                .Setup(repo => repo.GetSubscriberByPhone(It.IsAny<string>()))
                .ReturnsAsync(existingSubscriber);

            var newSubscriber = new Subscriber { Number = "1234567890", Password = "password" };
            var passport = new PassportInfo();

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() =>
                _subscriberManager.CreateSubscriber(newSubscriber, passport, null));
        }

        [Fact]
        public async Task CreateSubscriber_ShouldHashPassword_WhenNewSubscriber()
        {
            // Arrange
            _subscriberRepositoryMock
                .Setup(repo => repo.GetSubscriberByPhone(It.IsAny<string>()))
                .ReturnsAsync((SubscriberViewModel)null);

            _subscriberRepositoryMock
                .Setup(repo => repo.Create(It.IsAny<Subscriber>(), It.IsAny<PassportInfo>(), It.IsAny<int?>()))
                .ReturnsAsync(1);

            _encryptMock
                .Setup(e => e.HashPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("hashed_password");

            var newSubscriber = new Subscriber { Number = "1234567890", Password = "password" };
            var passport = new PassportInfo();

            // Act
            var result = await _subscriberManager.CreateSubscriber(newSubscriber, passport, null);

            // Assert
            Assert.NotNull(result);
            _encryptMock.Verify(e => e.HashPassword("password", It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSubscriber_ShouldThrowException_WhenSubscriberNotFound()
        {
            // Arrange
            _subscriberRepositoryMock
                .Setup(repo => repo.GetSubscriberById(It.IsAny<int?>()))
                .ReturnsAsync((SubscriberViewModel)null);

            var subscriber = new SubscriberViewModel { Id = 1, Number="1234567890" };

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() =>
                _subscriberManager.UpdateSubscriber(SubscriberMapper.UserVMToUserModel(subscriber), null, null));
        }

        [Fact]
        public async Task ValidateSubscriberCredentials_ShouldReturnSubscriber_WhenValidCredentials()
        {
            // Arrange
            var subscriber = new SubscriberViewModel
            {
                Id = 1,
                Number = "1234567890",
                Password = "hashed_password",
                Salt = "salt"
            };

            _subscriberRepositoryMock
                .Setup(repo => repo.GetSubscriberByPhone(It.IsAny<string>()))
                .ReturnsAsync(subscriber);

            _encryptMock
                .Setup(e => e.HashPassword("password", "salt"))
                .Returns("hashed_password");

            // Act
            var result = await _subscriberManager.ValidateSubscriberCredentials("1234567890", "password");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(subscriber.Id, result.Id);
        }

        [Fact]
        public async Task ValidateSubscriberCredentials_ShouldReturnNull_WhenInvalidPassword()
        {
            // Arrange
            var subscriber = new SubscriberViewModel
            {
                Id = 1,
                Number = "1234567890",
                Password = "hashed_password",
                Salt = "salt"
            };

            _subscriberRepositoryMock
                .Setup(repo => repo.GetSubscriberByPhone(It.IsAny<string>()))
                .ReturnsAsync(subscriber);

            _encryptMock
                .Setup(e => e.HashPassword("wrong_password", "salt"))
                .Returns("wrong_hashed_password");

            // Act
            var result = await _subscriberManager.ValidateSubscriberCredentials("1234567890", "wrong_password");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetSubscriberById_ShouldThrowException_WhenSubscriberNotFound()
        {
            // Arrange
            _subscriberRepositoryMock
                .Setup(repo => repo.GetSubscriberById(It.IsAny<int?>()))
                .ReturnsAsync((SubscriberViewModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() =>
                _subscriberManager.GetSubscriberById(1));
        }

        [Fact]
        public async Task GetSubscribers_ShouldReturnEmptyList_WhenNoSubscribersExist()
        {
            // Arrange
            _subscriberRepositoryMock
                .Setup(repo => repo.GetAll())
                .ReturnsAsync(new List<SubscriberViewModel>());

            // Act
            var result = await _subscriberManager.GetSubscribers();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetSubscribers_ShouldReturnListOfSubscribers()
        {
            // Arrange
            var subscribers = new List<SubscriberViewModel>
    {
        new SubscriberViewModel { Id = 1, Number = "1234567890" },
        new SubscriberViewModel { Id = 2, Number = "0987654321" }
    };

            _subscriberRepositoryMock
                .Setup(repo => repo.GetAll())
                .ReturnsAsync(subscribers);

            // Act
            var result = await _subscriberManager.GetSubscribers();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetSubscribersByTariff_ShouldReturnEmptyList_WhenNoSubscribersExistForTariff()
        {
            // Arrange
            _subscriberRepositoryMock
                .Setup(repo => repo.GetSubscribersByTariff(It.IsAny<int?>()))
                .ReturnsAsync(new List<SubscriberViewModel>());

            // Act
            var result = await _subscriberManager.GetSubscribersByTariff(1);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetSubscribersByTariff_ShouldReturnSubscribers_WhenSubscribersExist()
        {
            // Arrange
            var subscribers = new List<SubscriberViewModel>
    {
        new SubscriberViewModel { Id = 1, Number = "1234567890" }
    };

            _subscriberRepositoryMock
                .Setup(repo => repo.GetSubscribersByTariff(1))
                .ReturnsAsync(subscribers);

            // Act
            var result = await _subscriberManager.GetSubscribersByTariff(1);

            // Assert
            Assert.Single(result);
            Assert.Equal("1234567890", result.First().Number);
        }

        [Fact]
        public async Task AddExtraToSubscriber_ShouldThrowException_WhenPackageNotFound()
        {
            // Arrange
            _subscriberRepositoryMock
                .Setup(repo => repo.AddExtraToSubscriber(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int?)null);

            // Act & Assert
            await Assert.ThrowsAsync<PackageNotFoundException>(() =>
                _subscriberManager.AddExtraToSubscriber(1, 1));
        }

        [Fact]
        public async Task AddExtraToSubscriber_ShouldReturnId_WhenSuccessful()
        {
            // Arrange
            _subscriberRepositoryMock
                .Setup(repo => repo.AddExtraToSubscriber(1, 1))
                .ReturnsAsync(1);

            // Act
            var result = await _subscriberManager.AddExtraToSubscriber(1, 1);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task AddPaymentForTariff_ShouldThrowException_WhenSubscriberNotFound()
        {
            // Arrange
            _subscriberRepositoryMock
                .Setup(repo => repo.GetSubscriberById(It.IsAny<int?>()))
                .ReturnsAsync((SubscriberViewModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() =>
                _subscriberManager.AddPaymentForTariff(1));
        }

        [Fact]
        public async Task AddPaymentForTariff_ShouldAddPayment_WhenSubscriberExists()
        {
            // Arrange
            var subscriber = new SubscriberViewModel
            {
                Id = 1,
                Tariff = new Tariffs { Price = 100, Title="Название",Bundle = new Services.Models.Utilites.Bundle {Internet = 200} },
                Number = "1234567890"
            };

            _subscriberRepositoryMock
                .Setup(repo => repo.GetSubscriberById(1))
                .ReturnsAsync(subscriber);

            _paymentRepositoryMock
                .Setup(repo => repo.AddPayment(It.IsAny<Payment>()))
                .ReturnsAsync(1);

            _subscriberRepositoryMock
                .Setup(repo => repo.AddUserTraffic(1))
                .ReturnsAsync(1);

            // Act
            var result = await _subscriberManager.AddPaymentForTariff(1);

            // Assert
            Assert.Equal(1, 1);
            _paymentRepositoryMock.Verify(repo =>
                repo.AddPayment(It.Is<Payment>(p => p.Amount == 100 && p.PhoneId == 1)), Times.Once);
        }

        [Fact]
        public async Task GetExpensesCurrentYear_ShouldReturnExpenses()
        {
            // Arrange
            _subscriberRepositoryMock
                .Setup(repo => repo.GetExpensesCurrentYear(1))
                .ReturnsAsync(500);

            // Act
            var result = await _subscriberManager.GetExpensesCurrentYear(1);

            // Assert
            Assert.Equal(500, result);
        }

    }
}
