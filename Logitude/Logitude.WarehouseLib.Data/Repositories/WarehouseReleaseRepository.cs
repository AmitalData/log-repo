
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.WarehouseLib.Data.Repositories
{
   public partial class WarehouseReleaseRepository:IRepository<WarehouseRelease>
   {        
		public List<WarehouseRelease> GetMulti(EntityKeyFields entityKeys)
        {            
			throw new NotImplementedException();
        }

        public IQueryable<WarehouseRelease> GetWarehouseReleasesByshipmentId(string shipmentId, int tenant)
        {
            IQueryable<WarehouseRelease> myResult = (from a in context.WarehouseReleases
                                                     where a.ShipmentId == shipmentId && a.Tenant == tenant
                                                   select a);
            return myResult;
        }


        public List<WarehouseRelease> GetWarehouseReleasesFromIdList(List<string> ids, int tenant)
        {

            List<WarehouseRelease> myResult = (from a in context.WarehouseReleases.Include("WarehouseReleaseStatus")
                                             where a.Tenant == tenant && ids.Contains(a.Id)
                                             select a).ToList();
            return myResult;
        }



    }
}
   