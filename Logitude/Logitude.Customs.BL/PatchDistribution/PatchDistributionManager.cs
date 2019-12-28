using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Utils;
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
        //public const string LoggerSuffix = "Migrate";
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
        public List<PatchDistributionBase> GetPatchDistribution_MinorNotClosed(decimal MajorVersion, int currentClosedDbMinorVersion)
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

        public void Exec(decimal MajorVersion, int lastDbMinorVersion,Action<string> actionShowMessage)
        {

            var patchDistribution_Waiting2Exec = GetPatchDistribution_MinorNotClosed(MajorVersion, lastDbMinorVersion);

            foreach (var minorPatch in patchDistribution_Waiting2Exec)
            {
                int lastDbMigrateLine = GetLastDBMigartionLineThatNotExecuted(minorPatch);

                var scripts = minorPatch
                .GetSortedScripts()
                .Where(r => r.ScriptCounter > lastDbMigrateLine)
                .OrderBy(r => r.ScriptCounter).ToList();

                foreach (var script in scripts)
                {
                    if (!string.IsNullOrWhiteSpace(script.MessageBefore))
                    {
                        actionShowMessage(script.MessageBefore);
                    }
                    
                    ExecDBMigrationLine(minorPatch, script, scripts.Last() == script);

                    if (!string.IsNullOrWhiteSpace(script.MessageAfter))
                    {
                        actionShowMessage(script.MessageAfter);
                    }

                }
            }
        }

        private static int GetLastDBMigartionLineThatNotExecuted(PatchDistributionBase minorPatch)
        {
            int lastDbMigrateLine = 0;
            var myDBMigrationRepository = new DBMigrationRepository(0);
            var currDBMigration =
                myDBMigrationRepository.GetAll()
                .Where(r => r.MajorVersion == minorPatch.MajorVersionYYPRR)
                .Where(r => r.MinorVersion == minorPatch.PatchCounter_Minor)
                .FirstOrDefault();
            if (currDBMigration != null)
            {
                var myDBMigrationLineRepository = new DBMigrationLineRepository(0);
                var lastMigLine = myDBMigrationLineRepository.GetLastExec(currDBMigration.Id);
                if (lastMigLine != null)
                {
                    lastDbMigrateLine = lastMigLine.CounterKey;
                }
            }

            return lastDbMigrateLine;
        }

        private void ExecDBMigrationLine(PatchDistributionBase myPatchDistribution, ScriptDTO script, bool isLast, string approveRemark = null)
        {
            var customContext = CustomContext.GetContext(0);
            var qsDBMigration = new DBMigrationQueryService(customContext);
            var repoDBMigration = new DBMigrationRepository(customContext);
            var repoDBMigrationLine = new DBMigrationLineRepository(customContext);
            DBMigrationLine dBMigrationLine = null;
            DBMigration myDBMigration = null;
            using (var scope = TransactionFactory.GetNewTransaction())
            {
                try
                {

                    Logger.LogMe($"Start ExecDBMigrationLine({myPatchDistribution.MajorVersionYYPRR}.{myPatchDistribution.PatchCounter_Minor}.{script.ScriptCounter})", false);

                    var sqlDDL_NoNeedCommit = script.SqlScript;
                    Logger.LogMe(sqlDDL_NoNeedCommit, false);
                    if (string.IsNullOrWhiteSpace(approveRemark))
                    {
                        (customContext as DbContextBase).ExecuteReaderSingleResult<int>(sqlDDL_NoNeedCommit,
        (dr) =>
        {

            Logger.LogMe($"ExecuteReaderSingleResult: {dr.GetString(0)}", false);
            return 0;
        });

                    }
                    else
                    {
                        Logger.LogMe($"approve patch !!", false);
                    }


                    myDBMigration = repoDBMigration.GetAll()
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




                    approveRemark = approveRemark ?? "";


                    dBMigrationLine = new DBMigrationLine()
                    {
                        DBMigrationId = myDBMigration.Id,
                        CounterKey = script.ScriptCounter,
                        SqlScript = script.SqlScript.Substring(0, Math.Min(1024, script.SqlScript.Length)),
                        ApprovedRemarks = approveRemark.Substring(0, Math.Min(approveRemark.Length, 256))
                };
                    repoDBMigrationLine.Add(dBMigrationLine);
                    customContext.SaveChanges();

                    ///xxx
                    customContext.SaveChanges();
                    scope.Complete();
                    Logger.LogMe($"DBMigrationLine:{myDBMigration.MajorVersion}.{myDBMigration.MinorVersion}.{dBMigrationLine.CounterKey }", false);
                }
                catch (Exception eee)
                {
                    Logger.LogMe(eee.ToString(), false);
                    throw new PatchDistributionException("PatchDistributionException", eee,
                        myPatchDistribution, script, isLast);
                }

            }
        }

        private void Validate(List<PatchDistributionBase> listOfDistributions)
        {
            var myPatchsGroupByMajor = listOfDistributions
                .GroupBy(r => r.MajorVersionYYPRR)
                .Select(g => g.ToList())
               .ToList();
            foreach (List<PatchDistributionBase> majorPatchDist in myPatchsGroupByMajor)
            {

                var majorPatchDistOrderByList = majorPatchDist.OrderBy(r => r.PatchCounter_Minor);
                int seqMinor = 1;

                foreach (var myPatchDistribution in majorPatchDistOrderByList)
                {

                    if (myPatchDistribution.PatchCounter_Minor != seqMinor)
                    {
                        throw new Exception($"PatchDistributionBase Sequnce is not valid /שם המחלקה לא סדרתי " + myPatchDistribution.GetType().AssemblyQualifiedName);
                    }
                    var UpScripts = myPatchDistribution.GetSortedScripts();
                    if (UpScripts.Count() == 0)
                    {
                        throw new Exception($"אין סקריפטים  ?!?!" + myPatchDistribution.GetType().AssemblyQualifiedName + " ");
                    }
                    int seqScript = 1;
                    foreach (var scriptDTO in UpScripts)
                    {

                        if (string.IsNullOrWhiteSpace(scriptDTO.SqlScript))
                        {
                            throw new Exception($"אין סקריפט ?!?!" + myPatchDistribution.GetType().AssemblyQualifiedName + " " + scriptDTO.ScriptCounter);
                        }

                        //if (scriptDTO.SqlScript.TrimEnd(" "[0]).EndsWith(";"))
                        //{
                        //    throw new Exception($"נא להוריד את הפיסיק נקודה בסוף הסקיריפט" + myPatchDistribution.GetType().AssemblyQualifiedName + " " + scriptDTO.ScriptCounter + Environment.NewLine
                        //        +
                        //        scriptDTO.SqlScript);
                        //}

                        if (scriptDTO.ScriptCounter != seqScript)
                        {
                            throw new Exception($"הסקריפט לא סדרתי " + myPatchDistribution.GetType().AssemblyQualifiedName + " " + scriptDTO.ScriptCounter + Environment.NewLine + " צריך להיות " + seqScript);
                        }
                        seqScript++;

                    }
                    seqMinor++;
                }

            }
        }
        public void ApproveLastFailure(PatchDistributionException myPatchDistributionException, string approveRemarks)
        {

            ExecDBMigrationLine(
                myPatchDistributionException.MyPatchDistribution,
                myPatchDistributionException.MyScript, myPatchDistributionException.IsLast,
                approveRemarks);
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

        public DBMigrationLine MyDBMigrationLine { get; }


        public PatchDistributionBase MyPatchDistribution { get; }

        public ScriptDTO MyScript { get; }
        public bool IsLast { get; }

        public PatchDistributionException(string message, Exception inner,
            PatchDistributionBase myPatchDistribution, ScriptDTO myScript, bool isLast) : base(message, inner)
        {
            this.MyPatchDistribution = myPatchDistribution;
            this.MyScript = myScript;
            this.IsLast = isLast;
        }
    }
}
