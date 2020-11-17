using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseData
{
    public class TableClass
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
        public List<DWObjectFieldDB> DWObjectFieldDBLists { get; set; }
        public string DWTableKeyName { get; set; }
        public string FieldIndexes { get; set; }
        public string RefreshIds { get; set; }
        public List<TableClass> RelatedEntities { get; set; }
        public string ParentKeyName { get; set; }

        public string AdditionalIndexes { get; set; }

        public List<DWObjectFieldDB> ObjectFieldDBLists { get; set; }

        public List<IndexItem> Indexes { get; set; }

        public List<string> FieldsDBNameLists { get; set; }


    }

    public class DWObjectFieldDB
    {
        public string FieldName { get; set; }
        public string DataTypeCode { get; set; }
        public int MaxLength { get; set; }
        public int MinLength { get; set; }
        public bool IsRequired { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string DimensionTableCode { get; set; }
        
    }

    [DataContract(Namespace = "")]
    public class IndexItem
    {
        [DataMember]
        public string Columns { get; set; }

        [DataMember]
        public string Include { get; set; }
 

    }

}
