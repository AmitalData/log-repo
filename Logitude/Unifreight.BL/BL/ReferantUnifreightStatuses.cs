using Logitude.Server.Tools;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Unifreight.Data.AmitalModel.Repsitories;


namespace Unifreight.BL.BL
{
    public class ReferantUnifreightStatuses
    {
        List<AmitalContext> _AmitalContextList = new List<AmitalContext>();
        AmitalContext GetAmitalContext(int tenant)
        {
            var tenantAmitalContext = _AmitalContextList.FirstOrDefault(rec => rec.TenantSeed == tenant);
            if (tenantAmitalContext == null)
            {

                tenantAmitalContext = AmitalContext.GetContext(tenant);
                _AmitalContextList.Add(tenantAmitalContext);
            }
            return tenantAmitalContext;
        }
        public List<string> GetItemsList(int tenant)
        {
            var _GTBFUSTATUQueryService = new GTBFUSTATUQueryService(GetAmitalContext(tenant));
            var list = _GTBFUSTATUQueryService.GetAll();
            return list.Select(item=>item.STATUSCODE).ToList();
        }

        public IQueryable<GTBFUSTATU> GetAllGTBFUSTATU(int tenant)
        {
            var _GTBFUSTATURepository = new GTBFUSTATURepository(GetAmitalContext(tenant));
            return _GTBFUSTATURepository.GetAll();
        }

        

    }
}
