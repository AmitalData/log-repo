using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{


    public class SYSTBL_NG_9000_MSG_SystemTableRequestService
        : RequestServiceBase<SYSTBL_NG_9000_MSG_SystemTableRequest, SystemTableRequestParams>
    {
        public override SYSTBL_NG_9000_MSG_SystemTableRequest GetRequest(SystemTableRequestParams requestParams)
        {



            var testCancell = false;
            if (testCancell)
            {
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
            if (!requestParams.Pseudo)
            {
                ICustomContext customContext = CustomContext.GetContext(0);
                CustomsClosedTableRepository closedTableRep = new CustomsClosedTableRepository(customContext);
                CustomsClosedTable table = closedTableRep.GetSingle(new CustomsClosedTableKeys() { Id = requestParams.TableId });
                ObjectTableRepository objectTableRepository = new ObjectTableRepository(0);

                table.StatusCode = "2";
                closedTableRep.Update(table);
                closedTableRep.SubmitChanges();

            }
            var req = new SYSTBL_NG_9000_MSG_SystemTableRequest();
            req.RequestContentHeader = new RequestContentHeader() { SenderID = 1, RecieverID = new int[] { 1 } };
            req.tableName = requestParams.TableId;

            req.SelectOptions = new SYSTBL_NG_9000_MSG_SystemTableRequestSelectOptions();
            var lAsTableData = new List<string>() { "1892", "1091", "1144", "1354", "1416","1998", "1366","1423"/*, "1344" */};
            if (//table.Id == "1892" 
                //lAsTableData.Contains(table.Id)
                lAsTableData.Contains(requestParams.TableId)
                )
            {
                requestParams.AsTableData = true;

            }
            if (req.tableName == "1344")
            {
                req.tableName = "2653";
            }
            if (requestParams.AsTableData)
            {
                req.SelectOptions.GetAsDataTable = true;
                req.SelectOptions.PageNumber = 1;
                req.SelectOptions.PageSize = 505;
            }

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "אחזור טבלה " + requestParams.TableId;


            return req;
        }
    }
}
