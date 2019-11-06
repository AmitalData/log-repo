using Logitude.DBMigrations.Helpers;
using Logitude.DBMigrations.Models;
using System;
using System.IO;

namespace Logitude.DBMigrations
{
    class Program
    {
        static void Main(string[] args)
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string path = Path.Combine(projectDirectory, @"EntityFiles");
            string[] files = Directory.GetFiles(path, "*.dxml");

            string xmlString = File.ReadAllText(files[1]);
            TableDefinition table = xmlString.ParseXML<TableDefinition>();
            
            Console.WriteLine(table);
            //foreach (string file in files)
            //{
            //    string content = File.ReadAllText(file);
            //    Console.WriteLine(content);
            //}
        }
    }
}
