using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel
{
    public interface ICustomersDataViewContext : IContext
    {
        IDbSet<CustomersDataView> CustomersDataViews { get; }
        void SetAsModified(object entity);
        void DetectChanges();
        int SaveChanges();
    }
}