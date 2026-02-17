using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICityWcfService" in both code and config file together.
    [ServiceContract]
    public interface ICityWcfService
    {
        [OperationContract]
        Response Upsert(CountryCityPM entityPM, bool batch);

        [OperationContract]
        CountryCityList GetCityListByCode(string code, string countryCode, int tenant, ref Response response);
    }
}
