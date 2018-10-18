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
using System.ServiceModel.DomainServices.Server;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.CustomsMessaging.MessagingServices;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public CustomsRequestsSheetPM GetSingleCustomsRequestsSheetPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsRequestsSheetQuery = new CustomsRequestsSheetQueryService(customContext);
            CustomsRequestsSheetPM CustomsRequestsSheet = customsRequestsSheetQuery.GetSingle(id, false, false);
            return CustomsRequestsSheet;
        }

        

        public CustomsRequestsSheetList GetSingleCustomsRequestsSheetList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.CustomsRequestsSheet", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsRequestsSheetListQueryService listService = new CustomsRequestsSheetListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomsRequestsSheetList> GetCustomsRequestsSheetLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsRequestsSheet", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsRequestsSheetListQueryService listService = new CustomsRequestsSheetListQueryService(customContext);
            return listService.GetList(tenant);
        }
        public List<CustomsRequestsSheetPM> GetRequestCustomsDocumentInProgress(
            int Tenant,
            string declarationId, string DocumentsFilingId)
        {
            SecurityUtility.AuthenticationOnTenant(Tenant);
            customContext = CustomContext.GetContext(Tenant);
            if (!string.IsNullOrEmpty(DocumentsFilingId))
            {
                string InterfaceTypeCode = "2715";
                string ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");//task10676 
                string ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.CustomsDocument");//task10676 

                customsRequestsSheetQuery = new CustomsRequestsSheetQueryService(customContext);
                return customsRequestsSheetQuery.GetRequestInProgress(Tenant, InterfaceTypeCode,
                    ObjectTableId1, declarationId,
                    ObjectTableId2, DocumentsFilingId,
                    null);
                ;
            }
            return null;
        }

        //<--- Yuval Chalup 18.11.2014 TASK-4240
        public List<CustomsRequestsSheetPM> GetRequestInProgress(int Tenant,
            string InterfaceTypeCode,
            string ObjectTableId1, string EntityId1,
            string CustomFileNo,
            bool displayOnlyMode)
        {
            SecurityUtility.AuthenticationOnTenant(Tenant);
            customContext = CustomContext.GetContext(Tenant);
            customsRequestsSheetQuery = new CustomsRequestsSheetQueryService(customContext);
            return customsRequestsSheetQuery.GetRequestInProgress(Tenant, InterfaceTypeCode, ObjectTableId1, EntityId1, 
                null,null,
                CustomFileNo, displayOnlyMode);
        }
        //Yuval Chalup 18.11.2014 TASK-4240 --->

        public List<CustomsRequestsSheetList> GetDeclarationRequestsSheetLists( string customFileNumber ,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsRequestsSheet", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            customsRequestsSheetQuery = new CustomsRequestsSheetQueryService(customContext);
            return customsRequestsSheetQuery.GetCustomsRequestsSheetByCustomFileNumber(customFileNumber,tenant);
        }

        public List<CustomsRequestsSheetList> GetEntityRequestsSheetLists(string objectTableId, string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsRequestsSheet", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            customsRequestsSheetQuery = new CustomsRequestsSheetQueryService(customContext);
            return customsRequestsSheetQuery.GetEntityRequestsSheets(objectTableId,entityId, tenant);
        }


        [Query(HasSideEffects = true)] 
        public List<CustomsRequestsSheetList> GetCustomsRequestsSheetFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsRequestsSheet", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsRequestsSheetListQueryService listService = new CustomsRequestsSheetListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsRequestsSheetFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsRequestsSheet", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsRequestsSheetListQueryService queryService = new CustomsRequestsSheetListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void UpdateCustomsRequestsSheet(CustomsRequestsSheetPM currententityPm)
        {
            // SecurityUtility.CheckContactFeature("Customs.CustomsRequestsSheet", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            CustomsRequestsSheetUpdateService service = new CustomsRequestsSheetUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            service.Update(currententityPm, true);

        }
        public void UpdateCustomsRequestsSheetList(CustomsRequestsSheetList myCustomsRequestsSheetList)
        {
            //for change On Client ...
            ///Received	תשובה תקינה	21,Received,תשובה תקינה
            //customsRequestsSheetList.RequestStatusName = "תשובה תקינה";

        }
        


        [Invoke]
        //public void SetCustomsRequestSheetStatus(string id, int tenant, string statusCode)
        public string SetCustomsRequestSheetStatus(string id, int tenant, string statusCode)
        {
            try
            {
                customContext = CustomContext.GetContext(tenant);
                customsRequestsSheetQuery = new CustomsRequestsSheetQueryService(customContext);
                CustomsRequestsSheetPM CustomsRequestsSheet = customsRequestsSheetQuery.GetSingle(id, false, false);
                CustomsRequestsSheet.RequestStatusCode = statusCode;
                UpdateCustomsRequestsSheet(CustomsRequestsSheet);
                return null;
            }
            catch (Exception e)
            {
                return e.Message;               
            }

        }


        [Invoke]
        public string CustomsRequestSheetReQueue
            (string mainInterfaceCode, int tenant, string CustomsRequestSheetId)
        {

            try
            {

                MessagingServiceFactoryHelper.ResolveAndReQueue(mainInterfaceCode, tenant, CustomsRequestSheetId);

                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }
    }
}