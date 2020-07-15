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
                IgnoreHash = false,
                ValidateFiles = true,
                Root = null,
                SpecificDxmlFile = null,
                SpecificSxmlFile = null
            };

            bool isZeroDownTimeArgumentProvided = Array.IndexOf((Array.ConvertAll(args, a => a.ToLower())), ToolArguments.ZERODOWNTIME) != -1;

            if (!isZeroDownTimeArgumentProvided)
            {
                MigrationTool migrationTool = new MigrationTool(args, runSettings);
                migrationTool.RunTool();
            }
            else
            {
                ZeroTimeMigrationTool zeroTimeMigrationTool = new ZeroTimeMigrationTool(args, runSettings);
                zeroTimeMigrationTool.RunTool();
            }
        }
    }
}