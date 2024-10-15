using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SassRefactorApi.Contracts.IRepositories;
using SassRefactorApi.Contracts.Iservices;
using SassRefactorApi.Models.Auth.Students;

namespace SassRefactorApi.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _repository;
        public AuthServices(IAuthRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }
        public async Task<VOToken> AttemptLogin(string username, string password)
        {
            VOToken token = new VOToken();
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return token;
            }
            (VOProfile profile, bool isSuccess) = await _repository.AttempLogin(username, password);
            string jwt = isSuccess ? GenerateJWT(profile) : string.Empty;
            token.JWT = !string.IsNullOrEmpty(jwt) ? jwt : null;
            token.Message = !string.IsNullOrEmpty(jwt) ? "La autentificacion ha sido exitosa" : null;
            token.isActive = !string.IsNullOrEmpty(jwt);
            return token;
        }
        private string GenerateJWT(VOProfile profile)
        {
            Claim[] claims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, profile.NoControl),
                new Claim(JwtRegisteredClaimNames.Name, profile.Nombre),
                new Claim(JwtRegisteredClaimNames.NameId, profile.Apellido),
                new Claim("preferred_username", profile.Correo)
            };
            //SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials signIn = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signIn
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}