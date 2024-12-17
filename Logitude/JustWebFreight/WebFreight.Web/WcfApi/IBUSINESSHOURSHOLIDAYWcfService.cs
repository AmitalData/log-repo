using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

using System.ServiceModel;

using Logitude.Customs.Def.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace WebFreight.Web.WcfApi
{
    [ServiceContract]
    public interface IBusinessHoursHolidayWcfService
    {
        [OperationContract]
        Response Upsert(BusinessHoursHolidayPM entityPM, bool batch);

    }
}
