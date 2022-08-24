using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data; 
using Logitude.DashboardModule.Data.EntityMapping;

namespace Logitude.DashboardModule.Data
{

    public interface IDashboardContext : IContext
    {
   
       	 IDbSet<AnalyticsFactsFieldsMetaData> AnalyticsFactsFieldsMetaDatas { get; }
		 IDbSet<AnalyticsFactsMetaData> AnalyticsFactsMetaDatas { get; }
	 
         void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}