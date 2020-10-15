using System;

namespace Logitude.DeploymentUtilities.Models
{
    public static class ToolArguments
    {
        public static string[] Arguments;

        public static bool IsArgumentProvided(string argument)
        {
            string[] arguments = Array.ConvertAll(Arguments, a => a?.ToLower());
            return (Array.IndexOf(arguments, argument?.ToLower()) != -1);
        }

        public static string GetArgumentValue(string argument)
        {
            string[] arguments = Array.ConvertAll(Arguments, a => a?.ToLower());
            int indexOfArgumentValue = Array.IndexOf(arguments, argument?.ToLower()) + 1;
            if (indexOfArgumentValue < Arguments.Length && indexOfArgumentValue >= 0)
            {
                string argumentValue = Arguments[indexOfArgumentValue];
                return argumentValue;
            }
            else
            {
                return null;
            }
        }
    }
}