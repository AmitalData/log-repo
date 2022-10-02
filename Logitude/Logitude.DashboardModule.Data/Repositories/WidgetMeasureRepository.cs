 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.DashboardModule.Data.Repositories
{
   public partial class WidgetMeasureRepository:IRepository<WidgetMeasure>
   {        
		public List<WidgetMeasure> GetMulti(EntityKeyFields entityKeys)
        {
            WidgetKeys myEntityKeys = entityKeys as WidgetKeys;
            return (from a in context.WidgetMeasures where a.WidgetId == myEntityKeys.Id select a).ToList();
        }


        public List<WidgetMeasure> GetWidgetMeasuresByWidgetId(string widgetId, int tenant)
        {
            return (from a in context.WidgetMeasures where a.Tenant == tenant && a.WidgetId == widgetId select a).ToList();
        }
    }
}
   