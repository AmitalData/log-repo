using Logitude.DBMigrations.Helpers;
using Logitude.DBMigrations.Models;
using System;

namespace Logitude.DBMigrations
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (AppHelper.IsArgumentProvided(args, "-root"))// || true)
            {
                string root = AppHelper.GetRoot(args);
                //root = @"C:\Users\AbedMalakh\source\repos\log-repo\Logitude";
                //root = @"D:\TestDXML";

                if (!String.IsNullOrEmpty(root))
                {
                    string[] DXMLFiles = AppHelper.GetDXMLFilesFromRoot(root);
                    if (DXMLFiles != null)
                    {
                        GeneratedScript generatedScript = AppHelper.GenerateScriptFromDXMLFiles(DXMLFiles);
                        AppHelper.SaveScript(generatedScript);
                        if (AppHelper.IsArgumentProvided(args, "-exe"))
                        {
                            if (string.IsNullOrEmpty(generatedScript.GlobalScript) && string.IsNullOrEmpty(generatedScript.MainScript) && string.IsNullOrEmpty(generatedScript.SystemLogsScript))
                            {
                                Console.WriteLine("There Are No Changes To Execute");
                            }
                            else
                            {
                                AppHelper.ExecuteScript(generatedScript);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("There Is No DXML Files Found Under The Specified Root");
                    }

                    AppHelper.ExportPerformanceData();
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
    }
}