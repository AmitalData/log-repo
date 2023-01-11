
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Logitude.Customs.Data.DataContracts;
using Logitude.Customs.BL.BL;

namespace Logitude.Customs.Data.Repsitories
{
    public partial class InterfaceManagementRepository : IRepository<InterfaceManagement>
    {

        public List<InterfaceManagement> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public InterfaceManagement GetSingleInterfaceManagement(EntityKeyFields entityKeys)
        {
            InterfaceManagementKeys keys = entityKeys as InterfaceManagementKeys;
            return (from a in context.InterfaceManagements.Include("InterfaceSendOption")
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }

        public List<InterfaceManagement> GetAllFromCache() => CacheHelper.GetFromCache("InterfaceManagementGetAll", ()=> GetAll().ToList());

        public List<CustomsRequestsSheetSummary> GetQueueMessagesSatistic(int tenant, bool includingFuture)
        {
            List<InterfaceManagement> interfaceManagements = GetAllFromCache();

            var webFreightContext = WebFreightContext.GetContext(tenant);
            
            var summry = (from qm in webFreightContext.QueueMessages.AsEnumerable()

                    where qm.Tenant == tenant
                    && (includingFuture || qm.NextRunDateTime < DateTime.Now)
                    && qm.InterfaceTypeCode != null

                    group qm by qm.InterfaceTypeCode into g
                    select new CustomsRequestsSheetSummary(
                        g.Key,
                        g.Count()
                    )).ToList();

            summry.ForEach(qm => qm.InterfaceTypeName = interfaceManagements.Find(im => im.Code == qm.InterfaceTypeName).Description);

            return summry;
        }
    }
}
