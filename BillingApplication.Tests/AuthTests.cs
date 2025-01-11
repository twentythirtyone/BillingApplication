using BillingApplication.Server.Middleware;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Auth;
using BillingApplication.Services.Models.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace BillingApplication.Tests;

public class AuthTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IBlacklistService> _blacklistServiceMock;
    private readonly Auth _auth;

    public AuthTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _blacklistServiceMock = new Mock<IBlacklistService>();

        _configurationMock.Setup(config => config["secret"]).Returns("verybigsecretkeyitsveryhardtoknowwhatisthekeytomyapplicationomgimsosmartwow");
        _configurationMock.Setup(config => config["Jwt:Issuer"]).Returns("TestIssuer");
        _configurationMock.Setup(config => config["Jwt:Audience"]).Returns("TestAudience");
        _configurationMock.Setup(config => config["Jwt:ExpiresInMinutes"]).Returns("60");

        _auth = new Auth(_configurationMock.Object, _httpContextAccessorMock.Object, _blacklistServiceMock.Object);
    }

    [Fact]
    public void GetCurrentUserId_ShouldReturnUserId_WhenClaimsExist()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "123")
        };
        var claimsIdentity = new ClaimsIdentity(claims);
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(claimsIdentity)
        };
        _httpContextAccessorMock.Setup(accessor => accessor.HttpContext).Returns(httpContext);

        // Act
        var userId = _auth.GetCurrentUserId();

        // Assert
        Assert.Equal(123, userId);
    }

    [Fact]
    public void GetCurrentUserId_ShouldReturnNull_WhenNoClaimsExist()
    {
        // Arrange
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity())
        };
        _httpContextAccessorMock.Setup(accessor => accessor.HttpContext).Returns(httpContext);

        // Act
        var userId = _auth.GetCurrentUserId();

        // Assert
        Assert.Equal(-1, userId);
    }

    [Fact]
    public void GetCurrentUserRoles_ShouldReturnRoles_WhenRolesExist()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.Role, "User")
        };
        var claimsIdentity = new ClaimsIdentity(claims);
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(claimsIdentity)
        };
        _httpContextAccessorMock.Setup(accessor => accessor.HttpContext).Returns(httpContext);

        // Act
        var roles = _auth.GetCurrentUserRoles();

        // Assert
        Assert.Contains("Admin", roles);
        Assert.Contains("User", roles);
    }

    [Fact]
    public void GetCurrentUserRoles_ShouldReturnEmptyList_WhenNoRolesExist()
    {
        // Arrange
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity())
        };
        _httpContextAccessorMock.Setup(accessor => accessor.HttpContext).Returns(httpContext);

        // Act
        var roles = _auth.GetCurrentUserRoles();

        // Assert
        Assert.Empty(roles);
    }

    [Fact]
    public void GenerateJwtToken_ShouldGenerateToken_ForSubscriber()
    {
        // Arrange
        var subscriber = new Subscriber { Id = 123, Number = "1234567890" };

        // Act
        var token = _auth.GenerateJwtToken(subscriber);

        // Assert
        Assert.NotNull(token);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == "123");
        Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == UserRoles.USER);
    }

    [Fact]
    public void GenerateJwtToken_ShouldGenerateToken_ForOperator()
    {
        // Arrange
        var operatorUser = new Operator { Id = 456, IsAdmin = true, Email = "test", Nickname = "test", Password = "test" };

        // Act
        var token = _auth.GenerateJwtToken(operatorUser);

        // Assert
        Assert.NotNull(token);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == "456");
        Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == UserRoles.ADMIN);
        Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == UserRoles.OPERATOR);
    }

    [Fact]
    public void GenerateJwtToken_ShouldThrowException_ForUnsupportedUserType()
    {
        // Arrange
        var unsupportedUser = new { UniqueId = "789" };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _auth.GenerateJwtToken(unsupportedUser));
    }

    [Fact]
    public void Logout_ShouldAddTokenToBlacklist()
    {
        // Arrange
        var token = "sampleToken";

        // Act
        _auth.Logout(token);

        // Assert
        _blacklistServiceMock.Verify(service => service.AddTokenToBlacklist(token), Times.Once);
    }
}
