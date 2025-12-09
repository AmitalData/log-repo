namespace Logitude.Customs.BL.AzureSearch.Objects
{    
    public class FastSearchSettings
    {
        public int maxResults { get; set; }
        public int idleSearchTimeMs { get; set; }
        public int showTopResults { get; set; }
        public bool showRecent { get; set; }
        public string showRecentObject { get; set; }
        public string ddlHtmlLine { get; set; }
        public string recentLineHeader { get; set; }
        public int recentShowTopResults { get; set; }
        public string recentEditScreen { get; set; }
        public string recentEditScreenParam { get; set; }
        public string addAsteriskToNumberSearch { get; set; }
        public string DDLWidth { get; set; }
        public int? minimumSearchQueryLength { get; set; }
        public bool? showSeparator { get; set; }
        public bool? showHeader { get; set; }

    }
}
