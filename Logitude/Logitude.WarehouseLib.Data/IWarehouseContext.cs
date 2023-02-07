using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data; 
using Logitude.WarehouseLib.Data.EntityMapping;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.WarehouseLib.Data
{

    public interface IWarehouseContext : IContext
    {
   
       	 IDbSet<WarehouseEntry> WarehouseEntries { get; }
		 IDbSet<WarehouseEntryPackage> WarehouseEntryPackages { get; }
		 IDbSet<WarehouseEntryPackagesRelease> WarehouseEntryPackagesReleases { get; }
		 IDbSet<WarehouseEntryStatus> WarehouseEntryStatuses { get; }
		 IDbSet<WarehouseRelease> WarehouseReleases { get; }
		 IDbSet<WarehouseReleasePackage> WarehouseReleasePackages { get; }
		 IDbSet<WarehouseReleaseStatus> WarehouseReleaseStatuses { get; }
        IDbSet<CustomFieldsMainObject> CustomFieldsMainObjects { get; }

        void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}