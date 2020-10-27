using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class DWObjectFieldPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DWObjectTableCode { get; set; }
        public string DataTypeCode { get; set; }
        public string DimensionTableCode { get; set; }
        public int MaxLength { get; set; }
        public int MinLength { get; set; }
        public bool IsRequiered { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsMeasurement { get; set; }
        public string AggregationTypeCode { get; set; }
        public bool DisplayInQueryBuilder { get; set; }
        public string DisplayName { get; set; }
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public string LOVAdditionalColumns { get; set; }
        public string Category { get; set; }
        public int CategoryIndex { get; set; }
        public bool HideTree { get; set; }
        public string DimensionTableDisplayName { get; set; }
        public bool CannotFilter { get; set; }
        public string HelpText { get; set; }
        public bool IsCustom { get; set; }
        public string CustomPickListCode { get; set; }
        public string OriginalObjectFieldCode { get; set; }
        public string FullNameTextCodeCode { get; set; }
        public string PartnerFullNameTextCodeCode { get; set; }
        public string PartnerOriginalObjectFieldCode { get; set; }
        public string ViewFieldDisplayName { get; set; }
        public bool DontDisplayInView { get; set; }
        public string DimensionDataViewName { get; set; }
        public bool IsMultipleSelection { get; set; }
        public string RecordType { get; set; }
        public string FactTableCode { get; set; }




    }

}
