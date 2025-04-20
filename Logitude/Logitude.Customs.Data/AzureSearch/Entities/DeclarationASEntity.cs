using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using System;

namespace Logitude.Customs.Data.AzureSearch.Entities
{
    public class DeclarationASEntity
    {
        [SimpleField(IsKey = true, IsFilterable = true)]
        public string id { get; set; }

        [SimpleField(IsFilterable = true)]
        public string customFileNo { get; set; }

        [SimpleField(IsFilterable = true)]
        public int tenant { get; set; }

        [SimpleField(IsFilterable = true)]
        public string customerId { get; set; }

        [SimpleField(IsFilterable = true)]
        public string importerId { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.HeMicrosoft, IsFilterable = true)]
        public string searchFields { get; set; }

        [SimpleField(IsFilterable = true)]
        public string declarationNumber { get; set; }

        [SimpleField(IsFilterable = true)]
        public string externalDeclarationNumber { get; set; }

        [SimpleField(IsFilterable = true, IsFacetable = true)]
        public DateTimeOffset createDateTime { get; set; }

        [SimpleField(IsFilterable = true, IsFacetable = true)]
        public DateTimeOffset updateDateTime { get; set; }

        [SimpleField(IsFilterable = true, IsFacetable = true)]
        public bool isCancelled { get; set; }

        [SimpleField(IsFilterable = true, IsFacetable = true)]
        public string signerPesonalId { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.HeMicrosoft, IsFilterable = true)]
        public string casualSupplierName { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.HeMicrosoft, IsFilterable = true, IsFacetable = true)]
        public string courierHAWB { get; set; }

        [SimpleField(IsFilterable = true)]
        public string courierSearchFields { get; set; }

        [SimpleField(IsFilterable = true, IsFacetable = true)]
        public string amendmentRequestNumber { get; set; }

        [SimpleField(IsFilterable = true, IsFacetable = true)]
        public string exportFile { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.HeMicrosoft, IsFilterable = true)]
        public string cargoDescription { get; set; }

        [SimpleField(IsFilterable = true, IsFacetable = true)]
        public string exportCloseAmendRequestNumber { get; set; }

        [SimpleField(IsFilterable = true, IsFacetable = true)]
        public string direction { get; set; }

        [SimpleField(IsFilterable = true, IsFacetable = true)]
        public bool amendmentDontDisplayInList { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.HeMicrosoft, IsFilterable = true)]
        public string customerName { get; set; }

        [SimpleField(IsFilterable = true, IsFacetable = true)]
        public string transportModeId { get; set; }
    }
}
