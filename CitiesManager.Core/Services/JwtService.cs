using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CitiesManager.Core.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public AuthenticationResponse CreateJWTToken(ApplicationUser applicationUser)
        {
            DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:expiration_minutes"]));
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,applicationUser.Id.ToString()),//Subject Id
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()), //JWT Unique Id
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.Now.ToString()), //Token generation datetime
                new Claim(ClaimTypes.NameIdentifier,applicationUser.Email.ToString()), //Unique name identifier of user
                new Claim(ClaimTypes.Name,applicationUser.PersonName.ToString()) //Name of the user

            };
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtTokenGenerator = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires:expiration,
                signingCredentials:signingCredentials
                );
            // Create a JwtSecurityTokenHandler object and use it to write the token as a string.
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string token = tokenHandler.WriteToken(jwtTokenGenerator);

            // Create and return an AuthenticationResponse object containing the token, user email, user name, and token expiration time.
            return new AuthenticationResponse() { Token = token, Email = applicationUser.Email, PersonName = applicationUser.PersonName, Expiration = expiration };

        }
    }
}
