using System;

namespace Logitude.DBMigrations.Models
{
    public static class ToolArguments
    {
        public static string[] Arguments;

        public static bool IsArgumentProvided(string arg)
        {
            string[] arguments = Array.ConvertAll(Arguments, a => a.ToLower());
            return (Array.IndexOf(arguments, arg) != -1);
        }

        public static string GetRunCommand()
        {
            string toolName = "Logitude.DBMigrations.exe";
            if (Arguments == null || (Arguments != null && Arguments.Length == 0))
            {
                return toolName;
            }

            return toolName + " " + string.Join(" ", Arguments);
        }
    }
}