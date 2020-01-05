using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace Logitude.DBMigrations.Models
{
    public class MigrationTool
    {
        private string DatabaseType = ConfigurationManager.AppSettings["DatabseType"];
        private string PerformanceData = "Description,Time(ms)\n";
        private string[] Arguments;
        private List<TableDefinition> DXMLTables;

        public MigrationTool(string[] args)
        {
            Arguments = args;
        }

        public void RunTool()
        {
            if (IsArgumentProvided("-root"))
            {
                string root = GetRoot();

                if (!String.IsNullOrEmpty(root))
                {
                    string[] dxmlFiles = GetDXMLFilesFromRoot(root);

                    if (dxmlFiles != null)
                    {
                        ValidateDXMLFiles(dxmlFiles);

                        GeneratedScript generatedScript = GenerateScriptFromDXMLFiles(dxmlFiles);

                        SaveScript(generatedScript);

                        if (IsArgumentProvided("-exe"))
                        {
                            if (IsGeneratedScriptsEmpty(generatedScript))
                            {
                                Console.WriteLine("There Are No Changes To Execute");
                            }
                            else
                            {
                                ExecuteScript(generatedScript);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("There Is No DXML Files Found Under The Specified Root");
                    }

                    ExportPerformanceData();
                }
                else
                {
                    Console.WriteLine("There Is No Root Found For Looking About DXML Files");
                }
            }
            else
            {
                Console.WriteLine("There Is No Root Found For Looking About DXML Files");
            }
        }

        private string[] GetDXMLFilesFromRoot(string root)
        {
            Console.WriteLine("Reading DXML Files From The Root ...");

            try
            {
                var stopwatch = Stopwatch.StartNew();

                string dxmlFilesPath = Path.Combine(root);
                string[] dxmlFiles = Directory.GetFiles(dxmlFilesPath, "*.dxml", SearchOption.AllDirectories);

                AppendToPerformanceData("Get DXML Files From Root", stopwatch);

                if (dxmlFiles.Length > 0)
                {
                    return dxmlFiles;
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

        private GeneratedScript GenerateScriptFromDXMLFiles(string[] dxmlFiles)
        {
            GeneratedScript generatedScript = new GeneratedScript();
            RelationsScript relationsScript = new RelationsScript();

            var stopwatch = Stopwatch.StartNew();

            List<DXMLTable> dxmlTables = GetDXMLTables(dxmlFiles);

            DXMLTables = dxmlTables.Select(d => d.TableDefinition).ToList();
            
            foreach (var dxmlTable in dxmlTables)
            {
                Console.WriteLine("Generating Script For " + dxmlTable.DXMLFileName + " ...");

                DatabaseMigrations databaseMigrations = CreateDatabaseMigrations(dxmlTable.TableDefinition);

                string tableScript = databaseMigrations.GetScript();
                string tableRelationsScript = databaseMigrations.GetRelationsScript();

                if (!String.IsNullOrEmpty(tableScript))
                {
                    generatedScript = AppendToGeneratedScript(generatedScript, dxmlTable.TableDefinition.DBType, tableScript);
                }

                if (!String.IsNullOrEmpty(tableRelationsScript))
                {
                    relationsScript = AppendToRelationsScript(relationsScript, dxmlTable.TableDefinition.DBType, tableRelationsScript);
                }
            }

            generatedScript = AppendRelationsScriptToGeneratedScript(generatedScript, relationsScript);

            AppendToPerformanceData("Generate Scripts From DXML Files", stopwatch);

            return generatedScript;
        }

        private void SaveScript(GeneratedScript generatedScript)
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

        private void ExecuteScript(GeneratedScript generatedScript)
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

        private bool IsArgumentProvided(string arg)
        {
            string[] arguments = Array.ConvertAll(Arguments, a => a.ToLower());
            return (Array.IndexOf(arguments, arg) != -1);
        }

        private string GetRoot()
        {
            string[] arguments = Array.ConvertAll(Arguments, a => a.ToLower());
            int indexOfRootArgument = Array.IndexOf(arguments, "-root") + 1;
            if (indexOfRootArgument < Arguments.Length && indexOfRootArgument >= 0)
            {
                string root = Arguments[indexOfRootArgument];
                return root;
            }
            else
            {
                return null;
            }
        }

        private void ValidateDXMLFiles(string[] dxmlFiles)
        {
            Console.WriteLine("Validating DXML Files ...");

            DXMLValidation dxmlValidation = new DXMLValidation(dxmlFiles);
            dxmlValidation.Validate();
        }

        private void ExportPerformanceData()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string csvFilePath = Path.Combine(projectDirectory, @"Reports\DBMigrationsPerformance.csv");
            File.WriteAllText(csvFilePath, PerformanceData);
        }

        private string GetConnectionString(string dbType)
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

        private GeneratedScript AppendToGeneratedScript(GeneratedScript generatedScript, string dbType, string tableScript)
        {
            if (dbType == "Global")
            {
                generatedScript.GlobalScript += tableScript;
                generatedScript.GlobalScript += "\n";
                return generatedScript;
            }
            else if (dbType == "Main")
            {
                generatedScript.MainScript += tableScript;
                generatedScript.MainScript += "\n";
                return generatedScript;
            }
            else if (dbType == "SystemLogs")
            {
                generatedScript.SystemLogsScript += tableScript;
                generatedScript.SystemLogsScript += "\n";
                return generatedScript;
            }
            else
            {
                return generatedScript;
            }
        }

        private RelationsScript AppendToRelationsScript(RelationsScript relationsScript, string dbType, string tableRelationsScript)
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

        private GeneratedScript AppendRelationsScriptToGeneratedScript(GeneratedScript generatedScript, RelationsScript relationsScript)
        {
            generatedScript.GlobalScript += relationsScript.GlobalScript;
            generatedScript.MainScript += relationsScript.MainScript;
            generatedScript.SystemLogsScript += relationsScript.SystemLogsScript;
            return generatedScript;
        }

        private DatabaseMigrations CreateDatabaseMigrations(TableDefinition dxmlTableDefinition)
        {
            string connectonString = GetConnectionString(dxmlTableDefinition.DBType);

            if (DatabaseType.ToLower() == "oracle")
            {
                DatabaseMigrations oracleDatabaseMigrations = new OracleDatabaseMigrations(dxmlTableDefinition, connectonString, DXMLTables);
                return oracleDatabaseMigrations;
            }

            DatabaseMigrations sqlDatabaseMigrations = new SQLDatabaseMigrations(dxmlTableDefinition, connectonString, DXMLTables);
            return sqlDatabaseMigrations;
        }

        private string ExecuteScript(string script, string dbType)
        {
            string connectionString = GetConnectionString(dbType);

            if (DatabaseType.ToLower() == "oracle")
            {
                OracleConnection oracleConnection = new OracleConnection(connectionString);

                try
                {
                    string[] commands = script.Split(new string[] { ";\n" }, StringSplitOptions.None);
                    commands = commands.Take(commands.Count() - 1).ToArray();

                    foreach (var command in commands)
                    {
                        OracleCommand oracleCommand = new OracleCommand();
                        oracleCommand.Connection = oracleConnection;
                        oracleCommand.CommandText = (command.EndsWith(" END") ? command + ";" : command);
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
                    string[] commands = script.Split(new string[] { ";\n" }, StringSplitOptions.None);
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

        private List<DXMLTable> GetDXMLTables(string[] dxmlFiles)
        {
            List<DXMLTable> dxmlTables = new List<DXMLTable>();

            foreach (var dxmlFile in dxmlFiles)
            {
                string xmlString = File.ReadAllText(dxmlFile);
                TableDefinition dxmlTableDefinition = xmlString.ParseXML<TableDefinition>();

                DXMLTable dxmlTable = new DXMLTable
                {
                    DXMLFileName = Path.GetFileName(dxmlFile),
                    TableDefinition = dxmlTableDefinition
                };

                dxmlTables.Add(dxmlTable);
            }

            return dxmlTables;
        }

        private bool IsGeneratedScriptsEmpty(GeneratedScript generatedScript)
        {
            return String.IsNullOrEmpty(generatedScript.GlobalScript) && String.IsNullOrEmpty(generatedScript.MainScript) && String.IsNullOrEmpty(generatedScript.SystemLogsScript);
        }

        private void AppendToPerformanceData(string description, Stopwatch stopwatch)
        {
            stopwatch.Stop();
            PerformanceData += description + "," + stopwatch.ElapsedMilliseconds + "\n";
        }
    }
}