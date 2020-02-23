using Logitude.DXMLGenerator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DXMLGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            //////Delete all DXML files of type table before begin generating
            Console.WriteLine("Deleting All DXML Files Of Type Table ...\n");
            int deletedDxmlFilesCount = 0;
            string root = ConfigurationManager.AppSettings["Root"];
            string dxmlFilesRoot = Path.Combine(root);
            string[] dxmlFiles = Directory.GetFiles(dxmlFilesRoot, "*.dxml", SearchOption.AllDirectories);

            foreach(var dxmlFile in dxmlFiles)
            {
                string dxmlFileName = Path.GetFileName(dxmlFile);

                if (dxmlFileName.ToLower() != "DBMigrationsHistory.dxml".ToLower() && dxmlFileName.ToLower() != "DBScriptsHistory.dxml".ToLower())
                {
                    string xmlString = File.ReadAllText(dxmlFile);
                    if (xmlString.EndsWith("</Table>"))
                    {
                        File.Delete(dxmlFile);
                        deletedDxmlFilesCount++;
                    }
                }
            }

            Console.WriteLine(deletedDxmlFilesCount + " DXML Files Of Type Table Deleted Successfully\n");
            //////
            

            string globalConnectionString = ConfigurationManager.AppSettings["GlobalConnectionString"];
            string mainConnectionString = ConfigurationManager.AppSettings["MainConnectionString"];
            string systemLogsConnectionString = ConfigurationManager.AppSettings["SystemLogsConnectionString"];
            bool includeCustoms = ConfigurationManager.AppSettings["IncludeCustoms"].ToLower() == "true";


            Console.WriteLine("Generate DXML Files From Global Database ...\n");
            DXMLFilesGenerator globalDBGenerator = new DXMLFilesGenerator(globalConnectionString, "GlobalErrors.txt", includeCustoms);
            globalDBGenerator.GenerateDXMLFiles();
            Console.WriteLine("\n\n");


            Console.WriteLine("Generate DXML Files From Main Database ...\n");
            DXMLFilesGenerator mainDBGenerator = new DXMLFilesGenerator(mainConnectionString, "MainErrors.txt", includeCustoms);
            mainDBGenerator.GenerateDXMLFiles();
            Console.WriteLine("\n\n");


            Console.WriteLine("Generate DXML Files From SystemLogs Database ...\n");
            DXMLFilesGenerator systemLogsDBGenerator = new DXMLFilesGenerator(systemLogsConnectionString, "SystemLogsErrors.txt", includeCustoms);
            systemLogsDBGenerator.GenerateDXMLFiles();
            Console.WriteLine("\n\n");
        }
    }
}