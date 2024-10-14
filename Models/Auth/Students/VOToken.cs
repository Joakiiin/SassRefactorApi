using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SassRefactorApi.Models.Auth.Students
{
    public class VOToken
    {
        public string JWT { get; set; }
        public string Message { get; set; }
        public bool isActive { get; set; }
    }
}