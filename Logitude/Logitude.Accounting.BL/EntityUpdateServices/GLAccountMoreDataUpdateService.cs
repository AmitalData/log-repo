 
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
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
using Logitude.Accounting.BL.EntityQueryServices;

namespace Logitude.Accounting.BL.EntityUpdateServices
{ 
   public partial class GLAccountMoreDataUpdateService
    {
        protected override void OnCreating(GLAccountMoreDataPM entityPM, EntityPM entityParentPM)
        {
           

        }

        protected override void OnUpdating(GLAccountMoreDataPM entityPM, GLAccountMoreData entityPOCO)
        {
			if (currentContext == null) { 
               currentContext = AccountingContext.GetContext(entityPM.Tenant);
            }
            CashBookQueryService cashBookQueryService = new CashBookQueryService(entityPM.Tenant);
            if (!string.IsNullOrEmpty(entityPM.AccountId)) { 
                var cashBook=  cashBookQueryService.GetCashbookByAccountId(entityPM.AccountId,entityPM.Tenant);
                
			    if (cashBook !=null && cashBook.CashBookTypeCode == "1") {
                    CashBookUpdateService cashBookUpdateService = new CashBookUpdateService(currentContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                    cashBook.TotalAmount = entityPM.BalanceInForeignCurrency;
                    cashBook.ChangeSetOp = ChangeSetOperation.Update;
                    cashBookUpdateService.Update(cashBook, true);
                }
            }
        }

       

    }
}
	 