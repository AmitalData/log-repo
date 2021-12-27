using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices
{
    public class CargoSessionService
    {
        const int delayTime = -10;
        public double? GetSessionTimeOut(int tenant, string token)
        {
            var globalContext = GlobalContext.GetContext();
            var tenantManagement = globalContext.TenantManagements.Find(tenant);
            if (tenantManagement.CargoTokenTimeout == null)
                return null;
            AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(tenant);
            var authenticationToken = authenticationTokenRepository.GetSingleToken(token);
            var tokenLifeTime = GetTokenLifeTime(authenticationToken.CreateDate, tenantManagement.CargoTokenTimeout.Value);
            return tokenLifeTime;
        }

        private double GetTokenLifeTime(DateTime startDate, double timeout)
        {
            var endDate = startDate.AddHours(timeout).AddSeconds(delayTime);
            var deference = endDate - DateTime.Now;
            if (deference.TotalMilliseconds > 0)
                return deference.TotalMilliseconds;
            return 0;
        }
    }
}
