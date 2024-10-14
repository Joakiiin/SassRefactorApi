using System.Data;
using SassRefactorApi.Configuration.Attributes;
namespace SassRefactorApi.Models.StoreProcedures
{
    [Schema("DBSASS")]
    [StoreProcedure("SP0002Q")]
    public class SP00002Q
    {
        [StoreParameter(ParameterDirection.Input)]
        public int PR_INSTITUTEID  { get; set; }
        [StoreParameter(ParameterDirection.Input)]
        public int PR_SCHOOLID  { get; set; }
        [StoreParameter(ParameterDirection.Input)]
        public string PR_INPUT  { get; set; }
        [StoreParameter(ParameterDirection.Output)]
        public string PR_RESULT  { get; set; }
    }
}