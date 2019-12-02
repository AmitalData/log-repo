using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution
{
    public class PatchDistributionManager
    {
        public void CreateList()
        {
            var patchDistributionClassS =
                Assembly.GetAssembly(typeof(PatchDistributionBase))
                .GetTypes()
                .Where(myType => myType.IsSubclassOf(typeof(PatchDistributionBase)))
                .Where(myType => myType.IsClass)
                .Where(myType => !myType.IsAbstract).ToList();

            //patchDistributionClassS = patchDistributionClassS.OrderBy(r => r.AssemblyQualifiedName).ToList();

            var listOfDistributions =
            patchDistributionClassS.Select(objectType =>
           {
               var patchDistributionInstance = Activator.CreateInstance(objectType) as PatchDistributionBase;
               return patchDistributionInstance;

           }).ToList();

            listOfDistributions = listOfDistributions.OrderBy(r => r.Branch).ThenBy(r => r.PatchCounter).ToList();
            var mySqlScripts = 
                listOfDistributions
                .SelectMany(r => r.GetUpScripts().OrderBy(r1 => r1.ScriptCounter))
                .Select(r => r.ToString())
                .ToList();
            

        }
    }
}
