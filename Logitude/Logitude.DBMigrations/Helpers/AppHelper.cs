using Logitude.DBMigrations.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace Logitude.DBMigrations.Helpers
{
    public static class AppHelper
    {
        public static string[] GetDXMLFilesFromRoot(string root)
        {
            try
            {
                string DXMLFilesPath = Path.Combine(root);
                string[] DXMLFiles = Directory.GetFiles(DXMLFilesPath, "*.dxml", SearchOption.AllDirectories);
                if (DXMLFiles.Length > 0)
                {
                    return DXMLFiles;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static GeneratedScript GenerateScriptFromDXMLFiles(string[] DXMLFiles)
        {
            GeneratedScript generatedScript = new GeneratedScript();

            foreach (var dxmlFile in DXMLFiles)
            {
                string dxmlFileName = Path.GetFileName(dxmlFile);
                Console.WriteLine("Generating Script For " + dxmlFileName + " ...");

                string xmlString = File.ReadAllText(dxmlFile);
                TableDefinition dxmlTable = xmlString.ParseXML<TableDefinition>();

                DatabaseMigrations databaseMigrations = CreateDatabaseMigrations(dxmlTable);

                string script = databaseMigrations.GetScript();

                if (!String.IsNullOrEmpty(script))
                {
                    generatedScript = AppendGeneratedScript(generatedScript, dxmlTable.DBType, script, dxmlFileName);
                }
            }

            return generatedScript;
        }

        public static void SaveScript(GeneratedScript generatedScript)
        {
            Console.WriteLine("Saving The Generated Scripts ...");
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            string globalScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\GlobalScript.sql");
            File.WriteAllText(globalScriptFilePath, generatedScript.GlobalScript);

            string mainScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\MainScript.sql");
            File.WriteAllText(mainScriptFilePath, generatedScript.MainScript);

            string systemLogsScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\SystemLogsScript.sql");
            File.WriteAllText(systemLogsScriptFilePath, generatedScript.SystemLogsScript);
        }

        public static void ExecuteScript(GeneratedScript generatedScript)
        {
            if (!String.IsNullOrEmpty(generatedScript.GlobalScript))
            {
                Console.WriteLine("Executing Script On Global Database ...");
                string result = ExecuteScript(generatedScript.GlobalScript, "Global");
                if (!String.IsNullOrEmpty(result))
                {
                    Console.WriteLine(result);
                }
            }

            if (!String.IsNullOrEmpty(generatedScript.MainScript))
            {
                Console.WriteLine("Executing Script On Main Database ...");
                string result = ExecuteScript(generatedScript.MainScript, "Main");
                if (!String.IsNullOrEmpty(result))
                {
                    Console.WriteLine(result);
                }
            }

            if (!String.IsNullOrEmpty(generatedScript.SystemLogsScript))
            {
                Console.WriteLine("Executing Script On SystemLogs Database ...");
                string result = ExecuteScript(generatedScript.SystemLogsScript, "SystemLogs");
                if (!String.IsNullOrEmpty(result))
                {
                    Console.WriteLine(result);
                }
            }
        }

        public static bool IsArgumentProvided(string[] args, string arg)
        {
            string[] arguments = Array.ConvertAll(args, a => a.ToLower());
            return (Array.IndexOf(arguments, arg) != -1);
        }

        public static string GetRoot(string[] args)
        {
            string[] arguments = Array.ConvertAll(args, a => a.ToLower());
            int indexOfRootArgument = Array.IndexOf(arguments, "-root") + 1;
            if (indexOfRootArgument < args.Length && indexOfRootArgument >= 0)
            {
                string root = args[indexOfRootArgument];
                return root;
            }
            else
            {
                return null;
            }
        }

        private static string GetConnectionString(string dbType)
        {
            string connectionString;

            if (dbType == "Global")
            {
                connectionString = ConfigurationManager.AppSettings["GlobalConnectionString"];
            }
            else if (dbType == "Main")
            {
                connectionString = ConfigurationManager.AppSettings["MainConnectionString"];
            }
            else if (dbType == "SystemLogs")
            {
                connectionString = ConfigurationManager.AppSettings["SystemLogsConnectionString"];
            }
            else
            {
                connectionString = null;
            }

            return connectionString;
        }

        private static GeneratedScript AppendGeneratedScript(GeneratedScript generatedScript, string dbType, string script, string dxmlFileName)
        {
            if (dbType == "Global")
            {
                generatedScript.GlobalScript += "/* Generated Script For " + dxmlFileName + " */\n";
                generatedScript.GlobalScript += script;
                generatedScript.GlobalScript += "\n";
                return generatedScript;
            }
            else if (dbType == "Main")
            {
                generatedScript.MainScript += "/* Generated Script For " + dxmlFileName + " */\n";
                generatedScript.MainScript += script;
                generatedScript.MainScript += "\n";
                return generatedScript;
            }
            else if (dbType == "SystemLogs")
            {
                generatedScript.SystemLogsScript += "/* Generated Script For " + dxmlFileName + " */\n";
                generatedScript.SystemLogsScript += script;
                generatedScript.SystemLogsScript += "\n";
                return generatedScript;
            }
            else
            {
                return generatedScript;
            }
        }

        private static DatabaseMigrations CreateDatabaseMigrations(TableDefinition dxmlTable)
        {
            string connectonString = GetConnectionString(dxmlTable.DBType);
            string databseType = ConfigurationManager.AppSettings["DatabseType"];

            if (databseType.ToLower() == "oracle")
            {
                DatabaseMigrations oracleDatabaseMigrations = new OracleDatabaseMigrations(dxmlTable, connectonString);
                return oracleDatabaseMigrations;
            }

            DatabaseMigrations sqlDatabaseMigrations = new SQLDatabaseMigrations(dxmlTable, connectonString);
            return sqlDatabaseMigrations;
        }

        private static string ExecuteScript(string script, string dbType)
        {
            string databseType = ConfigurationManager.AppSettings["DatabseType"];

            if (databseType.ToLower() == "oracle")
            {
                try
                {
                    return null;
                }
                catch (Exception exception)
                {
                    return exception.Message;
                }
            }

            try
            {
                string connectionString = GetConnectionString(dbType);
                SqlConnection SqlConnection = new SqlConnection(connectionString);
                SqlCommand SqlCommand = SqlConnection.CreateCommand();
                SqlCommand.CommandText = script;
                SqlConnection.Open();
                SqlCommand.ExecuteNonQuery();
                return null;
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
    }
}