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
        public List<PatchDistributionBase> GetValidPatchDistributionList()
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

            Validate(listOfDistributions);
            listOfDistributions = listOfDistributions.OrderBy(r => r.Branch).ThenBy(r => r.PatchCounter_Minor).ToList();


            return listOfDistributions;

        }
        public List<PatchDistributionBase> GetPatchDistributionToDo(decimal MajorVersion, int currentDbMinorVersion)
        {
            List<PatchDistributionBase> listOfDistributions = GetValidPatchDistributionList();
            var listOfDistributionScripts =
                listOfDistributions
                .Where(r => r.MajorVersion == MajorVersion)
                .Where(r => r.PatchCounter_Minor > currentDbMinorVersion)
                //.SelectMany(r => r.GetUpScripts().OrderBy(r1 => r1.ScriptCounter))
                //.Select(r => r.ToString())
                .ToList();


            return listOfDistributionScripts;


        }

        private void Validate(List<PatchDistributionBase> listOfDistributions)
        {
            var myPatchs = listOfDistributions
                .GroupBy(r => r.MajorVersion)
                .Select(g => g.ToList())
               .ToList();
            myPatchs.ForEach(g =>
            {
                var l =
                g.OrderBy(r => r.PatchCounter_Minor)
                .Select((r, seq) =>
                {
                    if (r.PatchCounter_Minor != seq)
                    {
                        throw new Exception($"PatchDistributionBase Sequnce is not valid " + r.GetType().AssemblyQualifiedName);
                    }
                    return r;
                }
                ); ;




            });




        }

    }
    public class AssemblyDBMigrationModel
    {

        public string p_d { get; internal set; }
        public string YY { get; internal set; }
        public string YearRelease { get; internal set; }
        public string MinorVer { get; internal set; }

        public decimal MajorVersion { get; internal set; }

        public decimal MinorVersion { get; internal set; }

        public static AssemblyDBMigrationModel Parser(string Version)
        {
            //Version=1.P.19.03.0
            var parts = Version.Split('.');
            string p_d = parts[1];
            string YY = parts[2];
            string YearRelease = parts[3];

            string MinorVer = parts[4];

            return new AssemblyDBMigrationModel()
            {
                p_d = p_d,
                YY = YY,
                YearRelease = YearRelease,
                MinorVer = MinorVer,
                MajorVersion = decimal.Parse($"{YY}.{YearRelease}"),
                MinorVersion = int.Parse(MinorVer)

            };

        }

    }
}
