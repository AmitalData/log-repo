using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITruckerWcfService" in both code and config file together.
    [ServiceContract]
    public interface ITruckerWcfService
    {
        [OperationContract]
        Response Upsert(TruckerPM entityPM, bool batch);
        [OperationContract]
        TruckerPM GetTruckerPM(string code, int tenant, ref Response response);
    }
}
