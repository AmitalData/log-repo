using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using System.Reflection;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private AWBCustomsInformationRepository aWBCustomsInformationRepository;
        private AWBCustomsInformationQuery aWBCustomsInformationQuery;

        public AWBCustomsInformation GetaSingleAWBCustomsInformation(string code, int tenant)
        {
            aWBCustomsInformationRepository = new AWBCustomsInformationRepository(tenant);
            return aWBCustomsInformationRepository.GetSingleAWBCustomsInfo(code);
        }

        public AWBCustomsInformationPM GetSingleAWBCustomsInformation(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            aWBCustomsInformationQuery = new AWBCustomsInformationQuery(tenant);
            return aWBCustomsInformationQuery.GetSingleAWBCustomsInformationPM(code);
        }

        public AWBCustomsInformationList GetSingleAWBCustomsInformationList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            aWBCustomsInformationQuery = new AWBCustomsInformationQuery(tenant);
            AWBCustomsInformationPM entityPM = aWBCustomsInformationQuery.GetSingleAWBCustomsInformationPM(code);
            AWBCustomsInformationList entityList = new AWBCustomsInformationList()
            {
                Code = entityPM.Code,
                Name = entityPM.Name,
                SearchFields = entityPM.SearchFields,
            };

            return entityList;
        }

        public IQueryable<AWBCustomsInformationList> GetAWBCustomsInformationLists(int tenant)
        {
            aWBCustomsInformationRepository = new AWBCustomsInformationRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            IQueryable<AWBCustomsInformation> iQueryable = aWBCustomsInformationRepository.GetAWBCustomsInfos();
            var query2 = from entity in iQueryable
                         select new AWBCustomsInformationList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,

                             SearchFields = entity.SearchFields,
                         };
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AWBCustomsInformationList> GetAWBCustomsInformationFilters(byte[] xmlFilters, int tenant)
        {
            aWBCustomsInformationRepository = new AWBCustomsInformationRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AWBCustomsInformation> iQueryable = aWBCustomsInformationRepository.GetAWBCustomsInfos();

            //PortCustomFilter customfilters = new PortCustomFilter(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AWBCustomsInformation>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new AWBCustomsInformationList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,

                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<AWBCustomsInformationList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AWBCustomsInformationList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AWBCustomsInformation", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AWBCustomsInformationList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AWBCustomsInformationList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AWBCustomsInformationList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AWBCustomsInformationList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AWBCustomsInformationList, bool>(queryOperations, query2);
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

        public int GetAWBCustomsInformationFiltersCount(byte[] xmlFilters, int tenant)
        {
            aWBCustomsInformationRepository = new AWBCustomsInformationRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AWBCustomsInformation> iQueryable = aWBCustomsInformationRepository.GetAWBCustomsInfos();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AWBCustomsInformation>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new AWBCustomsInformationList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,

                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<AWBCustomsInformationList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertAWBCustomsInformation(AWBCustomsInformation entity)
        {
            aWBCustomsInformationRepository.Add(entity);
        }

        public void UpdateAWBCustomsInformation(AWBCustomsInformation currentEntity)
        {
            aWBCustomsInformationRepository.Update(currentEntity);
        }

        public void DeleteAWBCustomsInformation(AWBCustomsInformation entity)
        {
            aWBCustomsInformationRepository.Remove(entity);
        }
    }
}