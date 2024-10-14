using SassRefactorApi.Contracts.IRepositories;
using SassRefactorApi.Contracts.Iservices;
using SassRefactorApi.Models.Institute;
using SassRefactorApi.Repository;

namespace SassRefactorApi.Services
{
    public class InstituteServices : IServicesInstitute
    {
        private readonly IReposioryInstitute _repository;
        private VOInstitute _institute = null;
        public InstituteServices(IReposioryInstitute repository)
        {
            _repository = repository;
        }
        public async Task<VOInstitute> GetInstitute(string username = null, string idUser = null)
        {
            if (username != null)
            {
                _institute = await _repository.GetInstitute(username);
            }
            else
            {
                _institute = await _repository.GetInstitute(idUser);
            }
            return _institute;
        }
    }
}