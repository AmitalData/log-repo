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
		 IDbSet<Dashboard> Dashboards { get; }
		 IDbSet<DashboardSharedUser> DashboardSharedUsers { get; }
		 IDbSet<MeasureType> MeasureTypes { get; }
		 IDbSet<PermissionLevel> PermissionLevels { get; }
		 IDbSet<Widget> Widgets { get; }
		 IDbSet<WidgetMeasure> WidgetMeasures { get; }
		 IDbSet<WidgetType> WidgetTypes { get; }
	 
         void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}