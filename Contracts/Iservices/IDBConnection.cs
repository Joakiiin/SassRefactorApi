using System.Data;

namespace SassRefactorApi.Contracts.Iservices
{
    public interface IDBConnection
    {
        public IDbConnection Instance { get; }
        void Close();
    }
}