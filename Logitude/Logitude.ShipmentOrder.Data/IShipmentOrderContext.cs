using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Logitude.ShipmentOrder.Data.EntityPOCOs;

namespace Logitude.ShipmentOrder.Data
{

    public interface IShipmentOrderContext : IContext
    {
   
        
         void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}