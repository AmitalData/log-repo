using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWarehouseWcfService" in both code and config file together.
    [ServiceContract]
    public interface IWarehouseWcfService
    {
        [OperationContract]
        Response Upsert(WarehousePM entityPM, bool batch);
        [OperationContract]
        WarehousePM GetWarehousePM(string code, int tenant, ref Response response);
    }
}
