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
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.InvoiceModel.CustomFilters;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {
        public APPaymentPM GetSingleAPPayment(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APPayment", "READ", tenant);

            apPaymentQuery = new APPaymentQuery(tenant);
            return apPaymentQuery.GetSingleAPPaymentPM(id, tenant);
        }

        public void UpdateAPPaymentList(APPaymentList currentEntity)
        {

        }

        public APPaymentList GetSingleAPPaymentList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APPayment", "READ", tenant);

            APPaymentList entityList = null;
            aPPaymentRepository = new APPaymentRepository(tenant);
            apPaymentQuery = new APPaymentQuery(aPPaymentRepository);
            APPayment entity = aPPaymentRepository.GetSingleAPPayment(id, tenant);

            if (entity != null)
            {
                List<APPayment> SingleEntityList = new List<APPayment>();
                SingleEntityList.Add(entity);

                IQueryable<APPayment> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<APPaymentList> iQueryableEntityList = apPaymentQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<APPaymentList> GetAPPaymentFilters(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APPayment", "READ", tenant);

            aPPaymentRepository = new APPaymentRepository(tenant);
            apPaymentQuery = new APPaymentQuery(aPPaymentRepository);
            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);

            IQueryable<APPayment> iQueryable = aPPaymentRepository.GetAPPayments(tenant);
            APPaymentCustomFilter customFilters = new APPaymentCustomFilter(tenant);
            iQueryable = customFilters.GetFilteredQuery(queryOperations, iQueryable);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<APPayment>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<APPaymentList> query2 = apPaymentQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<APPaymentList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(APPaymentList).GetProperty(queryOperations.SortByColumnName);

                
                List<ObjectField> InvoiceObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("APPayment", tenant).ToList();

                ObjectField objectField = (from a in InvoiceObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<APPaymentList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<APPaymentList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<APPaymentList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<APPaymentList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<APPaymentList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<APPaymentList, bool>(queryOperations, query2);
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

        public int GetAPPaymentFiltersCount(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APPayment", "READ", tenant);

            aPPaymentRepository = new APPaymentRepository(tenant);
            apPaymentQuery = new APPaymentQuery(aPPaymentRepository);
            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);


            IQueryable<APPayment> iQueryable = aPPaymentRepository.GetAPPayments(tenant);
            APPaymentCustomFilter customFilters = new APPaymentCustomFilter(tenant);
            iQueryable = customFilters.GetFilteredQuery(queryOperations, iQueryable);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<APPayment>(nonListQueryOperation, iQueryable);

            IQueryable<APPaymentList> query2 = apPaymentQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<APPaymentList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertAPPayment(APPaymentPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("APPayment", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entityPM.Tenant);
            }

            APPaymentService service = new APPaymentService(objectContext, entityPM.Tenant);
            service.Create(entityPM);
        }

        public void UpdateAPPayment(APPaymentPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("APPayment", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entityPM.Tenant);
            }

            List<APPaymentInvoicePM> changeSetList = ChangeSet.GetAssociatedChanges(entityPM, d => d.PaymentInvoices).Cast<APPaymentInvoicePM>().ToList();
            List<APPaymentInvoicePM> changedList = changeSetList.Where(d => ChangeSet.GetChangeOperation(d) != ChangeOperation.None).ToList();

            foreach (APPaymentInvoicePM item in changedList)
            {
                switch (ChangeSet.GetChangeOperation(item))
                {
                    case ChangeOperation.Insert: { item.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { item.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { item.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { item.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            APPaymentService service = new APPaymentService(objectContext, entityPM.Tenant);
            service.SetChangeSet(changedList);
            service.Update(entityPM);

            if (this.ChangeSet != null)
            {
                this.ChangeSet.Associate(entityPM, service.payment, MapAPPaymentPMToAPPayment);
            }
        }

        public void DeleteAPPayment(APPaymentPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entityPM.Tenant);
            }

            aPPaymentRepository = new APPaymentRepository(objectContext);
            APPayment payment = aPPaymentRepository.GetSingleAPPayment(entityPM.Id, entityPM.Tenant);
            aPPaymentRepository.Remove(payment);
        }

        public void MapAPPaymentPMToAPPayment(APPaymentPM paymentpm, APPayment payment)
        {
            //aPPaymentStatusRepository = new APPaymentStatusRepository(payment.Tenant);
            //APPaymentStatus status = aPPaymentStatusRepository.GetSingleAPPaymentStatus(payment.StatusCode);
            //paymentpm.StatusName = status.Name;
        }
    }
}