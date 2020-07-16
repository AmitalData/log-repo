using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;

namespace Logitude.DBMigrations.Models
{
    public class MigrationTool
    {
        private readonly string DatabaseType;
        private readonly string[] Arguments;
        private readonly string ScriptSemicolonCode = "|(;)|";
        private readonly RunSettings RunSettings;

        private string MissingIndexesWarnings = "";
        private List<TableDefinition> DXMLTablesDefinitions;
        private List<DXMLHash> DXMLHashes;
        private List<ExecutedSxmlFile> ExecutedSxmlFiles;
        private IncludedModules IncludedModules;

        public MigrationTool(string[] arguments, RunSettings runSettings)
        {
            Arguments = arguments;
            RunSettings = runSettings;

            ValidateToolVersion();
            ValidateToolSettings();
            DisplayToolSettings();

            DatabaseType = ConfigurationManager.AppSettings["DatabaseType"];
        }

        public void RunTool()
        {
            if (IsArgumentProvided(ToolArguments.ROOT) || RunSettings.DebugMode)
            {
                string root = !RunSettings.DebugMode ? GetRoot() : RunSettings.Root;

                if (!String.IsNullOrEmpty(root))
                {
                    string[] dxmlFiles = GetDXMLFilesFromRoot(root);
                    string[] sxmlFiles = GetSXMLFilesFromRoot(root);

                    ValidateDBFiles(dxmlFiles, sxmlFiles);
                    PrepareRequiredData();

                    GeneratedScript scriptsToSave = GenerateAndExecuteDBScripts(dxmlFiles, sxmlFiles);
                    SaveScripts(scriptsToSave);
                    ExportMissingIndexesWarnings();
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

        private void ValidateDBFiles(string[] dxmlFiles, string[] sxmlFiles)
        {
            if (!(RunSettings.DebugMode && !RunSettings.ValidateFiles))
            {
                ValidateDXMLFiles(dxmlFiles);
                ValidateSXMLFiles(sxmlFiles);
            }
        }

        private GeneratedScript GenerateAndExecuteDBScripts(string[] dxmlFiles, string[] sxmlFiles)
        {
            List<string> toolDxmlFilesNames = GetToolDxmlFilesNames();
            string[] toolDxmlFiles = dxmlFiles?.Where(d => toolDxmlFilesNames.Contains(Path.GetFileName(d).ToLower())).ToArray();
            string[] migrationDxmlFiles = dxmlFiles?.Where(d => !toolDxmlFilesNames.Contains(Path.GetFileName(d).ToLower())).ToArray();
            bool isExecuteArgumentProvided = IsArgumentProvided(ToolArguments.EXE) || (RunSettings.DebugMode && RunSettings.ExecuteScripts);

            GeneratedScript toolTablesScript = HandleDXMLFiles(toolDxmlFiles, isExecuteArgumentProvided);
            GeneratedScript preGeneralScript = HandleSXMLFiles(sxmlFiles, isExecuteArgumentProvided, true);
            GeneratedScript migrationsScript = HandleDXMLFiles(migrationDxmlFiles, isExecuteArgumentProvided);
            GeneratedScript postGeneralScript = HandleSXMLFiles(sxmlFiles, isExecuteArgumentProvided, false);
            GeneratedScript scriptsToSave = GetScriptsToSave(toolTablesScript, preGeneralScript, migrationsScript, postGeneralScript);

            return scriptsToSave;
        }

        private GeneratedScript HandleDXMLFiles(string[] dxmlFiles, bool execute)
        {
            GeneratedScript generatedScript = null;

            if (dxmlFiles != null)
            {
                generatedScript = GenerateScriptsFromDXMLFiles(dxmlFiles);
                if (execute)
                {
                    ExecuteGeneratedScript(generatedScript);
                    SaveDXMLHashesOnDB(dxmlFiles);
                }
            }

            return generatedScript;
        }

        private GeneratedScript HandleSXMLFiles(string[] sxmlFiles, bool execute, bool pre)
        {
            GeneratedScript generatedScript = null;

            if (sxmlFiles != null)
            {
                generatedScript = GetGeneralScripts(sxmlFiles, pre);
                if (execute)
                {
                    ExecuteGeneralScripts(sxmlFiles, pre);
                }
            }

            return generatedScript;
        }

        private void PrepareRequiredData()
        {
            Console.WriteLine("Preparing Required Data ...");

            GetIncludedModulesFromDB();
            GetDXMLHashesFromDB();
            GetExecutedSXMLFilesFromDB();
        }

        private GeneratedScript GetScriptsToSave(GeneratedScript toolTablesScript, GeneratedScript preGeneralScript, GeneratedScript migrationsScript, GeneratedScript postGeneralScript)
        {
            GeneratedScript scriptsToSave = new GeneratedScript();

            if (toolTablesScript != null)
            {
                scriptsToSave.GlobalScript += toolTablesScript.GlobalScript;
                scriptsToSave.MainScript += toolTablesScript.MainScript;
                scriptsToSave.SystemLogsScript += toolTablesScript.SystemLogsScript;
            }

            if (preGeneralScript != null)
            {
                scriptsToSave.GlobalScript += preGeneralScript.GlobalScript;
                scriptsToSave.MainScript += preGeneralScript.MainScript;
                scriptsToSave.SystemLogsScript += preGeneralScript.SystemLogsScript;
            }

            if (migrationsScript != null)
            {
                scriptsToSave.GlobalScript += migrationsScript.GlobalScript;
                scriptsToSave.MainScript += migrationsScript.MainScript;
                scriptsToSave.SystemLogsScript += migrationsScript.SystemLogsScript;
            }

            if (postGeneralScript != null)
            {
                scriptsToSave.GlobalScript += postGeneralScript.GlobalScript;
                scriptsToSave.MainScript += postGeneralScript.MainScript;
                scriptsToSave.SystemLogsScript += postGeneralScript.SystemLogsScript;
            }

            return scriptsToSave;
        }

        private string[] GetDXMLFilesFromRoot(string root)
        {
            Console.WriteLine("Reading DXML Files From Root ...");

            try
            {
                string dxmlFilesRoot = Path.Combine(root);
                string[] dxmlFiles = Directory.GetFiles(dxmlFilesRoot, "*.dxml", SearchOption.AllDirectories);

                if (dxmlFiles.Length > 0)
                {
                    if (RunSettings.DebugMode && !String.IsNullOrEmpty(RunSettings.SpecificDxmlFile))
                    {
                        return dxmlFiles.Where(d => d.ToLower().Contains(@"\" + RunSettings.SpecificDxmlFile.ToLower())).ToArray();
                    }
                    return SortDXMLFiles(dxmlFiles);
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
                    if (RunSettings.DebugMode && !String.IsNullOrEmpty(RunSettings.SpecificSxmlFile))
                    {
                        return sxmlFiles.Where(s => s.ToLower().Contains(@"\" + RunSettings.SpecificSxmlFile.ToLower())).ToArray();
                    }
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

            DXMLTablesDefinitions = GetDXMLTablesDefinitions(dxmlFiles);
            DXMLDefinitions dxmlDefinitions = GetDXMLDefinitions(dxmlFiles);
            dxmlDefinitions = FilterDXMLDefinitions(dxmlDefinitions);

            List<DXMLTable> dxmlTables = dxmlDefinitions.DXMLTables;
            List<DXMLView> dxmlViews = dxmlDefinitions.DXMLViews;
            List<DXMLProcedure> dxmlProcedures = dxmlDefinitions.DXMLProcedures;
            List<DXMLTrigger> dxmlTriggers = dxmlDefinitions.DXMLTriggers;

            DXMLGeneratedScript dxmlGeneratedScript = GenerateScriptsFromDXMLTables(dxmlTables);
            generatedScript = AddToGeneratedScript(generatedScript, dxmlGeneratedScript.GeneratedScript);
            generatedScript = AddToGeneratedScript(generatedScript, dxmlGeneratedScript.RelationsScript);

            GeneratedScript generatedScriptFromDXMLViews = GenerateScriptsFromDXMLViews(dxmlViews);
            generatedScript = AddToGeneratedScript(generatedScript, generatedScriptFromDXMLViews);

            GeneratedScript generatedScriptFromDXMLProcedures = GenerateScriptsFromDXMLProcedures(dxmlProcedures);
            generatedScript = AddToGeneratedScript(generatedScript, generatedScriptFromDXMLProcedures);

            GeneratedScript generatedScriptFromDXMLTriggers = GenerateScriptsFromDXMLTriggers(dxmlTriggers);
            generatedScript = AddToGeneratedScript(generatedScript, generatedScriptFromDXMLTriggers);

            return generatedScript;
        }

        private DXMLGeneratedScript GenerateScriptsFromDXMLTables(List<DXMLTable> dxmlTables)
        {
            DXMLGeneratedScript dxmlsGeneratedScript = CreateNewDXMLGeneratedScript();

            foreach (var dxmlTable in dxmlTables)
            {
                Console.WriteLine("Generating Script For " + dxmlTable.DXMLFileName + " ...");

                if (IsDXMLFileForHistoryTable(dxmlTable.DXMLFileName))
                {
                    string[] dbTypes = new string[] { "Global", "Main", "SystemLogs" };
                    foreach (var dbType in dbTypes)
                    {
                        dxmlTable.TableDefinition.DBType = dbType;
                        DXMLGeneratedScript dxmlGeneratedScript = GenerateScriptsFromDXMLTable(dxmlTable);
                        dxmlsGeneratedScript = AddToDXMLGeneratedScript(dxmlsGeneratedScript, dxmlGeneratedScript);
                    }
                }
                else
                {
                    DXMLGeneratedScript dxmlGeneratedScript = GenerateScriptsFromDXMLTable(dxmlTable);
                    dxmlsGeneratedScript = AddToDXMLGeneratedScript(dxmlsGeneratedScript, dxmlGeneratedScript);
                }
            }

            return dxmlsGeneratedScript;
        }

        private DXMLGeneratedScript GenerateScriptsFromDXMLTable(DXMLTable dxmlTable)
        {
            DXMLGeneratedScript dxmlGeneratedScript = CreateNewDXMLGeneratedScript();

            DatabaseMigrations databaseMigrations = CreateDatabaseMigrations(dxmlTable.TableDefinition, dxmlTable.DXMLFileName);
            DatabaseMigrationsResult databaseMigrationsResult = GetDatabaseMigrationsResult(databaseMigrations);

            string generatedScripts = databaseMigrationsResult.MigrationsScript + databaseMigrationsResult.IndexesScript + databaseMigrationsResult.UniqueConstraintsScript;
            dxmlGeneratedScript.GeneratedScript = AppendToGeneratedScript(dxmlGeneratedScript.GeneratedScript, dxmlTable.TableDefinition.DBType, generatedScripts);
            dxmlGeneratedScript.RelationsScript = AppendToGeneratedScript(dxmlGeneratedScript.RelationsScript, dxmlTable.TableDefinition.DBType, databaseMigrationsResult.RelationsScript);
            MissingIndexesWarnings += databaseMigrationsResult.MissingIndexesWarnings;

            return dxmlGeneratedScript;
        }

        private DXMLGeneratedScript AddToDXMLGeneratedScript(DXMLGeneratedScript targetDxmlGeneratedScript, DXMLGeneratedScript sourceDxmlGeneratedScript)
        {
            targetDxmlGeneratedScript.GeneratedScript = AddToGeneratedScript(targetDxmlGeneratedScript.GeneratedScript, sourceDxmlGeneratedScript.GeneratedScript);
            targetDxmlGeneratedScript.RelationsScript = AddToGeneratedScript(targetDxmlGeneratedScript.RelationsScript, sourceDxmlGeneratedScript.RelationsScript);
            return targetDxmlGeneratedScript;
        }

        private DXMLGeneratedScript CreateNewDXMLGeneratedScript()
        {
            DXMLGeneratedScript newDxmlGeneratedScript = new DXMLGeneratedScript
            {
                GeneratedScript = new GeneratedScript(),
                RelationsScript = new GeneratedScript()
            };
            return newDxmlGeneratedScript;
        }

        private DatabaseMigrationsResult GetDatabaseMigrationsResult(DatabaseMigrations databaseMigrations)
        {
            string migrationsScript = databaseMigrations.GetScript();
            string relationsScript = databaseMigrations.GetRelationsScript();
            string indexesScript = databaseMigrations.GetIndexesScript();
            string missingIndexesWarnings = databaseMigrations.GetMissingIndexesWarnings();
            string uniqueConstraintsScript = databaseMigrations.GetUniqueConstraintsScript();
            
            return new DatabaseMigrationsResult
            {
                MigrationsScript = migrationsScript,
                RelationsScript = relationsScript,
                IndexesScript = indexesScript,
                MissingIndexesWarnings = missingIndexesWarnings,
                UniqueConstraintsScript = uniqueConstraintsScript
            };
        }

        private GeneratedScript GenerateScriptsFromDXMLViews(List<DXMLView> dxmlViews)
        {
            GeneratedScript generatedScript = new GeneratedScript();

            foreach (var dxmlView in dxmlViews)
            {
                Console.WriteLine("Generating Script For " + dxmlView.DXMLFileName + " ...");

                string viewScript = GetScriptFromViewDefinition(dxmlView.ViewDefinition, dxmlView.DXMLFileName);

                generatedScript = AppendToGeneratedScript(generatedScript, dxmlView.ViewDefinition.DBType, ReplaceScriptSemicolon(viewScript));
            }

            return generatedScript;
        }

        private GeneratedScript GenerateScriptsFromDXMLProcedures(List<DXMLProcedure> dxmlProcedures)
        {
            GeneratedScript generatedScript = new GeneratedScript();

            foreach (var dxmlProcedure in dxmlProcedures)
            {
                Console.WriteLine("Generating Script For " + dxmlProcedure.DXMLFileName + " ...");

                string procedureScript = GetScriptFromProcedureDefinition(dxmlProcedure.ProcedureDefinition, dxmlProcedure.DXMLFileName);

                generatedScript = AppendToGeneratedScript(generatedScript, dxmlProcedure.ProcedureDefinition.DBType, ReplaceScriptSemicolon(procedureScript));
            }

            return generatedScript;
        }

        private GeneratedScript GenerateScriptsFromDXMLTriggers(List<DXMLTrigger> dxmlTriggers)
        {
            GeneratedScript generatedScript = new GeneratedScript();

            foreach (var dxmlTrigger in dxmlTriggers)
            {
                Console.WriteLine("Generating Script For " + dxmlTrigger.DXMLFileName + " ...");

                string triggerScript = GetScriptFromTriggerDefinition(dxmlTrigger.TriggerDefinition, dxmlTrigger.DXMLFileName);

                generatedScript = AppendToGeneratedScript(generatedScript, dxmlTrigger.TriggerDefinition.DBType, ReplaceScriptSemicolon(triggerScript));
            }

            return generatedScript;
        }

        private void SaveScripts(GeneratedScript generatedScript)
        {
            Console.WriteLine("Saving The Generated Scripts ...");

            string globalScript = !String.IsNullOrEmpty(generatedScript.GlobalScript) ? generatedScript.GlobalScript.Replace(ScriptSemicolonCode, ";") : "";
            string mainScript = !String.IsNullOrEmpty(generatedScript.MainScript) ? generatedScript.MainScript.Replace(ScriptSemicolonCode, ";") : "";
            string systemLogsScript = !String.IsNullOrEmpty(generatedScript.SystemLogsScript) ? generatedScript.SystemLogsScript.Replace(ScriptSemicolonCode, ";") : "";

            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            if (IsArgumentProvided(ToolArguments.DEPLOYMENT))
            {
                projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }

            string generatedScriptDirectoryPath = Path.Combine(projectDirectory, @"GeneratedScript");
            string globalScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\GlobalScript.sql");
            string mainScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\MainScript.sql");
            string systemLogsScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\SystemLogsScript.sql");

            if (!Directory.Exists(generatedScriptDirectoryPath))
            {
                Directory.CreateDirectory(generatedScriptDirectoryPath);
            }

            File.WriteAllText(globalScriptFilePath, globalScript);
            File.WriteAllText(mainScriptFilePath, mainScript);
            File.WriteAllText(systemLogsScriptFilePath, systemLogsScript);

            if (IsGeneratedScriptsEmpty(generatedScript))
            {
                Console.WriteLine("There Are No Scripts Generated");
            }
            else
            {
                Console.WriteLine("The Generated Scripts Saved Successfully");
            }
        }

        private void ExecuteGeneratedScript(GeneratedScript generatedScript)
        {
            if (!IsGeneratedScriptsEmpty(generatedScript))
            {
                ExecuteScript(generatedScript.GlobalScript, "Global");
                ExecuteScript(generatedScript.MainScript, "Main");
                ExecuteScript(generatedScript.SystemLogsScript, "SystemLogs");
            }
        }
        
        private void ExecuteScript(string script, string dbType)
        {
            if (!String.IsNullOrEmpty(script))
            {
                Console.WriteLine("Executing Scripts On " + dbType + " Database ...\n");
                string result = ExecuteScriptOnDatabase(script, dbType);
                if (!String.IsNullOrEmpty(result))
                {
                    ExitTool(result);
                }
                else
                {
                    Console.WriteLine("Scripts Executed Successfully On " + dbType + " Database");
                }
            }
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
            if (dxmlFiles != null)
            {
                Console.WriteLine("Validating DXML Files ...");

                DXMLValidation dxmlValidation = new DXMLValidation(dxmlFiles);
                dxmlValidation.Validate();
            }
        }

        private void ValidateSXMLFiles(string[] sxmlFiles)
        {
            if (sxmlFiles != null)
            {
                Console.WriteLine("Validating SXML Files ...");

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

                if (error != null)
                {
                    ExitTool(error);
                }
            }
        }

        private void ExportMissingIndexesWarnings()
        {
            string missingIndexesWarningsToExport = !String.IsNullOrEmpty(MissingIndexesWarnings) ? MissingIndexesWarnings.TrimEnd('\n') : "";
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            if (IsArgumentProvided(ToolArguments.DEPLOYMENT))
            {
                projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }

            string warningsDirectoryPath = Path.Combine(projectDirectory, @"Warnings");
            string missingIndexesWarningsFilePath = Path.Combine(projectDirectory, @"Warnings\MissingIndexesWarnings.txt");

            if (!Directory.Exists(warningsDirectoryPath))
            {
                Directory.CreateDirectory(warningsDirectoryPath);
            }

            File.WriteAllText(missingIndexesWarningsFilePath, missingIndexesWarningsToExport);
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
            if (!String.IsNullOrEmpty(script))
            {
                if (dbType == "Global")
                {
                    generatedScript.GlobalScript += script + "\n";
                    return generatedScript;
                }
                else if (dbType == "Main")
                {
                    generatedScript.MainScript += script + "\n";
                    return generatedScript;
                }
                else if (dbType == "SystemLogs")
                {
                    generatedScript.SystemLogsScript += script + "\n";
                    return generatedScript;
                }
                else
                {
                    return generatedScript;
                }
            }

            return generatedScript;
        }

        private GeneratedScript AddToGeneratedScript(GeneratedScript targetGeneratedScript, GeneratedScript sourceGeneratedScript)
        {
            targetGeneratedScript.GlobalScript += sourceGeneratedScript.GlobalScript;
            targetGeneratedScript.MainScript += sourceGeneratedScript.MainScript;
            targetGeneratedScript.SystemLogsScript += sourceGeneratedScript.SystemLogsScript;
            return targetGeneratedScript;
        }

        private DatabaseMigrations CreateDatabaseMigrations(TableDefinition dxmlTableDefinition, string dxmlFileName)
        {
            string connectonString = GetConnectionString(dxmlTableDefinition.DBType);
            bool isBasicArgumentProvided = IsArgumentProvided(ToolArguments.BASIC);

            if (DatabaseType.ToLower() == "oracle")
            {
                DatabaseMigrations oracleDatabaseMigrations = new OracleDatabaseMigrations(dxmlTableDefinition, connectonString, DXMLTablesDefinitions, dxmlFileName, isBasicArgumentProvided);
                return oracleDatabaseMigrations;
            }

            DatabaseMigrations sqlDatabaseMigrations = new SQLDatabaseMigrations(dxmlTableDefinition, connectonString, DXMLTablesDefinitions, dxmlFileName, isBasicArgumentProvided);
            return sqlDatabaseMigrations;
        }

        private string ExecuteScriptOnDatabase(string script, string dbType)
        {
            string connectionString = GetConnectionString(dbType);

            if (DatabaseType.ToLower() == "oracle")
            {
                string currentCommandText = null;
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
                            currentCommandText = oracleCommand.CommandText;
                            PrintExecutingScript(currentCommandText);
                            oracleCommand.ExecuteNonQuery();
                        }
                    }

                    oracleConnection.Close();

                    return null;
                }
                catch (Exception exception)
                {
                    oracleConnection.Close();
                    return "Error: " + exception.Message + (!String.IsNullOrEmpty(currentCommandText) ? ("\nError While Executing:\n" + currentCommandText) : null);
                }
            }
            else
            {
                string currentCommandText = null;
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
                            sqlCommand.CommandTimeout = 3600;
                            currentCommandText = sqlCommand.CommandText;
                            PrintExecutingScript(currentCommandText);
                            sqlCommand.ExecuteNonQuery();
                        }
                    }

                    sqlConnection.Close();

                    return null;
                }
                catch (Exception exception)
                {
                    sqlConnection.Close();
                    return "Error: " + exception.Message + (!String.IsNullOrEmpty(currentCommandText) ? ("\nError While Executing:\n" + currentCommandText) : null);
                }
            }
        }

        private DXMLDefinitions GetDXMLDefinitions(string[] dxmlFiles)
        {
            List<DXMLTable> dxmlTables = new List<DXMLTable>();
            List<DXMLView> dxmlViews = new List<DXMLView>();
            List<DXMLProcedure> dxmlProcedures = new List<DXMLProcedure>();
            List<DXMLTrigger> dxmlTriggers = new List<DXMLTrigger>();
            bool checkDxmlHash = !RunSettings.DebugMode ? !IsArgumentProvided(ToolArguments.IGNOREHASH) : !RunSettings.IgnoreHash;

            foreach (var dxmlFile in dxmlFiles)
            {
                bool takeDxmlFile = true;

                string dxmlString = File.ReadAllText(dxmlFile);

                if (checkDxmlHash)
                {
                    DXMLHash dxmlHashFromDB = DXMLHashes.Where(d => d.FileName == Path.GetFileName(dxmlFile)).FirstOrDefault();
                    if (dxmlHashFromDB != null)
                    {
                        string dxmlHashStringFromFile = GenerateHashString(dxmlString);
                        string dxmlHashStringFromDB = dxmlHashFromDB.HashString;

                        if (dxmlHashStringFromFile == dxmlHashStringFromDB)
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
                    else if (dxmlString.EndsWith("</Trigger>"))
                    {
                        DXMLTrigger dxmlTrigger = CreateDXMLTrigger(dxmlString, dxmlFile);
                        if (dxmlTrigger == null)
                        {
                            ExitTool("Error: Cannot Create Trigger Definition For " + Path.GetFileName(dxmlFile));
                        }
                        dxmlTriggers.Add(dxmlTrigger);
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
                DXMLProcedures = dxmlProcedures,
                DXMLTriggers = dxmlTriggers
            };
        }

        private bool IsGeneratedScriptsEmpty(GeneratedScript generatedScript)
        {
            return String.IsNullOrEmpty(generatedScript.GlobalScript) && String.IsNullOrEmpty(generatedScript.MainScript) && String.IsNullOrEmpty(generatedScript.SystemLogsScript);
        }

        private string GetScriptFromViewDefinition(ViewDefinition viewDefinition, string dxmlFileName)
        {
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

        private string GetScriptFromTriggerDefinition(TriggerDefinition triggerDefinition, string dxmlFileName)
        {
            string triggerScript = "-- Trigger Script From " + dxmlFileName + "\n";

            if (DatabaseType.ToLower() == "oracle")
            {
                if (String.IsNullOrEmpty(triggerDefinition.OracleScript))
                {
                    return null;
                }

                string unescapedScript = UnescapeScript(triggerDefinition.OracleScript);
                triggerScript += unescapedScript + (unescapedScript.EndsWith(";") ? null : ";") + "\n\n";
                return triggerScript;
            }
            else
            {
                if (String.IsNullOrEmpty(triggerDefinition.SqlScript))
                {
                    return null;
                }

                triggerScript += "EXEC('IF (OBJECT_ID(''" + "[" + triggerDefinition.Schema + "].[" + triggerDefinition.Name + "]" + "'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER " + "[" + triggerDefinition.Schema + "].[" + triggerDefinition.Name + "]" + " END" + "');\n";
                triggerScript += "EXEC('" + UnescapeScript(triggerDefinition.SqlScript).Replace("'", "''") + "');" + "\n\n";
                return triggerScript;
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

        private DXMLTrigger CreateDXMLTrigger(string dxmlString, string dxmlFile)
        {
            try
            {
                string sqlScriptFromXmlString = dxmlString.Split(new string[] { "<SqlScript>" }, StringSplitOptions.None)[1].Split(new string[] { "</SqlScript>" }, StringSplitOptions.None)[0];
                string escapedSqlScript = SecurityElement.Escape(GetScriptFromCDataSection(sqlScriptFromXmlString)).Trim();

                string oracleScriptFromXmlString = dxmlString.Split(new string[] { "<OracleScript>" }, StringSplitOptions.None)[1].Split(new string[] { "</OracleScript>" }, StringSplitOptions.None)[0];
                string escapedOracleScript = SecurityElement.Escape(GetScriptFromCDataSection(oracleScriptFromXmlString)).Trim();

                dxmlString = dxmlString.Split(new string[] { "<SqlScript>" }, StringSplitOptions.None)[0] + "<SqlScript>" + escapedSqlScript + "</SqlScript>" + "<OracleScript>" + escapedOracleScript + "</OracleScript>" + "</Trigger>";

                TriggerDefinition dxmlTriggerDefinition = dxmlString.ParseXML<TriggerDefinition>();

                DXMLTrigger dxmlTrigger = new DXMLTrigger
                {
                    DXMLFileName = Path.GetFileName(dxmlFile),
                    TriggerDefinition = dxmlTriggerDefinition
                };

                return dxmlTrigger;
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
            if (String.IsNullOrEmpty(script))
            {
                return null;
            }

            script = string.Join("\n", script.Split('\n').Select(l => l.Trim()).Where(l => !String.IsNullOrEmpty(l)).ToArray());

            if (script.Contains("<![CDATA[") && script.Contains("]]>"))
            {
                return script.Split(new string[] { "<![CDATA[" }, StringSplitOptions.None)[1].Split(new string[] { "]]>" }, StringSplitOptions.None)[0];
            }

            return script;
        }

        private string ReplaceScriptSemicolon(string script)
        {
            if (String.IsNullOrEmpty(script))
            {
                return null;
            }

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

        private void GetDXMLHashesFromDB()
        {
            string connectionString = GetConnectionString("Main");

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
                catch (Exception)
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    connection.Close();
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
                catch (Exception)
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    connection.Close();
                }

                DXMLHashes = dxmlHashes;
            }
        }

        private void SaveDXMLHashesOnDB(string[] dxmlFiles)
        {
            string connectionString = GetConnectionString("Main");

            foreach (var dxmlFile in dxmlFiles)
            {
                bool saveDxmlHash = true;

                string dxmlFileName = Path.GetFileName(dxmlFile);
                string dxmlString = File.ReadAllText(dxmlFile);
                string dxmlHashStringFromFile = GenerateHashString(dxmlString);

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

        private string GenerateHashString(string anyString)
        {
            if (String.IsNullOrEmpty(anyString))
            {
                return "NULL";
            }

            MD5CryptoServiceProvider cryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] stringBytes = Encoding.UTF8.GetBytes(anyString);
            byte[] computedHash = cryptoServiceProvider.ComputeHash(stringBytes);
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
                    catch (Exception exception)
                    {
                        return "Error: " + exception.Message;
                    }

                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            SqlCommand sqlCommand = new SqlCommand(script, sqlConnection, sqlTransaction);
                            sqlCommand.CommandTimeout = 3600;
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

        private void ExecuteGeneralScripts(string[] sxmlFiles, bool preScripts)
        {
            foreach (var sxmlFile in sxmlFiles)
            {
                string sxmlFileName = Path.GetFileName(sxmlFile);
                ScriptDefinition scriptDefinition = GetScriptDefinition(sxmlFile);

                if (scriptDefinition != null)
                {
                    if (scriptDefinition.Pre == preScripts)
                    {
                        if (IncludeScriptDefinition(scriptDefinition.Module))
                        {
                            ExecuteSxmlFileResult executeSxmlFileResult = ShouldExecuteSxmlFile(sxmlFileName, scriptDefinition);

                            if (executeSxmlFileResult.ShouldExecute)
                            {
                                Console.WriteLine("Executing Script From " + sxmlFileName + " File ...");

                                string scriptBody;
                                if (DatabaseType.ToLower() == "oracle")
                                {
                                    string sxmlScript = GetScriptFromCDataSection(scriptDefinition.Oracle.Script);
                                    string saveScriptHistoryQuery = GetOracleSaveScriptHistoryQuery(executeSxmlFileResult.Action, sxmlFileName, scriptDefinition);

                                    if (!String.IsNullOrEmpty(sxmlScript))
                                    {
                                        scriptBody = "DECLARE\n" +
                                            "StartTime TIMESTAMP;\n" +
                                            "EndTime TIMESTAMP;\n" +
                                            "BEGIN\n" +
                                            "StartTime := SYSTIMESTAMP;\n" +
                                            "BEGIN\n" +
                                            sxmlScript + "\n" +
                                            "END;\n" +
                                            "EndTime:= SYSTIMESTAMP;\n" +
                                            "BEGIN\n" +
                                            "DECLARE ScriptBody NCLOB;\n" +
                                            "BEGIN\n" +
                                            "ScriptBody := '" + sxmlScript.Replace("'", "''").TrimEnd(new char[] { '\r', '\n' }) + "';\n" +
                                            saveScriptHistoryQuery + "\n" +
                                            "END;\n" +
                                            "END;\n" +
                                            "END;";
                                    }
                                    else
                                    {
                                        scriptBody = "DECLARE ScriptBody NCLOB;\n" +
                                            "BEGIN\n" +
                                            "ScriptBody := 'NULL';\n" +
                                            saveScriptHistoryQuery.Replace("EXTRACT(DAY FROM(EndTime - StartTime) * 24 * 60 * 60 * 1000)", "0") + "\n" +
                                            "END;";
                                    }
                                }
                                else
                                {
                                    string sxmlScript = GetScriptFromCDataSection(scriptDefinition.Sql.Script);
                                    scriptBody = "DECLARE @StartTime datetime\nDECLARE @EndTime datetime\nSELECT @StartTime = GETDATE()" +
                                        (String.IsNullOrEmpty(sxmlScript) ? null : "\n") + sxmlScript + "\nSELECT @EndTime = GETDATE()\n";
                                    scriptBody += GetSQLSaveScriptHistoryQuery(executeSxmlFileResult.Action, sxmlFileName, scriptDefinition, sxmlScript);
                                }

                                string result = ExecuteGeneralScript(scriptBody, scriptDefinition.DBType);
                                if (result != null)
                                {
                                    ExitTool(result);
                                }
                            }
                        }
                    }
                }
                else
                {
                    ExitTool("Error: Cannot Create Script Definition For " + sxmlFileName);
                }
            }
        }

        private GeneratedScript GetGeneralScripts(string[] sxmlFiles, bool preScripts)
        {
            GeneratedScript generalScripts = new GeneratedScript();

            foreach (var sxmlFile in sxmlFiles)
            {
                string sxmlFileName = Path.GetFileName(sxmlFile);
                ScriptDefinition scriptDefinition = GetScriptDefinition(sxmlFile);

                if (scriptDefinition != null)
                {
                    if (scriptDefinition.Pre == preScripts)
                    {
                        if (IncludeScriptDefinition(scriptDefinition.Module))
                        {
                            ExecuteSxmlFileResult executeSxmlFileResult = ShouldExecuteSxmlFile(sxmlFileName, scriptDefinition);
                            if (executeSxmlFileResult.ShouldExecute)
                            {
                                string scriptBody;
                                if (DatabaseType.ToLower() == "oracle")
                                {
                                    string sxmlScript = GetScriptFromCDataSection(scriptDefinition.Oracle.Script);
                                    string saveScriptHistoryQuery = GetOracleSaveScriptHistoryQuery(executeSxmlFileResult.Action, sxmlFileName, scriptDefinition);

                                    if (!String.IsNullOrEmpty(sxmlScript))
                                    {
                                        scriptBody = "DECLARE\n" +
                                            "StartTime TIMESTAMP;\n" +
                                            "EndTime TIMESTAMP;\n" +
                                            "BEGIN\n" +
                                            "SAVEPOINT ScriptSavePoint;\n" +
                                            "StartTime := SYSTIMESTAMP;\n" +
                                            "BEGIN\n" +
                                            sxmlScript + "\n" +
                                            "END;\n" +
                                            "EndTime:= SYSTIMESTAMP;\n" +
                                            "BEGIN\n" +
                                            "DECLARE ScriptBody NCLOB;\n" +
                                            "BEGIN\n" +
                                            "ScriptBody := '" + sxmlScript.Replace("'", "''").TrimEnd(new char[] { '\r', '\n' }) + "';\n" +
                                            saveScriptHistoryQuery + "\n" +
                                            "END;\n" +
                                            "END;\n" +
                                            "EXCEPTION\n" +
                                            "WHEN OTHERS THEN\n" +
                                            "ROLLBACK TO ScriptSavePoint;\n" +
                                            "COMMIT;\n" +
                                            "END;";
                                    }
                                    else
                                    {
                                        scriptBody = "DECLARE ScriptBody NCLOB;\n" +
                                            "BEGIN\n" +
                                            "ScriptBody := 'NULL';\n" +
                                            saveScriptHistoryQuery.Replace("EXTRACT(DAY FROM(EndTime - StartTime) * 24 * 60 * 60 * 1000)", "0") + "\n" +
                                            "END;";
                                    }
                                }
                                else
                                {
                                    string sxmlScript = GetScriptFromCDataSection(scriptDefinition.Sql.Script);
                                    scriptBody = "BEGIN TRAN\nBEGIN TRY\nDECLARE @StartTime datetime\nDECLARE @EndTime datetime\nSELECT @StartTime = GETDATE()" +
                                        (String.IsNullOrEmpty(sxmlScript) ? null : "\n") + sxmlScript +
                                        "\nSELECT @EndTime = GETDATE()\n";
                                    scriptBody += GetSQLSaveScriptHistoryQuery(executeSxmlFileResult.Action, sxmlFileName, scriptDefinition, sxmlScript);
                                    scriptBody += "COMMIT TRAN\nEND TRY\nBEGIN CATCH\nIF @@TRANCOUNT > 0\nROLLBACK TRAN\nEND CATCH;";
                                }

                                string scriptToAppend = "-- General Script From " + sxmlFileName + " File\n" + scriptBody + "\n";
                                generalScripts = AppendToGeneratedScript(generalScripts, scriptDefinition.DBType, scriptToAppend);
                            }
                        }
                    }
                }
                else
                {
                    ExitTool("Error: Cannot Create Script Definition For " + sxmlFileName);
                }
            }

            return generalScripts;
        }

        private ScriptDefinition GetScriptDefinition(string sxmlFile)
        {
            string sxmlString = File.ReadAllText(sxmlFile);
            try
            {
                ScriptDefinition ScriptDefinition = sxmlString.ParseXML<ScriptDefinition>();
                return ScriptDefinition;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool IncludeScriptDefinition(string scriptDefinitionModule)
        {
            bool includeScriptDefinition = true;
            if (IncludedModules != null)
            {
                if (IncludedModules.Include)
                {
                    includeScriptDefinition = IncludedModules.Modules.Contains(scriptDefinitionModule?.ToLower()) && !String.IsNullOrEmpty(scriptDefinitionModule);
                }
                else
                {
                    includeScriptDefinition = !IncludedModules.Modules.Contains(scriptDefinitionModule?.ToLower()) && !String.IsNullOrEmpty(scriptDefinitionModule);
                }
            }
            return includeScriptDefinition;
        }

        private string GetOracleSaveScriptHistoryQuery(string saveAction, string sxmlFileName, ScriptDefinition scriptDefinition)
        {
            string saveScriptHistoryQuery;
            if (saveAction == "Insert")
            {
                saveScriptHistoryQuery = "INSERT INTO \"DBSCRIPTSHISTORY\"(\"SXMLFILENAME\", \"EXECUTIONDATE\", \"SCRIPTBODY\", \"ELAPSEDTIMEINMS\", \"HASHVALUE\", \"VERSION\")" +
                    "VALUES('" + sxmlFileName + "', SYSDATE, ScriptBody, EXTRACT(DAY FROM(EndTime - StartTime) * 24 * 60 * 60 * 1000), '" +
                    GetScriptHashValue(scriptDefinition) + "', " + GetScriptVersion(scriptDefinition) + ");";
            }
            else
            {
                saveScriptHistoryQuery = "UPDATE \"DBSCRIPTSHISTORY\" SET \"EXECUTIONDATE\" = SYSDATE, \"SCRIPTBODY\" = ScriptBody, \"ELAPSEDTIMEINMS\" = " +
                    "EXTRACT(DAY FROM(EndTime - StartTime) * 24 * 60 * 60 * 1000), \"HASHVALUE\" = '" +
                    GetScriptHashValue(scriptDefinition) + "', \"VERSION\" = " +
                    GetScriptVersion(scriptDefinition) + " WHERE \"SXMLFILENAME\" = '" + sxmlFileName + "';";
            }
            return saveScriptHistoryQuery;
        }

        private string GetSQLSaveScriptHistoryQuery(string saveAction, string sxmlFileName, ScriptDefinition scriptDefinition, string sxmlScript)
        {
            string saveScriptHistoryQuery;
            if (saveAction == "Insert")
            {
                saveScriptHistoryQuery = "INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])" +
                    "VALUES('" + sxmlFileName + "', GETDATE(), '" + (!String.IsNullOrEmpty(sxmlScript) ? sxmlScript.Replace("'", "''").TrimEnd(new char[] { '\r', '\n' }) : "NULL") +
                    "', DATEDIFF(MS,@StartTime,@EndTime), '" + GetScriptHashValue(scriptDefinition) + "', " + GetScriptVersion(scriptDefinition) + ");\n";
            }
            else
            {
                saveScriptHistoryQuery = "UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = '" +
                    (!String.IsNullOrEmpty(sxmlScript) ? sxmlScript.Replace("'", "''").TrimEnd(new char[] { '\r', '\n' }) : "NULL") +
                    "', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = '" + GetScriptHashValue(scriptDefinition) + "', [Version] = " + GetScriptVersion(scriptDefinition) +
                    " WHERE [SxmlFileName] = '" + sxmlFileName + "';\n";
            }
            return saveScriptHistoryQuery;
        }

        private void GetExecutedSXMLFilesFromDB()
        {
            List<ExecutedSxmlFile> executedSxmlFiles = new List<ExecutedSxmlFile>();

            string[] dbTypes = new string[] { "Global", "Main", "SystemLogs" };

            foreach (var dbType in dbTypes)
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
                                DBType = dbType,
                                HashValue = reader["HASHVALUE"].ToString(),
                                Version = Convert.ToInt32(reader["VERSION"].ToString()),
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
                                DBType = dbType,
                                HashValue = reader["HashValue"].ToString(),
                                Version = Convert.ToInt32(reader["Version"].ToString()),
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

            ExecutedSxmlFiles = executedSxmlFiles;
        }

        private ExecuteSxmlFileResult ShouldExecuteSxmlFile(string sxmlFileName, ScriptDefinition scriptDefinition)
        {
            if (!ExecutedSxmlFiles.Where(e => e.SxmlFileName.ToLower() == sxmlFileName.ToLower() && e.DBType.ToLower() == scriptDefinition.DBType.ToLower()).Any())
            {
                return new ExecuteSxmlFileResult
                {
                    ShouldExecute = true,
                    Action = "Insert"
                };
            }
            else
            {
                ExecutedSxmlFile executedSxmlFile = ExecutedSxmlFiles.Where(e => e.SxmlFileName.ToLower() == sxmlFileName.ToLower() && e.DBType.ToLower() == scriptDefinition.DBType.ToLower()).First();
                string executedSxmlFileHashValue = executedSxmlFile.HashValue;
                int executedSxmlFileVersion = executedSxmlFile.Version;
                string sxmlFileHashValue = GetScriptHashValue(scriptDefinition);
                int sxmlFileVersion = GetScriptVersion(scriptDefinition);

                if (executedSxmlFileHashValue != sxmlFileHashValue && sxmlFileVersion <= executedSxmlFileVersion)
                {
                    ExitTool("Error: The Script Inside " + sxmlFileName + " File Has Been Changed, If You Are Sure You Want To Continue Executing The Script, You Should Change The Script Version");
                }

                if (executedSxmlFileHashValue != sxmlFileHashValue && sxmlFileVersion > executedSxmlFileVersion)
                {
                    return new ExecuteSxmlFileResult
                    {
                        ShouldExecute = true,
                        Action = "Update"
                    };
                }
                else
                {
                    return new ExecuteSxmlFileResult
                    {
                        ShouldExecute = false,
                        Action = null
                    };
                }
            }
        }

        private string GetScriptHashValue(ScriptDefinition scriptDefinition)
        {
            if (DatabaseType.ToLower() == "oracle")
            {
                return GenerateHashString(scriptDefinition.Oracle.Script);
            }
            else
            {
                return GenerateHashString(scriptDefinition.Sql.Script);
            }
        }

        private int GetScriptVersion(ScriptDefinition scriptDefinition)
        {
            if (DatabaseType.ToLower() == "oracle")
            {
                return scriptDefinition.Oracle.Version;
            }
            else
            {
                return scriptDefinition.Sql.Version;
            }
        }

        private void GetIncludedModulesFromDB()
        {
            string connectionString = GetConnectionString("Main");

            if (DatabaseType.ToLower() == "oracle")
            {
                string queryString = "SELECT * FROM \"DBMIGRATIONSETTINGS\"";

                OracleDataReader reader = null;
                OracleConnection connection = new OracleConnection(connectionString);
                OracleCommand command = new OracleCommand(queryString, connection);

                IncludedModules includedModules = null;

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    reader.Read();

                    if (reader.HasRows)
                    {
                        includedModules = new IncludedModules
                        {
                            Include = reader["MODE"].ToString().ToLower() == "include",
                            Modules = reader["MODULESLIST"].ToString().ToLower().Split(',').ToList()
                        };
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

                IncludedModules = includedModules;
            }
            else
            {
                string queryString = "SELECT * FROM [dbo].[DBMigrationSettings]";

                SqlDataReader reader = null;
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(queryString, connection);

                IncludedModules includedModules = null;

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    reader.Read();

                    if (reader.HasRows)
                    {
                        includedModules = new IncludedModules
                        {
                            Include = reader["Mode"].ToString().ToLower() == "include",
                            Modules = reader["ModulesList"].ToString().ToLower().Split(',').ToList()
                        };
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

                IncludedModules = includedModules;
            }
        }

        private DXMLDefinitions FilterDXMLDefinitions(DXMLDefinitions dxmlDefinitions)
        {
            if (IncludedModules != null)
            {
                if (IncludedModules.Include)
                {
                    dxmlDefinitions.DXMLTables = dxmlDefinitions.DXMLTables.Where(d => IncludedModules.Modules.Contains(d.TableDefinition.Module?.ToLower()) && !String.IsNullOrEmpty(d.TableDefinition.Module)).ToList();
                    dxmlDefinitions.DXMLViews = dxmlDefinitions.DXMLViews.Where(d => IncludedModules.Modules.Contains(d.ViewDefinition.Module?.ToLower()) && !String.IsNullOrEmpty(d.ViewDefinition.Module)).ToList();
                    dxmlDefinitions.DXMLProcedures = dxmlDefinitions.DXMLProcedures.Where(d => IncludedModules.Modules.Contains(d.ProcedureDefinition.Module?.ToLower()) && !String.IsNullOrEmpty(d.ProcedureDefinition.Module)).ToList();
                    dxmlDefinitions.DXMLTriggers = dxmlDefinitions.DXMLTriggers.Where(d => IncludedModules.Modules.Contains(d.TriggerDefinition.Module?.ToLower()) && !String.IsNullOrEmpty(d.TriggerDefinition.Module)).ToList();
                    return dxmlDefinitions;
                }
                else
                {
                    dxmlDefinitions.DXMLTables = dxmlDefinitions.DXMLTables.Where(d => !IncludedModules.Modules.Contains(d.TableDefinition.Module?.ToLower()) && !String.IsNullOrEmpty(d.TableDefinition.Module)).ToList();
                    dxmlDefinitions.DXMLViews = dxmlDefinitions.DXMLViews.Where(d => !IncludedModules.Modules.Contains(d.ViewDefinition.Module?.ToLower()) && !String.IsNullOrEmpty(d.ViewDefinition.Module)).ToList();
                    dxmlDefinitions.DXMLProcedures = dxmlDefinitions.DXMLProcedures.Where(d => !IncludedModules.Modules.Contains(d.ProcedureDefinition.Module?.ToLower()) && !String.IsNullOrEmpty(d.ProcedureDefinition.Module)).ToList();
                    dxmlDefinitions.DXMLTriggers = dxmlDefinitions.DXMLTriggers.Where(d => !IncludedModules.Modules.Contains(d.TriggerDefinition.Module?.ToLower()) && !String.IsNullOrEmpty(d.TriggerDefinition.Module)).ToList();
                    return dxmlDefinitions;
                }
            }
            else
            {
                return dxmlDefinitions;
            }
        }

        private List<string> GetToolDxmlFilesNames()
        {
            List<string> toolDxmlFilesNames = new List<string>
            {
                "DBMigrationsHistory.dxml".ToLower(),
                "DBScriptsHistory.dxml".ToLower(),
                "DXMLMigrationHashes.dxml".ToLower(),
                "DBMigrationSettings.dxml".ToLower()
            };

            return toolDxmlFilesNames;
        }

        private bool IsDXMLFileForHistoryTable(string dxmlFileName)
        {
            return (dxmlFileName.ToLower() == "DBMigrationsHistory.dxml".ToLower() || dxmlFileName.ToLower() == "DBScriptsHistory.dxml".ToLower());
        }

        private void ValidateToolVersion()
        {
            string versionInfoFilePath;

            if (IsArgumentProvided(ToolArguments.DEPLOYMENT))
            {
                string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
                versionInfoFilePath = Path.Combine(projectDirectory, @"VersionInfo.xml");
            }
            else
            {
                string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                versionInfoFilePath = Path.Combine(projectDirectory, @"Settings\VersionInfo.xml");
            }

            if (File.Exists(versionInfoFilePath))
            {
                try
                {
                    string versionInfoXmlString = File.ReadAllText(versionInfoFilePath);
                    VersionInfo versionInfo = versionInfoXmlString.ParseXML<VersionInfo>();
                    string toolVersion = GetAssemplyVersion();
                    if (toolVersion != versionInfo.Version)
                    {
                        ExitTool("Error: Invalid Tool Version, You Should Build The Tool After Get Latest Updates");
                    }
                }
                catch (Exception)
                {
                    ExitTool("Error: Cannot Read Version Info File");
                }
            }
            else
            {
                ExitTool("Error: Cannot Find File " + versionInfoFilePath);
            }
        }

        private void ValidateToolSettings()
        {
            string databaseType = ConfigurationManager.AppSettings["DatabaseType"];
            string globalConnectionString = ConfigurationManager.AppSettings["GlobalConnectionString"];
            string mainConnectionString = ConfigurationManager.AppSettings["MainConnectionString"];
            string systemLogsConnectionString = ConfigurationManager.AppSettings["SystemLogsConnectionString"];

            if (String.IsNullOrEmpty(databaseType))
            {
                ExitTool("Error: Cannot Find DatabaseType in Configuration File");
            }
            if (databaseType != "msql" && databaseType != "oracle")
            {
                ExitTool("Error: Invalid DatabaseType in Configuration File, DatabaseType should be msql or oracle");
            }
            if (String.IsNullOrEmpty(globalConnectionString))
            {
                ExitTool("Error: Cannot Find GlobalConnectionString in Configuration File");
            }
            if (String.IsNullOrEmpty(mainConnectionString))
            {
                ExitTool("Error: Cannot Find MainConnectionString in Configuration File");
            }
            if (String.IsNullOrEmpty(systemLogsConnectionString))
            {
                ExitTool("Error: Cannot Find SystemLogsConnectionString in Configuration File");
            }
        }

        private void DisplayToolSettings()
        {
            string databaseType = ConfigurationManager.AppSettings["DatabaseType"];
            string globalConnectionString = ConfigurationManager.AppSettings["GlobalConnectionString"];
            string mainConnectionString = ConfigurationManager.AppSettings["MainConnectionString"];
            string systemLogsConnectionString = ConfigurationManager.AppSettings["SystemLogsConnectionString"];
            string globalDB, globalSource, mainDB, mainSource, systemLogsDB, systemLogsSource, databaseTypeMessage, databaseNameMessage;

            if (databaseType.ToLower() == "oracle")
            {
                OracleConnectionStringBuilder globalConnectionStringBuilder = new OracleConnectionStringBuilder(globalConnectionString);
                OracleConnectionStringBuilder mainConnectionStringBuilder = new OracleConnectionStringBuilder(mainConnectionString);
                OracleConnectionStringBuilder systemLogsConnectionStringBuilder = new OracleConnectionStringBuilder(systemLogsConnectionString);
                globalDB = globalConnectionStringBuilder.UserID;
                globalSource = globalConnectionStringBuilder.DataSource;
                mainDB = mainConnectionStringBuilder.UserID;
                mainSource = mainConnectionStringBuilder.DataSource;
                systemLogsDB = systemLogsConnectionStringBuilder.UserID;
                systemLogsSource = systemLogsConnectionStringBuilder.DataSource;
                databaseTypeMessage = "Oracle";
                databaseNameMessage = "User ID";
            }
            else
            {
                SqlConnectionStringBuilder globalConnectionStringBuilder = new SqlConnectionStringBuilder(globalConnectionString);
                SqlConnectionStringBuilder mainConnectionStringBuilder = new SqlConnectionStringBuilder(mainConnectionString);
                SqlConnectionStringBuilder systemLogsConnectionStringBuilder = new SqlConnectionStringBuilder(systemLogsConnectionString);
                globalDB = globalConnectionStringBuilder.InitialCatalog;
                globalSource = globalConnectionStringBuilder.DataSource;
                mainDB = mainConnectionStringBuilder.InitialCatalog;
                mainSource = mainConnectionStringBuilder.DataSource;
                systemLogsDB = systemLogsConnectionStringBuilder.InitialCatalog;
                systemLogsSource = systemLogsConnectionStringBuilder.DataSource;
                databaseTypeMessage = "MSQL";
                databaseNameMessage = "Initial Catalog";
            }

            string appSettingsMessage = "Tool Database Settings\nDatabase Type: " + databaseTypeMessage + "\n" +
                                        "Applying Migrations On The Following Databases:\n" +
                                        "Global Database: " + databaseNameMessage + " = " + "\"" + globalDB + "\"" + " And Data Source = " + "\"" + globalSource + "\"" + "\n" +
                                        "Main Database: " + databaseNameMessage + " = " + "\"" + mainDB + "\"" + " And Data Source = " + "\"" + mainSource + "\"" + "\n" +
                                        "SystemLogs Database: " + databaseNameMessage + " = " + "\"" + systemLogsDB + "\"" + " And Data Source = " + "\"" + systemLogsSource + "\"" + "\n";

            Console.WriteLine(appSettingsMessage);

            if (!IsArgumentProvided(ToolArguments.IGNORESETTINGSCHECK))
            {
                Console.WriteLine("Are You Sure To Continue ? y/n");
                string userInput = Console.ReadLine().Trim().ToLower();
                if (userInput != "y")
                {
                    ExitTool("");
                }

                Console.Write("\n");
            }
        }

        private string GetAssemplyVersion()
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            CustomAttributeData AssemblyVersion = currentAssembly.CustomAttributes.Where(a => a.AttributeType.Name == "AssemblyFileVersionAttribute").FirstOrDefault();
            if (AssemblyVersion != null)
            {
                return (string)AssemblyVersion.ConstructorArguments[0].Value;
            }
            return "0.0";
        }

        private void PrintExecutingScript(string script)
        {
            if (!(script.ToLower().Contains("INSERT INTO".ToLower()) && script.ToLower().Contains("DBMigrationsHistory".ToLower())))
            {
                Console.WriteLine("Executing Script:\n" + script.TrimStart('\n').TrimEnd('\n') + "\n");
            }
        }

        private void ExitTool(string message)
        {
            Console.WriteLine(message);
            Environment.Exit(1);
        }
    }
}