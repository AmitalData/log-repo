using Logitude.DBMigrations.Helpers;
using System;

namespace Logitude.DBMigrations
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (AppHelper.CheckAppArguments(args, "-root"))
            {
                string root = AppHelper.GetRoot(args);
                //root = @"C:\Users\AbedMalakh\source\repos\log-repo\Logitude";

                if (!String.IsNullOrEmpty(root))
                {
                    string[] DXMLFiles = AppHelper.GetDXMLFilesFromRoot(root);
                    if (DXMLFiles != null)
                    {
                        if (AppHelper.IsConnectionStringValid("msql"))
                        {
                            string generatedScript = AppHelper.GenerateScriptFromDXMLFiles(DXMLFiles);
                            AppHelper.SaveScript(generatedScript);
                            if (AppHelper.CheckAppArguments(args, "-exe"))
                            {
                                if (string.IsNullOrEmpty(generatedScript))
                                {
                                    Console.WriteLine("There Are No Changes To Execute");
                                }
                                else
                                {
                                    AppHelper.ExecuteScript("msql", generatedScript);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Cannot Connect To Database Using The Connection String");
                        }
                    }
                    else
                    {
                        Console.WriteLine("There Is No DXML Files Found Under The Specified Root");
                    }
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