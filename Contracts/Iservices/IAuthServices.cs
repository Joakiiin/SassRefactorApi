using SassRefactorApi.Models.Auth.Students;

namespace SassRefactorApi.Contracts.Iservices
{
    public interface IAuthServices
    {
        Task<VOToken> AttemptLogin(string username, string password);
    }
}