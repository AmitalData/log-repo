using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace Logitude.Customs.Data.AzureSearch.Entities
{
    public class RemarksCustomsBookASEntity
    {
        [SimpleField(IsKey = true, IsFilterable = true)]
        public string Id { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public int Tenant { get; set; }

        public string Drop_CB_ID { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public int CustomsItemsID { get; set; }
        
        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.HeMicrosoft)]
        public string RemarkDescription { get; set; }
    }
}