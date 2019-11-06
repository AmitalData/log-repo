using Logitude.DBMigrations.Helpers;
using Logitude.DBMigrations.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace Logitude.DBMigrations
{
    class Program
    {
        static void Main(string[] args)
        {

            var arguments = Array.ConvertAll(args, a => a.ToLower());

            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string DXMLFilesPath = Path.Combine(projectDirectory, @"EntityFiles");
            string[] DXMLFiles = Directory.GetFiles(DXMLFilesPath, "*.dxml");

            string generatedScript = "";

            foreach (var DXMLFile in DXMLFiles)
            {
                Console.WriteLine("Getting Script For " + Path.GetFileName(DXMLFile).Split('.')[0] + " Entity ...");
                string xmlString = File.ReadAllText(DXMLFile);
                TableDefinition DXMLTable = xmlString.ParseXML<TableDefinition>();
                TableMigrations tableMigrations = new TableMigrations(DXMLTable);
                generatedScript += tableMigrations.GetScript();
                generatedScript += "\n-------------------------------------------------------\n";
            }

            Console.WriteLine("Saving Script To GeneratedScript/Script.sql ...");
            string generatedScriptFilePath = Path.Combine(projectDirectory, @"GeneratedScript\Script.sql");
            File.WriteAllText(generatedScriptFilePath, generatedScript);

            if (Array.IndexOf(arguments, "-exe") != -1)
            {
                Console.WriteLine("Executing The Generated Script To The Database ...");
                try
                {
                    string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

                    SqlConnection mySqlConnection = new SqlConnection(connectionString);
                    SqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                    mySqlCommand.CommandText = generatedScript;
                    mySqlConnection.Open();
                    mySqlCommand.ExecuteNonQuery();

                    Console.WriteLine("The Generated Script Executed Successfully ...");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error While Executing The Generated Script ...");
                    Console.WriteLine(e.ToString());
                }
            }

        }
    }
}
