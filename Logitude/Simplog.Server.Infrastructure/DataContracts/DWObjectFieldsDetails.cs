using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.DataContracts
{
    [DataContract(Namespace = "")]
    public class DWObjectFieldsDetails
    {
        [DataMember] 
        public List<DWObjectFieldsDetails> FilterItems { get; set; }
        [DataMember] 
        public string Category1 { get; set; }
        [DataMember] 
        public string Category2 { get; set; }
        [DataMember] 
        public int IndexOrder { get; set; }
        [DataMember] 
        public bool IsPrimaryKey { get; set; }
        [DataMember] 
        public string Name { get; set; }
        [DataMember] 
        public string DisplayName { get; set; }
        [DataMember] 
        public string Code { get; set; }

        [DataMember] 
        public string ParentDimTabelName { get; set; }
        [DataMember] 
        public string DWObjectTableCode { get; set; }
        [DataMember]
        public bool IsMeasurement { get; set; }
        [DataMember] 
        public string AggregationTypeCode { get; set; }
        [DataMember] 
        public string DataTypeCode { get; set; }
        [DataMember] 
        public string ParentDataTypeCode { get; set; }
        [DataMember] 
        public string DimensionTableCode { get; set; }
        [DataMember] 
        public object TextValue { get; set; }
        [DataMember] 
        public ObjectFieldOperator Operation { get; set; }
        [DataMember]
        public string OperationCode { get; set; }
        [DataMember] 
        public string OperationName { get; set; }
        [DataMember]
        public string AndOr { get; set; }
        [DataMember] 
        public bool IsSetDefaults { get; set; }
        [DataMember]
        public bool IsMandatoryFilter { get; set; }
        [DataMember] 
        public string FilterType { get; set; }
        [DataMember]
        public string DimensionTableDisplayName { get; set; }
        [DataMember]
        public string LOVAdditionalColumns { get; set; }
        [DataMember]
        public bool HideTree { get; set; }
        [DataMember] 
        public bool IsCustom { get; set; }
        [DataMember]
        public string CustomPickListCode { get; set; }
        [DataMember] 
        public bool IsMultipleSelection { get; set; }
        [DataMember]
        public bool UseUnitSelection { get; set; }
        [DataMember]
        public string SelectedUnitCode { get; set; }
        [DataMember]
        public List<MultiSelectedValue> MultiSelectedValueLists { get; set; }

    }

    [DataContract(Namespace = "")]
    public class MultiSelectedValue
    {
        [DataMember]
        public ValueDetails Value { get; set; }
        [DataMember]
        public ValueDetails Value1 { get; set; }
        [DataMember]
        public ValueDetails Value2 { get; set; }
        [DataMember]
        public ValueDetails Value3 { get; set; }
        [DataMember]
        public ValueDetails Value4 { get; set; }
        [DataMember]
        public ValueDetails Value5 { get; set; }
        [DataMember]
        public ValueDetails Value6 { get; set; }
        [DataMember]
        public ValueDetails Value7 { get; set; }
        [DataMember]
        public ValueDetails Value8 { get; set; }
        [DataMember]
        public ValueDetails Value9 { get; set; }
        [DataMember]
        public ValueDetails Value10 { get; set; }


    }

    [DataContract(Namespace = "")]
    public class ValueDetails
    {
        [DataMember]
        public string Header { get; set; }
        [DataMember]
        public string Row { get; set; }
    }

    [DataContract(Namespace = "")]
    public class ObjectFieldOperator
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}
