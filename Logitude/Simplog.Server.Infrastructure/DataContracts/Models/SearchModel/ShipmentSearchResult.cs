namespace Simplog.Server.Infrastructure.DataContracts.Models.SearchModel
{
    public class ShipmentSearchResult : GlobalSearchResult
    {
        public string TransportMode { get; set; }
        public string ShipmentNumber { get; set; }
        public string Direction { get; set; }
    }
}
