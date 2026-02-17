using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Security;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public IQueryable<SharedLogisticsUpdateStatus> GetSharedLogisticsUpdateStatus(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            sharedLogisticsUpdateStatusRepository = new SharedLogisticsUpdateStatusRepository(tenant);
            return sharedLogisticsUpdateStatusRepository.GetSharedLogisticsUpdateStatus();
        }

        public IQueryable<SharedLogisticsUpdateStatus> GetSharedLogisticsUpdateStatusByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            sharedLogisticsUpdateStatusRepository = new SharedLogisticsUpdateStatusRepository(tenant);
            return sharedLogisticsUpdateStatusRepository.GetSharedLogisticsUpdateStatus();
        }

        public void InsertSharedLogisticsUpdateStatus(SharedLogisticsUpdateStatus sharedLogisticsUpdateStatus)
        {
            sharedLogisticsUpdateStatusRepository.Add(sharedLogisticsUpdateStatus);
        }

        public void UpdateSharedLogisticsUpdateStatus(SharedLogisticsUpdateStatus currentSharedLogisticsUpdateStatus)
        {
            sharedLogisticsUpdateStatusRepository.Update(currentSharedLogisticsUpdateStatus);
        }

        public void DeleteSharedLogisticsUpdateStatus(SharedLogisticsUpdateStatus sharedLogisticsUpdateStatus)
        {
            sharedLogisticsUpdateStatusRepository.Remove(sharedLogisticsUpdateStatus);
        }
    }
}