using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {

        public ExternalSystemsTablesCodePM GetSingleExternalSystemsTablesCodePM(string id, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ExternalSystemsTablesCode", "READ", tenant);

            externalSystemsTablesCodeQuery = new ExternalSystemsTablesCodeQuery(tenant);
            return externalSystemsTablesCodeQuery.GetSinglePM(id, tenant);
        }

        public void UpdateExternalSystemsTablesCodeList(ExternalSystemsTablesCodeList currentEntity)
        {

        }

        public ExternalSystemsTablesCodeList GetSingleExternalSystemsTablesCodeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ExternalSystemsTablesCode", "READ", tenant);

            ExternalSystemsTablesCodeList entityList = null;
            externalSystemsTablesCodeRepository = new ExternalSystemsTablesCodeRepository(tenant);
            externalSystemsTablesCodeQuery = new ExternalSystemsTablesCodeQuery(externalSystemsTablesCodeRepository);
            ExternalSystemsTablesCode entity = externalSystemsTablesCodeRepository.GetSingleExternalSystemsTablesCode(id, tenant);

            if (entity != null)
            {
                List<ExternalSystemsTablesCode> SingleEntityList = new List<ExternalSystemsTablesCode>();
                SingleEntityList.Add(entity);

                IQueryable<ExternalSystemsTablesCode> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<ExternalSystemsTablesCodeList> iQueryableEntityList = externalSystemsTablesCodeQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }


        public IQueryable<ExternalSystemsTablesCodeList> GetExternalSystemsTablesCodeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            externalSystemsTablesCodeRepository = new ExternalSystemsTablesCodeRepository(tenant);
            externalSystemsTablesCodeQuery = new ExternalSystemsTablesCodeQuery(externalSystemsTablesCodeRepository);
            IQueryable<ExternalSystemsTablesCode> externalSystemsTablesCodes = externalSystemsTablesCodeRepository.GetExternalSystemsTablesCodes(tenant);

            IQueryable<ExternalSystemsTablesCodeList> query2 = externalSystemsTablesCodeQuery.GetIQueryableEntityList(externalSystemsTablesCodes);
            return query2;
        }


        [Query(HasSideEffects = true)]
        public IQueryable<ExternalSystemsTablesCodeList> GetExternalSystemsTablesCodeFilters(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ExternalSystemsTablesCode", "READ", tenant);

            externalSystemsTablesCodeRepository = new ExternalSystemsTablesCodeRepository(tenant);
            externalSystemsTablesCodeQuery = new ExternalSystemsTablesCodeQuery(externalSystemsTablesCodeRepository);

            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ExternalSystemsTablesCode> zones = externalSystemsTablesCodeRepository.GetExternalSystemsTablesCodes(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            zones = filter.GetFilteredQuery<ExternalSystemsTablesCode>(nonListQueryOperation, zones);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ExternalSystemsTablesCodeList> query2 = externalSystemsTablesCodeQuery.GetIQueryableEntityList(zones);

            query2 = filter.GetFilteredQuery<ExternalSystemsTablesCodeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ExternalSystemsTablesCodeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ExternalSystemsTablesCode", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalSystemsTablesCodeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalSystemsTablesCodeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalSystemsTablesCodeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalSystemsTablesCodeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalSystemsTablesCodeList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }
        public int GetExternalSystemsTablesCodeFiltersCount(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ExternalSystemsTablesCode", "READ", tenant);

            externalSystemsTablesCodeRepository = new ExternalSystemsTablesCodeRepository(tenant);
            externalSystemsTablesCodeQuery = new ExternalSystemsTablesCodeQuery(externalSystemsTablesCodeRepository);

            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ExternalSystemsTablesCode> zones = externalSystemsTablesCodeRepository.GetExternalSystemsTablesCodes(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            zones = filter.GetFilteredQuery<ExternalSystemsTablesCode>(nonListQueryOperation, zones);

            IQueryable<ExternalSystemsTablesCodeList> query2 = externalSystemsTablesCodeQuery.GetIQueryableEntityList(zones);

            query2 = filter.GetFilteredQuery<ExternalSystemsTablesCodeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }


        //public void InsertExternalSystemsTablesCode(ExternalSystemsTablesCodePM ExternalSystemsTablesCode)
        //{
        //    SecurityUtility.CheckContactFeature("ExternalSystemsTablesCode", "NEW", ExternalSystemsTablesCode.Tenant);

        //    if (objectContext == null)
        //    {
        //        objectContext = InvoiceContext.GetContext(ExternalSystemsTablesCode.Tenant);
        //    }
        //    ExternalSystemsTablesCodeService service = new ExternalSystemsTablesCodeService(objectContext, ExternalSystemsTablesCode.Tenant);
        //    service.Create(ExternalSystemsTablesCode);


        //    TableLastUpdateClass.UpdateTableHistory(ExternalSystemsTablesCode.Tenant, "ExternalSystemsTablesCode");
        //}

        //public void UpdateExternalSystemsTablesCode(ExternalSystemsTablesCodePM currentExternalSystemsTablesCode)
        //{
        //    SecurityUtility.CheckContactFeature("ExternalSystemsTablesCode", "UPDATE", currentExternalSystemsTablesCode.Tenant);

        //    if (objectContext == null)
        //    {
        //        objectContext = InvoiceContext.GetContext(currentExternalSystemsTablesCode.Tenant);
        //    }


        //    externalSystemsTablesCodeRepository = new ExternalSystemsTablesCodeRepository(objectContext);

        //    string entityName = "ExternalSystemsTablesCode" + currentExternalSystemsTablesCode.Id + currentExternalSystemsTablesCode.Tenant;
        //    string entityPmName = "ExternalSystemsTablesCodePM" + currentExternalSystemsTablesCode.Id + currentExternalSystemsTablesCode.Tenant;

        //    if (CacheManager.CacheWrapper.Get(entityName) != null)
        //    {
        //        CacheManager.CacheWrapper.Invalidate(entityName);
        //    }
        //    if (CacheManager.CacheWrapper.Get(entityPmName) != null)
        //    {
        //        CacheManager.CacheWrapper.Invalidate(entityPmName);
        //    }
        //    ExternalSystemsTablesCodeService service = new ExternalSystemsTablesCodeService(objectContext, currentExternalSystemsTablesCode.Tenant);
        //    service.Update(currentExternalSystemsTablesCode);
        //    TableLastUpdateClass.UpdateTableHistory(currentExternalSystemsTablesCode.Tenant, "ExternalSystemsTablesCode");


        //}

        //public void DeleteExternalSystemsTablesCode(ExternalSystemsTablesCodePM ExternalSystemsTablesCode)
        //{
        //    if (objectContext == null)
        //    {
        //        objectContext = InvoiceContext.GetContext(ExternalSystemsTablesCode.Tenant);
        //    }
        //    externalSystemsTablesCodeRepository = new ExternalSystemsTablesCodeRepository(objectContext);
        //    ExternalSystemsTablesCode entity = externalSystemsTablesCodeRepository.GetSingleExternalSystemsTablesCode(ExternalSystemsTablesCode.Id, ExternalSystemsTablesCode.Tenant);
        //    externalSystemsTablesCodeRepository.Remove(entity);
        //}



    }
}