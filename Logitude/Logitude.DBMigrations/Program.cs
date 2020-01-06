using Logitude.DBMigrations.Models;

namespace Logitude.DBMigrations
{
    public class Program
    {
        static void Main(string[] args)
        {
            MigrationTool migrationTool = new MigrationTool(args);
            migrationTool.RunTool();
        }
    }
}