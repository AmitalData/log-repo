using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution
{
    public class PatchDistributionManager
    {
        public List<PatchDistributionBase> Check_PatchDistributionListAreValid()
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
        public List<PatchDistributionBase> GetPatchDistribution_Waiting2Exec(decimal MajorVersion, int currentClosedDbMinorVersion)
        {
            List<PatchDistributionBase> listOfDistributions = Check_PatchDistributionListAreValid();
            var listOfDistributionScripts =
                listOfDistributions
                .Where(r => r.MajorVersionYYPRR == MajorVersion)
                .Where(r => r.PatchCounter_Minor > currentClosedDbMinorVersion)
                .OrderBy(r => r.PatchCounter_Minor)
                //.SelectMany(r => r.GetUpScripts().OrderBy(r1 => r1.ScriptCounter))
                //.Select(r => r.ToString())

                .ToList();


            return listOfDistributionScripts;


        }

        public void Exec(decimal MajorVersion, int currentDbMinorVersion)
        {

            GetPatchDistribution_Waiting2Exec(MajorVersion, currentDbMinorVersion)
                    .ForEach(myPatchDistribution =>
                {
                    var scripts = myPatchDistribution.GetUpScripts().OrderBy(r => r.ScriptCounter).ToList();
                    scripts.ForEach(script =>
                    {
                        ExecDBMigrationLine(myPatchDistribution, script, scripts.Last() == script);
                    });
                });
        }

        private static void ExecDBMigrationLine(PatchDistributionBase myPatchDistribution, ScriptDTO script, bool isLast)
        {
            var customContext = CustomContext.GetContext(0);
            var qsDBMigration = new DBMigrationQueryService(customContext);
            var repoDBMigration = new DBMigrationRepository(customContext);
            var repoDBMigrationLine = new DBMigrationLineRepository(customContext);
            DBMigrationLine dBMigrationLine = null;
            using (var scope = TransactionFactory.GetNewTransaction())
            {

                Debug.WriteLine($"ExecDBMigrationLine({myPatchDistribution.MajorVersionYYPRR}.{script.ScriptCounter})");
                var myDBMigration = repoDBMigration.GetAll()
.Where(dbMigration => dbMigration.MajorVersion == myPatchDistribution.MajorVersionYYPRR)
.Where(dbMigration => dbMigration.MinorVersion == myPatchDistribution.PatchCounter_Minor)
.FirstOrDefault();
                if (myDBMigration == null)
                {
                    var rem = (myPatchDistribution.PatchDetails ?? "NotSet");
                    myDBMigration = new Customs.Data.EntityPOCOs.DBMigration()
                    {

                        Id = (new IdCounterWrapper()).GetNumber("Customs.DBMigration", 0),
                        MajorVersion = myPatchDistribution.MajorVersionYYPRR,
                        MinorVersion = myPatchDistribution.PatchCounter_Minor,
                        ExecuteDate = DateTime.Now,
                        Remarks = rem.Substring(0, Math.Min(256, rem.Length)),
                        IsClose = isLast

                    };
                    repoDBMigration.Add(myDBMigration);
                }
                else
                {
                    myDBMigration.IsClose = isLast;
                    repoDBMigration.Update(myDBMigration);
                }


                try
                {
                    dBMigrationLine = new DBMigrationLine()
                    {
                        DBMigrationId = myDBMigration.Id,
                        CounterKey = script.ScriptCounter,
                        SqlScript = script.SqlScript.Substring(0, Math.Min(1024, script.SqlScript.Length)),
                    };
                    repoDBMigrationLine.Add(dBMigrationLine);
                    customContext.SaveChanges();

                    var sqlDDL_NoNeedCommit = script.SqlScript;

                    (customContext as DbContextBase).ExecuteReaderSingleResult<int>(sqlDDL_NoNeedCommit,
    (dr) =>
    {

        Debug.WriteLine($"ExecuteReaderSingleResult: {dr.GetString(0)}");
        return 0;
    });


                    customContext.SaveChanges();
                    scope.Complete();
                    Debug.WriteLine("DBMigrationLine:{myDBMigration.Id}.{CounterKey }");
                }
                catch (Exception eee)
                {
                    Debug.WriteLine(eee.ToString());
                    throw new PatchDistributionException("PatchDistributionException",eee, dBMigrationLine);
                }

            }
        }

        private void Validate(List<PatchDistributionBase> listOfDistributions)
        {
            var myPatchs = listOfDistributions
                .GroupBy(r => r.MajorVersionYYPRR)
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

    public class PatchDistributionException : Exception
    {

        public DBMigrationLine MyDBMigrationLine { get;  }
        
        public PatchDistributionException(string message, Exception inner, DBMigrationLine myDBMigrationLine) : base(message, inner) { MyDBMigrationLine = myDBMigrationLine; }
    }
}
