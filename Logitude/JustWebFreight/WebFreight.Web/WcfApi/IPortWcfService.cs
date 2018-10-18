using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.EntityLists;
using WebFreight.Web.DataContracts;
namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPortHypredService" in both code and config file together.
    [ServiceContract]
    public interface IPortWcfService
    {
        [OperationContract]
        Response Upsert(PortPM entityPM,bool batch);

        [OperationContract]
        List<PortList> GetList(ApiSearchFilters filters, int tenant, ref Response response);

        [OperationContract]
        string GetPortId(PortApiFilters filters, int tenant, ref Response response);
    }
}
