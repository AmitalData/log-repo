using Logitude.DXMLGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DXMLGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Get All Tables From The Database ...");
            List<string> dbTablesNames = Helper.GetAllTablesFromDB();
            Console.WriteLine(dbTablesNames.Count() + " Tables Found\n");


            //Console.WriteLine("Serialize And Save All Database Tables As DXML Files ...");
            //bool loopEnabled = true;
            //foreach (string tableName in dbTablesNames)
            //{
            //    //if (loopEnabled)
            //    if (tableName == "QueueMessages")
            //    {
            //        TableDefinition tableDefinition = Helper.GetTableDefinition(tableName);
            //        if (tableDefinition != null)
            //        {
            //            Helper.SerializeAndSaveTableDefinition(tableDefinition);
            //        }
            //        loopEnabled = false;
            //    }
            //}
            //Console.WriteLine("All Database Tables Serialized And Saved Successfully\n");


            int nullPathsCount = 0;
            int notNullPathsCount = 0;

            foreach (string tableName in dbTablesNames)
            {
                string path = Helper.GetPath(tableName);
                if (String.IsNullOrEmpty(path))
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Cannot Find Path For " + tableName + " Table");
                    Console.ResetColor();
                    nullPathsCount++;
                }
                else
                {
                    notNullPathsCount++;
                    Console.WriteLine(path);
                }
            }

            Console.WriteLine("\n");
            Console.WriteLine("Number Of Not Null Paths: " + notNullPathsCount);
            Console.WriteLine("Number Of Null Paths: " + nullPathsCount);
            Console.WriteLine("\n");


        }
    }

}
