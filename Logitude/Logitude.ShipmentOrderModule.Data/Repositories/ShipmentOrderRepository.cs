 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.ShipmentOrderModule.Data.Repositories
{
   public partial class ShipmentOrderRepository:IRepository<ShipmentOrder>
   {
        
		public List<ShipmentOrder> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public string GetIdByCode(string orderNumber, int tenant)
        {
            return (from a in context.ShipmentOrders where a.OrderNumber == orderNumber && a.Tenant == tenant select a.Id).FirstOrDefault();
        }

        public List<string> GetShipmentOrdersIdsByTenant(int tenant, int skip, int take, DateTime minStartDate)
        {
            List<string> Orders = (from a in context.ShipmentOrders
                                          where a.Tenant == tenant && a.AutomaticLastUpdateDate >= minStartDate
                                   select a.Id).OrderBy(d => d).Skip(skip).Take(take).ToList();
            return Orders;
        }
        public void UpdateLastUpdateDate(string OrderIds)
        {
            var query = $"update ShipmentOrders set AutomaticLastUpdateDate = GETDATE() where id in ({OrderIds}) ";
            context.GetActiveDbContext().Database.ExecuteSqlCommand(query);
        }

    }

}
   