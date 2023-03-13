using System.Collections.Generic;

namespace WebFreight.Web.Controllers.DigitalPortal.Models
{
    public class GeneralFilters
    {
        public GeneralFilters()
        {
            AdditionalFilters = new List<AdditionalFilters>();
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string SortBy { get; set; }

        public string SortDirection { get; set; }

        public bool GetCount { get; set; }

        public bool DontApplyVirtualization { get; set; }

        public int Tenant { get; set; }

        public string CardId { get; set; }
        public string SearchText { get; set; }
        public string CardType { get; set; }
        public string ObjectTableName { get; set; }
        public string ObjectTableId { get; set; }
        public string ProfileCode { get; set; }
        public string LanguageCode { get; set; }
        public string ProfileId { get; set; }
        public List<AdditionalFilters> AdditionalFilters { get; set; }
    }

    public class AdditionalFilters
    {
        public string FieldName { get; set; }

        public string FieldValue { get; set; }

        public string FieldValue2 { get; set; }
        public string FieldValue3 { get; set; }
        public string Operator { get; set; }

        public bool IsCustom { get; set; }

        public bool DisplayInList { get; set; }
    }
}