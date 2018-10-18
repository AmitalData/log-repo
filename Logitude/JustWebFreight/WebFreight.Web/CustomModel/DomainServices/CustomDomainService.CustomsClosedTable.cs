using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.Helpers;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public CustomsClosedTablePM GetSingleCustomsClosedTablePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsClosedTableQuery = new CustomsClosedTableQueryService(customContext);
            CustomsClosedTablePM CustomsClosedTable = customsClosedTableQuery.GetSingle(id, false, false);
            return CustomsClosedTable;
        }

        public CustomsClosedTableList GetSingleCustomsClosedTableList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsClosedTableListQueryService listService = new CustomsClosedTableListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomsClosedTableList> GetCustomsClosedTableLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsClosedTableListQueryService listService = new CustomsClosedTableListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsClosedTableList> GetCustomsClosedTableFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomsClosedTableListQueryService listService = new CustomsClosedTableListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsClosedTableFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsClosedTableListQueryService queryService = new CustomsClosedTableListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

        public void UpdateCustomsClosedTable(CustomsClosedTableList currentEntity)
        {
 
        }

        public List<CustomsClosedTablePM> GetCustomsClosedTablePMs(int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsClosedTableQuery = new CustomsClosedTableQueryService(customContext);
            List<CustomsClosedTablePM> CustomsClosedTables = customsClosedTableQuery.GetCustomsClosedTables(tenant);
            return CustomsClosedTables;
        }
    }
}