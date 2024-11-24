using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Models.Roles;
using BillingApplication.Server.Middleware;
using Newtonsoft.Json.Linq;


namespace BillingApplication.Services.Auth
{
    public class Auth : IAuth
    {
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IBlacklistService blacklistService;

        public Auth(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IBlacklistService blacklistService)
        {
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            this.blacklistService = blacklistService;
        }

        public int? GetCurrentUserId()
        {
            var claimsIdentity = httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;
            int id = int.Parse(claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");
            return id;
        }


        public List<string> GetCurrentUserRoles()
        {
            var claimsIdentity = httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;
            return claimsIdentity?.FindAll(ClaimTypes.Role).Select(claim => claim.Value).ToList() ?? new List<string>();
        }

        public string GenerateJwtToken<T>(T user)
        {
            Console.WriteLine($"Generating token for user: {(user as IUser)?.UniqueId}");
            var userRoles = new List<string>();

            if (user is Subscriber subscriber)
                userRoles.Add(UserRoles.USER);
            else if (user is Operator @operator)
            {
                if (@operator.IsAdmin)
                    userRoles.Add(UserRoles.ADMIN);
                userRoles.Add(UserRoles.OPERATOR);
            }
            else
                throw new ArgumentException("Неподдерживаемый тип пользователя");


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, (user as IUser)?.UniqueId ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void Logout(string token)
        {
            blacklistService.AddTokenToBlacklist(token);
        }
    }
}
