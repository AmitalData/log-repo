using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using WebFreight.Web.Security;

namespace WebFreight.Web.AccountingModel.DomainServices
{
    public partial class AccountingDomainService
    {
        //[Invoke]
        //public List<ReconciliationLineList> ReconciliationLineListsById(string id, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    accountingContext = AccountingContext.GetContext(tenant);
        //    ReconciliationLineListQueryService listService = new ReconciliationLineListQueryService(accountingContext);
        //    List<ReconciliationLineList> rvList = listService.GetReconciliationLineListsById(id, tenant);
        //    return rvList;
        //}

    }
}