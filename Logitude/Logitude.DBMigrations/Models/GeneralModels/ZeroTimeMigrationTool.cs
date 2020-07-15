using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Logitude.DBMigrations.Models
{
    public class ZeroTimeMigrationTool : MigrationTool
    {
        public ZeroTimeMigrationTool(string[] arguments, RunSettings runSettings) : base(arguments, runSettings){ }

        public override void RunTool()
        {
            while (true)
            {
                StartZeroDownTimeMigration();
                Thread.Sleep(600000);//10000
            }
        }

        protected void StartZeroDownTimeMigration()
        {
            Console.WriteLine("Zero Down Time Migration Started At " + DateTime.Now);






            string[] dxmlFiles = GetDXMLFilesFromRoot(Root);
            string[] sxmlFiles = GetSXMLFilesFromRoot(Root);

            ValidateDBFiles(dxmlFiles, sxmlFiles);
            PrepareRequiredData();

            DXMLDefinitions dxmlDefinitions = GetDXMLDefinitions(dxmlFiles);
            dxmlDefinitions = FilterDXMLDefinitions(dxmlDefinitions);

            List<DXMLTable> dxmlTables = dxmlDefinitions.DXMLTables;

            Console.WriteLine(dxmlTables.Count);






            Console.WriteLine("Zero Down Time Migration Finished At " + DateTime.Now + "\n");
        }
    }
}