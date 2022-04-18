using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
   public class DWObjectField
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DWObjectTableCode { get; set; }
        public string DataTypeCode { get; set; }
        public string DimensionTableCode { get; set; }
        public int MaxLength { get; set; }
        public int MinLength { get; set; }
        public bool IsRequired { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsMeasurement { get; set; }
        public string AggregationTypeCode { get; set; }
        public bool DisplayInQueryBuilder { get; set; }
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public string LOVAdditionalColumns { get; set; }
        public bool HideTree { get; set; }
        public bool CannotFilter { get; set; }
        public string HelpText { get; set; }
        public bool IsCustom { get; set; }
        public string OriginalObjectFieldCode { get; set; }
        public string ViewFieldDisplayName { get; set; }
        public bool DontDisplayInView { get; set; }
        public string DimensionDataViewName { get; set; }
        public bool IsMultipleSelection { get; set; }
        public bool UseUnitSelection { get; set; }
        public string RecordType { get; set; }

        
        [ForeignKey("DWObjectTableCode")]
        public virtual DWObjectTable DWObjectTable { get; set; }

        [ForeignKey("DimensionTableCode")]
        public virtual DWObjectTable DimensionTable { get; set; }
    }
}
