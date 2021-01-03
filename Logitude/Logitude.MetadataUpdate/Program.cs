using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.MetadataUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            string moduleName = "UpdateTenantZeroNew";
            if (args.Length > 0 && !string.IsNullOrEmpty(args[0]))
            {
                moduleName = args[0];
            }
            moduleName = "Customs";
            MetadataUpdateService metadataUpdateService = new MetadataUpdateService();
            metadataUpdateService.RunModulesUpdate(moduleName);
        }
    }
}
