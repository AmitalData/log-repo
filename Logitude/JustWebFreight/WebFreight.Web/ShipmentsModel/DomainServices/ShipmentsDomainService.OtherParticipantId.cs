using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
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
        private OtherParticipantIdRepository otherParticipantIdRepository;
        private OtherParticipantIdQuery otherParticipantIdQuery;

        public OtherParticipantId GetaSingleOtherParticipantId(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            otherParticipantIdRepository = new OtherParticipantIdRepository(tenant);
            return otherParticipantIdRepository.GetSingleOtherParticipantId(code);
        }

        public OtherParticipantIdPM GetSingleOtherParticipantIdPM(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            otherParticipantIdQuery = new OtherParticipantIdQuery(tenant);
            return otherParticipantIdQuery.GetSinglePM(code);
        }

        public OtherParticipantIdList GetSingleOtherParticipantIdList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            otherParticipantIdQuery = new OtherParticipantIdQuery(tenant);
            OtherParticipantIdPM entityPM = otherParticipantIdQuery.GetSinglePM(code);
            OtherParticipantIdList entityList = new OtherParticipantIdList()
            {
                Code = entityPM.Code,
                Name = entityPM.Name,
                SearchFields = entityPM.SearchFields,
            };

            return entityList;
        }

        public IQueryable<OtherParticipantIdList> GetOtherParticipantIdLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            otherParticipantIdRepository = new OtherParticipantIdRepository(tenant);
            IQueryable<OtherParticipantId> iQueryable = otherParticipantIdRepository.GetOtherParticipantIds();
            var query2 = from entity in iQueryable
                         select new OtherParticipantIdList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<OtherParticipantIdList> GetOtherParticipantIdFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            otherParticipantIdRepository = new OtherParticipantIdRepository(tenant);
            IQueryable<OtherParticipantId> iQueryable = otherParticipantIdRepository.GetOtherParticipantIds();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<OtherParticipantId>(nonListQueryOperation, iQueryable);

            int skippedEntities = queryOperations.PageIndex;

            var query2 = from a in iQueryable
                         select new OtherParticipantIdList()
                         {
                             Code = a.Code,
                             Name = a.Name,
                             SearchFields = a.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<OtherParticipantIdList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(OtherParticipantIdList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> entityObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("OtherParticipantId", tenant).ToList();

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
                                query2 = sortClass.GetSorterQuery<OtherParticipantIdList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<OtherParticipantIdList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<OtherParticipantIdList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<OtherParticipantIdList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<OtherParticipantIdList, bool>(queryOperations, query2);
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

        public int GetOtherParticipantIdFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            otherParticipantIdRepository = new OtherParticipantIdRepository(tenant);
            IQueryable<OtherParticipantId> iQueryable = otherParticipantIdRepository.GetOtherParticipantIds();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<OtherParticipantId>(nonListQueryOperation, iQueryable);

            var query2 = from a in iQueryable
                         select new OtherParticipantIdList()
                         {
                             Code = a.Code,
                             Name = a.Name,
                             SearchFields = a.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<OtherParticipantIdList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertOtherParticipantId(OtherParticipantId entity)
        {
            otherParticipantIdRepository = new OtherParticipantIdRepository(0);
            otherParticipantIdRepository.Add(entity);
        }

        public void UpdateOtherParticipantId(OtherParticipantId currentEntity)
        {
            otherParticipantIdRepository = new OtherParticipantIdRepository(0);
            otherParticipantIdRepository.Update(currentEntity);
        }

        public void DeleteOtherParticipantId(OtherParticipantId entity)
        {

        }
    }
}