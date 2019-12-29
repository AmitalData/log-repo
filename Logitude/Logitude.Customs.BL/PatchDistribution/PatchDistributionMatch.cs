using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution
{
    public class PatchDistributionMatch
    {

        public enum MajorVersionMatchEnum
        {
            NotDistributionBranch,//burn not in distribution branch
            OldSource,
            OldDB,
            OK_DBAndAssemblyREqual
        }
        public PatchDistributionMatchModel GetPatchDistributionMatchModel(string assemblyVersion)
        {

            var myDBMigrationQueryService = new DBMigrationQueryService(0);
            var myLastClosed_DBMigration = myDBMigrationQueryService.GetLastClosedPM();//Must Have

            //??
            //new DBMigrationPM()
            //{
            //    MajorVersion = 19.03m,
            //    MinorVersion = 0,
            //     //DBMigrationLines = new List<DBMigrationLinePM>()
            //     //{

            //     //}

            //};




            var myAssemblyDBMigrationModel = AssemblyDBMigrationModel.Parser(assemblyVersion);
            if (myAssemblyDBMigrationModel.MajorVersion==0.0m)
            {
                return new PatchDistributionMatchModel() { };
            }
            var myPatchDistributionMatchModel = new PatchDistributionMatchModel
            {
                LastClosed_DBMigration = myLastClosed_DBMigration,
                MyAssemblyDBMigrationModel = myAssemblyDBMigrationModel
            };
            if (myAssemblyDBMigrationModel.p_d != "D")
            {
                myPatchDistributionMatchModel.Message = "burn not in distribution branch";
                myPatchDistributionMatchModel.NotDistributionBranch = true;//MessageBox.Show("burn not in distribution branch");

            }



            if (myAssemblyDBMigrationModel.MajorVersion < myLastClosed_DBMigration.MajorVersion)
            {
                myPatchDistributionMatchModel.Message = /*MessageBox.Show*/($"DataBase Major Version is newer -  Assembly:{myAssemblyDBMigrationModel.MajorVersion} < DB Migration:{myLastClosed_DBMigration.MajorVersion}");
                //this.Close();
                myPatchDistributionMatchModel.MajorVersionMatch = MajorVersionMatchEnum.OldSource;
            }
            else if (myAssemblyDBMigrationModel.MajorVersion < myLastClosed_DBMigration.MajorVersion)
            {
                myPatchDistributionMatchModel.Message = /*MessageBox.Show*/($"DataBase Major Version is older -  Assembly:{myAssemblyDBMigrationModel.MajorVersion} > DB Migration:{myLastClosed_DBMigration.MajorVersion}");
                //this.Close();
                myPatchDistributionMatchModel.MajorVersionMatch = MajorVersionMatchEnum.OldDB;
            }
            else
            {
                myPatchDistributionMatchModel.MajorVersionMatch = MajorVersionMatchEnum.OK_DBAndAssemblyREqual;

            }
            Debug.WriteLine(myPatchDistributionMatchModel.Message);
            return myPatchDistributionMatchModel;
        }


    }
    public class PatchDistributionMatchModel
    {
        public DBMigrationPM LastClosed_DBMigration { get; internal set; }
        public AssemblyDBMigrationModel MyAssemblyDBMigrationModel { get; internal set; }
        public string Message { get; internal set; }
        public bool NotDistributionBranch { get; internal set; }
        public PatchDistributionMatch.MajorVersionMatchEnum MajorVersionMatch { get; set; }
        
    }

}
