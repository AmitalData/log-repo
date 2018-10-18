using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICustomBankCardWcfService" in both code and config file together.
    [ServiceContract]
    public interface ICustomBankCardWcfService
    {
        [OperationContract]
        Response Upsert(CustomBankPM entityPM, bool batch);

        [OperationContract]
        Response Delete(string bankId, string cardId, int tenant);
    }
}
