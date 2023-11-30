namespace Simplog.Server.Infrastructure.DataContracts.Models.SearchModel
{
    public class QuoteSearchResult : GlobalSearchResult
    {
        public string TransportMode { get; set; }
        public string QuoteNumber { get; set; }
        public string Direction { get; set; }
    }
}
