using SassRefactorApi.Contracts.IRepositories;
using SassRefactorApi.Contracts.Iservices;
using SassRefactorApi.Models.Auth.Students;

namespace SassRefactorApi.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IAuthRepository _repository;
        public AuthServices(IAuthRepository repository)
        {
            _repository = repository;
        }
        public async Task<VOToken> AttemptLogin(string username, string password)
        {
            VOToken token = new VOToken();
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return token;
            }
            (VOProfile profile, bool isSuccess) = await _repository.AttempLogin(username, password);
            if (isSuccess)
            {
                return token;
            }
            else
            {
                return token;
            }
        }
    }
}