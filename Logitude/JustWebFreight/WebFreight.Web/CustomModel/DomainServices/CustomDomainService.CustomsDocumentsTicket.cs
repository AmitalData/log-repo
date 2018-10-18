using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public CustomsDocumentsTicketPM GetSingleCustomsDocumentsTicketPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsDocumentsTicketQuery = new CustomsDocumentsTicketQueryService(customContext);
            CustomsDocumentsTicketPM CustomsDocumentsTicket = customsDocumentsTicketQuery.GetSingle(id, true, false);
            return CustomsDocumentsTicket;
        }

        public CustomsDocumentsTicketList GetSingleCustomsDocumentsTicketList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.CustomsDocumentsTicket", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentsTicketListQueryService listService = new CustomsDocumentsTicketListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomsDocumentsTicketList> GetCustomsDocumentsTicketLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentsTicket", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentsTicketListQueryService listService = new CustomsDocumentsTicketListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsDocumentsTicketList> GetCustomsDocumentsTicketFilters(byte[] xmlFilters, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentsTicket", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentsTicketListQueryService listService = new CustomsDocumentsTicketListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsDocumentsTicketFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentsTicket", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentsTicketListQueryService queryService = new CustomsDocumentsTicketListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertCustomsDocumentsTicket(CustomsDocumentsTicketPM entityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentsTicket", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }
            foreach (CustomsDocumentPointerPM pointer in entityPm.CustomsDocumentPointers)
            {
                pointer.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }
            CustomsDocumentsTicketUpdateService service = new CustomsDocumentsTicketUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            service.Update(entityPm, true);

        }

        public void UpdateCustomsDocumentsTicket(CustomsDocumentsTicketPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentsTicket", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

           
            CustomsDocumentsTicketUpdateService service = new CustomsDocumentsTicketUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            SetCustomsDocumentPointerChangeSet(currententityPm);
          
            service.Update(currententityPm, true);

        }

        private void SetCustomsDocumentPointerChangeSet(CustomsDocumentsTicketPM currententityPm)
        {
            List<CustomsDocumentPointerPM> vendorCommunicationchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.CustomsDocumentPointers).Cast<CustomsDocumentPointerPM>().ToList();
            foreach (CustomsDocumentPointerPM itemPM in vendorCommunicationchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            CustomsDocumentPointerPM currentItemPM = currententityPm.CustomsDocumentPointers.Where(d => d.Child1EntityId == itemPM.Child1EntityId && d.Child2EntityId==itemPM.Child2EntityId&&d.Child3EntityId==itemPM.Child3EntityId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            CustomsDocumentPointerPM currentItemPM = currententityPm.CustomsDocumentPointers.Where(d => d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            CustomsDocumentPointerPM currentItemPM = new CustomsDocumentPointerPM() { ChangeSetOp = ChangeSetOperation.Delete, Id = itemPM.Id };
                            currententityPm.DeletedCustomsDocumentPointers.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            CustomsDocumentPointerPM currentItemPM = currententityPm.CustomsDocumentPointers.Where(d => d.Id == itemPM.Id ).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        //public void DeleteCustomsDocumentsTicket(CustomsDocumentsTicketPM entityPm)
        //{
 
        //}

        [Invoke]
        public void DeleteCustomsDocumentsTicket(CustomsDocumentsTicketPM entityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentsTicket", "UPDATE", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            CustomsDocumentsTicketUpdateService service = new CustomsDocumentsTicketUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
            CustomsDocumentPointerQueryService pointerQueryService = new CustomsDocumentPointerQueryService(customContext);

            List<CustomsDocumentPointerPM> pointers = pointerQueryService.GetPointersForTicket(entityPm.Id, entityPm.Tenant);

            foreach (CustomsDocumentPointerPM pointer in pointers)
            {
                entityPm.DeletedCustomsDocumentPointers.Add(pointer);
                pointer.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
            }
            service.Update(entityPm, true);

        }



        public void UpdateCustomsDocumentsTicketList(CustomsDocumentsTicketList list)
        {

        }

        public List<CustomsDocumentsTicketPM> GetCustomsDocumentsTicketsByEntityIdAndChilds(string entityId, string childEntityId1, string childEntityId2, string childEntityId3, int tenant,string parentEntityCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customsDocumentsTicketQuery = new CustomsDocumentsTicketQueryService(tenant);
            return customsDocumentsTicketQuery.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(entityId, childEntityId1, childEntityId2, childEntityId3, tenant,parentEntityCode);
        }

        public bool CheckForPointers(string parentEntityId, string child1EntityId, string child2EntityId, string child3EntityId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsDocumentPointerQuery = new CustomsDocumentPointerQueryService(customContext);
            return customsDocumentPointerQuery.CheckForPointers(parentEntityId, child1EntityId, child2EntityId, child3EntityId, tenant);
        }
        [Query(HasSideEffects = true)]
        public List<CustomsDocumentPointerPM> GetCustomDocumentPoinersForItems(string parentEntityId, string invCounterKey, string itemsLineNumbers,int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsDocumentPointerQuery = new CustomsDocumentPointerQueryService(customContext);
            return customsDocumentPointerQuery.GetCustomDocumentPoinersForItems(parentEntityId, invCounterKey, itemsLineNumbers, tenant);
        }
    }
}