using System.Data;
using SassRefactorApi.Configuration.Attributes;

namespace SassRefactorApi.Models.StoreProcedures
{
    [Schema("DBSASS")]
    [StoreProcedure("SP0001Q")]
    public class SP00001Q
    {
        [StoreParameter(ParameterDirection.Input)]
        public string PR_INPUT  { get; set; }
    }
}