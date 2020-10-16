 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityUpdateServices
{ 
   public partial class UserDefinedReportUpdateService 
   {
        protected override void UpdateComposition(UserDefinedReportPM entityPM)
        {
            CalculatedChartsOfAccountUpdateService _CalculatedChartsOfAccountUpdateService = new CalculatedChartsOfAccountUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            _CalculatedChartsOfAccountUpdateService.UpdateMulti(entityPM.CalculatedChartsOfAccounts, entityPM.DeletedCalculatedChartsOfAccounts, entityPM, false);
        }

        protected override void OnCreating(UserDefinedReportPM entityPM, EntityPM entityParentPM)
        {

 
        }
        protected override void OnUpdating(UserDefinedReportPM entityPM, UserDefinedReport entityPOCO)
        { 
        
        
        
        }



    }

}
	 