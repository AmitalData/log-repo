using Logitude.DBMigrations.Models;
using System;

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
                DevMode = false,
                IgnoreHash = false,
                ValidateFiles = true,
                Root = null,
                SpecificDxmlFile = null,
                SpecificSxmlFile = null
            };

            ToolArguments.Arguments = args;
            MigrationTool migrationTool = new MigrationTool(runSettings);
            migrationTool.RunTool();
#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}