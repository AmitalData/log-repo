 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
 
namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class CalculatedChartsOfAccountQueryService 
   {

        public override void GetComposition(EntityKeyFields entityKeys, CalculatedChartsOfAccountPM entityPM)
        {
            IAccountingContext context = MainContext as IAccountingContext;
            CalculatedChartsOfAccountKeys activityKeys = entityKeys as CalculatedChartsOfAccountKeys;
            CalculatedChartsOfAccountsLineQueryService queryService = new CalculatedChartsOfAccountsLineQueryService(context);
            entityPM.CalculatedChartsOfAccountLines = queryService.GetMulti(activityKeys, true);
        }
    }
   
}
	 