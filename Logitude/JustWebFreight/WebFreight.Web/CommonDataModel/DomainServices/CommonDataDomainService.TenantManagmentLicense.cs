using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
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

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        private TenantManagmentLicenseQuery myTenantManagmentLicenseQuery;
        private TenantManagmentLicenseRepository myTenantManagmentLicenseRepository;
        public void UpdateTenantManagmentLicenseList(TenantManagmentLicenseList currentEntity)
        {

        }

        //public TenantManagmentLicensePM GetSingleTenantManagmentLicensePM(string id, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    myTenantManagmentLicenseQuery = new TenantManagmentLicenseQuery(tenant);
        //    return myTenantManagmentLicenseQuery.GetSinglePM(id);
        //}

        public TenantManagmentLicenseList GetSingleTenantManagmentLicenseList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            myTenantManagmentLicenseRepository = new TenantManagmentLicenseRepository(tenant);
            TenantManagmentLicenseList entityList = null;
            TenantManagmentLicense entityPOCO = myTenantManagmentLicenseRepository.GetSingleTenantManagmentLicense(id);

            if (entityPOCO != null)
            {
                List<TenantManagmentLicense> singleEntityList = new List<TenantManagmentLicense>();
                singleEntityList.Add(entityPOCO);

                myTenantManagmentLicenseQuery = new TenantManagmentLicenseQuery(myTenantManagmentLicenseRepository);
                IQueryable<TenantManagmentLicense> iQueryable = singleEntityList.AsQueryable();
                IQueryable<TenantManagmentLicenseList> iQueryableEntityList = myTenantManagmentLicenseQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<TenantManagmentLicenseList> GetTenantManagmentLicenseLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            myTenantManagmentLicenseRepository = new TenantManagmentLicenseRepository(tenant);
            myTenantManagmentLicenseQuery = new TenantManagmentLicenseQuery(myTenantManagmentLicenseRepository);

            IQueryable<TenantManagmentLicense> iQueryable = myTenantManagmentLicenseRepository.GetTenantManagmentLicenses(tenant);
            IQueryable<TenantManagmentLicenseList> query2 = myTenantManagmentLicenseQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<TenantManagmentLicenseList> GetTenantManagmentLicenseFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            myTenantManagmentLicenseRepository = new TenantManagmentLicenseRepository(tenant);
            myTenantManagmentLicenseQuery = new TenantManagmentLicenseQuery(myTenantManagmentLicenseRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<TenantManagmentLicense> iQueryable = myTenantManagmentLicenseRepository.GetTenantManagmentLicenses(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TenantManagmentLicense>(nonListQueryOperation, iQueryable);

            int skippedRecords = queryOperations.PageIndex;

            IQueryable<TenantManagmentLicenseList> query2 = myTenantManagmentLicenseQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TenantManagmentLicenseList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TenantManagmentLicenseList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> entityObjectFields = ObjectFieldsRepository.GetObjectFieldsByObjectTableName("TenantManagmentLicense", tenant).ToList();

                ObjectField objectField = (from a in entityObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<TenantManagmentLicenseList, string>(queryOperations, query2);
                                break;
                            }

                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TenantManagmentLicenseList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TenantManagmentLicenseList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TenantManagmentLicenseList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TenantManagmentLicenseList, bool>(queryOperations, query2);
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

            query2 = query2.Skip(skippedRecords);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetTenantManagmentLicenseFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            myTenantManagmentLicenseRepository = new TenantManagmentLicenseRepository(tenant);
            myTenantManagmentLicenseQuery = new TenantManagmentLicenseQuery(myTenantManagmentLicenseRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<TenantManagmentLicense> iQueryable = myTenantManagmentLicenseRepository.GetTenantManagmentLicenses(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TenantManagmentLicense>(nonListQueryOperation, iQueryable);

            IQueryable<TenantManagmentLicenseList> query2 = myTenantManagmentLicenseQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TenantManagmentLicenseList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        //public void InsertTenantManagmentLicense(TenantManagmentLicensePM entityPM)
        //{
        //    //SecurityUtility.CheckContactFeature("Vessel", "NEW", entityPM.Tenant);

        //    if (objectContext == null)
        //    {
        //        objectContext = CommonDataContext.GetContext(entityPM.Tenant);
        //    }

        //    //VesselService service = new VesselService(objectContext, entityPM.Tenant);
        //    //service.Create(entityPM);

        //    TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "TenantManagmentLicense");
        //}

        //public void UpdateTenantManagmentLicense(TenantManagmentLicensePM entityPM)
        //{
        //    //SecurityUtility.CheckContactFeature("Vessel", "UPDATE", entityPM.Tenant);

        //    if (objectContext == null)
        //    {
        //        objectContext = CommonDataContext.GetContext(entityPM.Tenant);
        //    }

        //    //VesselService service = new VesselService(objectContext, entityPM.Tenant);
        //    //service.Update(entityPM);

        //    TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "TenantManagmentLicense");
        //}

        //public void DeleteTenantManagmentLicense(TenantManagmentLicensePM entity)
        //{

        //}
    }
}