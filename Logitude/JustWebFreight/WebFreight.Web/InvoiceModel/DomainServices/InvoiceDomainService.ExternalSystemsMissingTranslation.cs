using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.Helpers;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {

        public ExternalSystemsMissingTranslationPM GetSingleExternalSystemsMissingTranslationPM(string id, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("ExternalSystemsMissingTranslation", "READ", tenant);

            externalSystemsMissingTranslationQuery = new ExternalSystemsMissingTranslationQuery(tenant);
            return externalSystemsMissingTranslationQuery.GetSingleExternalSystemsMissingTranslationPM(id, tenant);
        }

        public void UpdateExternalSystemsMissingTranslationList(ExternalSystemsMissingTranslationList currentEntity)
        {

        }

        public ExternalSystemsMissingTranslationList GetSingleExternalSystemsMissingTranslationList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("ExternalSystemsMissingTranslation", "READ", tenant);

            ExternalSystemsMissingTranslationList entityList = null;
            externalSystemsMissingTranslationRepository = new ExternalSystemsMissingTranslationRepository(tenant);
            externalSystemsMissingTranslationQuery = new ExternalSystemsMissingTranslationQuery(externalSystemsMissingTranslationRepository);
            ExternalSystemsMissingTranslation entity = externalSystemsMissingTranslationRepository.GetSingleExternalSystemsMissingTranslation(id, tenant);

            if (entity != null)
            {
                List<ExternalSystemsMissingTranslation> SingleEntityList = new List<ExternalSystemsMissingTranslation>();
                SingleEntityList.Add(entity);

                IQueryable<ExternalSystemsMissingTranslation> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<ExternalSystemsMissingTranslationList> iQueryableEntityList = externalSystemsMissingTranslationQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }


        public IQueryable<ExternalSystemsMissingTranslationList> GetExternalSystemsMissingTranslationLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            externalSystemsMissingTranslationRepository = new ExternalSystemsMissingTranslationRepository(tenant);
            externalSystemsMissingTranslationQuery = new ExternalSystemsMissingTranslationQuery(externalSystemsMissingTranslationRepository);
            IQueryable<ExternalSystemsMissingTranslation> ExternalSystemsMissingTranslations = externalSystemsMissingTranslationRepository.GetExternalSystemsMissingTranslationsByTenant(tenant);

            IQueryable<ExternalSystemsMissingTranslationList> query2 = externalSystemsMissingTranslationQuery.GetIQueryableEntityList(ExternalSystemsMissingTranslations);
            return query2;
        }


        [Query(HasSideEffects = true)]
        public IQueryable<ExternalSystemsMissingTranslationList> GetExternalSystemsMissingTranslationFilters(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("ExternalSystemsMissingTranslation", "READ", tenant);

            externalSystemsMissingTranslationRepository = new ExternalSystemsMissingTranslationRepository(tenant);
            externalSystemsMissingTranslationQuery = new ExternalSystemsMissingTranslationQuery(externalSystemsMissingTranslationRepository);

            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ExternalSystemsMissingTranslation> zones = externalSystemsMissingTranslationRepository.GetExternalSystemsMissingTranslations(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            zones = filter.GetFilteredQuery<ExternalSystemsMissingTranslation>(nonListQueryOperation, zones);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ExternalSystemsMissingTranslationList> query2 = externalSystemsMissingTranslationQuery.GetIQueryableEntityList(zones);

            query2 = filter.GetFilteredQuery<ExternalSystemsMissingTranslationList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ExternalSystemsMissingTranslationList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ExternalSystemsMissingTranslation", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalSystemsMissingTranslationList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalSystemsMissingTranslationList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalSystemsMissingTranslationList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalSystemsMissingTranslationList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalSystemsMissingTranslationList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.LogitudeTable);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.LogitudeTable);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }
        public int GetExternalSystemsMissingTranslationFiltersCount(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ExternalSystemsMissingTranslation", "READ", tenant);

            externalSystemsMissingTranslationRepository = new ExternalSystemsMissingTranslationRepository(tenant);
            externalSystemsMissingTranslationQuery = new ExternalSystemsMissingTranslationQuery(externalSystemsMissingTranslationRepository);

            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ExternalSystemsMissingTranslation> zones = externalSystemsMissingTranslationRepository.GetExternalSystemsMissingTranslations(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            zones = filter.GetFilteredQuery<ExternalSystemsMissingTranslation>(nonListQueryOperation, zones);

            IQueryable<ExternalSystemsMissingTranslationList> query2 = externalSystemsMissingTranslationQuery.GetIQueryableEntityList(zones);

            query2 = filter.GetFilteredQuery<ExternalSystemsMissingTranslationList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }



        public void InsertExternalSystemsMissingTranslation(ExternalSystemsMissingTranslationPM ExternalSystemsMissingTranslation)
        {
          //  SecurityUtility.CheckContactFeature("ExternalSystemsMissingTranslation", "NEW", ExternalSystemsMissingTranslation.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(ExternalSystemsMissingTranslation.Tenant);
            }
            ExternalSystemsMissingTranslationService service = new ExternalSystemsMissingTranslationService(objectContext, ExternalSystemsMissingTranslation.Tenant);
            service.Create(ExternalSystemsMissingTranslation);


            TableLastUpdateClass.UpdateTableHistory(ExternalSystemsMissingTranslation.Tenant, "ExternalSystemsMissingTranslation");
        }

        public void UpdateExternalSystemsMissingTranslation(ExternalSystemsMissingTranslationPM currentExternalSystemsMissingTranslation)
        {
            SecurityUtility.CheckContactFeature("ExternalSystemsMissingTranslation", "UPDATE", currentExternalSystemsMissingTranslation.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(currentExternalSystemsMissingTranslation.Tenant);
            }


            externalSystemsMissingTranslationRepository = new ExternalSystemsMissingTranslationRepository(objectContext);

            string entityName = "ExternalSystemsMissingTranslation" + currentExternalSystemsMissingTranslation.Id + currentExternalSystemsMissingTranslation.Tenant;
            string entityPmName = "ExternalSystemsMissingTranslationPM" + currentExternalSystemsMissingTranslation.Id + currentExternalSystemsMissingTranslation.Tenant;

            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            ExternalSystemsMissingTranslationService service = new ExternalSystemsMissingTranslationService(objectContext, currentExternalSystemsMissingTranslation.Tenant);
            service.Update(currentExternalSystemsMissingTranslation);
            TableLastUpdateClass.UpdateTableHistory(currentExternalSystemsMissingTranslation.Tenant, "ExternalSystemsMissingTranslation");


        }

        public void DeleteExternalSystemsMissingTranslation(ExternalSystemsMissingTranslationPM ExternalSystemsMissingTranslation)
        {
            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(ExternalSystemsMissingTranslation.Tenant);
            }
            externalSystemsMissingTranslationRepository = new ExternalSystemsMissingTranslationRepository(objectContext);
            ExternalSystemsMissingTranslation entity = externalSystemsMissingTranslationRepository.GetSingleExternalSystemsMissingTranslation(ExternalSystemsMissingTranslation.Id, ExternalSystemsMissingTranslation.Tenant);
            externalSystemsMissingTranslationRepository.Remove(entity);
        }



    }
}