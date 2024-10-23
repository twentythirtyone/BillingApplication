using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Models.Roles;


namespace BillingApplication.Services.Auth
{
    public class Auth : IAuth
    {
        private readonly IConfiguration configuration;

        public Auth(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
       
        public string GenerateJwtToken<T>(T user)
        {
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

            userRoles.Add(UserRoles.ADMIN); // для тестирования, 
            userRoles.Add(UserRoles.OPERATOR);// потом удалить

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
    }
}
