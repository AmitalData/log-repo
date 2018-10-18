using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public DeliverySiteTypePM GetSingleDeliverySiteTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            deliverySiteTypeQuery = new DeliverySiteTypeQueryService(customContext);
            DeliverySiteTypePM DeliverySiteType = deliverySiteTypeQuery.GetSingle(id, false, false);
            return DeliverySiteType;
        }

        public DeliverySiteTypeList GetSingleDeliverySiteTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.DeliverySiteType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DeliverySiteTypeListQueryService listService = new DeliverySiteTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<DeliverySiteTypeList> GetDeliverySiteTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.DeliverySiteType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DeliverySiteTypeListQueryService listService = new DeliverySiteTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DeliverySiteTypeList> GetDeliverySiteTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.DeliverySiteType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            DeliverySiteTypeListQueryService listService = new DeliverySiteTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetDeliverySiteTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.DeliverySiteType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DeliverySiteTypeListQueryService queryService = new DeliverySiteTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }



    }
}