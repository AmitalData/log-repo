using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.GlobalModel.Tools.EntityService;
using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Global.Data.GlobalModel;

namespace WebFreight.Web.GlobalModel
{
    public partial class GlobalDomainService
    {
        public List<PackagePM> GetPackagesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            packageQuery = new PackageQuery();
            return packageQuery.GetPackagePMs();
        }

        public PackagePM GetSinglePackagePM(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            packageQuery = new PackageQuery();

            return packageQuery.GetSinglePM(code);
        }

        public IQueryable<PackageList> GetPackagesLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            packageRepository = new PackageRepository();
            packageQuery = new PackageQuery(packageRepository);

            IQueryable<Package> iQueryable = packageRepository.GetPackages();
            IQueryable<PackageList> query2 = packageQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<PackageList> GetPackageFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            packageRepository = new PackageRepository();
            packageQuery = new PackageQuery(packageRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Package> iQueryable = packageRepository.GetPackages();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Package>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<PackageList> query2 = packageQuery.GetIQueryableEntityList(iQueryable);
            
            query2 = filter.GetFilteredQuery<PackageList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PackageList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Package", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<PackageList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PackageList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PackageList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PackageList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<PackageList, bool>(queryOperations, query2);
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

        public int GetPackageListFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            packageRepository = new PackageRepository();
            packageQuery = new PackageQuery(packageRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Package> iQueryable = packageRepository.GetPackages();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Package>(nonListQueryOperation, iQueryable);

            IQueryable<PackageList> query2 = packageQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<PackageList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertPackage(PackagePM entityPM)
        {
            int tenant = 0;
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = GlobalContext.GetContext();
            }

            PackageService service = new PackageService(objectContext, tenant);
            service.Create(entityPM);
        }

        public void UpdatePackage(PackagePM entityPM)
        {
            int tenant = 0;
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = GlobalContext.GetContext();
            }

            #region ConnectedPackages
            List<PackageConnectedPackagePM> connectedPackageChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ConnectedPackages).Cast<PackageConnectedPackagePM>().ToList();
            foreach (PackageConnectedPackagePM itemPM in connectedPackageChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            PackageService service = new PackageService(objectContext, tenant);
            service.SetChangeSet(connectedPackageChangeSet);
            service.Update(entityPM);
        }

        [Invoke]
        public void InActivePackage(string myPackageCode, bool inActive, int tenant)
        {
            if (!string.IsNullOrEmpty(myPackageCode))
            {
                PackageRepository entityRepository = new PackageRepository();
                Package myPackage = entityRepository.GetSinglePackage(myPackageCode);
                if (myPackage != null)
                {
                    myPackage.InActive = inActive;
                    entityRepository.Update(myPackage);
                    entityRepository.SubmitChanges();
                }
            }
        }
    }
}