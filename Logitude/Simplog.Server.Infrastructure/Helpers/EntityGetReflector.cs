using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.Helpers
{
    public class EntityGetReflector
    {
        public string EntityName { get; set; }
        public object[] Parameters { get; set; }
        public string MethodName { get; set; }
        public int Tenant { get; set; }
    }
}
