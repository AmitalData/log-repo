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
        public CargoIdentifireTypePM GetSingleCargoIdentifireTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            cargoIdentifireTypeQuery = new CargoIdentifireTypeQueryService(customContext);
            CargoIdentifireTypePM CargoIdentifireType = cargoIdentifireTypeQuery.GetSingle(id, false, false);
            return CargoIdentifireType;
        }

        public CargoIdentifireTypeList GetSingleCargoIdentifireTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.CargoIdentifireType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CargoIdentifireTypeListQueryService listService = new CargoIdentifireTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CargoIdentifireTypeList> GetCargoIdentifireTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.CargoIdentifireType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CargoIdentifireTypeListQueryService listService = new CargoIdentifireTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CargoIdentifireTypeList> GetCargoIdentifireTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.CargoIdentifireType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CargoIdentifireTypeListQueryService listService = new CargoIdentifireTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCargoIdentifireTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.CargoIdentifireType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CargoIdentifireTypeListQueryService queryService = new CargoIdentifireTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}