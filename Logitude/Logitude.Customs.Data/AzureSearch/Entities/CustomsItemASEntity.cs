using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Logitude.Customs.Data.EntityPOCOs;
using System;

namespace Logitude.Customs.Data.AzureSearch.Entities
{
    public class CustomsItemASEntity
    {
        [SimpleField(IsKey = true, IsFilterable = true)] 
        public string CB_ID { get; set; }
        public int ID { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true)]
        public int CustomsItemID { get; set; }

        [SearchableField]
        public string FullClassification { get; set; }
        public bool IsLeaf { get; set; }
        public int CustomsItemDetailsHistoryID { get; set; }
        public int PropertiesDetailsHistoryID { get; set; }
        public int PH_MeasurementUnitID { get; set; }
        public bool IsHistoryExists { get; set; }
        public bool IsRulesExists { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true)]
        public DateTime StartDate { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true)]
        public DateTime EndDate { get; set; }
        public int CI_Parent_CustomsItemIDNum { get; set; }
        public string CI_BaseFullClassification { get; set; }
        public string CI_ComputedCheckDigit { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string CI_CustomsBookTypeIDNum { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string CI_CustomsItemCategoryIDNum { get; set; }
        public string ItemHierarchicLocationID { get; set; }
        public string CIH_Title { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.HeMicrosoft)]
        public string CIH_GoodsDescription { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public int CustomsItemEntityStatusIDNum { get; set; }
        public bool PH_IsCarItem { get; set; }
        public string FullGoodsDescription { get; set; }
        public int BaseCustomsItemID { get; set; }

        public CustomsItemASEntity()
        {
        }

        public CustomsItemASEntity(CB_CustomsItemComputedData origin)
        {
            var originType = origin.GetType();
            var cloneType = GetType();
            var properties = originType.GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(origin);
                var cloneProperty = cloneType.GetProperty(property.Name);
                if (cloneProperty != null)
                    cloneProperty.SetValue(this, value);
            }
        }
    }
}