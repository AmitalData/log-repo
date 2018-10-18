using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Customs.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public TapagConnectionTablePM GetSingleTapagConnectionTablePM(string tapagId, string declarationId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            tapagConnectionTableQuery = new TapagConnectionTableQueryService(customContext);
            TapagConnectionTablePM TapagConnectionTable = tapagConnectionTableQuery.GetSingle(tapagId,declarationId, true, false);
            return TapagConnectionTable;
        }


        public string  GetTapagConnectionTableRequestFileNumberPM(string tapagId, string declarationId,  int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            tapagConnectionTableQuery = new TapagConnectionTableQueryService(customContext);
            TapagConnectionTablePM TapagConnectionTable = tapagConnectionTableQuery.GetSingle(tapagId, declarationId, false, false);

            return TapagConnectionTable != null ? TapagConnectionTable.RequestFileNumber : null;
        }

        public TapagConnectionTableList GetSingleTapagConnectionTableList(string tapagId, string declarationId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            TapagConnectionTableListQueryService listService = new TapagConnectionTableListQueryService(customContext);
            return listService.GetSingle( tapagId, declarationId);
        }

        public List<TapagConnectionTableList> GetTapagConnectionTableLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            TapagConnectionTableListQueryService listService = new TapagConnectionTableListQueryService(customContext);
            return listService.GetList(tenant);
        }





     
      

    }
}