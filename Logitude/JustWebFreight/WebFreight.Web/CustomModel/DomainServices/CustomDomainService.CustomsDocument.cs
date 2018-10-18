using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public CustomsDocumentPM GetSingleCustomsDocumentPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsDocumentQuery = new CustomsDocumentQueryService(customContext);
            CustomsDocumentPM CustomsDocument = customsDocumentQuery.GetSingleCustomsDocumentPMWithDeclarationId(id, tenant);//customsDocumentQuery.GetSingle(id, true, false);
            return CustomsDocument;
        }

        public CustomsDocumentList GetSingleCustomsDocumentList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.CustomsDocument", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentListQueryService listService = new CustomsDocumentListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomsDocumentList> GetCustomsDocumentLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocument", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentListQueryService listService = new CustomsDocumentListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsDocumentList> GetCustomsDocumentFilters(byte[] xmlFilters, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocument", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentListQueryService listService = new CustomsDocumentListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsDocumentFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocument", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentListQueryService queryService = new CustomsDocumentListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertCustomsDocument(CustomsDocumentPM entityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocument", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            CustomsDocumentUpdateService service = new CustomsDocumentUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            foreach (CustomsDocumentMetaDataValuePM value in entityPm.CustomsDocumentMetaDataValues)
            {
                value.ChangeSetOp = ChangeSetOperation.Insert;
            }

            service.Update(entityPm, true);

        }

        public void UpdateCustomsDocument(CustomsDocumentPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocument", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            CustomsDocumentUpdateService service = new CustomsDocumentUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            SetCustomsDocumentMetaDataValuesOperationSet(currententityPm);
            service.Update(currententityPm, true);

        }

        private void SetCustomsDocumentMetaDataValuesOperationSet(CustomsDocumentPM currententityPm)
        {
            List<CustomsDocumentMetaDataValuePM> customsDocumentValuechangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.CustomsDocumentMetaDataValues).Cast<CustomsDocumentMetaDataValuePM>().ToList();
            foreach (CustomsDocumentMetaDataValuePM itemPM in customsDocumentValuechangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            CustomsDocumentMetaDataValuePM currentItemPM = currententityPm.CustomsDocumentMetaDataValues.Where(d => d.CustomsDocumentId == itemPM.CustomsDocumentId && d.MetaDataTypeCode == itemPM.MetaDataTypeCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            CustomsDocumentMetaDataValuePM currentItemPM = currententityPm.CustomsDocumentMetaDataValues.Where(d => d.CustomsDocumentId == itemPM.CustomsDocumentId && d.MetaDataTypeCode == itemPM.MetaDataTypeCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            CustomsDocumentMetaDataValuePM currentItemPM = new CustomsDocumentMetaDataValuePM() { ChangeSetOp = ChangeSetOperation.Delete, CustomsDocumentId = itemPM.CustomsDocumentId, MetaDataTypeCode = itemPM.MetaDataTypeCode };
                            currententityPm.DeletedCustomsDocumentMetaDataValues.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            CustomsDocumentMetaDataValuePM currentItemPM = currententityPm.CustomsDocumentMetaDataValues.Where(d => d.CustomsDocumentId == itemPM.CustomsDocumentId && d.MetaDataTypeCode == itemPM.MetaDataTypeCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

      
        public void UpdateCustomsDocumentList(CustomsDocumentList list)
        {

        }

        //<--- Yuval Chalup 03.11.2014 TASK-4238
        public List<CustomsDocumentPM> GetDeclarationDocumentList(string parentEntityId, string parentEntityCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customsDocumentQuery = new CustomsDocumentQueryService(tenant);
            return customsDocumentQuery.GetDeclarationDocumentList(parentEntityId, parentEntityCode, tenant);
        }
        //Yuval Chalup 03.11.2014 TASK-4238 --->

        [Invoke]
        public bool CheckIfDocumentPointerExistsForConstraint(string child1EntityId, string child1EntityCode,int tenant)
        {
            customsDocumentPointerQuery = new CustomsDocumentPointerQueryService(tenant);
            return customsDocumentPointerQuery.CheckIfDocumentPointerExistsForConstraint(child1EntityId, child1EntityCode,tenant);
        }
    }
}