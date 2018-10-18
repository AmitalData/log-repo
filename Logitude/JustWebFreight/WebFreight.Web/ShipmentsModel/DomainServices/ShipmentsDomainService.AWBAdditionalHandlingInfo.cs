using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
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

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private AWBAdditionalHandlingInfoQuery aWBAdditionalHandlingInfoQuery;
        private AWBAdditionalHandlingInfoRepository aWBAdditionalHandlingInfoRepository;

        public AWBAdditionalHandlingInfoPM GetSingleAWBAdditionalHandlingInfoPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aWBAdditionalHandlingInfoQuery = new AWBAdditionalHandlingInfoQuery(tenant);
            return aWBAdditionalHandlingInfoQuery.GetSingleAWBInformationPM(id);
        }

        public AWBAdditionalHandlingInfoList GetSingleAWBAdditionalHandlingInfoList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aWBAdditionalHandlingInfoQuery = new AWBAdditionalHandlingInfoQuery(tenant);
            AWBAdditionalHandlingInfoPM entityPM = aWBAdditionalHandlingInfoQuery.GetSingleAWBInformationPM(id);

            AWBAdditionalHandlingInfoList entityList = new AWBAdditionalHandlingInfoList()
            {
                Id = entityPM.Id,
                Tenant = entityPM.Tenant,
                Code = entityPM.Code,
                Name = entityPM.Name,
                PrintDescription = entityPM.PrintDescription,
                SearchFields = entityPM.SearchFields,
            };

            return entityList;
        }

        public IQueryable<AWBAdditionalHandlingInfoList> GetAWBAdditionalHandlingInfoLists(int tenant)
        {            
            SecurityUtility.AuthenticationOnTenant(tenant);

            aWBAdditionalHandlingInfoRepository = new AWBAdditionalHandlingInfoRepository(tenant);
            IQueryable<AWBAdditionalHandlingInfo> iQueryable = aWBAdditionalHandlingInfoRepository.GetAWBAdditionalHandlingInfos(tenant);

            var query2 = from a in iQueryable
                         select new AWBAdditionalHandlingInfoList()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             Code = a.Code,
                             Name = a.Name,
                             PrintDescription = a.PrintDescription,
                             SearchFields = a.SearchFields,
                         };

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AWBAdditionalHandlingInfoList> GetAWBAdditionalHandlingInfoFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            aWBAdditionalHandlingInfoRepository = new AWBAdditionalHandlingInfoRepository(tenant);
            IQueryable<AWBAdditionalHandlingInfo> iQueryable = aWBAdditionalHandlingInfoRepository.GetAWBAdditionalHandlingInfos(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AWBAdditionalHandlingInfo>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            var query2 = from a in iQueryable
                         select new AWBAdditionalHandlingInfoList()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             Code = a.Code,
                             Name = a.Name,
                             PrintDescription = a.PrintDescription,
                             SearchFields = a.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<AWBAdditionalHandlingInfoList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AWBAdditionalHandlingInfoList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> entityObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AWBAdditionalHandlingInfo", tenant).ToList();

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
                                query2 = sortClass.GetSorterQuery<AWBAdditionalHandlingInfoList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AWBAdditionalHandlingInfoList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AWBAdditionalHandlingInfoList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AWBAdditionalHandlingInfoList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AWBAdditionalHandlingInfoList, bool>(queryOperations, query2);
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
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAWBAdditionalHandlingInfoFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
                        
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            aWBAdditionalHandlingInfoRepository = new AWBAdditionalHandlingInfoRepository(tenant);
            IQueryable<AWBAdditionalHandlingInfo> iQueryable = aWBAdditionalHandlingInfoRepository.GetAWBAdditionalHandlingInfos(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AWBAdditionalHandlingInfo>(nonListQueryOperation, iQueryable);

            var query2 = from a in iQueryable
                         select new AWBAdditionalHandlingInfoList()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             Code = a.Code,
                             Name = a.Name,
                             PrintDescription = a.PrintDescription,
                             SearchFields = a.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<AWBAdditionalHandlingInfoList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertAWBAdditionalHandlingInfo(AWBAdditionalHandlingInfoPM entityPM)
        {

        }

        public void UpdateAWBAdditionalHandlingInfo(AWBAdditionalHandlingInfoPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("AWBAdditionalHandlingInfo", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = ShipmentsContext.GetContext(entityPM.Tenant);
            }

            AWBAdditionalHandlingInfoService service = new AWBAdditionalHandlingInfoService(objectContext, entityPM, ServiceContext.User.Identity.Name);
            service.Update();
        }

        public void DeleteAWBAdditionalHandlingInfo(AWBAdditionalHandlingInfoPM entityPM)
        {

        }

    }
}