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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICardContactWcfService" in both code and config file together.
    [ServiceContract]
    public interface ICardContactWcfService
    {
        [OperationContract]
        Response Upsert(CardContactPM entityPM, bool batch);

        [OperationContract]
        Response Delete(string contactExternalId,string cardCode,int tenant, bool batch);

        [OperationContract]
        CardContactPM GetCardContactPM(string contactExternalId, string cardCode, int tenant, ref Response response);
    }
}
