using Logitude.Server.Tools.AnalyticTableServices;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Data.Entity;

namespace Logitude.BL.AnalyticTableServices
{
    public class ContainerAnalyticTableService : AnalyticTableService<Container, ContainerAnalytic>
    {
        public ContainerAnalyticTableService(DbContext context) : base(context)
        {

        }

        protected override void CustomMap(Container entity, ContainerAnalytic analyticTable)
        {

        }
    }
}
