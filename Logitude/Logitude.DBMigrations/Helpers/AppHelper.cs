using Logitude.DBMigrations.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

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
                if(DXMLFiles.Length > 0)
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

        public static string GenerateScriptFromDXMLFiles(string[] DXMLFiles)
        {
            string generatedScript = "";

            foreach (var DXMLFile in DXMLFiles)
            {
                var fileName = Path.GetFileName(DXMLFile);
                Console.WriteLine("Generating Script For " + fileName + " ...");
                string xmlString = File.ReadAllText(DXMLFile);
                TableDefinition DxmlTable = xmlString.ParseXML<TableDefinition>();
                SQLDatabaseMigrations databaseMigrations = new SQLDatabaseMigrations(DxmlTable);
                string DxmlTableScript = databaseMigrations.GetScript();
                if (!String.IsNullOrEmpty(DxmlTableScript))
                {
                    generatedScript += "/* Generated Script For " + fileName + " */\n";
                    generatedScript += DxmlTableScript;
                    generatedScript += "\n";
                }
            }

            return generatedScript;
        }

        public static void SaveScript(string generatedScript)
        {
            Console.WriteLine("Saving The Generated Scripts ...");
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string generatedScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\Script.sql");
            File.WriteAllText(generatedScriptFilePath, generatedScript);
            Console.WriteLine("The Generated Scripts Saved Successfully To /GeneratedScript/Script.sql");
        }
        
        public static void ExecuteScript(string generatedScript)
        {
            Console.WriteLine("Executing The Generated Script To The Database ...");
            try
            {
                string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = connection.CreateCommand();
                command.CommandText = generatedScript;
                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("The Generated Script Executed Successfully");
            }
            catch (Exception exception)
            {
                Console.Write("Error While Executing Script: ");
                Console.WriteLine(exception.Message);
            }
        }

        public static bool IsConnectionStringValid()
        {
            Console.WriteLine("Checking If The Connection String Is Valid ...");
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                bool isConnectionStringValid;
                try
                {
                    connection.Open();
                    isConnectionStringValid = true;
                    connection.Close();
                }
                catch (Exception)
                {
                    isConnectionStringValid = false;
                    connection.Close();
                }
                return isConnectionStringValid;
            }
        }

        public static bool CheckAppArguments(string[] args, string arg)
        {
            string[] arguments = Array.ConvertAll(args, a => a.ToLower());
            return (Array.IndexOf(arguments, arg) != -1);
        }

        public static string GetRoot(string[] args)
        {
            string[] arguments = Array.ConvertAll(args, a => a.ToLower());
            int indexOfRootArgument = Array.IndexOf(arguments, "-root") + 1;
            if(indexOfRootArgument < args.Length && indexOfRootArgument >= 0)
            {
                string root = args[indexOfRootArgument];
                return root;
            }
            else
            {
                return null;
            }
        }
    }
}