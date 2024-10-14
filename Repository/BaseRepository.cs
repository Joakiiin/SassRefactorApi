using System.Data;
using System.Reflection;
using Dapper;
using SassRefactorApi.Configuration.Attributes;
using SassRefactorApi.Configuration.Tools;

namespace SassRefactorApi.Repository
{
    public abstract class BaseRepository
    {
        protected string Schema { get; private set; } = string.Empty;
        protected string StoreProcedure { get; private set; } = string.Empty;
        protected DynamicParameters? Parameters { get; private set; }
        protected string Query { get; set; } = string.Empty;
        protected void SetStoreProcedure<T>(T t)
        {
            Schema = GetSchema(typeof(T));
            StoreProcedure = GetStoreProcedure(typeof(T));
            Parameters = GetStorePropertiesInput(t, typeof(T));
            List<string> args = new List<string>();
            Parameters.ParameterNames.ToList().ForEach(p =>
            {
                args.Add($"@{p}");
            });
            string arg = args.Count() > 0 ? string.Format("({0})", string.Join(", ", args)) : string.Empty;
            //Query = $"CALL {Schema}.{StoreProcedure}{arg}";
            Query = $"{Schema}.{StoreProcedure}";
        }
        protected string GetSchema(Type t)
        {
            object? attr = t.GetCustomAttributes(true).FirstOrDefault(attr => attr is SchemaAttribute);
            return attr != null ? ((SchemaAttribute)attr).Name : "";
        }
        protected string GetStoreProcedure(Type t)
        {
            object? attr = t.GetCustomAttributes(true).FirstOrDefault(attr => attr is StoreProcedureAttribute);
            return attr != null ? ((StoreProcedureAttribute)attr).Name : "";
        }
        protected DynamicParameters GetStorePropertiesInput<X>(X x, Type t)
        {
            DynamicParameters parameters = new DynamicParameters();
            PropertyInfo[] props = t.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    try
                    {
                        StoreParameterAttribute? paramAttr = attr as StoreParameterAttribute;
                        if (paramAttr != null)
                        {
                            string paramName = string.IsNullOrEmpty(paramAttr.Name) ? prop.Name : paramAttr.Name;
                            object? value = prop.GetValue(x, null);
                            DbType dbType = TypeToDBType.GetDbType(value);
                            ParameterDirection direction = paramAttr.Direction;
                            parameters.Add($"@{paramName}", value, dbType, direction);
                        }
                    }
                    catch (Exception ex)
                    {

                        throw new Exception(ex.Message);
                    }
                }
            }
            return parameters;
        }
    }
}