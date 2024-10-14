using Dapper;
using SassRefactorApi.Contracts.IRepositories;
using SassRefactorApi.Contracts.Iservices;
using SassRefactorApi.Models.Institute;
using SassRefactorApi.Models.StoreProcedures;
using SassRefactorApi.Models.StoreProcedures.Entities;
using System.Data;
namespace SassRefactorApi.Repository
{
    public class InstituteRepository : BaseRepository, IReposioryInstitute
    {
        private readonly IDBConnection _connection;
        public InstituteRepository(IDBConnection connection)
        {
            _connection = connection;
        }
        public async Task<VOInstitute> GetInstitute(string dataUser)
        {
            VOInstitute institute = new VOInstitute();
            SP00001Q sp = new SP00001Q();
            sp.PR_INPUT = dataUser;
            SetStoreProcedure<SP00001Q>(sp);
            SP00001QEntity? result = _connection.Instance.QueryFirstOrDefault<SP00001QEntity>(Query, Parameters, commandType: CommandType.StoredProcedure);
            if (result != null)
            {
                institute.Escuela = result?.schoolId?.Trim() ?? string.Empty;
                institute.Instituto = result?.instituteId?.Trim() ?? string.Empty;
            }
            return institute;
        }
    }
}