using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BillingApplication.Exceptions;
using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Server.Services.Manager.OperatorManager;
using BillingApplication.Services.Auth;
using BillingApplication.Services.Models.Roles;
using Moq;
using Xunit;

namespace BillingApplication.Tests
{
    public class OperatorManagerTests
    {
        private readonly Mock<IOperatorRepository> _operatorRepositoryMock;
        private readonly Mock<IEncrypt> _encryptMock;
        private readonly OperatorManager _operatorManager;

        public OperatorManagerTests()
        {
            _operatorRepositoryMock = new Mock<IOperatorRepository>();
            _encryptMock = new Mock<IEncrypt>();
            _operatorManager = new OperatorManager(_operatorRepositoryMock.Object, _encryptMock.Object);
        }


        [Fact]
        public async Task Create_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var existingOperator = new Operator { Email = "existing@example.com", Nickname = "test", IsAdmin = true, Password="123"};
            _operatorRepositoryMock.Setup(repo => repo.GetOperatorByEmail(existingOperator.Email)).ReturnsAsync(existingOperator);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _operatorManager.Create(existingOperator));
        }

        [Fact]
        public async Task Delete_ShouldDeleteOperator_WhenOperatorExists()
        {
            // Arrange
            _operatorRepositoryMock.Setup(repo => repo.Delete(1)).ReturnsAsync(1);

            // Act
            var result = await _operatorManager.Delete(1);

            // Assert
            Assert.Equal(1, result);
            _operatorRepositoryMock.Verify(repo => repo.Delete(1), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldThrowException_WhenOperatorDoesNotExist()
        {
            // Arrange
            _operatorRepositoryMock.Setup(repo => repo.Delete(1)).ReturnsAsync((int?)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _operatorManager.Delete(1));
        }

        [Fact]
        public async Task GetOperatorByEmail_ShouldReturnOperator_WhenExists()
        {
            // Arrange
            var @operator = new Operator { Email = "test@example.com", Nickname = "test", IsAdmin = true, Password = "123" };
            _operatorRepositoryMock.Setup(repo => repo.GetOperatorByEmail(@operator.Email)).ReturnsAsync(@operator);

            // Act
            var result = await _operatorManager.GetOperatorByEmail(@operator.Email);

            // Assert
            Assert.Equal(@operator, result);
            _operatorRepositoryMock.Verify(repo => repo.GetOperatorByEmail(@operator.Email), Times.Once);
        }

        [Fact]
        public async Task GetOperatorByEmail_ShouldThrowException_WhenDoesNotExist()
        {
            // Arrange
            _operatorRepositoryMock.Setup(repo => repo.GetOperatorByEmail("missing@example.com")).ReturnsAsync((Operator)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _operatorManager.GetOperatorByEmail("missing@example.com"));
        }

        [Fact]
        public async Task ValidateOperatorCredentials_ShouldReturnOperator_WhenCredentialsAreValid()
        {
            // Arrange
            var @operator = new Operator { Email = "valid@example.com", Password = "hashedPassword", Salt = "salt", Nickname = "test", IsAdmin = true };
            _operatorRepositoryMock.Setup(repo => repo.GetOperatorByEmail(@operator.Email)).ReturnsAsync(@operator);
            _encryptMock.Setup(encrypt => encrypt.HashPassword("password123", @operator.Salt)).Returns("hashedPassword");

            // Act
            var result = await _operatorManager.ValidateOperatorCredentials(@operator.Email, "password123");

            // Assert
            Assert.Equal(@operator, result);
            _operatorRepositoryMock.Verify(repo => repo.GetOperatorByEmail(@operator.Email), Times.Once);
            _encryptMock.Verify(encrypt => encrypt.HashPassword("password123", @operator.Salt), Times.Once);
        }

        [Fact]
        public async Task ValidateOperatorCredentials_ShouldReturnNull_WhenPasswordIsInvalid()
        {
            // Arrange
            var @operator = new Operator { Email = "valid@example.com", Password = "hashedPassword", Salt = "salt", Nickname = "test", IsAdmin = true };
            _operatorRepositoryMock.Setup(repo => repo.GetOperatorByEmail(@operator.Email)).ReturnsAsync(@operator);
            _encryptMock.Setup(encrypt => encrypt.HashPassword("wrongPassword", @operator.Salt)).Returns("wrongHashedPassword");

            // Act
            var result = await _operatorManager.ValidateOperatorCredentials(@operator.Email, "wrongPassword");

            // Assert
            Assert.Null(result);
            _operatorRepositoryMock.Verify(repo => repo.GetOperatorByEmail(@operator.Email), Times.Once);
            _encryptMock.Verify(encrypt => encrypt.HashPassword("wrongPassword", @operator.Salt), Times.Once);
        }

        [Fact]
        public async Task ValidateOperatorCredentials_ShouldThrowException_WhenOperatorDoesNotExist()
        {
            // Arrange
            _operatorRepositoryMock.Setup(repo => repo.GetOperatorByEmail("missing@example.com")).ReturnsAsync((Operator)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _operatorManager.ValidateOperatorCredentials("missing@example.com", "password123"));
        }
    }
}
