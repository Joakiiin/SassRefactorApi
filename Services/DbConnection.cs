using System.Data;
using MySql.Data.MySqlClient;
using SassRefactorApi.Contracts.Iservices;
using SassRefactorApi.Models.Connection;

namespace SassRefactorApi.Services
{
    public class DbConnection : IDBConnection, IDisposable
    {
        private readonly VOEnvironment _environment = new VOEnvironment();
        private IDbConnection? _connection = null;
        private bool _disposed = false;

        public DbConnection(IConfiguration configuration)
        {
            configuration.GetSection("Environment").Bind(_environment);
        }

        public IDbConnection Instance
        {
            get
            {
                if (_connection == null || _connection.State == ConnectionState.Closed)
                {
                    var connectionString = GenerateConnectionString();
                    _connection = new MySqlConnection(connectionString);
                    _connection.Open();
                }
                return _connection;
            }
        }

        private string GenerateConnectionString()
        {
            return string.Format("server={0};port=3306;user={1};database={2};password={3}",
              _environment.Host,
              _environment.Username,
              "dbsass",
              _environment.Password);
        }

        public void Close()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Close();
                    _connection?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
