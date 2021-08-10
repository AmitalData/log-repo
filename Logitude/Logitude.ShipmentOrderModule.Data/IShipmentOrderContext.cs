using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.Data; 
using Logitude.ShipmentOrderModule.Data.EntityMapping;

namespace Logitude.ShipmentOrderModule.Data
{

    public interface IShipmentOrderContext : IContext
    {
   
       	 IDbSet<ShipmentOrder> ShipmentOrders { get; }
	 
         void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}