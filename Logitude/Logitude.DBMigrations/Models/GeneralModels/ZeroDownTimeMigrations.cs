using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public abstract class ZeroDownTimeMigrations
    {
        public void Start()
        {
            List<ZeroDownTimeDefaultValueMigration> defaultValueMigrations = GetDefaultValueMigrations();

            foreach(var defaultValueMigration in defaultValueMigrations)
            {
                Console.WriteLine("Handle Default Value Migration For Column " + defaultValueMigration.ColumnName + " In Table " + defaultValueMigration.SchemaName + "." +  defaultValueMigration.TableName + " For " + defaultValueMigration.DatabaseType + " Database ...");
                HandleDefaultValueMigration(defaultValueMigration);
            }
        }

        protected void HandleDefaultValueMigration(ZeroDownTimeDefaultValueMigration defaultValueMigration)
        {
            UpdateDefaultValueMigration(defaultValueMigration.Id, "Status", "InProgress");
            UpdateDefaultValueMigration(defaultValueMigration.Id, "StartDate", DateTime.Now.ToString());
            SetDefaultValues(defaultValueMigration);
            UnsetColumnNullable(defaultValueMigration);
            UpdateDefaultValueMigration(defaultValueMigration.Id, "EndDate", DateTime.Now.ToString());
            UpdateDefaultValueMigration(defaultValueMigration.Id, "Status", "Done");
        }

        protected void ExitZeroDownTimeMigrations(string message)
        {
            Console.WriteLine("Error: " + message);
            Environment.Exit(1);
        }

        protected abstract List<ZeroDownTimeDefaultValueMigration> GetDefaultValueMigrations();

        protected abstract void SetDefaultValues(ZeroDownTimeDefaultValueMigration defaultValueMigration);

        protected abstract void UpdateDefaultValueMigration(string defaultValueMigrationId, string property, string value);

        protected abstract void UnsetColumnNullable(ZeroDownTimeDefaultValueMigration defaultValueMigration);
    }
}