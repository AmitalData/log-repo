using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Http;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using WebFreight.Web.WebServices;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.InfrastructureModel.EntityQueries;
using System.IdentityModel.Protocols.WSTrust;
using Logitude.SystemLogs;
using WebFreight.Web.App_Code;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;

namespace WebFreight.Web
{
    public class ShipmentsController : ApiController
    {
        public ShipmentsController()
        {
                
        }

        public ShipmentPM Get(string id)
        {
            return null;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getsinglepmwithoutcomposition/{id}/{tenant}")]
        public ShipmentPM GetSingleShipmentPMWithoutComposition(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            ShipmentPM pm = shipmentQuery.GetSinglePMWithoutComposition(id, tenant);
            CheckSharedContactAuthenticationForShipment(pm.AgentId, pm.CustomerId, tenant);
            return pm;
        }

        public List<ShipmentList> PostFilteredShipmentsForMobile(int tenant, bool ismobile, ShipmentFilters filters)
        {

           // string Id = Guid.NewGuid().ToString();
           // DateTime maindateStart = DateTime.Now;
           // string mainstarttime = maindateStart.ToString();

           // AzureLog.SaveLogsInStorage(Id + "      Start Post Filtered Shipments      " + "Start Date  : " + mainstarttime, "P", DateTime.Now, "", "", 0, filters.ContactId, "", HttpContext.Current.Request.UserHostAddress);


            DateTime DateBeforeGetShipmentList = DateTime.Now;
        
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckSharedContactAuthentication(tenant, filters.PartnerId);


            filters.PageSize = filters.PageSize == 0 ? 10 : filters.PageSize;

            List<ShipmentList> listQuery = new List<ShipmentList>();

            List<ShipmentList> ReslutFollowShipments = new List<ShipmentList>();
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM currentTenant = tenantQuery.GetSinglePM(tenant); //From Cach


            ObjectTableRepository rep = new ObjectTableRepository(tenant);
            ObjectTable table = rep.GetObjectTableByName("Shipment", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
    

            ContactsUnseenEntitieRepository contactsUnseenRepository = new ContactsUnseenEntitieRepository(tenant);
            SharedFollowedShipmentRepository sharedFollowedShipmentRepository = new SharedFollowedShipmentRepository(tenant);


            QueryOperations queryOperations = new QueryOperations();
            string value = "";
            string name = "";
            if (filters.PartnerType == "CS")
            {
                value = "D,H,A"; name = "CustomerId";
            }
            else

                if (filters.PartnerType == "AG")
                {
                    value = "D,C";
                    name = "AgentId";
                }

            queryOperations.SetFilter(name, filters.PartnerId, false, "Equals", null, false);
            queryOperations.SetFilter("ShipmentLevelCode", value, false, "InList", null, false);
            if (!string.IsNullOrEmpty(filters.SearchField))
            {
                queryOperations.SetFilter("SearchFields", filters.SearchField, false, "Contains", null, false);
            }
            IQueryable<ShipmentList> Allshipments = null;
            #region  AllShipment

            if (!filters.IsShipmentTracking)
            {

                if (filters.DirectionId == "I")
                {
                    queryOperations.SetFilter("DirectionId", "I,C", false, "InList", null, false);
                }

                else
                {
                    queryOperations.SetFilter("DirectionId", filters.DirectionId, false, "Equals", null, true);

                }
                queryOperations.SetFilter("IsOperationalClosed", filters.IsOperationalClosed, false, "Equals", null, true);

                if (!string.IsNullOrEmpty(filters.TransportModeId))
                {
                    queryOperations.SetFilter("TransportModeId", filters.TransportModeId, false, "Equals", null, true);
                }

                if (!string.IsNullOrEmpty(filters.QueryType))
                {

                    if (string.IsNullOrEmpty(filters.DirectionId) || string.IsNullOrWhiteSpace(filters.DirectionId)) filters.DirectionId = null;

                    queryOperations.SetFilter("DeparturesArrivalsMobileFilter", filters.QueryType, true, "Equals", filters.DirectionId, false);

                }




                GenericFilter filter = new GenericFilter();
                GenericSort sortClass = new GenericSort();

                ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);

                // DateTime startdateGetShipment = DateTime.Now;

                //   AzureLog.SaveLogsInStorage(Id + "      Start GetShipmentViewsByTenant      " + "Start Date:  " + startdateGetShipment.ToString(), "P", DateTime.Now, "", "", 0, filters.ContactId, "", HttpContext.Current.Request.UserHostAddress);
                
                IQueryable<ShipmentList> shipments = GetShipmentList(tenant, shipmentRepository);
              
                // TimeSpan timespantotalGetShipment = DateTime.Now - startdateGetShipment;
                // AzureLog.SaveLogsInStorage(Id + "      End GetShipmentViewsByTenant      " + "Total Date:  " + timespantotalGetShipment.ToString(), "P", DateTime.Now, "", "", 0, filters.ContactId, "", HttpContext.Current.Request.UserHostAddress);


                shipments = customfilters.GetFilteredQuery(queryOperations, shipments);

                 Allshipments = shipments;

                if ( !string.IsNullOrEmpty(filters.QueryType) && filters.QueryType == "LSW_EXP")
                {
                    EntityStatusRepository entityStatusRepository = new EntityStatusRepository(tenant);
                    string ArrivalsStatsId = entityStatusRepository.GetEntityStatusArrivals(tenant);

                    shipments = (from a in shipments
                                 where (a.DirectionId == "C" && a.CustomConnectToShipment == false && a.StatusId != ArrivalsStatsId) || (a.DirectionId != "C" && a.StatusId != ArrivalsStatsId)
                                 select a);
                }
                else
                {

                shipments = (from a in shipments
                                 where (a.DirectionId == "C" && a.CustomConnectToShipment == false) || a.DirectionId != "C"
                                 select a);
                }

            

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                shipments = filter.GetFilteredQuery<ShipmentList>(nonListQueryOperation, shipments);
                int skippedShipments = queryOperations.PageIndex;

               // var query2 = BuildShipmentList(currentTenant, shipments);
                var query2 = shipments;
                query2 = filter.GetFilteredQuery<ShipmentList>(listQueryOperation, query2);

                if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
                {
                    PropertyInfo propInfo = typeof(ShipmentList).GetProperty(queryOperations.SortByColumnName);
                    List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Shipment", tenant).ToList();

                    ObjectField objectField = (from a in shipmentObjectFields
                                               where a.FieldName == queryOperations.SortByColumnName
                                               select a).FirstOrDefault();

                    if (objectField != null)
                    {
                        if (!objectField.IsCustom)
                        {
                            switch (objectField.DataTypeCode.ToLower())
                            {
                                case "text":
                                    {
                                        query2 = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, query2);
                                        break;
                                    }
                                case "double":
                                    {
                                        query2 = sortClass.GetSorterQuery<ShipmentList, double>(queryOperations, query2);
                                        break;
                                    }
                                case "datetime":
                                    {
                                        query2 = sortClass.GetSorterQuery<ShipmentList, DateTime>(queryOperations, query2);
                                        break;
                                    }
                                case "integer":
                                    {
                                        query2 = sortClass.GetSorterQuery<ShipmentList, int>(queryOperations, query2);
                                        break;
                                    }
                                case "lookup":
                                    {
                                        query2 = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, query2);
                                        break;
                                    }
                                case "boolean":
                                    {
                                        query2 = sortClass.GetSorterQuery<ShipmentList, bool>(queryOperations, query2);
                                        break;
                                    }
                                default:
                                    {
                                        query2 = query2.OrderByDescending(d => d.StatusDate);
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            query2 = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, query2);
                        }
                    }
                }
                else
                {
                    //query2 = query2.OrderByDescending(d => d.LastStatusLogDate);
                    query2 = query2.OrderByDescending(d => d.StatusDate);
                }


                query2 = query2.Skip(filters.PageIndex);
                query2 = query2.Take(filters.PageSize);

                listQuery = query2.ToList();


                BuildUnssenFollowedShipment(tenant, listQuery, table, filters.ContactId, contactsUnseenRepository, sharedFollowedShipmentRepository, null);


            }

            #endregion

            #region Follow Shipment

            else
            {
                GenericFilter filter = new GenericFilter();
                GenericSort sortClass = new GenericSort();
                ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);


                List<string> SharedFollowedShipmentListIds = (from a in sharedFollowedShipmentRepository.context.SharedFollowedShipments
                                                              where a.ContactId == filters.ContactId && a.Tenant == tenant
                                                              orderby a.TrackDate descending
                                                              select a.ShipmentId).Skip(filters.PageIndex).Take(filters.PageSize).ToList();




                IQueryable<ShipmentList> shipments = GetFollowedShipmentList(tenant, shipmentRepository, SharedFollowedShipmentListIds);
                Allshipments = shipments;
                shipments = customfilters.GetFilteredQuery(queryOperations, shipments);

                shipments = (from a in shipments
                             where (a.DirectionId == "C" && a.CustomConnectToShipment == false) || a.DirectionId != "C"
                             select a);

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                shipments = filter.GetFilteredQuery<ShipmentList>(nonListQueryOperation, shipments);

                var query2 = shipments;


                query2 = filter.GetFilteredQuery<ShipmentList>(listQueryOperation, query2);
                query2 = query2.OrderByDescending(d => d.StatusDate);
                listQuery = query2.ToList();

                BuildUnssenFollowedShipment(tenant, listQuery, table, filters.ContactId, contactsUnseenRepository, sharedFollowedShipmentRepository, SharedFollowedShipmentListIds);


               
            }

            #endregion



            #region  MobileShipmentReference

            List<string> customListShipmentIds = new List<string>();
            foreach(ShipmentList shipmentList in listQuery)
            {
                shipmentList.MobileShipmentReference = GetMobileReference(null, shipmentList);

                if (LogitudeSettings.WorkEnvironment == "cloud" && !string.IsNullOrEmpty( shipmentList.CustomFileId) && string.IsNullOrEmpty(shipmentList.MobileShipmentReference))
                {
                    customListShipmentIds.Add(shipmentList.CustomFileId);
                }
            }

            //CustomShipment Ref
            if (LogitudeSettings.WorkEnvironment == "cloud")
            {
                if (customListShipmentIds.Count > 0)
                {
                    List<ShipmentList> customShipmentList = Allshipments.Where(d => customListShipmentIds.Contains(d.Id)).ToList();

                    if (customShipmentList.Count > 0)
                    {
                        foreach (ShipmentList customShipment in customShipmentList)
                        {
                            string _ref = GetMobileReference(null, customShipment);

                            foreach (ShipmentList shipmentList in listQuery.Where(d => d.CustomFileId == customShipment.Id))
                            {
                                if (string.IsNullOrEmpty(shipmentList.MobileShipmentReference))
                                {
                                    shipmentList.MobileShipmentReference = _ref;
                                }
                            }
                        }

                    }

                }
            }

           //
            #endregion


            int executionTime = (int)((DateTime.Now.Ticks - DateBeforeGetShipmentList.Ticks) / TimeSpan.TicksPerMillisecond);

            if (HttpContext.Current.Response.Headers["ServerTime"] != null)
            {
                HttpContext.Current.Response.Headers["ServerTime"] = executionTime.ToString();
            }
            else HttpContext.Current.Response.Headers.Add("ServerTime", executionTime.ToString());

            return listQuery;
        }

        private static IQueryable<ShipmentList> GetShipmentList(int tenant, ShipmentRepository shipmentRepository)
        {
            IQueryable<ShipmentList> shipments = from entity in shipmentRepository.context.Shipments.Include("Ports").Include("ComputedEntityStatus")//.Include("ShipperCard").Include("ConsigneeCard")
                                                 join sm in shipmentRepository.context.ShipmentMasterDatas.Include("Port").Include("Port.Country")
                                                 on entity.MasterShipmentDataId equals sm.Id into shipmentJoin
                                                 from m in shipmentJoin.DefaultIfEmpty()
                                                 where entity.Tenant == tenant && !entity.IsCancelled
                                                 select new ShipmentList()
                                                 {
                                                     DirectionId = entity.DirectionId,
                                                     DirectionName = entity.Direction.Name,
                                                     TransportModeId = entity.TransportModeId,
                                                     FromPortName = !string.IsNullOrEmpty(m.MainCarriageFromPort.EnglishName) ? m.MainCarriageFromPort.EnglishName : entity.FromPort.EnglishName,
                                                     ToPortName = !string.IsNullOrEmpty(m.MainCarriageToPort.EnglishName) ? m.MainCarriageToPort.EnglishName : entity.ToPort.EnglishName,
                                                     HasException = entity.HasException,
                                                     ExceptionDescription = entity.ExceptionDescription,
                                                     ExceptionResolvedDescription = entity.ExceptionResolvedDescription,
                                                     LastExceptionDescription = entity.LastExceptionDescription,
                                                     
                                                     ExceptionDate = entity.ExceptionDate,
                                                     ShipperReference1 = entity.ShipperReference1,
                                                     ShipperReference2 = entity.ShipperReference2,
                                                     AgentReference1 = entity.AgentReference1,
                                                     AgentReference2 = entity.AgentReference2,
                                                     ConsigneeReference1 = entity.ConsigneeReference1,
                                                     ConsigneeReference2 = entity.ConsigneeReference2,
                                                     ShipmentLevelCode = entity.ShipmentLevelCode,
                                                     CreateDateTime = entity.CreateDateTime,
                                                     StatusDate = entity.ComputedStatusDate,
                                                     StatusName = entity.ComputedEntityStatus != null ? entity.ComputedEntityStatus.Name : "", //entity.MasterShipmentDataId != null ? (m.EntityStatus.StatusWeight > entity.EntityStatus.StatusWeight ? m.EntityStatus.Name : entity.EntityStatus.Name) : (entity.EntityStatus.Name),//f.StatusName,
                                                     StatusId = entity.ComputedStatusId, //entity.MasterShipmentDataId != null ? (m.EntityStatus.StatusWeight > entity.EntityStatus.StatusWeight ? m.EntityStatus.Id : entity.StatusId) : (entity.StatusId),//f.StatusName,
                                                     CustomerShipmentNumber = entity.CustomerShipmentNumber,
                                                     Shipper = entity.ShipperName, //entity.ShipperCard != null ? entity.ShipperCard.EnglishName : "",
                                                     Consignee = entity.ConsigneeName,//entity.ConsigneeCard !=null? entity.ConsigneeCard.EnglishName: "",
                                                     IsCancelled = false,
                                                     CustomFileNumber = entity.CustomFileNumber,
                                                     CustomerId = entity.CustomerId,
                                                     IsOperationalClosed = false,
                                                     Id = entity.Id,
                                                     CustomFileId = entity.CustomFileId,
                                                     ShipmentNumber = entity.ShipmentNumber,
                                                     IsShipmentTracking = false,
                                                     IsOccurChange = false,
                                                     SearchFields = entity.SearchFields,
                                                     ShipmentMasterDataId = m.Id,
                                                     MainCarriageATD = m.MainCarriageATD,
                                                     MainCarriageATA = m.MainCarriageATA,
                                                     MasterShipmentNumber = m.MasterShipmentNumber,
                                                     MainCarriageETA = m.MainCarriageETA,
                                                     MainCarriageETD = m.MainCarriageETD,
                                                     CustomConnectToShipment = entity.CustomConnectToShipment,
                                                     ForeignPartnerCountryCode = entity.ForeignPartnerCountryCode,
                                                     House = entity.House,
                                                     LongMaster = entity.TransportModeId == "A" ? (m.AirlinePrefix != null && m.Master != null ? m.AirlinePrefix + "-" + m.Master : m.Master) : m.Master,
                                                   
                                                 };
            return shipments;
        }

        private static IQueryable<ShipmentList> GetFollowedShipmentList(int tenant, ShipmentRepository shipmentRepository, List<string> SharedFollowedShipmentListIds)
        {
            IQueryable<ShipmentList> shipments = from entity in shipmentRepository.context.Shipments.Include("Ports").Include("ComputedEntityStatus")//.Include("ShipperCard").Include("ConsigneeCard")
                                                 join sm in shipmentRepository.context.ShipmentMasterDatas.Include("Port").Include("Port.Country")
                                                 on entity.MasterShipmentDataId equals sm.Id into shipmentJoin
                                                 from m in shipmentJoin.DefaultIfEmpty()
                                                 where entity.Tenant == tenant && !entity.IsCancelled && SharedFollowedShipmentListIds.Contains(entity.Id)
                                                 select new ShipmentList()
                                                 {
                                                     DirectionId = entity.DirectionId,
                                                     DirectionName = entity.Direction.Name,
                                                     TransportModeId = entity.TransportModeId,
                                                     FromPortName = !string.IsNullOrEmpty(m.MainCarriageFromPort.EnglishName) ? m.MainCarriageFromPort.EnglishName : entity.FromPort.EnglishName,
                                                     ToPortName = !string.IsNullOrEmpty(m.MainCarriageToPort.EnglishName) ? m.MainCarriageToPort.EnglishName : entity.ToPort.EnglishName,
                                                     HasException = entity.HasException,
                                                     ExceptionDescription = entity.ExceptionDescription,
                                                     ExceptionResolvedDescription = entity.ExceptionResolvedDescription,
                                                     LastExceptionDescription = entity.LastExceptionDescription,
                                                     
                                                     ExceptionDate = entity.ExceptionDate,
                                                     ShipperReference1 = entity.ShipperReference1,
                                                     ShipperReference2 = entity.ShipperReference2,
                                                     AgentReference1 = entity.AgentReference1,
                                                     AgentReference2 = entity.AgentReference2,
                                                     ConsigneeReference1 = entity.ConsigneeReference1,
                                                     ConsigneeReference2 = entity.ConsigneeReference2,
                                                     ShipmentLevelCode = entity.ShipmentLevelCode,
                                                     CreateDateTime = entity.CreateDateTime,
                                                     StatusDate = entity.ComputedStatusDate,
                                                     StatusName = entity.ComputedEntityStatus != null ? entity.ComputedEntityStatus.Name : "", //entity.MasterShipmentDataId != null ? (m.EntityStatus.StatusWeight > entity.EntityStatus.StatusWeight ? m.EntityStatus.Name : entity.EntityStatus.Name) : (entity.EntityStatus.Name),//f.StatusName,
                                                     StatusId = entity.ComputedStatusId, //entity.MasterShipmentDataId != null ? (m.EntityStatus.StatusWeight > entity.EntityStatus.StatusWeight ? m.EntityStatus.Id : entity.StatusId) : (entity.StatusId),//f.StatusName,
                                                     CustomerShipmentNumber = entity.CustomerShipmentNumber,
                                                     Shipper = entity.ShipperName, //entity.ShipperCard != null ? entity.ShipperCard.EnglishName : "",
                                                     Consignee = entity.ConsigneeName,//entity.ConsigneeCard !=null? entity.ConsigneeCard.EnglishName: "",
                                                     IsCancelled = false,
                                                     CustomFileNumber = entity.CustomFileNumber,
                                                     CustomerId = entity.CustomerId,
                                                     IsOperationalClosed = false,
                                                     Id = entity.Id,
                                                     CustomFileId = entity.CustomFileId,
                                                     ShipmentNumber = entity.ShipmentNumber,
                                                     IsShipmentTracking = false,
                                                     IsOccurChange = false,
                                                     SearchFields = entity.SearchFields,
                                                     ShipmentMasterDataId = m.Id,
                                                     MainCarriageATD = m.MainCarriageATD,
                                                     MainCarriageATA = m.MainCarriageATA,
                                                     MasterShipmentNumber = m.MasterShipmentNumber,
                                                     MainCarriageETA = m.MainCarriageETA,
                                                     MainCarriageETD = m.MainCarriageETD,
                                                     CustomConnectToShipment = entity.CustomConnectToShipment,
                                                     ForeignPartnerCountryCode = entity.ForeignPartnerCountryCode,
                                                 };
            return shipments;
        }


        public List<ShipmentList> PostFilteredShipments(int tenant, ShipmentFilters filters)
        {

            //string Id = Guid.NewGuid().ToString();
           // DateTime maindateStart = DateTime.Now;
           // string mainstarttime = maindateStart.ToString();

          //  AzureLog.SaveLogsInStorage(Id +  "      Start Post Filtered Shipments      " + "Start Date  : " + mainstarttime , "P", DateTime.Now, "", "", 0, filters.ContactId, "", HttpContext.Current.Request.UserHostAddress);

          SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckSharedContactAuthentication(tenant, filters.PartnerId);

          
            filters.PageSize = filters.PageSize == 0 ? 10 : filters.PageSize;

            List<ShipmentList> listQuery = new List<ShipmentList>();

            List<ShipmentList> ReslutFollowShipments = new List<ShipmentList>();
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM currentTenant = tenantQuery.GetSinglePM(tenant); //From Cach


                ObjectTableRepository rep = new ObjectTableRepository(tenant);
                ObjectTable table = rep.GetObjectTableByName("Shipment", 0, true);
                string email = HttpContext.Current.User.Identity.Name;
                if (string.IsNullOrEmpty(filters.ContactId))
                {

                  //  DateTime StartDateGetSingleContactByEmail = DateTime.Now;
                   // AzureLog.SaveLogsInStorage(Id + "      Start GetSingleContactByEmail      " + "Start Date   : " + StartDateGetSingleContactByEmail.ToString(), "P", DateTime.Now, "", "", 0, filters.ContactId, "", HttpContext.Current.Request.UserHostAddress);         
                    ContactRepository contactRepository = new ContactRepository(tenant);
                Contact contact = contactRepository.GetSingleContactByEmail(email, tenant, true);

                  //  TimeSpan timespantotalGetSingleContactByEmail = DateTime.Now - StartDateGetSingleContactByEmail;
                  //  AzureLog.SaveLogsInStorage(Id + "      End GetSingleContactByEmail      " + "  Total Date   : " + timespantotalGetSingleContactByEmail.ToString(), "P", DateTime.Now, "", "", 0, filters.ContactId, "", HttpContext.Current.Request.UserHostAddress);
               
                    if (contact != null) filters.ContactId = contact.Id;
                }

                ContactsUnseenEntitieRepository contactsUnseenRepository = new ContactsUnseenEntitieRepository(tenant);
                SharedFollowedShipmentRepository sharedFollowedShipmentRepository = new SharedFollowedShipmentRepository(tenant);


            QueryOperations queryOperations = new QueryOperations();
                string value = "";
                string name = "";
                if (filters.PartnerType == "CS")
                {
                    value = "D,H,A"; name = "CustomerId";
                    }
                    else
                     
                    if (filters.PartnerType == "AG")
                    {
                        value = "D,C"; 
                        name = "AgentId";
                    }

                queryOperations.SetFilter(name, filters.PartnerId, false, "Equals", null, false);
                queryOperations.SetFilter("ShipmentLevelCode", value, false, "InList", null, false);
                if (!string.IsNullOrEmpty(filters.SearchField))
                {
                    queryOperations.SetFilter("SearchFields", filters.SearchField, false, "Contains", null, false);
                }

           #region  AllShipment

           if (!filters.IsShipmentTracking)
                 {

                     if (filters.DirectionId == "I")
                     {
                         queryOperations.SetFilter("DirectionId", "I,C", false, "InList", null, false);
                     }

                     else
                     {
                         queryOperations.SetFilter("DirectionId", filters.DirectionId, false, "Equals", null, true);

                     }
               queryOperations.SetFilter("IsOperationalClosed", filters.IsOperationalClosed, false, "Equals", null, true);

               if (!string.IsNullOrEmpty(filters.TransportModeId))
               {
                   queryOperations.SetFilter("TransportModeId", filters.TransportModeId, false, "Equals", null, true);
               }

                   if (!string.IsNullOrEmpty(filters.QueryType))
                   {

                       if (string.IsNullOrEmpty(filters.DirectionId) || string.IsNullOrWhiteSpace(filters.DirectionId)) filters.DirectionId = null;
                       
                       queryOperations.SetFilter("DeparturesArrivalsMobileFilter", filters.QueryType, true, "Equals", filters.DirectionId, false);

                   }




            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);

          //  DateTime startdateGetShipment = DateTime.Now;

         //   AzureLog.SaveLogsInStorage(Id + "      Start GetShipmentViewsByTenant      " + "Start Date:  " + startdateGetShipment.ToString(), "P", DateTime.Now, "", "", 0, filters.ContactId, "", HttpContext.Current.Request.UserHostAddress);
            IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant);
           // TimeSpan timespantotalGetShipment = DateTime.Now - startdateGetShipment;
           // AzureLog.SaveLogsInStorage(Id + "      End GetShipmentViewsByTenant      " + "Total Date:  " + timespantotalGetShipment.ToString(), "P", DateTime.Now, "", "", 0, filters.ContactId, "", HttpContext.Current.Request.UserHostAddress);


            shipments = customfilters.GetFilteredQuery(queryOperations, shipments);

            shipments = (from a in shipments
                         where ((a.DirectionId == "C" && a.CustomConnectToShipment == false) || a.DirectionId != "C") && !a.IsCancelled
                         select a);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            shipments = filter.GetFilteredQuery<ShipmentDataView>(nonListQueryOperation, shipments);
            int skippedShipments = queryOperations.PageIndex;

            var query2 = BuildShipmentList(currentTenant, shipments);

            query2 = filter.GetFilteredQuery<ShipmentList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ShipmentList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Shipment", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    if (!objectField.IsCustom)
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "text":
                                {
                                    query2 = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, query2);
                                    break;
                                }
                            case "double":
                                {
                                    query2 = sortClass.GetSorterQuery<ShipmentList, double>(queryOperations, query2);
                                    break;
                                }
                            case "datetime":
                                {
                                    query2 = sortClass.GetSorterQuery<ShipmentList, DateTime>(queryOperations, query2);
                                    break;
                                }
                            case "integer":
                                {
                                    query2 = sortClass.GetSorterQuery<ShipmentList, int>(queryOperations, query2);
                                    break;
                                }
                            case "lookup":
                                {
                                    query2 = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, query2);
                                    break;
                                }
                            case "boolean":
                                {
                                    query2 = sortClass.GetSorterQuery<ShipmentList, bool>(queryOperations, query2);
                                    break;
                                }
                            default:
                                {
                                    query2 = query2.OrderByDescending(d => d.StatusDate);
                                    break;
                                }
                        }
                    }
                    else
                    {
                        query2 = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, query2);
                    }
                }
            }
            else
            {
                //query2 = query2.OrderByDescending(d => d.LastStatusLogDate);
                query2 = query2.OrderByDescending(d => d.StatusDate);
            }


            query2 = query2.Skip(filters.PageIndex);
            query2 = query2.Take(filters.PageSize);

            listQuery = query2.ToList();

               
                BuildUnssenFollowedShipment(tenant, listQuery, table, filters.ContactId, contactsUnseenRepository, sharedFollowedShipmentRepository, null);
                
                                    
           }

           #endregion

           #region Follow Shipment

                    else
                    {
                GenericFilter filter = new GenericFilter();
                GenericSort sortClass = new GenericSort();
                ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);

                IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant);
                shipments = customfilters.GetFilteredQuery(queryOperations, shipments);

                shipments = (from a in shipments
                             where ((a.DirectionId == "C" && a.CustomConnectToShipment == false) || a.DirectionId != "C") && !a.IsCancelled
                             select a);

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                shipments = filter.GetFilteredQuery<ShipmentDataView>(nonListQueryOperation, shipments);
                int skippedShipments = queryOperations.PageIndex;

                List<string> SharedFollowedShipmentListIds = (from a in sharedFollowedShipmentRepository.context.SharedFollowedShipments
                                                                    where a.ContactId == filters.ContactId && a.Tenant == tenant
                                                                    orderby a.TrackDate descending
                                                                    select a.ShipmentId).Skip(filters.PageIndex).Take(filters.PageSize).ToList();

                IQueryable<ShipmentDataView> shipmentDataView = (from s in shipments
                                                     where SharedFollowedShipmentListIds.Contains(s.Id)
                                             select s);


                var query2 = BuildShipmentList(currentTenant, shipmentDataView);


                query2 = filter.GetFilteredQuery<ShipmentList>(listQueryOperation, query2);
                query2 = query2.OrderByDescending(d => d.StatusDate);
                listQuery = query2.ToList();

                BuildUnssenFollowedShipment(tenant, listQuery, table, filters.ContactId, contactsUnseenRepository, sharedFollowedShipmentRepository, SharedFollowedShipmentListIds);


            }

    #endregion 


          // TimeSpan maintotaldate = DateTime.Now - maindateStart;
           //AzureLog.SaveLogsInStorage(Id + "      End Post Filtered Shipments      " + " Total Date :   " + maintotaldate.ToString(), "P", DateTime.Now, "", "", 0, filters.ContactId, "", HttpContext.Current.Request.UserHostAddress);

           return listQuery;
        }

        private static IQueryable<ShipmentList> BuildShipmentList(TenantPM currentTenant, IQueryable<ShipmentDataView> shipments)
        {
            var query2 = from f in shipments
                         select new ShipmentList()
                         {
                             CarrierLastStatusDate = f.CarrierLastStatusDate,
                             CarrierLastStatusName = f.CarrierLastStatusName,
                             CarrierLastStatusCode = f.CarrierLastStatusCode,
                             IsOperationalClosed = f.IsOperationalClosed,
                             ShipmentViewId = f.Id,
                             Id = f.Id,
                             DirectionId = f.DirectionId,
                             DirectionName = f.DirectionName,
                             TransportModeName = f.TransportModeName,
                             MasterShipmentNumber = f.MasterShipmentNumber,
                             House = f.House,
                             CreateDateTime = f.CreateDateTime,
                             ShipmentNumber = f.ShipmentNumber,
                             ShipmentType = !string.IsNullOrEmpty(f.ShipmentTypeName) ? f.ShipmentTypeName + " " + f.ShipmentLevelName : f.ShipmentLevelName,
                             TransportModeId = f.TransportModeId,
                             Field1 = f.Field1,
                             Field2 = f.Field2,
                             Field3 = f.Field3,
                             Field4 = f.Field4,
                             Field5 = f.Field5,
                             Field6 = f.Field6,
                             Field7 = f.Field7,
                             Field9 = f.Field9,
                             Field8 = f.Field8,
                             Field11 = f.Field11,
                             Field12 = f.Field12,
                             Field13 = f.Field13,
                             Field14 = f.Field14,
                             Field15 = f.Field15,
                             Field16 = f.Field16,
                             Field17 = f.Field17,
                             Field18 = f.Field18,
                             Field19 = f.Field19,
                             Field20 = f.Field20,
                             Field21 = f.Field21,
                             Field22 = f.Field22,
                             Field23 = f.Field23,
                             Field24 = f.Field24,
                             Field25 = f.Field25,
                             Field26 = f.Field26,
                             Field27 = f.Field27,
                             Field28 = f.Field28,
                             Field29 = f.Field29,
                             Field30 = f.Field30,
                             Field31 = f.Field31,
                             Field32 = f.Field32,
                             Field33 = f.Field33,
                             Field34 = f.Field34,
                             Field35 = f.Field35,
                             Field36 = f.Field36,
                             Field37 = f.Field37,
                             Field38 = f.Field38,
                             Field39 = f.Field39,
                             Field40 = f.Field40,
                             CustomsDeclarationNumber = f.CustomsDeclarationNumber,
                             Field10 = f.Field10,
                             ChargeableWeightInKG = f.ChargeableWeightInKG,
                             ChargeableWeight = f.ChargeableWeight,
                             GrossWeight = f.GrossWeight,
                             Master = f.Master,
                             OpenReceivablesInLocalCurrency = f.OpenReceivablesInLocalCurrency,
                             OpenReceivablesInProfitCurrency = f.OpenReceivablesInProfitCurrency,
                             AccountedReceivablesInLocalCurrency = f.AccountedReceivablesInLocalCurrency,
                             OpenPayablesInLocalCurrency = f.OpenPayablesInLocalCurrency,
                             ShipmentPayableStatusCode = f.ShipmentPayableStatusCode,
                             ShipmentReceivableStatusCode = f.ShipmentReceivableStatusCode,
                             ShipmentPayableStatusName = f.ShipmentPayableStatusName,
                             ShipmentReceivableStatusName = f.ShipmentReceivableStatusName,
                             ProfitInLocalCurrency = f.ProfitInLocalCurrency,
                             ProfitInProfitCurrency = f.ProfitInProfitCurrency,
                             EstimateProfitInLocalCurrency = f.EstimateProfitInLocalCurrency,
                             EstimateProfitInProfitCurrency = f.EstimateProfitInProfitCurrency,
                             BranchId = f.BranchId,
                             DepartmentId = f.DepartmentId,
                             MainCarriageETA = f.MainCarriageETA,
                             MainCarriageATD = f.MainCarriageATD,
                             LocalCurrencyCode = currentTenant.CurrencyCode,
                             ProfitCurrencyCode = currentTenant.ProfitCurrencyCode,
                             NextETA = f.NextETA,
                             NextETD = f.NextETD,
                             NextLegName = f.NextLegName,
                             Routing = f.Routing,
                             MasterShipmentDataId = f.MasterShipmentDataId,
                             BranchName = f.BranchName,
                             GrossWeightInKG = f.GrossWeightInKG,
                             ShipmentLevelCode = f.ShipmentLevelCode,
                             ShipmentLevelName = f.ShipmentLevelName,
                             VolumetricWeight = f.VolumetricWeight,
                             AirlinePrefix = f.AirlinePrefix,
                             AccountedPayablesInLocalCurrency = f.AccountedPayablesInLocalCurrency,
                             AccountedPayablesInProfitCurrency = f.AccountedPayablesInProfitCurrency,
                             AccountedReceivablesInProfitCurrency = f.AccountedReceivablesInProfitCurrency,
                             MainCarriageATA = f.MainCarriageATA,
                             MainCarriageETD = f.MainCarriageETD,
                             IncotermId = f.IncotermId,
                             OpenPayablesInProfitCurrency = f.OpenPayablesInProfitCurrency,
                             IssuingCarrierAgentId = f.IssuingCarrierAgentId,
                             IncotermCode = f.IncotermCode,
                             MainCarriageCarrierId = f.MainCarriageCarrierId,
                             AsAgreedFreight = f.AsAgreedFreight,
                             AsAgreedOtherCharges = f.AsAgreedOtherCharges,
                             AccountNumber = f.AccountNumber,
                             AWBPrint = f.AWBPrint,
                             FHLStatusCode = f.FHLStatusCode,
                             FHLStatusName = f.FHLStatusName,
                             FWBStatusCode = f.FWBStatusCode,
                             FWBStatusName = f.FWBStatusName,
                             LocalCustomsTransmissionsStatusCode = f.LocalCustomsTransmissionsStatusCode,
                             LocalCustomsTransmissionsStatusName = f.LocalCustomsTransmissionsStatusName,
                             LocalCustomsTransmissionsStatusDate = f.LocalCustomsTransmissionsStatusDate,
                             LocalCustomsTransmissionsStatusError = f.LocalCustomsTransmissionsStatusError,
                             FNAReason = f.FNAReason,
                             FinalArrivalDate = f.FinalArrivalDate,
                             Shipper = f.ShipperName,
                             Consignee = f.ConsigneeName,
                             ShipperReference1 = f.ShipperReference1,
                             ShipperReference2 = f.ShipperReference2,
                             ConsigneeReference1 = f.ConsigneeReference1,
                             ConsigneeReference2 = f.ConsigneeReference2,
                             CustomerId = f.CustomerId,
                             CustomerName = f.CustomerName,
                             CustomerReference1 = f.CustomerReference1,
                             CustomerReference2 = f.CustomerReference2,
                             FromPortId = !string.IsNullOrEmpty(f.MainCarriageFromPortId) ? f.MainCarriageFromPortId : f.FromPortId,
                             FromPort = !string.IsNullOrEmpty(f.MainCarriageFromPortCode) ? f.MainCarriageFromPortCode : f.FromPortCode,
                             FromPortName = !string.IsNullOrEmpty(f.MainCarriageFromPortName) ? f.MainCarriageFromPortName : f.FromPortName,
                             FromPortCountry = f.MainCarriageFromPortCountryName,
                             MainCarriageFromPortId = f.MainCarriageFromPortId,
                             MainCarriageFromPortName = f.MainCarriageFromPortName,
                             ToPortId = !string.IsNullOrEmpty(f.MainCarriageToPortId) ? f.MainCarriageToPortId : f.ToPortId,
                             ToPort = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortCode) ? f.MainCarriageFinalDestinationPortCode : f.ToPortCode,
                             ToPortName = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortName) ? f.MainCarriageFinalDestinationPortName : f.ToPortName,
                             ToCountryCode = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationCountryCode) ? f.MainCarriageFinalDestinationCountryCode : f.ToPortCountryCode,
                             ToPortCountry = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationCountryName) ? f.MainCarriageFinalDestinationCountryName : f.ToPortCountryName,
                             FromCountryCode = f.ShipmentLevelCode == "H" && string.IsNullOrEmpty(f.MasterShipmentDataId) ? f.FromPortCountryCode : f.MainCarriageFromPortCountryCode,
                             //ToCountryCode = f.ShipmentLevelCode == "H" && string.IsNullOrEmpty(f.MasterShipmentDataId) ? f.ToPortCountryCode : f.MainCarriageToPortCountryCode,
                             ChargeableWeightUnitCode = f.ChargeableWeightUnitCode,
                             NumberOfPackages = f.NumberOfPackages,
                             NumberOfContainers = f.NumberOfContainers,
                             BookingNumberOfPackages = f.BookingNumberOfPackages,
                             OrderChargeableWeight = f.OrderChargeableWeight,
                             MainCarriageFromCity = f.MainCarriageFromCity,
                             MainCarriageFromCountryCode = f.MainCarriageFromCountryCode,
                             MainCarriageToCity = f.MainCarriageToCity,
                             MainCarriageToCountryCode = f.MainCarriageToCountryCode,
                             MainCarriageCarrierCode = f.MainCarriageCarrierCode,
                             MainCarriageCarrierName = f.MainCarriageCarrierName,
                             MainCarriageCarrierNumber = f.MainCarriageCarrierNumber,
                             AgentName = f.AgentName,
                             AgentReference1 = f.AgentReference1,
                             AgentReference2 = f.AgentReference2,
                             LastUpdateDate = f.LastUpdateDate,
                             LastFSRStatusRequestDate = f.LastFSRStatusRequestDate,
                             CarrierNumber = f.TransportModeId == "A" ? (f.MainCarriageCarrierCode + f.MainCarriageCarrierNumber) : f.TransportModeId == "O" ? (f.MainCarriageVesselName + "/" + f.MainCarriageCarrierNumber) : f.TransportModeId == "I" ? (f.MainCarriageCarrierNumber) : null,
                             LastStatusLogDate = f.LastStatusLogDate,
                             HasException = f.HasException,
                             ExceptionDate = f.ExceptionDate,
                             ExceptionDescription = f.ExceptionDescription,
                             ExceptionResolvedDescription = f.ExceptionResolvedDescription,

                             LastExceptionDescription = f.LastExceptionDescription,
                             MainCarriageFinalDestinationETA = f.MainCarriageFinalDestinationETA,
                             MainCarriageFinalDestinationATA = f.MainCarriageFinalDestinationATA,
                             StatusId = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusId : f.ShipmentStatusId) : (f.ShipmentStatusId),
                             StatusDate = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusDate : f.ShipmentStatusDate) : (f.ShipmentStatusDate),
                             StatusName = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusName : f.ShipmentStatusName) : (f.ShipmentStatusName),
                             StatusLocation = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusLocation : f.ShipmentStatusLocation) : (f.ShipmentStatusLocation),
                             LongMaster = f.TransportModeId == "A" ? (!string.IsNullOrEmpty(f.AirlinePrefix) && !string.IsNullOrEmpty(f.Master) ? f.AirlinePrefix + "-" + f.Master : "") : f.Master,
                             DescriptionOfGoods = f.DescriptionOfGoods,
                             ProjectNumber = f.ProjectNumber,
                             Tenant = f.Tenant,
                         };

            return query2;
        }

        private static void BuildUnssenFollowedShipment(int tenant, List<ShipmentList> listQuery, ObjectTable table, string contactId, ContactsUnseenEntitieRepository contactsUnseenRepository, SharedFollowedShipmentRepository sharedFollowedShipmentRepository, List<string> SharedFollowedShipmentListIds)
        {
            List<string> trackedIds = (from a in listQuery select a.Id).ToList();
            IQueryable<string> contactsUnseenEntitieList = contactsUnseenRepository.GetContactsUnseenEntitiesByContactAndObjectTable(contactId, table.Id, tenant, trackedIds);

            if (SharedFollowedShipmentListIds == null)
            {
                SharedFollowedShipmentListIds = sharedFollowedShipmentRepository.GetSharedFollowedShipmentByContactId(contactId, tenant, trackedIds);
            }
            if (contactsUnseenEntitieList.Count() > 0 || SharedFollowedShipmentListIds.Count() > 0)
            {
                foreach (ShipmentList item in listQuery)
                {
                    item.IsOccurChange = contactsUnseenEntitieList.Contains(item.Id) ? true : false;
                    item.IsShipmentTracking = SharedFollowedShipmentListIds.Contains(item.Id) ? true : false;

                }
            }

        }

        //  ////shipments/getsinglepm/1-1/1
        //  //  client.Get("shipments/getsingledto/1-241/1", (Exception ex, ShipmentDto shipment) =>
        //  //  {

        //  //      if (shipment != null)
        //  //      {
        //  //          //Mapper.Map<OrderPM, OrderViewModel>(order, this);
        //  //          //this.EntityPM = order;

        //  //          // OrderPM clone = Mapper.Map<OrderPM, OrderPM>(order);
        //  //          // clone.Customer = "test";
        //  //          // clone.OrderDetails.Add(new OrderDetailPM() { Id = 10 });

        //  //      }
        //  //  });


        //[OperationContract]
        //[WebGet(UriTemplate = "getsingledto/{id}/{tenant}")]
        //public ShipmentDTO GetSingleShipmentDto(string id, int tenant)
        //{
        //    ShipmentRepository repository = new ShipmentRepository(tenant);

        //    //SecurityUtility.AuthenticationOnTenant(tenant);
        //    //SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

        //    //ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
        //    //ShipmentPM pm = shipmentQuery.GetSinglePM(id, tenant);
        //    //return pm;


        //    Shipment shipment = (from a in repository.context.Shipments
        //                         where a.Id == id && a.Tenant == tenant
        //                         select a).FirstOrDefault();

        //    ShipmentMasterData masterData = (from a in repository.context.ShipmentMasterDatas
        //                                     where a.Id == shipment.MasterShipmentDataId
        //                                     select a).FirstOrDefault();

        //    ShipmentDTO shipmentDto = new ShipmentDTO()
        //    {
        //         Id = shipment.Id,
        //    };

        //    ShipmentReceivableQuery shipmentReceivablesQuery = new ShipmentReceivableQuery(tenant);
        //    List<ShipmentReceivablePM> receivables = shipmentReceivablesQuery.GetShipmentReceivablePMsByShipmentId(shipment.Id, shipment.Tenant).ToList();
        //    List<ShipmentReceivableDTO> receivableDtoList = new List<ShipmentReceivableDTO>();
        //    foreach (ShipmentReceivablePM item in receivables)
        //    {
        //        ShipmentReceivableDTO dto = new ShipmentReceivableDTO()
        //        {
        //            Id = item.Id,
        //            ShipmentId = item.ShipmentId,
        //            ChargesGroupCode = item.ChargesGroupCode,
        //            ChargesTypeCode = item.ChargesTypeCode,
        //            ChargesTypeId = item.ChargesTypeId,
        //            ChargesTypeName = item.ChargesTypeName,
        //            Tenant = item.Tenant,
        //        };

        //        receivableDtoList.Add(dto);
        //    }

        //    shipmentDto.ShipmentReceivables = receivableDtoList;
        //    return shipmentDto;
        //}

        //*******************************************************************************************************************************

        [OperationContract]
        [WebGet(UriTemplate = "getsinglepmformobile/{id}/{tenant}/{contactId}/{cardtype}")]

        public ShipmentMobilePM GetSingleShipmentPMForMobile(string id, int tenant, string contactId, string cardtype)
        {

            DateTime DateBeforeGetSingleShipment = DateTime.Now;
            

            SecurityUtility.AuthenticationOnTenant(tenant);

            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            ShipmentPM pm = shipmentQuery.GetSinglePmForMobile(id, tenant);
            ShipmentMobilePM shipmentMobilePM = null;

            if (pm != null)
            {
                CheckSharedContactAuthenticationForShipment(pm.AgentId, pm.CustomerId, tenant);
    
                shipmentMobilePM = MapPmToShipmentMobilePM(pm, tenant, contactId);

                #region SharedLogisticsSetting
                SharedLogisticsSettingRepository sharedLogisticsSettingRepository = new SharedLogisticsSettingRepository(pm.Tenant);
                SharedLogisticsSetting sharedLogisticsSetting = sharedLogisticsSettingRepository.GetSingle(pm.Tenant.ToString(), pm.Tenant);
                if (sharedLogisticsSetting != null)
                {
                    shipmentMobilePM.IsHideMainCarrier = !sharedLogisticsSetting.IsMainCarrierShared;
                    shipmentMobilePM.IsHidePickDelivCarrier = !sharedLogisticsSetting.IsPickDelivCarriesShared;
                }

                #endregion

                shipmentMobilePM.PartnerLists = GetShipmentPartnersForMobile(pm, tenant, true, sharedLogisticsSetting);

                shipmentMobilePM.EventsLists = GetEntityEvents(pm.Id, "Shipment", cardtype, tenant , pm);


                if (LogitudeSettings.WorkEnvironment == "cloud" && !string.IsNullOrEmpty(shipmentMobilePM.CustomFileId) && string.IsNullOrEmpty(shipmentMobilePM.MobileShipmentReference))
                {
                    ShipmentList shipmentList = shipmentQuery.GetShipmentListBycustomFileIdForMobile(shipmentMobilePM.CustomFileId, shipmentMobilePM.Tenant);
                    shipmentMobilePM.MobileShipmentReference = GetMobileReference(null, shipmentList);
                }


                //if( LogitudeSettings.WorkEnvironment != "cloud")
                // {
                //    DocumentsDataController documentsDataController = new DocumentsDataController();
                //    shipmentMobilePM.DocumentLists = documentsDataController.GetEntityDocuments(pm.Id,cardtype, tenant);

                // }



            }




            int executionTime = (int)((DateTime.Now.Ticks - DateBeforeGetSingleShipment.Ticks) / TimeSpan.TicksPerMillisecond);

            if (HttpContext.Current.Response.Headers["ServerTime"] != null)
            {
                HttpContext.Current.Response.Headers["ServerTime"] = executionTime.ToString();
            }
            else HttpContext.Current.Response.Headers.Add("ServerTime", executionTime.ToString());
            return shipmentMobilePM;
        }



        [OperationContract]
        [WebGet(UriTemplate = "getsinglepm/{id}/{tenant}")]
        public ShipmentPM GetSingleShipmentPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            ShipmentPM pm = shipmentQuery.GetSinglePM(id, tenant);
            CheckSharedContactAuthenticationForShipment(pm.AgentId, pm.CustomerId, tenant);

            SharedLogisticsSettingRepository sharedLogisticsSettingRepository = new SharedLogisticsSettingRepository(tenant);
            SharedLogisticsSetting sharedLogisticsSetting = sharedLogisticsSettingRepository.GetSingle(tenant.ToString(), tenant);
            if(sharedLogisticsSetting != null)
            {
                pm.IsSharedLogisticsMoneyTabEnabled = sharedLogisticsSetting.IsMoneyTabEnabled;
                pm.IsSharedLogisticsMainCarrierVisible = sharedLogisticsSetting.IsMainCarrierShared;
                pm.IsSharedLogisticsPickDelvCarrierVisible = sharedLogisticsSetting.IsPickDelivCarriesShared;
                pm.IsSharedLogisticsAgentVisible = sharedLogisticsSetting.IsAgentShared;
                pm.IsSharedLogisticsShipperVisible = sharedLogisticsSetting.IsShipperShared;
                pm.IsSharedLogisticsConsigneeVisible = sharedLogisticsSetting.IsConsigneeShared;
            }

            return pm;
        }


        [OperationContract]
        [WebGet(UriTemplate = "getsinglepmbykey/{securitykey}/{id}/{tenant}")]
        public ShipmentPM GetSingleShipmentPMByKey(string securitykey, string id, int tenant)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant); 

            ShipmentPM pm = shipmentQuery.GetSinglePMBySecurityKey(securitykey, id, tenant);

            SharedLogisticsSettingRepository sharedLogisticsSettingRepository = new SharedLogisticsSettingRepository(tenant);
            SharedLogisticsSetting sharedLogisticsSetting = sharedLogisticsSettingRepository.GetSingle(tenant.ToString(), tenant);
            if (sharedLogisticsSetting != null)
            {
                pm.IsSharedLogisticsMoneyTabEnabled = sharedLogisticsSetting.IsMoneyTabEnabled;
                pm.IsSharedLogisticsMainCarrierVisible = sharedLogisticsSetting.IsMainCarrierShared;
                pm.IsSharedLogisticsPickDelvCarrierVisible = sharedLogisticsSetting.IsPickDelivCarriesShared;
                pm.IsSharedLogisticsAgentVisible = sharedLogisticsSetting.IsAgentShared;
                pm.IsSharedLogisticsShipperVisible = sharedLogisticsSetting.IsShipperShared;
                pm.IsSharedLogisticsConsigneeVisible = sharedLogisticsSetting.IsConsigneeShared;
            }






            return pm;
        }

        public void Post([FromBody]string value)
        {

        }

        public void Put(int id, [FromBody]string value)
        {

        }

        public void Delete(int id)
        {

        }


        private List<ShipmentPartnerPM> GetShipmentPartnersForMobile(ShipmentPM shipment, int tenant, bool ismobile , SharedLogisticsSetting sharedLogisticsSetting)
        {
            List<ShipmentPartnerPM> result = new List<ShipmentPartnerPM>();

            AddressRepository addressRepository = new AddressRepository(tenant);
            ContactRepository contactRepository = new ContactRepository(tenant);
            CountryRepository countryRepository = new CountryRepository(tenant);
            CardRepository cardRepository = new CardRepository(tenant);


            if (shipment != null)
            {
                bool isShipperShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsShipperShared;
                bool isConsigneeShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsConsigneeShared;

                bool isAgentShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsAgentShared;
                bool isColoaderShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsColoaderShared;
                bool isConsigneeNotImporterShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsConsigneeNotImporterShared;
                bool isFreightForwarderShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsFreightForwarderShared;
                bool isNotify1Shared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsNotify1Shared;
                bool isNotify2Shared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsNotify2Shared;
                bool isShipperNotExporterShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsShipperNotExporterShared;
                bool isCustomsAgentExportShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsCustomsAgentExportShared;
                bool isCustomsAgentImportShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsCustomsAgentImportShared;
                bool isCustomClearancePoinShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsCustomClearancePoinShared;
                bool isConsolidatorShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsConsolidatorShared;
                bool isReleasingAgentShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsReleasingAgentShared;
                bool isIssuingCarrierAgentShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsIssuingCarrierAgentShared;

                if (!string.IsNullOrEmpty(shipment.ShipperId) && isShipperShared)
                {
                    #region Shipper
                    CheckSharedContactAuthenticationForShipment(shipment.AgentId, shipment.CustomerId, tenant);

                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.ShipperId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "visible";
                    item.Reference1 = string.IsNullOrEmpty(shipment.ShipperReference1) ? "" : shipment.ShipperReference1;
                    item.Reference2 = string.IsNullOrEmpty(shipment.ShipperReference2) ? "" : shipment.ShipperReference2;
                    item.PartnerType = "Shipper";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = cardRepository.GetSingleCardByIdAndTenant(shipment.ShipperId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = string.IsNullOrEmpty(card.EnglishName) ? "" : card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.ShipperAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountryByIdAndTenant(address.CountryId, tenant,true);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContactByIdAndTenant(shipment.ShipperContactId, tenant, true);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.ConsigneeId) && isConsigneeShared)
                {
                    #region Consignee
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.ConsigneeId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "visible";
                    item.Reference1 = string.IsNullOrEmpty(shipment.ConsigneeReference1) ? "" : shipment.ConsigneeReference1;
                    item.Reference2 = string.IsNullOrEmpty(shipment.ConsigneeReference2) ? "" : shipment.ConsigneeReference2;
                    item.PartnerType = "Consignee";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = cardRepository.GetSingleCardByIdAndTenant(shipment.ConsigneeId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.ConsigneeAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountryByIdAndTenant(address.CountryId, tenant,true);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContactByIdAndTenant(shipment.ConsigneeContactId, tenant,true);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.AgentId) && isAgentShared)
                {
                    #region Agent
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.AgentId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "visible";
                    item.Reference1 = string.IsNullOrEmpty(shipment.AgentReference1) ? "" : shipment.AgentReference1;
                    item.Reference2 = string.IsNullOrEmpty(shipment.AgentReference2) ? "" : shipment.AgentReference2;
                    item.PartnerType = "Agent";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = cardRepository.GetSingleCardByIdAndTenant(shipment.AgentId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.AgentAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountryByIdAndTenant(address.CountryId, tenant,true);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContactByIdAndTenant(shipment.AgentContactId, tenant,true);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.CustomAgentExportId) && isCustomsAgentExportShared)
                {
                    #region CustomAgentExport
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.CustomAgentExportId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.CustomAgentExportReference) ? "" : shipment.CustomAgentExportReference;
                    item.PartnerType = "Custom Agent Export";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = cardRepository.GetSingleCardByIdAndTenant(shipment.CustomAgentExportId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.CustomAgentExportAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountryByIdAndTenant(address.CountryId, tenant,true);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContactByIdAndTenant(shipment.CustomAgentExportContactId, tenant,true);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.CustomAgentImportId) && isCustomsAgentImportShared)
                {
                    #region CustomAgentImport
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.CustomAgentImportId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.CustomAgentImportReference) ? "" : shipment.CustomAgentImportReference;
                    item.PartnerType = "Custom Agent Import";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = cardRepository.GetSingleCardByIdAndTenant(shipment.CustomAgentImportId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.CustomAgentImportAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountryByIdAndTenant(address.CountryId, tenant,true);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContactByIdAndTenant(shipment.CustomAgentImportContactId, tenant,true);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.ColoaderId) && isColoaderShared)
                {
                    #region Coloader
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.ColoaderId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.ColoaderReference1) ? "" : shipment.ColoaderReference1;
                    item.PartnerType = "Coloader";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = cardRepository.GetSingleCardByIdAndTenant(shipment.ColoaderId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.ColoaderAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountryByIdAndTenant(address.CountryId, tenant,true);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContactByIdAndTenant(shipment.ColoaderContactId, tenant,true);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.ConsigneeNotImporterId) && isConsigneeNotImporterShared)
                {
                    #region ConsigneeNotImporter
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.ConsigneeNotImporterId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.ConsigneeNotImporterReference) ? "" : shipment.ConsigneeNotImporterReference;
                    item.PartnerType = "Consignee Not Importer";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = cardRepository.GetSingleCardByIdAndTenant(shipment.ConsigneeNotImporterId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.ConsigneeNotImporterAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountryByIdAndTenant(address.CountryId, tenant,true);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContactByIdAndTenant(shipment.ConsigneeNotImporterContactId, tenant,true);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.FreightForwarderId) && isFreightForwarderShared)
                {
                    #region FreightForwarder
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.FreightForwarderId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.FreightForwarderReference) ? "" : shipment.FreightForwarderReference;
                    item.PartnerType = "Freight Forwarder";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = cardRepository.GetSingleCardByIdAndTenant(shipment.FreightForwarderId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.FreightForwarderAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountryByIdAndTenant(address.CountryId, tenant,true);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContactByIdAndTenant(shipment.FreightForwarderContactId, tenant,true);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.Notify1Id) && isNotify1Shared)
                {
                    #region Notify1
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.Notify1Id;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.Notify1Reference) ? "" : shipment.Notify1Reference;
                    item.PartnerType = "Notify 1";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.Notify1Id, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.Notify1AddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountryByIdAndTenant(address.CountryId, tenant,true);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContactByIdAndTenant(shipment.Notify1ContactId, tenant,true);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.Notify2Id) && isNotify2Shared)
                {
                    #region Notify2
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.Notify2Id;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.Notify2Reference) ? "" : shipment.Notify2Reference;
                    item.PartnerType = "Notify 2";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.Notify2Id, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.Notify2AddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountryByIdAndTenant(address.CountryId, tenant,true);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContactByIdAndTenant(shipment.Notify2ContactId, tenant,true);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.ShipperNotExporterId) && isShipperNotExporterShared)
                {
                    #region ShipperNotExporter
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.ShipperNotExporterId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.ShipperNotExporterReference) ? "" : shipment.ShipperNotExporterReference;
                    item.PartnerType = "Shipper Not Exporter";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.ShipperNotExporterId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.ShipperNotExporterAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountryByIdAndTenant(address.CountryId, tenant,true);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContactByIdAndTenant(shipment.ShipperNotExporterContactId, tenant,true);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.CustomClearancePointId) && isCustomClearancePoinShared)
                {
                    #region CustomClearancePoint
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.CustomClearancePointId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.CustomClearancePointReference1) ? "" : shipment.CustomClearancePointReference1;
                    item.PartnerType = "Custom Clearance Point";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.CustomClearancePointId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.CustomClearancePointAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountryByIdAndTenant(address.CountryId, tenant,true);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContactByIdAndTenant(shipment.CustomClearancePointContactId, tenant,true);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.ConsolidatorId) && isConsolidatorShared)
                {
                    #region Consolidator
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.ConsolidatorId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.ConsolidatorReference) ? "" : shipment.ConsolidatorReference;
                    item.PartnerType = "Consolidator";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.ConsolidatorId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.ConsolidatorAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountryByIdAndTenant(address.CountryId, tenant,true);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContactByIdAndTenant(shipment.ConsolidatorContactId, tenant,true);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.ReleasingAgentId) && isReleasingAgentShared)
                {
                    #region ReleasingAgent
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.ReleasingAgentId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "visible";
                    item.Reference1 = string.IsNullOrEmpty(shipment.ReleasingAgentReference1) ? "" : shipment.ReleasingAgentReference1;
                    item.Reference2 = string.IsNullOrEmpty(shipment.ReleasingAgentReference2) ? "" : shipment.ReleasingAgentReference2;
                    item.PartnerType = "Releasing Agent";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.ReleasingAgentId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.ReleasingAgentAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountryByIdAndTenant(address.CountryId, tenant,true);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContact(shipment.ReleasingAgentContactId, tenant);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.IssuingCarrierAgentId) && isIssuingCarrierAgentShared && shipment.TransportModeId == "A")
                {
                    #region IssuingCarrierAgent
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.IssuingCarrierAgentId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.IssuingCarrierReference1) ? "" : shipment.IssuingCarrierReference1;
                    item.PartnerType = "Issuing Carrier Agent";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.IssuingCarrierAgentId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.IssuingCarrierAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountryByIdAndTenant(address.CountryId, tenant,true);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    result.Add(item);
                    #endregion
                }
            }

            return result;
        }

        private List<TraceEventPM> GetEntityEvents(string entityId, string objectTableName, string partnerType, int tenant , ShipmentPM shipmentPM = null)
        {
            List<TraceEventPM> result = new List<TraceEventPM>();

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
            if (objectTable != null)
            {
                string objectTableId = objectTable.Id;

                TraceEventRepository traceEventsRepository = new TraceEventRepository(tenant);
                TraceEventQuery traceEventQuery = new TraceEventQuery(traceEventsRepository);

                IQueryable<TraceEventPM> data = traceEventQuery.GetTraceEventPMsByEntityIdForMobile(tenant, entityId, objectTableId, shipmentPM);
               
                if (partnerType == "AG") result = data.Where(d => d.IsAgentView).ToList();
                else if (partnerType == "CS") result = data.Where(d => d.IsCustomerView).ToList();
           
            }

            return result.OrderByDescending(s => s.EventDateTime).ToList();
        }
        


        public List<ShipmentPartnerPM> GetShipmentPartners(string shipmentId, int tenant)
        {
            List<ShipmentPartnerPM> result = new List<ShipmentPartnerPM>();

            AddressRepository addressRepository = new AddressRepository(tenant);
            ContactRepository contactRepository = new ContactRepository(tenant);
            CountryRepository countryRepository = new CountryRepository(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
        
            Shipment shipment = shipmentRepository.GetSingleShipment(shipmentId, tenant);
            
            if (shipment != null)
            {
                SharedLogisticsSettingRepository sharedLogisticsSettingRepository = new SharedLogisticsSettingRepository(shipment.Tenant);
                SharedLogisticsSetting sharedLogisticsSetting = sharedLogisticsSettingRepository.GetSingle(shipment.Tenant.ToString(), shipment.Tenant);
                bool isShipperShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsShipperShared;
                bool isConsigneeShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsConsigneeShared;
                bool isAgentShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsAgentShared;
                bool isColoaderShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsColoaderShared;
                bool isConsigneeNotImporterShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsConsigneeNotImporterShared;
                bool isFreightForwarderShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsFreightForwarderShared;
                bool isNotify1Shared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsNotify1Shared;
                bool isNotify2Shared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsNotify2Shared;
                bool isShipperNotExporterShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsShipperNotExporterShared;
                bool isCustomsAgentExportShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsCustomsAgentExportShared;
                bool isCustomsAgentImportShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsCustomsAgentImportShared;
                bool isCustomClearancePoinShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsCustomClearancePoinShared;
                bool isConsolidatorShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsConsolidatorShared;
                bool isReleasingAgentShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsReleasingAgentShared;
                bool isIssuingCarrierAgentShared = sharedLogisticsSetting == null ? true : sharedLogisticsSetting.IsIssuingCarrierAgentShared;

                if (!string.IsNullOrEmpty(shipment.ShipperId) && isShipperShared)
                {
                    #region Shipper
                    CheckSharedContactAuthenticationForShipment(shipment.AgentId, shipment.CustomerId, tenant);
                    
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.ShipperId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "visible";
                    item.Reference1 = string.IsNullOrEmpty(shipment.ShipperReference1) ? "" : shipment.ShipperReference1;
                    item.Reference2 = string.IsNullOrEmpty(shipment.ShipperReference2) ? "" : shipment.ShipperReference2;
                    item.PartnerType = "Shipper";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.ShipperId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = string.IsNullOrEmpty(card.EnglishName) ? "" : card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.ShipperAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContact(shipment.ShipperContactId, tenant);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.ConsigneeId) && isConsigneeShared)
                {
                    #region Consignee
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.ConsigneeId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "visible";
                    item.Reference1 = string.IsNullOrEmpty(shipment.ConsigneeReference1) ? "" : shipment.ConsigneeReference1;
                    item.Reference2 = string.IsNullOrEmpty(shipment.ConsigneeReference2) ? "" : shipment.ConsigneeReference2;
                    item.PartnerType = "Consignee";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.ConsigneeId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.ConsigneeAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContact(shipment.ConsigneeContactId, tenant);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.AgentId) && isAgentShared)
                {
                    #region Agent
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.AgentId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "visible";
                    item.Reference1 = string.IsNullOrEmpty(shipment.AgentReference1) ? "" : shipment.AgentReference1;
                    item.Reference2 = string.IsNullOrEmpty(shipment.AgentReference2) ? "" : shipment.AgentReference2;
                    item.PartnerType = "Agent";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.AgentId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.AgentAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContact(shipment.AgentContactId, tenant);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.CustomAgentExportId) && isCustomsAgentExportShared)
                {
                    #region CustomAgentExport
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.CustomAgentExportId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.CustomAgentExportReference) ? "" : shipment.CustomAgentExportReference;
                    item.PartnerType = "Custom Agent Export";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.CustomAgentExportId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.CustomAgentExportAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContact(shipment.CustomAgentExportContactId, tenant);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.CustomAgentImportId) && isCustomsAgentImportShared)
                {
                    #region CustomAgentImport
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.CustomAgentImportId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.CustomAgentImportReference) ? "" : shipment.CustomAgentImportReference;
                    item.PartnerType = "Custom Agent Import";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.CustomAgentImportId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.CustomAgentImportAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContact(shipment.CustomAgentImportContactId, tenant);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.ColoaderId) && isColoaderShared)
                {
                    #region Coloader
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.ColoaderId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.ColoaderReference1) ? "" : shipment.ColoaderReference1;
                    item.PartnerType = "Coloader";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.ColoaderId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.ColoaderAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContact(shipment.ColoaderContactId, tenant);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.ConsigneeNotImporterId) && isConsigneeNotImporterShared)
                {
                    #region ConsigneeNotImporter
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.ConsigneeNotImporterId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.ConsigneeNotImporterReference) ? "" : shipment.ConsigneeNotImporterReference;
                    item.PartnerType = "Consignee Not Importer";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.ConsigneeNotImporterId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.ConsigneeNotImporterAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContact(shipment.ConsigneeNotImporterContactId, tenant);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.FreightForwarderId) && isFreightForwarderShared)
                {
                    #region FreightForwarder
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.FreightForwarderId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.FreightForwarderReference) ? "" : shipment.FreightForwarderReference;
                    item.PartnerType = "Freight Forwarder";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.FreightForwarderId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.FreightForwarderAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContact(shipment.FreightForwarderContactId, tenant);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.Notify1Id) && isNotify1Shared)
                {
                    #region Notify1
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.Notify1Id;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.Notify1Reference) ? "" : shipment.Notify1Reference;
                    item.PartnerType = "Notify 1";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.Notify1Id, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.Notify1AddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContact(shipment.Notify1ContactId, tenant);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.Notify2Id) && isNotify2Shared)
                {
                    #region Notify2
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.Notify2Id;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.Notify2Reference) ? "" : shipment.Notify2Reference;
                    item.PartnerType = "Notify 2";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.Notify2Id, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.Notify2AddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContact(shipment.Notify2ContactId, tenant);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.ShipperNotExporterId) && isShipperNotExporterShared)
                {
                    #region ShipperNotExporter
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.ShipperNotExporterId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.ShipperNotExporterReference) ? "" : shipment.ShipperNotExporterReference;
                    item.PartnerType = "Shipper Not Exporter";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.ShipperNotExporterId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.ShipperNotExporterAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContact(shipment.ShipperNotExporterContactId, tenant);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.CustomClearancePointId) && isCustomClearancePoinShared)
                {
                    #region CustomClearancePoint
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.CustomClearancePointId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.CustomClearancePointReference1) ? "" : shipment.CustomClearancePointReference1;
                    item.PartnerType = "Custom Clearance Point";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.CustomClearancePointId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.CustomClearancePointAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContact(shipment.CustomClearancePointContactId, tenant);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.ConsolidatorId) && isConsolidatorShared)
                {
                    #region Consolidator
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.ConsolidatorId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.ConsolidatorReference) ? "" : shipment.ConsolidatorReference;
                    item.PartnerType = "Consolidator";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.ConsolidatorId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.ConsolidatorAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContact(shipment.ConsolidatorContactId, tenant);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.ReleasingAgentId) && isReleasingAgentShared)
                {
                    #region ReleasingAgent
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.ReleasingAgentId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "visible";
                    item.Reference1 = string.IsNullOrEmpty(shipment.ReleasingAgentReference1) ? "" : shipment.ReleasingAgentReference1;
                    item.Reference2 = string.IsNullOrEmpty(shipment.ReleasingAgentReference2) ? "" : shipment.ReleasingAgentReference2;
                    item.PartnerType = "Releasing Agent";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.ReleasingAgentId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.ReleasingAgentAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }

                    Contact contact = contactRepository.GetSingleContact(shipment.ReleasingAgentContactId, tenant);
                    if (contact != null)
                    {
                        item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                        item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                    }

                    result.Add(item);
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.IssuingCarrierAgentId) && isIssuingCarrierAgentShared && shipment.TransportModeId == "A")
                {
                    #region IssuingCarrierAgent
                    ShipmentPartnerPM item = new ShipmentPartnerPM();
                    item.Id = shipment.IssuingCarrierAgentId;
                    item.ReferenceVisibility = "visible";
                    item.Reference2Visibility = "collapse";
                    item.Reference1 = string.IsNullOrEmpty(shipment.IssuingCarrierReference1) ? "" : shipment.IssuingCarrierReference1;
                    item.PartnerType = "Issuing Carrier Agent";
                    item.FlagSRC = "";
                    item.Email = "";
                    item.ContactName = "";

                    Card card = CardRepository.GetSingleCard(shipment.IssuingCarrierAgentId, tenant, true);
                    if (card != null)
                    {
                        item.PartnerName = card.EnglishName;
                    }

                    Address address = addressRepository.GetSingleAddress(shipment.IssuingCarrierAddressId, tenant);
                    if (address != null)
                    {
                        item.Name = address.Name;
                        item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                        item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                        item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                        item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                        item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                        Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                            item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                        }
                    }
                    
                    result.Add(item);
                    #endregion
                }
            }

            return result;
        }

        public List<ShipmentPartnerPM> GetShipmentPartnersByKey(string securitykey, string shipmentId, int tenant)
        {
            List<ShipmentPartnerPM> result = new List<ShipmentPartnerPM>();
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            Shipment shipment = shipmentRepository.GetSingleShipment(shipmentId, tenant);

            if (shipment != null && shipment.SecurityKey == securitykey)
            {
                SharedLogisticsSettingRepository settingRepository = new SharedLogisticsSettingRepository(shipment.Tenant);
                SharedLogisticsSetting setting = settingRepository.GetSingle(shipment.Tenant.ToString(), shipment.Tenant);
                
                SharedLogisticService service = new SharedLogisticService(shipment, setting);
                service.BuildPartners();
                result = service.Partners;                                              
            }

            return result;
        }

        public bool CheckSharedContactAuthenticationForShipment(string agentId, string customerId, int tenant)
        {
            if (tenant != 0)
            {

                bool exists = false;
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {//using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    //{
                    //}
                    ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                    string email = HttpContext.Current.User.Identity.Name;

                    ContactRepository contactrep = new ContactRepository(commonDataContext);
                    Contact contact = contactrep.GetSingleContactByEmail(email, tenant);

                    if (contact != null)
                    {
                        CardContact cardContact = commonDataContext.CardContacts.Where(d => d.ContactId == contact.Id && (d.CardId == customerId || d.CardId == agentId)).FirstOrDefault();
                        if (cardContact != null)
                        {
                            exists = true;

                        }
                    }


                }
                if (!exists)
                {
                    throw new AutenticationException("Sorry! you are not authorized to read data!");
                }
                return exists;
            }
            return true;


        }  

        #region DeparturesArrivals Count


        [OperationContract]
        [WebGet(UriTemplate = "getdeparturesarrivalscount/{tenant}/{directionId}/{transportModeId}")]
        public ShipmentCout GetDeparturesArrivalsShipmentCount(int tenant, string directionId, string PartnerType, string PartnerId)
        {

            ShipmentCout shipmentCout = GetDeparturesArrivalsCount(tenant, directionId, PartnerType, PartnerId, "");

            return shipmentCout;
        }

        private  ShipmentCout GetDeparturesArrivalsCount(int tenant, string directionId, string partnerType, string partnerId, string transportModeId)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
          

            string email = HttpContext.Current.User.Identity.Name;
         
            QueryOperations queryOperations = new QueryOperations();
           

            if (directionId == "I") queryOperations.SetFilter("DirectionId", "I,C", false, "InList", null, false);
            else queryOperations.SetFilter("DirectionId", directionId, false, "Equals", null, true);

            if (!string.IsNullOrEmpty(transportModeId)) queryOperations.SetFilter("TransportModeId", transportModeId, false, "Equals", null, true);
    
            string value = "";
            string name = "";
            if (partnerType == "CS")
            {
                value = "D,H,A"; name = "CustomerId";
            }
            else if (partnerType == "AG")
            {
                value = "D,C"; name = "AgentId";
            }

            queryOperations.SetFilter(name, partnerId, false, "Equals", null, false);
            queryOperations.SetFilter("ShipmentLevelCode", value, false, "InList", null, false);


            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);
            IQueryable<ShipmentList> shipments = GetDeparturesArrivalsShipmentList(tenant, shipmentRepository);

            shipments = customfilters.GetFilteredQuery(queryOperations, shipments);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            shipments = filter.GetFilteredQuery(nonListQueryOperation, shipments);
          //  shipments = filter.GetFilteredQuery<ShipmentDataView>(nonListQueryOperation, shipments);



            if (!string.IsNullOrEmpty(directionId))
            {
                if (directionId == "I")
                {
                    shipments = shipments.Where(d => (d.DirectionId == "I" || d.DirectionId == "C") && (d.ShipmentLevelCode != "H" || (d.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(d.ShipmentMasterDataId))));
                }
                else
                {
                    shipments = shipments.Where(d => d.DirectionId == directionId && (d.ShipmentLevelCode != "H" || (d.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(d.ShipmentMasterDataId))));
                }
             }
            else
            {
                shipments = shipments.Where(d => d.ShipmentLevelCode != "H" || (d.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(d.ShipmentMasterDataId)));
            }

            shipments = (from a in shipments
                         where (a.DirectionId == "C" && a.CustomConnectToShipment == false) || a.DirectionId != "C"
                         select a);

            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);


            List<FlightSummary> result = shipmentQuery.GetShipmentsDashBoardDeparturesArrivalsFroMobile(tenant, transportModeId, directionId, shipments);

            ShipmentCout shipmentCout = new ShipmentCout();

            EntityStatusRepository entityStatusRepository = new EntityStatusRepository(tenant);

            DateTime todayDate = DateTime.Today.Date;
            if (result != null)
            {
                List<FlightSummary> expectedList = result.Where(d => d.ActualDate == null && d.ExpectedDate != null).ToList();
                List<FlightSummary> actualList = result.Where(d => d.ActualDate != null).ToList();
                List<FlightSummary> EXPList = expectedList.Where(d => d.MainCarriageATA == null || (d.DirectionId != "E" && d.DirectionId != "R" && d.DirectionId != "D")).ToList();

                string ArrivalsStatsId = entityStatusRepository.GetEntityStatusArrivals(tenant);
                shipmentCout.LastWeekExp = EXPList.Where(d => d.ExpectedDate.Value.Date >= todayDate.AddDays(-7) &&  d.ComputedStatusId != ArrivalsStatsId && d.ExpectedDate.Value.Date <= todayDate.AddDays(-1) && (d.ActualDate == null || (d.DirectionId != "E" && d.DirectionId != "R" && d.DirectionId != "D"))).Count();
               
                shipmentCout.LastWeekAct = actualList.Where(d => d.ActualDate.Value.Date >= todayDate.AddDays(-7) && d.ActualDate.Value.Date <= todayDate.AddDays(-1)).Count();
                shipmentCout.YesterdayExp = expectedList.Where(d => d.ExpectedDate.Value.Date == todayDate.AddDays(-1)).Count();
                shipmentCout.YesterdayAct = actualList.Where(d => d.ActualDate.Value.Date == todayDate.AddDays(-1)).Count();


                shipmentCout.TodayExp = EXPList.Where(d => d.ExpectedDate.Value.Date == todayDate && (d.ActualDate == null || (d.DirectionId != "E" && d.DirectionId != "R" && d.DirectionId != "D"))).Count();
                shipmentCout.TodayAct = actualList.Where(d => d.ActualDate.Value.Date == todayDate).Count();


                shipmentCout.TomorrowExp = expectedList.Where(d => d.ExpectedDate.Value.Date == todayDate.AddDays(1)).Count();
                shipmentCout.TomorrowAct = actualList.Where(d => d.ActualDate.Value.Date == todayDate.AddDays(1)).Count();

                shipmentCout.NextWeekExp = expectedList.Where(d => d.ExpectedDate.Value.Date >= todayDate.AddDays(2) && d.ExpectedDate.Value.Date <= todayDate.AddDays(7)).Count();
                shipmentCout.NextWeekAct = actualList.Where(d => d.ActualDate.Value.Date >= todayDate.AddDays(2) && d.ActualDate.Value.Date <= todayDate.AddDays(7)).Count();
            }
            return shipmentCout;
        }



        private  IQueryable<ShipmentList> GetDeparturesArrivalsShipmentList(int tenant, ShipmentRepository shipmentRepository)
        {

            IQueryable<ShipmentList> shipments = from entity in shipmentRepository.context.Shipments.Include("ComputedEntityStatus")//.Include("ShipperCard").Include("ConsigneeCard")
                                                 join sm in shipmentRepository.context.ShipmentMasterDatas.Include("MainCarriageCarrierCard")
                                                 on entity.MasterShipmentDataId equals sm.Id into shipmentJoin
                                                 from m in shipmentJoin.DefaultIfEmpty()
                                                 where entity.Tenant == tenant && !entity.IsCancelled
                                                 select new ShipmentList()
                                                 {

                                                     Id = entity.Id,
                                                     DirectionId = entity.DirectionId,
                                                     TransportModeId = entity.TransportModeId,
                                                     MainCarriageATD = m.MainCarriageATD,
                                                     MainCarriageATA = m.MainCarriageATA,
                                                     MainCarriageETA = m.MainCarriageETA,
                                                     MainCarriageETD = m.MainCarriageETD,
                                                     ComputedStatusId = entity.ComputedStatusId,
                                                     MainCarriageCarrierCode = m.MainCarriageCarrierCard.Code,
                                                     MainCarriageCarrierName = m.MainCarriageCarrierCard.EnglishName,
                                                     MainCarriageCarrierId = m.MainCarriageCarrierId,
                                                     MainCarriageCarrierNumber = m.MainCarriageCarrierNumber,
                                                     TruckNumber = m.TruckNumber,
                                                     CustomerId = entity.CustomerId,
                                                     IsCancelled = entity.IsCancelled,
                                                     ShipmentLevelCode = entity.ShipmentLevelCode,
                                                     ShipmentMasterDataId = entity.MasterShipmentDataId,
                                                     CustomConnectToShipment = entity.CustomConnectToShipment,
                                                 };
            return shipments;
        }





        public ShipmentCout PostDeparturesArrivalsShipmentCount(int tenant, string type, DeparturesArivalsActivityFilter filters)
        {
            ShipmentCout shipmentCout = GetDeparturesArrivalsCount(tenant, filters.DirectionId, filters.PartnerType, filters.PartnerId, filters.TransportModeId);

            return shipmentCout;
     
        }




        #endregion

        public class ShipmentCout
        {
            public int LastWeekExp { get; set; }
            public int LastWeekAct { get; set; }

            public int YesterdayExp { get; set; }
            public int YesterdayAct { get; set; }

            public int TodayExp { get; set; }
            public int TodayAct { get; set; }

            public int TomorrowExp { get; set; }
            public int TomorrowAct { get; set; }

            public int NextWeekExp { get; set; }
            public int NextWeekAct { get; set; }

        }

        private ShipmentMobilePM MapPmToShipmentMobilePM(ShipmentPM pm, int tenant, string contactId)
        {
            bool isShipmentTracking = false;
            if (!string.IsNullOrEmpty(contactId))
            {
            SharedFollowedShipmentRepository sharedFollowedShipmentRepository = new SharedFollowedShipmentRepository(tenant);
                SharedFollowedShipment item = sharedFollowedShipmentRepository.GetSingleSharedFollowedShipmentByShipmentIdAndContactId(pm.Id, contactId, tenant);

                if (item != null) isShipmentTracking = true;
                else isShipmentTracking = false;

            }



            ShipmentMobilePM shipmentMobilePM = new ShipmentMobilePM()
            {
                Id = pm.Id,
             //   Declaration No
                OnCarriageCarrierName = pm.OnCarriageCarrierName,
                Transshipment1CarrierName = pm.Transshipment1CarrierName,
                CustomConnectToShipment = pm.CustomConnectToShipment,
                Transshipment2CarrierName = pm.Transshipment2CarrierName,
                Transshipment3CarrierName = pm.Transshipment3CarrierName,
                HasException = pm.HasException,
                ExceptionDate = pm.ExceptionDate,
                ExceptionDescription = pm.ExceptionDescription,
                ForeignPartnerCountryCode = pm.ForeignPartnerCountryCode,
             
                ShipmentLevelCode = pm.ShipmentLevelCode,
                 NumberOfContainers = pm.NumberOfContainers,
                    AgentReference1 = pm.AgentReference1,

                  CustomFileId = pm.CustomFileId,
                  CustomFileNumber = pm.CustomFileNumber,
                CustomsDeclarationNumber = pm.CustomsDeclarationNumber,

                AgentReference2 = pm.AgentReference2,
                ShipperReference1 = pm.ShipperReference1,
                ShipperReference2 = pm.ShipperReference2,



                ConsigneeReference1 = pm.ConsigneeReference1,
                ConsigneeReference2 = pm.ConsigneeReference2,
                MainCarriageVesselName = pm.MainCarriageVesselName,
                PreCarriageVesselName = pm.PreCarriageVesselName,
                OnCarriageVesselName = pm.OnCarriageVesselName,
                Transshipment1VesselName = pm.Transshipment1VesselName,
                Transshipment2VesselName = pm.Transshipment2VesselName,
                Transshipment3VesselName = pm.Transshipment3VesselName,


                #region Fields





                MasterShipmentNumber = pm.MasterShipmentNumber,
                ChargeableWeightInKG = pm.ChargeableWeightInKG,
                GrossWeightInKG = pm.GrossWeightInKG,
                ChargeableWeight = pm.ChargeableWeight,
                GrossWeight = pm.GrossWeight,
                CurrentUserId = pm.CurrentUserId,
                Tenant = pm.Tenant,
  
                ShipmentNumber = pm.ShipmentNumber,
                DirectionId = pm.DirectionId,
                DirectionName = pm.DirectionName,
                TransportModeId = pm.TransportModeId,
                TransportModeName = pm.TransportModeName,
                ShipmentTypeId = pm.ShipmentTypeId,
              
                House = pm.House,
                CreateDateTime = pm.CreateDateTime,
 

                //[Timestamp]

                LongMaster = pm.LongMaster,
                GrossWeightUnitCode = pm.GrossWeightUnitCode,
                ChargeableWeightUnitCode = pm.ChargeableWeightUnitCode,
                DimensionsUnitCode = pm.DimensionsUnitCode,
                VolumetricWeight = pm.VolumetricWeight,
         
                Volume = pm.Volume,
                NumberOfPackages = pm.NumberOfPackages,

                GrossWeightEdited = pm.GrossWeightEdited,
                VolumeUnitCode = pm.VolumeUnitCode,
                IsShipmentTracking = isShipmentTracking,
                StatusDate = pm.ComputedStatusDate, //pm.ComputedStatusDate!=null  && pm.ComputedStatusDate > pm.StatusDate ? pm.ComputedStatusDate: pm.StatusDate,
                StatusName = pm.ComputedStatusName, //pm.ComputedStatusDate != null && pm.ComputedStatusDate > pm.StatusDate ? pm.ComputedStatusName : pm.StatusName,
                ShipmentPickUps = pm.ShipmentPickUps,
                ShipmentDeliveries = pm.ShipmentDeliveries,
                ShipmentPackages = pm.ShipmentPackages,
                ShipmentOrderPackages = pm.ShipmentOrderPackages,
               

                #endregion

                #region Routings

       
                PreCarriageTransportModeId = pm.PreCarriageTransportModeId,
                PreCarriageFromPortId = pm.PreCarriageFromPortId,
                PreCarriageToPortId = pm.PreCarriageToPortId,
                PreCarriageCarrierId = pm.PreCarriageCarrierId,
                PreCarriageCarrierNumber = pm.PreCarriageCarrierNumber,
                PreCarriageCarrierName = pm.PreCarriageCarrierName,
                PreCarriageCarrierCode = pm.PreCarriageCarrierCode,
                PreCarriageFromPortCode = pm.PreCarriageFromPortCode,
                PreCarriageFromPortName = pm.PreCarriageFromPortName,
                PreCarriageFromPortCountryCode = pm.PreCarriageFromPortCountryCode,
                PreCarriageFromPortCountryName = pm.PreCarriageFromPortCountryName,
                PreCarriageToPortCode = pm.PreCarriageToPortCode,
                PreCarriageToPortName = pm.PreCarriageToPortName,
                PreCarriageToPortCountryCode = pm.PreCarriageToPortCountryCode,
                PreCarriageToPortCountryName = pm.PreCarriageToPortCountryName,
                PreCarriageETD = pm.PreCarriageETD,
                PreCarriageATD = pm.PreCarriageATD,
                PreCarriageETA = pm.PreCarriageETA,
                PreCarriageATA = pm.PreCarriageATA,


                OnCarriageTransportModeId = pm.OnCarriageTransportModeId,
                OnCarriageFromPortId = pm.OnCarriageFromPortId,
                OnCarriageToPortId = pm.OnCarriageToPortId,
              
                OnCarriageCarrierNumber = pm.OnCarriageCarrierNumber,
       
                OnCarriageCarrierCode = pm.OnCarriageCarrierCode,
                OnCarriageFromPortCode = pm.OnCarriageFromPortCode,
                OnCarriageFromPortName = pm.OnCarriageFromPortName,
                OnCarriageFromPortCountryCode = pm.OnCarriageFromPortCountryCode,
               
                OnCarriageToPortCode = pm.OnCarriageToPortCode,
                OnCarriageToPortName = pm.OnCarriageToPortName,
                OnCarriageToPortCountryCode = pm.OnCarriageToPortCountryCode,
                OnCarriageToPortCountryName = pm.OnCarriageToPortCountryName,
                OnCarriageETD = pm.OnCarriageETD,
                OnCarriageATD = pm.OnCarriageATD,
                OnCarriageETA = pm.OnCarriageETA,
                OnCarriageATA = pm.OnCarriageATA,

                MainCarriageCarrierName = pm.MainCarriageCarrierName,
                MainCarriageCarrierCode = pm.MainCarriageCarrierCode,
                MainCarriageFromPortCode = pm.MainCarriageFromPortCode,
                MainCarriageFromPortName = pm.MainCarriageFromPortName,
            
                MainCarriageFromPortCountryCode = pm.MainCarriageFromPortCountryCode,
                MainCarriageToPortCode = pm.MainCarriageToPortCode,
                MainCarriageToPortName = pm.MainCarriageToPortName,
                MainCarriageToPortCountryCode = pm.MainCarriageToPortCountryCode,

                MainCarriageCarrierNumber = pm.MainCarriageCarrierNumber,



                MainCarriageATD = pm.MainCarriageATD,
                MainCarriageATA = pm.MainCarriageATA,
                MainCarriageETD = pm.MainCarriageETD,
                MainCarriageETA = pm.MainCarriageETA,


                Transshipment1FromPortId = pm.Transshipment1FromPortId,
                Transshipment1ToPortId = pm.Transshipment1ToPortId,
                Transshipment1ATD = pm.Transshipment1ATD,
                Transshipment1ATA = pm.Transshipment1ATA,
                Transshipment1ETD = pm.Transshipment1ETD,
                Transshipment1ETA = pm.Transshipment1ETA,
                Transshipment1CarrierNumber = pm.Transshipment1CarrierNumber,

                Transshipment1CarrierCode = pm.Transshipment1CarrierCode,
                Transshipment1FromPortCode = pm.Transshipment1FromPortCode,
                Transshipment1FromPortName = pm.Transshipment1FromPortName,
                Transshipment1FromPortCountryCode = pm.Transshipment1FromPortCountryCode,

                Transshipment1ToPortCode = pm.Transshipment1ToPortCode,
                Transshipment1ToPortName = pm.Transshipment1ToPortName,
                Transshipment1ToPortCountryCode = pm.Transshipment1ToPortCountryCode,
    

                Transshipment2FromPortId = pm.Transshipment2FromPortId,
                Transshipment2ToPortId = pm.Transshipment2ToPortId,
                Transshipment2ATD = pm.Transshipment2ATD,
                Transshipment2ATA = pm.Transshipment2ATA,
                Transshipment2ETD = pm.Transshipment2ETD,
                Transshipment2ETA = pm.Transshipment2ETA,

                Transshipment2CarrierNumber = pm.Transshipment2CarrierNumber,
        
                Transshipment2CarrierCode = pm.Transshipment2CarrierCode,
                Transshipment2FromPortCode = pm.Transshipment2FromPortCode,
                Transshipment2FromPortName = pm.Transshipment2FromPortName,
                Transshipment2FromPortCountryCode = pm.Transshipment2FromPortCountryCode,
    
                Transshipment2ToPortCode = pm.Transshipment2ToPortCode,
                Transshipment2ToPortName = pm.Transshipment2ToPortName,
                Transshipment2ToPortCountryCode = pm.Transshipment2ToPortCountryCode,
              

                Transshipment3FromPortId = pm.Transshipment3FromPortId,
                Transshipment3ToPortId = pm.Transshipment3ToPortId,
                Transshipment3ATD = pm.Transshipment3ATD,
                Transshipment3ATA = pm.Transshipment3ATA,
                Transshipment3ETD = pm.Transshipment3ETD,
                Transshipment3ETA = pm.Transshipment3ETA,

                Transshipment3CarrierNumber = pm.Transshipment3CarrierNumber,
   
                Transshipment3CarrierCode = pm.Transshipment3CarrierCode,
                Transshipment3FromPortCode = pm.Transshipment3FromPortCode,
                Transshipment3FromPortName = pm.Transshipment3FromPortName,
                Transshipment3FromPortCountryCode = pm.Transshipment3FromPortCountryCode,
       
                Transshipment3ToPortCode = pm.Transshipment3ToPortCode,
                Transshipment3ToPortName = pm.Transshipment3ToPortName,
                Transshipment3ToPortCountryCode = pm.Transshipment3ToPortCountryCode,
   





 
                FromPortName = pm.FromPortName,

                FromPortCountryName = pm.FromPortCountryName,

             
                ToPortName = pm.ToPortName,
  
                ToPortCountryName = pm.ToPortCountryName,


                #endregion

          

               #region partner


          ShipperName = pm.ShipperName,



          ConsigneeName = pm.ConsigneeName,


                 #endregion
            };


            shipmentMobilePM.MobileShipmentReference = GetMobileReference(shipmentMobilePM,null);


            if (pm.ShipmentLevelCode == "H" && string.IsNullOrEmpty(pm.MasterShipmentDataId))
            {
                shipmentMobilePM.FromCountryCode = pm.FromPortCountry;
                shipmentMobilePM.ToCountryCode = pm.ToPortCountry;

            }
            else
            {
                shipmentMobilePM.FromCountryCode = pm.MainCarriageFromPortCountryCode;
                shipmentMobilePM.ToCountryCode = pm.MainCarriageToPortCountryCode;
            }

            return shipmentMobilePM;
        }

        private string GetMobileReference(ShipmentMobilePM shipmentMobilePM , ShipmentList shipmentlist)
        {
            string _myRef = "";

            string shipmentLevelCode = shipmentMobilePM != null ? shipmentMobilePM.ShipmentLevelCode : shipmentlist.ShipmentLevelCode;
            string directionId = shipmentMobilePM != null ? shipmentMobilePM.DirectionId : shipmentlist.DirectionId;


            string agentReference1 = shipmentMobilePM != null ? shipmentMobilePM.AgentReference1 : shipmentlist.AgentReference1;
            string agentReference2 = shipmentMobilePM != null ? shipmentMobilePM.AgentReference2 : shipmentlist.AgentReference2;

            string shipperReference1 = shipmentMobilePM != null ? shipmentMobilePM.ShipperReference1 : shipmentlist.ShipperReference1;
            string shipperReference2 = shipmentMobilePM != null ? shipmentMobilePM.ShipperReference2 : shipmentlist.ShipperReference2;

            string consigneeReference2 = shipmentMobilePM != null ? shipmentMobilePM.ConsigneeReference2 : shipmentlist.ConsigneeReference2;
            string consigneeReference1 = shipmentMobilePM != null ? shipmentMobilePM.ConsigneeReference1 : shipmentlist.ConsigneeReference1;

            if (shipmentLevelCode == "C")
            {


                _myRef = agentReference1;
                if (!string.IsNullOrEmpty(agentReference2))
                {
                    _myRef = _myRef == "" ? agentReference2 : _myRef + ", " + agentReference2;
                }
            }
            else
            {

                if (directionId == "E" || directionId == "R")
                {
                    _myRef =shipperReference1;
                    if (!string.IsNullOrEmpty(shipperReference2))
                    {
                        _myRef = _myRef == "" ? shipperReference2 : _myRef + ", " + shipperReference2;
                    }
                }

                else
                {
                    _myRef = consigneeReference1;
                    if (!string.IsNullOrEmpty(consigneeReference2))
                    {
                        _myRef = _myRef == "" ? consigneeReference2 : _myRef + ", " + consigneeReference2;
                    }
                }
            }

            if (_myRef == ", ") _myRef = "";

            return _myRef;
        }
    }

    public class SharedLogisticService
    {
        private int tenant;
        private Shipment shipment;
        private ICommonDataContext iCommonContext;
        private AddressRepository addressRepository;
        private ContactRepository contactRepository;
        private CountryRepository countryRepository;
        private SharedLogisticsSetting setting;
        public List<ShipmentPartnerPM> Partners { get; set; }

        private class PartnerArguments
        {
            public string Id { get; set; }
            public string Type { get; set; }
            public string AddressId { get; set; }
            public string ContactId { get; set; }
            public string Reference1 { get; set; }
            public string Reference2 { get; set; }
            public bool IsShared { get; set; }
            public bool HasReference2 { get; set; }
        }

        public SharedLogisticService(Shipment shipment, SharedLogisticsSetting setting)
        {
            this.shipment = shipment;
            this.tenant = shipment.Tenant;
            this.setting = setting;

            iCommonContext = CommonDataContext.GetContext(tenant);
            addressRepository = new AddressRepository(iCommonContext);
            contactRepository = new ContactRepository(iCommonContext);
            countryRepository = new CountryRepository(iCommonContext);
        }

        public void BuildPartners()
        {
            this.Partners = new List<ShipmentPartnerPM>();

            this.AddPartner(new PartnerArguments()
            {
                Type = "Shipper",
                Id = shipment.ShipperId,
                AddressId = shipment.ShipperAddressId,
                ContactId = shipment.ShipperContactId,
                Reference1 = shipment.ShipperReference1,
                Reference2 = shipment.ShipperReference2,
                HasReference2 = true,
                IsShared = setting.IsShipperShared
            });

            this.AddPartner(new PartnerArguments()
            {
                Type = "Consignee",
                Id = shipment.ConsigneeId,
                AddressId = shipment.ConsigneeAddressId,
                ContactId = shipment.ConsigneeContactId,
                Reference1 = shipment.ConsigneeReference1,
                Reference2 = shipment.ConsigneeReference2,
                HasReference2 = true,
                IsShared = setting.IsConsigneeShared
            });

            this.AddPartner(new PartnerArguments()
            {
                Type = "Agent",
                Id = shipment.AgentId,
                AddressId = shipment.AgentAddressId,
                ContactId = shipment.AgentContactId,
                Reference1 = shipment.AgentReference1,
                Reference2 = shipment.AgentReference2,
                HasReference2 = true,
                IsShared = setting.IsAgentShared
            });

            this.AddPartner(new PartnerArguments()
            {
                Type = "Custom Agent Export",
                Id = shipment.CustomAgentExportId,
                AddressId = shipment.CustomAgentExportAddressId,
                ContactId = shipment.CustomAgentExportContactId,
                Reference1 = shipment.CustomAgentExportReference,
                IsShared = setting.IsCustomsAgentExportShared
            });

            this.AddPartner(new PartnerArguments()
            {
                Type = "Custom Agent Import",
                Id = shipment.CustomAgentImportId,
                AddressId = shipment.CustomAgentImportAddressId,
                ContactId = shipment.CustomAgentImportContactId,
                Reference1 = shipment.CustomAgentImportReference,
                IsShared = setting.IsCustomsAgentImportShared
            });

            this.AddPartner(new PartnerArguments()
            {
                Type = "Coloader",
                Id = shipment.ColoaderId,
                AddressId = shipment.ColoaderAddressId,
                ContactId = shipment.ColoaderContactId,
                Reference1 = shipment.ColoaderReference1,
                IsShared = setting.IsColoaderShared
            });

            this.AddPartner(new PartnerArguments()
            {
                Type = "Consignee Not Importer",
                Id = shipment.ConsigneeNotImporterId,
                AddressId = shipment.ConsigneeNotImporterAddressId,
                ContactId = shipment.ConsigneeNotImporterContactId,
                Reference1 = shipment.ConsigneeNotImporterReference,
                IsShared = setting.IsConsigneeNotImporterShared
            });

            this.AddPartner(new PartnerArguments()
            {
                Type = "Freight Forwarder",
                Id = shipment.FreightForwarderId,
                AddressId = shipment.FreightForwarderAddressId,
                ContactId = shipment.FreightForwarderContactId,
                Reference1 = shipment.FreightForwarderReference,
                IsShared = setting.IsFreightForwarderShared
            });

            this.AddPartner(new PartnerArguments()
            {
                Type = "Notify 1",
                Id = shipment.Notify1Id,
                AddressId = shipment.Notify1AddressId,
                ContactId = shipment.Notify1ContactId,
                Reference1 = shipment.Notify1Reference,
                IsShared = setting.IsNotify1Shared
            });

            this.AddPartner(new PartnerArguments()
            {
                Type = "Notify 2",
                Id = shipment.Notify2Id,
                AddressId = shipment.Notify2AddressId,
                ContactId = shipment.Notify2ContactId,
                Reference1 = shipment.Notify2Reference,
                IsShared = setting.IsNotify2Shared
            });

            this.AddPartner(new PartnerArguments()
            {
                Type = "Shipper Not Exporter",
                Id = shipment.ShipperNotExporterId,
                AddressId = shipment.ShipperNotExporterAddressId,
                ContactId = shipment.ShipperNotExporterContactId,
                Reference1 = shipment.ShipperNotExporterReference,
                IsShared = setting.IsShipperNotExporterShared
            });

            this.AddPartner(new PartnerArguments()
            {
                Type = "Custom Clearance Point",
                Id = shipment.CustomClearancePointId,
                AddressId = shipment.CustomClearancePointAddressId,
                ContactId = shipment.CustomClearancePointContactId,
                Reference1 = shipment.CustomClearancePointReference1,
                IsShared = setting.IsCustomClearancePoinShared
            });

            this.AddPartner(new PartnerArguments()
            {
                Type = "Consolidator",
                Id = shipment.ConsolidatorId,
                AddressId = shipment.ConsolidatorAddressId,
                ContactId = shipment.ConsolidatorContactId,
                Reference1 = shipment.ConsolidatorReference,
                IsShared = setting.IsConsolidatorShared
            });

            this.AddPartner(new PartnerArguments()
            {
                Type = "Releasing Agent",
                Id = shipment.ReleasingAgentId,
                AddressId = shipment.ReleasingAgentAddressId,
                ContactId = shipment.ReleasingAgentContactId,
                Reference1 = shipment.ReleasingAgentReference1,
                Reference2 = shipment.ReleasingAgentReference2,
                HasReference2=true,
                IsShared = setting.IsReleasingAgentShared
            });
            if (shipment.TransportModeId == "A")
            {
                this.AddPartner(new PartnerArguments()
                {
                    Type = "Issuing Carrier Agent",
                    Id = shipment.IssuingCarrierAgentId,
                    AddressId = shipment.IssuingCarrierAddressId,
                    Reference1 = shipment.IssuingCarrierReference1,
                    IsShared = setting.IsIssuingCarrierAgentShared
                });
            }
        }

        private void AddPartner(PartnerArguments arguments)
        {
            if (arguments.Id != null && arguments.IsShared)
            {
                ShipmentPartnerPM item = new ShipmentPartnerPM()
                {
                    Id = arguments.Id,
                    PartnerType = arguments.Type,
                    Reference1 = string.IsNullOrEmpty(arguments.Reference1) ? "" : arguments.Reference1,
                    Reference2 = string.IsNullOrEmpty(arguments.Reference2) ? "" : arguments.Reference2,
                };

                if (arguments.HasReference2)
                {
                    item.Reference2Visibility = "visible";
                }

                Card card = CardRepository.GetSingleCard(arguments.Id, tenant, true);
                if (card != null)
                {
                    item.PartnerName = string.IsNullOrEmpty(card.EnglishName) ? "" : card.EnglishName;
                }

                Address address = addressRepository.GetSingleAddress(arguments.AddressId, tenant);
                if (address != null)
                {
                    item.Name = address.Name;
                    item.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
                    item.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
                    item.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
                    item.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
                    item.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);

                    Country country = countryRepository.GetSingleCountryByIdAndTenant(address.CountryId, tenant, true);
                    if (country != null)
                    {
                        item.CountryName = country.EnglishName;
                        item.FlagSRC = "../images/Flags/" + country.Code + ".png";
                    }
                }

                Contact contact = contactRepository.GetSingleContactByIdAndTenant(arguments.ContactId, tenant, true);
                if (contact != null)
                {
                    item.Email = string.IsNullOrEmpty(contact.Email) ? "" : contact.Email;
                    item.ContactName = string.IsNullOrEmpty(contact.EnglishName) ? "" : contact.EnglishName;
                }

                this.Partners.Add(item);
            }
        }
    }
}