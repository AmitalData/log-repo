using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
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
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private AWBInformationRepository aWBInformationRepository;
        private AWBInformationQuery aWBInformationQuery;

        public AWBInformation GetaSingleAWBInformation(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            aWBInformationRepository = new AWBInformationRepository(tenant);
            return aWBInformationRepository.GetSingleAWBInformation(code);
        }

        public AWBInformationPM GetSingleAWBInformation(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            aWBInformationQuery = new AWBInformationQuery(tenant);
            return aWBInformationQuery.GetSingleAWBInformationPM(code);
        }

        public AWBInformationList GetSingleAWBInformationList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            aWBInformationQuery = new AWBInformationQuery(tenant);
            AWBInformationPM entityPM = aWBInformationQuery.GetSingleAWBInformationPM(code);
            AWBInformationList entityList = new AWBInformationList()
            {
                Code = entityPM.Code,
                Name = entityPM.Name,
                SearchFields = entityPM.SearchFields,
            };

            return entityList;
        }

        public IQueryable<AWBInformationList> GetAWBInformationLists(int tenant)
        {
            aWBInformationRepository = new AWBInformationRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            IQueryable<AWBInformation> iQueryable = aWBInformationRepository.GetAWBInformations();
            var query2 = from entity in iQueryable
                         select new AWBInformationList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,

                             SearchFields = entity.SearchFields,
                         };
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AWBInformationList> GetAWBInformationFilters(byte[] xmlFilters, int tenant)
        {
            aWBInformationRepository = new AWBInformationRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AWBInformation> iQueryable = aWBInformationRepository.GetAWBInformations();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AWBInformation>(nonListQueryOperation, iQueryable);

            int skippedEntities = queryOperations.PageIndex;

            var query2 = from entity in iQueryable
                         select new AWBInformationList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<AWBInformationList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AWBInformationList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> entityObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AWBInformation", tenant).ToList();

                ObjectField objectField = (from a in entityObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AWBInformationList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AWBInformationList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AWBInformationList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AWBInformationList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AWBInformationList, bool>(queryOperations, query2);
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

            query2 = query2.Skip(skippedEntities);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAWBInformationFiltersCount(byte[] xmlFilters, int tenant)
        {
            aWBInformationRepository = new AWBInformationRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AWBInformation> iQueryable = aWBInformationRepository.GetAWBInformations();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AWBInformation>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new AWBInformationList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<AWBInformationList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertAWBInformation(AWBInformation entity)
        {
            aWBInformationRepository.Add(entity);
        }

        public void UpdateAWBInformation(AWBInformation currentEntity)
        {
            aWBInformationRepository.Update(currentEntity);
        }

        public void DeleteAWBInformation(AWBInformation entity)
        {
            aWBInformationRepository.Remove(entity);
        }

    }
}