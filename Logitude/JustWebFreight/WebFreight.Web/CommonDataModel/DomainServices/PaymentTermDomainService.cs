using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Xml.Serialization;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    // Implements application logic using the CommonDataContext context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class PaymentTermDomainService : LogitudeDomainService
    {
        private ICommonDataContext objectContext;
        private PaymentTermRepository paymentTermRepository;
        private PaymentTermQuery paymentTermQuery;

        #region PaymentTerms
        public IQueryable<PaymentTerm> GetPaymentTerms(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PaymentTerm", "READ", tenant);

            paymentTermRepository = new PaymentTermRepository(tenant);            
            return paymentTermRepository.GetPaymenTermsByTenant(0);
        }

        public IQueryable<PaymentTermPM> GetPaymentTermsSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PaymentTerm", "READ", tenant);

            paymentTermQuery = new PaymentTermQuery(tenant);            
            IQueryable<PaymentTermPM> q = paymentTermQuery.GetPaymentTermsByCodeOrName(code, name, tenant).Where(d => d.Tenant == tenant);
            return q;
        }

        public IQueryable<PaymentTermPM> GetPaymentTermsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PaymentTerm", "READ", tenant);

            paymentTermQuery = new PaymentTermQuery(tenant);                        
            return paymentTermQuery.GetPaymenTermPMsByTenant(tenant);
        }

        public IQueryable<PaymentTermPM> GetFirstPaymentTerms(string input, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PaymentTerm", "READ", tenant);

            paymentTermQuery = new PaymentTermQuery(tenant);
            input = input.ToUpper();
            return paymentTermQuery.GetPaymenTermPMsByTenant(tenant).Where(p => p.Id.ToUpper().StartsWith(input) || p.EnglishName.ToUpper().StartsWith(input)).Where(d => d.Tenant == tenant);
        }
        
        public PaymentTermPM GetSinglePaymentTerm(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PaymentTerm", "READ", tenant);

            paymentTermQuery = new PaymentTermQuery(tenant);
            return paymentTermQuery.GetSinglePM(id, tenant);
        }

        public PaymentTermList GetSinglePaymentTermList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PaymentTerm", "READ", tenant);

            PaymentTermList entityList = null;
            paymentTermRepository = new PaymentTermRepository(tenant);
            PaymentTerm entity = paymentTermRepository.GetSinglePaymentTerm(id, tenant);

            if (entity != null)
            {
                List<PaymentTerm> SingleEntityList = new List<PaymentTerm>();
                SingleEntityList.Add(entity);

                paymentTermQuery = new PaymentTermQuery(paymentTermRepository);
                IQueryable<PaymentTerm> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<PaymentTermList> iQueryableEntityList = paymentTermQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<PaymentTermList> GetPaymentTermLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PaymentTerm", "READ", tenant);

            paymentTermRepository = new PaymentTermRepository(tenant);
            paymentTermQuery = new PaymentTermQuery(paymentTermRepository);

            IQueryable<PaymentTerm> iQueryable = paymentTermRepository.GetPaymenTermsByTenant(tenant);
            IQueryable<PaymentTermList> query2 = paymentTermQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        public void UpdatePaymentTermList(PaymentTermList currentEntity)
        {
        }

        [Query(HasSideEffects = true)]
        public IQueryable<PaymentTermList> GetPaymentTermFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PaymentTerm", "READ", tenant);

            paymentTermRepository = new PaymentTermRepository(tenant);
            paymentTermQuery = new PaymentTermQuery(paymentTermRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<PaymentTerm> paymentTerms = paymentTermRepository.GetPaymenTermsByTenant(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
            paymentTerms = filter.GetFilteredQuery<PaymentTerm>(nonListQueryOperation, paymentTerms);
            int skippedPaymentTerms = queryOperations.PageIndex;

            IQueryable<PaymentTermList> query2 = paymentTermQuery.GetIQueryableEntityList(paymentTerms);

            query2 = filter.GetFilteredQuery<PaymentTermList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PaymentTermList).GetProperty(queryOperations.SortByColumnName);
               
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("PaymentTerm", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentTermList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentTermList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentTermList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentTermList, int>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.EnglishName);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.EnglishName);
            }

            query2 = query2.Skip(skippedPaymentTerms);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetPaymentTermFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("PaymentTerm", "READ", tenant);

            paymentTermRepository = new PaymentTermRepository(tenant);
            paymentTermQuery = new PaymentTermQuery(paymentTermRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<PaymentTerm> paymentTerms = paymentTermRepository.GetPaymenTermsByTenant(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            paymentTerms = filter.GetFilteredQuery<PaymentTerm>(nonListQueryOperation, paymentTerms);
            int skippedPaymentTerms = queryOperations.PageIndex;

            IQueryable<PaymentTermList> query2 = paymentTermQuery.GetIQueryableEntityList(paymentTerms);

            query2 = filter.GetFilteredQuery<PaymentTermList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertPaymentTerm(PaymentTermPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("PaymentTerm", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            PaymentTermService service = new PaymentTermService(objectContext, entityPM.Tenant);
            service.Create(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "PaymentTerm");
        }

        public void UpdatePaymentTerm(PaymentTermPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("PaymentTerm", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            PaymentTermService service = new PaymentTermService(objectContext, entityPM.Tenant);
            service.Update(entityPM);          

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "PaymentTerm");
        }

        public void DeletePaymentTerm(PaymentTermPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            paymentTermRepository = new PaymentTermRepository(objectContext);

            PaymentTerm entity = paymentTermRepository.GetSinglePaymentTerm(entityPM.Id, entityPM.Tenant);
            paymentTermRepository.Remove(entity);
        }
        #endregion

    }
}


