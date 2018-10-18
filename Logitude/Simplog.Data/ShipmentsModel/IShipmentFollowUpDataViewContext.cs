using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Core.Objects;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Data.Entity;

namespace Simplog.Data.ShipmentsModel
{
   public interface IShipmentFollowUpDataViewContext
    {
       IDbSet<ShipmentFollowUpDataView> ShipmentFollowUpDataViews { get; }
        void SetAsModified(object entity);
        void DetectChanges();
        int SaveChanges();
    }
}
