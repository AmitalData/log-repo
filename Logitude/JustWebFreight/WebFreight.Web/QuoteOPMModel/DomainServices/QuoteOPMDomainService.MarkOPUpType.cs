using Amital.QuoteOPM.BL.EntityQueryServices;
using Amital.QuoteOPM.Data;
using Amital.QuoteOPM.Data.EntityListQueryServices;
using Amital.QuoteOPM.Data.EntityLists;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data.Repsitories;
using Amital.QuoteOPM.Def.EntityPMs;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.QuoteOPMModel.DomainServices
{

    public partial class QuotesDomainService
    {
        public MarkUpOPTypeRepository MarkUpOPTypeRepository { get; private set; }

        //private MarkUpOPTypeRepository MarkUpOPTypeRepository;
        //public IQueryable<MarkUpOPType> GetMarkUpOPTypesByTenant(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    MarkUpOPTypeRepository = new MarkUpOPTypeRepository(tenant);
        //    return MarkUpOPTypeRepository.GetMarkUpOPTypes();
        //}

        //public MarkUpOPType GetSingleMarkUpOPType(string code, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    MarkUpOPTypeRepository = new MarkUpOPTypeRepository(tenant);
        //    return MarkUpOPTypeRepository.GetSingleMarkUpOPType(code);
        //}

        //public MarkUpOPTypeList GetSingleMarkUpOPTypeList(string code, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    MarkUpOPTypeRepository = new MarkUpOPTypeRepository(tenant);
        //    MarkUpOPTypeQuery = new MarkUpOPTypeQuery(tenant);

        //    MarkUpOPTypeList MarkUpOPTypeList = null;
        //    MarkUpOPType MarkUpOPType = MarkUpOPTypeRepository.GetSingleMarkUpOPType(code);

        //    if (MarkUpOPType != null)
        //    {
        //        List<MarkUpOPType> singleEntityList = new List<MarkUpOPType>();
        //        singleEntityList.Add(MarkUpOPType);

        //        IQueryable<MarkUpOPType> iQueryable = singleEntityList.AsQueryable();
        //        IQueryable<MarkUpOPTypeList> iQueryableEntityList = MarkUpOPTypeQuery.GetIQueryableEntityList(iQueryable);
        //        MarkUpOPTypeList = iQueryableEntityList.FirstOrDefault();
        //    }
        //    return MarkUpOPTypeList;
        //}

        //public IQueryable<MarkUpOPTypeList> GetMarkUpOPTypeLists(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    MarkUpOPTypeRepository = new MarkUpOPTypeRepository(tenant);
        //    MarkUpOPTypeQuery = new MarkUpOPTypeQueryList(tenant);
        //    IQueryable<MarkUpOPType> iQueryable = MarkUpOPTypeRepository.GetMarkUpOPTypes();
        //    IQueryable<MarkUpOPTypeList> query2 = MarkUpOPTypeQuery.GetIQueryableEntityList(iQueryable);
        //    return query2;
        //}

        //[Query(HasSideEffects = true)]
#if todo


        public IQueryable<MarkUpOPTypeList> GetMarkUpOPTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            MarkUpOPTypeRepository = new MarkUpOPTypeRepository(tenant);
            MarkUpOPTypeQuery = new MarkUpOPTypeQuery(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<MarkUpOPType> iQueryable = MarkUpOPTypeRepository.GetMarkUpOPTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<MarkUpOPType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<MarkUpOPTypeList> query2 = MarkUpOPTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<MarkUpOPTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(MarkUpOPTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("PartnerType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<MarkUpOPTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<MarkUpOPTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<MarkUpOPTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<MarkUpOPTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<MarkUpOPTypeList, bool>(queryOperations, query2);
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

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetMarkUpOPTypeCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            
            MarkUpOPTypeRepository = new MarkUpOPTypeRepository(tenant);
            MarkUpOPTypeQuery = new MarkUpOPTypeQuery(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<MarkUpOPType> iQueryable = MarkUpOPTypeRepository.GetMarkUpOPTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<MarkUpOPType>(nonListQueryOperation, iQueryable);

            IQueryable<MarkUpOPTypeList> query2 = MarkUpOPTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<MarkUpOPTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
#endif

        public void InsertMarkUpOPType(MarkUpOPType entity)
        {
            MarkUpOPTypeRepository.Add(entity);
        }

        public void UpdateMarkUpOPType(MarkUpOPType currentEntity)
        {
            MarkUpOPTypeRepository.Update(currentEntity);
        }

        public void DeleteMarkUpOPType(MarkUpOPType entity)
        {
            MarkUpOPTypeRepository.Remove(entity);
        }
    }
}