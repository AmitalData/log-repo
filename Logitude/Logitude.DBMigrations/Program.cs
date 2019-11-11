using Logitude.DBMigrations.Helpers;

namespace Logitude.DBMigrations
{
    public class Program
    {
        static void Main(string[] args)
        {
            string generatedScript = AppHelper.GenerateScriptFromDXMLFiles();
            AppHelper.SaveScript(generatedScript);
            if (AppHelper.CheckAppArguments(args, "-exe"))
            {
                AppHelper.ExecuteScript(generatedScript);
            }
        }
    }
}