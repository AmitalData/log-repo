using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data; 
using Logitude.TariffModule.Data.EntityMapping;

namespace Logitude.TariffModule.Data
{

    public interface ITariffModuleContext : IContext
    {
   
       	 IDbSet<Tariff> Tariffs { get; }
		 IDbSet<TariffLine> TariffLines { get; }
		 IDbSet<TariffLinesContainersPrice> TariffLinesContainersPrices { get; }
		 IDbSet<TariffSetting> TariffSettings { get; }
		 IDbSet<TariffSurchargesUpdate> TariffSurchargesUpdates { get; }
		 IDbSet<TariffSurchargesUpdateMethod> TariffSurchargesUpdateMethods { get; }
		 IDbSet<TariffType> TariffTypes { get; }
		 IDbSet<TariffVersion> TariffVersions { get; }
		 IDbSet<TariffVersionAllInCharge> TariffVersionAllInCharges { get; }
	 
         void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}