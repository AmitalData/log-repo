using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class DWObjectFieldsDetails
    {
        public List<DWObjectFieldsDetails> FilterItems { get; set; }
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public int IndexOrder { get; set; }
        //public bool IsGroup { get; set; }
        public bool IsPrimaryKey { get; set; }
        //public bool ShowBtns { get; set; }
        //public bool IsViewTree { get; set; }
        //public bool HasTree { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Code { get; set; }
        //public string ParentCode { get; set; }

        public string ParentDimTabelName { get; set; }
        public string DWObjectTableCode { get; set; }
        public bool IsMeasurement { get; set; }
        public string AggregationTypeCode { get; set; }
        public string DataTypeCode { get; set; }
        public string ParentDataTypeCode { get; set; }
        public string DimensionTableCode { get; set; }
        //public List<ObjectFieldOperator> Operators { get; set; }
        public object TextValue { get; set; }
        public ObjectFieldOperator Operation { get; set; }
        public string OperationCode { get; set; }
        public string OperationName { get; set; }
        public string AndOr { get; set; }
        public bool IsSetDefaults { get; set; }
        public bool IsMandatoryFilter { get; set; }
        public string FilterType { get; set; }
        

        [DataMember]
        public List<MultSelectValue> MultSelectValueLists{ get; set; }

    }

    public class ObjectFieldOperator
    {
        
        public string Code { get; set; }
     
        public string Name { get; set; }
    }
    [DataContract(Namespace = "")]
    public class MultSelectValue
    {
        [DataMember]
        public FieldDetails Value { get; set; }
        [DataMember]
        public FieldDetails Value1 { get; set; }
        [DataMember]
        public FieldDetails Value2 { get; set; }
        [DataMember]
        public FieldDetails Value3 { get; set; }
        [DataMember]
        public FieldDetails Value4 { get; set; }
        [DataMember]
        public FieldDetails Value5 { get; set; }
        [DataMember]
        public FieldDetails Value6 { get; set; }
        [DataMember]
        public FieldDetails Value7 { get; set; }
        [DataMember]
        public FieldDetails Value8 { get; set; }
        [DataMember]
        public FieldDetails Value9 { get; set; }
        [DataMember]
        public FieldDetails Value10 { get; set; }


    }

    [DataContract(Namespace = "")]
    public class FieldDetails
    {
        [DataMember]
        public string Column { get; set; }
        [DataMember]
        public string Row { get; set; }
    }

}