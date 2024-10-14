using SassRefactorApi.Models.Institute;

namespace SassRefactorApi.Contracts.IRepositories
{
    public interface IReposioryInstitute
    {
        Task<VOInstitute> GetInstitute(string dataUser);
    }
}