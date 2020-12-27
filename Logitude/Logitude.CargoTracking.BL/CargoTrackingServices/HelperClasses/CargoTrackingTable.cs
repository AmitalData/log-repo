using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class CargoTrackingTable
    {
        public string DBTableName { get; set; }
        public string CargoTracking_TableName { get; set; }
        public string CargoTracking_InnerTableName { get; set; }
        public string Pre_TableName { get; set; }
        public string Pre_InnerTableName { get; set; }
        public string KeyName { get; set; }
        public string InnerKeyName { get; set; }
        public string ConditionKey { get; set; }
        public string InnerConditionKey { get; set; }
        public string FieldsDBName { get; set; }
        public string FieldsDummyName { get; set; }
        public string CargoTracking_FieldsDBName { get; set; }
        public string InnerCargoTracking_FieldsDBName { get; set; }
        public string RefreshIds { get; set; }
        public string InnerRefreshIds { get; set; }
        public bool IsUpdated { get; set; }
        public int UpdatedCount { get; set; }
        public bool IsClosedTable { get; set; }
        public int ConditionsNumber { get; set; }
        public int CurrentCondition { get; set; }
        public string Main_CargoTracking_TableName { get; set; }
        public string Main_CargoTracking_InnerTableName { get; set; }
        public string ObjectTableName { get; set; }
        public string InnerObjectTableName { get; set; }
        public TableStructureHelper MainTableStructureHelper { get; set; }
        public TableStructureHelper InnerTableStructureHelper { get; set; }
        public string TableStructure { get; set; }
        public string InnerTableStructure { get; set; }

        public List<object> Labels { get; set; }


        public CargoTrackingTable(string FileName,string InnerFileName=null)
        {
            SetMainTableFields(FileName);
            SetInnerTableFields(InnerFileName);
        }


        private void SetMainTableFields(string FileName)
        {
            MainTableStructureHelper = new TableStructureHelper(ServiceHelper.GetInvokeDBTableByTableName(FileName, "Get" + FileName + "Dxml"));
            CargoTracking_TableName = MainTableStructureHelper.GetTableName();
            Pre_TableName = "Pre_" + CargoTracking_TableName;
            TableStructure = MainTableStructureHelper.GetTableStructure(Pre_TableName);
            KeyName = MainTableStructureHelper.primarykeyColumn;
            CargoTracking_FieldsDBName = string.Join(",", MainTableStructureHelper.TableCoulmnsNameWithoutIDentity.ToArray());
            FieldsDBName = ServiceHelper.GetInvokeClassWithMethode("Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure." + FileName + "TableStructure", "GetColumnsForCopy");
            FieldsDummyName = ServiceHelper.GetInvokeClassWithMethode("Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure." + FileName + "TableStructure", "GetDummyColumnsForCopy");
            Main_CargoTracking_TableName = CargoTracking_TableName;
            ObjectTableName = FileName;
        }


        private void SetInnerTableFields(string InnerFileName)
        {
            if (!string.IsNullOrEmpty(InnerFileName))
            {
                InnerTableStructureHelper = new TableStructureHelper(ServiceHelper.GetInvokeDBTableByTableName(InnerFileName, "Get" + InnerFileName + "Dxml"));
                CargoTracking_InnerTableName = InnerTableStructureHelper.GetTableName();
                Pre_InnerTableName = "Pre_" + CargoTracking_InnerTableName;
                InnerTableStructure = InnerTableStructureHelper.GetTableStructure(Pre_InnerTableName);
                InnerKeyName = InnerTableStructureHelper.primarykeyColumn;
                InnerCargoTracking_FieldsDBName = string.Join(",", InnerTableStructureHelper.TableCoulmnsNameWithoutIDentity.ToArray());
                Main_CargoTracking_InnerTableName = CargoTracking_InnerTableName;
                InnerObjectTableName = InnerFileName;
            }
        }
    }
}
