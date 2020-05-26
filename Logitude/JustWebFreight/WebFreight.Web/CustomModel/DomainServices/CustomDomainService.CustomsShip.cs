
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public CustomsShipPM GetSingleCustomsShipPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsShipQueryService = new CustomsShipQueryService(customContext);
            CustomsShipPM CustomsShip = customsShipQueryService.GetSingle(id, false, false);
            return CustomsShip;
        }

        public CustomsShipList GetSingleCustomsShipList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.CustomsShip", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsShipListQueryService listService = new CustomsShipListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomsShipList> GetCustomsShipLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.CustomsShip", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsShipListQueryService listService = new CustomsShipListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsShipList> GetCustomsShipFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.CustomsShip", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomsShipListQueryService listService = new CustomsShipListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsShipFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.CustomsShip", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsShipListQueryService queryService = new CustomsShipListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}