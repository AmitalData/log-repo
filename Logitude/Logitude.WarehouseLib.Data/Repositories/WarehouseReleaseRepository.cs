
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

        public IQueryable<WarehouseRelease> GetActiveWarehouseReleasesByshipmentId(string shipmentId, int tenant)
        {
            IQueryable<WarehouseRelease> myResult = (from a in context.WarehouseReleases 
                                                     where a.ShipmentId == shipmentId && a.Tenant == tenant && a.StatusCode != "CARE"
                                                     select a);
            IQueryable<WarehouseRelease>  result = myResult.OrderByDescending(r => r.ActualReleaseDate);
            return result;
        }


        public WarehouseRelease GetWarehouseReleasesByReleaseNumberAndShipmentId(string releaseNumber,string shipmentId, int tenant)
        {
            WarehouseRelease myResult = (from a in context.WarehouseReleases
                                                     where a.ReleaseNumber == releaseNumber && a.Tenant == tenant && a.ShipmentId == shipmentId
                                         select a).FirstOrDefault();
            return myResult;
        }




        public List<WarehouseRelease> GetWarehouseReleasesFromIdList(List<string> ids, int tenant)
        {

            List<WarehouseRelease> myResult = (from a in context.WarehouseReleases.Include("WarehouseReleaseStatus").Include("Direction").Include("TransportMode").Include("ToAddress.Country").Include("ToAddressCountry")
                                             where a.Tenant == tenant && ids.Contains(a.Id)
                                             select a).ToList();
            return myResult;
        }

        public int GetNumberofConnectedWarehouseReleasesByChildEntityReference(string childEntityReference, int tenant)
        {
            return (from a in context.WarehouseReleases
                    where a.Tenant == tenant && a.StatusCode != "CARE" && a.ChildEntityReference == childEntityReference
                    select a).Count();
        }



    }
}
   