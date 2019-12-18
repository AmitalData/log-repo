using Logitude.DXMLGenerator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DXMLGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            string globalConnectionString = ConfigurationManager.AppSettings["GlobalConnectionString"];
            string mainConnectionString = ConfigurationManager.AppSettings["MainConnectionString"];
            string systemLogsConnectionString = ConfigurationManager.AppSettings["SystemLogsConnectionString"];


            Console.WriteLine("Generate DXML Files From Global Database ...\n");
            DXMLFilesGenerator globalDBGenerator = new DXMLFilesGenerator(globalConnectionString, "GlobalErrors.txt");
            globalDBGenerator.GenerateDXMLFiles();
            Console.WriteLine("\n\n\n");


            Console.WriteLine("Generate DXML Files From Main Database ...\n");
            DXMLFilesGenerator mainDBGenerator = new DXMLFilesGenerator(mainConnectionString, "MainErrors.txt");
            mainDBGenerator.GenerateDXMLFiles();
            Console.WriteLine("\n\n\n");


            Console.WriteLine("Generate DXML Files From SystemLogs Database ...\n");
            DXMLFilesGenerator systemLogsDBGenerator = new DXMLFilesGenerator(systemLogsConnectionString, "SystemLogsErrors.txt");
            systemLogsDBGenerator.GenerateDXMLFiles();
            Console.WriteLine("\n\n\n");
        }
    }
}