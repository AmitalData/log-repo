using Devart.Data.Oracle;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.Repsitories;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    public class P19R03_0001_PatchDist: PatchDistributionBase
    {

        public P19R03_0001_PatchDist() : base("טבלאות תשתית הפצה ",new DateTime(2019,12,2))
        {
            
            //this.PatchName = "טבלאות תשתית הפצה ";
            //this.Branch = "19R03";
            //this.Counter  = 0;
            //this.ScriptCount = 5;
        }


        public void Enshure_SeedDbMigrateTable()
        {
            string sql = "";
            string res = "";
            var customContext = CustomContext.GetContext(0);
            //var openReaderSingleResult = new OpenReaderSingleResult(0);
            try
            {
                sql = //"select MinorVersion from DBMigrations  where isclose = 1 and  rownum < 2 order by MajorVersion desc, MinorVersion desc";
                    "select MinorVersion from ( select * from DBMigrations  where isclose = 1  order by MajorVersion desc, MinorVersion desc) a where rownum<2";
                (customContext as DbContextBase).ExecuteReaderSingleResult<int>(
    sql,
(dr) =>
{
    res = dr.GetString(0);
    Debug.WriteLine($"Enshure_SeedDbMigrateTable():Last MinorVersion is {res}");
    return 0;
});


                

            }
            catch (OracleException eOracleException)
            {
                //OracleException (0x80004005): ORA-00942: טבלה או view אינם קיימים
                if (eOracleException.ToString().Contains("ORA-00942"))
                {
                    sql =
                        @"CREATE TABLE DBMigrationLines ( 
  dbmigrationid VARCHAR2(15 CHAR) NOT NULL,
  CounterKey NUMBER(10) NOT NULL,
  SqlScript VARCHAR2(1024 CHAR) NOT NULL,
  ApprovedRemarks NVARCHAR2(256) NULL,
  PRIMARY KEY(dbmigrationid, CounterKey)
)";
                    

                    int? affect =
                        (customContext as DbContextBase).ExecuteReaderSingleResult<int>(
                            sql,
                        (dr) =>
                       {
                           res = dr.GetString(0);
                           return 0;
                       });

                    sql =
                        @"CREATE TABLE DBMigrations ( 
  Id VARCHAR2(15 CHAR) NOT NULL,
  ExecuteDate TIMESTAMP(7) NOT NULL,
  MajorVersion NUMBER(5,2) NOT NULL,
  MinorVersion NUMBER(10) NOT NULL,
  Remarks NVARCHAR2(256) NULL,
  IsClose NUMBER(1) NOT NULL,
  PRIMARY KEY (Id)
)";
                    (customContext as DbContextBase).ExecuteReaderSingleResult<int>(
    sql,
(dr) =>
{
    res = dr.GetString(0);
    return 0;
});



                    sql =
                        @"INSERT INTO DBMigrations VALUES ('19.03',sysdate,19.03,0,'Start 19.03',1) ";
                    (customContext as DbContextBase).ExecuteReaderSingleResult<int>(
    sql,
(dr) =>
{
    res = dr.GetString(0);
    return 0;
});

                }

            }
            catch (Exception e)
            {

                //ExecuteReaderSingleResult(sql);
                throw;
            }
        }
        public override List<ScriptDTO> GetUpScripts()
        {
            int ScriptCount = 0;
            return new List<ScriptDTO>()
            {
                new ScriptDTO() { 
                    ScriptCounter = ScriptCount++, 
                    SqlScript = "CREATE INDEX IX_DBMigrationLines_Id ON DBMigrationLines (DBMigrationId)"
                },
                new ScriptDTO() {
                    ScriptCounter = ScriptCount++,
                    SqlScript = @"ALTER TABLE DBMigrationLines
  ADD CONSTRAINT FK_N1148365077 FOREIGN KEY (DBMigrationId) REFERENCES DBMigrations (Id)
"
                },
            };
        }

        public override List<ScriptDTO> GetDownScripts()
        {
            throw new NotImplementedException();
        }
    }
   
}
