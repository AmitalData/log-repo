using Logitude.DBMigrations.Helpers;

namespace Logitude.DBMigrations
{
    public class Program
    {
        static void Main(string[] args)
        {
            string generatedScript = AppHelper.GenerateScriptFromDXMLFiles();
            AppHelper.SaveGeneratedScript(generatedScript);
            if (AppHelper.CheckAppArguments(args, "-exe"))
            {
                AppHelper.ExecuteGeneratedScript(generatedScript);
            }
        }
    }
}