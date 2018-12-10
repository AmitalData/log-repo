using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.CustomFilters;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {
        public ARInvoicePM GetSingleARInvoicePM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ARInvoice", "READ", tenant);

            arInvoiceQuery = new ARInvoiceQuery(tenant);
            return arInvoiceQuery.GetSinglePM(id, tenant);
        }

        public ARInvoiceList GetSingleARInvoiceList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ARInvoice", "READ", tenant);

            ARInvoiceList entityList = null;
            aRInvoiceRepository = new ARInvoiceRepository(tenant);
            arInvoiceQuery = new ARInvoiceQuery(aRInvoiceRepository);
            ARInvoice entity = aRInvoiceRepository.GetSingleInvoice(id);

            if (entity != null)
            {
                List<ARInvoice> SingleEntityList = new List<ARInvoice>();
                SingleEntityList.Add(entity);

                IQueryable<ARInvoice> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<ARInvoiceList> iQueryableEntityList = arInvoiceQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetCustomFieldsValues("ARInvoice", tenant, new List<ARInvoiceList> { entityList }.Cast<object>().ToList());

            return entityList;
        }

        public void UpdateARInvoicList(ARInvoiceList currentEntity)
        {

        }

        [Query(HasSideEffects = true)]
        public IQueryable<ARInvoiceList> GetARInvoiceFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ARInvoice", "READ", tenant);

            aRInvoiceRepository = new ARInvoiceRepository(tenant);
            arInvoiceQuery = new ARInvoiceQuery(aRInvoiceRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);

            IQueryable<ARInvoice> iQueryable = aRInvoiceRepository.GetIQueryableInvoices(tenant);

            InvoiceCustomFilter customFilters = new InvoiceCustomFilter(tenant);
            iQueryable = customFilters.GetFilteredQuery(queryOperations, iQueryable);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARInvoice>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ARInvoiceList> query2 = arInvoiceQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARInvoiceList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ARInvoiceList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> invoiceObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ARInvoice", tenant).ToList();

                ObjectField objectField = (from a in invoiceObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<ARInvoiceList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ARInvoiceList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ARInvoiceList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ARInvoiceList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<ARInvoiceList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ARInvoiceList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.InvoiceDate);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.InvoiceDate);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);


            List<ARInvoiceList> listQuery = query2.ToList();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetCustomFieldsValues("ARInvoice", tenant, listQuery.Cast<object>().ToList());


            return query2;
        }

        public int GetARInvoiceFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ARInvoice", "READ", tenant);

            aRInvoiceRepository = new ARInvoiceRepository(tenant);
            arInvoiceQuery = new ARInvoiceQuery(aRInvoiceRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);

            IQueryable<ARInvoice> iQueryable = aRInvoiceRepository.GetIQueryableInvoices(tenant);
            InvoiceCustomFilter customFilters = new InvoiceCustomFilter(tenant);
            iQueryable = customFilters.GetFilteredQuery(queryOperations, iQueryable);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARInvoice>(nonListQueryOperation, iQueryable);

            IQueryable<ARInvoiceList> query2 = arInvoiceQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARInvoiceList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertARInvoice(ARInvoicePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("ARInvoice", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entityPM.Tenant);
            }

            ARInvoiceService service = new ARInvoiceService(objectContext, entityPM.Tenant);
            service.Create(entityPM);           
        }

        public void UpdateARInvoice(ARInvoicePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("ARInvoice", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entityPM.Tenant);
            }

            List<ARInvoiceLinePM> invoiceLinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.InvoiceLines).Cast<ARInvoiceLinePM>().ToList();
            foreach (ARInvoiceLinePM item in invoiceLinesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(item))
                {
                    case ChangeOperation.Insert: { item.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { item.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { item.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { item.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            List<ARInvoicePaymentPM> invoicePaymentsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.InvoicePayments).Cast<ARInvoicePaymentPM>().ToList();
            foreach (ARInvoicePaymentPM item in invoicePaymentsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(item))
                {
                    case ChangeOperation.Insert: { item.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { item.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { item.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { item.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            List<ConstituentPM> invoiceConstituentsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ConstituentInvoices).Cast<ConstituentPM>().ToList();
            foreach (ConstituentPM item in invoiceConstituentsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(item))
                {
                    case ChangeOperation.Insert: { item.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { item.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { item.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { item.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            ARInvoiceService service = new ARInvoiceService(objectContext, entityPM.Tenant);
            service.SetChangeSet(invoiceLinesChangeSet, invoicePaymentsChangeSet, invoiceConstituentsChangeSet);
            service.Update(entityPM);
        }

        public void DeleteARInvoice(ARInvoicePM entityPM)
        {

        }

        private ARInvoiceEntityRepository aRInvoiceEntityRepository;

        [Invoke]
        public string AutoCreditInvoice(string entityId, bool IsInvoiceNumberManuallySet, string AutoCreditManualNumber, int tenant, DateTime? AutoCreditDate)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ARInvoice", "NEW", tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(tenant);
            }

            ARInvoiceService service = new ARInvoiceService(objectContext, tenant);
            string AutoCreditId = service.CreateAutoCredit(entityId, IsInvoiceNumberManuallySet, AutoCreditManualNumber, AutoCreditDate);
            return AutoCreditId;
        }

        private string GetLoggedContact(int tenant)
        {
            string loggedContactId = null;

            ContactRepository contactRepository = new ContactRepository(tenant);

            string email = HttpContext.Current.User.Identity.Name;

            if (email != null)
            {
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
                loggedContactId = loggedContact.Id;
            }

            else
            {
                ContactPM loggedContact = new ContactQuery(tenant).GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
                loggedContactId = loggedContact.Id;
            }

            return loggedContactId;
        }

        [Invoke]
        public bool IsARInvoiceNumberExists(string invoiceNumber,int tenant)
        {
            aRInvoiceRepository = new ARInvoiceRepository(tenant);

            bool result = aRInvoiceRepository.IsARInvoiceNumberExists(invoiceNumber, tenant);

            return result;
        }

        public ARInvoiceEntityPM GetSingleARInvoiceEntityPMByInvoiceAndEntity(string invoiceid, string entityid, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            arInvoiceEntityQuery = new ARInvoiceEntityQuery(tenant);

            return arInvoiceEntityQuery.GetSingleInvoiceEntityPMByInvoiceAndEntity(invoiceid, entityid, tenant);
        }

        [Invoke]
        public DateTime InsertARInvoiceTraceEvent(ARInvoicePM entityPM, DateTime? eventDate, string note, string eventTypeId, string objectTableId, string userId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            string entityId = entityPM.Id;  

            //aRInvoiceRepository = new ARInvoiceRepository(tenant);
            //EventTypeRepository eventTypeRep = new EventTypeRepository(tenant);
            //EventType eventType = eventTypeRep.GetSingleEventType(eventTypeId, tenant);
            //WebFreightDomainService webFreightService = new WebFreightDomainService();
            //ARInvoice entity = aRInvoiceRepository.GetSingleInvoice(entityPM.Id);

            TraceEvent newTraceEvent = new TraceEvent();
            newTraceEvent.Id = Guid.NewGuid().ToString();
            newTraceEvent.LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            newTraceEvent.Notes = note;
            newTraceEvent.ObjectTableId = objectTableId;
            newTraceEvent.Tenant = tenant;
            newTraceEvent.UserId = userId;
            newTraceEvent.EventTypeId = eventTypeId;
            newTraceEvent.EventDateTime = eventDate != null ? eventDate.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
            newTraceEvent.EntityId = entityId;
            newTraceEvent.Deleted = false;
            newTraceEvent.IsAddedManually = true;

            TraceEventRepository traceEventRep = new TraceEventRepository(tenant);
            traceEventRep.Add(newTraceEvent);
            traceEventRep.SubmitChanges();

            
            //aRInvoiceRepository.Update(entity);
            //aRInvoiceRepository.SubmitChanges();

            return newTraceEvent.LogDateTime;
        }

        [Invoke]
        public void DeleteARInvoiceTraceEvent(ARInvoicePM entityPM, string traceEventId, int tenant, bool external)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            TraceEventRepository traceEventRep = new TraceEventRepository(tenant);
            TraceEvent traceEvent = traceEventRep.GetSingleTraceEvent(traceEventId);
            traceEvent.Deleted = true;
            traceEventRep.Update(traceEvent);
            traceEventRep.SubmitChanges();
        }

        [Invoke]
        public void SetARInvoiceAsDontTransfer(string invoiceId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aRInvoiceRepository = new ARInvoiceRepository(tenant);

            ARInvoice entity = aRInvoiceRepository.GetSingleInvoice(invoiceId);

            if (entity != null)
            {
                entity.TransferStatusCode = "BL";
            }

            aRInvoiceRepository.Update(entity);
            aRInvoiceRepository.SubmitChanges();
        }

        public List<ARInvoiceTransferHistoryPM> GetARInvoiceTransferHistory(string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ARInvoice", "READ", tenant);

            arInvoiceQuery = new ARInvoiceQuery(tenant);
            return arInvoiceQuery.GetARInvoiceTransferHistory(entityId, tenant);
        }
    }
}