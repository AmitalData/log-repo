using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel
{
    public interface IAWBStocksDataViewContext : IContext
    {
        IDbSet<AWBStocksDataView> AWBStocksDataViews { get; }
        void SetAsModified(object entity);
        void DetectChanges();
        int SaveChanges();
    }
}