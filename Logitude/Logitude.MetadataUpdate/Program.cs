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
            MetadataUpdateService metadataUpdateService = new MetadataUpdateService();
            metadataUpdateService.RunAllModulesUpdate();
        }
    }
}
