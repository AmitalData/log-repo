using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data
{
    public interface IProceduralFaultsDeclarationsViewContext : IContext
    {
        IDbSet<ProceduralFaultDeclarationView> ProceduralFaultDeclarationViews { get; }
        void SetAsModified(object entity);
        void DetectChanges();
        int SaveChanges();


    }
}
