namespace SassRefactorApi.Configuration.Attributes
{
    [System.AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SchemaAttribute : Attribute
    {
        public string Name { get; private set; }
        public SchemaAttribute (string name)
        {
            Name = name;
        }
    }
}