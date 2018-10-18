using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateIndustryList(IndustryList currentEntity)
        {
        }

        public IQueryable<Industry> GetIndustryes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Industry", "READ", tenant);

            industryRepository = new IndustryRepository(tenant);
            return industryRepository.GetIndustries(0);
        }

        public IQueryable<IndustryPM> GetIndustryesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Industry", "READ", tenant);

            industryQuery = new IndustryQuery(tenant);
            return industryQuery.GetIndustryPMsByTenant(tenant);
        }

        public IndustryPM GetSingleIndustry(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Industry", "READ", tenant);

            industryQuery = new IndustryQuery(tenant);
            return industryQuery.GetSinglePM(id, tenant);
        }

        public IndustryPM GetIndustryById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Industry", "READ", tenant);

            industryQuery = new IndustryQuery(tenant);
            return industryQuery.GetSinglePM(id, tenant);
        }

        public IndustryList GetSingleIndustryList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Industry", "READ", tenant);

            industryRepository = new IndustryRepository(tenant);
            industryQuery = new IndustryQuery(industryRepository);
            IndustryList IndustryList = null;
            Industry Industry = industryRepository.GetSingleIndustry(id, tenant);

            if (Industry != null)
            {
                List<Industry> singleEntityList = new List<Industry>();
                singleEntityList.Add(Industry);

                IQueryable<Industry> iQueryable = singleEntityList.AsQueryable();
                IQueryable<IndustryList> iQueryableEntityList = industryQuery.GetIQueryableEntityList(iQueryable);
                IndustryList = iQueryableEntityList.FirstOrDefault();
            }
            return IndustryList;
        }

        public IQueryable<IndustryList> GetIndustryLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Industry", "READ", tenant);

            industryRepository = new IndustryRepository(tenant);
            industryQuery = new IndustryQuery(industryRepository);
            IQueryable<Industry> Industryes = industryRepository.GetIndustries(tenant);
            IQueryable<IndustryList> query2 = industryQuery.GetIQueryableEntityList(Industryes);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<IndustryList> GetIndustryFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Industry", "READ", tenant);

            industryRepository = new IndustryRepository(tenant);
            industryQuery = new IndustryQuery(industryRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Industry> Industries = industryRepository.GetIndustries(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            Industries = filter.GetFilteredQuery<Industry>(nonListQueryOperation, Industries);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<IndustryList> query2 = industryQuery.GetIQueryableEntityList(Industries);

            query2 = filter.GetFilteredQuery<IndustryList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(IndustryList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Industry", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<IndustryList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<IndustryList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<IndustryList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<IndustryList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<IndustryList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Name);
            }
            //--------------------------------------------------------------------------------------------------

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetIndustryFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Industry", "READ", tenant);

            industryRepository = new IndustryRepository(tenant);
            industryQuery = new IndustryQuery(industryRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Industry> Industries = industryRepository.GetIndustries(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            Industries = filter.GetFilteredQuery<Industry>(nonListQueryOperation, Industries);

            IQueryable<IndustryList> query2 = industryQuery.GetIQueryableEntityList(Industries);

            query2 = filter.GetFilteredQuery<IndustryList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertIndustry(IndustryPM Industry)
        {
            SecurityUtility.CheckContactFeature("Industry", "NEW", Industry.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(Industry.Tenant);
            }
            IndustryService service = new IndustryService(objectContext, Industry.Tenant);
            service.Create(Industry);

            TableLastUpdateClass.UpdateTableHistory(Industry.Tenant, "Industry");
        }

        public void UpdateIndustry(IndustryPM currentIndustry)
        {
            SecurityUtility.CheckContactFeature("Industry", "UPDATE", currentIndustry.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentIndustry.Tenant);
            }

            industryRepository = new IndustryRepository(objectContext);

            string entityName = "Industry" + currentIndustry.Id + currentIndustry.Tenant;
            string entityPmName = "IndustryPM" + currentIndustry.Id + currentIndustry.Tenant;

            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            IndustryService service = new IndustryService(objectContext, currentIndustry.Tenant);
            service.Update(currentIndustry);
            TableLastUpdateClass.UpdateTableHistory(currentIndustry.Tenant, "Industry");
        }

        public void DeleteIndustry(IndustryPM Industry)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(Industry.Tenant);
            }
            industryRepository = new IndustryRepository(objectContext);
            Industry entity = industryRepository.GetSingleIndustry(Industry.Id, Industry.Tenant);
            industryRepository.Remove(entity);
        }
    }
}