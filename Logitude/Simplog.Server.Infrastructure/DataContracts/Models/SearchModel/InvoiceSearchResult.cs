
namespace Simplog.Server.Infrastructure.DataContracts.Models.SearchModel
{
    public class InvoiceSearchResult : GlobalSearchResult
    {
        public string InvoiceNumber { get; set; }
        public string Type { get; set; }
        public bool IsConsolidationInvoice { get; set; }
    }
}
