using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.Helpers.ClosedTable
{
    public class ManipulateCustomResponse
    {
        public static void DataSetToTableData(SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse, Action<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData, DataRow> OverrideDefault)
        {
            var myTableData = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData>();
            if (String.IsNullOrWhiteSpace(customResponse.TableAsDataSetTableData))
            {
                throw new System.Exception("DataSetToTableData()  TableAsDataSetTableData is null");
            }
            var ds = SystemTables.DataSetReadXML(customResponse.TableAsDataSetTableData);
            var dt = ds.Tables[0];
            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData newResponseTableData = null;
            LogMessagingUtil.Instance.AppendLine("DataSet To TableData ... ");
            foreach (System.Data.DataRow dr in dt.Rows)
            {

                newResponseTableData = new SYSTBL_NG_9001_MSG_SystemTablesResponseTableData()
                {

                    id = dr["ID"].ToString(),
                    name = dr["Name"].ToString()

                };
                newResponseTableData.state = int.Parse(dr["State"].ToString());
                newResponseTableData.malamID = int.Parse(dr["MalamID"].ToString());
                if (dt.Columns.Contains("ExtraNumericData"))//20190306 sadenly MECHES dont send for table 1144 ?!?!- let it be ..
                {
                    string sExtraNumericData = dr["ExtraNumericData"].ToString();
                    if (!string.IsNullOrWhiteSpace(sExtraNumericData))
                    {
                        newResponseTableData.extraNumericData = int.Parse(sExtraNumericData);
                    }
                }
                
                
                OverrideDefault
                (newResponseTableData, dr);
                myTableData.Add(newResponseTableData);
            }
            customResponse.TableData = myTableData.ToArray();
        }



    }
}
