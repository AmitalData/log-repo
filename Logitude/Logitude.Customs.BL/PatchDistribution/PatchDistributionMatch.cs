using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
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
            OK
        }
        public PatchDistributionMatchModel GetPatchDistributionMatchModel(string assemblyVersion)
        {

            var myDBMigrationQueryService = new DBMigrationQueryService(0);
            var lastDBMigration = myDBMigrationQueryService.GetLastPM() ??
                new DBMigrationPM()
                {
                    MajorVersion = 19.03m,
                    MinorVersion = 0,
                     //DBMigrationLines = new List<DBMigrationLinePM>()
                     //{
                         
                     //}
                     
                };




            var myAssemblyDBMigrationModel = AssemblyDBMigrationModel.Parser(assemblyVersion);
            var myPatchDistributionMatchModel = new PatchDistributionMatchModel
            {
                LastDBMigration = lastDBMigration,
                MyAssemblyDBMigrationModel = myAssemblyDBMigrationModel
            };
            if (myAssemblyDBMigrationModel.p_d != "D")
            {
                myPatchDistributionMatchModel.Message = "burn not in distribution branch";
                myPatchDistributionMatchModel.NotDistributionBranch = true;//MessageBox.Show("burn not in distribution branch");

            }



            if (myAssemblyDBMigrationModel.MajorVersion < lastDBMigration.MajorVersion)
            {
                myPatchDistributionMatchModel.Message = /*MessageBox.Show*/($"DataBase MajorVersion is newer -  Assembly:{myAssemblyDBMigrationModel.MajorVersion} < DBMigration{lastDBMigration.MajorVersion}");
                //this.Close();
                myPatchDistributionMatchModel.MajorVersionMatch = MajorVersionMatchEnum.OldSource;
            }
            else if (myAssemblyDBMigrationModel.MajorVersion < lastDBMigration.MajorVersion)
            {
                myPatchDistributionMatchModel.Message = /*MessageBox.Show*/($"DataBase MajorVersion is older -  Assembly:{myAssemblyDBMigrationModel.MajorVersion} > DBMigration{lastDBMigration.MajorVersion}");
                //this.Close();
                myPatchDistributionMatchModel.MajorVersionMatch = MajorVersionMatchEnum.OldDB;
            }
            else
            {
                myPatchDistributionMatchModel.MajorVersionMatch = MajorVersionMatchEnum.OK;
            }

            return myPatchDistributionMatchModel;
        }


    }
    public class PatchDistributionMatchModel
    {
        public DBMigrationPM LastDBMigration { get; internal set; }
        public AssemblyDBMigrationModel MyAssemblyDBMigrationModel { get; internal set; }
        public string Message { get; internal set; }
        public bool NotDistributionBranch { get; internal set; }
        public PatchDistributionMatch.MajorVersionMatchEnum MajorVersionMatch { get; set; }
    }

}
