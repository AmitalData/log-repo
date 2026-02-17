using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Security;
namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public IQueryable<InboundEmailPM> GetInboundEmailsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("InboundEmail", "READ", tenant);

            InboundEmailQuery InboundEmailQuery = new InboundEmailQuery(tenant);
            return inboundEmailQuery.GetInboundEmailsPMsByTenant(tenant);
        }

        public InboundEmailPM GetInboundEmailById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("InboundEmail", "READ", tenant);

            InboundEmailQuery inboundEmailQuery = new InboundEmailQuery(tenant);
            InboundEmailPM InboundEmail = inboundEmailQuery.GetSinglePM(id, tenant);
            return InboundEmail;
        }

        public InboundEmailList GetSingleInboundEmailList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("InboundEmail", "READ", tenant);
            InboundEmailRepository InboundEmailRepository;
            InboundEmailRepository = new InboundEmailRepository(tenant);
            InboundEmailQuery InboundEmailQuery = new InboundEmailQuery(InboundEmailRepository);
            InboundEmailList inboundEmailList = null;
            InboundEmail InboundEmail = inboundEmailRepository.GetSingleInboundEmail(id, tenant);

            if (InboundEmail != null)
            {
                List<InboundEmail> singleEntityList = new List<InboundEmail>();
                singleEntityList.Add(InboundEmail);

                IQueryable<InboundEmail> iQueryable = singleEntityList.AsQueryable();
                IQueryable<InboundEmailList> iQueryableEntityList = inboundEmailQuery.GetIQueryableEntityList(iQueryable);
                inboundEmailList = iQueryableEntityList.FirstOrDefault();
            }
            return inboundEmailList;
        }

        public IQueryable<InboundEmailList> GetInboundEmailList(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("InboundEmail", "READ", tenant);
            InboundEmailRepository InboundEmailRepository;
            InboundEmailRepository = new InboundEmailRepository(tenant);
            InboundEmailQuery InboundEmailQuery = new InboundEmailQuery(InboundEmailRepository);
            IQueryable<InboundEmail> InboundEmails = InboundEmailRepository.GetInboundEmails(tenant);
            IQueryable<InboundEmailList> query2 = inboundEmailQuery.GetIQueryableEntityList(InboundEmails);
            return query2;
        }

        public IQueryable<InboundEmailList> GetInboundEmailFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("InboundEmail", "READ", tenant);
            InboundEmailRepository inboundEmailRepository;
            inboundEmailRepository = new InboundEmailRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<InboundEmail> InboundEmails;

            if (tenant == 0)
            {
                InboundEmails = inboundEmailRepository.GetAllInboundEmails();
            }
            else
            {
                InboundEmails = inboundEmailRepository.GetInboundEmails(tenant);
            }

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            InboundEmails = filter.GetFilteredQuery<InboundEmail>(nonListQueryOperation, InboundEmails);
            int skippedPorts = queryOperations.PageIndex;
            InboundEmailQuery inboundEmailQuery = new InboundEmailQuery(inboundEmailRepository);
            IQueryable<InboundEmailList> query2 = inboundEmailQuery.GetIQueryableEntityList(InboundEmails);

            query2 = filter.GetFilteredQuery<InboundEmailList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(InboundEmailList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> InboundEmailObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("InboundEmail", tenant).ToList();

                ObjectField objectField = (from a in InboundEmailObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<InboundEmailList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<InboundEmailList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<InboundEmailList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<InboundEmailList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<InboundEmailList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Id);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Id);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetInboundEmailFiltersCount(byte[] xmlFilters, int tenant)
        {
            InboundEmailRepository InboundEmailRepository;
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("InboundEmail", "READ", tenant);

            InboundEmailRepository = new InboundEmailRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<InboundEmail> InboundEmails;

            if (tenant == 0)
            {
                InboundEmails = InboundEmailRepository.GetAllInboundEmails();
            }
            else
            {
                InboundEmails = InboundEmailRepository.GetInboundEmails(tenant);
            }

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            InboundEmails = filter.GetFilteredQuery<InboundEmail>(nonListQueryOperation, InboundEmails);
            InboundEmailQuery InboundEmailQuery = new InboundEmailQuery(InboundEmailRepository);
            IQueryable<InboundEmailList> query2 = InboundEmailQuery.GetIQueryableEntityList(InboundEmails);

            query2 = filter.GetFilteredQuery<InboundEmailList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertInboundEmail(InboundEmailPM currentInboundEmail)
        {
            SecurityUtility.AuthenticationOnTenant(currentInboundEmail.Tenant);
            SecurityUtility.CheckContactFeature("InboundEmail", "NEW", currentInboundEmail.Tenant);
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentInboundEmail.Tenant);
            }
            
            InboundEmailService service = new InboundEmailService(objectContext, currentInboundEmail.Tenant);
            service.Create(currentInboundEmail);

            TableLastUpdateClass.UpdateTableHistory(currentInboundEmail.Tenant, "InboundEmail");

        }

        public void UpdateInboundEmail(InboundEmailPM currentInboundEmail)
        {
            SecurityUtility.AuthenticationOnTenant(currentInboundEmail.Tenant);
            SecurityUtility.CheckContactFeature("InboundEmail", "UPDATE", currentInboundEmail.Tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentInboundEmail.Tenant);
            }

            string entityName = "InboundEmail" + currentInboundEmail.Id + currentInboundEmail.Tenant;
            string entityPmName = "InboundEmailPM" + currentInboundEmail.Id + currentInboundEmail.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            List<InboundEmailLinePM> inboundEmailLinesChangeSet = ChangeSet.GetAssociatedChanges(currentInboundEmail, d => d.InboundEmailLines).Cast<InboundEmailLinePM>().ToList();
            foreach (InboundEmailLinePM itemPM in inboundEmailLinesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            InboundEmailService service = new InboundEmailService(objectContext, currentInboundEmail.Tenant);
            service.SetChangeSet(inboundEmailLinesChangeSet);
            service.Update(currentInboundEmail);
            TableLastUpdateClass.UpdateTableHistory(currentInboundEmail.Tenant, "InboundEmail");

        }

    }
}