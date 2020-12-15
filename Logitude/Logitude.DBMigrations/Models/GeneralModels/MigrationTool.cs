using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using System.Xml;
using Oracle.ManagedDataAccess.Client;

namespace Logitude.DBMigrations.Models
{
    public class MigrationTool
    {
        protected readonly string ScriptSemicolonCode = "|(;)|";
        protected readonly RunSettings RunSettings;

        protected string Root;
        protected string MissingIndexesWarnings = "";
        protected List<TableDefinition> DXMLTablesDefinitions;
        protected List<DXMLHash> DXMLHashes;
        protected List<ExecutedSxmlFile> ExecutedSxmlFiles;
        protected IncludedModules IncludedModules;
        protected Configurations Configurations;

        public MigrationTool(RunSettings runSettings)
        {
            RunSettings = runSettings;

            ValidateToolVersion();
            ReadConfigurations();
            SetToolConfigurations();
            ValidateToolArguments();
            ValidateToolSettings();
            ValidateAndReadRoot();
            DisplayToolSettings();
        }

        public void RunTool()
        {
            if (!ToolArguments.IsArgumentProvided(Arguments.SERVICE))
            {
                if (!ToolArguments.IsArgumentProvided(Arguments.DEV))
                {
                    StartNormalMigrations();
                    StartZeroDownTimeMigrations();
                }
                else
                {
                    ToolArguments.Arguments = ToolArguments.Arguments.Except(new string[] { Arguments.ZERODOWNTIME }).ToArray();
                    StartNormalMigrations();

                    Console.WriteLine("");

                    ToolArguments.Arguments = new List<string>(ToolArguments.Arguments) { Arguments.ZERODOWNTIME }.ToArray();
                    StartNormalMigrations();
                    StartZeroDownTimeMigrations();
                }
            }
            else
            {
                StartZeroDownTimeService();
            }
        }

        protected void StartNormalMigrations()
        {
            string[] dxmlFiles = GetDXMLFilesFromRoot(Root);
            string[] sxmlFiles = GetSXMLFilesFromRoot(Root);

            ValidateDBFiles(dxmlFiles, sxmlFiles);
            PrepareRequiredData();

            GeneratedScript scriptsToSave = GenerateAndExecuteDBScripts(dxmlFiles, sxmlFiles);
            SaveScripts(scriptsToSave);
            ExportMissingIndexesWarnings();
        }

        protected void ValidateDBFiles(string[] dxmlFiles, string[] sxmlFiles)
        {
            if (!(RunSettings.DebugMode && !RunSettings.ValidateFiles))
            {
                ValidateDXMLFiles(dxmlFiles);
                ValidateSXMLFiles(sxmlFiles);
            }
        }

        protected void PrepareRequiredData()
        {
            Console.WriteLine("Preparing Required Data ...");

            GetIncludedModulesFromArguments();
            GetIncludedModulesFromDB();
            GetDXMLHashesFromDB();
            GetExecutedSXMLFilesFromDB();
        }

        protected GeneratedScript GenerateAndExecuteDBScripts(string[] dxmlFiles, string[] sxmlFiles)
        {
            List<string> toolDxmlFilesNames = GetToolDxmlFilesNames();
            string[] toolDxmlFiles = dxmlFiles?.Where(d => toolDxmlFilesNames.Contains(Path.GetFileName(d).ToLower())).ToArray();
            string[] migrationDxmlFiles = dxmlFiles?.Where(d => !toolDxmlFilesNames.Contains(Path.GetFileName(d).ToLower())).ToArray();
            bool isExecuteArgumentProvided = ToolArguments.IsArgumentProvided(Arguments.EXE) || (RunSettings.DebugMode && RunSettings.ExecuteScripts);

            List<ScriptDefinition> scriptDefinitions = GetScriptDefinitionsFromSxmlFiles(sxmlFiles);
            ValidateNotExecutedAOTScripts(scriptDefinitions);

            GeneratedScript toolTablesScript = HandleDXMLFiles(toolDxmlFiles, isExecuteArgumentProvided);
            GeneratedScript preGeneralScript = HandleSXMLFiles(scriptDefinitions, isExecuteArgumentProvided, true);
            GeneratedScript migrationsScript = HandleDXMLFiles(migrationDxmlFiles, isExecuteArgumentProvided);
            GeneratedScript postGeneralScript = HandleSXMLFiles(scriptDefinitions, isExecuteArgumentProvided, false);
            GeneratedScript scriptsToSave = GetScriptsToSave(toolTablesScript, preGeneralScript, migrationsScript, postGeneralScript);

            return scriptsToSave;
        }

        protected GeneratedScript HandleDXMLFiles(string[] dxmlFiles, bool execute)
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
        
        protected GeneratedScript HandleSXMLFiles(List<ScriptDefinition> scriptDefinitions, bool execute, bool pre)
        {
            GeneratedScript generatedScript = null;

            if (scriptDefinitions.Count > 0)
            {
                generatedScript = GetGeneralScripts(scriptDefinitions, pre);
                bool isZeroDownTimeArgumentProvided = ToolArguments.IsArgumentProvided(Arguments.ZERODOWNTIME);
                if (execute && !isZeroDownTimeArgumentProvided)
                {
                    ExecuteGeneralScripts(scriptDefinitions, pre);
                }
            }

            return generatedScript;
        }

        protected GeneratedScript GetScriptsToSave(GeneratedScript toolTablesScript, GeneratedScript preGeneralScript, GeneratedScript migrationsScript, GeneratedScript postGeneralScript)
        {
            GeneratedScript scriptsToSave = new GeneratedScript();

            if (toolTablesScript != null)
            {
                scriptsToSave.GlobalScript += toolTablesScript.GlobalScript;
                scriptsToSave.MainScript += toolTablesScript.MainScript;
                scriptsToSave.SystemLogsScript += toolTablesScript.SystemLogsScript;
                scriptsToSave.CargoTrackingScript += toolTablesScript.CargoTrackingScript;
            }

            if (preGeneralScript != null)
            {
                scriptsToSave.GlobalScript += preGeneralScript.GlobalScript;
                scriptsToSave.MainScript += preGeneralScript.MainScript;
                scriptsToSave.SystemLogsScript += preGeneralScript.SystemLogsScript;
                scriptsToSave.CargoTrackingScript += preGeneralScript.CargoTrackingScript;
            }

            if (migrationsScript != null)
            {
                scriptsToSave.GlobalScript += migrationsScript.GlobalScript;
                scriptsToSave.MainScript += migrationsScript.MainScript;
                scriptsToSave.SystemLogsScript += migrationsScript.SystemLogsScript;
                scriptsToSave.CargoTrackingScript += migrationsScript.CargoTrackingScript;
            }

            if (postGeneralScript != null)
            {
                scriptsToSave.GlobalScript += postGeneralScript.GlobalScript;
                scriptsToSave.MainScript += postGeneralScript.MainScript;
                scriptsToSave.SystemLogsScript += postGeneralScript.SystemLogsScript;
                scriptsToSave.CargoTrackingScript += postGeneralScript.CargoTrackingScript;
            }

            return scriptsToSave;
        }

        protected string[] GetDXMLFilesFromRoot(string root)
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

        protected string[] GetSXMLFilesFromRoot(string root)
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

        protected GeneratedScript GenerateScriptsFromDXMLFiles(string[] dxmlFiles)
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

        protected DXMLGeneratedScript GenerateScriptsFromDXMLTables(List<DXMLTable> dxmlTables)
        {
            DXMLGeneratedScript dxmlsGeneratedScript = CreateNewDXMLGeneratedScript();

            foreach (var dxmlTable in dxmlTables)
            {
                Console.WriteLine("Generating Script For " + dxmlTable.DXMLFileName + " ...");

                if (IsDXMLFileForHistoryTable(dxmlTable.DXMLFileName))
                {
                    string[] dbTypes = new string[] { "Global", "Main", "SystemLogs", "CargoTracking" };
                    foreach (var dbType in dbTypes)
                    {
                        if(dbType == "CargoTracking" && !IsModuleIncluded("CargoTracking"))
                        {
                            continue;
                        }

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

        protected DXMLGeneratedScript GenerateScriptsFromDXMLTable(DXMLTable dxmlTable)
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

        protected DXMLGeneratedScript AddToDXMLGeneratedScript(DXMLGeneratedScript targetDxmlGeneratedScript, DXMLGeneratedScript sourceDxmlGeneratedScript)
        {
            targetDxmlGeneratedScript.GeneratedScript = AddToGeneratedScript(targetDxmlGeneratedScript.GeneratedScript, sourceDxmlGeneratedScript.GeneratedScript);
            targetDxmlGeneratedScript.RelationsScript = AddToGeneratedScript(targetDxmlGeneratedScript.RelationsScript, sourceDxmlGeneratedScript.RelationsScript);
            return targetDxmlGeneratedScript;
        }

        protected DXMLGeneratedScript CreateNewDXMLGeneratedScript()
        {
            DXMLGeneratedScript newDxmlGeneratedScript = new DXMLGeneratedScript
            {
                GeneratedScript = new GeneratedScript(),
                RelationsScript = new GeneratedScript()
            };
            return newDxmlGeneratedScript;
        }

        protected DatabaseMigrationsResult GetDatabaseMigrationsResult(DatabaseMigrations databaseMigrations)
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

        protected GeneratedScript GenerateScriptsFromDXMLViews(List<DXMLView> dxmlViews)
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

        protected GeneratedScript GenerateScriptsFromDXMLProcedures(List<DXMLProcedure> dxmlProcedures)
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

        protected GeneratedScript GenerateScriptsFromDXMLTriggers(List<DXMLTrigger> dxmlTriggers)
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

        protected void SaveScripts(GeneratedScript generatedScript)
        {
            Console.WriteLine("Saving The Generated Scripts ...");

            bool isScriptsArgumentProvided = ToolArguments.IsArgumentProvided(Arguments.SCRIPTS);

            string globalScript = isScriptsArgumentProvided ? (!String.IsNullOrEmpty(generatedScript.GlobalScript) ? generatedScript.GlobalScript.Replace(ScriptSemicolonCode, ";") : "") : null;
            string mainScript = isScriptsArgumentProvided ? (!String.IsNullOrEmpty(generatedScript.MainScript) ? generatedScript.MainScript.Replace(ScriptSemicolonCode, ";") : "") : null;
            string systemLogsScript = isScriptsArgumentProvided ? (!String.IsNullOrEmpty(generatedScript.SystemLogsScript) ? generatedScript.SystemLogsScript.Replace(ScriptSemicolonCode, ";") : "") : null;
            string cargoTrackingScript = isScriptsArgumentProvided ? (!String.IsNullOrEmpty(generatedScript.CargoTrackingScript) ? generatedScript.CargoTrackingScript.Replace(ScriptSemicolonCode, ";") : "") : null;

            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            if (ToolArguments.IsArgumentProvided(Arguments.DEPLOYMENT))
            {
                projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }

            string generatedScriptDirectoryPath = Path.Combine(projectDirectory, @"GeneratedScript");
            string globalScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\GlobalScript.sql");
            string mainScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\MainScript.sql");
            string systemLogsScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\SystemLogsScript.sql");
            string cargoTrackingScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\CargoTrackingScript.sql");

            if (!Directory.Exists(generatedScriptDirectoryPath))
            {
                Directory.CreateDirectory(generatedScriptDirectoryPath);
            }

            File.WriteAllText(globalScriptFilePath, globalScript);
            File.WriteAllText(mainScriptFilePath, mainScript);
            File.WriteAllText(systemLogsScriptFilePath, systemLogsScript);
            File.WriteAllText(cargoTrackingScriptFilePath, cargoTrackingScript);

            if (IsGeneratedScriptsEmpty(generatedScript))
            {
                Console.WriteLine("There Are No Scripts Generated");
            }
            else
            {
                Console.WriteLine("The Generated Scripts Saved Successfully");
            }
        }

        protected void ExecuteGeneratedScript(GeneratedScript generatedScript)
        {
            if (!IsGeneratedScriptsEmpty(generatedScript))
            {
                ExecuteScript(generatedScript.GlobalScript, "Global");
                ExecuteScript(generatedScript.MainScript, "Main");
                ExecuteScript(generatedScript.SystemLogsScript, "SystemLogs");
                ExecuteScript(generatedScript.CargoTrackingScript, "CargoTracking");
            }
        }
        
        protected void ExecuteScript(string script, string dbType)
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

        protected string GetRoot()
        {
            string[] arguments = Array.ConvertAll(ToolArguments.Arguments, a => a.ToLower());
            int indexOfRootArgument = Array.IndexOf(arguments, Arguments.ROOT) + 1;
            if (indexOfRootArgument < ToolArguments.Arguments.Length && indexOfRootArgument >= 0)
            {
                string root = ToolArguments.Arguments[indexOfRootArgument];
                return root;
            }
            else
            {
                return null;
            }
        }

        protected string GetModulesFromArguments(string modulesArgument)
        {
            string[] arguments = Array.ConvertAll(ToolArguments.Arguments, a => a.ToLower());
            int indexOfArgument = Array.IndexOf(arguments, modulesArgument) + 1;
            if (indexOfArgument < ToolArguments.Arguments.Length && indexOfArgument >= 0)
            {
                string root = ToolArguments.Arguments[indexOfArgument];
                return root;
            }
            else
            {
                return null;
            }
        }

        protected void ValidateDXMLFiles(string[] dxmlFiles)
        {
            if (dxmlFiles != null)
            {
                Console.WriteLine("Validating DXML Files ...");

                DXMLValidation dxmlValidation = new DXMLValidation(dxmlFiles);
                dxmlValidation.Validate();
            }
            else
            {
                ExitTool("Error: Cannot Find Any DXML Files Under The Provided Root Path");
            }
        }

        protected void ValidateSXMLFiles(string[] sxmlFiles)
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

        protected void ExportMissingIndexesWarnings()
        {
            string missingIndexesWarningsToExport = !String.IsNullOrEmpty(MissingIndexesWarnings) ? MissingIndexesWarnings.TrimEnd('\n') : "";
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            if (ToolArguments.IsArgumentProvided(Arguments.DEPLOYMENT))
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

        protected GeneratedScript AppendToGeneratedScript(GeneratedScript generatedScript, string dbType, string script)
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
                else if (dbType == "CargoTracking")
                {
                    generatedScript.CargoTrackingScript += script;
                    generatedScript.CargoTrackingScript += "\n";
                    return generatedScript;
                }
                else
                {
                    return generatedScript;
                }
            }

            return generatedScript;
        }

        protected GeneratedScript AddToGeneratedScript(GeneratedScript targetGeneratedScript, GeneratedScript sourceGeneratedScript)
        {
            targetGeneratedScript.GlobalScript += sourceGeneratedScript.GlobalScript;
            targetGeneratedScript.MainScript += sourceGeneratedScript.MainScript;
            targetGeneratedScript.SystemLogsScript += sourceGeneratedScript.SystemLogsScript;
            targetGeneratedScript.CargoTrackingScript += sourceGeneratedScript.CargoTrackingScript;
            return targetGeneratedScript;
        }

        protected DatabaseMigrations CreateDatabaseMigrations(TableDefinition dxmlTableDefinition, string dxmlFileName)
        {
            DatabaseMigrationSettings databaseMigrationSettings = new DatabaseMigrationSettings
            {
                DxmlFileName = dxmlFileName,
                DxmlTableDefinition = dxmlTableDefinition,
                DxmlTablesDefinitions = DXMLTablesDefinitions
            };

            if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
            {
                DatabaseMigrations oracleDatabaseMigrations = new OracleDatabaseMigrations(databaseMigrationSettings);
                return oracleDatabaseMigrations;
            }

            DatabaseMigrations sqlDatabaseMigrations = new SQLDatabaseMigrations(databaseMigrationSettings);
            return sqlDatabaseMigrations;
        }

        protected string ExecuteScriptOnDatabase(string script, string dbType)
        {
            string connectionString = ToolConfigurations.GetConnectionString(dbType);

            if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
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

        protected DXMLDefinitions GetDXMLDefinitions(string[] dxmlFiles)
        {
            List<DXMLTable> dxmlTables = new List<DXMLTable>();
            List<DXMLView> dxmlViews = new List<DXMLView>();
            List<DXMLProcedure> dxmlProcedures = new List<DXMLProcedure>();
            List<DXMLTrigger> dxmlTriggers = new List<DXMLTrigger>();
            bool checkDxmlHash = !RunSettings.DebugMode ? !ToolArguments.IsArgumentProvided(Arguments.IGNOREHASH) : !RunSettings.IgnoreHash;

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

        protected bool IsGeneratedScriptsEmpty(GeneratedScript generatedScript)
        {
            return String.IsNullOrEmpty(generatedScript.GlobalScript) && String.IsNullOrEmpty(generatedScript.MainScript) && String.IsNullOrEmpty(generatedScript.SystemLogsScript) && String.IsNullOrEmpty(generatedScript.CargoTrackingScript);
        }

        protected string GetScriptFromViewDefinition(ViewDefinition viewDefinition, string dxmlFileName)
        {
            string viewScript = "-- DataView Script From " + dxmlFileName + "\n";

            if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
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

        protected string GetScriptFromProcedureDefinition(ProcedureDefinition procedureDefinition, string dxmlFileName)
        {
            string procedureScript = "-- Procedure Script From " + dxmlFileName + "\n";

            if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
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

        protected string GetScriptFromTriggerDefinition(TriggerDefinition triggerDefinition, string dxmlFileName)
        {
            string triggerScript = "-- Trigger Script From " + dxmlFileName + "\n";

            if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
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

        protected DXMLTable CreateDXMLTable(string dxmlString, string dxmlFile)
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

        protected DXMLView CreateDXMLView(string dxmlString, string dxmlFile)
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

        protected DXMLProcedure CreateDXMLProcedure(string dxmlString, string dxmlFile)
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

        protected DXMLTrigger CreateDXMLTrigger(string dxmlString, string dxmlFile)
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

        protected string UnescapeScript(string escapedScript)
        {
            string unescapedScript = escapedScript;
            unescapedScript = unescapedScript.Replace("&apos;", "'");
            unescapedScript = unescapedScript.Replace("&quot;", "\"");
            unescapedScript = unescapedScript.Replace("&gt;", ">");
            unescapedScript = unescapedScript.Replace("&lt;", "<");
            unescapedScript = unescapedScript.Replace("&amp;", "&");
            return unescapedScript;
        }

        protected string GetScriptFromCDataSection(string script)
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

        protected string ReplaceScriptSemicolon(string script)
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

        protected List<TableDefinition> GetDXMLTablesDefinitions(string[] dxmlFiles)
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

        protected void GetDXMLHashesFromDB()
        {
            string connectionString = ToolConfigurations.GetConnectionString("Main");

            if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
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

        protected void SaveDXMLHashesOnDB(string[] dxmlFiles)
        {
            if (!ToolArguments.IsArgumentProvided(Arguments.ZERODOWNTIME))
            {
                string connectionString = ToolConfigurations.GetConnectionString("Main");

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
                        if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
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
        }

        protected string GenerateHashString(string anyString)
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

        protected string[] SortDXMLFiles(string[] dxmlFiles)
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

        protected string ExecuteGeneralScript(string script, string dbType)
        {
            string connectionString = ToolConfigurations.GetConnectionString(dbType);

            if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
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

        protected List<ScriptDefinition> GetScriptDefinitionsFromSxmlFiles(string[] sxmlFiles)
        {
            List<ScriptDefinition> scriptDefinitions = new List<ScriptDefinition>();

            foreach (var sxmlFile in sxmlFiles)
            {
                string sxmlFileName = Path.GetFileName(sxmlFile);
                ScriptDefinition scriptDefinition = GetScriptDefinition(sxmlFile);
                if (scriptDefinition != null)
                {
                    if (IncludeScriptDefinition(scriptDefinition.Module))
                    {
                        ExecuteSxmlFileResult executeSxmlFileResult = ShouldExecuteSxmlFile(sxmlFileName, scriptDefinition);
                        if (executeSxmlFileResult.ShouldExecute)
                        {
                            scriptDefinition.SxmlFileName = sxmlFileName;
                            scriptDefinition.ScriptHistoryAction = executeSxmlFileResult.Action;
                            scriptDefinitions.Add(scriptDefinition);
                        }
                    }
                }
                else
                {
                    ExitTool("Error: Cannot Create Script Definition For " + sxmlFileName);
                }
            }

            if(ToolConfigurations.DatabaseType.ToLower() == "oracle")
            {
                scriptDefinitions = scriptDefinitions.Where(s => !s.AOT).ToList();
            }

            return scriptDefinitions;
        }

        protected void ValidateNotExecutedAOTScripts(List<ScriptDefinition> scriptDefinitions)
        {
            if (!ToolArguments.IsArgumentProvided(Arguments.ZERODOWNTIME) && ToolArguments.IsArgumentProvided(Arguments.EXE))
            {
                List<ScriptDefinition> aotScripts = scriptDefinitions.Where(s => s.AOT).ToList();

                if (aotScripts.Any() && !ToolArguments.IsArgumentProvided(Arguments.DEV))
                {
                    string scriptsSxmlNames = string.Join("\n", aotScripts.Select(s => s.SxmlFileName).ToArray());
                    ExitTool("Error: There Is Some Not Executed Scripts That Defined As AOT And You Need To Run The Tool With -Dev Argument, The Scripts Are:\n" + scriptsSxmlNames);
                }
            }
        }

        protected GeneratedScript GetGeneralScripts(List<ScriptDefinition> scriptDefinitions, bool preScripts)
        {
            bool isZeroDownTimeArgumentProvided = ToolArguments.IsArgumentProvided(Arguments.ZERODOWNTIME);
            scriptDefinitions = scriptDefinitions.Where(s => s.Pre == preScripts && s.AOT == isZeroDownTimeArgumentProvided).ToList();

            GeneratedScript generalScripts = new GeneratedScript();

            foreach (var scriptDefinition in scriptDefinitions)
            {
                Console.WriteLine("Generating Script From File " + scriptDefinition.SxmlFileName + " ...");

                if (!isZeroDownTimeArgumentProvided)
                {
                    string scriptBody;
                    if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
                    {
                        string sxmlScript = GetScriptFromCDataSection(scriptDefinition.Oracle.Script);
                        string saveScriptHistoryQuery = GetOracleSaveScriptHistoryQuery(scriptDefinition.ScriptHistoryAction, scriptDefinition.SxmlFileName, scriptDefinition);

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
                        scriptBody = "BEGIN TRAN\n" +
                            "BEGIN TRY\n" +
                            "DECLARE @StartTime datetime\n" +
                            "DECLARE @EndTime datetime\n" +
                            "SELECT @StartTime = GETDATE()" +
                            (String.IsNullOrEmpty(sxmlScript) ? null : "\n") + sxmlScript + "\n" +
                            "SELECT @EndTime = GETDATE()\n";

                        scriptBody += GetSQLSaveScriptHistoryQuery(scriptDefinition.ScriptHistoryAction, scriptDefinition.SxmlFileName, scriptDefinition, sxmlScript);
                        scriptBody += "COMMIT TRAN\n" +
                            "END TRY\n" +
                            "BEGIN CATCH\n" +
                            "IF @@TRANCOUNT > 0\n" +
                            "ROLLBACK TRAN\n" +
                            "END CATCH;";
                    }

                    string scriptToAppend = "-- General Script From " + scriptDefinition.SxmlFileName + " File\n" + scriptBody + "\n";
                    generalScripts = AppendToGeneratedScript(generalScripts, scriptDefinition.DBType, scriptToAppend);
                }
                else
                {
                    if (!IsSxmlInDBMigrationsDataScripts(scriptDefinition.SxmlFileName))
                    {
                        InsertIntoDBMigrationsDataScripts(scriptDefinition);
                    }
                    else
                    {
                        UpdateDBMigrationsDataScripts(scriptDefinition);
                    }
                }
            }

            return generalScripts;
        }

        protected void ExecuteGeneralScripts(List<ScriptDefinition> scriptDefinitions, bool preScripts)
        {
            scriptDefinitions = scriptDefinitions.Where(s => s.Pre == preScripts && !s.AOT).ToList();

            foreach (var scriptDefinition in scriptDefinitions)
            {
                Console.WriteLine("Executing Script From File " + scriptDefinition.SxmlFileName + " ...");

                string scriptBody;
                if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
                {
                    string sxmlScript = GetScriptFromCDataSection(scriptDefinition.Oracle.Script);
                    string saveScriptHistoryQuery = GetOracleSaveScriptHistoryQuery(scriptDefinition.ScriptHistoryAction, scriptDefinition.SxmlFileName, scriptDefinition);

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

                    scriptBody = "DECLARE @StartTime datetime\n" +
                        "DECLARE @EndTime datetime\n" +
                        "SELECT @StartTime = GETDATE()" +
                        (String.IsNullOrEmpty(sxmlScript) ? null : "\n") + sxmlScript + "\n" +
                        "SELECT @EndTime = GETDATE()\n";

                    scriptBody += GetSQLSaveScriptHistoryQuery(scriptDefinition.ScriptHistoryAction, scriptDefinition.SxmlFileName, scriptDefinition, sxmlScript);
                }

                string result = ExecuteGeneralScript(scriptBody, scriptDefinition.DBType);
                if (result != null)
                {
                    ExitTool(result);
                }
            }
        }

        protected ScriptDefinition GetScriptDefinition(string sxmlFile)
        {
            string sxmlString = File.ReadAllText(sxmlFile);
            try
            {
                ScriptDefinition scriptDefinition = sxmlString.ParseXML<ScriptDefinition>();
                return scriptDefinition;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected bool IncludeScriptDefinition(string scriptDefinitionModule)
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

        protected bool IsModuleIncluded(string moduleName)
        {
            bool isModuleIncluded = true;
            if (IncludedModules != null)
            {
                if (IncludedModules.Include)
                {
                    isModuleIncluded = IncludedModules.Modules.Contains(moduleName?.ToLower()) && !String.IsNullOrEmpty(moduleName);
                }
                else
                {
                    isModuleIncluded = !IncludedModules.Modules.Contains(moduleName?.ToLower()) && !String.IsNullOrEmpty(moduleName);
                }
            }
            return isModuleIncluded;
        }

        protected string GetOracleSaveScriptHistoryQuery(string saveAction, string sxmlFileName, ScriptDefinition scriptDefinition)
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

        protected string GetSQLSaveScriptHistoryQuery(string saveAction, string sxmlFileName, ScriptDefinition scriptDefinition, string sxmlScript)
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

        protected void GetExecutedSXMLFilesFromDB()
        {
            List<ExecutedSxmlFile> executedSxmlFiles = new List<ExecutedSxmlFile>();

            string[] dbTypes = new string[] { "Global", "Main", "SystemLogs","CargoTracking" };

            foreach (var dbType in dbTypes)
            {
                string connectionString = ToolConfigurations.GetConnectionString(dbType);

                if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
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

        protected ExecuteSxmlFileResult ShouldExecuteSxmlFile(string sxmlFileName, ScriptDefinition scriptDefinition)
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

        protected string GetScriptHashValue(ScriptDefinition scriptDefinition)
        {
            if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
            {
                return GenerateHashString(scriptDefinition.Oracle.Script);
            }
            else
            {
                return GenerateHashString(scriptDefinition.Sql.Script);
            }
        }

        protected int GetScriptVersion(ScriptDefinition scriptDefinition)
        {
            if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
            {
                return scriptDefinition.Oracle.Version;
            }
            else
            {
                return scriptDefinition.Sql.Version;
            }
        }

        protected void GetIncludedModulesFromArguments()
        {
            bool isIncludeModulesArgumentProvided = ToolArguments.IsArgumentProvided(Arguments.INCLUDEMODULES);
            bool isExcludeModulesArgumentProvided = ToolArguments.IsArgumentProvided(Arguments.EXCLUDEMODULES);

            if (isIncludeModulesArgumentProvided || isExcludeModulesArgumentProvided)
            {
                string modules;
                string mode;
                if (isIncludeModulesArgumentProvided)
                {
                    modules = GetModulesFromArguments(Arguments.INCLUDEMODULES);
                    mode = "include";
                }
                else
                {
                    modules = GetModulesFromArguments(Arguments.EXCLUDEMODULES);
                    mode = "exclude";
                }

                if (!String.IsNullOrEmpty(modules))
                {
                    IncludedModules = new IncludedModules
                    {
                        Include = mode == "include",
                        Modules = modules.ToLower().Split(',').ToList()
                    };
                }
            }
        }

        protected void GetIncludedModulesFromDB()
        {
            if(IncludedModules == null)
            {
                string connectionString = ToolConfigurations.GetConnectionString("Main");

                if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
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
        }

        protected DXMLDefinitions FilterDXMLDefinitions(DXMLDefinitions dxmlDefinitions)
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

        protected List<string> GetToolDxmlFilesNames()
        {
            List<string> toolDxmlFilesNames = new List<string>
            {
                "DBMigrationsHistory.dxml".ToLower(),
                "DBScriptsHistory.dxml".ToLower(),
                "DXMLMigrationHashes.dxml".ToLower(),
                "DBMigrationSettings.dxml".ToLower(),

                "DBMigrationsSetDefaultValues.dxml".ToLower(),
                "DBMigrationsSetValueCounters.dxml".ToLower(),
                "DBMigrationsDataScripts.dxml".ToLower(),
                "DBMigrationsDataScriptCounters.dxml".ToLower()
            };

            return toolDxmlFilesNames;
        }

        protected bool IsDXMLFileForHistoryTable(string dxmlFileName)
        {
            return (dxmlFileName.ToLower() == "DBMigrationsHistory.dxml".ToLower() || dxmlFileName.ToLower() == "DBScriptsHistory.dxml".ToLower());
        }

        protected void ValidateToolVersion()
        {
            string versionInfoFilePath;

            if (ToolArguments.IsArgumentProvided(Arguments.DEPLOYMENT))
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

        protected void ValidateToolArguments()
        {
            if (ToolArguments.IsArgumentProvided(Arguments.ZERODOWNTIME) && !ToolArguments.IsArgumentProvided(Arguments.EXE))
            {
                ExitTool("Error: Cannot Use Zero Down Time Mode Without Execute Argument");
            }

            if (ToolArguments.IsArgumentProvided(Arguments.DEV) && !ToolArguments.IsArgumentProvided(Arguments.EXE))
            {
                ExitTool("Error: Cannot Use Dev Mode Without Execute Argument");
            }
        }

        protected void ValidateToolSettings()
        {
            string databaseType = ToolConfigurations.DatabaseType;
            string globalConnectionString = ToolConfigurations.GlobalConnectionString;
            string mainConnectionString = ToolConfigurations.MainConnectionString;
            string systemLogsConnectionString = ToolConfigurations.SystemLogsConnectionString;

            if (String.IsNullOrEmpty(databaseType))
            {
                ExitTool("Error: Cannot Find DatabaseType in Configuration File");
            }
            if (databaseType.ToLower() != "msql" && databaseType.ToLower() != "oracle")
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

            if (databaseType.ToLower() == "oracle" && (ToolArguments.IsArgumentProvided(Arguments.ZERODOWNTIME) || ToolArguments.IsArgumentProvided(Arguments.DEV)))
            {
                ExitTool("Error: Zero Down Time Mode For Oracle Not Ready To Use");
            }
        }

        protected void DisplayToolSettings()
        {
            string databaseType = ToolConfigurations.DatabaseType;
            string globalConnectionString = ToolConfigurations.GlobalConnectionString;
            string mainConnectionString = ToolConfigurations.MainConnectionString;
            string systemLogsConnectionString = ToolConfigurations.SystemLogsConnectionString;
            string cargoTrackingConnectionString = ToolConfigurations.CargoTrackingConnectionString;
            string globalDB, globalSource, mainDB, mainSource, systemLogsDB, systemLogsSource, databaseTypeMessage, databaseNameMessage;
            string cargoTrackingDB = null, cargoTrackingSource = null;

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
                if (!string.IsNullOrEmpty(cargoTrackingConnectionString))
                {
                    OracleConnectionStringBuilder cargoTrackingConnectionStringBuilder = new OracleConnectionStringBuilder(cargoTrackingConnectionString);
                    cargoTrackingDB = cargoTrackingConnectionStringBuilder.UserID;
                    cargoTrackingSource = cargoTrackingConnectionStringBuilder.DataSource;
                }
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
                if (!string.IsNullOrEmpty(cargoTrackingConnectionString))
                {
                    SqlConnectionStringBuilder cargoTrackingConnectionStringBuilder = new SqlConnectionStringBuilder(cargoTrackingConnectionString);
                    cargoTrackingDB = cargoTrackingConnectionStringBuilder.InitialCatalog;
                    cargoTrackingSource = cargoTrackingConnectionStringBuilder.DataSource;
                }
            }

            string appSettingsMessage = "Tool Database Settings\nDatabase Type: " + databaseTypeMessage + "\n" +
                                        "Applying Migrations On The Following Databases:\n" +
                                        "Global Database: " + databaseNameMessage + " = " + "\"" + globalDB + "\"" + " And Data Source = " + "\"" + globalSource + "\"" + "\n" +
                                        "Main Database: " + databaseNameMessage + " = " + "\"" + mainDB + "\"" + " And Data Source = " + "\"" + mainSource + "\"" + "\n" +
                                        "SystemLogs Database: " + databaseNameMessage + " = " + "\"" + systemLogsDB + "\"" + " And Data Source = " + "\"" + systemLogsSource + "\"" + "\n";

            if (!string.IsNullOrEmpty(cargoTrackingDB))
            {
                appSettingsMessage = appSettingsMessage + "CargoTracking Database: " + databaseNameMessage + " = " + "\"" + cargoTrackingDB + "\"" + " And Data Source = " + "\"" + cargoTrackingSource + "\"" + "\n";
            }



            Console.WriteLine(appSettingsMessage);

            if (!ToolArguments.IsArgumentProvided(Arguments.IGNORESETTINGSCHECK))
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

        protected void ValidateAndReadRoot()
        {
            if (!ToolArguments.IsArgumentProvided(Arguments.SERVICE))
            {
                if (ToolArguments.IsArgumentProvided(Arguments.ROOT) || RunSettings.DebugMode)
                {
                    string root = !RunSettings.DebugMode ? GetRoot() : RunSettings.Root;
                    if (String.IsNullOrEmpty(root))
                    {
                        ExitTool("Error: There Is No Root Found For Looking About Files");
                    }
                    else
                    {
                        if (!Directory.Exists(root))
                        {
                            ExitTool("Error: Cannot Find The Provided Root Path");
                        }
                        else
                        {
                            Root = root;
                        }
                    }
                }
                else
                {
                    ExitTool("Error: There Is No Root Found For Looking About Files");
                }
            }
        }

        protected string GetAssemplyVersion()
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            CustomAttributeData AssemblyVersion = currentAssembly.CustomAttributes.Where(a => a.AttributeType.Name == "AssemblyFileVersionAttribute").FirstOrDefault();
            if (AssemblyVersion != null)
            {
                return (string)AssemblyVersion.ConstructorArguments[0].Value;
            }
            return "0.0";
        }

        protected void PrintExecutingScript(string script)
        {
            if (!(script.ToLower().Contains("INSERT INTO".ToLower()) && script.ToLower().Contains("DBMigrationsHistory".ToLower())))
            {
                Console.WriteLine("Executing Script:\n" + script.TrimStart('\n').TrimEnd('\n') + "\n");
            }
        }

        protected void StartZeroDownTimeMigrations()
        {
            if (ToolArguments.IsArgumentProvided(Arguments.ZERODOWNTIME))
            {
                Console.WriteLine("\nZero Down Time Migrations Started");
                ZeroDownTimeMigrations zeroDownTimeMigrations  = CreateZeroDownTimeMigrations();
                zeroDownTimeMigrations.Start();
                Console.WriteLine("Zero Down Time Migrations Finished");
            }
        }

        protected void StartZeroDownTimeService()
        {
            Console.WriteLine("Zero Down Time Service Started");
            ZeroDownTimeMigrations zeroDownTimeMigrations = CreateZeroDownTimeMigrations();
            zeroDownTimeMigrations.StartAsService();
        }

        protected ZeroDownTimeMigrations CreateZeroDownTimeMigrations()
        {
            if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
            {
                return null;
            }

            ZeroDownTimeMigrations sqlZeroDownTimeMigrations = new SQLZeroDownTimeMigrations();
            return sqlZeroDownTimeMigrations;
        }

        protected bool IsSxmlInDBMigrationsDataScripts(string sxmlFileName)
        {
            bool result = false;

            if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
            {

            }
            else
            {
                string queryString = "SELECT * FROM [dbo].[DBMigrationsDataScripts] WHERE [SxmlFileName] = '" + sxmlFileName + "';";
                SqlDataReader reader = null;
                SqlConnection connection = new SqlConnection(ToolConfigurations.MainConnectionString);
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        result = true;
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
                    ExitTool(exception.Message);
                }
            }
            return result;
        }

        protected void InsertIntoDBMigrationsDataScripts(ScriptDefinition scriptDefinition)
        {
            UpdateDataScriptCounter(scriptDefinition.TargetTableName);
            int scriptExecutionNumber = GetDataScriptCounter(scriptDefinition.TargetTableName);
            
            if(ToolConfigurations.DatabaseType.ToLower() == "oracle")
            {

            }
            else
            {
                string sxmlScript = GetScriptFromCDataSection(scriptDefinition.Sql.Script).Replace("'", "''").TrimEnd(new char[] { '\r', '\n' });
                int sxmlVersion = scriptDefinition.Sql.Version;
                string sxmlScriptHashValue = GenerateHashString(scriptDefinition.Sql.Script);

                string queryString = "INSERT INTO [dbo].[DBMigrationsDataScripts]([Id], [SxmlFileName], [DatabaseType], [SxmlScript], [IsPreSxml], [Status], [ScriptExecutionNumber], " +
                    "[StartDate], [EndDate], [LastBatchElapsedTime], [ScriptVersion], [ScriptHashValue], [ScriptHistoryAction], [TargetTableName], [BatchSize]) " +
                    "VALUES('" + Guid.NewGuid().ToString() + "', '" + scriptDefinition.SxmlFileName + "', '" + scriptDefinition.DBType + "', '" + sxmlScript + "', " +
                    (scriptDefinition.Pre ? "1" : "0") + ", 'Waiting', " + scriptExecutionNumber + ", NULL, NULL, 0, " + sxmlVersion + ", '" + sxmlScriptHashValue + "', '" + scriptDefinition.ScriptHistoryAction + "', '" + scriptDefinition.TargetTableName + "', " + (scriptDefinition.BatchSize > 0 ? scriptDefinition.BatchSize.ToString() : "NULL") + ");";

                SqlConnection sqlConnection = new SqlConnection(ToolConfigurations.MainConnectionString);

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
                    ExitTool(exception.Message);
                }
            }
        }
        
        protected void UpdateDBMigrationsDataScripts(ScriptDefinition scriptDefinition)
        {
            UpdateDataScriptCounter(scriptDefinition.TargetTableName);
            int scriptExecutionNumber = GetDataScriptCounter(scriptDefinition.TargetTableName);

            if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
            {

            }
            else
            {
                string sxmlScript = GetScriptFromCDataSection(scriptDefinition.Sql.Script).Replace("'", "''").TrimEnd(new char[] { '\r', '\n' });
                int sxmlVersion = scriptDefinition.Sql.Version;
                string sxmlScriptHashValue = GenerateHashString(scriptDefinition.Sql.Script);

                string queryString = "UPDATE [dbo].[DBMigrationsDataScripts] SET [SxmlScript] = '" + sxmlScript + "', [Status] = 'Waiting', [ScriptExecutionNumber] = " + scriptExecutionNumber + ", [StartDate] = NULL, [EndDate] = NULL, " +
                    "[LastBatchElapsedTime] = 0, [ScriptVersion] = " + sxmlVersion + ", [ScriptHashValue] = '" + sxmlScriptHashValue + "', " +
                    "[ScriptHistoryAction] = '" + scriptDefinition.ScriptHistoryAction + "' WHERE [SxmlFileName] = '" + scriptDefinition.SxmlFileName + "'";

                SqlConnection sqlConnection = new SqlConnection(ToolConfigurations.MainConnectionString);

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
                    ExitTool(exception.Message);
                }
            }
        }

        protected void UpdateDataScriptCounter(string tableName)
        {
            if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
            {

            }
            else
            {
                string queryString = "EXEC('IF (SELECT COUNT(*) FROM [dbo].[DBMigrationsDataScriptCounters] WHERE [TableName] = ''" + tableName + "'') = 0 " +
                 "INSERT INTO [dbo].[DBMigrationsDataScriptCounters]([TableName], [LastCounter]) VALUES(''" + tableName + "'', 1); " +
                 "ELSE " +
                 "UPDATE [dbo].[DBMigrationsDataScriptCounters] SET [LastCounter] = [LastCounter] + 1 WHERE [TableName] = ''" + tableName + "''');";

                SqlConnection sqlConnection = new SqlConnection(ToolConfigurations.MainConnectionString);

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
                    ExitTool(exception.Message);
                }
            }
        }

        protected int GetDataScriptCounter(string tableName)
        {
            int lastCounter = 0;

            if (ToolConfigurations.DatabaseType.ToLower() == "oracle")
            {

            }
            else
            {
                string queryString = "SELECT [LastCounter] FROM [dbo].[DBMigrationsDataScriptCounters] WHERE [TableName] = '" + tableName + "'";

                SqlDataReader reader = null;
                SqlConnection connection = new SqlConnection(ToolConfigurations.MainConnectionString);
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    reader.Read();

                    if (reader.HasRows)
                    {
                        lastCounter = Convert.ToInt32(reader["LastCounter"].ToString());
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
                    ExitTool(exception.Message);
                }
            }

            return lastCounter;
        }

        protected void ReadConfigurations()
        {
            string configurationsFilePath;
            if (ToolArguments.IsArgumentProvided(Arguments.DEPLOYMENT))
            {
                string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
                configurationsFilePath = Path.Combine(projectDirectory, @"Configurations.xml");
            }
            else
            {
                string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                configurationsFilePath = Path.Combine(projectDirectory, @"Settings\Configurations.xml");
            }

            if (File.Exists(configurationsFilePath))
            {
                try
                {
                    string configurationsXmlString = File.ReadAllText(configurationsFilePath);
                    Configurations = configurationsXmlString.ParseXML<Configurations>();
                }
                catch (Exception)
                {
                    ExitTool("Error: Cannot Read Configurations File");
                }
            }
            else
            {
                ExitTool("Error: Cannot Find File " + configurationsFilePath);
            }
        }

        protected void SetToolConfigurations()
        {
            try
            {
                string dbConfigFileName = null;
                int aotScriptsExecutionTimeOut = 30;
                bool aotCreateIndexWithOnline = true;
                Config dbConfigFileNameConfig = Configurations.Configs.Where(c => c.Name.ToLower() == "DBConfigFileName".ToLower()).FirstOrDefault();
                Config aotScriptsExecutionTimeOutConfig = Configurations.Configs.Where(c => c.Name.ToLower() == "AOTScriptsExecutionTimeOut".ToLower()).FirstOrDefault();
                Config aotCreateIndexWithOnlineConfig = Configurations.Configs.Where(c => c.Name.ToLower() == "AOTCreateIndexWithOnline".ToLower()).FirstOrDefault();

                Config smtpClientHostConfig = Configurations.Configs.Where(c => c.Name.ToLower() == "SmtpClientHost".ToLower()).FirstOrDefault();
                Config smtpClientPortConfig = Configurations.Configs.Where(c => c.Name.ToLower() == "SmtpClientPort".ToLower()).FirstOrDefault();
                Config smtpClientUsernameConfig = Configurations.Configs.Where(c => c.Name.ToLower() == "SmtpClientUsername".ToLower()).FirstOrDefault();
                Config smtpClientPasswordConfig = Configurations.Configs.Where(c => c.Name.ToLower() == "SmtpClientPassword".ToLower()).FirstOrDefault();
                Config fromEmailAddressConfig = Configurations.Configs.Where(c => c.Name.ToLower() == "FromEmailAddress".ToLower()).FirstOrDefault();
                Config toEmailAddressesConfig = Configurations.Configs.Where(c => c.Name.ToLower() == "ToEmailAddresses".ToLower()).FirstOrDefault();
                
                if (dbConfigFileNameConfig != null)
                {
                    dbConfigFileName = dbConfigFileNameConfig.Value;
                }

                if(aotScriptsExecutionTimeOutConfig != null)
                {
                    aotScriptsExecutionTimeOut = String.IsNullOrEmpty(aotScriptsExecutionTimeOutConfig.Value) ? 30 : Convert.ToInt32(aotScriptsExecutionTimeOutConfig.Value);
                }

                if(aotCreateIndexWithOnlineConfig != null)
                {
                    aotCreateIndexWithOnline = String.IsNullOrEmpty(aotCreateIndexWithOnlineConfig.Value) || (aotCreateIndexWithOnlineConfig.Value == "true");
                }

                if (String.IsNullOrEmpty(dbConfigFileName))
                {
                    ExitTool("Error: Cannot Find DBConfigFileName In Configurations File");
                }

                string dbConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbConfigFileName);

                if (!File.Exists(dbConfigFilePath))
                {
                    ExitTool("Error: Cannot Find File " + dbConfigFilePath);
                }

                string dbConfigXmlString = File.ReadAllText(dbConfigFilePath);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dbConfigXmlString);

                XmlElement databaseTypeElement = (XmlElement)doc.SelectSingleNode(string.Format("appSettings/add[@key='{0}']", "DatabaseType"));
                XmlElement globalConnectionStringElement = (XmlElement)doc.SelectSingleNode(string.Format("appSettings/add[@key='{0}']", "GlobalConnectionString"));
                XmlElement mainConnectionStringElement = (XmlElement)doc.SelectSingleNode(string.Format("appSettings/add[@key='{0}']", "MainConnectionString"));
                XmlElement systemLogsConnectionStringElement = (XmlElement)doc.SelectSingleNode(string.Format("appSettings/add[@key='{0}']", "SystemLogsConnectionString"));
                XmlElement cargoTrackingConnectionStringElement = (XmlElement)doc.SelectSingleNode(string.Format("appSettings/add[@key='{0}']", "CargoTrackingConnectionString"));

                ToolConfigurations.DatabaseType = databaseTypeElement == null ? null : (databaseTypeElement.Attributes["value"]?.Value);
                ToolConfigurations.GlobalConnectionString = globalConnectionStringElement == null ? null : (globalConnectionStringElement.Attributes["value"]?.Value);
                ToolConfigurations.MainConnectionString = mainConnectionStringElement == null ? null : (mainConnectionStringElement.Attributes["value"]?.Value);
                ToolConfigurations.SystemLogsConnectionString = systemLogsConnectionStringElement == null ? null : (systemLogsConnectionStringElement.Attributes["value"]?.Value);
                ToolConfigurations.CargoTrackingConnectionString = cargoTrackingConnectionStringElement == null ? null : (cargoTrackingConnectionStringElement.Attributes["value"]?.Value);
                ToolConfigurations.AOTScriptsExecutionTimeOut = aotScriptsExecutionTimeOut;
                ToolConfigurations.AOTCreateIndexWithOnline = aotCreateIndexWithOnline;

                ToolConfigurations.SmtpClientHost = smtpClientHostConfig?.Value;
                ToolConfigurations.SmtpClientPort = Convert.ToInt32(smtpClientPortConfig?.Value);
                ToolConfigurations.SmtpClientUsername = smtpClientUsernameConfig?.Value;
                ToolConfigurations.SmtpClientPassword = smtpClientPasswordConfig?.Value;
                ToolConfigurations.FromEmailAddress = fromEmailAddressConfig?.Value;
                ToolConfigurations.ToEmailAddresses = toEmailAddressesConfig?.Value;
            }
            catch (Exception)
            {
                ExitTool("Error: Cannot Set Tool Configurations");
            }
        }

        protected void ExitTool(string message)
        {
            Console.WriteLine(message);
            Environment.Exit(1);
        }
    }
}