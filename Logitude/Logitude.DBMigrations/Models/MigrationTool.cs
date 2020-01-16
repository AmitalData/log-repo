using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security;

namespace Logitude.DBMigrations.Models
{
    public class MigrationTool
    {
        private string DatabaseType = ConfigurationManager.AppSettings["DatabseType"];
        private string PerformanceData = "Description,Time(ms)\n";
        private string[] Arguments;
        private List<TableDefinition> DXMLTables;
        private string ScriptSemicolonCode = "|(;)|";
        
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
                        
                        GeneratedScript generatedScript = GenerateScriptsFromDXMLFiles(dxmlFiles);

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

        private GeneratedScript GenerateScriptsFromDXMLFiles(string[] dxmlFiles)
        {
            GeneratedScript generatedScript = new GeneratedScript();
            RelationsScript relationsScript = new RelationsScript();

            var stopwatch = Stopwatch.StartNew();

            DXMLDefinitions dxmlDefinitions = GetDXMLDefinitions(dxmlFiles);

            List<DXMLTable> dxmlTables = dxmlDefinitions.DXMLTables;
            List<DXMLView> dxmlViews = dxmlDefinitions.DXMLViews;
            List<DXMLProcedure> dxmlProcedures = dxmlDefinitions.DXMLProcedures;

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

            foreach (var dxmlFile in dxmlFiles)
            {
                string xmlString = File.ReadAllText(dxmlFile);

                if (xmlString.EndsWith("</Table>"))
                {
                    DXMLTable dxmlTable = CreateDXMLTable(xmlString, dxmlFile);
                    if(dxmlTable == null)
                    {
                        ExitTool("Cannot Create Table Definition For " + Path.GetFileName(dxmlFile));
                    }
                    dxmlTables.Add(dxmlTable);
                }
                else if (xmlString.EndsWith("</View>"))
                {
                    DXMLView dxmlView = CreateDXMLView(xmlString, dxmlFile);
                    if(dxmlView == null)
                    {
                        ExitTool("Cannot Create View Definition For " + Path.GetFileName(dxmlFile));
                    }
                    dxmlViews.Add(dxmlView);
                }
                else if (xmlString.EndsWith("</Procedure>"))
                {
                    DXMLProcedure dxmlProcedure = CreateDXMLProcedure(xmlString, dxmlFile);
                    if(dxmlProcedure == null)
                    {
                        ExitTool("Cannot Create Procedure Definition For " + Path.GetFileName(dxmlFile));
                    }
                    dxmlProcedures.Add(dxmlProcedure);
                }
                else
                {
                    ExitTool("Cannot Create Class Definition For " + Path.GetFileName(dxmlFile));
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

        private DXMLTable CreateDXMLTable(string xmlString, string dxmlFile)
        {
            try
            {
                TableDefinition dxmlTableDefinition = xmlString.ParseXML<TableDefinition>();

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

        private DXMLView CreateDXMLView(string xmlString, string dxmlFile)
        {
            try
            {
                string sqlScriptFromXmlString = xmlString.Split(new string[] { "<SqlScript>" }, StringSplitOptions.None)[1].Split(new string[] { "</SqlScript>" }, StringSplitOptions.None)[0];
                string escapedSqlScript = SecurityElement.Escape(GetScriptFromCDataSection(sqlScriptFromXmlString)).Trim();

                string oracleScriptFromXmlString = xmlString.Split(new string[] { "<OracleScript>" }, StringSplitOptions.None)[1].Split(new string[] { "</OracleScript>" }, StringSplitOptions.None)[0];
                string escapedOracleScript = SecurityElement.Escape(GetScriptFromCDataSection(oracleScriptFromXmlString)).Trim();

                xmlString = xmlString.Split(new string[] { "<SqlScript>" }, StringSplitOptions.None)[0] + "<SqlScript>" + escapedSqlScript + "</SqlScript>" + "<OracleScript>" + escapedOracleScript + "</OracleScript>" + "</View>";

                ViewDefinition dxmlViewDefinition = xmlString.ParseXML<ViewDefinition>();

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

        private DXMLProcedure CreateDXMLProcedure(string xmlString, string dxmlFile)
        {
            try
            {
                string sqlScriptFromXmlString = xmlString.Split(new string[] { "<SqlScript>" }, StringSplitOptions.None)[1].Split(new string[] { "</SqlScript>" }, StringSplitOptions.None)[0];
                string escapedSqlScript = SecurityElement.Escape(GetScriptFromCDataSection(sqlScriptFromXmlString)).Trim();

                string oracleScriptFromXmlString = xmlString.Split(new string[] { "<OracleScript>" }, StringSplitOptions.None)[1].Split(new string[] { "</OracleScript>" }, StringSplitOptions.None)[0];
                string escapedOracleScript = SecurityElement.Escape(GetScriptFromCDataSection(oracleScriptFromXmlString)).Trim();

                xmlString = xmlString.Split(new string[] { "<SqlScript>" }, StringSplitOptions.None)[0] + "<SqlScript>" + escapedSqlScript + "</SqlScript>" + "<OracleScript>" + escapedOracleScript + "</OracleScript>" + "</Procedure>";

                ProcedureDefinition dxmlProcedureDefinition = xmlString.ParseXML<ProcedureDefinition>();

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
            script = string.Join("\n", script.Split('\n').Select(l => l.Trim()).ToArray());

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

        private void ExitTool(string message)
        {
            Console.WriteLine(message);
            Environment.Exit(0);
        }
    }
}