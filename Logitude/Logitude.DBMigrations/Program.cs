using Logitude.DBMigrations.Models;

namespace Logitude.DBMigrations
{
    public class Program
    {
        static void Main(string[] args)
        {
            RunSettings runSettings = new RunSettings
            {
                DebugMode = false,
                ExecuteScripts = false,
                IgnoreHash = false,
                ValidateFiles = true,
                Root = null,
                SpecificDxmlFile = null,
                SpecificSxmlFile = null
            };

            MigrationTool migrationTool = new MigrationTool(args, runSettings);
            migrationTool.RunTool();
        }
    }
}