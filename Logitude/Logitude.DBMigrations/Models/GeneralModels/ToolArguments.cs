using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}