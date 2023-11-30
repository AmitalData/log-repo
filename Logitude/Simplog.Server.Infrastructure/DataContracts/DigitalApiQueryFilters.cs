namespace Simplog.Server.Infrastructure.DataContracts
{
    public class DigitalApiQueryFilters
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public bool GetCount { get; set; }
        public int? Tenant { get; set; }
        public string CardId { get; set; }
        public string SearchFields { get; set; }
        public string AdditionalFilters { get; set; }

    }
}
