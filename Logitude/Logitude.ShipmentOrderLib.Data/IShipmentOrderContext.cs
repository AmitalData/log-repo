using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Logitude.ShipmentOrderLib.Data.EntityPOCOs;
using Logitude.ShipmentOrderLib.Data; 
using Logitude.ShipmentOrderLib.Data.EntityMapping;

namespace Logitude.ShipmentOrderLib.Data
{

    public interface IShipmentOrderContext : IContext
    {
   
       	 IDbSet<ShipmentOrder> ShipmentOrders { get; }
	 
         void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}