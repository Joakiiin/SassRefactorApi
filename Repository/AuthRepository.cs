using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using SassRefactorApi.Contracts.IRepositories;
using SassRefactorApi.Contracts.Iservices;
using SassRefactorApi.Models.Auth.Students;
using SassRefactorApi.Models.Institute;
using SassRefactorApi.Models.StoreProcedures;
using SassRefactorApi.Models.StoreProcedures.Entities;

namespace SassRefactorApi.Repository
{
    public class AuthRepository : BaseRepository, IAuthRepository
    {
        private readonly IServicesInstitute _servicesInstitute;
        private readonly IDBConnection _connection;
        public AuthRepository(IServicesInstitute servicesInstitute, IDBConnection connection)
        {
            _servicesInstitute = servicesInstitute;
            _connection = connection;
        }
        public async Task<(VOProfile profile, bool isSuccess)> AttempLogin(string username, string password)
        {
            VOInstitute institute = await _servicesInstitute.GetInstitute(username);
            VORequestParams parameter = new VORequestParams();

            if (institute != null)
            {
                parameter.instituteId = institute?.Instituto?.Trim() ?? string.Empty;
                parameter.schoolId = institute?.Escuela?.Trim() ?? string.Empty;
                parameter.parameter = $"{username}|{password}";
                parameter.result = "";
            }
            return await AttempLogin(parameter);
        }

        private async Task<(VOProfile profile, bool isSuccess)> AttempLogin(VORequestParams parameter)
        {
            VOProfile profile = new VOProfile();
            bool isSuccess = false;

            if (parameter != null)
            {
                SP00002Q sp = new SP00002Q
                {
                    PR_INSTITUTEID = int.Parse(parameter.instituteId),
                    PR_SCHOOLID = int.Parse(parameter.schoolId),
                    PR_INPUT = parameter.parameter,
                    PR_RESULT = parameter.result
                };

                SetStoreProcedure<SP00002Q>(sp);

                // Obtener el resultado
                SP00002QEntity? result = await _connection.Instance.QueryFirstOrDefaultAsync<SP00002QEntity>(Query, Parameters, commandType: CommandType.StoredProcedure);

                if (result != null)
                {
                    profile.NoControl = result?.idUser?.Trim() ?? string.Empty;
                    profile.Nombre = result?.nombre?.Trim() ?? string.Empty;
                    profile.Apellido = result?.apellido?.Trim() ?? string.Empty;
                    profile.Correo = result?.username?.Trim() ?? string.Empty;
                    profile.Institucion = result?.instituteName?.Trim() ?? string.Empty;
                    profile.Escuela = result?.schoolName?.Trim() ?? string.Empty;

                    // Verificar el resultado
                    string resultMessage = Parameters.Get<string>("@PR_RESULT");
                    isSuccess = resultMessage.Equals("OK", StringComparison.OrdinalIgnoreCase);
                }
            }
            return (profile, isSuccess);
        }
    }
}