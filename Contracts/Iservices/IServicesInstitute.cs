using SassRefactorApi.Models.Institute;

namespace SassRefactorApi.Contracts.Iservices
{
    public interface IServicesInstitute
    {
        Task<VOInstitute> GetInstitute(string username = null, string idUser = null);
    }
}