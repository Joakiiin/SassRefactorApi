using SassRefactorApi.Models.Auth.Students;

namespace SassRefactorApi.Contracts.IRepositories
{
    public interface IAuthRepository
    {
        Task<(VOProfile profile, bool isSuccess)> AttempLogin(string username, string password);
    }
}