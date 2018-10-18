using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.InvoiceModel.CustomFilters;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {
        public ARPaymentPM GetSingleARPayment(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ARPayment", "READ", tenant);
            arPaymentQuery = new ARPaymentQuery(tenant);
            aRPaymentRepository = new ARPaymentRepository(tenant);
            return arPaymentQuery.GetSinglePM(id, tenant);
        }

        public void UpdateARPaymentList(ARPaymentList currentEntity)
        {

        }

        public ARPaymentList GetSingleARPaymentList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ARPayment", "READ", tenant);

            ARPaymentList entityList = null;
            aRPaymentRepository = new ARPaymentRepository(tenant);
            arPaymentQuery = new ARPaymentQuery(aRPaymentRepository);
            ARPayment entity = aRPaymentRepository.GetSingleARPayment(id, tenant);

            if (entity != null)
            {
                List<ARPayment> SingleEntityList = new List<ARPayment>();
                SingleEntityList.Add(entity);

                IQueryable<ARPayment> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<ARPaymentList> iQueryableEntityList = arPaymentQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ARPaymentList> GetARPaymentFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ARPayment", "READ", tenant);

            aRPaymentRepository = new ARPaymentRepository(tenant);
            arPaymentQuery = new ARPaymentQuery(aRPaymentRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);

            IQueryable<ARPayment> iQueryable = aRPaymentRepository.GetARPayments(tenant);
            ARPaymentCustomFilter customFilters = new ARPaymentCustomFilter(tenant);
            iQueryable = customFilters.GetFilteredQuery(queryOperations, iQueryable);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARPayment>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ARPaymentList> query2 = arPaymentQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARPaymentList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ARPaymentList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> invoiceObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ARPayment", tenant).ToList();

                ObjectField objectField = (from a in invoiceObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.PaymentNo);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.PaymentNo);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetARPaymentFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ARPayment", "READ", tenant);

            aRPaymentRepository = new ARPaymentRepository(tenant);
            arPaymentQuery = new ARPaymentQuery(aRPaymentRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);

            IQueryable<ARPayment> iQueryable = aRPaymentRepository.GetARPayments(tenant);
            ARPaymentCustomFilter customFilters = new ARPaymentCustomFilter(tenant);
            iQueryable = customFilters.GetFilteredQuery(queryOperations, iQueryable);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARPayment>(nonListQueryOperation, iQueryable);

            IQueryable<ARPaymentList> query2 = arPaymentQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARPaymentList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertARPayment(ARPaymentPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("ARPayment", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entityPM.Tenant);
            }

            ARPaymentService service = new ARPaymentService(objectContext, entityPM.Tenant);
            service.Create(entityPM);
        }

        public void UpdateARPayment(ARPaymentPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("ARPayment", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entityPM.Tenant);
            }

            List<ARPaymentInvoicePM> changeSetList = ChangeSet.GetAssociatedChanges(entityPM, d => d.PaymentInvoices).Cast<ARPaymentInvoicePM>().ToList();
            List<ARPaymentInvoicePM> changedList = changeSetList.Where(d => ChangeSet.GetChangeOperation(d) != ChangeOperation.None).ToList();

            foreach (ARPaymentInvoicePM item in changedList)
            {
                switch (ChangeSet.GetChangeOperation(item))
                {
                    case ChangeOperation.Insert: { item.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { item.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { item.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { item.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            ARPaymentService service = new ARPaymentService(objectContext, entityPM.Tenant);
            service.SetChangedList(changedList);
            service.Update(entityPM);
        }

        public void DeleteARPayment(ARPaymentPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entityPM.Tenant);
            }

            aRPaymentRepository = new ARPaymentRepository(objectContext);
            ARPayment payment = aRPaymentRepository.GetSingleARPayment(entityPM.Id, entityPM.Tenant);
            aRPaymentRepository.Remove(payment);
        }
    }
}