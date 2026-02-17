
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
   public partial class WarehouseEntryRepository:IRepository<WarehouseEntry>
   {        
		public List<WarehouseEntry> GetMulti(EntityKeyFields entityKeys)
        {            
			throw new NotImplementedException();
        }

        public IQueryable<WarehouseEntry> GetWarehouseEntriesByshipmentId(string shipmentId, int tenant)
        {
            IQueryable<WarehouseEntry> myResult = (from a in context.WarehouseEntries
                                                 where a.ShipmentId == shipmentId && a.Tenant == tenant
                                                 select a);
            return myResult;
        }

  

        public List<WarehouseEntry> GetWarehouseEntriesFromIdList(List<string> ids, int tenant)
        {

            List<WarehouseEntry> myResult = (from a in context.WarehouseEntries.Include("WarehouseEntryStatus")
                                             where a.Tenant == tenant && ids.Contains(a.Id)
                                                   select a).ToList();
            return myResult;
        }



    }
}
   