using Logitude.DBMigrations.Models;

namespace Logitude.DBMigrations
{
    public class Program
    {
        static void Main(string[] args)
        {
            RunSettings runSettings = new RunSettings
            {
                DebugMode = true,
                ExecuteScripts = false,
                IgnoreHash = false,
                ValidateFiles = true,
                Root = @"C:\source\log-repo\Logitude",
                SpecificDxmlFile = null,
                SpecificSxmlFile = null
            };

            MigrationTool migrationTool = new MigrationTool(args, runSettings);
            migrationTool.RunTool();
        }
    }
}