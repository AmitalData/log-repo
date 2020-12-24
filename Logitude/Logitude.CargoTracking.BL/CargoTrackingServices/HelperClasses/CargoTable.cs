using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class CargoTable
    {
        public string TableName { get; set; }
        public string DBTableName { get; set; }
        public string CT_TableName { get; set; }
        public string CT2_TableName { get; set; }

        public string Pre_TableName { get; set; }
        public string Pre2_TableName { get; set; }

        public string KeyName { get; set; }
        public string KeyName2 { get; set; }
        public string ConditionKey { get; set; }
        public string ConditionKey2 { get; set; }
        public string FieldsDBName { get; set; }
        public string FieldsDummyName { get; set; }
        public string CT_FieldsDBName { get; set; }
        public string CT2_FieldsDBName { get; set; }
        public string RefreshIds { get; set; }
        public string RefreshIds2 { get; set; }

        public bool IsUpdated { get; set; }
        public int UpdatedCount { get; set; }
        public string Condition1 { get; set; }
        public string Condition2 { get; set; }
        public string Condition3 { get; set; }
        public bool IsClosedTable { get; set; }
        public int ConditionsNumber { get; set; }
        public int CurrentCondition { get; set; }
        public string Main_CT_TableName { get; set; }
        public string Main_CT2_TableName { get; set; }
        public string ObjectTableName { get; set; }
        public string InnerObjectTableName { get; set; }

        public List<object> Labels { get; set; }
    }
}
