using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Core.Objects;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Data.Entity;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel
{
    public interface ILogBoxShipmentDataViewContext : IContext
    {
        IDbSet<LogBoxShipmentDataView> LogBoxShipmentDataView { get; }
        void SetAsModified(object entity);
        void DetectChanges();
        int SaveChanges();
    }
}
