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
        public string KeyName { get; set; }
        public string ConditionKey { get; set; }
        public string FieldsDBName { get; set; }
        public string CT_FieldsDBName { get; set; }
        public string RefreshIds { get; set; }
        public bool IsUpdated { get; set; }
        public int UpdatedCount { get; set; }
        public List<object> Labels { get; set; }
    }
}
