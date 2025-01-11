using BillingApplication.Entities;
using BillingApplication;
using BillingApplication.Server.DataLayer.Repositories.Implementations;
using BillingApplication.Server.Mapper;
using BillingApplication.Services.Models.Roles;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BillingApplication.Tests;

public class OperatorRepositoryTests
{
    private readonly DbContextOptions<BillingAppDbContext> _dbContextOptions;

    public OperatorRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<BillingAppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Create_ValidOperator_ReturnsOperatorId()
    {
        // Arrange
        await using var context = new BillingAppDbContext(_dbContextOptions);
        var repository = new OperatorRepository(context);

        var operatorModel = new Operator
        {
            Nickname = "TestNick",
            Email = "test@operator.com",
            Password = "SecurePassword123",
            IsAdmin = false,
            Salt = "GeneratedSaltValue"
        };

        // Act
        var result = await repository.Create(operatorModel);

        // Assert
        Assert.NotNull(result);
        var createdOperator = await context.Operators.FindAsync(result);
        Assert.NotNull(createdOperator);
        Assert.Equal("TestNick", createdOperator.Nickname);
        Assert.Equal("test@operator.com", createdOperator.Email);
        Assert.Equal("SecurePassword123", createdOperator.Password);
        Assert.False(createdOperator.IsAdmin);
    }

    [Fact]
    public async Task Delete_ExistingOperator_ReturnsOperatorId()
    {
        // Arrange
        await using var context = new BillingAppDbContext(_dbContextOptions);
        var repository = new OperatorRepository(context);

        var operatorEntity = new OperatorEntity
        {
            Nickname = "TestNick",
            Email = "test@operator.com",
            Password = "SecurePassword123",
            IsAdmin = false,
            Salt = "GeneratedSaltValue"
        };
        context.Operators.Add(operatorEntity);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.Delete(operatorEntity.Id);

        // Assert
        Assert.NotNull(result);
        var deletedOperator = await context.Operators.FindAsync(result);
        Assert.Null(deletedOperator);
    }

    [Fact]
    public async Task GetAll_ReturnsAllOperators()
    {
        // Arrange
        await using var context = new BillingAppDbContext(_dbContextOptions);
        var repository = new OperatorRepository(context);

        context.Operators.AddRange(
            new OperatorEntity
            {
                Nickname = "Operator1",
                Email = "op1@operator.com",
                Password = "Password1",
                IsAdmin = false,
                Salt = "GeneratedSaltValue"
            },
            new OperatorEntity
            {
                Nickname = "Operator2",
                Email = "op2@operator.com",
                Password = "Password2",
                IsAdmin = true,
                Salt = "GeneratedSaltValue"
            }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetOperatorById_ExistingOperator_ReturnsOperator()
    {
        // Arrange
        await using var context = new BillingAppDbContext(_dbContextOptions);
        var repository = new OperatorRepository(context);

        var operatorEntity = new OperatorEntity
        {
            Nickname = "TestNick",
            Email = "test@operator.com",
            Password = "SecurePassword123",
            IsAdmin = false,
            Salt = "GeneratedSaltValue"
        };
        context.Operators.Add(operatorEntity);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetOperatorById(operatorEntity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestNick", result.Nickname);
        Assert.Equal("test@operator.com", result.Email);
        Assert.Equal("SecurePassword123", result.Password);
        Assert.False(result.IsAdmin);
    }

    [Fact]
    public async Task GetOperatorByEmail_ExistingOperator_ReturnsOperator()
    {
        // Arrange
        await using var context = new BillingAppDbContext(_dbContextOptions);
        var repository = new OperatorRepository(context);

        var operatorEntity = new OperatorEntity
        {
            Nickname = "TestNick",
            Email = "test@operator.com",
            Password = "SecurePassword123",
            IsAdmin = true,
            Salt = "GeneratedSaltValue"
        };
        context.Operators.Add(operatorEntity);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetOperatorByEmail("test@operator.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestNick", result.Nickname);
        Assert.Equal("test@operator.com", result.Email);
        Assert.Equal("SecurePassword123", result.Password);
        Assert.True(result.IsAdmin);
    }

    [Fact]
    public async Task Update_ExistingOperator_UpdatesOperatorDetails()
    {
        // Arrange
        await using var context = new BillingAppDbContext(_dbContextOptions);
        var repository = new OperatorRepository(context);

        var operatorEntity = new OperatorEntity
        {
            Nickname = "TestNick",
            Email = "test@operator.com",
            Password = "SecurePassword123",
            IsAdmin = false,
            Salt = "GeneratedSaltValue"
        };
        context.Operators.Add(operatorEntity);
        await context.SaveChangesAsync();

        var updatedModel = new Operator
        {
            Id = operatorEntity.Id,
            Nickname = "UpdatedNick",
            Email = "updated@operator.com",
            Password = "UpdatedPassword123",
            IsAdmin = true,
            Salt = "GeneratedSaltValue"
        };

        // Act
        var result = await repository.Update(updatedModel);

        // Assert
        Assert.NotNull(result);
        var updatedOperator = await context.Operators.FindAsync(result);
        Assert.NotNull(updatedOperator);
        Assert.Equal("UpdatedNick", updatedOperator.Nickname);
        Assert.Equal("updated@operator.com", updatedOperator.Email);
        Assert.Equal("UpdatedPassword123", updatedOperator.Password);
        Assert.True(updatedOperator.IsAdmin);
    }
}
