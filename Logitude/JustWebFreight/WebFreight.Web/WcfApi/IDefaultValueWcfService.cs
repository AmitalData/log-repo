using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;

using System.ServiceModel;

using Logitude.Customs.Def.EntityPMs;

namespace WebFreight.Web.WcfApi
{
    [ServiceContract]
    public interface IDefaultValueWcfService
    {
        [OperationContract]
        Response Upsert(DefaultValuePM entityPM, bool batch);

    }
}
