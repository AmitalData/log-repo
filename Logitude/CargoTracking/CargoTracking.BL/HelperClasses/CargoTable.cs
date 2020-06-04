using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CargoTracking.CargoTracking.BL.HelperClasses
{
    public class CargoTable
    {
        public string TableName { get; set; }
        public string DBTableName { get; set; }
        public string Dw_TableName { get; set; }
        public bool HasConstraint { get; set; }
        public bool HasFactTable { get; set; }
        public bool HasNotSpecifiedValue { get; set; }
        public string KeyName { get; set; }
        public bool HasDimensionTable { get; set; }
        public string FieldsDBName { get; set; }
        public bool DispayInScreen { get; set; }
        public bool IsUpdated { get; set; }
        public int UpdatedCount { get; set; }
        public bool IsCloseTable { get; set; }
        public string BuildScriptName { get; set; }
        public string IncrementalScriptName { get; set; }
        public string ObjectTableId { get; set; }
        public string DWObjectTableCode { get; set; }
        public bool HasCustomFields { get; set; }
        public int CustomFieldsCount { get; set; }   
        public string DWTableKeyName { get; set; }
        public string FieldIndexes { get; set; }
        public string RefreshIds { get; set; }
        public string ParentKeyName { get; set; }
        public string AdditionalIndexes { get; set; }
        public List<Label> Labels { get; set; }
    }
}
