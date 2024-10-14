using System.Data;

namespace SassRefactorApi.Configuration.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class StoreParameterAttribute : Attribute
    {
        public string? Name { get; private set; }
        public DbType Type { get; private set; }
        public ParameterDirection Direction { get; private set; }
        public StoreParameterAttribute(string name)
        {
            Name = name;
        }
        public StoreParameterAttribute(DbType type)
        {
            Type = type;
        }
        public StoreParameterAttribute(ParameterDirection direction)
        {
            Direction = direction;
        }
    }
}