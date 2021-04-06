using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAccountingPartnerWcfService" in both code and config file together.
    [ServiceContract]
    public interface IAccountingPartnerWcfService
    {
        [OperationContract]
        Response Upsert(AccountingPartnerPM entityPM, bool batch);

        [OperationContract]
        AccountingPartnerPM GetAccountingPartnerPM(AccountingPartnerApiFilters filters, int tenant, ref Response response);
    }
}
