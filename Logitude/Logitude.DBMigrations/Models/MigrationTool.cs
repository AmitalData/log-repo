using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Logitude.DBMigrations.Models
{
    public class MigrationTool
    {
        private readonly string DatabaseType = ConfigurationManager.AppSettings["DatabseType"];
        private readonly string[] Arguments;
        private readonly string ScriptSemicolonCode = "|(;)|";

        private string PerformanceData = "Description,Time(ms)\n";
        private string MissingIndexesWarnings = "";
        private List<TableDefinition> DXMLTables;
        private List<DXMLHash> DXMLHashes;

        public MigrationTool(string[] args)
        {
            Arguments = args;
        }

        public void RunTool()
        {
            if (IsArgumentProvided("-root"))// || true)
            {
                string root = GetRoot();

                //root = @"D:\TestDXMLFilesWithUniqueConstraints";

                if (!String.IsNullOrEmpty(root))
                {
                    string[] dxmlFiles = GetDXMLFilesFromRoot(root);
                    string[] sxmlFiles = GetSXMLFilesFromRoot(root);
                    
                    GeneratedScript generatedScript = null;
                    GeneratedScript generalScripts = null;

                    if (dxmlFiles != null)
                    {
                        ValidateDXMLFiles(dxmlFiles);

                        GetDXMLHashesFromDB();

                        generatedScript = GenerateScriptsFromDXMLFiles(dxmlFiles);

                        if (IsArgumentProvided("-exe"))
                        {
                            ExecuteScript(generatedScript);

                            CreateDXMLMigrationHashesTable();

                            SaveDXMLHashsOnDB(dxmlFiles);
                        }
                    }
                    else
                    {
                        Console.WriteLine("There Is No DXML Files Found Under The Specified Root");
                    }


                    if (sxmlFiles != null)
                    {
                        ValidateSXMLFilesNames(sxmlFiles);

                        List<ExecutedSxmlFile> executedSxmlFiles = GetExecutedSXMLFilesFromDB();

                        generalScripts = GetGeneralScripts(sxmlFiles, executedSxmlFiles);

                        if (IsArgumentProvided("-exe"))
                        {
                            ExecuteGeneralScripts(sxmlFiles, executedSxmlFiles);
                        }
                    }
                    else
                    {
                        Console.WriteLine("There Is No SXML Files Found Under The Specified Root");
                    }

                    GeneratedScript scriptsToSave = new GeneratedScript();

                    if(generatedScript != null)
                    {
                        scriptsToSave.GlobalScript += generatedScript.GlobalScript;
                        scriptsToSave.MainScript += generatedScript.MainScript;
                        scriptsToSave.SystemLogsScript += generatedScript.SystemLogsScript;
                    }

                    if(generalScripts != null)
                    {
                        scriptsToSave.GlobalScript += generalScripts.GlobalScript;
                        scriptsToSave.MainScript += generalScripts.MainScript;
                        scriptsToSave.SystemLogsScript += generalScripts.SystemLogsScript;
                    }

                    SaveScript(scriptsToSave);

                    //ExportPerformanceData();
                    PrintMissingIndexesWarnings();
                }
                else
                {
                    Console.WriteLine("There Is No Root Found For Looking About Files");
                }
            }
            else
            {
                Console.WriteLine("There Is No Root Found For Looking About Files");
            }
        }

        private string[] GetDXMLFilesFromRoot(string root)
        {
            Console.WriteLine("Reading DXML Files From Root ...");

            try
            {
                var stopwatch = Stopwatch.StartNew();

                string dxmlFilesRoot = Path.Combine(root);
                string[] dxmlFiles = Directory.GetFiles(dxmlFilesRoot, "*.dxml", SearchOption.AllDirectories);

                AppendToPerformanceData("Get DXML Files From Root", stopwatch);

                if (dxmlFiles.Length > 0)
                {
                    return SortDXMLFiles(dxmlFiles);
                    //return dxmlFiles.Where(d => d.ToLower().Contains(@"DBMigrationsHistory.dxml".ToLower())).ToArray();
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

        private string[] GetSXMLFilesFromRoot(string root)
        {
            Console.WriteLine("Reading SXML Files From Root ...");

            try
            {
                string sxmlFilesRoot = Path.Combine(root);
                string[] sxmlFiles = Directory.GetFiles(sxmlFilesRoot, "*.sxml", SearchOption.AllDirectories);

                if (sxmlFiles.Length > 0)
                {
                    return sxmlFiles;
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

        private GeneratedScript GenerateScriptsFromDXMLFiles(string[] dxmlFiles)
        {
            GeneratedScript generatedScript = new GeneratedScript();
            RelationsScript relationsScript = new RelationsScript();

            var stopwatch = Stopwatch.StartNew();

            DXMLTables = GetDXMLTablesDefinitions(dxmlFiles);
            DXMLDefinitions dxmlDefinitions = GetDXMLDefinitions(dxmlFiles);

            List<DXMLTable> dxmlTables = dxmlDefinitions.DXMLTables;
            List<DXMLView> dxmlViews = dxmlDefinitions.DXMLViews;
            List<DXMLProcedure> dxmlProcedures = dxmlDefinitions.DXMLProcedures;

            foreach (var dxmlTable in dxmlTables)
            {
                Console.WriteLine("Generating Script For " + dxmlTable.DXMLFileName + " ...");

                if(dxmlTable.DXMLFileName.ToLower() == "DBMigrationsHistory.dxml".ToLower() || dxmlTable.DXMLFileName.ToLower() == "DBScriptsHistory.dxml".ToLower())
                {
                    string[] dbTypes = new string[] { "Global", "Main", "SystemLogs" };

                    foreach (var dbType in dbTypes)
                    {
                        dxmlTable.TableDefinition.DBType = dbType;

                        DatabaseMigrations databaseMigrations = CreateDatabaseMigrations(dxmlTable.TableDefinition, dxmlTable.DXMLFileName);

                        string tableScript = databaseMigrations.GetScript();
                        string tableRelationsScript = databaseMigrations.GetRelationsScript();
                        string tableIndexesScript = databaseMigrations.GetIndexesScript();
                        string tableMissingIndexesWarnings = databaseMigrations.GetMissingIndexesWarnings();
                        string tableUniqueConstraintsScript = databaseMigrations.GetUniqueConstraintsScript();

                        if (!String.IsNullOrEmpty(tableScript))
                        {
                            generatedScript = AppendToGeneratedScript(generatedScript, dxmlTable.TableDefinition.DBType, tableScript);
                        }

                        if (!String.IsNullOrEmpty(tableIndexesScript))
                        {
                            generatedScript = AppendToGeneratedScript(generatedScript, dxmlTable.TableDefinition.DBType, tableIndexesScript);
                        }

                        if (!String.IsNullOrEmpty(tableMissingIndexesWarnings))
                        {
                            MissingIndexesWarnings += tableMissingIndexesWarnings;
                        }

                        if (!String.IsNullOrEmpty(tableUniqueConstraintsScript))
                        {
                            generatedScript = AppendToGeneratedScript(generatedScript, dxmlTable.TableDefinition.DBType, tableUniqueConstraintsScript);
                        }

                        if (!String.IsNullOrEmpty(tableRelationsScript))
                        {
                            relationsScript = AppendToRelationsScript(relationsScript, dxmlTable.TableDefinition.DBType, tableRelationsScript);
                        }
                    }
                }
                else
                {
                    DatabaseMigrations databaseMigrations = CreateDatabaseMigrations(dxmlTable.TableDefinition, dxmlTable.DXMLFileName);

                    string tableScript = databaseMigrations.GetScript();
                    string tableRelationsScript = databaseMigrations.GetRelationsScript();
                    string tableIndexesScript = databaseMigrations.GetIndexesScript();
                    string tableMissingIndexesWarnings = databaseMigrations.GetMissingIndexesWarnings();
                    string tableUniqueConstraintsScript = databaseMigrations.GetUniqueConstraintsScript();

                    if (!String.IsNullOrEmpty(tableScript))
                    {
                        generatedScript = AppendToGeneratedScript(generatedScript, dxmlTable.TableDefinition.DBType, tableScript);
                    }

                    if (!String.IsNullOrEmpty(tableIndexesScript))
                    {
                        generatedScript = AppendToGeneratedScript(generatedScript, dxmlTable.TableDefinition.DBType, tableIndexesScript);
                    }

                    if (!String.IsNullOrEmpty(tableMissingIndexesWarnings))
                    {
                        MissingIndexesWarnings += tableMissingIndexesWarnings;
                    }

                    if (!String.IsNullOrEmpty(tableUniqueConstraintsScript))
                    {
                        generatedScript = AppendToGeneratedScript(generatedScript, dxmlTable.TableDefinition.DBType, tableUniqueConstraintsScript);
                    }

                    if (!String.IsNullOrEmpty(tableRelationsScript))
                    {
                        relationsScript = AppendToRelationsScript(relationsScript, dxmlTable.TableDefinition.DBType, tableRelationsScript);
                    }
                }
            }

            generatedScript = AppendRelationsScriptToGeneratedScript(generatedScript, relationsScript);

            foreach (var dxmlView in dxmlViews)
            {
                Console.WriteLine("Generating Script For " + dxmlView.DXMLFileName + " ...");

                string viewScript = GetScriptFromViewDefinition(dxmlView.ViewDefinition, dxmlView.DXMLFileName);

                if (!String.IsNullOrEmpty(viewScript))
                {
                    generatedScript = AppendToGeneratedScript(generatedScript, dxmlView.ViewDefinition.DBType, ReplaceScriptSemicolon(viewScript));
                }
            }

            foreach (var dxmlProcedure in dxmlProcedures)
            {
                Console.WriteLine("Generating Script For " + dxmlProcedure.DXMLFileName + " ...");

                string procedureScript = GetScriptFromProcedureDefinition(dxmlProcedure.ProcedureDefinition, dxmlProcedure.DXMLFileName);

                if (!String.IsNullOrEmpty(procedureScript))
                {
                    generatedScript = AppendToGeneratedScript(generatedScript, dxmlProcedure.ProcedureDefinition.DBType, ReplaceScriptSemicolon(procedureScript));
                }
            }

            AppendToPerformanceData("Generate Scripts From DXML Files", stopwatch);

            return generatedScript;
        }

        private void SaveScript(GeneratedScript generatedScript)
        {
            var stopwatch = Stopwatch.StartNew();

            string globalScript = generatedScript.GlobalScript.Replace(ScriptSemicolonCode, ";");
            string mainScript = generatedScript.MainScript.Replace(ScriptSemicolonCode, ";");
            string systemLogsScript = generatedScript.SystemLogsScript.Replace(ScriptSemicolonCode, ";");

            Console.WriteLine("Saving The Generated Scripts ...");
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            string globalScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\GlobalScript.sql");
            File.WriteAllText(globalScriptFilePath, globalScript);

            string mainScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\MainScript.sql");
            File.WriteAllText(mainScriptFilePath, mainScript);

            string systemLogsScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\SystemLogsScript.sql");
            File.WriteAllText(systemLogsScriptFilePath, systemLogsScript);

            if (IsGeneratedScriptsEmpty(generatedScript))
            {
                Console.WriteLine("There Are No Scripts Generated");
            }
            else
            {
                Console.WriteLine("The Generated Scripts Saved Successfully");
            }

            AppendToPerformanceData("Save The Generated Scripts", stopwatch);
        }

        private void ExecuteScript(GeneratedScript generatedScript)
        {
            var stopwatch = Stopwatch.StartNew();

            if (IsGeneratedScriptsEmpty(generatedScript))
            {
                Console.WriteLine("There Are No Changes To Execute");
            }
            else
            {
                if (!String.IsNullOrEmpty(generatedScript.GlobalScript))
                {
                    Console.WriteLine("Executing Script On Global Database ...");
                    string result = ExecuteScript(generatedScript.GlobalScript, "Global");
                    if (!String.IsNullOrEmpty(result))
                    {
                        ExitTool(result);
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
                        ExitTool(result);
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
                        ExitTool(result);
                    }
                    else
                    {
                        Console.WriteLine("Scripts Executed Successfully On SystemLogs Database");
                    }
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

        private void PrintMissingIndexesWarnings()
        {
            if (!String.IsNullOrEmpty(MissingIndexesWarnings))
            {
                Console.WriteLine(MissingIndexesWarnings.TrimEnd('\n'));
            }
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

        private GeneratedScript AppendToGeneratedScript(GeneratedScript generatedScript, string dbType, string script)
        {
            if (dbType == "Global")
            {
                generatedScript.GlobalScript += script;
                generatedScript.GlobalScript += "\n";
                return generatedScript;
            }
            else if (dbType == "Main")
            {
                generatedScript.MainScript += script;
                generatedScript.MainScript += "\n";
                return generatedScript;
            }
            else if (dbType == "SystemLogs")
            {
                generatedScript.SystemLogsScript += script;
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

        private DatabaseMigrations CreateDatabaseMigrations(TableDefinition dxmlTableDefinition, string dxmlFileName)
        {
            string connectonString = GetConnectionString(dxmlTableDefinition.DBType);

            if (DatabaseType.ToLower() == "oracle")
            {
                DatabaseMigrations oracleDatabaseMigrations = new OracleDatabaseMigrations(dxmlTableDefinition, connectonString, DXMLTables, dxmlFileName);
                return oracleDatabaseMigrations;
            }

            DatabaseMigrations sqlDatabaseMigrations = new SQLDatabaseMigrations(dxmlTableDefinition, connectonString, DXMLTables, dxmlFileName);
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
                    oracleConnection.Open();

                    string[] commands = script.Split(new string[] { ";\n" }, StringSplitOptions.None);
                    commands = commands.Take(commands.Count() - 1).Select(c => c.Replace(ScriptSemicolonCode, ";")).ToArray();

                    using (OracleCommand oracleCommand = new OracleCommand())
                    {
                        oracleCommand.Connection = oracleConnection;
                        
                        foreach (var command in commands)
                        {
                            oracleCommand.CommandText = (command.ToUpper().EndsWith(" END") || command.ToUpper().EndsWith("\nEND")) ? (command + ";") : command;
                            oracleCommand.ExecuteNonQuery();
                        }
                    }

                    oracleConnection.Close();

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
                    sqlConnection.Open();

                    string[] commands = script.Split(new string[] { ";\n" }, StringSplitOptions.None);
                    commands = commands.Take(commands.Count() - 1).Select(c => c.Replace(ScriptSemicolonCode, ";")).ToArray();

                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;

                        foreach (var command in commands)
                        {
                            sqlCommand.CommandText = command;
                            sqlCommand.ExecuteNonQuery();
                        }
                    }

                    sqlConnection.Close();

                    return null;
                }
                catch (Exception exception)
                {
                    sqlConnection.Close();
                    return "Error: " + exception.Message;
                }
            }
        }

        private DXMLDefinitions GetDXMLDefinitions(string[] dxmlFiles)
        {
            List<DXMLTable> dxmlTables = new List<DXMLTable>();
            List<DXMLView> dxmlViews = new List<DXMLView>();
            List<DXMLProcedure> dxmlProcedures = new List<DXMLProcedure>();
            bool checkDxmlHash = !IsArgumentProvided("-ignorehash");

            foreach (var dxmlFile in dxmlFiles)
            {
                bool takeDxmlFile = true;

                string dxmlString = File.ReadAllText(dxmlFile);
                
                if (checkDxmlHash)
                {
                    DXMLHash dxmlHashFromDB = DXMLHashes.Where(d => d.FileName == Path.GetFileName(dxmlFile)).FirstOrDefault();
                    if(dxmlHashFromDB != null)
                    {
                        string dxmlHashStringFromFile = GenerateDXMLHashString(dxmlString);
                        string dxmlHashStringFromDB = dxmlHashFromDB.HashString;

                        if(dxmlHashStringFromFile == dxmlHashStringFromDB)
                        {
                            takeDxmlFile = false;
                        }
                    }
                }

                if (takeDxmlFile)
                {
                    if (dxmlString.EndsWith("</Table>"))
                    {
                        DXMLTable dxmlTable = CreateDXMLTable(dxmlString, dxmlFile);
                        if (dxmlTable == null)
                        {
                            ExitTool("Error: Cannot Create Table Definition For " + Path.GetFileName(dxmlFile));
                        }
                        dxmlTables.Add(dxmlTable);
                    }
                    else if (dxmlString.EndsWith("</View>"))
                    {
                        DXMLView dxmlView = CreateDXMLView(dxmlString, dxmlFile);
                        if (dxmlView == null)
                        {
                            ExitTool("Error: Cannot Create View Definition For " + Path.GetFileName(dxmlFile));
                        }
                        dxmlViews.Add(dxmlView);
                    }
                    else if (dxmlString.EndsWith("</Procedure>"))
                    {
                        DXMLProcedure dxmlProcedure = CreateDXMLProcedure(dxmlString, dxmlFile);
                        if (dxmlProcedure == null)
                        {
                            ExitTool("Error: Cannot Create Procedure Definition For " + Path.GetFileName(dxmlFile));
                        }
                        dxmlProcedures.Add(dxmlProcedure);
                    }
                    else
                    {
                        ExitTool("Error: Cannot Create Class Definition For " + Path.GetFileName(dxmlFile));
                    }
                }
            }

            return new DXMLDefinitions
            {
                DXMLTables = dxmlTables,
                DXMLViews = dxmlViews,
                DXMLProcedures = dxmlProcedures
            };
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

        private string GetScriptFromViewDefinition(ViewDefinition viewDefinition, string dxmlFileName)
        {
            return null;

            string viewScript = "-- DataView Script From " + dxmlFileName + "\n";

            if (DatabaseType.ToLower() == "oracle")
            {
                if (String.IsNullOrEmpty(viewDefinition.OracleScript))
                {
                    return null;
                }

                string unescapedScript = UnescapeScript(viewDefinition.OracleScript);
                viewScript += unescapedScript + (unescapedScript.EndsWith(";") ? null : ";") + "\n\n";
                return viewScript;
            }
            else
            {
                if (String.IsNullOrEmpty(viewDefinition.SqlScript))
                {
                    return null;
                }

                viewScript += "EXEC('IF (OBJECT_ID(''" + "[" + viewDefinition.Schema + "].[" + viewDefinition.Name + "]" + "'', ''V'') IS NOT NULL) BEGIN DROP VIEW " + "[" + viewDefinition.Schema + "].[" + viewDefinition.Name + "]" + " END" + "');\n";
                viewScript += "EXEC('" + UnescapeScript(viewDefinition.SqlScript).Replace("'", "''") + "');" + "\n\n";
                return viewScript;
            }
        }

        private string GetScriptFromProcedureDefinition(ProcedureDefinition procedureDefinition, string dxmlFileName)
        {
            return null;

            string procedureScript = "-- Procedure Script From " + dxmlFileName + "\n";

            if (DatabaseType.ToLower() == "oracle")
            {
                if (String.IsNullOrEmpty(procedureDefinition.OracleScript))
                {
                    return null;
                }

                string unescapedScript = UnescapeScript(procedureDefinition.OracleScript);
                procedureScript += unescapedScript + (unescapedScript.EndsWith(";") ? null : ";") + "\n\n";
                return procedureScript;
            }
            else
            {
                if (String.IsNullOrEmpty(procedureDefinition.SqlScript))
                {
                    return null;
                }

                procedureScript += "EXEC('IF (OBJECT_ID(''" + "[" + procedureDefinition.Schema + "].[" + procedureDefinition.Name + "]" + "'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE " + "[" + procedureDefinition.Schema + "].[" + procedureDefinition.Name + "]" + " END" + "');\n";
                procedureScript += "EXEC('" + UnescapeScript(procedureDefinition.SqlScript).Replace("'", "''") + "');" + "\n\n";
                return procedureScript;
            }
        }

        private DXMLTable CreateDXMLTable(string dxmlString, string dxmlFile)
        {
            try
            {
                TableDefinition dxmlTableDefinition = dxmlString.ParseXML<TableDefinition>();

                DXMLTable dxmlTable = new DXMLTable
                {
                    DXMLFileName = Path.GetFileName(dxmlFile),
                    TableDefinition = dxmlTableDefinition
                };

                return dxmlTable;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private DXMLView CreateDXMLView(string dxmlString, string dxmlFile)
        {
            try
            {
                string sqlScriptFromXmlString = dxmlString.Split(new string[] { "<SqlScript>" }, StringSplitOptions.None)[1].Split(new string[] { "</SqlScript>" }, StringSplitOptions.None)[0];
                string escapedSqlScript = SecurityElement.Escape(GetScriptFromCDataSection(sqlScriptFromXmlString)).Trim();

                string oracleScriptFromXmlString = dxmlString.Split(new string[] { "<OracleScript>" }, StringSplitOptions.None)[1].Split(new string[] { "</OracleScript>" }, StringSplitOptions.None)[0];
                string escapedOracleScript = SecurityElement.Escape(GetScriptFromCDataSection(oracleScriptFromXmlString)).Trim();

                dxmlString = dxmlString.Split(new string[] { "<SqlScript>" }, StringSplitOptions.None)[0] + "<SqlScript>" + escapedSqlScript + "</SqlScript>" + "<OracleScript>" + escapedOracleScript + "</OracleScript>" + "</View>";

                ViewDefinition dxmlViewDefinition = dxmlString.ParseXML<ViewDefinition>();

                DXMLView dxmlView = new DXMLView
                {
                    DXMLFileName = Path.GetFileName(dxmlFile),
                    ViewDefinition = dxmlViewDefinition
                };

                return dxmlView;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private DXMLProcedure CreateDXMLProcedure(string dxmlString, string dxmlFile)
        {
            try
            {
                string sqlScriptFromXmlString = dxmlString.Split(new string[] { "<SqlScript>" }, StringSplitOptions.None)[1].Split(new string[] { "</SqlScript>" }, StringSplitOptions.None)[0];
                string escapedSqlScript = SecurityElement.Escape(GetScriptFromCDataSection(sqlScriptFromXmlString)).Trim();

                string oracleScriptFromXmlString = dxmlString.Split(new string[] { "<OracleScript>" }, StringSplitOptions.None)[1].Split(new string[] { "</OracleScript>" }, StringSplitOptions.None)[0];
                string escapedOracleScript = SecurityElement.Escape(GetScriptFromCDataSection(oracleScriptFromXmlString)).Trim();

                dxmlString = dxmlString.Split(new string[] { "<SqlScript>" }, StringSplitOptions.None)[0] + "<SqlScript>" + escapedSqlScript + "</SqlScript>" + "<OracleScript>" + escapedOracleScript + "</OracleScript>" + "</Procedure>";

                ProcedureDefinition dxmlProcedureDefinition = dxmlString.ParseXML<ProcedureDefinition>();

                DXMLProcedure dxmlProcedure = new DXMLProcedure
                {
                    DXMLFileName = Path.GetFileName(dxmlFile),
                    ProcedureDefinition = dxmlProcedureDefinition
                };

                return dxmlProcedure;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string UnescapeScript(string escapedScript)
        {
            string unescapedScript = escapedScript;
            unescapedScript = unescapedScript.Replace("&apos;", "'");
            unescapedScript = unescapedScript.Replace("&quot;", "\"");
            unescapedScript = unescapedScript.Replace("&gt;", ">");
            unescapedScript = unescapedScript.Replace("&lt;", "<");
            unescapedScript = unescapedScript.Replace("&amp;", "&");
            return unescapedScript;
        }

        private string GetScriptFromCDataSection(string script)
        {
            script = string.Join("\n", script.Split('\n').Select(l => l.Trim()).Where(l => !String.IsNullOrEmpty(l)).ToArray());

            if (script.Contains("<![CDATA[") && script.Contains("]]>"))
            {
                return script.Split(new string[] { "<![CDATA[" }, StringSplitOptions.None)[1].Split(new string[] { "]]>" }, StringSplitOptions.None)[0];
            }

            return script;
        }

        private string ReplaceScriptSemicolon(string script)
        {
            int lastIndex = script.LastIndexOf(';');
            if (lastIndex > 0)
            {
                script = script.Substring(0, lastIndex).Replace(";", ScriptSemicolonCode) + script.Substring(lastIndex);
                return script;
            }
            else
            {
                return script;
            }
        }

        private List<TableDefinition> GetDXMLTablesDefinitions(string[] dxmlFiles)
        {
            List<TableDefinition> dxmlTablesDefinitions = new List<TableDefinition>();

            foreach (var dxmlFile in dxmlFiles)
            {
                string dxmlString = File.ReadAllText(dxmlFile);

                if (dxmlString.EndsWith("</Table>"))
                {
                    try
                    {
                        TableDefinition dxmlTableDefinition = dxmlString.ParseXML<TableDefinition>();
                        dxmlTablesDefinitions.Add(dxmlTableDefinition);
                    }
                    catch (Exception)
                    {
                        ExitTool("Error: Cannot Create Table Definition For " + Path.GetFileName(dxmlFile));
                    }
                }
            }

            return dxmlTablesDefinitions;
        }

        private void CreateDXMLMigrationHashesTable()
        {
            string connectionString = GetConnectionString("Main");

            if (DatabaseType.ToLower() == "oracle")
            {
                string queryString = "DECLARE TableCount NUMBER; " +
                    "BEGIN " +
                    "SELECT COUNT(*) INTO TableCount FROM USER_TABLES WHERE TABLE_NAME = 'DXMLMIGRATIONHASHES'; " +
                    "IF (TableCount = 0) " +
                    "THEN " +
                    "EXECUTE IMMEDIATE 'CREATE TABLE \"DXMLMIGRATIONHASHES\"( " +
                    "\"DXMLFILENAME\" VARCHAR2(500 CHAR) NOT NULL, " +
                    "\"HASHSTRING\" NCLOB NOT NULL, " +
                    "PRIMARY KEY(\"DXMLFILENAME\"))'; " +
                    "END IF; " +
                    "END;";

                OracleConnection oracleConnection = new OracleConnection(connectionString);

                try
                {
                    oracleConnection.Open();
                    OracleCommand oracleCommand = new OracleCommand();
                    oracleCommand.Connection = oracleConnection;
                    oracleCommand.CommandText = queryString;
                    oracleCommand.ExecuteNonQuery();
                    oracleConnection.Close();
                }
                catch (Exception exception)
                {
                    oracleConnection.Close();
                    ExitTool("Error: " + exception.Message);
                }
            }
            else
            {
                string queryString = "EXEC('IF (OBJECT_ID(''[dbo].[DXMLMigrationHashes]'', ''U'') IS NULL) " +
                                     "BEGIN " +
                                     "CREATE TABLE [dbo].[DXMLMigrationHashes]( " +
                                     "[DxmlFileName] VARCHAR(500) NOT NULL, " +
                                     "[HashString] NVARCHAR(MAX) NOT NULL, " +
                                     "PRIMARY KEY([DxmlFileName]) " +
                                     ") " +
                                     "END');";

                SqlConnection sqlConnection = new SqlConnection(connectionString);

                try
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = queryString;
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                catch (Exception exception)
                {
                    sqlConnection.Close();
                    ExitTool("Error: " + exception.Message);
                }
            }
        }

        private void GetDXMLHashesFromDB()
        {
            string connectionString = GetConnectionString("Main");

            if (IsTableInDB("DXMLMigrationHashes"))
            {
                if (DatabaseType.ToLower() == "oracle")
                {
                    string queryString = "SELECT * FROM \"DXMLMIGRATIONHASHES\"";

                    List<DXMLHash> dxmlHashes = new List<DXMLHash>();

                    OracleDataReader reader = null;
                    OracleConnection connection = new OracleConnection(connectionString);
                    OracleCommand command = new OracleCommand(queryString, connection);

                    try
                    {
                        connection.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            DXMLHash dxmlHash = new DXMLHash
                            {
                                FileName = reader["DXMLFILENAME"].ToString(),
                                HashString = reader["HASHSTRING"].ToString()
                            };
                            dxmlHashes.Add(dxmlHash);
                        }

                        reader.Close();
                        connection.Close();
                    }
                    catch (Exception exception)
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                        connection.Close();

                        ExitTool("Error: " + exception.Message);
                    }

                    DXMLHashes = dxmlHashes;
                }
                else
                {
                    string queryString = "SELECT * FROM [dbo].[DXMLMigrationHashes]";

                    List<DXMLHash> dxmlHashes = new List<DXMLHash>();

                    SqlDataReader reader = null;
                    SqlConnection connection = new SqlConnection(connectionString);
                    SqlCommand command = new SqlCommand(queryString, connection);

                    try
                    {
                        connection.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            DXMLHash dxmlHash = new DXMLHash
                            {
                                FileName = reader["DxmlFileName"].ToString(),
                                HashString = reader["HashString"].ToString()
                            };
                            dxmlHashes.Add(dxmlHash);
                        }

                        reader.Close();
                        connection.Close();
                    }
                    catch (Exception exception)
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                        connection.Close();

                        ExitTool("Error: " + exception.Message);
                    }

                    DXMLHashes = dxmlHashes;
                }
            }
            else
            {
                DXMLHashes = new List<DXMLHash>();
            }
        }

        private void SaveDXMLHashsOnDB(string[] dxmlFiles)
        {
            string connectionString = GetConnectionString("Main");

            foreach(var dxmlFile in dxmlFiles)
            {
                bool saveDxmlHash = true;

                string dxmlFileName = Path.GetFileName(dxmlFile);
                string dxmlString = File.ReadAllText(dxmlFile);
                string dxmlHashStringFromFile = GenerateDXMLHashString(dxmlString);

                DXMLHash dxmlHashFromDB = DXMLHashes.Where(d => d.FileName == dxmlFileName).FirstOrDefault();
                if (dxmlHashFromDB != null)
                {
                    string dxmlHashStringFromDB = dxmlHashFromDB.HashString;

                    if (dxmlHashStringFromFile == dxmlHashStringFromDB)
                    {
                        saveDxmlHash = false;
                    }
                }

                if (saveDxmlHash)
                {
                    if (DatabaseType.ToLower() == "oracle")
                    {
                        string queryString = "DECLARE FileCount NUMBER; " +
                                             "BEGIN " +
                                             "SELECT COUNT(*) INTO FileCount FROM \"DXMLMIGRATIONHASHES\" WHERE DXMLFILENAME = '" + dxmlFileName + "'; " +
                                             "IF(FileCount = 0) " +
                                             "THEN " +
                                             "EXECUTE IMMEDIATE 'INSERT INTO \"DXMLMIGRATIONHASHES\"(DXMLFILENAME, HASHSTRING) VALUES(''" + dxmlFileName + "'', ''" + dxmlHashStringFromFile + "'')'; " +
                                             "ELSE " +
                                             "EXECUTE IMMEDIATE 'UPDATE \"DXMLMIGRATIONHASHES\" SET HASHSTRING = ''" + dxmlHashStringFromFile + "'' WHERE DXMLFILENAME = ''" + dxmlFileName + "'''; " +
                                             "END IF; " +
                                             "END;";

                        OracleConnection oracleConnection = new OracleConnection(connectionString);

                        try
                        {
                            oracleConnection.Open();
                            OracleCommand oracleCommand = new OracleCommand();
                            oracleCommand.Connection = oracleConnection;
                            oracleCommand.CommandText = queryString;
                            oracleCommand.ExecuteNonQuery();
                            oracleConnection.Close();
                        }
                        catch (Exception exception)
                        {
                            oracleConnection.Close();
                            ExitTool("Error: " + exception.Message);
                        }
                    }
                    else
                    {
                        string queryString = "EXEC('IF (SELECT COUNT(*) FROM [dbo].[DXMLMigrationHashes] WHERE DxmlFileName = ''" + dxmlFileName + "'') = 0 " +
                                             "BEGIN " +
                                             "INSERT INTO [dbo].[DXMLMigrationHashes](DxmlFileName, HashString) VALUES(''" + dxmlFileName + "'', ''" + dxmlHashStringFromFile + "'') " +
                                             "END " +
                                             "ELSE " +
                                             "BEGIN " +
                                             "UPDATE [dbo].[DXMLMigrationHashes] SET HashString = ''" + dxmlHashStringFromFile + "'' WHERE DxmlFileName = ''" + dxmlFileName + "'' " +
                                             "END');";

                        SqlConnection sqlConnection = new SqlConnection(connectionString);

                        try
                        {
                            sqlConnection.Open();
                            SqlCommand sqlCommand = new SqlCommand();
                            sqlCommand.Connection = sqlConnection;
                            sqlCommand.CommandText = queryString;
                            sqlCommand.ExecuteNonQuery();
                            sqlConnection.Close();
                        }
                        catch (Exception exception)
                        {
                            sqlConnection.Close();
                            ExitTool("Error: " + exception.Message);
                        }
                    }
                }
            }
        }

        private bool IsTableInDB(string tableName)
        {
            string connectionString = GetConnectionString("Main");

            if (DatabaseType.ToLower() == "oracle")
            {
                string queryString = "SELECT * FROM USER_TABLES WHERE TABLE_NAME = '" + tableName.ToUpper() + "'";

                bool tableExists = false;

                OracleDataReader reader = null;
                OracleConnection connection = new OracleConnection(connectionString);
                OracleCommand command = new OracleCommand(queryString, connection);

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        tableExists = true;
                    }

                    reader.Close();
                    connection.Close();
                }
                catch (Exception exception)
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    connection.Close();

                    ExitTool("Error: " + exception.Message);
                }

                return tableExists;
            }
            else
            {
                string queryString = "SELECT * FROM SYSOBJECTS WHERE name='" + tableName + "' AND xtype='U'";

                bool tableExists = false;

                SqlDataReader reader = null;
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        tableExists = true;
                    }

                    reader.Close();
                    connection.Close();
                }
                catch (Exception exception)
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    connection.Close();

                    ExitTool("Error: " + exception.Message);
                }

                return tableExists;
            }
        }

        private string GenerateDXMLHashString(string dxmlString)
        {
            MD5CryptoServiceProvider cryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] dxmlStringBytes = Encoding.UTF8.GetBytes(dxmlString);
            byte[] computedHash = cryptoServiceProvider.ComputeHash(dxmlStringBytes);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var h in computedHash)
            {
                stringBuilder.Append(h.ToString("x2").ToLower());
            }
            return stringBuilder.ToString();
        }

        private string[] SortDXMLFiles(string[] dxmlFiles)
        {
            List<string> dxmlFilesList = dxmlFiles.ToList();

            string dbScriptsHistoryDxmlFile = dxmlFilesList.Where(d => d.ToLower().Contains(@"\DBScriptsHistory.dxml".ToLower())).FirstOrDefault();
            string dbMigrationsHistoryDxmlFile = dxmlFilesList.Where(d => d.ToLower().Contains(@"\DBMigrationsHistory.dxml".ToLower())).FirstOrDefault();

            if (dbScriptsHistoryDxmlFile != null)
            {
                int indexOfDBScriptsHistoryDxmlFile = dxmlFilesList.FindIndex(d => d.ToLower().Contains(@"\DBScriptsHistory.dxml".ToLower()));
                dxmlFilesList.RemoveAt(indexOfDBScriptsHistoryDxmlFile);
                dxmlFilesList.Insert(0, dbScriptsHistoryDxmlFile);
            }

            if (dbMigrationsHistoryDxmlFile != null)
            {
                int indexOfDBMigrationsHistoryDxmlFile = dxmlFilesList.FindIndex(d => d.ToLower().Contains(@"\DBMigrationsHistory.dxml".ToLower()));
                dxmlFilesList.RemoveAt(indexOfDBMigrationsHistoryDxmlFile);
                dxmlFilesList.Insert(0, dbMigrationsHistoryDxmlFile);
            }

            return dxmlFilesList.ToArray();
        }

        private string ExecuteGeneralScript(string script, string dbType)
        {
            string connectionString = GetConnectionString(dbType);

            if (DatabaseType.ToLower() == "oracle")
            {
                using (OracleConnection oracleConnection = new OracleConnection(connectionString))
                {
                    try
                    {
                        oracleConnection.Open();
                    }
                    catch (Exception exception)
                    {
                        return "Error: " + exception.Message;
                    }

                    using (OracleTransaction oracleTransaction = oracleConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            OracleCommand oracleCommand = new OracleCommand(script, oracleConnection);
                            oracleCommand.Transaction = oracleTransaction;
                            oracleCommand.ExecuteNonQuery();
                            oracleTransaction.Commit();
                            return null;
                        }
                        catch (Exception exception)
                        {
                            oracleTransaction.Rollback();
                            oracleConnection.Close();
                            return "Error: " + exception.Message;
                        }
                    }
                }
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    try
                    {
                        sqlConnection.Open();
                    }
                    catch(Exception exception)
                    {
                        return "Error: " + exception.Message;
                    }
                    
                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            SqlCommand sqlCommand = new SqlCommand(script, sqlConnection, sqlTransaction);
                            sqlCommand.ExecuteNonQuery();
                            sqlTransaction.Commit();
                            return null;
                        }
                        catch (Exception exception)
                        {
                            sqlTransaction.Rollback();
                            sqlConnection.Close();
                            return "Error: " + exception.Message;
                        }
                    }
                }
            }
        }

        private void ExecuteGeneralScripts(string[] sxmlFiles, List<ExecutedSxmlFile> executedSxmlFiles)
        {
            int executedSxmlFilesCount = 0;

            foreach (var sxmlFile in sxmlFiles)
            {
                string sxmlFileName = Path.GetFileName(sxmlFile);

                ScriptDefinition scriptDefinition;
                string sxmlString = File.ReadAllText(sxmlFile);

                try
                {
                    scriptDefinition = sxmlString.ParseXML<ScriptDefinition>();
                }
                catch (Exception)
                {
                    scriptDefinition = null;
                }

                if (scriptDefinition != null)
                {
                    if (!executedSxmlFiles.Where(e => e.SxmlFileName.ToLower() == sxmlFileName.ToLower() && e.DBType.ToLower() == scriptDefinition.DBType.ToLower()).Any())
                    {
                        Console.WriteLine("Executing Script From " + sxmlFileName + " File ...");

                        string scriptBody;

                        if (DatabaseType.ToLower() == "oracle")
                        {
                            scriptBody = GetScriptFromCDataSection(scriptDefinition.Oracle);
                            string blockedScriptBody;
                            if (!String.IsNullOrEmpty(scriptBody))
                            {
                                blockedScriptBody = "BEGIN\nBEGIN\n" + scriptBody + "\nEND;\nBEGIN\n" + "DECLARE ScriptBody NCLOB; BEGIN ScriptBody := '" + scriptBody.Replace("'", "''").TrimEnd(new char[] { '\r', '\n' }) + "'; INSERT INTO \"DBSCRIPTSHISTORY\"(\"SXMLFILENAME\", \"EXECUTIONDATE\", \"SCRIPTBODY\")VALUES('" + sxmlFileName + "', SYSDATE, ScriptBody); END;\nEND;\nEND;";
                            }
                            else
                            {
                                blockedScriptBody = "BEGIN\nDECLARE ScriptBody NCLOB; BEGIN ScriptBody := '" + "NULL" + "'; INSERT INTO \"DBSCRIPTSHISTORY\"(\"SXMLFILENAME\", \"EXECUTIONDATE\", \"SCRIPTBODY\")VALUES('" + sxmlFileName + "', SYSDATE, ScriptBody); END;\nEND;";
                            }
                            scriptBody = blockedScriptBody;
                        }
                        else
                        {
                            scriptBody = GetScriptFromCDataSection(scriptDefinition.Sql);
                            if (!String.IsNullOrEmpty(scriptBody))
                            {
                                scriptBody += "\n";
                            }
                            scriptBody += "INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody])VALUES('" + sxmlFileName + "', GETDATE(), '" + (!String.IsNullOrEmpty(scriptBody) ? scriptBody.Replace("'", "''").TrimEnd(new char[] { '\r', '\n' }) : "NULL") + "');";
                        }

                        string result = ExecuteGeneralScript(scriptBody, scriptDefinition.DBType);

                        if (result != null)
                        {
                            ExitTool(result);
                        }
                        else
                        {
                            executedSxmlFilesCount++;
                        }
                    }
                }
                else
                {
                    ExitTool("Error: Cannot Create Script Definition For " + sxmlFileName);
                }
            }

            if (executedSxmlFilesCount > 0)
            {
                Console.WriteLine("General Scripts Executed Successfully");
            }
            else
            {
                Console.WriteLine("All General Scripts Already Executed");
            }
        }

        private GeneratedScript GetGeneralScripts(string[] sxmlFiles, List<ExecutedSxmlFile> executedSxmlFiles)
        {
            GeneratedScript generalScripts = new GeneratedScript();

            foreach (var sxmlFile in sxmlFiles)
            {
                string sxmlFileName = Path.GetFileName(sxmlFile);

                ScriptDefinition scriptDefinition;
                string sxmlString = File.ReadAllText(sxmlFile);

                try
                {
                    scriptDefinition = sxmlString.ParseXML<ScriptDefinition>();
                }
                catch (Exception)
                {
                    scriptDefinition = null;
                }

                if (scriptDefinition != null)
                {
                    if (!executedSxmlFiles.Where(e => e.SxmlFileName.ToLower() == sxmlFileName.ToLower() && e.DBType.ToLower() == scriptDefinition.DBType.ToLower()).Any())
                    {
                        string scriptBody;

                        if (DatabaseType.ToLower() == "oracle")
                        {
                            scriptBody = GetScriptFromCDataSection(scriptDefinition.Oracle);
                            string blockedScriptBody;
                            if (!String.IsNullOrEmpty(scriptBody))
                            {
                                blockedScriptBody = "BEGIN\nBEGIN\n" + scriptBody + "\nEND;\nBEGIN\n" + "DECLARE ScriptBody NCLOB; BEGIN ScriptBody := '" + scriptBody.Replace("'", "''").TrimEnd(new char[] { '\r', '\n' }) + "'; INSERT INTO \"DBSCRIPTSHISTORY\"(\"SXMLFILENAME\", \"EXECUTIONDATE\", \"SCRIPTBODY\")VALUES('" + sxmlFileName + "', SYSDATE, ScriptBody); END;\nEND;\nEND;";
                            }
                            else
                            {
                                blockedScriptBody = "BEGIN\nDECLARE ScriptBody NCLOB; BEGIN ScriptBody := '" + "NULL" + "'; INSERT INTO \"DBSCRIPTSHISTORY\"(\"SXMLFILENAME\", \"EXECUTIONDATE\", \"SCRIPTBODY\")VALUES('" + sxmlFileName + "', SYSDATE, ScriptBody); END;\nEND;";
                            }
                            scriptBody = blockedScriptBody;
                        }
                        else
                        {
                            scriptBody = GetScriptFromCDataSection(scriptDefinition.Sql);
                            if (!String.IsNullOrEmpty(scriptBody))
                            {
                                scriptBody += "\n";
                            }
                            scriptBody += "INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody])VALUES('" + sxmlFileName + "', GETDATE(), '" + (!String.IsNullOrEmpty(scriptBody) ? scriptBody.Replace("'", "''").TrimEnd(new char[] { '\r', '\n' }) : "NULL") + "');";
                        }

                        string scriptToAppend = "-- General Script From " + sxmlFileName + " File\n" + scriptBody;

                        AppendToGeneratedScript(generalScripts, scriptDefinition.DBType, scriptToAppend);
                    }
                }
                else
                {
                    ExitTool("Error: Cannot Create Script Definition For " + sxmlFileName);
                }
            }

            return generalScripts;
        }

        private List<ExecutedSxmlFile> GetExecutedSXMLFilesFromDB()
        {
            Console.WriteLine("Reading Executed SXML Files From DB ...");

            List<ExecutedSxmlFile> executedSxmlFiles = new List<ExecutedSxmlFile>();

            string[] dbTypes = new string[] { "Global", "Main", "SystemLogs" };

            foreach(var dbType in dbTypes)
            {
                string connectionString = GetConnectionString(dbType);

                if (DatabaseType.ToLower() == "oracle")
                {
                    string queryString = "SELECT * FROM \"DBSCRIPTSHISTORY\"";

                    OracleDataReader reader = null;
                    OracleConnection connection = new OracleConnection(connectionString);
                    OracleCommand command = new OracleCommand(queryString, connection);

                    try
                    {
                        connection.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ExecutedSxmlFile executedSxmlFile = new ExecutedSxmlFile
                            {
                                SxmlFileName = reader["SXMLFILENAME"].ToString(),
                                DBType = dbType
                            };
                            executedSxmlFiles.Add(executedSxmlFile);
                        }

                        reader.Close();
                        connection.Close();
                    }
                    catch (Exception)
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                        connection.Close();
                    }
                }
                else
                {
                    string queryString = "SELECT * FROM [dbo].[DBScriptsHistory]";

                    SqlDataReader reader = null;
                    SqlConnection connection = new SqlConnection(connectionString);
                    SqlCommand command = new SqlCommand(queryString, connection);

                    try
                    {
                        connection.Open();
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ExecutedSxmlFile executedSxmlFile = new ExecutedSxmlFile
                            {
                                SxmlFileName = reader["SxmlFileName"].ToString(),
                                DBType = dbType
                            };
                            executedSxmlFiles.Add(executedSxmlFile);
                        }

                        reader.Close();
                        connection.Close();
                    }
                    catch (Exception)
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                        connection.Close();
                    }
                }
            }

            return executedSxmlFiles;
        }

        private void ValidateSXMLFilesNames(string[] sxmlFiles)
        {
            string error = null;

            List<string> duplicatedSxmlFiles = sxmlFiles.Select(s => Path.GetFileName(s)).ToList().GroupBy(s => s).SelectMany(g => g.Skip(1)).ToList();

            if (duplicatedSxmlFiles.Any())
            {
                error = "Error: Duplicate SXML Files:\n";
                foreach (var sxmlFile in sxmlFiles.Where(s => s.Contains(@"\" + duplicatedSxmlFiles.First())).ToList())
                {
                    error += sxmlFile + "\n";
                }
                error = error.TrimEnd('\n');
            }

            if(error != null)
            {
                ExitTool(error);
            }
        }

        private void ExitTool(string message)
        {
            Console.WriteLine(message);
            Environment.Exit(0);
        }
    }
}