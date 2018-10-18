using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.ServiceModel.DomainServices.Server;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public CustomsDocumentMetaDataValuePM GetSingleCustomsDocumentMetaDataValuePM(string customDocumentId, string metadataTypeCode, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsDocumentMetaDataValueQuery = new CustomsDocumentMetaDataValueQueryService(customContext);
            CustomsDocumentMetaDataValuePM CustomsDocumentMetaDataValue = customsDocumentMetaDataValueQuery.GetSingle(customDocumentId, metadataTypeCode, true, false);
            return CustomsDocumentMetaDataValue;
        }

        public CustomsDocumentMetaDataValueList GetSingleCustomsDocumentMetaDataValueList(string customDocumentId, string metadataTypeCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentMetaDataValue", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentMetaDataValueListQueryService listService = new CustomsDocumentMetaDataValueListQueryService(customContext);
            return listService.GetSingle(customDocumentId, metadataTypeCode);
        }

        public List<CustomsDocumentMetaDataValueList> GetCustomsDocumentMetaDataValueLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentMetaDataValue", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentMetaDataValueListQueryService listService = new CustomsDocumentMetaDataValueListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsDocumentMetaDataValueList> GetCustomsDocumentMetaDataValueFilters(byte[] xmlFilters, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentMetaDataValue", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentMetaDataValueListQueryService listService = new CustomsDocumentMetaDataValueListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsDocumentMetaDataValueFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentMetaDataValue", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentMetaDataValueListQueryService queryService = new CustomsDocumentMetaDataValueListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations,tenant);

        }


        public List<CustomsDocumentMetaDataValuePM> GetCustomsDocumentMetaDataValuesByDocument(string customDocumentId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsDocumentMetaDataValueQuery = new CustomsDocumentMetaDataValueQueryService(customContext);
            List<CustomsDocumentMetaDataValuePM> CustomsDocumentMetaDataValues = customsDocumentMetaDataValueQuery.GetCustomDocumentMetaDataValues(customDocumentId, tenant);
            return CustomsDocumentMetaDataValues;
        }

        [Invoke]
        public List<CustomsDocumentMetaDataValuePM> GetCustomsDocumentMetaDataValuesByConnectedEntity(string entityId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsDocumentMetaDataValueQuery = new CustomsDocumentMetaDataValueQueryService(customContext);
            List<CustomsDocumentMetaDataValuePM> CustomsDocumentMetaDataValues = customsDocumentMetaDataValueQuery.GetCustomsDocumentMetaDataValuesByConnectedEntity(entityId, tenant);
            return CustomsDocumentMetaDataValues;
        }


        [Invoke]
        public List<CustomsDocumentMetaDataValuePM> GetCustomsDocumentMetaDataValuesByCustomsDocumentFilingIds(string customsDocumentFilingsId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsDocumentMetaDataValueQuery = new CustomsDocumentMetaDataValueQueryService(customContext);
            List<CustomsDocumentMetaDataValuePM> CustomsDocumentMetaDataValues = customsDocumentMetaDataValueQuery.GetCustomsDocumentMetaDataValuesByCustomsDocumentFilingIds(customsDocumentFilingsId, tenant);
            return CustomsDocumentMetaDataValues;
        }


        //public void InsertCustomsDocumentMetaDataValue(CustomsDocumentMetaDataValuePM entityPm)
        //{
        //    SecurityUtility.CheckContactFeature("Customs.CustomsDocumentMetaDataValue", "NEW", entityPm.Tenant);

        //    if (customContext == null)
        //    {
        //        customContext = CustomContext.GetContext(entityPm.Tenant);
        //    }

        //    CustomsDocumentMetaDataValueUpdateService service = new CustomsDocumentMetaDataValueUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
        //    entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
        //    service.Update(entityPm, true);

        //}

        //public void UpdateCustomsDocumentMetaDataValue(CustomsDocumentMetaDataValuePM currententityPm)
        //{
        //    SecurityUtility.CheckContactFeature("Customs.CustomsDocumentMetaDataValue", "UPDATE", currententityPm.Tenant);

        //    if (customContext == null)
        //    {
        //        customContext = CustomContext.GetContext(currententityPm.Tenant);
        //    }

        //    CustomsDocumentMetaDataValueUpdateService service = new CustomsDocumentMetaDataValueUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
        //    currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
        //    service.Update(currententityPm, true);

        //}


    }
}