using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using System.ServiceModel.DomainServices.Server;
using System.IO;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using System.Reflection;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.CustomFilters;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private AWBSpecialHandlingCodeRepository aWBSpecialHandlingCodeRepository;
        private AWBSpecialHandlingCodeQuery awbSpecialHandlingCodeQuery;

        public void UpdateAWBSpecialHandlingCodeList(AWBSpecialHandlingCodeList currentEntity)
        {

        }

        public AWBSpecialHandlingCodePM GetSingleAWBSpecialHandlingCode(string id, int tenant)
        {             
            SecurityUtility.AuthenticationOnTenant(tenant);
            awbSpecialHandlingCodeQuery = new AWBSpecialHandlingCodeQuery(tenant);
            return awbSpecialHandlingCodeQuery.GetSingleAWBHandlingCodePM(id);
        }

        public AWBSpecialHandlingCodeList GetSingleAWBSpecialHandlingCodeList(string id, int tenant)
        {             
            SecurityUtility.AuthenticationOnTenant(tenant);
            awbSpecialHandlingCodeQuery = new AWBSpecialHandlingCodeQuery(tenant);
            AWBSpecialHandlingCodePM entityPM = awbSpecialHandlingCodeQuery.GetSingleAWBHandlingCodePM(id);
            AWBSpecialHandlingCodeList entityList = new AWBSpecialHandlingCodeList()
            {
                Id = entityPM.Id,
                Code = entityPM.Code,
                Name = entityPM.Name,
                IsIATA = entityPM.IsIATA,
                InActive = entityPM.InActive,
                AirlineId = entityPM.AirlineId,
                SearchFields = entityPM.SearchFields,
            };

            return entityList;
        }

        public IQueryable<AWBSpecialHandlingCodeList> GetAWBSpecialHandlingCodeLists(int tenant)
        {
            aWBSpecialHandlingCodeRepository = new AWBSpecialHandlingCodeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            IQueryable<AWBSpecialHandlingCode> iQueryable = aWBSpecialHandlingCodeRepository.GetAWBHandlingCodes();
            
            var query2 = from entity in iQueryable
                         select new AWBSpecialHandlingCodeList()
                         {
                             Id = entity.Id,
                             Code = entity.Code,
                             Name = entity.Name,
                             IsIATA = entity.IsIATA,
                             InActive = entity.InActive,
                             AirlineId = entity.AirlineId,
                             SearchFields = entity.SearchFields,
                         };

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AWBSpecialHandlingCodeList> GetAWBSpecialHandlingCodeFilters(byte[] xmlFilters, int tenant)
        {
            aWBSpecialHandlingCodeRepository = new AWBSpecialHandlingCodeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AWBSpecialHandlingCode> iQueryable = aWBSpecialHandlingCodeRepository.GetAWBHandlingCodes();

            AWBSpecialHandlingCodeCustomFilter customfilters = new AWBSpecialHandlingCodeCustomFilter(tenant);
            iQueryable = customfilters.GetFilteredQuery(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AWBSpecialHandlingCode>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            var query2 = from entity in iQueryable
                         select new AWBSpecialHandlingCodeList()
                         {
                             Id = entity.Id,
                             Code = entity.Code,
                             Name = entity.Name,
                             IsIATA = entity.IsIATA,
                             InActive = entity.InActive,
                             AirlineId = entity.AirlineId,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<AWBSpecialHandlingCodeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AWBSpecialHandlingCodeList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AWBSpecialHandlingCode", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<AWBSpecialHandlingCodeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AWBSpecialHandlingCodeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AWBSpecialHandlingCodeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AWBSpecialHandlingCodeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AWBSpecialHandlingCodeList, bool>(queryOperations, query2);
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

        public int GetAWBSpecialHandlingCodeFiltersCount(byte[] xmlFilters, int tenant)
        {
            aWBSpecialHandlingCodeRepository = new AWBSpecialHandlingCodeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AWBSpecialHandlingCode> iQueryable = aWBSpecialHandlingCodeRepository.GetAWBHandlingCodes();
            
            AWBSpecialHandlingCodeCustomFilter customfilters = new AWBSpecialHandlingCodeCustomFilter(tenant);
            iQueryable = customfilters.GetFilteredQuery(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AWBSpecialHandlingCode>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new AWBSpecialHandlingCodeList()
                         {
                             Id = entity.Id,
                             Code = entity.Code,
                             Name = entity.Name,
                             IsIATA = entity.IsIATA,
                             InActive = entity.InActive,
                             AirlineId = entity.AirlineId,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<AWBSpecialHandlingCodeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertAWBSpecialHandlingCodePM(AWBSpecialHandlingCodePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            //SecurityUtility.CheckContactFeature("AWBSpecialHandlingCode", "NEW", 0);
            if (objectContext == null)
            {
                objectContext = ShipmentsContext.GetContext(0);
            }

            AWBSpecialHandlingCodeService service = new AWBSpecialHandlingCodeService(objectContext, entityPM);
            service.Create();

            TableLastUpdateClass.UpdateTableHistory(0, "AWBSpecialHandlingCode");
        }

        public void UpdateAWBSpecialHandlingCodePM(AWBSpecialHandlingCodePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            //SecurityUtility.CheckContactFeature("AWBSpecialHandlingCode", "NEW", 0);

            if (objectContext == null)
            {
                objectContext = ShipmentsContext.GetContext(0);
            }

            AWBSpecialHandlingCodeService service = new AWBSpecialHandlingCodeService(objectContext, entityPM);
            service.Update();

            TableLastUpdateClass.UpdateTableHistory(0, "AWBSpecialHandlingCode");
        }
    }
}