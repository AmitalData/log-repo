using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

using System.ServiceModel;

using Logitude.Customs.Def.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace WebFreight.Web.WcfApi
{
    [ServiceContract]
    public interface IBusinessHourWcfService
    {
        [OperationContract]
        Response Upsert(BusinessHourPM entityPM, bool batch);

    }
}
