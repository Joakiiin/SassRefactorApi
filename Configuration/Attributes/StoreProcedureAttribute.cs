namespace SassRefactorApi.Configuration.Attributes
{
    [System.AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class StoreProcedureAttribute : Attribute
    {
        public string Name { get; private set; }
        public StoreProcedureAttribute (string name)
        {
            Name = name;
        }
    }
}