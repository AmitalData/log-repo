using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public abstract class ZeroDownTimeMigrations
    {
        protected bool ServiceMode;

        public void Start()
        {
            ServiceMode = false;
            StartZeroDownTimeForDataScripts(true);
            StartZeroDownTimeForDefaultValues();
            StartZeroDownTimeForDataScripts(false);
        }

        public void StartAsService()
        {
            ServiceMode = true;
            while (true)
            {
                StartZeroDownTimeForDataScripts(true);
                StartZeroDownTimeForDataScripts(false);
            }
        }

        protected void StartZeroDownTimeForDefaultValues()
        {
            List<DBMigrationsSetDefaultValue> dbMigrationsSetDefaultValues = GetDBMigrationsSetDefaultValues();

            foreach (var dbMigrationsSetDefaultValue in dbMigrationsSetDefaultValues)
            {
                Console.WriteLine("Handle Default Value For Column " + dbMigrationsSetDefaultValue.ColumnName + " In Table " + dbMigrationsSetDefaultValue.TableName + " On " + dbMigrationsSetDefaultValue.DatabaseType + " Database ...");
                HandleDBMigrationsSetDefaultValue(dbMigrationsSetDefaultValue);
            }
        }

        protected void StartZeroDownTimeForDataScripts(bool preScripts)
        {
            List<string> dataScriptsStatuses = !ServiceMode ? new List<string>() { "Waiting", "FullBuild" } : new List<string>() { "IncrementalBuild" };
            List<DBMigrationsDataScript> dbMigrationsDataScripts = GetDBMigrationsDataScripts(preScripts, dataScriptsStatuses);

            foreach (var dbMigrationsDataScript in dbMigrationsDataScripts)
            {
                Console.WriteLine("Handle Data Script For SXML File " + dbMigrationsDataScript.SxmlFileName + " On " + dbMigrationsDataScript.DatabaseType + " Database ...");
                HandleDBMigrationsDataScript(dbMigrationsDataScript);
            }
        }

        protected void HandleDBMigrationsSetDefaultValue(DBMigrationsSetDefaultValue dbMigrationsSetDefaultValue)
        {
            CreateDBMigrationsLastDefaultValueColumn(dbMigrationsSetDefaultValue.DatabaseType, dbMigrationsSetDefaultValue.TableName);

            UpdateDBMigrationsSetDefaultValue(dbMigrationsSetDefaultValue.Id, "Status", "InProgress");
            UpdateDBMigrationsSetDefaultValue(dbMigrationsSetDefaultValue.Id, "StartDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            SetDefaultValueAsBatches(dbMigrationsSetDefaultValue);
            AddNotNullCheckConstraint(dbMigrationsSetDefaultValue);
            UpdateDBMigrationsSetDefaultValue(dbMigrationsSetDefaultValue.Id, "EndDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            UpdateDBMigrationsSetDefaultValue(dbMigrationsSetDefaultValue.Id, "Status", "Done");
        }

        protected void HandleDBMigrationsDataScript(DBMigrationsDataScript dbMigrationsDataScript)
        {
            CreateDBMigrationsLastScriptColumn(dbMigrationsDataScript.DatabaseType, dbMigrationsDataScript.TargetTableName);
            CreateResetLastScriptTrigger(dbMigrationsDataScript.DatabaseType, dbMigrationsDataScript.TargetTableName);

            if (!ServiceMode)
            {
                UpdateDBMigrationsDataScript(dbMigrationsDataScript.Id, "Status", "FullBuild");
            }
            
            dbMigrationsDataScript.StartDate = DateTime.Now;
            UpdateDBMigrationsDataScript(dbMigrationsDataScript.Id, "StartDate", dbMigrationsDataScript.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            ExecuteScriptAsBatches(dbMigrationsDataScript);
            dbMigrationsDataScript.EndDate = DateTime.Now;
            UpdateDBMigrationsDataScript(dbMigrationsDataScript.Id, "EndDate", dbMigrationsDataScript.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));

            if (!ServiceMode)
            {
                UpdateDBMigrationsDataScript(dbMigrationsDataScript.Id, "Status", "IncrementalBuild");
                InsertIntoDBScriptsHistory(dbMigrationsDataScript);
            }
        }

        protected void SendEmailsForDataScriptTimeout(DBMigrationsDataScript dbMigrationsDataScript)
        {
            string subject = "Timeout Exception While Executing Script By DBMigrations Tool";
            string messageBody = "Timeout Exception Occured On " + dbMigrationsDataScript.DatabaseType + " Database While Executing Script From SXML File: " +
                                 dbMigrationsDataScript.SxmlFileName + "\nDBMigrationsDataScript Id: " + dbMigrationsDataScript.Id;

            EmailSender emailSender = new EmailSender(subject, messageBody);
            emailSender.Send();
        }

        protected void ExitTool(string message)
        {
            Console.WriteLine(message);
            Environment.Exit(1);
        }

        protected abstract List<DBMigrationsSetDefaultValue> GetDBMigrationsSetDefaultValues();

        protected abstract List<DBMigrationsDataScript> GetDBMigrationsDataScripts(bool preScripts, List<string> statuses);

        protected abstract void SetDefaultValueAsBatches(DBMigrationsSetDefaultValue dbMigrationsSetDefaultValue);
        
        protected abstract void ExecuteScriptAsBatches(DBMigrationsDataScript dbMigrationsDataScript);

        protected abstract void InsertIntoDBScriptsHistory(DBMigrationsDataScript dbMigrationsDataScript);
        
        protected abstract void UpdateDBMigrationsSetDefaultValue(string dbMigrationsSetDefaultValueId, string property, string value);

        protected abstract void UpdateDBMigrationsDataScript(string dbMigrationsDataScriptId, string property, string value);
        
        protected abstract void AddNotNullCheckConstraint(DBMigrationsSetDefaultValue dbMigrationsSetDefaultValue);
        
        protected abstract string FormatDefaultValue(string defaultValue);

        protected abstract void CreateDBMigrationsLastDefaultValueColumn(string databaseType, string tableName);

        protected abstract void CreateDBMigrationsLastScriptColumn(string databaseType, string tableName);

        protected abstract void CreateResetLastScriptTrigger(string databaseType, string tableName);
    }
}