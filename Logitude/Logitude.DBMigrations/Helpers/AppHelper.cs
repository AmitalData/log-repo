using Logitude.DBMigrations.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace Logitude.DBMigrations.Helpers
{
    public static class AppHelper
    {
        public static string GenerateScriptFromDXMLFiles()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string DXMLFilesPath = Path.Combine(projectDirectory, @"EntityFiles");
            string[] DXMLFiles = Directory.GetFiles(DXMLFilesPath, "*.dxml");

            string generatedScript = "";

            foreach (var DXMLFile in DXMLFiles)
            {
                var fileName = Path.GetFileName(DXMLFile).Split('.')[0];
                Console.WriteLine("Generating Script For " + fileName + " Entity ...");
                string xmlString = File.ReadAllText(DXMLFile);
                TableDefinition DXMLTable = xmlString.ParseXML<TableDefinition>();
                SQLDatabaseMigrations databaseMigrations = new SQLDatabaseMigrations(DXMLTable);
                generatedScript += databaseMigrations.GetScript();
                //generatedScript += "\n------------------------------------------------\n";
            }

            return generatedScript;
        }

        public static void SaveScript(string generatedScript)
        {
            Console.WriteLine("Saving The Generated Script ...");
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string generatedScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\Script.sql");
            File.WriteAllText(generatedScriptFilePath, generatedScript);
            Console.WriteLine("The Generated Script Saved Successfully To GeneratedScript/Script.sql");
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
            catch (Exception e)
            {
                Console.WriteLine("Error While Executing The Generated Script:");
                Console.WriteLine(e.ToString());
            }
        }

        public static bool CheckAppArguments(string[] args, string arg)
        {
            string[] arguments = Array.ConvertAll(args, a => a.ToLower());
            return (Array.IndexOf(arguments, arg) != -1);
        }
    }
}