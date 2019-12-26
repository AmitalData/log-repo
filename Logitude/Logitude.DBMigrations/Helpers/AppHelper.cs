using Logitude.DBMigrations.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace Logitude.DBMigrations.Helpers
{
    public static class AppHelper
    {
        private static string PerformanceData = "Description,Time(ms)\n";

        public static string[] GetDXMLFilesFromRoot(string root)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                string DXMLFilesPath = Path.Combine(root);
                string[] DXMLFiles = Directory.GetFiles(DXMLFilesPath, "*.dxml", SearchOption.AllDirectories);

                AppendToPerformanceData("Get DXML Files From Root", stopwatch);

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
            RelationsScript relationsScript = new RelationsScript();

            var stopwatch = Stopwatch.StartNew();

            foreach (var dxmlFile in DXMLFiles)
            {
                string dxmlFileName = Path.GetFileName(dxmlFile);
                Console.WriteLine("Generating Script For " + dxmlFileName + " ...");

                string xmlString = File.ReadAllText(dxmlFile);
                TableDefinition dxmlTable = xmlString.ParseXML<TableDefinition>();

                DatabaseMigrations databaseMigrations = CreateDatabaseMigrations(dxmlTable);

                string tableScript = databaseMigrations.GetScript();
                string tableRelationsScript = databaseMigrations.GetRelationsScript();
                //string tableRelationsScript = null;

                if (!String.IsNullOrEmpty(tableScript))
                {
                    generatedScript = AppendToGeneratedScript(generatedScript, dxmlTable.DBType, tableScript, dxmlFileName);
                }

                if (!String.IsNullOrEmpty(tableRelationsScript))
                {
                    relationsScript = AppendToRelationsScript(relationsScript, dxmlTable.DBType, tableRelationsScript);
                }
            }

            generatedScript = AppendRelationsScriptToGeneratedScript(generatedScript, relationsScript);

            AppendToPerformanceData("Generate Scripts From DXML Files", stopwatch);

            return generatedScript;
        }

        public static void SaveScript(GeneratedScript generatedScript)
        {
            var stopwatch = Stopwatch.StartNew();

            Console.WriteLine("Saving The Generated Scripts ...");
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            string globalScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\GlobalScript.sql");
            File.WriteAllText(globalScriptFilePath, generatedScript.GlobalScript);

            string mainScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\MainScript.sql");
            File.WriteAllText(mainScriptFilePath, generatedScript.MainScript);

            string systemLogsScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\SystemLogsScript.sql");
            File.WriteAllText(systemLogsScriptFilePath, generatedScript.SystemLogsScript);

            Console.WriteLine("The Generated Scripts Saved Successfully");

            AppendToPerformanceData("Save The Generated Scripts", stopwatch);
        }

        public static void ExecuteScript(GeneratedScript generatedScript)
        {
            var stopwatch = Stopwatch.StartNew();

            if (!String.IsNullOrEmpty(generatedScript.GlobalScript))
            {
                Console.WriteLine("Executing Script On Global Database ...");
                string result = ExecuteScript(generatedScript.GlobalScript, "Global");
                if (!String.IsNullOrEmpty(result))
                {
                    Console.WriteLine(result);
                }
                else
                {
                    Console.WriteLine("Scripts Executed Successfully On Global Database");
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
                else
                {
                    Console.WriteLine("Scripts Executed Successfully On Main Database");
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
                else
                {
                    Console.WriteLine("Scripts Executed Successfully On SystemLogs Database");
                }
            }

            AppendToPerformanceData("Execute The Generated Scripts", stopwatch);
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

        public static void ExportPerformanceData()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string csvFilePath = Path.Combine(projectDirectory, @"Reports\DBMigrationsPerformance.csv");
            File.WriteAllText(csvFilePath, PerformanceData);
            Console.WriteLine("\nPerformance Time Extracted To /Reports/DBMigrationsPerformance.csv\n");
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

        private static GeneratedScript AppendToGeneratedScript(GeneratedScript generatedScript, string dbType, string tableScript, string dxmlFileName)
        {
            if (dbType == "Global")
            {
                //generatedScript.GlobalScript += "/* Generated Script For " + dxmlFileName + " */\n";
                generatedScript.GlobalScript += tableScript;
                generatedScript.GlobalScript += "\n";
                return generatedScript;
            }
            else if (dbType == "Main")
            {
                //generatedScript.MainScript += "/* Generated Script For " + dxmlFileName + " */\n";
                generatedScript.MainScript += tableScript;
                generatedScript.MainScript += "\n";
                return generatedScript;
            }
            else if (dbType == "SystemLogs")
            {
                //generatedScript.SystemLogsScript += "/* Generated Script For " + dxmlFileName + " */\n";
                generatedScript.SystemLogsScript += tableScript;
                generatedScript.SystemLogsScript += "\n";
                return generatedScript;
            }
            else
            {
                return generatedScript;
            }
        }

        private static RelationsScript AppendToRelationsScript(RelationsScript relationsScript, string dbType, string tableRelationsScript)
        {
            if (dbType == "Global")
            {
                relationsScript.GlobalScript += tableRelationsScript;
                relationsScript.GlobalScript += "\n";
                return relationsScript;
            }
            else if (dbType == "Main")
            {
                relationsScript.MainScript += tableRelationsScript;
                relationsScript.MainScript += "\n";
                return relationsScript;
            }
            else if (dbType == "SystemLogs")
            {
                relationsScript.SystemLogsScript += tableRelationsScript;
                relationsScript.SystemLogsScript += "\n";
                return relationsScript;
            }
            else
            {
                return relationsScript;
            }
        }

        private static GeneratedScript AppendRelationsScriptToGeneratedScript(GeneratedScript generatedScript, RelationsScript relationsScript)
        {
            generatedScript.GlobalScript += relationsScript.GlobalScript;
            generatedScript.MainScript += relationsScript.MainScript;
            generatedScript.SystemLogsScript += relationsScript.SystemLogsScript;
            return generatedScript;
        }

        private static DatabaseMigrations CreateDatabaseMigrations(TableDefinition dxmlTable)
        {
            string databseType = ConfigurationManager.AppSettings["DatabseType"];
            string connectonString = GetConnectionString(dxmlTable.DBType);

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
            string connectionString = GetConnectionString(dbType);

            if (databseType.ToLower() == "oracle")
            {
                OracleConnection oracleConnection = new OracleConnection(connectionString);

                try
                {
                    string[] commands = script.Split(';');
                    commands = commands.Take(commands.Count() - 1).ToArray();
                    
                    foreach (var command in commands)
                    {
                        OracleCommand oracleCommand = new OracleCommand();
                        oracleCommand.Connection = oracleConnection;
                        oracleCommand.CommandText = command;
                        oracleConnection.Open();
                        oracleCommand.ExecuteNonQuery();
                        oracleConnection.Close();
                    }
                    return null;
                }
                catch (Exception exception)
                {
                    oracleConnection.Close();
                    return "Error: " + exception.Message;
                }
            }
            else
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);

                try
                {
                    string[] commands = script.Split(';');
                    commands = commands.Take(commands.Count() - 1).ToArray();

                    foreach (var command in commands)
                    {
                        SqlCommand sqlCommand = sqlConnection.CreateCommand();
                        sqlCommand.CommandText = command;
                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                    return null;
                }
                catch (Exception exception)
                {
                    sqlConnection.Close();
                    return "Error: " + exception.Message;
                }
            }
        }

        private static void AppendToPerformanceData(string description, Stopwatch stopwatch)
        {
            stopwatch.Stop();
            PerformanceData += description + "," + stopwatch.ElapsedMilliseconds + "\n";
        }
    }
}