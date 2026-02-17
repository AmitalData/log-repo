using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Helpers;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BookingLib.BL.DataContracts;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public void UpdateAirlineStatisticsList(AirlineStatisticsList currentEntity)
        {
        }

        public IQueryable<AirlineStatistics> GetAirlineStatisticss(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AirlineStatistics", "READ", tenant);

            airlineStatisticsRepository = new AirlineStatisticsRepository(tenant);
            return airlineStatisticsRepository.GetAirlineStatistics(tenant);
        }

        public AirlineStatisticsPM GetSingleAirlineStatistics(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AirlineStatistics", "READ", tenant);

            airlineStatisticsQuery = new AirlineStatisticsQuery(tenant);
            return airlineStatisticsQuery.GetSinglePM(id, tenant);
        }

        public AirlineStatisticsList GetSingleAirlineStatisticsList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AirlineStatistics", "READ", tenant);

            airlineStatisticsRepository = new AirlineStatisticsRepository(tenant);
            airlineStatisticsQuery = new AirlineStatisticsQuery(airlineStatisticsRepository);
            AirlineStatisticsList airlineStatisticsList = null;
            AirlineStatistics airlineStatistics = airlineStatisticsRepository.GetSingleAirlineStatistics(tenant, id);

            if (airlineStatistics != null)
            {
                List<AirlineStatistics> singleEntityList = new List<AirlineStatistics>();
                singleEntityList.Add(airlineStatistics);

                IQueryable<AirlineStatistics> iQueryable = singleEntityList.AsQueryable();
                IQueryable<AirlineStatisticsList> iQueryableEntityList = airlineStatisticsQuery.GetIQueryableEntityList(iQueryable);
                airlineStatisticsList = iQueryableEntityList.FirstOrDefault();
            }
            return airlineStatisticsList;
        }

        public IQueryable<AirlineStatisticsList> GetAirlineStatisticsLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AirlineStatistics", "READ", tenant);

            airlineStatisticsRepository = new AirlineStatisticsRepository(tenant);
            airlineStatisticsQuery = new AirlineStatisticsQuery(airlineStatisticsRepository);

            IQueryable<AirlineStatistics> logs = airlineStatisticsRepository.GetAirlineStatistics(tenant);
            IQueryable<AirlineStatisticsList> query2 = airlineStatisticsQuery.GetIQueryableEntityList(logs);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AirlineStatisticsList> GetAirlineStatisticsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AirlineStatistics", "READ", tenant);

            airlineStatisticsRepository = new AirlineStatisticsRepository(tenant);
            airlineStatisticsQuery = new AirlineStatisticsQuery(airlineStatisticsRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AirlineStatistics> iQueryable = airlineStatisticsRepository.GetAirlineStatistics(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            AirlineStatisticsCustomFilter customfilters = new AirlineStatisticsCustomFilter(tenant);
            iQueryable = customfilters.GetFilteredQuery(queryOperations, iQueryable);

            iQueryable = filter.GetFilteredQuery<AirlineStatistics>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<AirlineStatisticsList> query2 = airlineStatisticsQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<AirlineStatisticsList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AirlineStatisticsList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AirlineStatistics", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AirlineStatisticsList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AirlineStatisticsList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AirlineStatisticsList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AirlineStatisticsList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AirlineStatisticsList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.CreateDate);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.CreateDate);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAirlineStatisticsFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AirlineStatistics", "READ", tenant);

            airlineStatisticsRepository = new AirlineStatisticsRepository(tenant);
            airlineStatisticsQuery = new AirlineStatisticsQuery(airlineStatisticsRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AirlineStatistics> iQueryable = airlineStatisticsRepository.GetAirlineStatistics(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            AirlineStatisticsCustomFilter customfilters = new AirlineStatisticsCustomFilter(tenant);
            iQueryable = customfilters.GetFilteredQuery(queryOperations, iQueryable);

            iQueryable = filter.GetFilteredQuery<AirlineStatistics>(nonListQueryOperation, iQueryable);

            IQueryable<AirlineStatisticsList> query2 = airlineStatisticsQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<AirlineStatisticsList>(listQueryOperation, query2);

            int count = query2.Take(1001).Count();
            return count;
        }

        public IQueryable<AirlineStatisticsPM> GetAirlineStatisticsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AirlineStatistics", "READ", tenant);

            airlineStatisticsQuery = new AirlineStatisticsQuery(tenant);
            return airlineStatisticsQuery.GetAirlineStatisticsPMsByTenant(tenant);
        }

        public void InsertAirlineStatistics(AirlineStatisticsPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            AirlineStatisticsService service = new AirlineStatisticsService(objectContext, entityPM.Tenant);
            service.Create(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "AirlineStatistics");
        }

        public void UpdateAirlineStatistics(AirlineStatisticsPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            AirlineStatisticsService service = new AirlineStatisticsService(objectContext, entityPM.Tenant);
            service.Update(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "AirlineStatistics");
        }

        public void DeleteAirlineStatistics(AirlineStatisticsPM entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }

            airlineStatisticsRepository = new AirlineStatisticsRepository(objectContext);
            AirlineStatistics deletedEntity = airlineStatisticsRepository.GetSingleAirlineStatistics(entity.Tenant, entity.Id);
            airlineStatisticsRepository.Remove(deletedEntity);
        }

        public List<ChartingDataClass> GetDashBoardBookings(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AirlineStatistics", "READ", tenant);

            airlineStatisticsQuery = new AirlineStatisticsQuery(tenant);            

            List<DashBoardBookingClass> data = airlineStatisticsQuery.GetDashBoardBookings(tenant);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            foreach (DashBoardBookingClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.FFRStatusName != null)
                {
                    string[] nameString = item.FFRStatusName.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.linePrimary.ToString(),
                    StringProperty = item.FFRStatusName,
                    IntegerProperty = item.Count,
                    ShortLabelProperty = myShortLabelProperty,
                    Code = item.FFRStatusName,
                });
            }

            return myResult;
        }

        public List<ChartingDataClass> GetTopParticipantsDashBoard(int lastDays, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AirlineStatistics", "READ", tenant);

            airlineStatisticsQuery = new AirlineStatisticsQuery(tenant);

            List<ChartingDataClass> data = airlineStatisticsQuery.GetTopParticipantsDashBoard(lastDays, tenant);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            foreach (ChartingDataClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    ParticipantId = item.ParticipantId,
                    ShortLabelProperty = myShortLabelProperty,
                });
            }

            return myResult;
        }
    }
}