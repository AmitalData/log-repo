namespace Logitude.DBMigrations.Models
{
    public class RunSettings
    {
        public bool DebugMode { get; set; }
        public bool ExecuteScripts { get; set; }
        public bool DevMode { get; set; }
        public bool IgnoreHash { get; set; }
        public bool ValidateFiles { get; set; }
        public string Root { get; set; }
        public string SpecificDxmlFile { get; set; }
        public string SpecificSxmlFile { get; set; }
    }
}