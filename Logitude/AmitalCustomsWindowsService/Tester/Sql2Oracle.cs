using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCustomsWindowsService.Tester
{
    internal class Sql2Oracle
    {
        List<MyTable> myTables = new List<MyTable>();
        public void GetReNameLongColumns(string root)
        {
            var files = Directory.GetFiles(root, "*Map.cs", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var lines = File.ReadLines(file).ToList();

                string table = "";
                int count = lines.Count();
                int cur = 0;
                while (cur < count)
                {

                    string line = lines[cur];



                    int pos = -1;
                    if (line.Contains("oracle"))
                    {
                        if (file.Contains("ShipmentReceivable"))
                        {

                        }
                    }

                    if ((pos = line.IndexOf(@"ToTable")) > 0)
                    {

                        string startTable = line.Substring(pos + "ToTable".Length + 2);
                        pos = startTable.IndexOf('"');
                        table = startTable.Substring(0, pos);
                    }
                    else if (line.Contains("if") && line.Contains("\"oracle\""))
                    {
                        var myTable = new MyTable();
                        myTable.Table = table;
                        myTable.IsGlobalModel = file.Contains("GlobalModel");
                        this.myTables.Add(myTable);
                        bool msql = false;
                        if (line.Contains("=="))
                        {
                            bool elseEnd = false;
                            bool elseOccure = false;
                            string field = "";
                            line = NextLine(lines, ref cur);
                            while (!elseEnd)
                            {
                                if (line.Contains(".Property(") && line.Contains(".HasColumnName("))
                                {
                                    field = line.Substring(line.IndexOf(".Property(") + ".Property(".Length);
                                    field = field.Substring(field.IndexOf(".") + 1);
                                    field = field.Substring(0, field.IndexOf(")"));
                                    var myProperty = GetProperty(myTable, field);

                                    var dbName = line.Substring(line.IndexOf(".HasColumnName(") + ".HasColumnName(".Length + 1);
                                    dbName = dbName.Substring(0, dbName.IndexOf('"'));
                                    if (msql)
                                    {
                                        myProperty.MSql = dbName;
                                    }
                                    else
                                    {
                                        myProperty.Oracle = dbName;
                                    }
                                }
                                else if (!elseOccure && line.Contains("else"))
                                {
                                    msql = !msql;
                                    elseOccure = true;
                                }
                                else if (line.Contains("}"))
                                {
                                    if (elseOccure)
                                    {
                                        elseEnd = true;
                                    }
                                }

                                line = NextLine(lines, ref cur);
                            }
                        }
                        else if (line.Contains("!="))
                        {

                        }
                        else
                        {

                        }

                    }
                    cur++;
                }
            }

            DebugWriteRename(true);
            DebugWriteRename(false);
        }

        private void DebugWriteRename(bool isGlobalModel)
        {


            System.Diagnostics.Debug.WriteLine(isGlobalModel ? "---global" : "---main");

            foreach (var tab in myTables.Where(r => r.IsGlobalModel == isGlobalModel))
            {
                foreach (var prop in tab.MyPropertys)
                {
                    System.Diagnostics.Debug.WriteLine($"EXEC sp_rename '{tab.Table}.{prop.Oracle}', '{prop.MSql}';");

                }
            }
        }

        private MyProperty GetProperty(MyTable myTable, string field)
        {
            var f = myTable.MyPropertys.FirstOrDefault(r => r.Name == field);
            if (f == null)
            {
                f = new MyProperty() { Name = field };
                myTable.MyPropertys.Add(f);
            }
            return f;
        }

        private static string NextLine(List<string> lines, ref int cur)
        {
            string line;
            cur++;
            line = lines[cur];
            return line;
        }



        public void ChangeToBit()
        {
            //The 'NumberOfRetries' property on 'ContactPassword' could not be set to a 'System.Decimal' value. You must set this property to a non-null value of type 'System.Int32'. 
            //The 'IsUpgrading' property on 'GlobalDB' could not be set to a 'System.Decimal' value.You must set this property to a non-null value of type 'System.Boolean'.

            string sql = @"SELECT CONCAT( C.TABLE_NAME,   '-'  , C.COLUMN_NAME)
--c.* 
from INFORMATION_SCHEMA.columns c
INNER JOIN INFORMATION_SCHEMA.tables t ON t.table_name = c.table_name
WHERE c.data_type = 'numeric' AND t.table_type = 'base table' and IS_NULLABLE='no' 
and NUMERIC_PRECISION_RADIX=10 and NUMERIC_PRECISION=1 and NUMERIC_SCALE=0
";
            string entities = @"ANALYZEQUEUES-CONNECTEDTOTENANT_ORA
ANALYZEQUEUES-CONNECTEDTOENTITY
BATCHSERVICESDEFINITIONMODS-INACTIVE
BATCHSERVICESDEFINITIONS-USERABBITMQ
BLUESNAPCONTRACTS-INACTIVE
CAPTCHAKEYS-ISUSED
CONTACTMOBILEDEVICES-ISSIGNOUT
CONTACTPASSWORDS-MUSTCHANGEPASSWORD
CONTACTPASSWORDS-ISLOCKED
CONTACTPASSWORDS-SharedMobileAppAlertsforFollowedShipment
CONTACTPASSWORDS-SharedMobileAppAlertonExceptions
CONTACTPASSWORDS-ISSENDNOTIFICATIONFORMOBILE
CONTACTPASSWORDS-ISBCRYPT
CONVERTPROGRAMINFOES-ISAPPLIED
GLOBALCONTACTS-INACTIVE
GLOBALCONTACTS-ISUSER
GLOBALCONTACTS-INTERNETACCESS
GLOBALDBS-IsUpgrading_ORA
GLOBALDBS-IsActive_ORA
GLOBALDBS-IsBlocking_ORA
GLOBALTENANTS-ISACTIVE
HELPRESOURCES-ISNEW
HELPRESOURCES-INACTIVE
LOGITUDELEADS-ISEMAILVERIFIED
LOGITUDELEADS-ISSENTTOCUSTOMER
LOGITUDELEADS-ISUSEROPENED
LOGITUDELEADS-ISUSEREMAILSENT
MOBILENOTIFICATIONLOGS-ISEXCEPTION
MOBILENOTIFICATIONLOGS-ISREAD
MOBILENOTIFICATIONLOGS-ISDELETE
ONETIMEPASSWORDS-ISUSED
PASSWORDRESETREQUESTS-ISDONE
PASSWORDRESETREQUESTS-ISMOBILEONLY
PERFORMANCELOGS-MONITORINGSERVICE
SETTINGS-USINGAZURE_ORA
SETTINGS-IsLogEnabled_ORA
SETTINGS-ForceHttps_ORA
SETTINGS-EnableHybridQueue_ORA
SETTINGS-IsUpgradingChamp_ORA
SETTINGS-SameUserLoginEnabled_ORA
SETTINGS-ReportsRunUsingWR_ORA
TENANTMANAGEMENTS-ISTRIAL
TENANTMANAGEMENTS-ISRECURRING
TENANTMANAGEMENTS-ISSYSTEMSUPPORTENABLED
TENANTMANAGEMENTS-ISDISTRIBUTORSUPPORTENABLED
TENANTMANAGEMENTS-ISAWBSTOCKPREPAID
TENANTMANAGEMENTS-MANAGELICENCESPERUSER
TENANTMANAGEMENTS-ISCARGONAUTENABLED
TENANTMANAGEMENTS-ISDEXXCONNECTIONENABLED
TENANTMANAGEMENTS-PAYMENTFAILURE
TENANTMANAGEMENTS-ISEAWBONLYDEMO
TENANTMANAGEMENTS-ISRESTRICTEDBYAIRLINE
TENANTMANAGEMENTS-MANAGESREGISTEREDAGENT
TENANTMANAGEMENTS-BILLINGBYLOGITUDE
TENANTMANAGEMENTS-SUPPORTACTIVATED
TENANTMANAGEMENTS-ISMULTIPACKAGE
TENANTMANAGEMENTS-ENABLEBRANDING
TENANTMANAGEMENTS-HIDESHAREDLOGISTICS
TENANTMANAGEMENTS-ISPARENTTENANT
TENANTMANAGEMENTS-CHANGEHEADERCOLOR
TENANTMANAGEMENTS-ISINTTRASTOCKPREPAID
TENANTMANAGEMENTS-ISINTTRAONLYDEMO
TENANTMANAGEMENTS-MAINADDITIONALPACKAGEAPPLIED
TENANTMANAGEMENTS-NOPAYMENTFORCHILDTENANTS
TENANTMANAGMENTPRIVATELABELS-RECEIVEALLSTATUSES
TENANTMANAGMENTPRIVATELABELS-INACTIVE
TENANTMANAGMENTPRIVATELABELS-HASLOGBOXACCESS
WEBHOOKKEYS-INACTIVE";
            List<string> entities2 = new List<string>();
            List<string> restScript = new List<string>();
            foreach (var line in entities.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {

                var arry = line.Split('-');
                if (arry[1].EndsWith("_ORA"))
                {
                    continue;
                }

                entities2.Add($"ALTER TABLE {arry[0]} add {arry[1]}_MSQL bit not null default(0)");
                string alterScript = 
@" --{line}
--ALTER TABLE SETTINGS add USINGAZURE_MSQL bit not null default(0)
update SETTINGS  set USINGAZURE_MSQL= CAST(IIF ( USINGAZURE= 1, 1, 0 ) AS BIT)  
EXEC sp_rename 'SETTINGS.USINGAZURE', 'USINGAZURE_ORA';
EXEC sp_rename 'SETTINGS.USINGAZURE_MSQL', 'USINGAZURE';

";
                restScript.Add(
                    alterScript
                    .Replace("line", line)
                    .Replace("SETTINGS", arry[0])
                    .Replace("USINGAZURE", arry[1])
                    );
            }
            Debug.WriteLine(string.Join(Environment.NewLine, entities2));
            Debug.WriteLine("------");
            Debug.WriteLine(string.Join(Environment.NewLine, restScript));
        }


        public void ChangeToINT()
        {
            //The 'NumberOfRetries' property on 'ContactPassword' could not be set to a 'System.Decimal' value. You must set this property to a non-null value of type 'System.Int32'. 
            

            string sql = @"--ALTER TABLE SETTINGS  ALTER COLUMN OITenantNumber  INT
SELECT CONCAT('ALTER TABLE ', C.TABLE_NAME,   ' ALTER COLUMN  '  , C.COLUMN_NAME , '  INT; ')
--c.* 
from INFORMATION_SCHEMA.columns c
INNER JOIN INFORMATION_SCHEMA.tables t ON t.table_name = c.table_name
WHERE c.data_type = 'numeric' AND t.table_type = 'base table' 
and IS_NULLABLE='no' 
and NUMERIC_PRECISION_RADIX=10 
and NUMERIC_PRECISION>1 and NUMERIC_SCALE=0
--and c.COLUMN_NAME='NumberOfRetries'
";
            
            
        }
        class MyTable
        {
            public bool IsGlobalModel { get; set; }

            public string Table { get; set; }
            public List<MyProperty> MyPropertys { get; set; } = new List<MyProperty>();

        }
        class MyProperty
        {
            public string Name { get; set; }
            public string Oracle { get; set; }
            public string MSql { get; set; }

        }
    }
}
