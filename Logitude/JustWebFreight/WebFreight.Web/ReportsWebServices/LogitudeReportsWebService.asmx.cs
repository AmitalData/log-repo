using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Services;
using System.Xml.Serialization;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.DataProviders;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityLists;
using Simplog.Data.Helpers;
using Simplog.Data.QuoteModel.Repositories;
using Logitude.BL.QuoteModel.EntityQueries;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Logitude.BL.QuoteModel.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Logitude.BL.CommonDataModel.BusinessUnitFilters;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.QuoteModel;
using Logitude.BL.InvoiceModel.CustomFilters;
using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InvoiceModel;
using Logitude.WarehouseLib.BL.EntityQueryServices;
using Logitude.WarehouseLib.BL.DataContracts;
using Logitude.TimeManagement.Data.Repositories;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data;
using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityLists;
using static Logitude.Accounting.BL.CoreBL.Reports.RevenueExpenseReportParam;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using System.Data;
using Newtonsoft.Json;
using Logitude.Accounting.Data;
using WebFreight.Web.Helpers.DataProviderHelpers;
using System.Text;
using System.Web;
using Logitude.BL.DataContracts;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.Resolvers;
using WebFreight.Web.ReportsWebServices.LogitudeReports.Accounting;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using System.Reflection;
using System.Collections;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.Reports;
using WebFreight.Web.WebServices;
using WebFreight.Web.Services;
using System.Threading.Tasks;
using NLog;

namespace WebFreight.Web.ReportsWebServices
{
    /// <summary>
    /// Summary description for LogitudeReportsWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LogitudeReportsWebService : System.Web.Services.WebService
    {
        #region AirlineStatistics
        [WebMethod]
        public byte[] LoadAirlineStatisticsData(byte[] xmlFilters, bool isClosed, int tenant)
        {
            AirlineStatisticsDataProvider dataprovider = LoadAirlineStatisticsDataProvider(xmlFilters, isClosed, tenant);
            dataprovider.Logo = DataProviders.General.GetLogo(tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(AirlineStatisticsDataProvider), tenant);
        }

        public AirlineStatisticsDataProvider LoadAirlineStatisticsDataProvider(byte[] xmlFilters, bool isClosed, int tenant)
        {
            AirlineStatisticsDataProvider dataProvider = new AirlineStatisticsDataProvider();
            dataProvider.AirlineStatisticsReportList = new List<AirlineStatisticsDataProvider.AirlineStatisticsReport>();

            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant);

            AddressQuery addressQuery = new AddressQuery(tenant);
            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CreateDateTime" && d.Operator == "GreaterThanOrEqual").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CreateDateTime" && d.Operator == "LessThanOrEqual").FirstOrDefault();
            QueryFilterItem filterItem_Direction = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Direction").FirstOrDefault();
            QueryFilterItem filterItem_Closed = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IsOperationalClosed").FirstOrDefault();

            DateTime fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            DateTime toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));

            string direction = null;
            bool includeOperationalClosed = false;

            if (filterItem_Direction != null)
            {
                if (filterItem_Direction.FieldValue != null)
                {
                    direction = filterItem_Direction.FieldValue.ToString();
                }
            }

            if (filterItem_Closed != null)
            {
                if (filterItem_Closed.FieldValue != null)
                {
                    includeOperationalClosed = (bool)filterItem_Closed.FieldValue;
                }
            }

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);
            }

            if (filterItem_ToDate != null)
            {
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
            }

            #endregion

            #region General Data

            dataProvider.Name = "Airline statistics";
            dataProvider.FromPeriod = fromDate;
            dataProvider.ToPeriod = toDate;

            if (!string.IsNullOrEmpty(direction))
            {
                dataProvider.Direction = direction == "E" ? "Export" : direction == "I" ? "Import" : "Domestic";
            }
            else
            {
                dataProvider.Direction = "All";
            }

            if (currentTenant != null)
            {
                dataProvider.TenantName = currentTenant.Company;
                dataProvider.Signature = currentTenant.Signature;
                dataProvider.Logo = DataProviders.General.GetLogo(currentTenant.Id);

                AddressPM address = addressQuery.GetSingleAddressPM(currentTenant.AddressId, currentTenant.Id, false);
                if (address != null)
                {
                    dataProvider.Address1 = address.Address1;
                    dataProvider.Address2 = address.Address2;
                    dataProvider.City = address.City;
                    dataProvider.Country = address.CountryName;
                    dataProvider.TenantFax = address.FaxNumber;
                    dataProvider.TenantPhone = address.PhoneNumber;
                    dataProvider.State = address.StateEnglishName;
                    dataProvider.ZipCode = address.ZipCode;
                }
            }

            #endregion

            #region Base Data Filterd

            shipments = shipments.Where(f => f.ShipmentLevelCode != "H" && f.TransportModeId == "A" && !f.IsCancelled);

            if (!includeOperationalClosed)
            {
                shipments = shipments.Where(d => !d.IsOperationalClosed);
            }

            if (!string.IsNullOrEmpty(direction))
            {
                shipments = shipments.Where(d => d.DirectionId == direction);
            }

            if (fromDate != null)
            {
                shipments = shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
            }

            if (toDate != null)
            {
                shipments = shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
            }

            #endregion

            #region Fill Report Data

            double? totalGrossWeight = shipments.Sum(d => d.GrossWeightInKG);
            int totalCount = shipments.Count();

            dataProvider.TotalGrossWeight = totalGrossWeight;
            dataProvider.TotalShipments = totalCount;
            dataProvider.TotalChargeableWeight = shipments.Sum(d => d.ChargeableWeightInKG);
            dataProvider.TotalVolume = shipments.Sum(d => d.VolumeInCBM);

            dataProvider.AirlineStatisticsReportList = (from a in shipments
                                                        group a by new
                                                        {
                                                            a.MainCarriageCarrierId,
                                                            a.MainCarriageCarrierName,
                                                        } into gr
                                                        orderby gr.Key.MainCarriageCarrierName
                                                        select new AirlineStatisticsDataProvider.AirlineStatisticsReport()
                                                        {
                                                            AirlineName = gr.Key.MainCarriageCarrierName == null ? "(No Carrier Specified)" : gr.Key.MainCarriageCarrierName,
                                                            ChargeableWeight = gr.Sum(d => d.ChargeableWeightInKG),
                                                            GrossWeight = gr.Sum(d => d.GrossWeightInKG),
                                                            Shipments = gr.Count(),
                                                            PercentageFromTotalGrossWeight = totalGrossWeight == 0 ? 0 : (gr.Sum(d => d.GrossWeightInKG) / totalGrossWeight),
                                                            PercentageFromTotalShipment = totalCount == 0 ? 0 : ((double)gr.Count() / (double)totalCount),
                                                            Volume = gr.Sum(d => d.VolumeInCBM),
                                                        }).ToList();

            #endregion

            return dataProvider;
        }
        #endregion

        #region ShippingLineStatistics
        [WebMethod]
        public byte[] LoadShippingLineStatisticsData(byte[] xmlFilters, bool isClosed, int tenant)
        {
            ShippingLineStatisticsDataProvider dataprovider = LoadShippingLineStatisticsDataProvider(xmlFilters, isClosed, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(ShippingLineStatisticsDataProvider), tenant);
        }

        public ShippingLineStatisticsDataProvider LoadShippingLineStatisticsDataProvider(byte[] xmlFilters, bool isClosed, int tenant)
        {
            ShippingLineStatisticsDataProvider dataProvider = new ShippingLineStatisticsDataProvider();
            dataProvider.ShippingLineStatisticsReportList = new List<ShippingLineStatisticsDataProvider.ShippingLineStatisticsReport>();

            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            AddressQuery addressQuery = new AddressQuery(tenant);

            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);
            IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant);

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CreateDateTime" && d.Operator == "GreaterThanOrEqual").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CreateDateTime" && d.Operator == "LessThanOrEqual").FirstOrDefault();
            QueryFilterItem filterItem_Direction = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Direction").FirstOrDefault();
            QueryFilterItem filterItem_Closed = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IsOperationalClosed").FirstOrDefault();

            DateTime fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            DateTime toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));

            string direction = null;
            bool includeOperationalClosed = false;

            if (filterItem_Direction != null)
            {
                if (filterItem_Direction.FieldValue != null)
                {
                    direction = filterItem_Direction.FieldValue.ToString();
                }
            }

            if (filterItem_Closed != null)
            {
                if (filterItem_Closed.FieldValue != null)
                {
                    includeOperationalClosed = (bool)filterItem_Closed.FieldValue;
                }
            }

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);
            }

            if (filterItem_ToDate != null)
            {
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
            }

            #endregion

            #region General Data

            dataProvider.Name = "Shipping Line statistics";
            dataProvider.FromPeriod = fromDate;
            dataProvider.ToPeriod = toDate;

            if (!string.IsNullOrEmpty(direction))
            {
                dataProvider.Direction = direction == "E" ? "Export" : direction == "I" ? "Import" : "Domestic";
            }
            else
            {
                dataProvider.Direction = "All";
            }

            if (currentTenant != null)
            {
                dataProvider.TenantName = currentTenant.Company;
                dataProvider.Signature = currentTenant.Signature;
                dataProvider.Logo = DataProviders.General.GetLogo(currentTenant.Id);

                AddressPM address = addressQuery.GetSingleAddressPM(currentTenant.AddressId, currentTenant.Id, false);
                if (address != null)
                {
                    dataProvider.Address1 = address.Address1;
                    dataProvider.Address2 = address.Address2;
                    dataProvider.City = address.City;
                    dataProvider.Country = address.CountryName;
                    dataProvider.TenantFax = address.FaxNumber;
                    dataProvider.TenantPhone = address.PhoneNumber;
                    dataProvider.State = address.StateEnglishName;
                    dataProvider.ZipCode = address.ZipCode;
                }
            }

            #endregion

            #region Base Data Filterd

            shipments = shipments.Where(f => f.ShipmentLevelCode != "H" && f.TransportModeId == "O" && !f.IsCancelled);

            if (!includeOperationalClosed)
            {
                shipments = shipments.Where(d => !d.IsOperationalClosed);
            }

            if (!string.IsNullOrEmpty(direction))
            {
                shipments = shipments.Where(d => d.DirectionId == direction);
            }

            if (fromDate != null)
            {
                shipments = shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
            }

            if (toDate != null)
            {
                shipments = shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
            }

            #endregion

            #region Fill Report Data


            dataProvider.TotalFCLShipments = shipments.Where(d => d.ShipmentTypeId == "FCLD" || d.ShipmentTypeId == "MYGO").Count();
            dataProvider.TotalLCLShipments = shipments.Where(d => d.ShipmentTypeId == "LCLD").Count();
            dataProvider.TotalLCLWeight = shipments.Where(d => d.ShipmentTypeId == "LCLD").Sum(d => d.GrossWeightInKG);
            dataProvider.TotalShipments = shipments.Count();
            dataProvider.TotalTEU = shipments.Sum(d => d.TEU);

            dataProvider.ShippingLineStatisticsReportList = (from a in shipments
                                                             group a by new
                                                             {
                                                                 a.MainCarriageCarrierId,
                                                                 a.MainCarriageCarrierName,
                                                             } into gr
                                                             orderby gr.Key.MainCarriageCarrierName
                                                             select new ShippingLineStatisticsDataProvider.ShippingLineStatisticsReport()
                                                             {
                                                                 Carrier = gr.Key.MainCarriageCarrierName == null ? "(No Carrier Specified)" : gr.Key.MainCarriageCarrierName,
                                                                 FCLShipments = gr.Where(d => d.ShipmentTypeId == "FCLD" || d.ShipmentTypeId == "MYGO").Count(),
                                                                 LCLShipments = gr.Where(d => d.ShipmentTypeId == "LCLD").Count(),
                                                                 TotalShipments = gr.Count(),
                                                                 LCLWeight = gr.Where(d => d.ShipmentTypeId == "LCLD").Sum(t => t.GrossWeightInKG),
                                                                 TEU = gr.Sum(t => t.TEU),
                                                                 PercentageFromTotalShipment = ((double)gr.Count() / (double)dataProvider.TotalShipments),
                                                                 VolumeInCBM = gr.Where(d => d.ShipmentTypeId == "LCLD").Sum(t => t.VolumeInCBM),
                                                             }).ToList();
            #endregion

            return dataProvider;
        }
        #endregion

        #region ProfitByShipment
        [WebMethod]
        public byte[] LoadProfitByShipmentData(byte[] xmlFilters, string currency, bool closed, int tenant)
        {
            ProfitByShipmentDataProvider dataprovider = LoadProfitByShipmentDataProvider(xmlFilters, currency, closed, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(ProfitByShipmentDataProvider), tenant);
        }

        public ProfitByShipmentDataProvider LoadProfitByShipmentDataProvider(byte[] xmlFilters, string currency, bool closed, int tenant)
        {
            ProfitByShipmentDataProvider dataProvider = new ProfitByShipmentDataProvider();
            dataProvider.ProfitByShipmentReportList = new List<ProfitByShipmentDataProvider.ProfitByShipmentReport>();

            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            UserQuery userQuery = new UserQuery(tenant);

            IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant);

            #region Report Filters
            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_DirectionId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DirectionId").FirstOrDefault();
            QueryFilterItem filterItem_AgentId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "AgentId").FirstOrDefault();
            QueryFilterItem filterItem_CustomerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();
            QueryFilterItem filterItem_SalesmanUserId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "SalesmanUserId").FirstOrDefault();
            QueryFilterItem filterItem_AccountingClosed = queryOperations.QueryFilterItems.Where(d => d.FieldName == "AccountingClosed").FirstOrDefault();
            QueryFilterItem filterItem_IncludeAccountedOnly = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeAccountedOnly").FirstOrDefault();
            QueryFilterItem filterItem_IsByCreateDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IsByCreateDate").FirstOrDefault();
            QueryFilterItem filterItem_DepartmentId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DepartmentId").FirstOrDefault();
            QueryFilterItem filterItem_CarrierId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CarrierId").FirstOrDefault();
            QueryFilterItem filterItem_TransportMode = queryOperations.QueryFilterItems.Where(d => d.FieldName == "TransportMode").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);

            DateTime fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            DateTime toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));
            string directionId = null;
            string agentId = null;
            string customerId = null;
            string salesmanUserId = null;
            bool accountingClosed = false;
            bool includeAccountedOnly = false;
            bool isByCreateDate = false;
            string departmentId = null;
            string carrierId = null;
            string transportMode = null;

            if (filterItem_AccountingClosed != null)
            {
                if (filterItem_AccountingClosed.FieldValue != null)
                {
                    accountingClosed = (bool)filterItem_AccountingClosed.FieldValue;
                    shipments = shipments.Where(d => d.IsAccountingClosed == accountingClosed);
                }
            }

            if (filterItem_IncludeAccountedOnly != null)
            {
                if (filterItem_IncludeAccountedOnly.FieldValue != null)
                {
                    includeAccountedOnly = (bool)filterItem_IncludeAccountedOnly.FieldValue;
                }
            }

            if (filterItem_IsByCreateDate != null)
            {
                if (filterItem_IsByCreateDate.FieldValue != null)
                {
                    isByCreateDate = (bool)filterItem_IsByCreateDate.FieldValue;
                }
            }

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);
            }

            if (filterItem_ToDate != null)
            {
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
            }

            if (filterItem_DirectionId != null)
            {
                if (filterItem_DirectionId.FieldValue != null)
                {
                    directionId = filterItem_DirectionId.FieldValue.ToString();
                }
            }

            if (filterItem_AgentId != null)
            {
                if (filterItem_AgentId.FieldValue != null)
                {
                    agentId = filterItem_AgentId.FieldValue.ToString();
                }
            }

            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    customerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }

            if (filterItem_SalesmanUserId != null)
            {
                if (filterItem_SalesmanUserId.FieldValue != null)
                {
                    salesmanUserId = filterItem_SalesmanUserId.FieldValue.ToString();
                }
            }

            if (filterItem_DepartmentId != null)
            {
                if (filterItem_DepartmentId.FieldValue != null)
                {
                    departmentId = filterItem_DepartmentId.FieldValue.ToString();
                }
            }

            if (filterItem_CarrierId != null)
            {
                if (filterItem_CarrierId.FieldValue != null)
                {
                    carrierId = filterItem_CarrierId.FieldValue.ToString();
                }
            }

            if (filterItem_TransportMode != null)
            {
                if (filterItem_TransportMode.FieldValue != null)
                {
                    transportMode = filterItem_TransportMode.FieldValue.ToString();
                }
            }
            #endregion

            #region Filter data
            shipments = shipments.Where(f => f.IsCancelled == false && (f.ShipmentLevelCode == "H" || f.ShipmentLevelCode == "D"));

            if (fromDate != null)
            {
                if (isByCreateDate)
                {
                    shipments = shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                }

                else
                {
                    shipments = shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OperationalDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                }
            }

            if (toDate != null)
            {
                if (isByCreateDate)
                {
                    shipments = shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                }

                else
                {
                    shipments = shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OperationalDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                }
            }

            if (!string.IsNullOrEmpty(directionId) && directionId != "All")
            {
                shipments = shipments.Where(d => d.DirectionId == directionId);
                dataProvider.Direction = directionId == "E" ? "Export" : directionId == "I" ? "Import" : "Domestic";
            }
            else
            {
                dataProvider.Direction = "All";
            }

            if (!string.IsNullOrEmpty(customerId))
            {
                shipments = shipments.Where(d => d.CustomerId == customerId);

                Card customer = CardRepository.GetSingleCard(customerId, tenant, true);
                dataProvider.Customer = customer.EnglishName;
            }
            else
            {
                dataProvider.Customer = "All";
            }

            if (!string.IsNullOrEmpty(agentId))
            {
                shipments = shipments.Where(d => d.AgentId == agentId);

                Card agent = CardRepository.GetSingleCard(agentId, tenant, true);
                dataProvider.Agent = agent.EnglishName;
            }
            else
            {
                dataProvider.Agent = "All";
            }

            if (!string.IsNullOrEmpty(salesmanUserId))
            {
                shipments = shipments.Where(d => d.SalesmanUserId == salesmanUserId);
            }

            if (closed)
            {
                shipments = shipments.Where(d => d.IsOperationalClosed);
            }

            if (!string.IsNullOrEmpty(departmentId))
            {
                shipments = shipments.Where(d => d.DepartmentId == departmentId);
            }

            if (!string.IsNullOrEmpty(carrierId))
            {
                shipments = shipments.Where(d => d.MainCarriageCarrierId == carrierId);
            }

            if (!string.IsNullOrEmpty(transportMode))
            {
                shipments = shipments.Where(d => d.TransportModeId == transportMode);
            }
            #endregion

            #region General Data
            dataProvider.Name = @"P/L by Shipments";

            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);
            if (currentTenant != null)
            {
                dataProvider.TenantName = currentTenant.Company;
                dataProvider.Signature = currentTenant.Signature;
                dataProvider.Logo = DataProviders.General.GetLogo(currentTenant.Id);
                dataProvider.FromPeriod = fromDate;
                dataProvider.ToPeriod = toDate;

                AddressQuery addressQuery = new AddressQuery(tenant);
                AddressPM address = addressQuery.GetSingleAddressPM(currentTenant.AddressId, currentTenant.Id, false);
                if (address != null)
                {
                    dataProvider.Address1 = address.Address1;
                    dataProvider.Address2 = address.Address2;
                    dataProvider.City = address.City;
                    dataProvider.Country = address.CountryName;
                    dataProvider.TenantFax = address.FaxNumber;
                    dataProvider.TenantPhone = address.PhoneNumber;
                    dataProvider.State = address.StateEnglishName;
                    dataProvider.ZipCode = address.ZipCode;
                }
            }
            #endregion

            #region Fill Report Data
            string[] currencyarray = currency.Split(',');
            dataProvider.Currency = currencyarray[0];

            if (shipments.Count() > 0)
            {
                dataProvider.TotalChargeableWeightMT = shipments.Sum(d => (d.ChargeableWeightInKG / 1000.0));
                dataProvider.TotalWeight = shipments.Sum(d => d.GrossWeightInKG);
                dataProvider.TotalTEU = shipments.Sum(d => d.TEU);

                foreach (ShipmentDataView a in shipments)
                {
                    //Arrival: This should give the Main Carriage ATA if available and the ETA if there is no ATA.
                    //Departure: This should give the Main Carriage ATD if available and the ETD if there is no ATD

                    ProfitByShipmentDataProvider.ProfitByShipmentReport profitrecord = new ProfitByShipmentDataProvider.ProfitByShipmentReport();
                    profitrecord.ArrivalDepartureDate = a.DirectionId == "E" ? a.MainCarriageATD : a.MainCarriageATA;
                    profitrecord.OperationalDate = a.OperationalDate;
                    profitrecord.Routing = a.Routing;
                    profitrecord.Carrier = a.MainCarriageCarrierName;
                    profitrecord.Customer = a.CustomerName;
                    profitrecord.ShipmentType = a.TransportModeName + " " + a.DirectionName;
                    profitrecord.AccountManagerName = a.AccountManagerUserName;
                    profitrecord.OperationalStatus = a.IsOperationalClosed ? "Close" : "Open";
                    profitrecord.AccountingStatus = a.IsAccountingClosed ? "Close" : "Open";
                    profitrecord.ShipmentCreateDate = a.CreateDateTime;
                    profitrecord.ChargeableWeight = a.ChargeableWeight;
                    profitrecord.ShipmentNo = a.ShipmentNumber;
                    profitrecord.NumberOfContainers = MethodHelper.IsLCLEntity(a.TransportModeId, a.ShipmentTypeId) ? a.NumberOfPackages : a.NumberOfContainers;
                    profitrecord.TEU = a.TEU;
                    profitrecord.Agent = a.AgentName;
                    profitrecord.Weight = a.GrossWeightInKG;
                    profitrecord.ChargeableWeightMT = a.ChargeableWeightInKG / 1000.0;
                    profitrecord.ShipmentMasterNumber = a.MasterShipmentNumber;
                    profitrecord.Origin = a.MainCarriageFromPortName;
                    profitrecord.Destination = a.ToPortName;
                    profitrecord.Notes = a.Notes;
                    profitrecord.RealShipmentType = a.ShipmentTypeName;
                    profitrecord.Master = a.LongMaster;
                    profitrecord.FromPortCode = a.MainCarriageFromPortCode;
                    profitrecord.FromPortName = a.MainCarriageFromPortName;
                    profitrecord.FinalDestinationPortCode = a.MainCarriageFinalDestinationPortCode;
                    profitrecord.FinalDestinationPortName = a.MainCarriageFinalDestinationPortName;
                    profitrecord.ConsigneeReference1 = a.ConsigneeReference1;
                    profitrecord.ConsigneeReference2 = a.ConsigneeReference2;

                    CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
                    customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, a, profitrecord);

                    if (!string.IsNullOrEmpty(a.DepartmentId))
                    {
                        DepartmentRepository departmentRepository = new DepartmentRepository(tenant);
                        Department department = departmentRepository.GetSingleDepartment(a.DepartmentId, tenant);
                        if (department != null)
                        {
                            profitrecord.Department = department.EnglishName;
                        }
                    }

                    if (!string.IsNullOrEmpty(a.IncotermId))
                    {
                        IncotermRepository incotermRepository = new IncotermRepository(tenant);
                        Incoterm incoterm = incotermRepository.GetSingleIncoterm(a.IncotermId, tenant);
                        if (incoterm != null)
                        {
                            profitrecord.Incoterm = incoterm.Name;
                        }
                    }

                    if (a.MainCarriageATA != null)
                    {
                        profitrecord.Arrival = String.Format("{0:dd MMM yyyy}", a.MainCarriageATA);
                    }

                    else if (a.MainCarriageETA != null)
                    {
                        profitrecord.Arrival = String.Format("{0:dd MMM yyyy}", a.MainCarriageETA) + " (expected)";
                    }

                    if (a.MainCarriageATD != null)
                    {
                        profitrecord.Departure = String.Format("{0:dd MMM yyyy}", a.MainCarriageATD);
                    }

                    else if (a.MainCarriageETD != null)
                    {
                        profitrecord.Departure = String.Format("{0:dd MMM yyyy}", a.MainCarriageETD) + " (expected)";
                    }

                    if (a.CustomerId == a.ShipperId)
                    {
                        profitrecord.ShipperConsignee = a.Consignee;
                    }

                    else if (a.CustomerId == a.ConsigneeId)
                    {
                        profitrecord.ShipperConsignee = a.Shipper;
                    }

                    else if (a.CustomerId == a.AgentId)
                    {
                        profitrecord.ShipperConsignee = a.Shipper;
                    }

                    else if (a.DirectionId == "E")
                    {
                        profitrecord.ShipperConsignee = a.Consignee;
                    }

                    else if (a.DirectionId == "I")
                    {
                        profitrecord.ShipperConsignee = a.Shipper;
                    }

                    if (!string.IsNullOrEmpty(a.SalesmanUserId))
                    {
                        UserPM salesman = userQuery.GetSinglePM(a.SalesmanUserId, tenant);
                        if (salesman != null)
                        {
                            profitrecord.Salesman = salesman.EnglishName;
                        }
                    }

                    if (currencyarray[1] == "profit")
                    {
                        if (includeAccountedOnly)
                        {
                            profitrecord.Payables = a.AccountedPayablesInProfitCurrency;
                            profitrecord.Margin = a.ProfitInProfitCurrency / profitrecord.Payables;

                            if (profitrecord.Margin != null && (double.IsInfinity(profitrecord.Margin.Value) || double.IsNaN(profitrecord.Margin.Value)))
                            {
                                profitrecord.Margin = null;
                            }

                            profitrecord.Receivables = a.AccountedReceivablesInProfitCurrency;
                            profitrecord.Profit = profitrecord.Receivables - profitrecord.Payables;
                        }

                        else
                        {
                            profitrecord.Payables = a.OpenPayablesInProfitCurrency + a.AccountedPayablesInProfitCurrency;
                            profitrecord.Margin = a.ProfitInProfitCurrency / profitrecord.Payables;

                            if (profitrecord.Margin != null && (double.IsInfinity(profitrecord.Margin.Value) || double.IsNaN(profitrecord.Margin.Value)))
                            {
                                profitrecord.Margin = null;
                            }

                            profitrecord.Receivables = (a.OpenReceivablesInProfitCurrency + a.AccountedReceivablesInProfitCurrency);
                            profitrecord.Profit = profitrecord.Receivables - profitrecord.Payables;
                        }
                    }

                    else if (currencyarray[1] == "local")
                    {
                        if (includeAccountedOnly)
                        {
                            profitrecord.Payables = a.AccountedPayablesInLocalCurrency;
                            profitrecord.Margin = (a.ProfitInLocalCurrency / profitrecord.Payables);

                            if (profitrecord.Margin != null && (double.IsInfinity(profitrecord.Margin.Value) || double.IsNaN(profitrecord.Margin.Value)))
                            {
                                profitrecord.Margin = null;
                            }

                            profitrecord.Receivables = a.AccountedReceivablesInLocalCurrency;
                            profitrecord.Profit = profitrecord.Receivables - profitrecord.Payables;
                        }

                        else
                        {
                            profitrecord.Payables = a.OpenPayablesInLocalCurrency + a.AccountedPayablesInLocalCurrency;
                            profitrecord.Margin = (a.ProfitInLocalCurrency / profitrecord.Payables);

                            if (profitrecord.Margin != null && (double.IsInfinity(profitrecord.Margin.Value) || double.IsNaN(profitrecord.Margin.Value)))
                            {
                                profitrecord.Margin = null;
                            }

                            profitrecord.Receivables = a.OpenReceivablesInLocalCurrency + a.AccountedReceivablesInLocalCurrency;
                            profitrecord.Profit = profitrecord.Receivables - profitrecord.Payables;
                        }
                    }

                    dataProvider.ProfitByShipmentReportList.Add(profitrecord);
                }

                dataProvider.TotalPayables = dataProvider.ProfitByShipmentReportList.Sum(d => (d.Payables));
                dataProvider.TotalReceivables = dataProvider.ProfitByShipmentReportList.Sum(d => (d.Receivables));
                dataProvider.TotalProfit = dataProvider.ProfitByShipmentReportList.Sum(d => d.Profit);
            }
            #endregion

            return dataProvider;
        }
        #endregion

        #region StatementAging
        [WebMethod]
        public byte[] LoadStatementAgingData(byte[] xmlFilters, int tenant)
        {
            StatementDataProvider dataprovider = LoadStatementAgingDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(StatementDataProvider), tenant);
        }

        public StatementDataProvider LoadStatementAgingDataProvider(byte[] xmlFilters, int tenant)
        {
            StatementDataProvider dataProvider = new StatementDataProvider();
            ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(tenant);
            ARPaymentRepository aRPaymentRepository = new ARPaymentRepository(tenant);
            ARPaymentQuery arPaymentQuery = new ARPaymentQuery(aRPaymentRepository);
            ARInvoiceQuery arInvoiceQuery = new ARInvoiceQuery(aRInvoiceRepository);
            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARInvoice> iQueryable = aRInvoiceRepository.GetUnpaidARInvoices(tenant);
            IQueryable<ARPayment> iQueryablePayment = aRPaymentRepository.GetOpenedARPayments(tenant);

            InvoiceCustomFilter customFilters = new InvoiceCustomFilter(tenant);
            iQueryable = customFilters.GetFilteredQuery(queryOperations, iQueryable);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARInvoice>(nonListQueryOperation, iQueryable);
            iQueryablePayment = filter.GetFilteredQuery<ARPayment>(nonListQueryOperation, iQueryablePayment);

            IQueryable<ARInvoiceList> invoicequery = arInvoiceQuery.GetIQueryableEntityList(iQueryable);
            IQueryable<ARPaymentList> paymentquery = arPaymentQuery.GetIQueryableEntityList(iQueryablePayment);


            QueryFilterItem customerItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "BillToId" && d.Operator == "Equals").FirstOrDefault();
            string id = "";
            Card customer = null;
            if (customerItem != null)
            {
                id = customerItem.FieldValue.ToString();
                customer = CardRepository.GetSingleCard(id, tenant, true);
                dataProvider.CustomerName = customer.EnglishName;
            }
            else
            {
                dataProvider.CustomerName = "All";
            }


            AddressRepository addressRepository = new AddressRepository(tenant);
            Address cardAddress = addressRepository.GetMainAddressByCardId(id, tenant);

            if (cardAddress != null)
            {

                dataProvider.Phone = cardAddress.PhoneNumber;
                dataProvider.Fax = cardAddress.FaxNumber;
                dataProvider.CustomerName = cardAddress.Name;
                dataProvider.Address = DataProviders.General.GetAddress(cardAddress);

            }

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            dataProvider.StatementAgingSummaryRecordList = new List<StatmentAging>();

            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);
            AddressQuery addressQuery = new AddressQuery(tenant);
            AddressPM address = addressQuery.GetSingleAddressPM(currentTenant.AddressId, currentTenant.Id, false);
            if (address != null)
            {
                dataProvider.Address1 = address.Address1;
                dataProvider.Address2 = address.Address2;
                dataProvider.City = address.City;
                dataProvider.Country = address.CountryName;
                dataProvider.TenantFax = address.FaxNumber;
                dataProvider.TenantPhone = address.PhoneNumber;
                dataProvider.State = address.StateEnglishName;
                dataProvider.ZipCode = address.ZipCode;
            }

            dataProvider.TenantName = currentTenant.Company;
            dataProvider.Signature = currentTenant.Signature;
            dataProvider.Logo = DataProviders.General.GetLogo(currentTenant.Id);

            foreach (ARInvoiceList a in invoicequery)
            {
                IQueryable<ARInvoiceList> currentDueInvoices = invoicequery.Where(d => (d.DueDate) >= todayDate);
                IQueryable<ARInvoiceList> Due1_30Invoices = invoicequery.Where(d => (d.DueDate.Value - todayDate).TotalDays < 30);
                IQueryable<ARInvoiceList> Due31_60Invoices = invoicequery.Where(d => (((d.DueDate.Value - todayDate).TotalDays) > 30) && (((d.DueDate.Value - todayDate).TotalDays) < 60));
                IQueryable<ARInvoiceList> Due61_90Invoices = invoicequery.Where(d => (((d.DueDate.Value - todayDate).TotalDays) > 60) && (((d.DueDate.Value - todayDate).TotalDays) < 90));
                IQueryable<ARInvoiceList> Due90Invoices = invoicequery.Where(d => (((d.DueDate.Value - todayDate).TotalDays) > 90));

                IQueryable<ARInvoiceList> Due31_45Invoices = invoicequery.Where(d => (((d.DueDate.Value - todayDate).TotalDays) > 30) && (((d.DueDate.Value - todayDate).TotalDays) <= 45));
                IQueryable<ARInvoiceList> Due46_60Invoices = invoicequery.Where(d => (((d.DueDate.Value - todayDate).TotalDays) > 45) && (((d.DueDate.Value - todayDate).TotalDays) <= 60));
                IQueryable<ARInvoiceList> Due61_75Invoices = invoicequery.Where(d => (((d.DueDate.Value - todayDate).TotalDays) > 60) && (((d.DueDate.Value - todayDate).TotalDays) <= 75));
                IQueryable<ARInvoiceList> Due76_90Invoices = invoicequery.Where(d => (((d.DueDate.Value - todayDate).TotalDays) > 75) && (((d.DueDate.Value - todayDate).TotalDays) <= 90));

                dataProvider.CurrentDue = (currentDueInvoices.Sum(d => d.AmountDue));
                dataProvider.DaysPastDue1_30 = Due1_30Invoices.Sum(d => d.AmountDue);
                dataProvider.DaysPastDue31_60 = Due31_60Invoices.Sum(d => d.AmountDue);
                dataProvider.DaysPastDue61_90 = Due61_90Invoices.Sum(d => d.AmountDue);
                dataProvider.Over90DaysPastDue = Due90Invoices.Sum(d => d.AmountDue);
                dataProvider.DaysPastDue31_45 = Due31_45Invoices.Sum(d => d.AmountDue);
                dataProvider.DaysPastDue46_60 = Due46_60Invoices.Sum(d => d.AmountDue);
                dataProvider.DaysPastDue61_75 = Due61_75Invoices.Sum(d => d.AmountDue);
                dataProvider.DaysPastDue76_90 = Due76_90Invoices.Sum(d => d.AmountDue);

                StatmentAging statementAgingRecord = new StatmentAging();
                statementAgingRecord.Currency = a.InvoiceCurrencyCode;
                statementAgingRecord.currentDue = dataProvider.CurrentDue;
                statementAgingRecord.Due1_30 = dataProvider.DaysPastDue1_30;
                statementAgingRecord.Due31_60 = dataProvider.DaysPastDue31_60;
                statementAgingRecord.Due61_90 = dataProvider.DaysPastDue61_90;
                statementAgingRecord.Due90 = dataProvider.Over90DaysPastDue;
                statementAgingRecord.Due31_45 = dataProvider.DaysPastDue31_45;
                statementAgingRecord.Due46_60 = dataProvider.DaysPastDue46_60;
                statementAgingRecord.Due61_75 = dataProvider.DaysPastDue61_75;
                statementAgingRecord.Due76_90 = dataProvider.DaysPastDue76_90;

                dataProvider.StatementAgingSummaryRecordList.Add(statementAgingRecord);
            }

            foreach (ARPaymentList a in paymentquery)
            {
                IQueryable<ARPaymentList> currentDuePayments = paymentquery.Where(d => (d.RegisterDate) >= todayDate);
                IQueryable<ARPaymentList> Due1_30Payments = paymentquery.Where(d => (d.RegisterDate.Value - todayDate).TotalDays < 30);
                IQueryable<ARPaymentList> Due31_60Payments = paymentquery.Where(d => (((d.RegisterDate.Value - todayDate).TotalDays) > 30) && (((d.RegisterDate.Value - todayDate).TotalDays) < 60));
                IQueryable<ARPaymentList> Due61_90Payments = paymentquery.Where(d => (((d.RegisterDate.Value - todayDate).TotalDays) > 60) && (((d.RegisterDate.Value - todayDate).TotalDays) < 90));
                IQueryable<ARPaymentList> Due90Payments = paymentquery.Where(d => (((d.RegisterDate.Value - todayDate).TotalDays) > 90));

                IQueryable<ARPaymentList> Due31_45Payments = paymentquery.Where(d => (((d.RegisterDate.Value - todayDate).TotalDays) > 30) && (((d.RegisterDate.Value - todayDate).TotalDays) <= 45));
                IQueryable<ARPaymentList> Due46_60Payments = paymentquery.Where(d => (((d.RegisterDate.Value - todayDate).TotalDays) > 45) && (((d.RegisterDate.Value - todayDate).TotalDays) <= 60));
                IQueryable<ARPaymentList> Due61_75Payments = paymentquery.Where(d => (((d.RegisterDate.Value - todayDate).TotalDays) > 60) && (((d.RegisterDate.Value - todayDate).TotalDays) <= 75));
                IQueryable<ARPaymentList> Due76_90Payments = paymentquery.Where(d => (((d.RegisterDate.Value - todayDate).TotalDays) > 75) && (((d.RegisterDate.Value - todayDate).TotalDays) <= 90));

                dataProvider.CurrentDue = (currentDuePayments.Sum(d => d.OpenAmount));
                dataProvider.DaysPastDue1_30 = Due1_30Payments.Sum(d => d.OpenAmount);
                dataProvider.DaysPastDue31_60 = Due31_60Payments.Sum(d => d.OpenAmount);
                dataProvider.DaysPastDue61_90 = Due61_90Payments.Sum(d => d.OpenAmount);
                dataProvider.Over90DaysPastDue = Due90Payments.Sum(d => d.OpenAmount);
                dataProvider.DaysPastDue31_45 = Due31_45Payments.Sum(d => d.OpenAmount);
                dataProvider.DaysPastDue46_60 = Due46_60Payments.Sum(d => d.OpenAmount);
                dataProvider.DaysPastDue61_75 = Due61_75Payments.Sum(d => d.OpenAmount);
                dataProvider.DaysPastDue76_90 = Due76_90Payments.Sum(d => d.OpenAmount);

                StatmentAging statementAgingRecord = new StatmentAging();
                statementAgingRecord.Currency = a.PaymentCurrencyCode;
                statementAgingRecord.currentDue = dataProvider.CurrentDue;
                statementAgingRecord.Due1_30 = dataProvider.DaysPastDue1_30;
                statementAgingRecord.Due31_60 = dataProvider.DaysPastDue31_60;
                statementAgingRecord.Due61_90 = dataProvider.DaysPastDue61_90;
                statementAgingRecord.Due90 = dataProvider.Over90DaysPastDue;
                statementAgingRecord.Due31_45 = dataProvider.DaysPastDue31_45;
                statementAgingRecord.Due46_60 = dataProvider.DaysPastDue46_60;
                statementAgingRecord.Due61_75 = dataProvider.DaysPastDue61_75;
                statementAgingRecord.Due76_90 = dataProvider.DaysPastDue76_90;

                dataProvider.StatementAgingSummaryRecordList.Add(statementAgingRecord);
            }

            dataProvider.Name = "Statement Aging Summary";

            return dataProvider;
        }
        #endregion

        #region Unpaid Invoices
        [WebMethod]
        public byte[] LoadInvoiceByPartnerData(byte[] xmlFilters, string currency, string dateType, int tenant)
        {
            InvoicesByPartnerDataProvider dataprovider = LoadInvoicesByPartnerDataProvider(xmlFilters, currency, dateType, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(InvoicesByPartnerDataProvider), tenant);
        }

        public InvoicesByPartnerDataProvider LoadInvoicesByPartnerDataProvider(byte[] xmlFilters, string currency, string dateType, int tenant)
        {
            InvoicesByPartnerDataProvider dataProvider = new InvoicesByPartnerDataProvider();
            ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(tenant);
            ARInvoiceQuery arInvoiceQuery = new ARInvoiceQuery(aRInvoiceRepository);
            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            IQueryable<ARInvoice> iQueryable = aRInvoiceRepository.GetUnpaidARInvoices(tenant);
            iQueryable = iQueryable.Where(d => !d.IsConstituentInvoice);

            InvoiceCustomFilter customFilters = new InvoiceCustomFilter(tenant);
            iQueryable = customFilters.GetFilteredQuery(queryOperations, iQueryable);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);
            AddressQuery addressQuery = new AddressQuery(tenant);
            AddressPM address = addressQuery.GetSingleAddressPM(currentTenant.AddressId, currentTenant.Id, false);
            AddressRepository addressRep = new AddressRepository(currentTenant.Id);
            Address tenantAddress = addressRep.GetSingleAddress(currentTenant.AddressId, currentTenant.Id);
            dataProvider.GeneralAddress = DataProviders.General.GetAddress(tenantAddress);
            if (address != null)
            {
                dataProvider.Address1 = address.Address1;
                dataProvider.Address2 = address.Address2;
                dataProvider.City = address.City;
                dataProvider.Country = address.CountryName;
                dataProvider.TenantFax = address.FaxNumber;
                dataProvider.TenantPhone = address.PhoneNumber;
                dataProvider.State = address.StateEnglishName;
                dataProvider.ZipCode = address.ZipCode;
            }

            dataProvider.TenantName = currentTenant.Company;
            dataProvider.Signature = currentTenant.Signature;
            dataProvider.Logo = DataProviders.General.GetLogo(currentTenant.Id);

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime? fromDate = null;
            DateTime? toDate = null;

            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();

            if (filterItem_FromDate != null)
            {
                if (filterItem_FromDate.FieldValue != null)
                {
                    fromDate = (DateTime)filterItem_FromDate.FieldValue;
                }
            }

            if (filterItem_ToDate != null)
            {
                if (filterItem_ToDate.FieldValue != null)
                {
                    toDate = (DateTime)filterItem_ToDate.FieldValue;
                }
            }

            if (fromDate != null)
            {
                dataProvider.FromPeriod = fromDate.Value;

                if (dateType == "CreateDate")
                {
                    iQueryable = iQueryable.Where(d => d.CreateDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                }

                else
                {
                    iQueryable = iQueryable.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                }
            }

            if (toDate != null)
            {
                dataProvider.ToPeriod = toDate.Value;

                if (dateType == "CreateDate")
                {
                    iQueryable = iQueryable.Where(d => d.CreateDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                }

                else
                {
                    iQueryable = iQueryable.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                }
            }

            string partnerId = null;
            QueryFilterItem partnerFilterItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "PartnerId" && d.Operator == "Equals").FirstOrDefault();
            if (partnerFilterItem != null && partnerFilterItem.FieldValue != null)
            {
                partnerId = partnerFilterItem.FieldValue.ToString();
            }

            if (!string.IsNullOrEmpty(partnerId))
            {
                iQueryable = iQueryable.Where(d => d.PartnerId == partnerId);
            }

            QueryFilterItem customerItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "BillToId" && d.Operator == "Equals").FirstOrDefault();
            if (customerItem != null)
            {
                string customerId = customerItem.FieldValue.ToString();

                iQueryable = iQueryable.Where(d => d.BillToId == customerId);

                Card customer = CardRepository.GetSingleCard(customerId, tenant, true);
                dataProvider.CustomerName = customer?.EnglishName;
                dataProvider.CustomerCode = customer?.Code;

                Address myFilterdCustomerAddress = addressRep.GetMainAddressByCardId(customerId, tenant);
                if (myFilterdCustomerAddress != null)
                {
                    dataProvider.CustomerPhone = myFilterdCustomerAddress.PhoneNumber;
                }
            }
            else
            {
                dataProvider.CustomerName = "All";
            }


            IQueryable<ARInvoiceList> invoicequery = arInvoiceQuery.GetIQueryableEntityList(iQueryable);

            dataProvider.InvoicesByPartnerList = new List<InvoicesByPartnerDataProvider.InvoicesByPartner>();

            string[] currencyarray = currency.Split(',');
            dataProvider.Currency = currencyarray[0];
            List<string> shipmentIds = (from a in invoicequery select a.MainEntityId).ToList();

            List<ShipmentPM> shipments = shipmentQuery.GetShipmentsForUnpaidInvoicesReport(shipmentIds);
            foreach (ARInvoiceList a in invoicequery)
            {
                ShipmentPM shipment = (from s in shipments
                                       where s.Id == a.MainEntityId
                                       select s).FirstOrDefault();

                InvoicesByPartnerDataProvider.InvoicesByPartner invoicesRecored = new InvoicesByPartnerDataProvider.InvoicesByPartner();
                invoicesRecored.InvoiceDate = a.InvoiceDate;
                invoicesRecored.InvoiceType = a.ARInvoiceTypeName;
                invoicesRecored.DueDate = a.DueDate;
                invoicesRecored.OurReference = a.StatusCode == "DR" ? a.DraftNumber : a.InvoiceNumber;
                invoicesRecored.MasterNumber = a.MasterNumber;
                invoicesRecored.HouseNumber = a.HouseNumber;
                invoicesRecored.YourRefrence = a.CustomerRef;
                invoicesRecored.BillToName = a.BillToName;
                invoicesRecored.InvoicePrintNotes = a.PrintNotes;

                if (shipment != null)
                {
                    invoicesRecored.ShipmentNumber = shipment.ShipmentNumber;
                    invoicesRecored.Routing = shipment.Routing;

                    switch (shipment.DirectionId)
                    {
                        case "E":
                            {
                                if (shipment.ShipmentLevelCode == "H")
                                {
                                    invoicesRecored.Description = "Export to " + shipment.ToPort;
                                }
                                else
                                {
                                    invoicesRecored.Description = "Export to " + shipment.MainCarriageFinalDestinationPortCode;
                                }

                                break;
                            }
                        case "I": { invoicesRecored.Description = "Import from " + shipment.MainCarriageFromPortCode; break; }
                        case "D": { invoicesRecored.Description = "Ship to " + shipment.ToPartnerCity; break; }
                    }
                }

                if (currencyarray[1] == "profit")
                {
                    invoicesRecored.Amount = a.AmountDueInProfitCurrency;
                }
                else if (currencyarray[1] == "local")
                {
                    invoicesRecored.Amount = a.AmountDueInLocalCurrency;
                }

                invoicesRecored.AmountInProfitCurrency = a.AmountDueInProfitCurrency;
                dataProvider.InvoicesByPartnerList.Add(invoicesRecored);
            }

            dataProvider.Name = @"Invoices By Partner";
            return dataProvider;
        }
        #endregion

        #region Quotes
        [WebMethod]
        public byte[] LoadQuotesData(byte[] xmlFilters, string Type, int tenant)
        {
            QuotesDataProvider dataprovider = LoadQuotesDataProvider(xmlFilters, Type, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(QuotesDataProvider), tenant);
        }

        public QuotesDataProvider LoadQuotesDataProvider(byte[] xmlFilters, string Type, int tenant)
        {
            QuotesDataProvider dataProvider = new QuotesDataProvider();
            dataProvider.QuotesList = new List<QuotesDataProvider.Quotes>();

            QuoteRepository quoteRepository = new QuoteRepository(tenant);
            QuoteQuery quoteQuery = new QuoteQuery(quoteRepository);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Quote> iQueryable = quoteRepository.GetQuotes(tenant);

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_OpenDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "OpenDate" && d.Operator == "GreaterThanOrEqual").FirstOrDefault();
            QueryFilterItem filterItem_ExpirationDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ExpirationDate" && d.Operator == "LessThanOrEqual").FirstOrDefault();
            QueryFilterItem filterItem_CustomerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();
            QueryFilterItem filterItem_SalesmanUserId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "SalesmanUserId").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);

            DateTime openDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            DateTime expirationDate;
            string customerId = null;
            string salesmanUserId = null;

            if (filterItem_OpenDate != null)
            {
                DateTime.TryParse(filterItem_OpenDate.FieldValue.ToString(), out openDate);
            }

            if (filterItem_ExpirationDate != null)
            {
                DateTime.TryParse(filterItem_ExpirationDate.FieldValue.ToString(), out expirationDate);

                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ExpirationDate) <= System.Data.Entity.DbFunctions.TruncateTime(expirationDate));
                dataProvider.To = expirationDate.Month.ToString() + @"/" + expirationDate.Year.ToString();
            }

            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    customerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }

            if (filterItem_SalesmanUserId != null)
            {
                if (filterItem_SalesmanUserId.FieldValue != null)
                {
                    salesmanUserId = filterItem_SalesmanUserId.FieldValue.ToString();
                }
            }

            #endregion

            #region Base Data Filtered

            if (openDate != null)
            {
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OpenDate) >= System.Data.Entity.DbFunctions.TruncateTime(openDate));
                dataProvider.From = openDate.Month.ToString() + @"/" + openDate.Year.ToString();
            }

            //if (expirationDate != null)
            //{
            //    iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ExpirationDate) <= System.Data.Entity.DbFunctions.TruncateTime(expirationDate));
            //    dataProvider.To = expirationDate.Month.ToString() + @"/" + expirationDate.Year.ToString();
            //}

            if (!string.IsNullOrEmpty(customerId))
            {
                iQueryable = iQueryable.Where(d => d.CustomerId == customerId);

                Card customer = CardRepository.GetSingleCard(customerId, tenant, true);
                if (customer != null)
                {
                    dataProvider.CustomerName = customer.EnglishName;
                }
            }

            else
            {
                dataProvider.CustomerName = "All";
            }

            if (!string.IsNullOrEmpty(salesmanUserId))
            {
                UserRepository repository = new UserRepository(tenant);
                iQueryable = iQueryable.Where(d => d.SalesmanUserId == salesmanUserId);

                User salesman = repository.GetSingleUser(salesmanUserId, tenant);
                if (salesman != null)
                {
                    dataProvider.Salesman = salesman.Contact.EnglishName;
                }
            }

            else
            {
                dataProvider.Salesman = "All";
            }

            #endregion

            #region General Data

            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);
            AddressQuery addressQuery = new AddressQuery(tenant);
            AddressPM address = addressQuery.GetSingleAddressPM(currentTenant.AddressId, currentTenant.Id, false);
            if (address != null)
            {
                dataProvider.Address1 = address.Address1;
                dataProvider.Address2 = address.Address2;
                dataProvider.City = address.City;
                dataProvider.Country = address.CountryName;
                dataProvider.TenantFax = address.FaxNumber;
                dataProvider.TenantPhone = address.PhoneNumber;
                dataProvider.State = address.StateEnglishName;
                dataProvider.ZipCode = address.ZipCode;
            }

            dataProvider.TenantName = currentTenant.Company;
            dataProvider.Signature = currentTenant.Signature;
            dataProvider.Logo = DataProviders.General.GetLogo(currentTenant.Id);

            #endregion

            QuoteCustomFilter customFilters = new QuoteCustomFilter(tenant);
            iQueryable = customFilters.GetFilteredQuery(queryOperations, iQueryable);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            iQueryable = filter.GetFilteredQuery<Quote>(nonListQueryOperation, iQueryable);

            IQueryable<QuoteList> quotequery = quoteQuery.GetIQueryableEntityList(iQueryable);

            dataProvider.NumberOfQuotesApproved = quotequery.Where(d => d.StageName == "Accepted" || d.StageName == "Used").Count();
            dataProvider.NumberOfQuotesSent = quotequery.Where(d => d.StageName == "Sent" || d.StageName == "No Answer").Count();
            dataProvider.NumberOfQuotesNotApproved = quotequery.Where(d => d.StageName == "Declined" || d.StageName == "Sent" || d.StageName == "Viewed").Count();
            dataProvider.NumberOfQuotesApprovedWithoutShipment = quotequery.Where(d => d.StageName == "Accepted").Count();

            foreach (QuoteList a in quotequery)
            {
                QuotesDataProvider.Quotes quotesRecored = new QuotesDataProvider.Quotes();
                quotesRecored.ExpiredDate = a.ExpirationDate;
                quotesRecored.From = a.FromPort;
                quotesRecored.To = a.ToPort;
                quotesRecored.Status = a.StageName;
                quotesRecored.QuoteNumber = a.QuoteNumber;
                quotesRecored.Type = a.QuoteTypeName;
                quotesRecored.OpenDate = a.OpenDate;
                quotesRecored.DirectionTransportMode = a.DirectionName + " / " + a.TransportModeName;
                quotesRecored.DeclineReason = a.QuoteClosingReasonName;
                quotesRecored.EstimateProfit = a.EstimateProfit;

                if (a.QuoteTypeCode == "A")
                {
                    if (a.ShipmentTypeId == "LCL" || a.ShipmentTypeId == "LTL" || a.TransportModeId == "A")
                    {
                        if (a.ChargeableWeight != null && a.NumberOfPackages == null)
                        {
                            quotesRecored.Details = "Weight: " + a.ChargeableWeight.ToString() + " KG ";
                        }
                        else if (a.ChargeableWeight == null && a.NumberOfPackages != null)
                        {
                            quotesRecored.Details = "Packages: " + a.NumberOfPackages.ToString() + " Pcs";
                        }
                        else if (a.ChargeableWeight != null && a.NumberOfPackages != null)
                        {
                            quotesRecored.Details = "Weight: " + a.ChargeableWeight.ToString() + " KG " + "/ " + "Packages: " + a.NumberOfPackages.ToString() + " Pcs";
                        }
                    }
                    else if ((a.ShipmentTypeId == "FCL" || a.ShipmentTypeId == "FTL") && a.NumberOfContainers != null)
                    {
                        quotesRecored.Details = "Total Containers: " + a.NumberOfContainers.ToString();
                    }
                }

                dataProvider.QuotesList.Add(quotesRecored);
            }

            dataProvider.Name = @"Quotes";
            return dataProvider;
        }
        #endregion

        #region AR Invoice Include Vat

        [WebMethod]
        public byte[] LoadInvoicesData(byte[] xmlFilters, int tenant)
        {
            InvoiceDataProvider dataprovider = LoadInvoicesDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(InvoiceDataProvider), tenant);
        }

        public InvoiceDataProvider LoadInvoicesDataProvider(byte[] xmlFilters, int tenant)
        {
            InvoiceDataProvider dataProvider = new InvoiceDataProvider();
            dataProvider.InvoicesReportList = new List<InvoiceDataProvider.InvoicesReport>();
            dataProvider.InvoicesReportList_NotSorted = new List<InvoiceDataProvider.InvoicesReport>();
            dataProvider.InvoiceTotalsList = new List<InvoiceDataProvider.InvoiceTotals>();

            ARInvoiceTotalVATRepository aRInvoiceToatalVatRepository = new ARInvoiceTotalVATRepository(tenant);
            ARInvoiceQuery arInvoiceQuery = new ARInvoiceQuery(tenant);
            AddressQuery addressQuery = new AddressQuery(tenant);
            VatTypeRepository vatTypeRepository = new VatTypeRepository(tenant);
            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            ARInvoiceLineRepository aRInvoiceLineRepository = new ARInvoiceLineRepository(tenant);
            ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            IQueryable<ARInvoiceList> iQueryable = arInvoiceQuery.GetInvoiceListByTenant(tenant);
            List<VatType> tenantVatTypes = vatTypeRepository.GetVatTypes(tenant).ToList();
            IQueryable<ARInvoiceLine> tenantARInvoiceLines = aRInvoiceLineRepository.GetInvoiceLinesByTenant(tenant);

            List<Shipment> shipments = this.GetShipmentsByARInvoicesMainEntityId(iQueryable, tenant);

            #region Report Filters
            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_InvoiceDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "InvoiceDate").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_BranchId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "BranchId").FirstOrDefault();
            QueryFilterItem filterItem_LocalCurrency = queryOperations.QueryFilterItems.Where(d => d.FieldName == "LocalCurrency").FirstOrDefault();
            QueryFilterItem filterItem_IncludeVoidInvoices = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeVoidInvoices").FirstOrDefault();
            QueryFilterItem filterItem_IncludeDraftInvoices = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeDraftInvoices").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime date1 = todayDate.AddDays(-(todayDate.Day - 1)).AddMonths(-1);
            DateTime date2 = date1.AddMonths(2);

            bool invoiceDate = true;
            if (filterItem_InvoiceDate != null)
            {
                if (filterItem_InvoiceDate.FieldValue != null)
                {
                    invoiceDate = (bool)filterItem_InvoiceDate.FieldValue;
                }
            }

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out date1);

                if (filterItem_ToDate != null)
                {
                    DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out date2);
                    //date2 = date2.AddMonths(1);
                }

                else
                {
                    date2 = date1.AddMonths(1);
                }
            }

            string branchId = null;
            if (filterItem_BranchId != null)
            {
                if (filterItem_BranchId.FieldValue != null)
                {
                    branchId = filterItem_BranchId.FieldValue.ToString();
                }
            }

            bool localCurrency = true;
            if (filterItem_LocalCurrency != null)
            {
                if (filterItem_LocalCurrency.FieldValue != null)
                {
                    localCurrency = (bool)filterItem_LocalCurrency.FieldValue;
                }
            }

            bool includeVoidInvoices = true;
            if (filterItem_IncludeVoidInvoices != null)
            {
                if (filterItem_IncludeVoidInvoices.FieldValue != null)
                {
                    includeVoidInvoices = (bool)filterItem_IncludeVoidInvoices.FieldValue;
                }
            }

            bool includeDraftInvoices = true;
            if (filterItem_IncludeDraftInvoices != null)
            {
                if (filterItem_IncludeDraftInvoices.FieldValue != null)
                {
                    includeDraftInvoices = (bool)filterItem_IncludeDraftInvoices.FieldValue;
                }
            }
            #endregion

            #region Filter Data
            iQueryable = iQueryable.Where(d => !d.IsConstituentInvoice);

            if (!includeDraftInvoices)
            {
                iQueryable = iQueryable.Where(d => d.StatusCode != "DR");
            }

            if (!includeVoidInvoices)
            {
                iQueryable = iQueryable.Where(d => d.StatusCode != "VD");
            }

            if (!string.IsNullOrEmpty(branchId))
            {
                iQueryable = iQueryable.Where(d => d.BranchId == branchId);
            }

            if (invoiceDate)
            {
                iQueryable = iQueryable.Where(d => d.InvoiceDate >= date1 && d.InvoiceDate <= date2);
            }

            else
            {
                iQueryable = iQueryable.Where(d => d.CreateDate >= date1 && d.CreateDate <= date2);
            }
            #endregion

            #region Fill General Data
            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);
            AddressPM address = addressQuery.GetSingleAddressPM(currentTenant.AddressId, currentTenant.Id, false);

            if (address != null)
            {
                dataProvider.Address1 = address.Address1;
                dataProvider.Address2 = address.Address2;
                dataProvider.City = address.City;
                dataProvider.Country = address.CountryName;
                dataProvider.TenantFax = address.FaxNumber;
                dataProvider.TenantPhone = address.PhoneNumber;
                dataProvider.State = address.StateEnglishName;
                dataProvider.ZipCode = address.ZipCode;
            }

            dataProvider.TenantName = currentTenant.Company;
            dataProvider.Signature = currentTenant.Signature;
            dataProvider.Logo = DataProviders.General.GetLogo(currentTenant.Id);
            dataProvider.FromPeriod = date1;
            dataProvider.ToPeriod = date2;
            #endregion

            #region Fill Report
            double? totalVat = 0;
            double? totalGrands = 0;
            double? subTotals = 0;
            List<ARInvoiceTotalVAT> totalVats = aRInvoiceToatalVatRepository.GetInvoiceTotalVATsByTenant(tenant).ToList();
            int counter = 1;

            foreach (ARInvoiceList arInvoice in iQueryable)
            {
                InvoiceDataProvider.InvoicesReport invoicesRecored = new InvoiceDataProvider.InvoicesReport();
                List<ARInvoiceTotalVAT> myTotalVats = totalVats.Where(d => d.ARInvoiceId == arInvoice.Id).ToList();
                List<VATClass> myVATS = new List<VATClass>();
                List<ARInvoiceLine> myInvoiceLines = tenantARInvoiceLines.Where(l => (l.ARInvoiceId == arInvoice.Id)).ToList();
                customFieldResolver.SetDataProviderCustomFieldsValues("ARInvoice", tenant, arInvoice, invoicesRecored);

                Shipment shipment = shipments.Where(d => d.Id == arInvoice.MainEntityId).FirstOrDefault();
                customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, shipment, invoicesRecored);

                //ARInvoicePM invoicePM = invoiceQuery.GetSinglePM(currentInvoice.Id, currentInvoice.Tenant);
                foreach (ARInvoiceTotalVAT vat in myTotalVats)
                {
                    VATClass item = new VATClass()
                    {
                        Index = counter,
                        Percentage = vat.VatPercent,
                        VATName = tenantVatTypes.Where(d => d.Id == vat.VatTypeId).FirstOrDefault().EnglishName,
                        VATCode = tenantVatTypes.Where(d => d.Id == vat.VatTypeId).FirstOrDefault().Code,
                        InvoiceAmount = vat.InvoiceCurrencyVATAmount,
                        LocalAmount = vat.LocalVATAmount,
                    };

                    myVATS.Add(item);

                    counter++;
                }

                foreach (VATClass item in myVATS)
                {
                    //1
                    if (string.IsNullOrEmpty(dataProvider.VAT1Code))
                    {
                        dataProvider.VAT1Code = item.VATCode;
                        dataProvider.VAT1Header = item.Percentage + "% " + item.VATName;
                        invoicesRecored.VAT1Amount = localCurrency ? item.LocalAmount : item.InvoiceAmount;
                        continue;
                    }

                    else
                    {
                        if (dataProvider.VAT1Code == item.VATCode)
                        {
                            dataProvider.VAT1Code = item.VATCode;
                            dataProvider.VAT1Header = item.Percentage + "% " + item.VATName;
                            invoicesRecored.VAT1Amount = localCurrency ? item.LocalAmount : item.InvoiceAmount;
                            continue;
                        }
                    }

                    //2
                    if (string.IsNullOrEmpty(dataProvider.VAT2Code))
                    {
                        dataProvider.VAT2Code = item.VATCode;
                        dataProvider.VAT2Header = item.Percentage + "% " + item.VATName;
                        invoicesRecored.VAT2Amount = localCurrency ? item.LocalAmount : item.InvoiceAmount;
                        continue;
                    }

                    else
                    {
                        if (dataProvider.VAT2Code == item.VATCode)
                        {
                            dataProvider.VAT2Code = item.VATCode;
                            dataProvider.VAT2Header = item.Percentage + "% " + item.VATName;
                            invoicesRecored.VAT2Amount = localCurrency ? item.LocalAmount : item.InvoiceAmount;
                            continue;
                        }
                    }

                    //3
                    if (string.IsNullOrEmpty(dataProvider.VAT3Code))
                    {
                        dataProvider.VAT3Code = item.VATCode;
                        dataProvider.VAT3Header = item.Percentage + "% " + item.VATName;
                        invoicesRecored.VAT3Amount = localCurrency ? item.LocalAmount : item.InvoiceAmount;
                        continue;
                    }

                    else
                    {
                        if (dataProvider.VAT3Code == item.VATCode)
                        {
                            dataProvider.VAT3Code = item.VATCode;
                            dataProvider.VAT3Header = item.Percentage + "% " + item.VATName;
                            invoicesRecored.VAT3Amount = localCurrency ? item.LocalAmount : item.InvoiceAmount;
                            continue;
                        }
                    }

                    //4
                    if (string.IsNullOrEmpty(dataProvider.VAT4Code))
                    {
                        dataProvider.VAT4Code = item.VATCode;
                        dataProvider.VAT4Header = item.Percentage + "% " + item.VATName;
                        invoicesRecored.VAT4Amount = localCurrency ? item.LocalAmount : item.InvoiceAmount;
                        continue;
                    }

                    else
                    {
                        if (dataProvider.VAT4Code == item.VATCode)
                        {
                            dataProvider.VAT4Code = item.VATCode;
                            dataProvider.VAT4Header = item.Percentage + "% " + item.VATName;
                            invoicesRecored.VAT4Amount = localCurrency ? item.LocalAmount : item.InvoiceAmount;
                            continue;
                        }
                    }
                }

                if (shipment != null)
                {
                    ShipmentMasterData shipmentMasterData = shipmentRepository.GetSingleShipmentMasterData(shipment.MasterShipmentDataId, tenant);
                    invoicesRecored.ShipmentMainCarriageETA = shipmentMasterData?.MainCarriageETA;
                }

                invoicesRecored.InvoiceType = arInvoice.ARInvoiceTypeName;
                invoicesRecored.InvoiceDate = arInvoice.InvoiceDate.Value;
                invoicesRecored.InvoiceNumber = arInvoice.InvoiceNumber;
                invoicesRecored.TotalAmountForTaxReport = arInvoice.TotalAmountForTaxReport;
                invoicesRecored.BillTo = arInvoice.BillToName;
                invoicesRecored.PartnerName = arInvoice.PartnerName;
                invoicesRecored.OurRefNumber = arInvoice.MainEntityReference;
                invoicesRecored.InvoiceStatus = arInvoice.StatusName;
                invoicesRecored.Currency = arInvoice.InvoiceCurrencyCode;
                invoicesRecored.CreateDate = arInvoice.CreateDate;
                invoicesRecored.DueDate = arInvoice.DueDate;
                invoicesRecored.Salesman = arInvoice.SalesmanUserName;
                invoicesRecored.SubTotalInLocalCurrency = arInvoice.SubTotalInLocalCurrency;
                invoicesRecored.VATInLocalCurrency = myTotalVats.Sum(d => d.LocalVATAmount);
                invoicesRecored.GrandTotalInLocalCurrency = invoicesRecored.SubTotalInLocalCurrency + invoicesRecored.VATInLocalCurrency;
                invoicesRecored.ExpenseChargesInLocalCurrency = myInvoiceLines.Where(l => l.IsExpense).Sum(s => s.LocalCurrencyAmount);
                invoicesRecored.BillToCode = arInvoice.BillToCode;
                invoicesRecored.AmountDueInInvoiceCurrency = arInvoice.AmountDue;
                invoicesRecored.AmountDueInLocalCurrency = arInvoice.AmountDueInLocalCurrency;
                invoicesRecored.BillToVatNumber = arInvoice.VatNumber;
                invoicesRecored.PaidDate = arInvoice.PaidDate;
                BranchRepository branchRepository = new BranchRepository(tenant);
                Branch branch = branchRepository.GetSingleBranch(arInvoice.BranchId, tenant);
                if (branch != null)
                {
                    invoicesRecored.BranchCode = branch.Code;
                }

                Card billTo = CardRepository.GetSingleCard(arInvoice.BillToId, tenant, false);

                if (string.IsNullOrEmpty(invoicesRecored.BillToVatNumber))
                {
                    if (billTo != null)
                    {
                        invoicesRecored.BillToVatNumber = billTo.VatNumber;
                    }
                }
                if (billTo != null)
                {
                    FillBillToContact(invoicesRecored, billTo);
                    invoicesRecored.BillToAddress1 = billTo.Address1;
                    invoicesRecored.BillToAddress2 = billTo.Address2;
                    invoicesRecored.BillToCity = billTo.CityName;
                    invoicesRecored.BillToState = billTo.StateName;
                    invoicesRecored.BillToZipCode = billTo.ZipCode;
                    invoicesRecored.BillToCountry = billTo.CountryName;
                    if (billTo.IsCustomer)
                    {

                        FillCustomerField(invoicesRecored, billTo, tenant);
                    }
                }

                if (localCurrency)
                {
                    totalVat = totalVat + myTotalVats.Sum(d => d.LocalVATAmount);
                    subTotals = subTotals + arInvoice.SubTotalInLocalCurrency;
                    totalGrands = totalGrands + totalVat + subTotals;

                    invoicesRecored.Currency = arInvoice.LocalCurrencyCode;
                    invoicesRecored.SubTotallocal = arInvoice.SubTotalInLocalCurrency;
                    invoicesRecored.VATlocal = myTotalVats.Sum(d => d.LocalVATAmount);
                    invoicesRecored.GrandTotallocal = invoicesRecored.SubTotallocal + invoicesRecored.VATlocal;
                    invoicesRecored.ExpenseCharges = myInvoiceLines.Where(l => l.IsExpense).Sum(s => s.LocalCurrencyAmount);
                    invoicesRecored.VatableAmountLocalCurrency = myInvoiceLines.Where(d => d.VatPercentage > 0).Sum(s => s.LocalCurrencyAmount);
                    invoicesRecored.NonVatableAmountLocalCurrency = myInvoiceLines.Where(d => d.VatPercentage == 0).Sum(s => s.LocalCurrencyAmount);
                    invoicesRecored.RegionalTaxAmountLocalCurrency = myTotalVats.Where(d => d.IsRegionalTax).Sum(s => s.LocalVATAmount);
                }

                else
                {
                    invoicesRecored.Currency = arInvoice.InvoiceCurrencyCode;
                    invoicesRecored.SubTotallocal = arInvoice.SubTotalInInvoiceCurrency;
                    invoicesRecored.VATlocal = myTotalVats.Sum(d => d.InvoiceCurrencyVATAmount);
                    invoicesRecored.GrandTotallocal = invoicesRecored.SubTotallocal + invoicesRecored.VATlocal;
                    invoicesRecored.vatInLocal = myTotalVats.Sum(d => d.LocalVATAmount);
                    invoicesRecored.subInLocal = arInvoice.SubTotalInLocalCurrency;
                    invoicesRecored.LocalCurrency = arInvoice.LocalCurrencyCode;
                    invoicesRecored.ExpenseCharges = myInvoiceLines.Where(l => l.IsExpense).Sum(s => s.InvoiceCurrencyAmount);
                    invoicesRecored.VatableAmountInvoiceCurrency = myInvoiceLines.Where(d => d.VatPercentage > 0).Sum(s => s.InvoiceCurrencyAmount);
                    invoicesRecored.NonVatableAmountInvoiceCurrency = myInvoiceLines.Where(d => d.VatPercentage == 0).Sum(s => s.InvoiceCurrencyAmount);
                    invoicesRecored.RegionalTaxAmountInvoiceCurrency = myTotalVats.Where(d => d.IsRegionalTax).Sum(s => s.InvoiceCurrencyVATAmount);
                }

                if (arInvoice.SATXML != null)
                {
                    List<System.Xml.XmlElement> myLXmlComplementos = GetComprobanteComplementos(arInvoice.SATXML);
                    var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
                    if (timbreFiscalDigitalElement != null)
                    {
                        Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
                        invoicesRecored.UUID = digitalTi.UUID;
                    }
                }

                dataProvider.InvoicesReportList.Add(invoicesRecored);
                dataProvider.InvoicesReportList_NotSorted.Add(invoicesRecored);
            }


            dataProvider.InvoiceTotalsList = (from b in dataProvider.InvoicesReportList
                                              group b by new { b.Currency } into g
                                              select new WebFreight.Web.DataProviders.InvoiceDataProvider.InvoiceTotals()
                                              {
                                                  Currency = g.Key.Currency,
                                                  TotalSubTotals = g.Sum(b => b.SubTotallocal),
                                                  TotalGrands = g.Sum(b => b.GrandTotallocal),
                                                  TotalVats = g.Sum(b => b.VATlocal).Value,
                                                  totalGrandTotal = g.Sum(b => b.vatInLocal) + g.Sum(b => b.subInLocal),
                                                  VAT1Amount = g.Sum(b => b.VAT1Amount),
                                                  VAT2Amount = g.Sum(b => b.VAT2Amount),
                                                  VAT3Amount = g.Sum(b => b.VAT3Amount),
                                                  VAT4Amount = g.Sum(b => b.VAT4Amount),
                                              }).ToList();

            dataProvider.TotalVats_1 = dataProvider.InvoicesReportList.Sum(s => s.VAT1Amount);
            dataProvider.TotalVats_2 = dataProvider.InvoicesReportList.Sum(s => s.VAT2Amount);
            dataProvider.TotalVats_3 = dataProvider.InvoicesReportList.Sum(s => s.VAT3Amount);
            dataProvider.TotalVats_4 = dataProvider.InvoicesReportList.Sum(s => s.VAT4Amount);

            dataProvider.TotalVats = totalVat;
            dataProvider.TotalSub = subTotals;
            dataProvider.TotalsGrands = totalVat + subTotals;
            dataProvider.Name = @"Invoices";

            dataProvider.InvoicesReportList = dataProvider.InvoicesReportList.OrderBy(d => d.InvoiceDate).ToList();
            #endregion

            return dataProvider;
        }
        private void FillBillToContact(InvoiceDataProvider.InvoicesReport invoicesRecored, Card billTo)
        {
            ContactRepository contactRepository = new ContactRepository(billTo.Tenant);
            Contact contact = contactRepository.GetSingleContact(billTo.PrimaryContactId, billTo.Tenant);
            if (contact != null)
            {
                invoicesRecored.BillToContactName = contact.EnglishName;
                invoicesRecored.BillToContactEmail = contact.Email;
            }

        }

        private void FillCustomerField(InvoiceDataProvider.InvoicesReport invoicesRecored, Card billTo, int tenant)
        {
            CustomerRepository customerRepository = new CustomerRepository(tenant);
            Customer customer = customerRepository.GetSingleCustomer(billTo.Id, tenant);
            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            if (customer != null)
            {
                customFieldResolver.SetDataProviderCustomFieldsValues("Customer", tenant, customer, invoicesRecored);
            }
        }

        private List<System.Xml.XmlElement> GetComprobanteComplementos(string sATXML)
        {
            try
            {
                Profact.TimbraCFDI.Comprobante comprobante = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.Comprobante>(sATXML);
                return comprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
            }
            catch (Exception ex)
            {
                return GetNewVersionsComprobanteComplementos(sATXML);
            }
        }

        private static List<System.Xml.XmlElement> GetNewVersionsComprobanteComplementos(string sATXML)
        {
            try
            {
                Profact.TimbraCFDI40.Comprobante comprobante40 = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI40.Comprobante>(sATXML);
                return comprobante40.Complemento.Any.ToList<System.Xml.XmlElement>();
            }
            catch (Exception ex1)
            {
                Profact.TimbraCFDI33.Comprobante comprobante33 = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(sATXML);
                return comprobante33.Complemento.Any.ToList<System.Xml.XmlElement>();
            }
        }

        private List<Shipment> GetShipmentsByARInvoicesMainEntityId(IQueryable<ARInvoiceList> aRInvoices, int tenant)
        {
            List<string> shipmentsIds = aRInvoices.Where(d => d.MainEntityId != null).Select(s => s.MainEntityId).ToList();
            List<Shipment> shipments = new List<Shipment>();
            if (shipmentsIds.Count > 0)
            {
                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                shipments = shipmentRepository.GetShipmentsListFromIdList(shipmentsIds, tenant);
            }
            return shipments;
        }
        #endregion

        #region VDK Report
        internal byte[] LoadVDKDataProvider(byte[] xmlFilters, int tenant)
        {
            VDKDataProvider dataprovider = GetVDKDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(VDKDataProvider), tenant);
        }

        private VDKDataProvider GetVDKDataProvider(byte[] xmlFilters, int tenant)
        {
            VDKDataProvider totalData = new DataProviders.VDKDataProvider();
            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            AddressRepository addressRepository = new AddressRepository(commonContext);

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_tODate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_BranchId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "BranchId").FirstOrDefault();
            QueryFilterItem filterItem_CustomerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();
            QueryFilterItem filterItem_EntityStatus = queryOperations.QueryFilterItems.Where(d => d.FieldName == "EntityStatus").FirstOrDefault();
            QueryFilterItem filterItem_SupplierId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "SupplierId").FirstOrDefault();
            QueryFilterItem filterItem_IncludeOperationalyClose = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeOperationalClose").FirstOrDefault();

            //ToDate
            DateTime? toDate = null;
            if (filterItem_tODate != null)
            {
                if (filterItem_tODate.FieldValue != null)
                {
                    toDate = (DateTime)filterItem_tODate.FieldValue;
                }
            }

            //FromDate
            DateTime? FromDate = null;
            if (filterItem_FromDate != null)
            {
                if (filterItem_FromDate.FieldValue != null)
                {
                    FromDate = (DateTime)filterItem_FromDate.FieldValue;
                }
            }

            string branchId = null;
            totalData.BranchName = "All";
            if (filterItem_BranchId != null)
            {
                if (filterItem_BranchId.FieldValue != null)
                {
                    branchId = filterItem_BranchId.FieldValue.ToString();
                    if (!String.IsNullOrEmpty(branchId))
                    {
                        BranchRepository branchRepository = new BranchRepository(commonContext);
                        Branch branch = branchRepository.GetSingleBranch(branchId, tenant);
                        if (branch != null)
                            totalData.BranchName = branch.EnglishName;
                    }
                }
            }

            string supplierId = null;
            if (filterItem_SupplierId != null)
            {
                if (filterItem_SupplierId.FieldValue != null)
                {
                    supplierId = filterItem_SupplierId.FieldValue.ToString();
                    if (!String.IsNullOrEmpty(supplierId))
                    {
                        CardRepository cardRepository = new CardRepository(commonContext);
                        Card customer = cardRepository.GetSingleCard(supplierId, tenant);
                        if (customer != null)
                            totalData.ShipperName = customer.EnglishName;
                    }
                }
            }

            string entityStatus = null;
            totalData.StatusName = "All";
            if (filterItem_EntityStatus != null)
            {
                if (filterItem_EntityStatus.FieldValue != null)
                {
                    entityStatus = filterItem_EntityStatus.FieldValue.ToString();
                    if (!String.IsNullOrEmpty(entityStatus))
                    {
                        EntityStatusRepository entitiyStatusRepository = new EntityStatusRepository(WebFreightContext.GetContext(tenant));
                        EntityStatus status = entitiyStatusRepository.GetSingleEntityStatus(entityStatus, tenant);
                        if (status != null)
                            totalData.StatusName = status.Name;
                    }
                }
            }

            string CustomerId = null;
            totalData.CustomerName = "All";
            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    CustomerId = filterItem_CustomerId.FieldValue.ToString();

                    if (!String.IsNullOrEmpty(CustomerId))
                    {
                        CardRepository cardRepository = new CardRepository(commonContext);
                        Card customerCard = cardRepository.GetSingleCard(CustomerId, tenant);
                        if (customerCard != null)
                            totalData.CustomerName = customerCard.EnglishName;
                    }
                }
            }


            bool IncludeOperationallyClosed = false;
            if (filterItem_IncludeOperationalyClose != null)
            {
                if (filterItem_IncludeOperationalyClose.FieldValue != null)
                {
                    IncludeOperationallyClosed = (bool)filterItem_IncludeOperationalyClose.FieldValue;
                }
            }

            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant);

            shipments = shipments.Where(d => (d.ShipmentLevelCode == "D" || d.ShipmentLevelCode == "H") && !d.IsCancelled);

            shipments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ShipmentDataView>(new QueryOperations(), shipments, tenant);
            shipments = ProductPermitionsFilter.AddUserProductRestrictionFilters<ShipmentDataView>(new QueryOperations(), shipments, tenant);

            if (toDate != null || FromDate != null)
            {
                #region
                GenericFilter genericFilter = new GenericFilter();

                QueryOperations shipmentsQueryOperations = new QueryOperations()
                {
                    ObjectTableName = "Shipment",
                    PageIndex = 1,
                    PageSize = 10,
                    QuerySection = "Shipments",
                    SortByColumnName = null,
                    SortDirectin = null,
                    QueryFilterItems = new List<QueryFilterItem>(),
                };

                QueryFilterItem item = new QueryFilterItem();
                item.DisplayInList = true;
                item.FieldDataType = "Date";
                item.FieldName = "Field2";

                item.FieldValue = FromDate;
                item.FieldValue2 = toDate;
                item.IsCustomField = true;
                if (toDate == null && FromDate != null)
                {
                    item.Operator = "GreaterThanOrEqual";
                }

                else if (FromDate == null && toDate != null)
                {
                    item.FieldValue = toDate;
                    item.FieldValue2 = null;
                    item.Operator = "LessThanOrEqual";
                }

                //else if (FromDate == null && toDate == null)
                //{
                //    item.Operator = "IsNotNull";
                //}

                else
                {
                    item.Operator = "Between";

                }

                queryOperations.QueryFilterItems.Add(item);

                shipments = genericFilter.GetFilteredQuery<ShipmentDataView>(shipmentsQueryOperations, shipments);
                #endregion
            }

            if (!string.IsNullOrEmpty(branchId))
            {
                shipments = shipments.Where(d => d.BranchId == branchId);
            }


            if (!IncludeOperationallyClosed)
            {
                shipments = shipments.Where(d => d.IsOperationalClosed == false);
            }



            if (!string.IsNullOrEmpty(CustomerId))
            {
                shipments = shipments.Where(d => d.CustomerId == CustomerId);
            }


            if (!string.IsNullOrEmpty(entityStatus))
            {
                shipments = shipments.Where(d => d.StatusId == entityStatus);
            }


            if (!string.IsNullOrEmpty(supplierId))
            {
                shipments = shipments.Where(d => d.ShipperId == supplierId);
            }

            List<ShipmentDataView> Shipments = shipments.ToList();
            List<string> ShipmentIds = Shipments.Select(p => p.Id).ToList();
            // List<string> ConsigneeAdressIds = Shipments.Select(p => p.ConsigneeAddressId).ToList();

            List<ShipmentPackage> shipmentPackages = (from d in shipmentsContext.ShipmentPackages.Include("PackageType") where ShipmentIds.Contains(d.ShipmentId) select d).ToList();
            // List<Card> Consiness = (from d in commonContext.Cards.Include("Address") where ShipmentIds.Contains(d.ShipmentId) select d).ToList();


            if (shipmentPackages.Count > 0)
            {
                totalData.ShipmentPackages = new List<ShipmentPackageRecord>();
                foreach (ShipmentPackage Item in shipmentPackages)
                {
                    ShipmentPackageRecord shipment = new ShipmentPackageRecord();
                    ShipmentDataView dataView = Shipments.Where(d => d.Id == Item.ShipmentId).FirstOrDefault();

                    shipment.ActualETD = dataView.MainCarriageATD;
                    shipment.ConsigneeAddress = dataView.ConsigneeName;
                    shipment.CommodityNumber = Item.CommodityNumber;
                    shipment.ProjectNumber = dataView.ProjectNumber;
                    shipment.Customer = dataView.CustomerName;
                    shipment.Supplier = dataView.ShipperName;
                    shipment.Status = dataView.StatusName;
                    shipment.CustomerRef = dataView.CustomerReference1;
                    shipment.Product = Item.CommodityName;
                    shipment.Weight = Item.VolumetricWeight;
                    shipment.Unit = Item.PackageType != null ? Item.PackageType.EnglishName : null;
                    shipment.RequestETD = dataView.FirstPickupETD;
                    shipment.EstimateETD = dataView.FirstPickupETA;
                    shipment.EstimateETA = dataView.MainCarriageFinalDestinationETA;
                    shipment.Shipper = dataView.ShipperName;
                    shipment.Pieces = Item.Quantity;
                    shipment.ContainerNr = Item.ContainerNumber;
                    shipment.ActualETA = dataView.MainCarriageATA;

                    CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
                    customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, dataView, shipment);

                    totalData.ShipmentPackages.Add(shipment);

                }
            }

            totalData.FromDate = FromDate;
            totalData.ToDate = toDate;
            #endregion

            return totalData;
        }
        #endregion

        #region AgedAccountsReceivable
        [WebMethod]
        public byte[] LoadAgedAccountsReceivableData(byte[] xmlFilters, int tenant)
        {
            AgedAccountsReceivableDataProvider dataprovider = LoadAgedAccountsReceivableDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(AgedAccountsReceivableDataProvider), tenant);
        }

        public AgedAccountsReceivableDataProvider LoadAgedAccountsReceivableDataProvider(byte[] xmlFilters, int tenant)
        {
            AgedAccountsReceivableDataProvider dataProvider = new AgedAccountsReceivableDataProvider();
            dataProvider.AgedAccountsReceivableList = new List<AgedAccountsReceivableDataProvider.AgedAccountsReceivable>();

            IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);

            ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(invoiceContext);
            ARPaymentRepository aRPaymentRepository = new ARPaymentRepository(invoiceContext);
            APInvoiceRepository aPInvoiceRepository = new APInvoiceRepository(invoiceContext);
            APPaymentRepository aPPaymentRepository = new APPaymentRepository(invoiceContext);

            AddressRepository addressRepository = new AddressRepository(commonContext);

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_InvoiceType = queryOperations.QueryFilterItems.Where(d => d.FieldName == "InvoiceType").FirstOrDefault();
            QueryFilterItem filterItem_CurrencyType = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CurrencyType").FirstOrDefault();

            string invoiceType = null;
            string currencyType = null;

            if (filterItem_InvoiceType != null)
            {
                if (filterItem_InvoiceType.FieldValue != null)
                {
                    invoiceType = filterItem_InvoiceType.FieldValue.ToString();
                }
            }

            if (filterItem_CurrencyType != null)
            {
                if (filterItem_CurrencyType.FieldValue != null)
                {
                    currencyType = filterItem_CurrencyType.FieldValue.ToString();
                }
            }

            #endregion

            #region General Report Data

            string localCurrencyCode = "";
            string profitCurrencyCode = "";

            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);
            if (currentTenant != null)
            {
                localCurrencyCode = currentTenant.AccountingCurrencyCode;
                profitCurrencyCode = currentTenant.ProfitCurrencyCode;

                dataProvider.CompanyName = currentTenant.Company;
                dataProvider.TenantName = currentTenant.Company;
                dataProvider.Signature = currentTenant.Signature;
                dataProvider.Logo = DataProviders.General.GetLogo(currentTenant.Id);
                dataProvider.Name = @" Aging report - Statement summarized";

                if (currencyType == "profit")
                {
                    dataProvider.Currency = profitCurrencyCode;
                }
                else
                {
                    dataProvider.Currency = localCurrencyCode;
                }

                Address tenantAddress = addressRepository.GetSingleAddress(currentTenant.AddressId, currentTenant.Id);
                if (tenantAddress != null)
                {
                    dataProvider.Address1 = tenantAddress.Address1;
                    dataProvider.Address2 = tenantAddress.Address2;
                    dataProvider.City = tenantAddress.City;
                    dataProvider.Country = tenantAddress.Country == null ? null : tenantAddress.Country.EnglishName;
                    dataProvider.TenantFax = tenantAddress.FaxNumber;
                    dataProvider.TenantPhone = tenantAddress.PhoneNumber;
                    dataProvider.State = tenantAddress.State == null ? null : tenantAddress.State.EnglishName;
                    dataProvider.ZipCode = tenantAddress.ZipCode;
                }
            }
            #endregion

            #region Base Data Filtered

            IQueryable<ARInvoice> iQueryable_ARInvoice = aRInvoiceRepository.GetUnpaidARInvoices(tenant);
            IQueryable<APInvoice> iQueryable_APInvoice = aPInvoiceRepository.GetUnpaidAPInvoices(tenant);
            IQueryable<ARPayment> iQueryable_ARPayment = aRPaymentRepository.GetOpenedARPayments(tenant);
            IQueryable<APPayment> iQueryable_APPayment = aPPaymentRepository.GetOpenedAPPayments(tenant);
            iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => !d.IsConstituentInvoice);

            #endregion

            #region Fill Aging Statemant Data

            List<AgingStatemantDataItem> list_ARInvoice = (from d in iQueryable_ARInvoice
                                                           where d.DueDate != null
                                                           select new AgingStatemantDataItem()
                                                           {
                                                               Id = "ARInvoice:" + d.Id,
                                                               TypeCode = "AR",
                                                               EntityName = "ARInvoice",
                                                               EntityTypeCode = d.ARInvoiceTypeCode,
                                                               CardId = d.BillToId,
                                                               Date = d.DueDate,
                                                               PartnerId = d.PartnerId,
                                                               Debit = d.AmountDueInLocalCurrency == null ? null : ((d.ARInvoiceTypeCode == "CD" || d.ARInvoiceTypeCode == "CC") ? null : d.AmountDueInLocalCurrency),
                                                               Credit = d.AmountDueInLocalCurrency == null ? null : ((d.ARInvoiceTypeCode != "CD" && d.ARInvoiceTypeCode != "CC") ? null : d.AmountDueInLocalCurrency),
                                                               ProfitExchangeRate = d.ProfitCurrencyExchangeRate,
                                                           }).ToList();

            List<AgingStatemantDataItem> list_APInvoice = (from d in iQueryable_APInvoice
                                                           where d.DueDate != null
                                                           select new AgingStatemantDataItem()
                                                           {
                                                               Id = "APInvoice:" + d.Id,
                                                               TypeCode = "AP",
                                                               EntityName = "APInvoice",
                                                               CardId = d.VendorId,
                                                               Date = d.DueDate,
                                                               Debit = d.AmountDueInLocalCurrency == null ? null : (d.AmountDueInLocalCurrency > 0 ? null : d.AmountDueInLocalCurrency),
                                                               Credit = d.AmountDueInLocalCurrency == null ? null : (d.AmountDueInLocalCurrency > 0 ? d.AmountDueInLocalCurrency : null),
                                                               ProfitExchangeRate = d.ProfitCurrencyExchangeRate,
                                                           }).ToList();

            List<AgingStatemantDataItem> list_ARPayment = (from d in iQueryable_ARPayment
                                                           where d.ValueDate != null
                                                           select new AgingStatemantDataItem()
                                                           {
                                                               Id = "ARPayment:" + d.Id,
                                                               TypeCode = "AR",
                                                               EntityName = "ARPayment",
                                                               CardId = d.BillToId,
                                                               PartnerId = d.PartnerId,
                                                               Date = d.ValueDate,
                                                               Credit = d.OpenAmount * d.PaymentCurrencyExchangeRate,
                                                               ProfitExchangeRate = d.ProfitCurrencyExchangeRate,
                                                           }).ToList();

            List<AgingStatemantDataItem> list_APPayment = (from d in iQueryable_APPayment
                                                           where d.ValueDate != null
                                                           select new AgingStatemantDataItem()
                                                           {
                                                               Id = "APPayment:" + d.Id,
                                                               TypeCode = "AP",
                                                               EntityName = "APPayment",
                                                               CardId = d.VendorId,
                                                               Date = d.ValueDate,
                                                               Debit = d.OpenAmount * d.PaymentCurrencyExchangeRate,
                                                               ProfitExchangeRate = d.ProfitCurrencyExchangeRate,
                                                           }).ToList();
            this.FixValues(list_ARInvoice);
            this.FixValues(list_APInvoice);
            this.FixValues(list_ARPayment);
            this.FixValues(list_APPayment);

            List<AgingStatemantDataItem> totalList = new List<AgingStatemantDataItem>();

            if (invoiceType == "All")
            {
                totalList = list_ARInvoice.Concat(list_APInvoice).Concat(list_ARPayment).Concat(list_APPayment).ToList();
            }

            else if (invoiceType == "AR")
            {
                totalList = list_ARInvoice.Concat(list_ARPayment).ToList();
            }

            else if (invoiceType == "AP")
            {
                totalList = list_APInvoice.Concat(list_APPayment).ToList();
            }

            #endregion

            #region Fill


            List<string> cardIdsList = totalList.Select(s => s.CardId).ToList();
            cardIdsList = cardIdsList.Distinct().ToList();
            List<CardEntityClass> allCardData = (from d in commonContext.Cards.Include("PaymentTerm").Include("PartnerType").Include("SalesmanUser").Include("SalesmanUser.Contact")
                                                 where d.Tenant == tenant && cardIdsList.Contains(d.Id)
                                                 select new CardEntityClass
                                                 {
                                                     Id = d.Id,
                                                     Name = d.EnglishName,
                                                     PaymentTerm = d.PaymentTerm == null ? null : d.PaymentTerm.EnglishName,
                                                     CardCode = d.Code,
                                                     CardTypeName = d.PartnerType == null ? null : d.PartnerType.Name,
                                                     Salesman = d.SalesmanUser == null ? null : (d.SalesmanUser.Contact == null ? null : d.SalesmanUser.Contact.EnglishName),
                                                 }).ToList();

            List<string> partnersIdsFromTheTotalList = totalList.Select(s => s.PartnerId).ToList();
            partnersIdsFromTheTotalList = partnersIdsFromTheTotalList.Distinct().ToList();

            List<CardEntityClass> partnersDataAsCards = (from d in commonContext.Cards.Include("PaymentTerm").Include("PartnerType")
                                                         where d.Tenant == tenant && partnersIdsFromTheTotalList.Contains(d.Id)
                                                         select new CardEntityClass
                                                         {
                                                             Id = d.Id,
                                                             Name = d.EnglishName,
                                                             PaymentTerm = d.PaymentTerm == null ? null : d.PaymentTerm.EnglishName,
                                                             CardCode = d.Code,
                                                             CardTypeName = d.PartnerType == null ? null : d.PartnerType.Name,
                                                         }).ToList();
            double? currencyRate = 0;
            if (!string.IsNullOrEmpty(localCurrencyCode) && !string.IsNullOrEmpty(profitCurrencyCode))
            {
                Currency localCurrency = commonContext.Currencies.Where(d => d.Code == localCurrencyCode && d.Tenant == tenant).FirstOrDefault();
                Currency profictCurrency = commonContext.Currencies.Where(d => d.Code == profitCurrencyCode && d.Tenant == tenant).FirstOrDefault();

                if (localCurrency != null && profictCurrency != null)
                {
                    LastRate rate = this.GetCurrencysExchangeRate(tenant, localCurrency, profictCurrency, TenantServerConfigration.GetCurrentDateTime(tenant).Date);
                    if (rate != null && rate.Rate != 0)
                    {
                        dataProvider.Rate = "1 " + profitCurrencyCode + " = " + rate.Rate + " " + localCurrencyCode;
                        currencyRate = rate.Rate;
                    }
                }
            }

            foreach (string cardId in cardIdsList)
            {
                AgedAccountsReceivableDataProvider.AgedAccountsReceivable acountsRecored = new AgedAccountsReceivableDataProvider.AgedAccountsReceivable();

                double? currentsum = 0;
                double? sum1_30 = 0;
                double? sum31_60 = 0;
                double? sum61_90 = 0;
                double? sum91_120 = 0;
                double? over120 = 0;

                double? sum1_15 = 0;
                double? sum16_30 = 0;
                double? sum1_24 = 0;
                double? sum25_30 = 0;

                double? sum31_45 = 0;
                double? sum46_60 = 0;

                double? sum61_75 = 0;
                double? sum76_90 = 0;
                double? sum91_105 = 0;
                double? sum106_120 = 0;

                List<AgingStatemantDataItem> tempList = totalList.Where(d => d.CardId == cardId).ToList();

                List<AgingStatemantDataItem> currentDueItems = tempList.Where(d => d.Date >= todayDate).ToList();
                List<AgingStatemantDataItem> Due1_30Items = tempList.Where(d => (todayDate - d.Date.Value).TotalDays >= 1 && (todayDate - d.Date.Value).TotalDays <= 30).ToList();
                List<AgingStatemantDataItem> Due31_60Items = tempList.Where(d => (((todayDate - d.Date.Value).TotalDays) > 30) && (((todayDate - d.Date.Value).TotalDays) <= 60)).ToList();
                List<AgingStatemantDataItem> Due61_90Items = tempList.Where(d => (((todayDate - d.Date.Value).TotalDays) > 60) && (((todayDate - d.Date.Value).TotalDays) <= 90)).ToList();
                List<AgingStatemantDataItem> Due91_120Items = tempList.Where(d => (((todayDate - d.Date.Value).TotalDays) > 90) && (((todayDate - d.Date.Value).TotalDays) <= 120)).ToList();
                List<AgingStatemantDataItem> over120Items = tempList.Where(d => (todayDate - d.Date.Value).TotalDays > 120).ToList();

                List<AgingStatemantDataItem> Due1_15Items = tempList.Where(d => (todayDate - d.Date.Value).TotalDays >= 1 && (todayDate - d.Date.Value).TotalDays <= 15).ToList();
                List<AgingStatemantDataItem> Due16_30Items = tempList.Where(d => (((todayDate - d.Date.Value).TotalDays) > 15) && (((todayDate - d.Date.Value).TotalDays) <= 30)).ToList();

                List<AgingStatemantDataItem> Due1_24Items = tempList.Where(d => (todayDate - d.Date.Value).TotalDays >= 1 && (todayDate - d.Date.Value).TotalDays <= 24).ToList();
                List<AgingStatemantDataItem> Due25_30Items = tempList.Where(d => (((todayDate - d.Date.Value).TotalDays) > 24) && (((todayDate - d.Date.Value).TotalDays) <= 30)).ToList();

                List<AgingStatemantDataItem> Due31_45Items = tempList.Where(d => (todayDate - d.Date.Value).TotalDays >= 31 && (todayDate - d.Date.Value).TotalDays <= 45).ToList();
                List<AgingStatemantDataItem> Due46_60Items = tempList.Where(d => (((todayDate - d.Date.Value).TotalDays) > 45) && (((todayDate - d.Date.Value).TotalDays) <= 60)).ToList();

                List<AgingStatemantDataItem> Due61_75Items = tempList.Where(d => (todayDate - d.Date.Value).TotalDays >= 61 && (todayDate - d.Date.Value).TotalDays <= 75).ToList();
                List<AgingStatemantDataItem> Due76_90Items = tempList.Where(d => ((todayDate - d.Date.Value).TotalDays >= 76) && ((todayDate - d.Date.Value).TotalDays <= 90)).ToList();
                List<AgingStatemantDataItem> Due91_105Items = tempList.Where(d => (todayDate - d.Date.Value).TotalDays >= 91 && (todayDate - d.Date.Value).TotalDays <= 105).ToList();
                List<AgingStatemantDataItem> Due106_120Items = tempList.Where(d => ((todayDate - d.Date.Value).TotalDays >= 106) && ((todayDate - d.Date.Value).TotalDays <= 120)).ToList();

                if (currencyType == "profit")
                {
                    currentsum = (currentDueItems.Sum(d => (d.Debit + d.Credit) / d.ProfitExchangeRate));
                    sum1_30 = (Due1_30Items.Sum(d => (d.Debit + d.Credit) / d.ProfitExchangeRate));
                    sum31_60 = (Due31_60Items.Sum(d => (d.Debit + d.Credit) / d.ProfitExchangeRate));
                    sum61_90 = (Due61_90Items.Sum(d => (d.Debit + d.Credit) / d.ProfitExchangeRate));
                    sum91_120 = (Due91_120Items.Sum(d => (d.Debit + d.Credit) / d.ProfitExchangeRate));
                    over120 = (over120Items.Sum(d => (d.Debit + d.Credit) / d.ProfitExchangeRate));

                    sum1_15 = (Due1_15Items.Sum(d => (d.Debit + d.Credit) / d.ProfitExchangeRate));
                    sum16_30 = (Due16_30Items.Sum(d => (d.Debit + d.Credit) / d.ProfitExchangeRate));
                    sum1_24 = (Due1_24Items.Sum(d => (d.Debit + d.Credit) / d.ProfitExchangeRate));
                    sum25_30 = (Due25_30Items.Sum(d => (d.Debit + d.Credit) / d.ProfitExchangeRate));

                    sum31_45 = (Due31_45Items.Sum(d => (d.Debit + d.Credit) / d.ProfitExchangeRate));
                    sum46_60 = (Due46_60Items.Sum(d => (d.Debit + d.Credit) / d.ProfitExchangeRate));

                    sum61_75 = Due61_75Items.Sum(d => (d.Debit + d.Credit) / d.ProfitExchangeRate);
                    sum76_90 = Due76_90Items.Sum(d => (d.Debit + d.Credit) / d.ProfitExchangeRate);
                    sum91_105 = Due91_105Items.Sum(d => (d.Debit + d.Credit) / d.ProfitExchangeRate);
                    sum106_120 = Due106_120Items.Sum(d => (d.Debit + d.Credit) / d.ProfitExchangeRate);
                }

                else
                {
                    currentsum = currentDueItems.Sum(d => d.Debit + d.Credit);
                    sum1_30 = Due1_30Items.Sum(d => d.Debit + d.Credit);
                    sum31_60 = Due31_60Items.Sum(d => d.Debit + d.Credit);
                    sum61_90 = Due61_90Items.Sum(d => d.Debit + d.Credit);
                    sum91_120 = Due91_120Items.Sum(d => d.Debit + d.Credit);
                    over120 = over120Items.Sum(d => d.Debit + d.Credit);

                    sum1_15 = Due1_15Items.Sum(d => d.Debit + d.Credit);
                    sum16_30 = Due16_30Items.Sum(d => d.Debit + d.Credit);
                    sum1_24 = Due1_24Items.Sum(d => d.Debit + d.Credit);
                    sum25_30 = Due25_30Items.Sum(d => d.Debit + d.Credit);

                    sum31_45 = Due31_45Items.Sum(d => (d.Debit + d.Credit));
                    sum46_60 = Due46_60Items.Sum(d => (d.Debit + d.Credit));

                    sum61_75 = Due61_75Items.Sum(d => d.Debit + d.Credit);
                    sum76_90 = Due76_90Items.Sum(d => d.Debit + d.Credit);
                    sum91_105 = Due91_105Items.Sum(d => d.Debit + d.Credit);
                    sum106_120 = Due106_120Items.Sum(d => d.Debit + d.Credit);
                }

                CardEntityClass cardEntity = allCardData.Where(d => d.Id == cardId).FirstOrDefault();
                if (cardEntity != null)
                {
                    acountsRecored.PaymentTerm = cardEntity.PaymentTerm;
                    acountsRecored.CustomerName = cardEntity.Name;
                    acountsRecored.CardCode = cardEntity.CardCode;
                    acountsRecored.CardTypeName = cardEntity.CardTypeName;
                    acountsRecored.Salesman = cardEntity.Salesman;
                }

                foreach (string partnerId in partnersIdsFromTheTotalList)
                {
                    List<AgingStatemantDataItem> partnersSharedToSameCustomer = tempList.Where(d => d.PartnerId == partnerId).ToList();
                    if (partnersSharedToSameCustomer.Count != 0)
                    {
                        CardEntityClass cardEntityForPartner = partnersDataAsCards.Where(d => d.Id == partnerId).FirstOrDefault();
                        if (cardEntityForPartner != null)
                        {
                            acountsRecored.PartnerName = acountsRecored.PartnerName == null ? cardEntityForPartner.Name : acountsRecored.PartnerName + ", " + cardEntityForPartner.Name;
                        }
                    }
                }

                acountsRecored.CurrentDue = currentsum;
                acountsRecored.DaysPastDue1_30 = sum1_30;
                acountsRecored.DaysPastDue31_60 = sum31_60;
                acountsRecored.DaysPastDue61_90 = sum61_90;
                acountsRecored.DaysPastDue91_120 = sum91_120;
                acountsRecored.Over120DaysPastDue = over120;
                acountsRecored.CustomerTotals = currentsum + sum1_30 + sum31_60 + sum61_90 + sum91_120 + over120;

                acountsRecored.DaysPastDue1_15 = sum1_15;
                acountsRecored.DaysPastDue16_30 = sum16_30;
                acountsRecored.DaysPastDue1_24 = sum1_24;
                acountsRecored.DaysPastDue25_30 = sum25_30;

                acountsRecored.DaysPastDue31_45 = sum31_45;
                acountsRecored.DaysPastDue46_60 = sum46_60;

                acountsRecored.DaysPastDue61_75 = sum61_75;
                acountsRecored.DaysPastDue76_90 = sum76_90;
                acountsRecored.DaysPastDue91_105 = sum91_105;
                acountsRecored.DaysPastDue106_120 = sum106_120;

                if (acountsRecored.CustomerTotals != 0)
                {

                    dataProvider.AgedAccountsReceivableList.Add(acountsRecored);

                }
            }

            #endregion

            return dataProvider;
        }
        private void FixValues(List<AgingStatemantDataItem> list)
        {
            foreach (AgingStatemantDataItem item in list)
            {
                if (item.Credit == null)
                {
                    item.Credit = 0;
                }

                else if (item.Credit > 0)
                {
                    item.Credit *= -1;
                }

                if (item.Debit == null)
                {
                    item.Debit = 0;
                }

                else if (item.Debit < 0)
                {
                    item.Debit *= -1;
                }
            }
        }
        private LastRate GetCurrencysExchangeRate(int tenant, Currency localCurrency, Currency foreignCurrency, DateTime rateDate)
        {
            LastRate result = null;

            IWebFreightContext objectContext = WebFreightContext.GetContext(tenant);
            RatesTableRepository ratesTablesRepository = new RatesTableRepository(objectContext);
            RatesTableQuery ratesTableQuery = new RatesTableQuery(ratesTablesRepository);
            CurrencyRepository currencyRepository = new CurrencyRepository(tenant);

            LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, foreignCurrency.Id, localCurrency.Id, rateDate);
            if (lastRate != null)
            {
                lastRate.BaseCurrencyId = localCurrency.Id;
                lastRate.BaseCurrencyCode = localCurrency.Code;
                result = lastRate;
            }

            return result;
        }
        #endregion

        #region IATAStatistics
        [WebMethod]
        public byte[] LoadIATAStatisticsData(byte[] xmlFilters, bool isClosed, int tenant)
        {
            IATAStatisticsDataProvider dataprovider = LoadIATAStatisticsDataProvider(xmlFilters, isClosed, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(IATAStatisticsDataProvider), tenant);
        }

        public IATAStatisticsDataProvider LoadIATAStatisticsDataProvider(byte[] xmlFilters, bool isClosed, int tenant)
        {
            IATAStatisticsDataProvider dataProvider = new IATAStatisticsDataProvider();
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);

            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            GenericFilter filter = new GenericFilter();
            ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);
            IQueryable<ShipmentDataView> iQueryable = shipmentRepository.GetMasterViewsByTenant(tenant);

            iQueryable = customfilters.GetFilteredQuery(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            QueryFilterItem fromDateItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate" && d.Operator == "GreaterThanOrEqual").FirstOrDefault();
            QueryFilterItem ToDateItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate" && d.Operator == "LessThanOrEqual").FirstOrDefault();

            nonListQueryOperation.QueryFilterItems.Remove(fromDateItem);
            nonListQueryOperation.QueryFilterItems.Remove(ToDateItem);
            iQueryable = filter.GetFilteredQuery<ShipmentDataView>(nonListQueryOperation, iQueryable);

            QueryFilterItem customerItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "MainCarriageCarrierId" && d.Operator == "Equals").FirstOrDefault();
            if (customerItem != null)
            {
                string id = customerItem.FieldValue.ToString();
                Card airline = CardRepository.GetSingleCard(id, tenant, true);
                dataProvider.SelectedAirline = airline.EnglishName;
            }
            else
            {
                // dataProvider.airline = "All";
            }

            AddressQuery addressQuery = new AddressQuery(tenant);
            AddressPM address = addressQuery.GetSingleAddressPM(currentTenant.AddressId, currentTenant.Id, false);
            if (address != null)
            {
                dataProvider.Address1 = address.Address1;
                dataProvider.Address2 = address.Address2;
                dataProvider.City = address.City;
                dataProvider.Country = address.CountryName;
                dataProvider.TenantFax = address.FaxNumber;
                dataProvider.TenantPhone = address.PhoneNumber;
                dataProvider.State = address.StateEnglishName;
                dataProvider.ZipCode = address.ZipCode;
            }

            dataProvider.TenantName = currentTenant.Company;
            dataProvider.Signature = currentTenant.Signature;
            dataProvider.Logo = DataProviders.General.GetLogo(currentTenant.Id);
            dataProvider.ChargeableWeightUnit = currentTenant.ChargeableWeightUnitCode;

            iQueryable = iQueryable.Where(f => f.IsCancelled == false && f.TransportModeId == "A" && f.DirectionId == "E");
            IQueryable<ShipmentDataView> iQueryable1 = iQueryable.Where(d => d.MainCarriageATD != null);
            IQueryable<ShipmentDataView> iQueryable2 = iQueryable.Where(d => d.MainCarriageATD == null && d.MainCarriageETD != null);
            IQueryable<ShipmentDataView> iQueryable3 = iQueryable.Where(d => d.MainCarriageATD == null && d.MainCarriageETD == null);

            QueryFilterItem newfromDateItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate" && d.Operator == "GreaterThanOrEqual").FirstOrDefault();
            DateTime fromDate;
            if (newfromDateItem.FieldValue != null)
            {
                DateTime.TryParse(newfromDateItem.FieldValue.ToString(), out fromDate);
                dataProvider.FromPeriod = fromDate;

                iQueryable1 = iQueryable1.Where(f => f.MainCarriageATD >= fromDate);
                iQueryable2 = iQueryable2.Where(f => f.MainCarriageETD >= fromDate);
                iQueryable3 = iQueryable3.Where(f => f.CreateDateTime >= fromDate);
            }

            QueryFilterItem newToDateItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate" && d.Operator == "LessThanOrEqual").FirstOrDefault();
            DateTime toDate;
            if (newToDateItem.FieldValue != null)
            {
                DateTime.TryParse(newToDateItem.FieldValue.ToString(), out toDate);
                dataProvider.ToPeriod = toDate;

                iQueryable1 = iQueryable1.Where(f => f.MainCarriageATD <= toDate);
                iQueryable2 = iQueryable2.Where(f => f.MainCarriageETD <= toDate);
                iQueryable3 = iQueryable3.Where(f => f.CreateDateTime <= toDate);
            }

            #region queryATD
            IQueryable<ShipmentList> queryATD = from f in iQueryable1
                                                select new ShipmentList()
                                                {
                                                    MainCarriageCarrierPrefix = f.MainCarriageAirlinePrefix,
                                                    CarrierLastStatusDate = f.CarrierLastStatusDate,
                                                    CarrierLastStatusName = f.CarrierLastStatusName,
                                                    CarrierLastStatusCode = f.CarrierLastStatusCode,
                                                    IsOperationalClosed = f.IsOperationalClosed,
                                                    ShipmentViewId = f.Id,
                                                    Id = f.Id,
                                                    Shipper = f.ShipperName,
                                                    Consignee = f.ConsigneeName,
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
                                                    Field10 = f.Field10,
                                                    ChargeableWeightInKG = f.ChargeableWeightInKG,
                                                    ChargeableWeight = f.ChargeableWeight,
                                                    GrossWeight = f.GrossWeight,
                                                    ShipperReference1 = f.ShipperReference1,
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
                                                    FromPortId = !string.IsNullOrEmpty(f.MainCarriageFromPortId) ? f.MainCarriageFromPortId : f.FromPortId,
                                                    ToPortId = !string.IsNullOrEmpty(f.MainCarriageToPortId) ? f.MainCarriageToPortId : f.ToPortId,
                                                    FromPort = !string.IsNullOrEmpty(f.MainCarriageFromPortCode) ? f.MainCarriageFromPortCode : f.FromPortCode,
                                                    FromPortName = !string.IsNullOrEmpty(f.MainCarriageFromPortName) ? f.MainCarriageFromPortName : f.FromPortName,
                                                    FromPortCountry = f.MainCarriageFromPortCountryName,
                                                    ToPort = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortCode) ? f.MainCarriageFinalDestinationPortCode : f.ToPortCode,
                                                    ToPortName = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortName) ? f.MainCarriageFinalDestinationPortName : f.ToPortName,
                                                    ToPortCountry = f.MainCarriageToPortCountryName,
                                                    MasterShipmentDataId = f.MasterShipmentDataId,
                                                    BranchName = f.BranchName,
                                                    CustomerName = f.CustomerName,
                                                    GrossWeightInKG = f.GrossWeightInKG,
                                                    ShipmentLevelCode = f.ShipmentLevelCode,
                                                    ShipmentLevelName = f.ShipmentLevelName,
                                                    VolumetricWeight = f.VolumetricWeight,
                                                    AirlinePrefix = f.AirlinePrefix,
                                                    AccountedPayablesInLocalCurrency = f.AccountedPayablesInLocalCurrency,
                                                    AccountedPayablesInProfitCurrency = f.AccountedPayablesInProfitCurrency,
                                                    AccountedReceivablesInProfitCurrency = f.AccountedReceivablesInProfitCurrency,
                                                    MainCarriageFromPortId = f.MainCarriageFromPortId,
                                                    MainCarriageFromPortName = f.MainCarriageFromPortName,
                                                    MainCarriageATA = f.MainCarriageATA,
                                                    MainCarriageETD = f.MainCarriageETD,
                                                    IncotermId = f.IncotermId,
                                                    OpenPayablesInProfitCurrency = f.OpenPayablesInProfitCurrency,
                                                    CustomerReference1 = f.CustomerReference1,
                                                    CustomerReference2 = f.CustomerReference2,
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
                                                    FNAReason = f.FNAReason,
                                                    AgentName = f.AgentName,
                                                    MainCarriageCarrierName = f.MainCarriageCarrierName,
                                                    FinalArrivalDate = f.FinalArrivalDate,
                                                    CustomerId = f.CustomerId,
                                                    ShipperId = f.ShipperId,
                                                    ConsigneeId = f.ConsigneeId,
                                                    TEU = f.TEU,
                                                    VolumeInCBM = f.VolumeInCBM,
                                                    CASSCode = f.CASSCode,
                                                    ChargeableWeightUnitCode = f.ChargeableWeightUnitCode,
                                                    AWBCurrencyCode = f.AWBCurrencyCode,
                                                    AWBChargeAmount = f.FreightPrepaidCollectId == "P" ? f.AWBFreightAmountPrepaid : f.AWBFreightAmountCollect,
                                                    StatusId = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusId : f.ShipmentStatusId) : (f.ShipmentStatusId),
                                                    StatusDate = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusDate : f.ShipmentStatusDate) : (f.ShipmentStatusDate),
                                                    StatusName = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusName : f.ShipmentStatusName) : (f.ShipmentStatusName),
                                                    StatusLocation = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusLocation : f.ShipmentStatusLocation) : (f.ShipmentStatusLocation),
                                                    LongMaster = f.TransportModeId == "A" ? (!string.IsNullOrEmpty(f.AirlinePrefix) && !string.IsNullOrEmpty(f.Master) ? f.AirlinePrefix + "-" + f.Master : "") : f.Master,
                                                };
            #endregion

            #region queryETD
            IQueryable<ShipmentList> queryETD = from f in iQueryable2
                                                select new ShipmentList()
                                                {
                                                    MainCarriageCarrierPrefix = f.MainCarriageAirlinePrefix,
                                                    CarrierLastStatusDate = f.CarrierLastStatusDate,
                                                    CarrierLastStatusName = f.CarrierLastStatusName,
                                                    CarrierLastStatusCode = f.CarrierLastStatusCode,
                                                    IsOperationalClosed = f.IsOperationalClosed,
                                                    ShipmentViewId = f.Id,
                                                    Id = f.Id,
                                                    Shipper = f.ShipperName,
                                                    Consignee = f.ConsigneeName,
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
                                                    Field10 = f.Field10,
                                                    ChargeableWeightInKG = f.ChargeableWeightInKG,
                                                    ChargeableWeight = f.ChargeableWeight,
                                                    GrossWeight = f.GrossWeight,
                                                    ShipperReference1 = f.ShipperReference1,
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
                                                    FromPortId = !string.IsNullOrEmpty(f.MainCarriageFromPortId) ? f.MainCarriageFromPortId : f.FromPortId,
                                                    ToPortId = !string.IsNullOrEmpty(f.MainCarriageToPortId) ? f.MainCarriageToPortId : f.ToPortId,
                                                    FromPort = !string.IsNullOrEmpty(f.MainCarriageFromPortCode) ? f.MainCarriageFromPortCode : f.FromPortCode,
                                                    FromPortName = !string.IsNullOrEmpty(f.MainCarriageFromPortName) ? f.MainCarriageFromPortName : f.FromPortName,
                                                    FromPortCountry = f.MainCarriageFromPortCountryName,
                                                    ToPort = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortCode) ? f.MainCarriageFinalDestinationPortCode : f.ToPortCode,
                                                    ToPortName = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortName) ? f.MainCarriageFinalDestinationPortName : f.ToPortName,
                                                    ToPortCountry = f.MainCarriageToPortCountryName,
                                                    MasterShipmentDataId = f.MasterShipmentDataId,
                                                    BranchName = f.BranchName,
                                                    CustomerName = f.CustomerName,
                                                    GrossWeightInKG = f.GrossWeightInKG,
                                                    ShipmentLevelCode = f.ShipmentLevelCode,
                                                    ShipmentLevelName = f.ShipmentLevelName,
                                                    VolumetricWeight = f.VolumetricWeight,
                                                    AirlinePrefix = f.AirlinePrefix,
                                                    AccountedPayablesInLocalCurrency = f.AccountedPayablesInLocalCurrency,
                                                    AccountedPayablesInProfitCurrency = f.AccountedPayablesInProfitCurrency,
                                                    AccountedReceivablesInProfitCurrency = f.AccountedReceivablesInProfitCurrency,
                                                    MainCarriageFromPortId = f.MainCarriageFromPortId,
                                                    MainCarriageFromPortName = f.MainCarriageFromPortName,
                                                    MainCarriageATA = f.MainCarriageATA,
                                                    MainCarriageETD = f.MainCarriageETD,
                                                    IncotermId = f.IncotermId,
                                                    OpenPayablesInProfitCurrency = f.OpenPayablesInProfitCurrency,
                                                    CustomerReference1 = f.CustomerReference1,
                                                    CustomerReference2 = f.CustomerReference2,
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
                                                    FNAReason = f.FNAReason,
                                                    AgentName = f.AgentName,
                                                    MainCarriageCarrierName = f.MainCarriageCarrierName,
                                                    FinalArrivalDate = f.FinalArrivalDate,
                                                    CustomerId = f.CustomerId,
                                                    ShipperId = f.ShipperId,
                                                    ConsigneeId = f.ConsigneeId,
                                                    TEU = f.TEU,
                                                    VolumeInCBM = f.VolumeInCBM,
                                                    CASSCode = f.CASSCode,
                                                    ChargeableWeightUnitCode = f.ChargeableWeightUnitCode,
                                                    AWBCurrencyCode = f.AWBCurrencyCode,
                                                    AWBChargeAmount = f.FreightPrepaidCollectId == "P" ? f.AWBFreightAmountPrepaid : f.AWBFreightAmountCollect,
                                                    StatusId = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusId : f.ShipmentStatusId) : (f.ShipmentStatusId),
                                                    StatusDate = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusDate : f.ShipmentStatusDate) : (f.ShipmentStatusDate),
                                                    StatusName = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusName : f.ShipmentStatusName) : (f.ShipmentStatusName),
                                                    StatusLocation = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusLocation : f.ShipmentStatusLocation) : (f.ShipmentStatusLocation),
                                                    LongMaster = f.TransportModeId == "A" ? (!string.IsNullOrEmpty(f.AirlinePrefix) && !string.IsNullOrEmpty(f.Master) ? f.AirlinePrefix + "-" + f.Master : "") : f.Master,
                                                };
            #endregion

            #region queryCreatDate
            IQueryable<ShipmentList> queryCreatDate = from f in iQueryable3
                                                      select new ShipmentList()
                                                      {
                                                          MainCarriageCarrierPrefix = f.MainCarriageAirlinePrefix,
                                                          CarrierLastStatusDate = f.CarrierLastStatusDate,
                                                          CarrierLastStatusName = f.CarrierLastStatusName,
                                                          CarrierLastStatusCode = f.CarrierLastStatusCode,
                                                          IsOperationalClosed = f.IsOperationalClosed,
                                                          ShipmentViewId = f.Id,
                                                          Id = f.Id,
                                                          Shipper = f.ShipperName,
                                                          Consignee = f.ConsigneeName,
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
                                                          Field10 = f.Field10,
                                                          ChargeableWeightInKG = f.ChargeableWeightInKG,
                                                          ChargeableWeight = f.ChargeableWeight,
                                                          GrossWeight = f.GrossWeight,
                                                          ShipperReference1 = f.ShipperReference1,
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
                                                          FromPortId = !string.IsNullOrEmpty(f.MainCarriageFromPortId) ? f.MainCarriageFromPortId : f.FromPortId,
                                                          ToPortId = !string.IsNullOrEmpty(f.MainCarriageToPortId) ? f.MainCarriageToPortId : f.ToPortId,
                                                          FromPort = !string.IsNullOrEmpty(f.MainCarriageFromPortCode) ? f.MainCarriageFromPortCode : f.FromPortCode,
                                                          FromPortName = !string.IsNullOrEmpty(f.MainCarriageFromPortName) ? f.MainCarriageFromPortName : f.FromPortName,
                                                          FromPortCountry = f.MainCarriageFromPortCountryName,
                                                          ToPort = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortCode) ? f.MainCarriageFinalDestinationPortCode : f.ToPortCode,
                                                          ToPortName = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortName) ? f.MainCarriageFinalDestinationPortName : f.ToPortName,
                                                          ToPortCountry = f.MainCarriageToPortCountryName,
                                                          MasterShipmentDataId = f.MasterShipmentDataId,
                                                          BranchName = f.BranchName,
                                                          CustomerName = f.CustomerName,
                                                          GrossWeightInKG = f.GrossWeightInKG,
                                                          ShipmentLevelCode = f.ShipmentLevelCode,
                                                          ShipmentLevelName = f.ShipmentLevelName,
                                                          VolumetricWeight = f.VolumetricWeight,
                                                          AirlinePrefix = f.AirlinePrefix,
                                                          AccountedPayablesInLocalCurrency = f.AccountedPayablesInLocalCurrency,
                                                          AccountedPayablesInProfitCurrency = f.AccountedPayablesInProfitCurrency,
                                                          AccountedReceivablesInProfitCurrency = f.AccountedReceivablesInProfitCurrency,
                                                          MainCarriageFromPortId = f.MainCarriageFromPortId,
                                                          MainCarriageFromPortName = f.MainCarriageFromPortName,
                                                          MainCarriageATA = f.MainCarriageATA,
                                                          MainCarriageETD = f.MainCarriageETD,
                                                          IncotermId = f.IncotermId,
                                                          OpenPayablesInProfitCurrency = f.OpenPayablesInProfitCurrency,
                                                          CustomerReference1 = f.CustomerReference1,
                                                          CustomerReference2 = f.CustomerReference2,
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
                                                          FNAReason = f.FNAReason,
                                                          AgentName = f.AgentName,
                                                          MainCarriageCarrierName = f.MainCarriageCarrierName,
                                                          FinalArrivalDate = f.FinalArrivalDate,
                                                          CustomerId = f.CustomerId,
                                                          ShipperId = f.ShipperId,
                                                          ConsigneeId = f.ConsigneeId,
                                                          TEU = f.TEU,
                                                          VolumeInCBM = f.VolumeInCBM,
                                                          CASSCode = f.CASSCode,
                                                          ChargeableWeightUnitCode = f.ChargeableWeightUnitCode,
                                                          AWBCurrencyCode = f.AWBCurrencyCode,
                                                          AWBChargeAmount = f.FreightPrepaidCollectId == "P" ? f.AWBFreightAmountPrepaid : f.AWBFreightAmountCollect,
                                                          StatusId = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusId : f.ShipmentStatusId) : (f.ShipmentStatusId),
                                                          StatusDate = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusDate : f.ShipmentStatusDate) : (f.ShipmentStatusDate),
                                                          StatusName = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusName : f.ShipmentStatusName) : (f.ShipmentStatusName),
                                                          StatusLocation = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusLocation : f.ShipmentStatusLocation) : (f.ShipmentStatusLocation),
                                                          LongMaster = f.TransportModeId == "A" ? (!string.IsNullOrEmpty(f.AirlinePrefix) && !string.IsNullOrEmpty(f.Master) ? f.AirlinePrefix + "-" + f.Master : "") : f.Master,
                                                      };
            #endregion

            IQueryable<ShipmentList> fullQuery = queryATD.Concat(queryETD).Concat(queryCreatDate);

            dataProvider.RecordList = new List<IATAStatisticsDataProvider.IATAStatisticsRecord>();
            dataProvider.GroupList = new List<IATAStatisticsDataProvider.IATAStatisticsGroup>();

            if (currentTenant.IATA != null && currentTenant.CASSCode != null)
            {
                dataProvider.IataCASSCode = currentTenant.IATA + '/' + currentTenant.CASSCode;
            }
            else if (currentTenant.IATA != null && currentTenant.CASSCode == null)
            {
                dataProvider.IataCASSCode = currentTenant.IATA;
            }
            else if (currentTenant.IATA == null && currentTenant.CASSCode != null)
            {
                dataProvider.IataCASSCode = currentTenant.CASSCode;
            }

            foreach (ShipmentList a in fullQuery)
            {
                IATAStatisticsDataProvider.IATAStatisticsRecord statisticsrecord = new IATAStatisticsDataProvider.IATAStatisticsRecord();

                statisticsrecord.AirlineId = a.MainCarriageCarrierId;
                statisticsrecord.AirlineName = a.MainCarriageCarrierName;
                statisticsrecord.ChargeableWeight = a.ChargeableWeight;
                statisticsrecord.Destination = a.ToPortName;
                statisticsrecord.Master = a.LongMaster;
                statisticsrecord.ShipperName = a.Shipper;
                statisticsrecord.ConsigneeName = a.Consignee;
                statisticsrecord.Departure = a.FromPortName;
                statisticsrecord.Prefix = a.MainCarriageCarrierPrefix;
                statisticsrecord.PortOfOriginCode = a.MainCarriageFromPortCode;
                statisticsrecord.MainCarriageETA = a.MainCarriageETA;
                statisticsrecord.GrossWeight = a.GrossWeight;
                statisticsrecord.ShipmentStatus = a.StatusName;

                if (a.ChargeableWeight != null && a.ChargeableWeight != 0)
                {
                    statisticsrecord.WeightUnit = a.ChargeableWeightUnitCode;
                }

                if (a.AsAgreedFreight)
                {
                    statisticsrecord.FreightChargeString = "As Agreed";
                    statisticsrecord.FreightChargeAmount = null;
                    statisticsrecord.FreightChargeCurrency = null;
                }
                else
                {
                    statisticsrecord.FreightChargeAmount = a.AWBChargeAmount;

                    if (a.AWBChargeAmount != null && a.AWBChargeAmount != 0)
                    {
                        statisticsrecord.FreightChargeCurrency = a.AWBCurrencyCode;
                    }
                }

                if (a.MainCarriageATD != null)
                {
                    statisticsrecord.DepartureDate = a.MainCarriageATD;
                }
                else if (a.MainCarriageETD != null)
                {
                    statisticsrecord.DepartureDate = a.MainCarriageETD;
                }
                else
                {
                    statisticsrecord.DepartureDate = a.CreateDateTime;
                }

                if (a.Consignee != null)
                {
                    statisticsrecord.ShipperOrConsignee = a.Consignee;
                }
                else if (a.Shipper != null)
                {
                    statisticsrecord.ShipperOrConsignee = a.Shipper;
                }

                if (statisticsrecord.AirlineName == null)
                {
                    statisticsrecord.AirlineName = "No Carrier Specified";
                }

                dataProvider.RecordList.Add(statisticsrecord);
            }

            List<IATAStatisticsDataProvider.IATAStatisticsGroup> finalResults = (from p in dataProvider.RecordList
                                                                                 group p by new { p.AirlineId, p.AirlineName } into g
                                                                                 select new IATAStatisticsDataProvider.IATAStatisticsGroup()
                                                                                 {
                                                                                     AirlineId = g.Key.AirlineId,
                                                                                     AirlineName = g.Key.AirlineName,
                                                                                     InsideGroupList = g.ToList(),
                                                                                 }).ToList();

            List<AmountsClass> allReportAmounts = new List<AmountsClass>();
            foreach (IATAStatisticsDataProvider.IATAStatisticsGroup item in finalResults)
            {
                List<AmountsClass> allAmounts = new List<AmountsClass>();

                foreach (IATAStatisticsDataProvider.IATAStatisticsRecord record in item.InsideGroupList)
                {
                    AmountsClass myAmountsClass = allAmounts.Where(d => d.Currency == record.FreightChargeCurrency).FirstOrDefault();

                    if (myAmountsClass == null)
                    {
                        myAmountsClass = new AmountsClass()
                        {
                            Amount = record.FreightChargeAmount == null ? 0 : record.FreightChargeAmount.Value,
                            Currency = record.FreightChargeCurrency,
                        };

                        allAmounts.Add(myAmountsClass);
                        allReportAmounts.Add(myAmountsClass);
                    }

                    else
                    {
                        myAmountsClass.Amount = myAmountsClass.Amount + (record.FreightChargeAmount == null ? 0 : record.FreightChargeAmount.Value);
                    }
                }

                string str = "";
                foreach (AmountsClass amount in allAmounts)
                {
                    if (amount.Amount != 0)
                    {
                        str = str + amount.Amount + " " + amount.Currency + Environment.NewLine;
                    }
                }

                item.TotalFreight = str;
            }

            List<AmountsClass> newList = (from p in allReportAmounts
                                          group p by new { p.Currency } into g
                                          select new AmountsClass()
                                          {
                                              Amount = g.Sum(s => s.Amount),
                                              Currency = g.Key.Currency,
                                          }).ToList();

            foreach (AmountsClass item in newList)
            {
                if (item.Amount != 0)
                {
                    dataProvider.TotalFreightCharges = dataProvider.TotalFreightCharges + item.Amount + " " + item.Currency + Environment.NewLine;
                }
            }

            dataProvider.GroupList = finalResults.OrderBy(d => d.AirlineName).ToList();
            dataProvider.TotalChargeableWeight = dataProvider.RecordList.Sum(s => s.ChargeableWeight);
            dataProvider.Name = @"IATA Statistics";

            return dataProvider;
        }
        #endregion

        #region AP Invoice Include Vat

        [WebMethod]
        public byte[] LoadAPInvoicesData(byte[] xmlFilters, int tenant)
        {
            InvoiceDataProvider dataprovider = LoadAPInvoicesDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(InvoiceDataProvider), tenant);
        }

        public InvoiceDataProvider LoadAPInvoicesDataProvider(byte[] xmlFilters, int tenant)
        {
            InvoiceDataProvider dataProvider = new InvoiceDataProvider();
            dataProvider.InvoicesReportList = new List<InvoiceDataProvider.InvoicesReport>();
            dataProvider.InvoiceTotalsList = new List<InvoiceDataProvider.InvoiceTotals>();

            APInvoiceTotalVATRepository aPInvoiceToatalVatRepository = new APInvoiceTotalVATRepository(tenant);
            APInvoiceQuery aPInvoiceQuery = new APInvoiceQuery(tenant);
            AddressQuery addressQuery = new AddressQuery(tenant);
            VatTypeRepository vatTypeRepository = new VatTypeRepository(tenant);
            APInvoiceLineRepository apInvoiceLineRepository = new APInvoiceLineRepository(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);

            IQueryable<APInvoiceList> iQueryable = aPInvoiceQuery.GetInvoiceListByTenant(tenant);
            List<VatType> tenantVatTypes = vatTypeRepository.GetVatTypes(tenant).ToList();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            List<Shipment> shipments = this.GetShipmentsByAPInvoicesMainEntityId(iQueryable, tenant);

            #region Report Filters
            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_InvoiceDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "InvoiceDate").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_BranchId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "BranchId").FirstOrDefault();
            QueryFilterItem filterItem_LocalCurrency = queryOperations.QueryFilterItems.Where(d => d.FieldName == "LocalCurrency").FirstOrDefault();
            QueryFilterItem filterItem_IncludeVoidInvoices = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeVoidInvoices").FirstOrDefault();
            QueryFilterItem filterItem_IncludeDraftInvoices = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeDraftInvoices").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime date1 = todayDate.AddDays(-(todayDate.Day - 1)).AddMonths(-1);
            DateTime date2 = date1.AddMonths(2);

            bool invoiceDate = true;
            if (filterItem_InvoiceDate != null)
            {
                if (filterItem_InvoiceDate.FieldValue != null)
                {
                    invoiceDate = (bool)filterItem_InvoiceDate.FieldValue;
                }
            }

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out date1);

                if (filterItem_ToDate != null)
                {
                    DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out date2);
                    //date2 = date2.AddMonths(1);
                }

                else
                {
                    date2 = date1.AddMonths(1);
                }
            }

            string branchId = null;
            if (filterItem_BranchId != null)
            {
                if (filterItem_BranchId.FieldValue != null)
                {
                    branchId = filterItem_BranchId.FieldValue.ToString();
                }
            }

            bool localCurrency = true;
            if (filterItem_LocalCurrency != null)
            {
                if (filterItem_LocalCurrency.FieldValue != null)
                {
                    bool.TryParse(filterItem_LocalCurrency.FieldValue.ToString(), out localCurrency);
                }
            }

            bool includeVoidInvoices = true;
            if (filterItem_IncludeVoidInvoices != null)
            {
                if (filterItem_IncludeVoidInvoices.FieldValue != null)
                {
                    bool.TryParse(filterItem_IncludeVoidInvoices.FieldValue.ToString(), out includeVoidInvoices);

                }
            }

            bool includeDraftInvoices = true;
            if (filterItem_IncludeDraftInvoices != null)
            {
                if (filterItem_IncludeDraftInvoices.FieldValue != null)
                {
                    bool.TryParse(filterItem_IncludeDraftInvoices.FieldValue.ToString(), out includeDraftInvoices);

                }
            }
            #endregion

            #region Filter Data
            if (!includeDraftInvoices)
            {
                iQueryable = iQueryable.Where(d => d.StatusCode != "DR");
            }

            if (!includeVoidInvoices)
            {
                iQueryable = iQueryable.Where(d => d.StatusCode != "VD");
            }

            if (!string.IsNullOrEmpty(branchId))
            {
                iQueryable = iQueryable.Where(d => d.BranchId == branchId);
            }

            if (invoiceDate)
            {
                iQueryable = iQueryable.Where(d => d.InvoiceDate >= date1 && d.InvoiceDate <= date2);
            }

            else
            {
                iQueryable = iQueryable.Where(d => d.CreateDate >= date1 && d.CreateDate <= date2);
            }
            #endregion

            #region Fill General Data
            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);
            AddressPM address = addressQuery.GetSingleAddressPM(currentTenant.AddressId, currentTenant.Id, false);

            if (address != null)
            {
                dataProvider.Address1 = address.Address1;
                dataProvider.Address2 = address.Address2;
                dataProvider.City = address.City;
                dataProvider.Country = address.CountryName;
                dataProvider.TenantFax = address.FaxNumber;
                dataProvider.TenantPhone = address.PhoneNumber;
                dataProvider.State = address.StateEnglishName;
                dataProvider.ZipCode = address.ZipCode;
            }

            dataProvider.TenantName = currentTenant.Company;
            dataProvider.Signature = currentTenant.Signature;
            dataProvider.Logo = DataProviders.General.GetLogo(currentTenant.Id);
            dataProvider.FromPeriod = date1;
            dataProvider.ToPeriod = date2;
            #endregion

            #region Fill Report
            double? totalVat = 0;
            double? totalGrands = 0;
            double? subTotals = 0;
            List<APInvoiceTotalVAT> totalVats = aPInvoiceToatalVatRepository.GetAPInvoiceTotalVats(tenant).ToList();
            int counter = 1;

            foreach (APInvoiceList apInvoice in iQueryable.OrderBy(d => d.InvoiceDate))
            {
                InvoiceDataProvider.InvoicesReport invoicesRecored = new InvoiceDataProvider.InvoicesReport();
                List<APInvoiceTotalVAT> myTotalVats = totalVats.Where(d => d.APInvoiceId == apInvoice.Id).ToList();
                List<VATClass> myVATS = new List<VATClass>();
                IQueryable<APInvoiceLine> expenseInvoiceLines = apInvoiceLineRepository.GetExpenseInvoiceLinesByInvoiceId(apInvoice.Id, tenant);

                Shipment shipment = shipments.Where(d => d.Id == apInvoice.MainEntityId).FirstOrDefault();
                customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, shipment, invoicesRecored);


                foreach (APInvoiceTotalVAT vat in myTotalVats)
                {
                    VATClass item = new VATClass()
                    {
                        Index = counter,
                        Percentage = vat.VatPercent,
                        VATName = tenantVatTypes.Where(d => d.Id == vat.VatTypeId).FirstOrDefault().EnglishName,
                        VATCode = tenantVatTypes.Where(d => d.Id == vat.VatTypeId).FirstOrDefault().Code,
                        InvoiceAmount = vat.InvoiceCurrencyVATAmount,
                        LocalAmount = vat.LocalVATAmount,
                    };

                    myVATS.Add(item);

                    counter++;
                }

                foreach (VATClass item in myVATS)
                {
                    //1
                    if (string.IsNullOrEmpty(dataProvider.VAT1Code))
                    {
                        dataProvider.VAT1Code = item.VATCode;
                        dataProvider.VAT1Header = item.Percentage + "% " + item.VATName;
                        invoicesRecored.VAT1Amount = localCurrency ? item.LocalAmount : item.InvoiceAmount;
                        continue;
                    }

                    else
                    {
                        if (dataProvider.VAT1Code == item.VATCode)
                        {
                            dataProvider.VAT1Code = item.VATCode;
                            dataProvider.VAT1Header = item.Percentage + "% " + item.VATName;
                            invoicesRecored.VAT1Amount = localCurrency ? item.LocalAmount : item.InvoiceAmount;
                            continue;
                        }
                    }

                    //2
                    if (string.IsNullOrEmpty(dataProvider.VAT2Code))
                    {
                        dataProvider.VAT2Code = item.VATCode;
                        dataProvider.VAT2Header = item.Percentage + "% " + item.VATName;
                        invoicesRecored.VAT2Amount = localCurrency ? item.LocalAmount : item.InvoiceAmount;
                        continue;
                    }

                    else
                    {
                        if (dataProvider.VAT2Code == item.VATCode)
                        {
                            dataProvider.VAT2Code = item.VATCode;
                            dataProvider.VAT2Header = item.Percentage + "% " + item.VATName;
                            invoicesRecored.VAT2Amount = localCurrency ? item.LocalAmount : item.InvoiceAmount;
                            continue;
                        }
                    }

                    //3
                    if (string.IsNullOrEmpty(dataProvider.VAT3Code))
                    {
                        dataProvider.VAT3Code = item.VATCode;
                        dataProvider.VAT3Header = item.Percentage + "% " + item.VATName;
                        invoicesRecored.VAT3Amount = localCurrency ? item.LocalAmount : item.InvoiceAmount;
                        continue;
                    }

                    else
                    {
                        if (dataProvider.VAT3Code == item.VATCode)
                        {
                            dataProvider.VAT3Code = item.VATCode;
                            dataProvider.VAT3Header = item.Percentage + "% " + item.VATName;
                            invoicesRecored.VAT3Amount = localCurrency ? item.LocalAmount : item.InvoiceAmount;
                            continue;
                        }
                    }

                    //4
                    if (string.IsNullOrEmpty(dataProvider.VAT4Code))
                    {
                        dataProvider.VAT4Code = item.VATCode;
                        dataProvider.VAT4Header = item.Percentage + "% " + item.VATName;
                        invoicesRecored.VAT4Amount = localCurrency ? item.LocalAmount : item.InvoiceAmount;
                        continue;
                    }

                    else
                    {
                        if (dataProvider.VAT4Code == item.VATCode)
                        {
                            dataProvider.VAT4Code = item.VATCode;
                            dataProvider.VAT4Header = item.Percentage + "% " + item.VATName;
                            invoicesRecored.VAT4Amount = localCurrency ? item.LocalAmount : item.InvoiceAmount;
                            continue;
                        }
                    }
                }

                double? expenseCharges = 0;
                double? expenseChargesInLocalCurrency = 0;

                invoicesRecored.InvoiceType = apInvoice.APInvoiceTypeName;
                invoicesRecored.InvoiceDate = apInvoice.InvoiceDate.Value;
                invoicesRecored.InvoiceNumber = apInvoice.InvoiceNumber;
                invoicesRecored.BillTo = apInvoice.VendorName;
                invoicesRecored.BillToVatNumber = apInvoice.VATNumber;
                invoicesRecored.OurRefNumber = apInvoice.MainEntityReference;
                invoicesRecored.InvoiceStatus = apInvoice.StatusName;
                invoicesRecored.Currency = apInvoice.InvoiceCurrencyCode;
                invoicesRecored.CreateDate = apInvoice.CreateDate;
                invoicesRecored.BillToCode = apInvoice.VendorCode;
                invoicesRecored.AmountDueInInvoiceCurrency = apInvoice.AmountDue;
                invoicesRecored.AmountDueInLocalCurrency = apInvoice.AmountDueInLocalCurrency;
                expenseChargesInLocalCurrency = expenseInvoiceLines.Sum(s => s.LocalCurrencyAmount);
                invoicesRecored.PaidDate = apInvoice.PaidDate;
                invoicesRecored.DueDate = apInvoice.DueDate.Value;

                Card vendorCard = CardRepository.GetSingleCard(apInvoice.VendorId, tenant, false);
                if (vendorCard != null)
                {
                    if (vendorCard.PartnerType != null)
                    {
                        invoicesRecored.BillToType = vendorCard.PartnerType.Name;
                    }
                }

                if (localCurrency)
                {
                    totalVat = totalVat + myTotalVats.Sum(d => d.LocalVATAmount);
                    subTotals = subTotals + apInvoice.SubTotalInLocalCurrency;
                    totalGrands = totalGrands + totalVat + subTotals;
                    invoicesRecored.SubTotallocal = apInvoice.SubTotalInLocalCurrency;
                    invoicesRecored.VATlocal = myTotalVats.Sum(d => d.LocalVATAmount);
                    invoicesRecored.Currency = apInvoice.LocalCurrencyCode;
                    invoicesRecored.GrandTotallocal = invoicesRecored.SubTotallocal + invoicesRecored.VATlocal;
                    expenseCharges = expenseInvoiceLines.Sum(s => s.LocalCurrencyAmount);
                }
                else
                {
                    invoicesRecored.SubTotallocal = apInvoice.SubTotalInInvoiceCurrency;
                    invoicesRecored.VATlocal = myTotalVats.Sum(d => d.InvoiceCurrencyVATAmount);
                    invoicesRecored.GrandTotallocal = invoicesRecored.SubTotallocal + invoicesRecored.VATlocal;
                    invoicesRecored.Currency = apInvoice.InvoiceCurrencyCode;
                    invoicesRecored.vatInLocal = myTotalVats.Sum(d => d.LocalVATAmount);
                    invoicesRecored.subInLocal = apInvoice.SubTotalInLocalCurrency;
                    invoicesRecored.LocalCurrency = apInvoice.LocalCurrencyCode;
                    expenseCharges = expenseInvoiceLines.Sum(s => s.InvoiceCurrencyAmount);
                }

                invoicesRecored.ExpenseCharges = expenseCharges == null ? 0 : expenseCharges;
                invoicesRecored.ExpenseChargesInLocalCurrency = expenseChargesInLocalCurrency == null ? 0 : expenseChargesInLocalCurrency;

                if (shipment != null)
                {
                    ShipmentMasterData shipmentMasterData = shipmentRepository.GetSingleShipmentMasterData(shipment.MasterShipmentDataId, tenant);
                    invoicesRecored.ShipmentMainCarriageETA = shipmentMasterData?.MainCarriageETA;
                }

                dataProvider.InvoicesReportList.Add(invoicesRecored);
            }

            dataProvider.InvoiceTotalsList = (from b in dataProvider.InvoicesReportList
                                              group b by new { b.Currency } into g
                                              select new WebFreight.Web.DataProviders.InvoiceDataProvider.InvoiceTotals()
                                              {
                                                  Currency = g.Key.Currency,
                                                  TotalSubTotals = g.Sum(b => b.SubTotallocal),
                                                  TotalGrands = g.Sum(b => b.GrandTotallocal),
                                                  TotalVats = g.Sum(b => b.VATlocal).Value,
                                                  VAT1Amount = g.Sum(b => b.VAT1Amount),
                                                  VAT2Amount = g.Sum(b => b.VAT2Amount),
                                                  VAT3Amount = g.Sum(b => b.VAT3Amount),
                                                  VAT4Amount = g.Sum(b => b.VAT4Amount),
                                                  totalGrandTotal = g.Sum(b => b.vatInLocal) + g.Sum(b => b.subInLocal),
                                              }).ToList();

            dataProvider.TotalVats_1 = dataProvider.InvoicesReportList.Sum(s => s.VAT1Amount);
            dataProvider.TotalVats_2 = dataProvider.InvoicesReportList.Sum(s => s.VAT2Amount);
            dataProvider.TotalVats_3 = dataProvider.InvoicesReportList.Sum(s => s.VAT3Amount);
            dataProvider.TotalVats_4 = dataProvider.InvoicesReportList.Sum(s => s.VAT4Amount);

            dataProvider.TotalVats = totalVat;
            dataProvider.TotalSub = subTotals;
            dataProvider.TotalsGrands = totalVat + subTotals;
            dataProvider.Name = @"Invoices";


            #endregion

            return dataProvider;
        }

        private List<Shipment> GetShipmentsByAPInvoicesMainEntityId(IQueryable<APInvoiceList> aPInvoices, int tenant)
        {
            List<string> shipmentsIds = aPInvoices.Where(d => d.MainEntityId != null).Select(s => s.MainEntityId).ToList();
            List<Shipment> shipments = new List<Shipment>();
            if (shipmentsIds.Count > 0)
            {
                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                shipments = shipmentRepository.GetShipmentsListFromIdList(shipmentsIds, tenant);
            }
            return shipments;
        }

        #endregion

        #region Statement By Invoice Date
        [WebMethod]
        public byte[] LoadStatementByInvoiceDateData(byte[] xmlFilters, int tenant)
        {
            StatementByInvoiceDateDataProvider dataProviderData = LoadStatementByInvoiceDateDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataProviderData, typeof(StatementByInvoiceDateDataProvider), tenant);
        }

        public StatementByInvoiceDateDataProvider LoadStatementByInvoiceDateDataProvider(byte[] xmlFilters, int tenant)
        {
            StatementByInvoiceDateDataProvider totalData = new StatementByInvoiceDateDataProvider();
            totalData.RecordList = new List<StatementByInvoiceDateDataProvider.StatementByInvoiceRecord>();
            totalData.GroupList = new List<StatementByInvoiceDateDataProvider.StatementByInvoiceGroup>();

            IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);
            CardRepository cardRepository = new CardRepository(tenant);
            UserRepository userRepository = new UserRepository(tenant);
            ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(invoiceContext);
            ARPaymentRepository aRPaymentRepository = new ARPaymentRepository(invoiceContext);
            APPaymentRepository aPPaymentRepository = new APPaymentRepository(invoiceContext);
            APInvoiceRepository aPInvoiceRepository = new APInvoiceRepository(invoiceContext);
            ARPaymentQuery arPaymentQuery = new ARPaymentQuery(aRPaymentRepository);
            ARInvoiceQuery arInvoiceQuery = new ARInvoiceQuery(aRInvoiceRepository);
            APPaymentQuery aPPaymentQuery = new APPaymentQuery(aPPaymentRepository);
            APInvoiceQuery aPInvoiceQuery = new APInvoiceQuery(aPInvoiceRepository);
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);

            IQueryable<ARInvoiceList> iQueryableARInvoice = arInvoiceQuery.GetUnpaidARInvoices(tenant);
            IQueryable<ARPaymentList> iQueryableARPayment = arPaymentQuery.GetOpenedARPayments(tenant);
            IQueryable<APPaymentList> iQueryableAPPayment = aPPaymentQuery.GetOpenedAPPayments(tenant);
            IQueryable<APInvoiceList> iQueryableAPInvoice = aPInvoiceQuery.GetUnpaidAPInvoices(tenant);

            iQueryableARInvoice = iQueryableARInvoice.Where(d => !d.IsConstituentInvoice);
            List<string> shipmentsIds = iQueryableARInvoice.Select(s => s.MainEntityId).Concat(iQueryableAPInvoice.Select(s => s.MainEntityId)).ToList();
            shipmentsIds = shipmentsIds.Distinct().ToList();
            IQueryable<Shipment> shipments = shipmentRepository.GetShipmentsForCrossDock(shipmentsIds, tenant);

            #region Report Filters
            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem customerItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId" && d.Operator == "Equals").FirstOrDefault();
            QueryFilterItem partnerItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "PartnerId" && d.Operator == "Equals").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            string customerId = null;
            string partnerId = null;
            Card customer = null;
            Card partner = null;

            if (customerItem != null && customerItem.FieldValue != null)
            {
                customerId = customerItem.FieldValue.ToString();
            }

            if (partnerItem != null && partnerItem.FieldValue != null)
            {
                partnerId = partnerItem.FieldValue.ToString();
            }

            if (!string.IsNullOrEmpty(customerId))
            {
                customer = CardRepository.GetSingleCard(customerId, tenant, true);

                iQueryableARInvoice = iQueryableARInvoice.Where(d => d.BillToId == customerId);
                iQueryableARPayment = iQueryableARPayment.Where(d => d.BillToId == customerId);
                iQueryableAPPayment = iQueryableAPPayment.Where(d => d.VendorId == customerId);
                iQueryableAPInvoice = iQueryableAPInvoice.Where(d => d.VendorId == customerId);
            }

            if (!string.IsNullOrEmpty(partnerId))
            {
                partner = CardRepository.GetSingleCard(partnerId, tenant, true);

                iQueryableARInvoice = iQueryableARInvoice.Where(d => d.PartnerId == partnerId);
                iQueryableARPayment = iQueryableARPayment.Where(d => d.PartnerId == partnerId);
                iQueryableAPPayment = iQueryableAPPayment.Where(d => d.VendorId == partnerId);
                iQueryableAPInvoice = iQueryableAPInvoice.Where(d => d.VendorId == partnerId);
            }

            totalData.CustomerName = customer != null ? customer.EnglishName : "";
            totalData.PartnerName = partner != null ? partner.EnglishName : "";
            totalData.CurrentDate = todayDate;
            #endregion

            #region General Data
            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);
            if (currentTenant != null)
            {
                AddressQuery addressQuery = new AddressQuery(tenant);
                AddressPM address = addressQuery.GetSingleAddressPM(currentTenant.AddressId, currentTenant.Id, false);
                AddressRepository addressRep = new AddressRepository(currentTenant.Id);
                Address tenantAddress = addressRep.GetSingleAddress(currentTenant.AddressId, currentTenant.Id);
                totalData.GeneralAddress = DataProviders.General.GetAddress(tenantAddress);

                if (address != null)
                {
                    totalData.Address1 = address.Address1;
                    totalData.Address2 = address.Address2;
                    totalData.City = address.City;
                    totalData.Country = address.CountryName;
                    totalData.TenantFax = address.FaxNumber;
                    totalData.TenantPhone = address.PhoneNumber;
                    totalData.State = address.StateEnglishName;
                    totalData.ZipCode = address.ZipCode;
                }

                totalData.TenantName = currentTenant.Company;
                totalData.Signature = currentTenant.Signature;
                totalData.Logo = DataProviders.General.GetLogo(currentTenant.Id);
            }
            #endregion

            #region ARinvoice

            if (iQueryableARInvoice.Count() > 0)
            {
                StatementByInvoiceDateDataProvider.StatementByInvoiceRecord statementRecord = null; ;

                foreach (ARInvoiceList a in iQueryableARInvoice)
                {
                    statementRecord = new StatementByInvoiceDateDataProvider.StatementByInvoiceRecord();

                    Card billTo = cardRepository.GetSingleCard(a.BillToId, tenant);
                    Shipment shipment = shipments.Where(d => d.Id == a.MainEntityId).FirstOrDefault();

                    statementRecord.InvoiceCurrency = a.InvoiceCurrencyCode;
                    statementRecord.ShipmentNumber = a.MainEntityReference;
                    statementRecord.InvoiceNumber = a.InvoiceNumber;
                    statementRecord.SentDate = a.InvoiceDate;
                    statementRecord.Age = Convert.ToInt32((todayDate - a.InvoiceDate.Value).TotalDays);
                    statementRecord.DueAge = Convert.ToInt32((todayDate - a.DueDate.Value).TotalDays);
                    statementRecord.PastTotalAmount = a.AmountDue;

                    if (a.DueDate.Value.Date >= todayDate.Date)
                    {
                        statementRecord.OverDue = "";
                    }
                    else
                    {
                        statementRecord.OverDue = "Yes (" + statementRecord.DueAge + " days)";
                    }

                    if (billTo != null)
                    {
                        statementRecord.Customer = billTo.EnglishName;

                        User salesman = userRepository.GetSingleUser(billTo.SalesmanUserId, tenant);
                        if (salesman != null)
                        {
                            statementRecord.Salesman = salesman.Contact.EnglishName;
                        }
                    }

                    if (a.DueDate >= todayDate)
                    {
                        statementRecord.CurrentAmount = a.AmountDue;
                    }

                    else if (((todayDate - a.DueDate.Value).TotalDays >= 1) && ((todayDate - a.DueDate.Value).TotalDays <= 30))
                    {
                        statementRecord.PastAmount_30 = a.AmountDue;
                    }

                    else if (((todayDate - a.DueDate.Value).TotalDays > 30) && ((todayDate - a.DueDate.Value).TotalDays <= 45))
                    {
                        statementRecord.PastAmount_45 = a.AmountDue;
                    }

                    else if (((todayDate - a.DueDate.Value).TotalDays > 45) && ((todayDate - a.DueDate.Value).TotalDays <= 60))
                    {
                        statementRecord.PastAmount_60 = a.AmountDue;
                    }

                    else if (((todayDate - a.DueDate.Value).TotalDays > 60) && ((todayDate - a.DueDate.Value).TotalDays <= 90))
                    {
                        statementRecord.PastAmount_90 = a.AmountDue;
                    }
                    else if ((todayDate - a.DueDate.Value).TotalDays > 90)
                    {
                        statementRecord.PastAmountOver_90 = a.AmountDue;
                    }
                    //////////////////////////////
                    if (((todayDate - a.DueDate.Value).TotalDays >= 1) && ((todayDate - a.DueDate.Value).TotalDays <= 15))
                    {
                        statementRecord.PastAmount1_15 = a.AmountDue;
                    }
                    else if (((todayDate - a.DueDate.Value).TotalDays >= 16) && ((todayDate - a.DueDate.Value).TotalDays <= 30))
                    {
                        statementRecord.PastAmount16_30 = a.AmountDue;
                    }
                    else if (((todayDate - a.DueDate.Value).TotalDays >= 31) && ((todayDate - a.DueDate.Value).TotalDays <= 60))
                    {
                        statementRecord.PastAmount31_60 = a.AmountDue;
                    }
                    else if (((todayDate - a.DueDate.Value).TotalDays >= 61) && ((todayDate - a.DueDate.Value).TotalDays <= 90))
                    {
                        statementRecord.PastAmount61_90 = a.AmountDue;
                    }
                    else if (((todayDate - a.DueDate.Value).TotalDays >= 91) && ((todayDate - a.DueDate.Value).TotalDays <= 120))
                    {
                        statementRecord.PastAmount91_120 = a.AmountDue;
                    }
                    else if ((todayDate - a.DueDate.Value).TotalDays > 120)
                    {
                        statementRecord.PastAmountOver_120 = a.AmountDue;
                    }

                    if (shipment != null)
                    {
                        statementRecord.ShipperRef1 = shipment.ShipperReference1;
                        statementRecord.ShipperRef2 = shipment.ShipperReference2;
                    }

                    totalData.RecordList.Add(statementRecord);
                }
            }
            #endregion

            #region APinvoice

            if (iQueryableAPInvoice.Count() > 0)
            {
                StatementByInvoiceDateDataProvider.StatementByInvoiceRecord statementRecord = null;

                foreach (APInvoiceList a in iQueryableAPInvoice)
                {
                    statementRecord = new StatementByInvoiceDateDataProvider.StatementByInvoiceRecord();

                    Card vendor = cardRepository.GetSingleCard(a.VendorId, tenant);
                    Shipment shipment = shipments.Where(d => d.Id == a.MainEntityId).FirstOrDefault();

                    statementRecord.InvoiceCurrency = a.InvoiceCurrencyCode;
                    statementRecord.ShipmentNumber = a.MainEntityReference;
                    statementRecord.InvoiceNumber = a.InvoiceNumber;
                    statementRecord.SentDate = a.InvoiceDate;
                    statementRecord.Age = Convert.ToInt32((todayDate - a.InvoiceDate.Value).TotalDays);
                    statementRecord.DueAge = Convert.ToInt32((todayDate - a.DueDate.Value).TotalDays);
                    statementRecord.PastTotalAmount = a.AmountDue * -1;

                    if (a.DueDate.Value.Date >= todayDate.Date)
                    {
                        statementRecord.OverDue = "";
                    }
                    else
                    {
                        statementRecord.OverDue = "Yes (" + statementRecord.DueAge + " days)";
                    }

                    if (vendor != null)
                    {
                        statementRecord.Customer = vendor.EnglishName;

                        User salesman = userRepository.GetSingleUser(vendor.SalesmanUserId, tenant);
                        if (salesman != null)
                        {
                            statementRecord.Salesman = salesman.Contact.EnglishName;
                        }
                    }

                    if (a.DueDate >= todayDate)
                    {
                        statementRecord.CurrentAmount = a.AmountDue * -1;
                    }

                    else if (((todayDate - a.DueDate.Value).TotalDays >= 1) && ((todayDate - a.DueDate.Value).TotalDays <= 30))
                    {
                        statementRecord.PastAmount_30 = a.AmountDue * -1;
                    }

                    else if (((todayDate - a.DueDate.Value).TotalDays > 30) && ((todayDate - a.DueDate.Value).TotalDays <= 45))
                    {
                        statementRecord.PastAmount_45 = a.AmountDue * -1;
                    }

                    else if (((todayDate - a.DueDate.Value).TotalDays > 45) && ((todayDate - a.DueDate.Value).TotalDays <= 60))
                    {
                        statementRecord.PastAmount_60 = a.AmountDue * -1;
                    }

                    else if (((todayDate - a.DueDate.Value).TotalDays > 60) && ((todayDate - a.DueDate.Value).TotalDays <= 90))
                    {
                        statementRecord.PastAmount_90 = a.AmountDue * -1;
                    }

                    else if ((todayDate - a.DueDate.Value).TotalDays > 90)
                    {
                        statementRecord.PastAmountOver_90 = a.AmountDue * -1;
                    }

                    //////////////////////////////////////////////
                    if (((todayDate - a.DueDate.Value).TotalDays >= 1) && ((todayDate - a.DueDate.Value).TotalDays <= 15))
                    {
                        statementRecord.PastAmount1_15 = a.AmountDue * -1;
                    }
                    else if (((todayDate - a.DueDate.Value).TotalDays >= 16) && ((todayDate - a.DueDate.Value).TotalDays <= 30))
                    {
                        statementRecord.PastAmount16_30 = a.AmountDue * -1;
                    }
                    else if (((todayDate - a.DueDate.Value).TotalDays >= 31) && ((todayDate - a.DueDate.Value).TotalDays <= 60))
                    {
                        statementRecord.PastAmount31_60 = a.AmountDue * -1;
                    }
                    else if (((todayDate - a.DueDate.Value).TotalDays >= 61) && ((todayDate - a.DueDate.Value).TotalDays <= 90))
                    {
                        statementRecord.PastAmount61_90 = a.AmountDue * -1;
                    }
                    else if (((todayDate - a.DueDate.Value).TotalDays >= 91) && ((todayDate - a.DueDate.Value).TotalDays <= 120))
                    {
                        statementRecord.PastAmount91_120 = a.AmountDue * -1;
                    }
                    else if ((todayDate - a.DueDate.Value).TotalDays > 120)
                    {
                        statementRecord.PastAmountOver_120 = a.AmountDue * -1;
                    }

                    if (shipment != null)
                    {
                        statementRecord.ShipperRef1 = shipment.ShipperReference1;
                        statementRecord.ShipperRef2 = shipment.ShipperReference2;
                    }

                    totalData.RecordList.Add(statementRecord);
                }
            }
            #endregion

            #region ARPayment

            if (iQueryableARPayment.Count() > 0)
            {
                StatementByInvoiceDateDataProvider.StatementByInvoiceRecord statementRecord = null; ;

                foreach (ARPaymentList a in iQueryableARPayment)
                {
                    statementRecord = new StatementByInvoiceDateDataProvider.StatementByInvoiceRecord();

                    Card billTo = cardRepository.GetSingleCard(a.BillToId, tenant);

                    statementRecord.InvoiceCurrency = a.PaymentCurrencyCode;
                    statementRecord.ShipmentNumber = ""; // ????
                    statementRecord.InvoiceNumber = a.PaymentNo;
                    statementRecord.SentDate = a.RegisterDate;
                    statementRecord.Age = Convert.ToInt32((todayDate - a.RegisterDate.Value).TotalDays);
                    statementRecord.DueAge = Convert.ToInt32((todayDate - a.ValueDate.Value).TotalDays);
                    statementRecord.PastTotalAmount = a.OpenAmount * -1;

                    if (a.ValueDate.Value.Date >= todayDate.Date)
                    {
                        statementRecord.OverDue = "";
                    }
                    else
                    {
                        statementRecord.OverDue = "Yes (" + statementRecord.DueAge + " days)";
                    }

                    if (billTo != null)
                    {
                        statementRecord.Customer = billTo.EnglishName;

                        User salesman = userRepository.GetSingleUser(billTo.SalesmanUserId, tenant);
                        if (salesman != null)
                        {
                            statementRecord.Salesman = salesman.Contact.EnglishName;
                        }
                    }

                    if (a.ValueDate >= todayDate)
                    {
                        statementRecord.CurrentAmount = a.OpenAmount * -1;
                    }

                    else if (((todayDate - a.ValueDate.Value).TotalDays >= 1) && ((todayDate - a.ValueDate.Value).TotalDays <= 30))
                    {
                        statementRecord.PastAmount_30 = a.OpenAmount * -1;
                    }

                    else if (((todayDate - a.ValueDate.Value).TotalDays > 30) && ((todayDate - a.ValueDate.Value).TotalDays <= 45))
                    {
                        statementRecord.PastAmount_45 = a.OpenAmount * -1;
                    }

                    else if (((todayDate - a.ValueDate.Value).TotalDays > 45) && ((todayDate - a.ValueDate.Value).TotalDays <= 60))
                    {
                        statementRecord.PastAmount_60 = a.OpenAmount * -1;
                    }

                    else if (((todayDate - a.ValueDate.Value).TotalDays > 60) && ((todayDate - a.ValueDate.Value).TotalDays <= 90))
                    {
                        statementRecord.PastAmount_90 = a.OpenAmount * -1;
                    }

                    else if ((todayDate - a.ValueDate.Value).TotalDays > 90)
                    {
                        statementRecord.PastAmountOver_90 = a.OpenAmount * -1;
                    }




                    //////////////////////////////////////////////
                    if (((todayDate - a.ValueDate.Value).TotalDays >= 1) && ((todayDate - a.ValueDate.Value).TotalDays <= 15))
                    {
                        statementRecord.PastAmount1_15 = a.OpenAmount * -1;
                    }
                    else if (((todayDate - a.ValueDate.Value).TotalDays >= 16) && ((todayDate - a.ValueDate.Value).TotalDays <= 30))
                    {
                        statementRecord.PastAmount16_30 = a.OpenAmount * -1;
                    }
                    else if (((todayDate - a.ValueDate.Value).TotalDays >= 31) && ((todayDate - a.ValueDate.Value).TotalDays <= 60))
                    {
                        statementRecord.PastAmount31_60 = a.OpenAmount * -1;
                    }
                    else if (((todayDate - a.ValueDate.Value).TotalDays >= 61) && ((todayDate - a.ValueDate.Value).TotalDays <= 90))
                    {
                        statementRecord.PastAmount61_90 = a.OpenAmount * -1;
                    }
                    else if (((todayDate - a.ValueDate.Value).TotalDays >= 91) && ((todayDate - a.ValueDate.Value).TotalDays <= 120))
                    {
                        statementRecord.PastAmount91_120 = a.OpenAmount * -1;
                    }
                    else if ((todayDate - a.ValueDate.Value).TotalDays > 120)
                    {
                        statementRecord.PastAmountOver_120 = a.OpenAmount * -1;
                    }

                    /////////////////////////////////////////////
                    totalData.RecordList.Add(statementRecord);
                }
            }
            #endregion

            #region APPayment

            if (iQueryableAPPayment.Count() > 0)
            {
                StatementByInvoiceDateDataProvider.StatementByInvoiceRecord statementRecord = null;

                foreach (APPaymentList a in iQueryableAPPayment)
                {
                    statementRecord = new StatementByInvoiceDateDataProvider.StatementByInvoiceRecord();

                    Card vendor = cardRepository.GetSingleCard(a.VendorId, tenant);

                    statementRecord.InvoiceCurrency = a.PaymentCurrencyCode;
                    statementRecord.ShipmentNumber = ""; // ????
                    statementRecord.InvoiceNumber = a.PaymentNo;
                    statementRecord.SentDate = a.RegisterDate;
                    statementRecord.Age = Convert.ToInt32((todayDate - a.RegisterDate.Value).TotalDays);
                    statementRecord.DueAge = Convert.ToInt32((todayDate - a.ValueDate.Value).TotalDays);
                    statementRecord.PastTotalAmount = a.OpenAmount;

                    if (a.ValueDate.Value.Date >= todayDate.Date)
                    {
                        statementRecord.OverDue = "";
                    }
                    else
                    {
                        statementRecord.OverDue = "Yes (" + statementRecord.DueAge + " days)";
                    }

                    if (vendor != null)
                    {
                        statementRecord.Customer = vendor.EnglishName;

                        User salesman = userRepository.GetSingleUser(vendor.SalesmanUserId, tenant);
                        if (salesman != null)
                        {
                            statementRecord.Salesman = salesman.Contact.EnglishName;
                        }
                    }

                    if (a.ValueDate >= todayDate)
                    {
                        statementRecord.CurrentAmount = a.OpenAmount;
                    }

                    else if (((todayDate - a.ValueDate.Value).TotalDays >= 1) && ((todayDate - a.ValueDate.Value).TotalDays <= 30))
                    {
                        statementRecord.PastAmount_30 = a.OpenAmount;
                    }

                    else if (((todayDate - a.ValueDate.Value).TotalDays > 30) && ((todayDate - a.ValueDate.Value).TotalDays <= 45))
                    {
                        statementRecord.PastAmount_45 = a.OpenAmount;
                    }

                    else if (((todayDate - a.ValueDate.Value).TotalDays > 45) && ((todayDate - a.ValueDate.Value).TotalDays <= 60))
                    {
                        statementRecord.PastAmount_60 = a.OpenAmount;
                    }

                    else if (((todayDate - a.ValueDate.Value).TotalDays > 60) && ((todayDate - a.ValueDate.Value).TotalDays <= 90))
                    {
                        statementRecord.PastAmount_90 = a.OpenAmount;
                    }

                    else if ((todayDate - a.ValueDate.Value).TotalDays > 90)
                    {
                        statementRecord.PastAmountOver_90 = a.OpenAmount;
                    }
                    //////////////////////////////////////////////
                    if (((todayDate - a.ValueDate.Value).TotalDays >= 1) && ((todayDate - a.ValueDate.Value).TotalDays <= 15))
                    {
                        statementRecord.PastAmount1_15 = a.OpenAmount;
                    }
                    else if (((todayDate - a.ValueDate.Value).TotalDays >= 16) && ((todayDate - a.ValueDate.Value).TotalDays <= 30))
                    {
                        statementRecord.PastAmount16_30 = a.OpenAmount;
                    }
                    else if (((todayDate - a.ValueDate.Value).TotalDays >= 31) && ((todayDate - a.ValueDate.Value).TotalDays <= 60))
                    {
                        statementRecord.PastAmount31_60 = a.OpenAmount;
                    }
                    else if (((todayDate - a.ValueDate.Value).TotalDays >= 61) && ((todayDate - a.ValueDate.Value).TotalDays <= 90))
                    {
                        statementRecord.PastAmount61_90 = a.OpenAmount;
                    }
                    else if (((todayDate - a.ValueDate.Value).TotalDays >= 91) && ((todayDate - a.ValueDate.Value).TotalDays <= 120))
                    {
                        statementRecord.PastAmount91_120 = a.OpenAmount;
                    }
                    else if ((todayDate - a.ValueDate.Value).TotalDays > 120)
                    {
                        statementRecord.PastAmountOver_120 = a.OpenAmount;
                    }

                    /////////////////////////////////////////////
                    totalData.RecordList.Add(statementRecord);
                }
            }
            #endregion

            totalData.RecordList = totalData.RecordList.OrderBy(or => or.InvoiceCurrency).ToList();

            List<StatementByInvoiceDateDataProvider.StatementByInvoiceGroup> finalResults = (from p in totalData.RecordList
                                                                                             group p by p.InvoiceCurrency into g
                                                                                             select new StatementByInvoiceDateDataProvider.StatementByInvoiceGroup()
                                                                                             {
                                                                                                 InvoiceCurrency = g.Key,
                                                                                                 StatementRecordList = g.ToList(),
                                                                                             }).ToList();

            totalData.GroupList = finalResults.OrderBy(d => d.InvoiceCurrency).ToList();

            foreach (StatementByInvoiceDateDataProvider.StatementByInvoiceGroup item in totalData.GroupList)
            {
                item.TotalCurrentAmount = item.StatementRecordList.Sum(d => d.CurrentAmount);
                item.TotalPastAmount_30 = item.StatementRecordList.Sum(d => d.PastAmount_30);
                item.TotalPastAmount_45 = item.StatementRecordList.Sum(d => d.PastAmount_45);
                item.TotalPastAmount_60 = item.StatementRecordList.Sum(d => d.PastAmount_60);
                item.TotalPastAmount_90 = item.StatementRecordList.Sum(d => d.PastAmount_90);
                item.TotalPastAmountOver_90 = item.StatementRecordList.Sum(d => d.PastAmountOver_90);
                ////////////////////////////
                item.TotalPastAmount1_15 = item.StatementRecordList.Sum(d => d.PastAmount1_15);
                item.TotalPastAmount16_30 = item.StatementRecordList.Sum(d => d.PastAmount16_30);
                item.TotalPastAmount31_60 = item.StatementRecordList.Sum(d => d.PastAmount31_60);
                item.TotalPastAmount61_90 = item.StatementRecordList.Sum(d => d.PastAmount61_90);
                item.TotalPastAmount91_120 = item.StatementRecordList.Sum(d => d.PastAmount91_120);
                item.TotalPastAmountOver_120 = item.StatementRecordList.Sum(d => d.PastAmountOver_120);
                /////////////////////////////
                item.TotalAmount = item.StatementRecordList.Sum(d => d.PastTotalAmount);
            }

            return totalData;
        }
        #endregion

        #region Opportunity Stage Changing
        [WebMethod]
        public byte[] LoadOpportunityStageChangingData(byte[] xmlFilters, int tenant)
        {
            OpportunityStageChangingDataProvider dataProvider = this.LoadOpportunityStageChangingDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataProvider, typeof(OpportunityStageChangingDataProvider), tenant);
        }

        private OpportunityStageChangingDataProvider LoadOpportunityStageChangingDataProvider(byte[] xmlFilters, int tenant)
        {
            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_IsByStageDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IsByStageDate").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_OwnerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "OwnerId").FirstOrDefault();
            QueryFilterItem filterItem_LeadSources = queryOperations.QueryFilterItems.Where(d => d.FieldName == "LeadSources").FirstOrDefault();
            QueryFilterItem filterItem_OpportunityTypeId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "OpportunityTypeId").FirstOrDefault();

            bool isByStageDate = true;
            if (filterItem_IsByStageDate != null)
            {
                if (filterItem_IsByStageDate.FieldValue != null)
                {
                    isByStageDate = (bool)filterItem_IsByStageDate.FieldValue;
                }
            }

            string ownerId = null;
            if (filterItem_OwnerId != null)
            {
                if (filterItem_OwnerId.FieldValue != null)
                {
                    ownerId = filterItem_OwnerId.FieldValue.ToString();
                }
            }

            string leadSources = null;
            if (filterItem_LeadSources != null)
            {
                if (filterItem_LeadSources.FieldValue != null)
                {
                    leadSources = filterItem_LeadSources.FieldValue.ToString();
                }
            }

            string opportunityTypeId = null;
            if (filterItem_OpportunityTypeId != null)
            {
                if (filterItem_OpportunityTypeId.FieldValue != null)
                {
                    opportunityTypeId = filterItem_OpportunityTypeId.FieldValue.ToString();
                }
            }

            List<string> myLeadSourcesList = new List<string>();
            if (!string.IsNullOrEmpty(leadSources))
            {
                leadSources = leadSources.Replace(" ", "");
                leadSources = leadSources.Trim(',');
                string[] myLeadSources = leadSources.Split(',');
                myLeadSourcesList = myLeadSources.ToList();
            }

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);

            DateTime date1 = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            DateTime date2 = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out date1);
            }

            if (filterItem_ToDate != null)
            {
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out date2);
            }

            OpportunityStageChangingDataProvider dataProvider = new OpportunityStageChangingDataProvider();
            dataProvider.RecordsList = new List<OpportunityStageChangingDataProvider.StageChangingRecord>();

            ICRMContext context = CRMContext.GetContext(tenant);
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            OpportunityStageListQueryService oppStageQuery = new OpportunityStageListQueryService(context);

            IQueryable<OpportunityStageList> iQueryable = oppStageQuery.GetOpportunityStagesByTenant(tenant);
            iQueryable = iQueryable.Where(d => d.IsCancelled == false);

            if (!string.IsNullOrEmpty(ownerId))
            {
                iQueryable = iQueryable.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(opportunityTypeId))
            {
                iQueryable = iQueryable.Where(d => d.OpportunityTypeId == opportunityTypeId);
            }

            if (date1 != null)
            {
                if (isByStageDate)
                {
                    iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.EndDate) >= System.Data.Entity.DbFunctions.TruncateTime(date1));
                }

                else
                {
                    iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= System.Data.Entity.DbFunctions.TruncateTime(date1));
                }
            }

            if (date2 != null)
            {
                if (isByStageDate)
                {
                    iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.EndDate) <= System.Data.Entity.DbFunctions.TruncateTime(date2));
                }

                else
                {
                    iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= System.Data.Entity.DbFunctions.TruncateTime(date2));
                }
            }

            if (myLeadSourcesList.Count() > 0)
            {
                iQueryable = iQueryable.Where(d => myLeadSourcesList.Contains(d.LeadSourceId));
            }

            var customerIds = iQueryable.Select(y => y.CustomerId).Distinct().ToList();
            var customers = commonDataContext.Customers.Where(x => customerIds.Contains(x.Id)).ToList();

            foreach (OpportunityStageList item in iQueryable.OrderByDescending(d => d.LastModifiedDate))
            {
                OpportunityStageChangingDataProvider.StageChangingRecord record = new OpportunityStageChangingDataProvider.StageChangingRecord();
                TimeSpan? duration = item.EndDate - item.StartDate;

                record.OpportunityName = !string.IsNullOrEmpty(item.OpportunityTopic) ? item.OpportunityTopic : "";
                record.Country = !string.IsNullOrEmpty(item.CountryName) ? item.CountryName : "";
                record.FromStage = !string.IsNullOrEmpty(item.FromStageName) ? item.FromStageName : "";
                record.ToStage = !string.IsNullOrEmpty(item.ToStageName) ? item.ToStageName : "";
                record.LastModifiedDate = item.LastModifiedDate;

                int days = Convert.ToInt32(duration.Value.TotalDays);
                if (days < 1)
                {
                    record.StageDuration = "0 days";
                }
                else
                {
                    record.StageDuration = days + " days";
                }

                record.CreateDate = item.CreateDate;
                record.Customer = item.Customer;
                record.OpportunityType = item.OpportunityType;
                record.Salesman = item.OwnerName;
                record.StartDate = item.StartDate;
                record.EndDate = item.EndDate;

                var customer = customers.FirstOrDefault(x => x.Id == item.CustomerId);
                if (customer != null)
                {
                    record.CustomerPrimaryContactName = customer.PrimaryContactName;
                    record.CustomerPrimaryContactEmail = customer.PrimaryContactEmail;
                }

                dataProvider.RecordsList.Add(record);
            }

            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);
            AddressQuery addressQuery = new AddressQuery(tenant);
            AddressPM address = addressQuery.GetSingleAddressPM(currentTenant.AddressId, currentTenant.Id, false);

            if (address != null)
            {

                dataProvider.Address1 = address.Address1;
                dataProvider.Address2 = address.Address2;
                dataProvider.City = address.City;
                dataProvider.Country = address.CountryName;
                dataProvider.TenantFax = address.FaxNumber;
                dataProvider.TenantPhone = address.PhoneNumber;
                dataProvider.State = address.StateEnglishName;
                dataProvider.ZipCode = address.ZipCode;
            }

            dataProvider.TenantName = currentTenant.Company;
            dataProvider.Signature = currentTenant.Signature;
            dataProvider.Logo = DataProviders.General.GetLogo(currentTenant.Id);

            return dataProvider;
        }

        #endregion

        #region Monthly Conversion Report
        [WebMethod]
        public byte[] LoadOpportunityMonthlyConversionData(byte[] xmlFilters, int tenant)
        {
            OpportunityMonthlyConversionDataProvider dataProvider = this.LoadOpportunityMonthlyConversionDataProvider(xmlFilters, tenant);
            XmlSerializer serializer = new XmlSerializer(typeof(OpportunityMonthlyConversionDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, dataProvider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        private OpportunityMonthlyConversionDataProvider LoadOpportunityMonthlyConversionDataProvider(byte[] xmlFilters, int tenant)
        {
            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_IsByCreateDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IsByCreateDate").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_DataType = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DataType").FirstOrDefault();
            QueryFilterItem filterItem_CountryId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CountryId").FirstOrDefault();
            QueryFilterItem filterItem_OwnerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "OwnerId").FirstOrDefault();
            QueryFilterItem filterItem_BusinessUnitId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "BusinessUnitId").FirstOrDefault();
            QueryFilterItem filterItem_LeadSources = queryOperations.QueryFilterItems.Where(d => d.FieldName == "LeadSources").FirstOrDefault();
            QueryFilterItem filterItem_Reseller = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ResellerId").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime date1 = todayDate;
            DateTime date2 = todayDate;

            bool isByCreateDate = true;
            if (filterItem_IsByCreateDate != null)
            {
                if (filterItem_IsByCreateDate.FieldValue != null)
                {
                    isByCreateDate = (bool)filterItem_IsByCreateDate.FieldValue;
                }
            }

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out date1);

                if (filterItem_ToDate != null)
                {
                    DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out date2);
                }

            }

            string opportunityTypeCode = null;
            if (filterItem_DataType != null)
            {
                if (filterItem_DataType.FieldValue != null)
                {
                    opportunityTypeCode = filterItem_DataType.FieldValue.ToString();
                }
            }

            string countryId = null;
            if (filterItem_CountryId != null)
            {
                if (filterItem_CountryId.FieldValue != null)
                {
                    countryId = filterItem_CountryId.FieldValue.ToString();
                }
            }

            string ownerId = null;
            if (filterItem_OwnerId != null)
            {
                if (filterItem_OwnerId.FieldValue != null)
                {
                    ownerId = filterItem_OwnerId.FieldValue.ToString();
                }
            }

            string businessUnitId = null;
            if (filterItem_BusinessUnitId != null)
            {
                if (filterItem_BusinessUnitId.FieldValue != null)
                {
                    businessUnitId = filterItem_BusinessUnitId.FieldValue.ToString();
                }
            }

            string leadSources = null;
            if (filterItem_LeadSources != null)
            {
                if (filterItem_LeadSources.FieldValue != null)
                {
                    leadSources = filterItem_LeadSources.FieldValue.ToString();
                }
            }

            List<string> myLeadSourcesList = new List<string>();
            if (!string.IsNullOrEmpty(leadSources))
            {
                leadSources = leadSources.Replace(" ", "");

                if (leadSources.ToLower() == "all")
                {
                    //LeadSourceRepository leadSourceRepository = new LeadSourceRepository(tenant);
                    //IQueryable<LeadSource> iQueryable = leadSourceRepository.GetLeadSources(tenant).Where(d => !d.InActive);
                    //if (iQueryable.Count() > 0)
                    //{
                    //    myLeadSourcesList = iQueryable.Select(s => s.Id).ToList();
                    //}
                }

                else
                {
                    leadSources = leadSources.Trim(',');
                    string[] myLeadSources = leadSources.Split(',');
                    myLeadSourcesList = myLeadSources.ToList();
                }
            }

            string resellerId = null;
            if (filterItem_Reseller != null)
            {
                if (filterItem_Reseller.FieldValue != null)
                {
                    resellerId = filterItem_Reseller.FieldValue.ToString();
                }
            }

            OpportunityMonthlyConversionDataProvider myDataProvider = null;

            if (isByCreateDate)
            {
                myDataProvider = GetMonthlyConversionByCreateDate(tenant, date1, date2, opportunityTypeCode, countryId, ownerId, businessUnitId, myLeadSourcesList, resellerId);
            }

            else
            {
                myDataProvider = GetMonthlyConversionByStageDate(tenant, date1, date2, opportunityTypeCode, countryId, ownerId, businessUnitId, myLeadSourcesList, resellerId);
            }

            return myDataProvider;
        }

        private OpportunityMonthlyConversionDataProvider GetMonthlyConversionByCreateDate(int tenant, DateTime date1, DateTime date2, string opportunityTypeCode, string countryId, string ownerId, string businessUnitId, List<string> myLeadSourcesList, string resellerId)
        {
            OpportunityMonthlyConversionDataProvider myDataProvider = new OpportunityMonthlyConversionDataProvider();
            myDataProvider.MonthlyDataList = new List<MonthItemClass>();

            #region Get Base Data
            ICRMContext myCRMCotnext = CRMContext.GetContext(tenant);
            StageRepository stageRepository = new StageRepository(myCRMCotnext);
            OpportunityRepository opportunityRepository = new OpportunityRepository(myCRMCotnext);
            OpportunityStageRepository opportunityStageRepository = new OpportunityStageRepository(myCRMCotnext);

            IQueryable<OpportunityStage> iQueryable_OpportunityStages = opportunityStageRepository.GetAll(tenant);

            IQueryable<Opportunity> iQueryable_Opportunities =
                (from a in opportunityRepository.GetAllForReport(tenant)
                 where a.IsCancelled == false
                 && System.Data.Entity.DbFunctions.TruncateTime(a.CreateDate) >= date1
                 && System.Data.Entity.DbFunctions.TruncateTime(a.CreateDate) < date2
                 select a);

            if (!string.IsNullOrEmpty(opportunityTypeCode))
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => d.OpportunityTypeId == opportunityTypeCode);
            }

            if (!string.IsNullOrEmpty(countryId))
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => d.Customer.CountryId == countryId);
            }

            if (!string.IsNullOrEmpty(ownerId))
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(resellerId))
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => d.Customer.Customer != null && d.Customer.Customer.Field2 == resellerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => d.BusinessUnitId == businessUnitId);
            }

            if (myLeadSourcesList.Count() > 0)
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => myLeadSourcesList.Contains(d.LeadSourceId));
            }

            IQueryable<Stage> allStages = stageRepository.GetAll(tenant).Where(d => !d.InActive);
            IQueryable<Stage> allStages_WithProbability = allStages.Where(d => d.Probability != 0);
            IQueryable<Stage> allStages_ZeroProbability = allStages.Where(d => d.Probability == 0);

            //Qualification Stage
            string myQuaStageId = null;
            string myQuaStageName = null;
            Stage myQuaStage = allStages_WithProbability.Where(d => d.Code == "QUA").FirstOrDefault();
            if (myQuaStage != null)
            {
                myQuaStageId = myQuaStage.Id;
                myQuaStageName = myQuaStage.Name;
                allStages_WithProbability = allStages_WithProbability.Where(d => d.Id != myQuaStageId);
            }

            // Closed Won Stage
            string myCloseWonStageId = null;
            string myCloseWonStageName = null;
            Stage myCloseWonStage = allStages_WithProbability.Where(d => d.Code == "CWN").FirstOrDefault();
            if (myCloseWonStage != null)
            {
                myCloseWonStageId = myCloseWonStage.Id;
                myCloseWonStageName = myCloseWonStage.Name;
                allStages_WithProbability = allStages_WithProbability.Where(d => d.Id != myCloseWonStageId);
            }

            // Closed Lost Stage
            string myCloseLostStageId = null;
            string myCloseLostStageName = null;
            Stage myCloseLostStage = allStages_ZeroProbability.Where(d => d.Code == "CLS").FirstOrDefault();
            if (myCloseLostStage != null)
            {
                myCloseLostStageId = myCloseLostStage.Id;
                myCloseLostStageName = myCloseLostStage.Name;
                allStages_ZeroProbability = allStages_ZeroProbability.Where(d => d.Id != myCloseLostStageId);
            }

            // First Stage
            string myFirstStageId = null;
            string myFirstStageName = null;
            allStages_WithProbability = allStages_WithProbability.OrderBy(o => o.Probability);
            Stage myFirstStage = allStages_WithProbability.FirstOrDefault();
            if (myFirstStage != null)
            {
                myFirstStageId = myFirstStage.Id;
                myFirstStageName = myFirstStage.Name;
                allStages_WithProbability = allStages_WithProbability.Where(d => d.Id != myFirstStageId);
            }
            #endregion

            int index = 0;

            DateTime myDate1 = date1;
            int myMonthItemIndex = 0;
            int numberOfMonths = 0;

            while (myDate1 < date2)
            {
                numberOfMonths++;

                #region
                index = 0;

                IQueryable<Opportunity> iQueryable_Monthly =
                    (from f in iQueryable_Opportunities
                     where f.CreateDate.Value.Year == myDate1.Year
                     && f.CreateDate.Value.Month == myDate1.Month
                     select f);

                #region First Stage
                int myLeadCount = iQueryable_Monthly.Count();

                myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = myDate1.Month + "." + myDate1.Year,
                    StageName = myFirstStageName,
                    OpportunitiesCount = myLeadCount,
                    RowIndex = index++,
                });
                #endregion

                #region QUA Stage
                int myQuasCount = (from a in iQueryable_Monthly
                                   join b in iQueryable_OpportunityStages
                                   on a.Id equals b.OpportunityId
                                   where b.FromStageId == myQuaStageId
                                   select a).Count();

                myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = myDate1.Month + "." + myDate1.Year,
                    StageName = myQuaStageName,
                    OpportunitiesCount = myQuasCount,
                    RowIndex = index++,
                    Percentage = myLeadCount == 0 ? 0 : (myQuasCount * 100 / myLeadCount),
                    PercentageString = myLeadCount == 0 ? "" : (myQuasCount * 100 / myLeadCount) + "%"
                });
                #endregion

                #region Loop stages Probability != 0
                foreach (Stage myStage in allStages_WithProbability.OrderBy(o => o.Probability))
                {
                    IQueryable<Opportunity> iQueryable_ByStage =
                        (from a in iQueryable_Monthly
                         join b in iQueryable_OpportunityStages
                         on a.Id equals b.OpportunityId
                         where b.ToStageId == myStage.Id
                         select a);

                    int myCount = iQueryable_ByStage.Count();

                    myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                    {
                        Id = myMonthItemIndex++,
                        Date = myDate1,
                        DateString = myDate1.Month + "." + myDate1.Year,
                        StageName = myStage.Name,
                        OpportunitiesCount = myCount,
                        RowIndex = index++,
                        Percentage = myLeadCount == 0 ? 0 : (myCount * 100 / myLeadCount),
                        PercentageString = myLeadCount == 0 ? "" : (myCount * 100 / myLeadCount) + "%"
                    });
                }
                #endregion

                #region Closed Won Stage
                int myWonsCount = iQueryable_Monthly.Where(d => d.Stage.Id == myCloseWonStageId).Count();

                myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = myDate1.Month + "." + myDate1.Year,
                    StageName = myCloseWonStageName,
                    OpportunitiesCount = myWonsCount,
                    RowIndex = index++,
                    Percentage = myLeadCount == 0 ? 0 : (myWonsCount * 100 / myLeadCount),
                    PercentageString = myLeadCount == 0 ? "" : (myWonsCount * 100 / myLeadCount) + "%"
                });
                #endregion

                #region Closed Lost Stage
                int myLostCount = iQueryable_Monthly.Where(d => d.Stage.Id == myCloseLostStageId).Count();

                myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = myDate1.Month + "." + myDate1.Year,
                    StageName = myCloseLostStageName,
                    OpportunitiesCount = myLostCount,
                    RowIndex = index++,
                    Percentage = myLeadCount == 0 ? 0 : (myLostCount * 100 / myLeadCount),
                    PercentageString = myLeadCount == 0 ? "" : (myLostCount * 100 / myLeadCount) + "%"
                });
                #endregion

                #region Loop stages Probability == 0
                foreach (Stage myStage in allStages_ZeroProbability.OrderBy(o => o.Name))
                {
                    IQueryable<Opportunity> iQueryable_ByStage =
                        (from a in iQueryable_Monthly
                         join b in iQueryable_OpportunityStages
                         on a.Id equals b.OpportunityId
                         where b.ToStageId == myStage.Id
                         select a);

                    int myCount = iQueryable_ByStage.Count();

                    myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                    {
                        Id = myMonthItemIndex++,
                        Date = myDate1,
                        DateString = myDate1.Month + "." + myDate1.Year,
                        StageName = myStage.Name,
                        OpportunitiesCount = myCount,
                        RowIndex = index++,
                        Percentage = myLeadCount == 0 ? 0 : (myCount * 100 / myLeadCount),
                        PercentageString = myLeadCount == 0 ? "" : (myCount * 100 / myLeadCount) + "%"
                    });
                }
                #endregion

                myDate1 = myDate1.AddMonths(1);
                #endregion
            }

            #region Average

            myDataProvider.MonthlyDataList.Add(new MonthItemClass()
            {
                Id = myMonthItemIndex++,
                Date = myDate1,
                DateString = "Average",
                StageName = myFirstStageName,
                OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageName == myFirstStageName).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageName == myFirstStageName).FirstOrDefault().RowIndex,
            });

            myDataProvider.MonthlyDataList.Add(new MonthItemClass()
            {
                Id = myMonthItemIndex++,
                Date = myDate1,
                DateString = "Average",
                StageName = myQuaStageName,
                OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageName == myQuaStageName).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageName == myQuaStageName).FirstOrDefault().RowIndex,
            });

            foreach (Stage myStage in allStages_WithProbability.OrderBy(o => o.Probability))
            {
                myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = "Average",
                    StageName = myStage.Name,
                    OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageName == myStage.Name).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                    RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageName == myStage.Name).FirstOrDefault().RowIndex,
                });
            }

            myDataProvider.MonthlyDataList.Add(new MonthItemClass()
            {
                Id = myMonthItemIndex++,
                Date = myDate1,
                DateString = "Average",
                StageName = myCloseWonStageName,
                OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageName == myCloseWonStageName).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageName == myCloseWonStageName).FirstOrDefault().RowIndex,
            });

            myDataProvider.MonthlyDataList.Add(new MonthItemClass()
            {
                Id = myMonthItemIndex++,
                Date = myDate1,
                DateString = "Average",
                StageName = myCloseLostStageName,
                OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageName == myCloseLostStageName).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageName == myCloseLostStageName).FirstOrDefault().RowIndex,
            });

            foreach (Stage myStage in allStages_ZeroProbability.OrderBy(o => o.Name))
            {
                myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = "Average",
                    StageName = myStage.Name,
                    OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageName == myStage.Name).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                    RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageName == myStage.Name).FirstOrDefault().RowIndex,
                });
            }

            #endregion

            return myDataProvider;
        }

        private OpportunityMonthlyConversionDataProvider GetMonthlyConversionByStageDate(int tenant, DateTime date1, DateTime date2, string opportunityTypeCode, string countryId, string ownerId, string businessUnitId, List<string> myLeadSourcesList, string resellerId)
        {
            OpportunityMonthlyConversionDataProvider myDataProvider = new OpportunityMonthlyConversionDataProvider();
            myDataProvider.MonthlyDataList = new List<MonthItemClass>();

            #region Get Base Data
            ICRMContext myCRMCotnext = CRMContext.GetContext(tenant);
            StageRepository stageRepository = new StageRepository(myCRMCotnext);
            OpportunityRepository opportunityRepository = new OpportunityRepository(myCRMCotnext);
            OpportunityStageRepository opportunityStageRepository = new OpportunityStageRepository(myCRMCotnext);

            IQueryable<OpportunityStage> iQueryable_OpportunityStages = opportunityStageRepository.GetAll(tenant);

            IQueryable<Opportunity> iQueryable_Opportunities =
                (from a in opportunityRepository.GetAllForReport(tenant)
                 where a.IsCancelled == false
                 select a);

            if (!string.IsNullOrEmpty(opportunityTypeCode))
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => d.OpportunityTypeId == opportunityTypeCode);
            }

            if (!string.IsNullOrEmpty(countryId))
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => d.Customer.CountryId == countryId);
            }

            if (!string.IsNullOrEmpty(ownerId))
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(resellerId))
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => d.Customer.Customer != null && d.Customer.Customer.Field2 == resellerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => d.BusinessUnitId == businessUnitId);
            }

            if (myLeadSourcesList.Count() > 0)
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => myLeadSourcesList.Contains(d.LeadSourceId));
            }

            IQueryable<Stage> allStages = stageRepository.GetAll(tenant).Where(d => !d.InActive);
            IQueryable<Stage> allStages_WithProbability = allStages.Where(d => d.Probability != 0);
            IQueryable<Stage> allStages_ZeroProbability = allStages.Where(d => d.Probability == 0);

            //Qualification Stage
            string myQuaStageId = null;
            string myQuaStageName = null;
            Stage myQuaStage = allStages_WithProbability.Where(d => d.Code == "QUA").FirstOrDefault();
            if (myQuaStage != null)
            {
                myQuaStageId = myQuaStage.Id;
                myQuaStageName = myQuaStage.Name;
                allStages_WithProbability = allStages_WithProbability.Where(d => d.Id != myQuaStageId);
            }

            // Closed Won Stage
            string myCloseWonStageId = null;
            string myCloseWonStageName = null;
            Stage myCloseWonStage = allStages_WithProbability.Where(d => d.Code == "CWN").FirstOrDefault();
            if (myCloseWonStage != null)
            {
                myCloseWonStageId = myCloseWonStage.Id;
                myCloseWonStageName = myCloseWonStage.Name;
                allStages_WithProbability = allStages_WithProbability.Where(d => d.Id != myCloseWonStageId);
            }

            // Closed Lost Stage
            string myCloseLostStageId = null;
            string myCloseLostStageName = null;
            Stage myCloseLostStage = allStages_ZeroProbability.Where(d => d.Code == "CLS").FirstOrDefault();
            if (myCloseLostStage != null)
            {
                myCloseLostStageId = myCloseLostStage.Id;
                myCloseLostStageName = myCloseLostStage.Name;
                allStages_ZeroProbability = allStages_ZeroProbability.Where(d => d.Id != myCloseLostStageId);
            }

            // First Stage
            string myFirstStageId = null;
            string myFirstStageName = null;
            allStages_WithProbability = allStages_WithProbability.OrderBy(o => o.Probability);
            Stage myFirstStage = allStages_WithProbability.FirstOrDefault();
            if (myFirstStage != null)
            {
                myFirstStageId = myFirstStage.Id;
                myFirstStageName = myFirstStage.Name;
                allStages_WithProbability = allStages_WithProbability.Where(d => d.Id != myFirstStageId);
            }
            #endregion

            int index = 0;
            DateTime myDate1 = date1;
            int myMonthItemIndex = 0;
            int numberOfMonths = 0;

            while (myDate1 < date2)
            {
                numberOfMonths++;

                #region
                index = 0;

                IQueryable<Opportunity> iQueryable =
                    (from f in iQueryable_Opportunities
                     where f.CreateDate.Value.Year == myDate1.Year
                     && f.CreateDate.Value.Month == myDate1.Month
                     select f);

                IQueryable<Opportunity> iQueryable1 =
                    (from f in iQueryable_Opportunities
                     select f);

                #region First Stage
                int myLeadCount = iQueryable.Count();

                myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = myDate1.Month + "." + myDate1.Year,
                    StageName = myFirstStageName,
                    OpportunitiesCount = myLeadCount,
                    RowIndex = index++,
                });
                #endregion

                #region QUA Stage
                int myQuasCount = (from a in iQueryable1
                                   join b in iQueryable_OpportunityStages
                                   on a.Id equals b.OpportunityId
                                   where b.FromStageId == myQuaStageId
                                   && b.EndDate.Value.Year == myDate1.Year
                                   && b.EndDate.Value.Month == myDate1.Month
                                   select a).Count();

                myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = myDate1.Month + "." + myDate1.Year,
                    StageName = myQuaStageName,
                    OpportunitiesCount = myQuasCount,
                    RowIndex = index++,
                    Percentage = myLeadCount == 0 ? 0 : (myQuasCount * 100 / myLeadCount),
                    PercentageString = myLeadCount == 0 ? "" : (myQuasCount * 100 / myLeadCount) + "%"
                });

                #endregion

                #region Loop stages Probability != 0
                foreach (Stage myStage in allStages_WithProbability.OrderBy(o => o.Probability))
                {
                    IQueryable<OpportunityStage> iQueryable_ByStage =
                        (from a in iQueryable_OpportunityStages
                         join b in iQueryable_Opportunities
                         on a.OpportunityId equals b.Id
                         where a.ToStageId == myStage.Id
                         && a.EndDate != null
                         && a.EndDate.Value.Year == myDate1.Year
                         && a.EndDate.Value.Month == myDate1.Month
                         select a);

                    int myCount = iQueryable_ByStage.Count();

                    myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                    {
                        Id = myMonthItemIndex++,
                        Date = myDate1,
                        DateString = myDate1.Month + "." + myDate1.Year,
                        StageName = myStage.Name,
                        OpportunitiesCount = myCount,
                        RowIndex = index++,
                        Percentage = myLeadCount == 0 ? 0 : (myCount * 100 / myLeadCount),
                        PercentageString = myLeadCount == 0 ? "" : (myCount * 100 / myLeadCount) + "%"
                    });
                }
                #endregion

                #region Close Won
                int myWonsCount = iQueryable.Where(d => d.Stage.Id == myCloseWonStageId).Count();

                myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = myDate1.Month + "." + myDate1.Year,
                    StageName = myCloseWonStageName,
                    OpportunitiesCount = myWonsCount,
                    RowIndex = index++,
                    Percentage = myLeadCount == 0 ? 0 : (myWonsCount * 100 / myLeadCount),
                    PercentageString = myLeadCount == 0 ? "" : (myWonsCount * 100 / myLeadCount) + "%"
                });
                #endregion

                #region Close Lost
                int myLostCount = iQueryable.Where(d => d.Stage.Id == myCloseLostStageId).Count();

                myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = myDate1.Month + "." + myDate1.Year,
                    StageName = myCloseLostStageName,
                    OpportunitiesCount = myLostCount,
                    RowIndex = index++,
                    Percentage = myLeadCount == 0 ? 0 : (myLostCount * 100 / myLeadCount),
                    PercentageString = myLeadCount == 0 ? "" : (myLostCount * 100 / myLeadCount) + "%"
                });
                #endregion

                #region Loop stages Probability == 0
                foreach (Stage myStage in allStages_ZeroProbability.OrderBy(o => o.Name))
                {
                    IQueryable<OpportunityStage> iQueryable_ByStage =
                        (from a in iQueryable_OpportunityStages
                         where a.ToStageId == myStage.Id
                         && a.EndDate != null
                         && a.EndDate.Value.Year == myDate1.Year
                         && a.EndDate.Value.Month == myDate1.Month
                         select a);

                    int myCount = iQueryable_ByStage.Count();

                    myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                    {
                        Id = myMonthItemIndex++,
                        Date = myDate1,
                        DateString = myDate1.Month + "." + myDate1.Year,
                        StageName = myStage.Name,
                        OpportunitiesCount = myCount,
                        RowIndex = index++,
                        Percentage = myLeadCount == 0 ? 0 : (myCount * 100 / myLeadCount),
                        PercentageString = myLeadCount == 0 ? "" : (myCount * 100 / myLeadCount) + "%"
                    });
                }
                #endregion

                myDate1 = myDate1.AddMonths(1);
                #endregion
            }

            #region Average

            myDataProvider.MonthlyDataList.Add(new MonthItemClass()
            {
                Id = myMonthItemIndex++,
                Date = myDate1,
                DateString = "Average",
                StageName = myFirstStageName,
                OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageName == myFirstStageName).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageName == myFirstStageName).FirstOrDefault().RowIndex,
            });

            myDataProvider.MonthlyDataList.Add(new MonthItemClass()
            {
                Id = myMonthItemIndex++,
                Date = myDate1,
                DateString = "Average",
                StageName = myQuaStageName,
                OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageName == myQuaStageName).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageName == myQuaStageName).FirstOrDefault().RowIndex,
            });

            foreach (Stage myStage in allStages_WithProbability.OrderBy(o => o.Probability))
            {
                myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = "Average",
                    StageName = myStage.Name,
                    OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageName == myStage.Name).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                    RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageName == myStage.Name).FirstOrDefault().RowIndex,
                });
            }

            myDataProvider.MonthlyDataList.Add(new MonthItemClass()
            {
                Id = myMonthItemIndex++,
                Date = myDate1,
                DateString = "Average",
                StageName = myCloseWonStageName,
                OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageName == myCloseWonStageName).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageName == myCloseWonStageName).FirstOrDefault().RowIndex,
            });

            myDataProvider.MonthlyDataList.Add(new MonthItemClass()
            {
                Id = myMonthItemIndex++,
                Date = myDate1,
                DateString = "Average",
                StageName = myCloseLostStageName,
                OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageName == myCloseLostStageName).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageName == myCloseLostStageName).FirstOrDefault().RowIndex,
            });

            foreach (Stage myStage in allStages_ZeroProbability.OrderBy(o => o.Name))
            {
                myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = "Average",
                    StageName = myStage.Name,
                    OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageName == myStage.Name).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                    RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageName == myStage.Name).FirstOrDefault().RowIndex,
                });
            }

            #endregion

            return myDataProvider;
        }
        #endregion

        #region Expected Income Report
        [WebMethod]
        public byte[] LoadExpectedIncomeData(byte[] xmlFilters, int tenant)
        {
            ExpectedIncomeDataProvider dataProvider = this.LoadExpectedIncomeDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataProvider, typeof(ExpectedIncomeDataProvider), tenant);
        }

        private ExpectedIncomeDataProvider LoadExpectedIncomeDataProvider(byte[] xmlFilters, int tenant)
        {
            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            CustomerRepository customerRepository = new CustomerRepository(tenant);
            ExpectedIncomeDataProvider dataProvider = new ExpectedIncomeDataProvider();
            dataProvider.RecordList = new List<CRMCustomer>();
            dataProvider.ErrorsList = new List<ErrorItemClass>();

            // Need it for the sort
            List<CRMCustomer> myRecordList = new List<CRMCustomer>();
            List<ErrorItemClass> myErrorList = new List<ErrorItemClass>();

            #region Customers Base Data
            QueryFilterItem filterItem_Salesman = queryOperations.QueryFilterItems.Where(d => d.FieldName == "SalesmanId").FirstOrDefault();
            QueryFilterItem filterItem_Country = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CountryId").FirstOrDefault();
            QueryFilterItem filterItem_Reseller = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ResellerId").FirstOrDefault();

            IQueryable<CustomersDataView> iQueryable_AllCustomers = customerRepository.GetCustomersDataViews(LogitudeSettings.LogitudeCRMTenantNumber);

            if (filterItem_Salesman != null)
            {
                string mySalesmanId = filterItem_Salesman.FieldValue.ToString();

                if (!string.IsNullOrEmpty(mySalesmanId))
                {
                    iQueryable_AllCustomers = iQueryable_AllCustomers.Where(d => d.SalesmanUserId == mySalesmanId);
                }
            }

            if (filterItem_Country != null)
            {
                string myCountryId = filterItem_Country.FieldValue.ToString();

                if (!string.IsNullOrEmpty(myCountryId))
                {
                    iQueryable_AllCustomers = iQueryable_AllCustomers.Where(d => d.CountryId == myCountryId);
                }
            }

            if (filterItem_Reseller != null)
            {
                string myResellerId = filterItem_Reseller.FieldValue.ToString();

                if (!string.IsNullOrEmpty(myResellerId))
                {
                    iQueryable_AllCustomers = iQueryable_AllCustomers.Where(d => d.Field2 == myResellerId);
                }
            }
            #endregion

            #region Tenant Managements Base Data
            QueryFilterItem filterItem_Payment = queryOperations.QueryFilterItems.Where(d => d.FieldName == "PaymentChannelCode" && d.Operator == "Equals").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_Recurring = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Recurring").FirstOrDefault();

            IQueryable<TenantManagement> iQueryable_TenantManagements = null;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                iQueryable_TenantManagements = tenantManagementRepository.GetTenants();
                scope.Complete();
            }

            if (filterItem_Payment != null)
            {
                string myPaymentChannelCode = filterItem_Payment.FieldValue.ToString();

                if (!string.IsNullOrEmpty(myPaymentChannelCode))
                {
                    iQueryable_TenantManagements = iQueryable_TenantManagements.Where(d => d.PaymentChannelCode == myPaymentChannelCode);
                }
            }

            if (filterItem_Recurring != null)
            {
                string myRecurringCode = filterItem_Recurring.FieldValue.ToString();

                if (!string.IsNullOrEmpty(myRecurringCode))
                {
                    if (myRecurringCode == "A")
                    {

                    }

                    else if (myRecurringCode == "Y")
                    {
                        iQueryable_TenantManagements = iQueryable_TenantManagements.Where(d => d.IsRecurring);
                    }

                    else if (myRecurringCode == "N")
                    {
                        iQueryable_TenantManagements = iQueryable_TenantManagements.Where(d => !d.IsRecurring);
                    }
                }
            }

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            //DateTime myStartDate = todayDate.AddMonths(-1);

            //DateTime? fromDate = null;
            //DateTime? toDate = null;

            if (filterItem_FromDate != null)
            {
                DateTime fromDate;
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);
                if (fromDate != null)
                {
                    dataProvider.FromDate = fromDate;
                    iQueryable_TenantManagements = iQueryable_TenantManagements.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.PaidUntilDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                }
            }

            if (filterItem_ToDate != null)
            {
                DateTime toDate;
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
                if (toDate != null)
                {
                    dataProvider.ToDate = toDate;
                    iQueryable_TenantManagements = iQueryable_TenantManagements.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.PaidUntilDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                }
            }
            #endregion

            IQueryable<CustomersDataView> iQueryable_IsActiveCustomers = iQueryable_AllCustomers.Where(d => d.ReceivablesAccountingCard != null && d.IsCustomer == true && d.CustomerStatusCode == "ACT");
            IQueryable<CustomersDataView> iQueryable_InActiveCustomers = iQueryable_AllCustomers.Where(d => d.ReceivablesAccountingCard != null && d.CustomerStatusCode != "ACT");

            #region Error list
            if (iQueryable_InActiveCustomers.Count() > 0 && iQueryable_TenantManagements.Count() > 0)
            {
                foreach (CustomersDataView customerView in iQueryable_InActiveCustomers)
                {
                    int externalTenantId = 0;
                    bool isTenantManagementExists = Int32.TryParse(customerView.ReceivablesAccountingCard, out externalTenantId);

                    if (isTenantManagementExists)
                    {
                        TenantManagement myTenantManagement = iQueryable_TenantManagements.Where(d => d.Id == externalTenantId).FirstOrDefault();

                        if (myTenantManagement != null)
                        {
                            bool isOnErrorRecord = false;

                            if (myTenantManagement.IsRecurring)
                            {
                                isOnErrorRecord = true;
                            }

                            else if (myTenantManagement.PaidUntilDate != null)
                            {
                                if (myTenantManagement.PaidUntilDate > todayDate)
                                {
                                    isOnErrorRecord = true;
                                }
                            }

                            if (isOnErrorRecord)
                            {
                                myErrorList.Add(new ErrorItemClass()
                                {
                                    Id = customerView.Id,
                                    Tenant = externalTenantId,
                                    CustomerName = customerView.EnglishName,
                                    CustomerCode = customerView.Code,
                                });
                            }
                        }
                    }
                }
            }
            #endregion

            #region Report list
            if (iQueryable_IsActiveCustomers.Count() > 0 && iQueryable_TenantManagements.Count() > 0)
            {
                TenantRepository tenantRepository = new TenantRepository(tenant);
                PackageRepository packageRepository = new PackageRepository(tenant);
                PaymentChannelRepository paymentChannelRepository = new PaymentChannelRepository(tenant);
                CustomerSalesNoteRepository salesNoteRepository = new CustomerSalesNoteRepository(tenant);
                WebFreightDomainService webFreightDomain = new WebFreightDomainService();

                IQueryable<Tenant> iQueryable_Tenants = tenantRepository.GetTenants();
                IQueryable<PaymentChannel> iQueryable_PaymentChannels = paymentChannelRepository.GetPaymentChannels();
                IQueryable<CustomerSalesNote> iQueryable_SalesNotes = salesNoteRepository.GetCustomerSalesNotes();

                foreach (CustomersDataView customerView in iQueryable_IsActiveCustomers)
                {
                    if (!string.IsNullOrEmpty(customerView.ReceivablesAccountingCard))
                    {
                        int externalTenantId = 0;
                        bool isTenantManagementExists = Int32.TryParse(customerView.ReceivablesAccountingCard, out externalTenantId);

                        if (isTenantManagementExists)
                        {
                            TenantManagement myTenantManagement = iQueryable_TenantManagements.Where(d => d.Id == externalTenantId).FirstOrDefault();

                            if (myTenantManagement != null)
                            {
                                if (!myTenantManagement.IsTrial)
                                {
                                    CRMCustomer myResultItem = new DataProviders.CRMCustomer();

                                    myResultItem.TenantNumber = externalTenantId;
                                    myResultItem.CustomerName = customerView.EnglishName;
                                    myResultItem.SalesmanName = customerView.SalesmanUserEnglishName;
                                    myResultItem.CountryName = customerView.CountryName;
                                    myResultItem.FreeUsersNumber = myTenantManagement.FreeUsers;
                                    myResultItem.LicensedUsersNumber = myTenantManagement.NumberOfUsers;
                                    myResultItem.PaidUntilDate = myTenantManagement.PaidUntilDate;

                                    if (!string.IsNullOrEmpty(myTenantManagement.PackageCode))
                                    {
                                        Package package = packageRepository.GetSinglePackage(myTenantManagement.PackageCode);
                                        if (package != null)
                                        {
                                            myResultItem.Package = package.Name;
                                        }
                                    }

                                    #region Recurring
                                    if (!string.IsNullOrEmpty(myTenantManagement.RecurringPeriodCode) && myTenantManagement.IsRecurring)
                                    {
                                        myResultItem.RecurringPeriodCode = myTenantManagement.RecurringPeriodCode.Substring(0, 1);
                                        myResultItem.PaidUntilDate = null;
                                    }
                                    #endregion

                                    #region PaymentChannel

                                    myResultItem.PaymentDate = myTenantManagement.FirstPaymentDate;

                                    if (!string.IsNullOrEmpty(myTenantManagement.PaymentChannelCode))
                                    {
                                        PaymentChannel myPaymentChannel = iQueryable_PaymentChannels.Where(d => d.Code == myTenantManagement.PaymentChannelCode).FirstOrDefault();
                                        if (myPaymentChannel != null)
                                        {
                                            myResultItem.PaymentChannelName = myPaymentChannel.Name;
                                        }
                                    }
                                    #endregion

                                    #region SalesNotes
                                    IQueryable<CustomerSalesNote> mySalesNotes = iQueryable_SalesNotes.Where(d => d.CustomerId == customerView.Id && d.Tenant == externalTenantId);
                                    if (mySalesNotes.Count() > 0)
                                    {
                                        string mySalesNotesString = "";

                                        foreach (CustomerSalesNote item in mySalesNotes)
                                        {
                                            if (string.IsNullOrEmpty(mySalesNotesString))
                                            {
                                                mySalesNotesString = item.Notes;
                                            }

                                            else
                                            {
                                                mySalesNotesString += "\n" + item.Notes;
                                            }
                                        }

                                        myResultItem.SalesNotes = mySalesNotesString;
                                    }
                                    #endregion

                                    #region Reseller
                                    if (!string.IsNullOrEmpty(customerView.Field2))
                                    {
                                        CardRepository cardRepository = new CardRepository(tenant);
                                        Card card = cardRepository.GetSingleCard(customerView.Field2, tenant);
                                        if (card != null)
                                        {
                                            myResultItem.ResellerName = card.EnglishName;
                                        }
                                    }
                                    #endregion

                                    #region Currency

                                    myResultItem.CurrencyCode = myTenantManagement.PaymentCurrencyCode;

                                    if (string.IsNullOrEmpty(myResultItem.CurrencyCode))
                                    {
                                        myResultItem.CurrencyCode = "null";
                                    }
                                    #endregion

                                    if (myTenantManagement.LicensePrice != null && !string.IsNullOrEmpty(myTenantManagement.PaymentCurrencyCode))
                                    {
                                        double myPrice = 0;
                                        int myLicensedUsers = 0;

                                        if (myTenantManagement.LicensePrice != null)
                                        {
                                            myPrice = myTenantManagement.LicensePrice.Value;
                                        }

                                        if (myResultItem.LicensedUsersNumber != null)
                                        {
                                            myLicensedUsers = myResultItem.LicensedUsersNumber.Value;
                                        }

                                        myResultItem.Price = myPrice;
                                        myResultItem.Total = (myPrice * myLicensedUsers);

                                        #region USD : EUR

                                        if (myTenantManagement.PaymentCurrencyCode == "USD")
                                        {
                                            myResultItem.TotalInUSD = myResultItem.Total;
                                            myResultItem.GrandTotalInUSD = myResultItem.Total;
                                        }

                                        else if (myTenantManagement.PaymentCurrencyCode == "EUR")
                                        {
                                            myResultItem.TotalInEUR = myResultItem.Total;
                                            myResultItem.GrandTotalInEUR = myResultItem.Total;
                                        }

                                        #endregion
                                    }

                                    myRecordList.Add(myResultItem);
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            if (myErrorList.Count > 0)
            {
                foreach (ErrorItemClass item in myErrorList.OrderBy(o1 => o1.CustomerName).OrderBy(o => o.Tenant))
                {
                    dataProvider.ErrorsList.Add(item);
                }
            }

            if (myRecordList.Count > 0)
            {
                foreach (CRMCustomer item in myRecordList.OrderBy(o1 => o1.ResellerName).ThenBy(o => o.TenantNumber))
                {
                    dataProvider.RecordList.Add(item);
                }
            }

            return dataProvider;
        }
        #endregion

        #region Container Trucking Report
        [WebMethod]
        public byte[] LoadContainerTruckingData(byte[] xmlFilters, int tenant)
        {
            ContainerTruckingDataProvider dataProviderData = LoadContainerTruckingDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataProviderData, typeof(ContainerTruckingDataProvider), tenant);
        }

        private ContainerTruckingDataProvider LoadContainerTruckingDataProvider(byte[] xmlFilters, int tenant)
        {
            ContainerTruckingDataProvider totalData = new ContainerTruckingDataProvider();
            totalData.RecordsList = new List<ContainerTruckingRecord>();

            CustomerRepository customerRep = new CustomerRepository(tenant);
            AgentRepository agentRep = new AgentRepository(tenant);
            DirectionRepository directionRep = new DirectionRepository(tenant);

            ShipmentDeliveryQuery query = new ShipmentDeliveryQuery(tenant);
            List<ShipmentDeliveryPM> deliveries = query.GetShipmentDeliveryByTenant(tenant);
            deliveries = deliveries.Where(d => d.IsCancelled == false).ToList();

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_CustomerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();
            QueryFilterItem filterItem_AgentId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "AgentId").FirstOrDefault();
            QueryFilterItem filterItem_DirectionId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DirectionId").FirstOrDefault();

            string customerId = null;
            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    customerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }

            string agentId = null;
            if (filterItem_AgentId != null)
            {
                if (filterItem_AgentId.FieldValue != null)
                {
                    agentId = filterItem_AgentId.FieldValue.ToString();
                }
            }

            string directionId = null;
            if (filterItem_DirectionId != null)
            {
                if (filterItem_DirectionId.FieldValue != null)
                {
                    directionId = filterItem_DirectionId.FieldValue.ToString();
                }
            }

            Customer customer = null;
            if (!string.IsNullOrEmpty(customerId))
            {
                deliveries = deliveries.Where(d => d.CustomerId == customerId).ToList();
                customer = customerRep.GetSingleCustomer(customerId, tenant, true);
            }

            Agent agent = null;
            if (!string.IsNullOrEmpty(agentId))
            {
                deliveries = deliveries.Where(d => d.AgentId == agentId).ToList();
                agent = agentRep.GetSingleAgent(tenant, agentId);
            }

            Direction direction = null;
            if (!string.IsNullOrEmpty(directionId))
            {
                deliveries = deliveries.Where(d => d.DirectionId == directionId).ToList();
                direction = directionRep.GetSingleDirection(directionId);
            }

            totalData.CustomerFilter = customer == null ? "All Customers" : customer.Card.EnglishName;
            totalData.DirectionFilter = direction == null ? "All Directions" : direction.Name;
            totalData.AgentFilter = agent == null ? "All Agents" : agent.Card.EnglishName;

            foreach (ShipmentDeliveryPM item in deliveries)
            {
                if (item.ShipmentPickUpDeliveryPackages.Count > 0)
                {
                    ContainerTruckingRecord record = new ContainerTruckingRecord();

                    record.AgentName = item.AgentName;
                    record.FBLNumber = item.MasterNumber;
                    record.FileNumber = item.ShipmentNumber;
                    record.ShippingLine = item.ShippingLine;
                    record.DeliveryATA = item.ATA;
                    record.DeliveryATD = item.ATD;
                    record.DischargePort = item.ToPortName;

                    if (item.ShipmentPickUpDeliveryPackages.Count > 0)
                    {
                        foreach (ShipmentPickUpDeliveryPackagePM package in item.ShipmentPickUpDeliveryPackages)
                        {
                            record.ContainerNumber += package.ContainerNumber + ", ";
                            record.TEU += package.PackageTypeTEU + ", ";
                        }

                        if (!string.IsNullOrEmpty(record.ContainerNumber))
                        {
                            record.ContainerNumber = record.ContainerNumber.TrimEnd(' ', ',');
                        }

                        if (!string.IsNullOrEmpty(record.TEU))
                        {
                            record.TEU = record.TEU.TrimEnd(' ', ',');
                        }
                    }

                    totalData.RecordsList.Add(record);
                }
            }

            return totalData;
        }
        #endregion

        #region Container Details by Voyage Report
        [WebMethod]
        public byte[] LoadContainerDetailsVoyageData(byte[] xmlFilters, int tenant)
        {
            ContainerDetailsVoyageDataProvider dataProviderData = LoadContainerDetailsVoyageDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataProviderData, typeof(ContainerDetailsVoyageDataProvider), tenant);
        }

        private ContainerDetailsVoyageDataProvider LoadContainerDetailsVoyageDataProvider(byte[] xmlFilters, int tenant)
        {
            ContainerDetailsVoyageDataProvider totalData = new ContainerDetailsVoyageDataProvider();

            CardRepository cardRep = new CardRepository(tenant);
            DirectionRepository directionRep = new DirectionRepository(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            IQueryable<ShipmentDataView> iQueryable = shipmentRepository.GetShipmentViewsByTenant(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_CustomerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();
            QueryFilterItem filterItem_DirectionId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DirectionId").FirstOrDefault();
            QueryFilterItem filterItem_Voyage = queryOperations.QueryFilterItems.Where(d => d.FieldName == "VoyageNumber").FirstOrDefault();
            QueryFilterItem filterItem_CreateFrom = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CreateFromDate" && d.Operator == "GreaterThanOrEqual").FirstOrDefault();
            QueryFilterItem filterItem_CreateTo = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CreateToDate" && d.Operator == "LessThanOrEqual").FirstOrDefault();
            QueryFilterItem filterItem_SalingFrom = queryOperations.QueryFilterItems.Where(d => d.FieldName == "SalingFromDate" && d.Operator == "GreaterThanOrEqual").FirstOrDefault();
            QueryFilterItem filterItem_SalingTo = queryOperations.QueryFilterItems.Where(d => d.FieldName == "SalingToDate" && d.Operator == "LessThanOrEqual").FirstOrDefault();

            string customerId = null;
            string directionId = null;
            string voyageNumber = null;

            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    customerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }

            if (filterItem_DirectionId != null)
            {
                if (filterItem_DirectionId.FieldValue != null)
                {
                    directionId = filterItem_DirectionId.FieldValue.ToString();
                }
            }

            if (filterItem_Voyage != null)
            {
                if (filterItem_Voyage.FieldValue != null)
                {
                    voyageNumber = filterItem_Voyage.FieldValue.ToString();
                }
            }

            #endregion

            #region Base Data Filtered

            iQueryable = iQueryable.Where(a => a.TransportModeId == "O" && a.ShipmentTypeId == "FCLD");
            iQueryable = iQueryable.Where(a => a.IsCancelled == false);

            string customerLable = "All Customers";
            string directionLable = "All Directions";

            if (!string.IsNullOrEmpty(customerId))
            {
                iQueryable = iQueryable.Where(d => d.CustomerId == customerId);

                Card partner = cardRep.GetSingleCard(customerId, tenant);
                customerLable = partner == null ? "All Customers" : partner.EnglishName;
            }

            if (!string.IsNullOrEmpty(directionId))
            {
                iQueryable = iQueryable.Where(d => d.DirectionId == directionId);

                Direction direction = directionRep.GetSingleDirection(directionId);
                directionLable = direction == null ? "All Directions" : direction.Name;
            }

            if (!string.IsNullOrEmpty(voyageNumber))
            {
                iQueryable = iQueryable.Where(d => d.MainCarriageCarrierNumber == voyageNumber);
            }

            totalData.DirectionFilter = directionLable;
            totalData.CustomerFilter = customerLable;
            totalData.VoyageNumberFilter = string.IsNullOrEmpty(voyageNumber) ? "All Voyages" : voyageNumber;

            if (filterItem_CreateFrom != null)
            {
                DateTime fromDate;
                DateTime.TryParse(filterItem_CreateFrom.FieldValue.ToString(), out fromDate);
                if (fromDate != null)
                {
                    iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= fromDate.Date);
                }
            }

            if (filterItem_CreateTo != null)
            {
                DateTime toDate;
                DateTime.TryParse(filterItem_CreateTo.FieldValue.ToString(), out toDate);
                if (toDate != null)
                {
                    iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= toDate.Date);
                }
            }

            if (filterItem_SalingFrom != null)
            {
                DateTime fromDate;
                DateTime.TryParse(filterItem_SalingFrom.FieldValue.ToString(), out fromDate);
                if (fromDate != null)
                {
                    iQueryable = iQueryable.Where(d => d.MainCarriageATD != null && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATD) >= fromDate.Date);
                }
            }

            if (filterItem_SalingTo != null)
            {
                DateTime toDate;
                DateTime.TryParse(filterItem_SalingTo.FieldValue.ToString(), out toDate);
                if (toDate != null)
                {
                    iQueryable = iQueryable.Where(d => d.MainCarriageATD != null && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATD) <= toDate.Date);
                }
            }

            #endregion

            #region Fill Report Data

            List<ShipmentDataView> allShipments = iQueryable.ToList();
            List<string> allShipmentsId = allShipments.Select(s => s.Id).ToList();

            ShipmentPackageQuery query = new ShipmentPackageQuery(tenant);

            List<ShipmentPackagePM> containers = query.GetShipmentPackages(allShipmentsId, tenant);

            List<ContainerVoyageDetailsRecord> tempList = new List<ContainerVoyageDetailsRecord>();
            foreach (ShipmentPackagePM itemContainer in containers)
            {
                ContainerVoyageDetailsRecord record = new ContainerVoyageDetailsRecord();

                ShipmentDataView myShipmentDataView = allShipments.Where(d => d.Id == itemContainer.ShipmentId).FirstOrDefault();
                if (myShipmentDataView != null)
                {
                    record.LoadingPortCode = myShipmentDataView.MainCarriageFromPortCode;
                    record.DischargePortCode = myShipmentDataView.MainCarriageFinalDestinationPortCode;
                    record.BookingNumber = myShipmentDataView.BookingConfirmationNumber;
                    record.Master = myShipmentDataView.LongMaster;
                    record.House = myShipmentDataView.House;
                    record.VoyageNumber = myShipmentDataView.MainCarriageCarrierNumber;
                    record.VesselId = myShipmentDataView.MainCarriageVesselId;
                    record.ShippingLineId = myShipmentDataView.MainCarriageCarrierId;
                    record.VesselName = myShipmentDataView.MainCarriageVesselName;
                    record.ShippingLineName = myShipmentDataView.MainCarriageCarrierName;
                    record.ReleasingAgentName = myShipmentDataView.ReleasingAgentName;
                }

                record.ContainerNumber = itemContainer.ContainerNumber;
                record.ContainerType = string.IsNullOrEmpty(itemContainer.PrintAs) ? itemContainer.PackageTypeCode : itemContainer.PrintAs;
                record.SealNumber = itemContainer.ShipperSeal;
                record.InsidePackagesCount = itemContainer.NumberOfInsidePackages;
                record.DescriptionOfGoods = itemContainer.Description;
                record.GrossWeight = itemContainer.Weight;
                record.REEFTemp = itemContainer.Temperature;
                record.IMO = itemContainer.IsDangerous ? "Yes" : "";
                record.IMDGNumber = itemContainer.IMDGCode;
                record.IMOClass = itemContainer.ClassNumber;

                tempList.Add(record);
            }

            totalData.TotalContainersCount = containers.Count;

            #endregion

            #region Group Data
            List<ContainerVoyageDetailsGroup> finalResults = (from p in tempList
                                                              group p by new { p.VoyageNumber, p.VesselId, p.ShippingLineId } into g
                                                              select new ContainerVoyageDetailsGroup()
                                                              {
                                                                  VoyageNumber = g.Key.VoyageNumber,
                                                                  VesselId = g.Key.VesselId,
                                                                  ShippingLineId = g.Key.ShippingLineId,
                                                                  RecordsList = g.ToList(),
                                                              }).ToList();

            foreach (ContainerVoyageDetailsGroup item in finalResults)
            {
                Vessel vessel = commonContext.Vessels.Where(d => d.Id == item.VesselId).FirstOrDefault();
                Card carrier = commonContext.Cards.Where(d => d.Id == item.ShippingLineId).FirstOrDefault();

                if (vessel != null)
                {
                    item.VesselName = vessel.EnglishName;
                }

                if (carrier != null)
                {
                    item.ShippingLineName = carrier.EnglishName;
                }

                ContainerVoyageDetailsRecord totalRecord = new ContainerVoyageDetailsRecord();
                totalRecord.ContainerNumber = "Total";
                totalRecord.ContainerType = item.RecordsList.Count.ToString();

                item.RecordsList.Add(totalRecord);
            }

            totalData.GroupList = finalResults.OrderBy(d => d.VoyageNumber).ToList();
            #endregion

            return totalData;
        }
        #endregion

        #region Customer Additional Services Report
        [WebMethod]
        public byte[] LoadCustomerAdditionalServicesData(byte[] xmlFilters, int tenant)
        {
            CustomerAdditionalServicesDataProvider dataProviderData = LoadCustomerAdditionalServicesDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataProviderData, typeof(CustomerAdditionalServicesDataProvider), tenant);
        }

        private CustomerAdditionalServicesDataProvider LoadCustomerAdditionalServicesDataProvider(byte[] xmlFilters, int tenant)
        {
            CustomerAdditionalServicesDataProvider totalData = new CustomerAdditionalServicesDataProvider();
            totalData.AdditionalServices = new List<AdditionalServicesDataList>();

            CustomerAdditionalServiceRepository serviceRep = new CustomerAdditionalServiceRepository(tenant);
            IQueryable<CustomerAdditionalService> allServices = serviceRep.GetAdditionalServicesByTenant(tenant).Include("Customer").Include("Customer.Card").Include("Customer.SalesmanUser").Include("Customer.Card.PrimaryContact").Include("Customer.SalesmanUser.Contact").Include("AdditionalService").Include("Customer.CustomerStatus");

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            #region Report Filters

            QueryFilterItem filterItem_BusinessUnitId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "BusinessUnitId").FirstOrDefault();
            QueryFilterItem filterItem_SalesmanUserId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "SalesmanUserId").FirstOrDefault();
            QueryFilterItem filterItem_AdditionalServices = queryOperations.QueryFilterItems.Where(d => d.FieldName == "AdditionalServices").FirstOrDefault();
            QueryFilterItem filterItem_ServiceType = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ServiceType").FirstOrDefault();
            QueryFilterItem filterItem_CustomerStatus = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerStatus").FirstOrDefault();

            string businessUnitId = null;
            string salesmanId = null;
            string additionalServices = null;
            string serviceType = null;
            string customerStatus = null;

            if (filterItem_BusinessUnitId != null)
            {
                if (filterItem_BusinessUnitId.FieldValue != null)
                {
                    businessUnitId = filterItem_BusinessUnitId.FieldValue.ToString();
                }
            }

            if (filterItem_SalesmanUserId != null)
            {
                if (filterItem_SalesmanUserId.FieldValue != null)
                {
                    salesmanId = filterItem_SalesmanUserId.FieldValue.ToString();
                }
            }

            if (filterItem_AdditionalServices != null)
            {
                if (filterItem_AdditionalServices.FieldValue != null)
                {
                    additionalServices = filterItem_AdditionalServices.FieldValue.ToString();
                }
            }

            if (filterItem_ServiceType != null)
            {
                if (filterItem_ServiceType.FieldValue != null)
                {
                    serviceType = filterItem_ServiceType.FieldValue.ToString();
                }
            }

            if (filterItem_CustomerStatus != null)
            {
                if (filterItem_CustomerStatus.FieldValue != null)
                {
                    customerStatus = filterItem_CustomerStatus.FieldValue.ToString();
                }
            }

            #endregion

            #region Base Data Filtered

            allServices = allServices.Where(d => d.Customer.ActivityWatch);

            BusinessUnitRepository unitRep = new BusinessUnitRepository(tenant);
            UserRepository userRep = new UserRepository(tenant);
            CustomerBusinessUnitFilter myBusinessUnitFilter = new CustomerBusinessUnitFilter(tenant);
            allServices = myBusinessUnitFilter.RunFilter(allServices);

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                allServices = allServices.Where(d => d.Customer.SalesmanUser != null && d.Customer.SalesmanUser.BusinessUnitId == businessUnitId);
            }

            if (!string.IsNullOrEmpty(salesmanId))
            {
                allServices = allServices.Where(d => d.Customer.SalesmanUserId == salesmanId);
            }

            if (!string.IsNullOrEmpty(customerStatus))
            {
                allServices = allServices.Where(d => d.Customer.CustomerStatusCode == customerStatus);
            }

            if (!string.IsNullOrEmpty(additionalServices))
            {
                List<string> myadditionalServicesList = new List<string>();

                additionalServices = additionalServices.Replace(" ", "");

                if (additionalServices.ToLower() == "all")
                {
                    AdditionalServiceRepository additionalServiceRepository = new AdditionalServiceRepository(tenant);
                    IQueryable<AdditionalService> iQueryable = additionalServiceRepository.GetAdditionalServices(tenant);
                    if (iQueryable.Count() > 0)
                    {
                        myadditionalServicesList = iQueryable.Select(s => s.Id).ToList();
                    }
                }
                else
                {
                    additionalServices = additionalServices.Trim(',');

                    string[] myServices = additionalServices.Split(',');

                    myadditionalServicesList = myServices.ToList();
                }

                if (myadditionalServicesList.Count() > 0)
                {
                    allServices = allServices.Where(d => myadditionalServicesList.Contains(d.AdditionalServiceId));
                }
            }

            if (serviceType == "Potential")
            {
                allServices = allServices.Where(d => d.Potential);
            }
            else if (serviceType == "In Use")
            {
                allServices = allServices.Where(d => !d.Potential);
            }

            #endregion

            BusinessUnit unit = unitRep.GetSingleBusinessUnit(businessUnitId, tenant);
            User user = userRep.GetSingleUser(salesmanId, tenant, false);

            totalData.BusinessUnitName = unit == null ? "All business units" : unit.Name;
            totalData.SalesmanUserName = user == null ? "All users" : user.Contact.EnglishName;

            foreach (CustomerAdditionalService service in allServices.OrderBy(o => o.Customer.Card.EnglishName))
            {
                totalData.AdditionalServices.Add(new AdditionalServicesDataList()
                {
                    CustomerName = service.Customer.Card.EnglishName,
                    PrimaryContact = service.Customer.Card.PrimaryContact != null ? service.Customer.Card.PrimaryContact.EnglishName : null,
                    PrimaryContactEmail = service.Customer.Card.PrimaryContact != null ? service.Customer.Card.PrimaryContact.Email : null,
                    Salesman = service.Customer.SalesmanUser != null ? service.Customer.SalesmanUser.Contact.EnglishName : null,
                    ServiceName = service.AdditionalService.Name,
                    Potential_InUse = service.Potential ? "Potential" : "In Use",
                    NumberOfShipments = tenant == 341 ? Convert.ToInt32(service.Customer.Field1) : 0,
                    NumberOfShipmentsLabel = tenant == 341 ? "Number of Users" : "Number of Shipments",
                    CustomerStatus = service.Customer.CustomerStatus != null ? service.Customer.CustomerStatus.Name : null,
                });
            }

            return totalData;
        }
        #endregion

        #region Customer Potential vs. Actual Report
        [WebMethod]
        public byte[] LoadCustomerPotentialActualData(byte[] xmlFilters, int tenant)
        {
            CustomerPotentialActualDataProvider dataProviderData = LoadCustomerPotentialActualDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataProviderData, typeof(CustomerPotentialActualDataProvider), tenant);
        }

        private CustomerPotentialActualDataProvider LoadCustomerPotentialActualDataProvider(byte[] xmlFilters, int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            CustomerPotentialActualDataProvider myResult = new CustomerPotentialActualDataProvider();
            myResult.Customers = new List<CustomersData>();

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            #region Report Filters
            QueryFilterItem filterItem_TimeRange = queryOperations.QueryFilterItems.Where(d => d.FieldName == "TimeRange").FirstOrDefault();
            QueryFilterItem filterItem_DataTypeCode = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DataTypeCode").FirstOrDefault();
            QueryFilterItem filterItem_ProductsTypes = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ProductsTypes").FirstOrDefault();
            QueryFilterItem filterItem_OwnerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "OwnerId").FirstOrDefault();
            QueryFilterItem filterItem_BusinessUnitId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "BusinessUnitId").FirstOrDefault();
            QueryFilterItem filterItem_CountryId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CountryId").FirstOrDefault();
            QueryFilterItem filterItem_ProductCode = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ProductCode").FirstOrDefault();

            string timeRange = null;
            string dataTypeCode = null;
            string productsTypes = null;
            string ownerId = null;
            string businessUnitId = null;
            string countryId = null;
            string productCode = null;

            if (filterItem_TimeRange != null)
            {
                if (filterItem_TimeRange.FieldValue != null)
                {
                    timeRange = filterItem_TimeRange.FieldValue.ToString();
                }
            }

            if (filterItem_DataTypeCode != null)
            {
                if (filterItem_DataTypeCode.FieldValue != null)
                {
                    dataTypeCode = filterItem_DataTypeCode.FieldValue.ToString();
                }
            }

            if (filterItem_ProductsTypes != null)
            {
                if (filterItem_ProductsTypes.FieldValue != null)
                {
                    productsTypes = filterItem_ProductsTypes.FieldValue.ToString();
                }
            }

            if (filterItem_OwnerId != null)
            {
                if (filterItem_OwnerId.FieldValue != null)
                {
                    ownerId = filterItem_OwnerId.FieldValue.ToString();
                }
            }

            if (filterItem_BusinessUnitId != null)
            {
                if (filterItem_BusinessUnitId.FieldValue != null)
                {
                    businessUnitId = filterItem_BusinessUnitId.FieldValue.ToString();
                }
            }

            if (filterItem_CountryId != null)
            {
                if (filterItem_CountryId.FieldValue != null)
                {
                    countryId = filterItem_CountryId.FieldValue.ToString();
                }
            }

            if (filterItem_ProductCode != null)
            {
                if (filterItem_ProductCode.FieldValue != null)
                {
                    productCode = filterItem_ProductCode.FieldValue.ToString();
                }
            }
            #endregion

            #region Base Data Filtered
            CustomerBusinessUnitFilter myBusinessUnitFilter = new CustomerBusinessUnitFilter(tenant);

            ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);

            CustomerProductRepository customerProductRepository = new CustomerProductRepository(myCommonContext);
            CustomerProductActualDataRepository customerProductActualDataRepository = new CustomerProductActualDataRepository(myCommonContext);
            CustomerProductLocationRepository customerProductLocationRepository = new CustomerProductLocationRepository(myCommonContext);
            CustomerProductLocationActualDataRepository customerProductLocationActualDataRepository = new CustomerProductLocationActualDataRepository(myCommonContext);

            IQueryable<CustomerProduct> iQueryable_1 = customerProductRepository.GetCustomerProducts(tenant).Include("Customer").Include("Customer.Card").Include("Customer.SalesmanUser").Include("Customer.SalesmanUser.Contact").Include("Customer.Card.PrimaryContact");
            IQueryable<CustomerProductActualData> iQueryable_2 = customerProductActualDataRepository.GetCustomerProductActualDatas(tenant).Include("Customer").Include("Customer.Card").Include("Customer.SalesmanUser").Include("Customer.SalesmanUser.Contact").Include("Customer.Card.PrimaryContact");

            if (!string.IsNullOrEmpty(countryId))
            {
                iQueryable_1 = (from myCustomerProduct in myCommonContext.CustomerProducts
                                join db_CustomerProductLocations in myCommonContext.CustomerProductLocations
                                on myCustomerProduct.CustomerId equals db_CustomerProductLocations.CustomerId into CustomerProductsLocations
                                from myCustomerProductLocation in CustomerProductsLocations.DefaultIfEmpty()
                                where myCustomerProduct.Tenant == tenant
                                && myCustomerProductLocation.CountryId == countryId
                                select myCustomerProduct).Include("Customer").Include("Customer.Card").Include("Customer.SalesmanUser").Include("Customer.SalesmanUser.Contact").Include("Customer.Card.PrimaryContact");

                iQueryable_2 = (from myCustomerProduct in myCommonContext.CustomerProductActualDatas
                                join db_CustomerProductLocations in myCommonContext.CustomerProductLocationActualDatas
                                on myCustomerProduct.CustomerId equals db_CustomerProductLocations.CustomerId into CustomerProductsLocations
                                from myCustomerProductLocation in CustomerProductsLocations.DefaultIfEmpty()
                                where myCustomerProduct.Tenant == tenant
                                && myCustomerProductLocation.CountryId == countryId
                                select myCustomerProduct).Include("Customer").Include("Customer.Card").Include("Customer.SalesmanUser").Include("Customer.SalesmanUser.Contact").Include("Customer.Card.PrimaryContact");
            }

            iQueryable_1 = myBusinessUnitFilter.RunFilter(iQueryable_1);
            iQueryable_1 = iQueryable_1.Where(d => d.Customer.ActivityWatch);
            iQueryable_1 = iQueryable_1.Where(d => !d.Customer.Card.InActive);

            iQueryable_2 = myBusinessUnitFilter.RunFilter(iQueryable_2);
            iQueryable_2 = iQueryable_2.Where(d => d.Customer.ActivityWatch);
            iQueryable_2 = iQueryable_2.Where(d => !d.Customer.Card.InActive);

            if (!string.IsNullOrEmpty(ownerId))
            {
                iQueryable_1 = iQueryable_1.Where(d => d.Customer.SalesmanUserId == ownerId);
                iQueryable_2 = iQueryable_2.Where(d => d.Customer.SalesmanUserId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                iQueryable_1 = iQueryable_1.Where(d => d.Customer.SalesmanUser != null && d.Customer.SalesmanUser.BusinessUnitId == businessUnitId);
                iQueryable_2 = iQueryable_2.Where(d => d.Customer.SalesmanUser != null && d.Customer.SalesmanUser.BusinessUnitId == businessUnitId);
            }

            if (!string.IsNullOrEmpty(productsTypes))
            {
                List<string> myproductsTypesList = new List<string>();

                productsTypes = productsTypes.Replace(" ", "");

                if (productsTypes.ToLower() == "all")
                {
                    ProductTypeRepository productTypeRepository = new ProductTypeRepository(tenant);
                    IQueryable<ProductType> iQueryable = productTypeRepository.GetActiveProductTypes(tenant);
                    if (iQueryable.Count() > 0)
                    {
                        myproductsTypesList = iQueryable.Select(s => s.Code).ToList();
                    }
                }

                else
                {
                    productsTypes = productsTypes.Trim(',');

                    string[] myProductsTypes = productsTypes.Split(',');

                    myproductsTypesList = myProductsTypes.ToList();
                }

                if (myproductsTypesList.Count() > 0)
                {
                    iQueryable_1 = iQueryable_1.Where(d => myproductsTypesList.Contains(d.ProductTypeCode));
                    iQueryable_2 = iQueryable_2.Where(d => myproductsTypesList.Contains(d.ProductTypeCode));
                }
            }

            #endregion

            #region Build Same DataType

            int indexCounter = 0;
            List<ProductActualDataHelper> list1 = new List<ProductActualDataHelper>();
            List<ProductActualDataHelper> list2 = new List<ProductActualDataHelper>();

            if (iQueryable_1.Count() > 0)
            {
                indexCounter = 0;
                foreach (CustomerProduct item in iQueryable_1)
                {
                    IQueryable<CustomerProductLocation> locations = customerProductLocationRepository.GetCustomerProductLocations(item.CustomerId, item.ProductTypeCode, tenant);

                    ProductActualDataHelper myRecord = new ProductActualDataHelper()
                    {
                        Id = indexCounter,
                        CustomerId = item.CustomerId,
                        CustomerName = item.Customer == null ? "" : (item.Customer.Card == null ? "" : item.Customer.Card.EnglishName),
                        SalesmanId = item.Customer == null ? "" : item.Customer.SalesmanUserId,
                        SalesmanName = item.Customer == null ? "" : (item.Customer.SalesmanUser == null ? "" : item.Customer.SalesmanUser.Contact.EnglishName),
                        PrimaryContactName = item.Customer == null ? "" : (item.Customer.Card.PrimaryContact == null ? "" : item.Customer.Card.PrimaryContact.EnglishName),
                        PrimaryContactEmail = item.Customer == null ? "" : (item.Customer.Card.PrimaryContact == null ? "" : item.Customer.Card.PrimaryContact.EnglishName),
                        Date = null,
                        ProductCode = item.ProductTypeCode,
                        TEU = item.PotentialTEU,
                        Revenue = item.PotentialRevenue,
                        NumberOfShipments = item.PotentialNumberOfShipments,
                        ChargeableWeight = item.PotentialChargeableWeight,
                        LocationsCount = locations.Count(),
                    };

                    list1.Add(myRecord);
                    indexCounter++;
                }
            }

            if (iQueryable_2.Count() > 0)
            {
                indexCounter = 0;
                foreach (CustomerProductActualData item in iQueryable_2)
                {
                    IQueryable<CustomerProductLocationActualData> locations = customerProductLocationActualDataRepository.GetCustomerProductLocationActualDatas(tenant);
                    locations = locations.Where(d => d.CustomerId == item.CustomerId && d.ProductTypeCode == item.ProductTypeCode);

                    ProductActualDataHelper myRecord = new ProductActualDataHelper()
                    {
                        Id = indexCounter,
                        CustomerId = item.CustomerId,
                        CustomerName = item.Customer == null ? "" : (item.Customer.Card == null ? "" : item.Customer.Card.EnglishName),
                        SalesmanId = item.Customer == null ? "" : item.Customer.SalesmanUserId,
                        SalesmanName = item.Customer == null ? "" : (item.Customer.SalesmanUser == null ? "" : item.Customer.SalesmanUser.Contact.EnglishName),
                        PrimaryContactName = item.Customer == null ? "" : (item.Customer.Card.PrimaryContact == null ? "" : item.Customer.Card.PrimaryContact.EnglishName),
                        PrimaryContactEmail = item.Customer == null ? "" : (item.Customer.Card.PrimaryContact == null ? "" : item.Customer.Card.PrimaryContact.EnglishName),
                        Date = new DateTime(item.Year, item.Month, 1),
                        ProductCode = item.ProductTypeCode,
                        TEU = item.TEU,
                        Revenue = item.Revenue,
                        NumberOfShipments = item.NumberOfShipments,
                        ChargeableWeight = item.ChargeableWeight,
                        LocationsCount = locations.Count(),
                    };

                    list2.Add(myRecord);
                    indexCounter++;
                }
            }
            #endregion

            #region Union
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime todayDate = new DateTime(todayDateTime.Year, todayDateTime.Month, 1);

            int numberOfMonths = 1;
            if (timeRange == "L3M")
            {
                numberOfMonths = 3;
            }
            else if (timeRange == "L12M")
            {
                numberOfMonths = 12;
            }

            DateTime startDate = todayDate.AddMonths(-1 * numberOfMonths);

            List<CompareDataClass> myList = new List<CompareDataClass>();
            myList = (

                (from a in list1
                 group a by new { a.CustomerId, a.CustomerName, a.SalesmanId, a.SalesmanName, a.ProductCode, a.PrimaryContactName, a.PrimaryContactEmail } into g1
                 select new CompareDataClass()
                 {
                     Id = g1.Key.CustomerId,
                     EntityId = g1.Key.CustomerId,
                     EntityName = g1.Key.CustomerName,
                     SalesmanId = g1.Key.SalesmanId,
                     SalesmanName = g1.Key.SalesmanName,
                     PrimaryContactName = g1.Key.PrimaryContactName,
                     PrimaryContactEmail = g1.Key.PrimaryContactEmail,
                     ProductCode = g1.Key.ProductCode,

                     TEU_Potential = g1.Sum(s => s.TEU),
                     Revenue_Potential = g1.Sum(s => s.Revenue),
                     ChargeableWeight_Potential = g1.Sum(s => s.ChargeableWeight),
                     NumberOfShipments_Potential = g1.Sum(s => s.NumberOfShipments),

                     TEU_Actual = 0,
                     Revenue_Actual = 0,
                     ChargeableWeight_Actual = 0,
                     NumberOfShipments_Actual = 0,

                     LocationsCount_Potential = g1.Sum(s => s.LocationsCount),
                     LocationsCount_Actual = 0,
                 })

                 .Union

                 (from b in list2
                  where b.Date >= startDate && b.Date < todayDate
                  group b by new { b.CustomerId, b.CustomerName, b.SalesmanId, b.SalesmanName, b.ProductCode, b.PrimaryContactName, b.PrimaryContactEmail } into g2
                  select new CompareDataClass()
                  {
                      Id = g2.Key.CustomerId,
                      EntityId = g2.Key.CustomerId,
                      EntityName = g2.Key.CustomerName,
                      SalesmanId = g2.Key.SalesmanId,
                      SalesmanName = g2.Key.SalesmanName,
                      PrimaryContactName = g2.Key.PrimaryContactName,
                      PrimaryContactEmail = g2.Key.PrimaryContactEmail,
                      ProductCode = g2.Key.ProductCode,

                      TEU_Potential = 0,
                      Revenue_Potential = 0,
                      ChargeableWeight_Potential = 0,
                      NumberOfShipments_Potential = 0,

                      TEU_Actual = MethodHelper.Round(g2.Sum(s => s.TEU) / numberOfMonths, 2),
                      Revenue_Actual = MethodHelper.Round(g2.Sum(s => s.Revenue) / numberOfMonths, 2),
                      ChargeableWeight_Actual = MethodHelper.Round(g2.Sum(s => s.ChargeableWeight) / numberOfMonths, 2),
                      NumberOfShipments_Actual = MethodHelper.Round(g2.Sum(s => s.NumberOfShipments) / numberOfMonths, 2),

                      LocationsCount_Potential = 0,
                      LocationsCount_Actual = g2.Sum(s => s.LocationsCount),
                  })
                  )

                  .GroupBy(d => new { d.Id, d.EntityName, d.SalesmanId, d.SalesmanName, d.ProductCode, d.PrimaryContactName, d.PrimaryContactEmail })

                  .Select(s => new CompareDataClass()
                  {
                      Id = s.Key.Id,
                      EntityId = s.Key.Id,
                      EntityName = s.Key.EntityName,
                      SalesmanId = s.Key.SalesmanId,
                      SalesmanName = s.Key.SalesmanName,
                      PrimaryContactName = s.Key.PrimaryContactName,
                      PrimaryContactEmail = s.Key.PrimaryContactEmail,
                      ProductCode = s.Key.ProductCode,

                      TEU_Potential = s.Sum(k => k.TEU_Potential),
                      Revenue_Potential = s.Sum(k => k.Revenue_Potential),
                      ChargeableWeight_Potential = s.Sum(k => k.ChargeableWeight_Potential),
                      NumberOfShipments_Potential = s.Sum(k => k.NumberOfShipments_Potential),

                      TEU_Actual = s.Sum(k => k.TEU_Actual),
                      Revenue_Actual = s.Sum(k => k.Revenue_Actual),
                      ChargeableWeight_Actual = s.Sum(k => k.ChargeableWeight_Actual),
                      NumberOfShipments_Actual = s.Sum(k => k.NumberOfShipments_Actual),

                      LocationsCount_Potential = s.Sum(k => k.LocationsCount_Potential),
                      LocationsCount_Actual = s.Sum(k => k.LocationsCount_Actual),
                  }).ToList();

            #endregion

            #region Group
            var myGroup = (from d in myList
                           group d by new { d.Id, d.EntityName, d.SalesmanName, d.SalesmanId, d.PrimaryContactName, d.PrimaryContactEmail } into go
                           select new
                           {
                               Id = go.Key.Id,
                               EntityName = go.Key.EntityName,
                               SalesmanId = go.Key.SalesmanId,
                               SalesmanName = go.Key.SalesmanName,
                               PrimaryContactName = go.Key.PrimaryContactName,
                               PrimaryContactEmail = go.Key.PrimaryContactEmail,
                               TotalNumberOfShipments = go.Sum(d => d.NumberOfShipments_Potential) + go.Sum(d => d.NumberOfShipments_Actual),
                               TotalTEU = go.Sum(d => d.TEU_Potential) + go.Sum(d => d.TEU_Actual),
                               TotalRevenue = go.Sum(d => d.Revenue_Potential) + go.Sum(d => d.Revenue_Actual),
                               TotalChargeableWeight = go.Sum(d => d.ChargeableWeight_Potential) + go.Sum(d => d.ChargeableWeight_Actual),
                               TotalLocationsCount = go.Sum(d => d.LocationsCount_Potential) + go.Sum(d => d.LocationsCount_Actual),
                           });
            #endregion

            #region Fill Data
            foreach (var item in myGroup.OrderBy(d => d.EntityName))
            {
                CustomersData myRecord = null;

                switch (dataTypeCode)
                {
                    case "TEU":
                        {
                            myRecord = new CustomersData()
                            {
                                CustomerId = item.Id,
                                CustomerName = item.EntityName,
                                Salesman = item.SalesmanName,
                                PrimaryContactName = item.PrimaryContactName,
                                PrimaryContactEmail = item.PrimaryContactEmail,
                                LocationsCount = item.TotalLocationsCount,
                                AD_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AD").Sum(s => s.TEU_Actual),
                                AR_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AR").Sum(s => s.TEU_Actual),
                                AE_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AE").Sum(s => s.TEU_Actual),
                                AI_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AI").Sum(s => s.TEU_Actual),
                                ID_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "ID").Sum(s => s.TEU_Actual),
                                IR_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IR").Sum(s => s.TEU_Actual),
                                IE_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IE").Sum(s => s.TEU_Actual),
                                II_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "II").Sum(s => s.TEU_Actual),
                                OD_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OD").Sum(s => s.TEU_Actual),
                                OR_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OR").Sum(s => s.TEU_Actual),
                                OE_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OE").Sum(s => s.TEU_Actual),
                                OI_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OI").Sum(s => s.TEU_Actual),
                                CI_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "CI").Sum(s => s.TEU_Actual),
                                DL_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "DL").Sum(s => s.TEU_Actual),
                                IN_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IN").Sum(s => s.TEU_Actual),
                                AD_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AD").Sum(s => s.TEU_Potential),
                                AR_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AR").Sum(s => s.TEU_Potential),
                                AE_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AE").Sum(s => s.TEU_Potential),
                                AI_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AI").Sum(s => s.TEU_Potential),
                                ID_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "ID").Sum(s => s.TEU_Potential),
                                IR_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IR").Sum(s => s.TEU_Potential),
                                IE_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IE").Sum(s => s.TEU_Potential),
                                II_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "II").Sum(s => s.TEU_Potential),
                                OD_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OD").Sum(s => s.TEU_Potential),
                                OR_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OR").Sum(s => s.TEU_Potential),
                                OE_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OE").Sum(s => s.TEU_Potential),
                                OI_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OI").Sum(s => s.TEU_Potential),
                                CI_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "CI").Sum(s => s.TEU_Potential),
                                DL_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "DL").Sum(s => s.TEU_Potential),
                                IN_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IN").Sum(s => s.TEU_Potential),
                            };
                            break;
                        }

                    case "REV":
                        {
                            myRecord = new CustomersData()
                            {
                                CustomerId = item.Id,
                                CustomerName = item.EntityName,
                                Salesman = item.SalesmanName,
                                PrimaryContactName = item.PrimaryContactName,
                                PrimaryContactEmail = item.PrimaryContactEmail,
                                LocationsCount = item.TotalLocationsCount,
                                AD_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AD").Sum(s => s.Revenue_Actual),
                                AR_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AR").Sum(s => s.Revenue_Actual),
                                AE_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AE").Sum(s => s.Revenue_Actual),
                                AI_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AI").Sum(s => s.Revenue_Actual),
                                ID_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "ID").Sum(s => s.Revenue_Actual),
                                IR_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IR").Sum(s => s.Revenue_Actual),
                                IE_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IE").Sum(s => s.Revenue_Actual),
                                II_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "II").Sum(s => s.Revenue_Actual),
                                OD_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OD").Sum(s => s.Revenue_Actual),
                                OR_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OR").Sum(s => s.Revenue_Actual),
                                OE_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OE").Sum(s => s.Revenue_Actual),
                                OI_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OI").Sum(s => s.Revenue_Actual),
                                CI_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "CI").Sum(s => s.Revenue_Actual),
                                DL_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "DL").Sum(s => s.Revenue_Actual),
                                IN_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IN").Sum(s => s.Revenue_Actual),
                                AD_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AD").Sum(s => s.Revenue_Potential),
                                AR_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AR").Sum(s => s.Revenue_Potential),
                                AE_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AE").Sum(s => s.Revenue_Potential),
                                AI_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AI").Sum(s => s.Revenue_Potential),
                                ID_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "ID").Sum(s => s.Revenue_Potential),
                                IR_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IR").Sum(s => s.Revenue_Potential),
                                IE_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IE").Sum(s => s.Revenue_Potential),
                                II_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "II").Sum(s => s.Revenue_Potential),
                                OD_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OD").Sum(s => s.Revenue_Potential),
                                OR_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OR").Sum(s => s.Revenue_Potential),
                                OE_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OE").Sum(s => s.Revenue_Potential),
                                OI_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OI").Sum(s => s.Revenue_Potential),
                                CI_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "CI").Sum(s => s.Revenue_Potential),
                                DL_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "DL").Sum(s => s.Revenue_Potential),
                                IN_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IN").Sum(s => s.Revenue_Potential),
                            };
                            break;
                        }

                    case "NSH":
                        {
                            myRecord = new CustomersData()
                            {
                                CustomerId = item.Id,
                                CustomerName = item.EntityName,
                                Salesman = item.SalesmanName,
                                PrimaryContactName = item.PrimaryContactName,
                                PrimaryContactEmail = item.PrimaryContactEmail,
                                LocationsCount = item.TotalLocationsCount,
                                AD_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AD").Sum(s => s.NumberOfShipments_Actual),
                                AR_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AR").Sum(s => s.NumberOfShipments_Actual),
                                AE_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AE").Sum(s => s.NumberOfShipments_Actual),
                                AI_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AI").Sum(s => s.NumberOfShipments_Actual),
                                ID_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "ID").Sum(s => s.NumberOfShipments_Actual),
                                IR_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IR").Sum(s => s.NumberOfShipments_Actual),
                                IE_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IE").Sum(s => s.NumberOfShipments_Actual),
                                II_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "II").Sum(s => s.NumberOfShipments_Actual),
                                OD_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OD").Sum(s => s.NumberOfShipments_Actual),
                                OR_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OR").Sum(s => s.NumberOfShipments_Actual),
                                OE_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OE").Sum(s => s.NumberOfShipments_Actual),
                                OI_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OI").Sum(s => s.NumberOfShipments_Actual),
                                CI_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "CI").Sum(s => s.NumberOfShipments_Actual),
                                DL_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "DL").Sum(s => s.NumberOfShipments_Actual),
                                IN_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IN").Sum(s => s.NumberOfShipments_Actual),
                                AD_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AD").Sum(s => s.NumberOfShipments_Potential),
                                AR_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AR").Sum(s => s.NumberOfShipments_Potential),
                                AE_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AE").Sum(s => s.NumberOfShipments_Potential),
                                AI_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AI").Sum(s => s.NumberOfShipments_Potential),
                                ID_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "ID").Sum(s => s.NumberOfShipments_Potential),
                                IR_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IR").Sum(s => s.NumberOfShipments_Potential),
                                IE_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IE").Sum(s => s.NumberOfShipments_Potential),
                                II_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "II").Sum(s => s.NumberOfShipments_Potential),
                                OD_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OD").Sum(s => s.NumberOfShipments_Potential),
                                OR_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OR").Sum(s => s.NumberOfShipments_Potential),
                                OE_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OE").Sum(s => s.NumberOfShipments_Potential),
                                OI_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OI").Sum(s => s.NumberOfShipments_Potential),
                                CI_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "CI").Sum(s => s.NumberOfShipments_Potential),
                                DL_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "DL").Sum(s => s.NumberOfShipments_Potential),
                                IN_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IN").Sum(s => s.NumberOfShipments_Potential),
                            };
                            break;
                        }

                    case "CHW":
                        {
                            myRecord = new CustomersData()
                            {
                                CustomerId = item.Id,
                                CustomerName = item.EntityName,
                                Salesman = item.SalesmanName,
                                PrimaryContactName = item.PrimaryContactName,
                                PrimaryContactEmail = item.PrimaryContactEmail,
                                LocationsCount = item.TotalLocationsCount,
                                AD_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AD").Sum(s => s.ChargeableWeight_Actual),
                                AR_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AR").Sum(s => s.ChargeableWeight_Actual),
                                AE_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AE").Sum(s => s.ChargeableWeight_Actual),
                                AI_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AI").Sum(s => s.ChargeableWeight_Actual),
                                ID_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "ID").Sum(s => s.ChargeableWeight_Actual),
                                IR_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IR").Sum(s => s.ChargeableWeight_Actual),
                                IE_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IE").Sum(s => s.ChargeableWeight_Actual),
                                II_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "II").Sum(s => s.ChargeableWeight_Actual),
                                OD_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OD").Sum(s => s.ChargeableWeight_Actual),
                                OR_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OR").Sum(s => s.ChargeableWeight_Actual),
                                OE_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OE").Sum(s => s.ChargeableWeight_Actual),
                                OI_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OI").Sum(s => s.ChargeableWeight_Actual),
                                CI_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "CI").Sum(s => s.ChargeableWeight_Actual),
                                DL_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "DL").Sum(s => s.ChargeableWeight_Actual),
                                IN_ACT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IN").Sum(s => s.ChargeableWeight_Actual),
                                AD_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AD").Sum(s => s.ChargeableWeight_Potential),
                                AR_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AR").Sum(s => s.ChargeableWeight_Potential),
                                AE_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AE").Sum(s => s.ChargeableWeight_Potential),
                                AI_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "AI").Sum(s => s.ChargeableWeight_Potential),
                                ID_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "ID").Sum(s => s.ChargeableWeight_Potential),
                                IR_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IR").Sum(s => s.ChargeableWeight_Potential),
                                IE_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IE").Sum(s => s.ChargeableWeight_Potential),
                                II_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "II").Sum(s => s.ChargeableWeight_Potential),
                                OD_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OD").Sum(s => s.ChargeableWeight_Potential),
                                OR_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OR").Sum(s => s.ChargeableWeight_Potential),
                                OE_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OE").Sum(s => s.ChargeableWeight_Potential),
                                OI_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "OI").Sum(s => s.ChargeableWeight_Potential),
                                CI_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "CI").Sum(s => s.ChargeableWeight_Potential),
                                DL_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "DL").Sum(s => s.ChargeableWeight_Potential),
                                IN_POT = myList.Where(d => d.Id == item.Id && d.ProductCode == "IN").Sum(s => s.ChargeableWeight_Potential),
                            };
                            break;
                        }
                }

                if (myRecord != null)
                {
                    if (productCode.ToLower() == "all")
                    {
                        myResult.Customers.Add(myRecord);
                    }

                    else
                    {
                        decimal? potential = myRecord.AD_POT + myRecord.AR_POT + myRecord.AE_POT + myRecord.AI_POT + myRecord.ID_POT + myRecord.IR_POT + myRecord.IE_POT + myRecord.II_POT
                            + myRecord.OD_POT + myRecord.OR_POT + myRecord.OE_POT + myRecord.OI_POT + myRecord.CI_POT + myRecord.DL_POT + myRecord.IN_POT;

                        decimal? actual = myRecord.AD_ACT + myRecord.AR_ACT + myRecord.AE_ACT + myRecord.AI_ACT + myRecord.ID_ACT + myRecord.IR_ACT + myRecord.IE_ACT + myRecord.II_ACT
                           + myRecord.OD_ACT + myRecord.OR_ACT + myRecord.OE_ACT + myRecord.OI_ACT + myRecord.CI_ACT + myRecord.DL_ACT + myRecord.IN_ACT;

                        decimal? none = potential + actual;

                        if (productCode.ToLower() == "pot")
                        {
                            if (potential > 0 && actual == 0 && myRecord.LocationsCount > 0)
                            {
                                myResult.Customers.Add(myRecord);
                            }
                        }

                        else if (productCode.ToLower() == "act" && myRecord.LocationsCount > 0)
                        {
                            if (potential == 0 && actual > 0)
                            {
                                myResult.Customers.Add(myRecord);
                            }
                        }

                        else if (productCode.ToLower() == "non")
                        {
                            if (none == 0)
                            {
                                myResult.Customers.Add(myRecord);
                            }
                        }
                    }
                }
            }
            #endregion

            return myResult;
        }
        #endregion

        #region Statistics by Agent Report
        [WebMethod]
        public byte[] LoadStatisticsByAgentData(byte[] xmlFilters, int tenant)
        {
            StatisticsByAgentDataProvider dataprovider = LoadStatisticsByAgentProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(StatisticsByAgentDataProvider), tenant);
        }

        public StatisticsByAgentDataProvider LoadStatisticsByAgentProvider(byte[] xmlFilters, int tenant)
        {
            StatisticsByAgentDataProvider dataProvider = new StatisticsByAgentDataProvider();

            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            IQueryable<ShipmentDataView> iQueryable = shipmentRepository.GetShipmentViewsByTenant(tenant);

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_IsByCreateDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IsByCreateDate").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_AgentId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "AgentId").FirstOrDefault();
            QueryFilterItem filterItem_CurrencyCodeType = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CurrencyCodeType").FirstOrDefault();
            QueryFilterItem filterItem_IncludeOperationalyClosed = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeOperationalyClosed").FirstOrDefault();
            QueryFilterItem filterItem_Direction = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Direction").FirstOrDefault();
            QueryFilterItem filterItem_TransportMode = queryOperations.QueryFilterItems.Where(d => d.FieldName == "TransportMode").FirstOrDefault();
            QueryFilterItem filterItem_ShipmentLevel = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ShipmentLevel").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);

            DateTime fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            DateTime toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));
            string AgentId = null;
            string CurrencyCodeType = null;
            bool IncludeOperationalyClosed = false;
            bool isByCreateDate = true;
            string Direction = null;
            string TransportMode = null;
            string ShipmentLevel = null;

            if (filterItem_IsByCreateDate != null)
            {
                if (filterItem_IsByCreateDate.FieldValue != null)
                {
                    isByCreateDate = (bool)filterItem_IsByCreateDate.FieldValue;
                }
            }

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);
            }

            if (filterItem_ToDate != null)
            {
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
            }

            if (filterItem_AgentId != null)
            {
                if (filterItem_AgentId.FieldValue != null)
                {
                    AgentId = filterItem_AgentId.FieldValue.ToString();
                }
            }

            if (filterItem_CurrencyCodeType != null)
            {
                if (filterItem_CurrencyCodeType.FieldValue != null)
                {
                    CurrencyCodeType = filterItem_CurrencyCodeType.FieldValue.ToString();
                }
            }

            if (filterItem_IncludeOperationalyClosed != null)
            {
                if (filterItem_IncludeOperationalyClosed.FieldValue != null)
                {
                    IncludeOperationalyClosed = (bool)filterItem_IncludeOperationalyClosed.FieldValue;
                }
            }

            if (filterItem_Direction != null)
            {
                if (filterItem_Direction.FieldValue != null)
                {
                    Direction = filterItem_Direction.FieldValue.ToString();
                }
            }

            if (filterItem_TransportMode != null)
            {
                if (filterItem_TransportMode.FieldValue != null)
                {
                    TransportMode = filterItem_TransportMode.FieldValue.ToString();
                }
            }

            if (filterItem_ShipmentLevel != null)
            {
                if (filterItem_ShipmentLevel.FieldValue != null)
                {
                    ShipmentLevel = filterItem_ShipmentLevel.FieldValue.ToString();
                }
            }
            #endregion

            #region Base Data Filtered

            if (isByCreateDate)
            {
                if (fromDate != null)
                {
                    iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                }

                if (toDate != null)
                {
                    iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                }
            }

            else
            {
                if (fromDate != null)
                {
                    iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OperationalDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                }

                if (toDate != null)
                {
                    iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OperationalDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                }
            }

            if (!string.IsNullOrEmpty(AgentId))
            {
                iQueryable = iQueryable.Where(d => d.AgentComputed == AgentId);

                Card agent = CardRepository.GetSingleCard(AgentId, tenant, true);
                dataProvider.Agent = agent.EnglishName;
            }
            else
            {
                dataProvider.Agent = "All agents";
            }

            if (!IncludeOperationalyClosed)
            {
                iQueryable = iQueryable.Where(d => !d.IsOperationalClosed);
            }

            if (!string.IsNullOrEmpty(Direction))
            {
                iQueryable = iQueryable.Where(d => d.DirectionId == Direction);
            }

            if (!string.IsNullOrEmpty(TransportMode))
            {
                iQueryable = iQueryable.Where(d => d.TransportModeId == TransportMode);
            }

            if (!string.IsNullOrEmpty(ShipmentLevel))
            {
                if (ShipmentLevel.ToLower() == "shipments")
                {
                    iQueryable = iQueryable.Where(d => d.ShipmentLevelCode == "D" || d.ShipmentLevelCode == "H");
                }

                else
                {
                    iQueryable = iQueryable.Where(d => d.ShipmentLevelCode == "D" || d.ShipmentLevelCode == "C");
                }
            }

            #endregion

            #region General Data
            dataProvider.Name = @"Statistics By Agent";
            string[] currencyarray = CurrencyCodeType.Split(',');
            dataProvider.FromPeriod = fromDate;
            dataProvider.ToPeriod = toDate;
            dataProvider.Currency = currencyarray[0];

            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);

            if (currentTenant != null)
            {
                dataProvider.TenantName = currentTenant.Company;
                dataProvider.Signature = currentTenant.Signature;
                dataProvider.Logo = WebFreight.Web.DataProviders.General.GetLogo(currentTenant.Id);

                AddressQuery addressQuery = new AddressQuery(tenant);
                AddressPM address = addressQuery.GetSingleAddressPM(currentTenant.AddressId, currentTenant.Id, false);
                if (address != null)
                {
                    dataProvider.Address1 = address.Address1;
                    dataProvider.Address2 = address.Address2;
                    dataProvider.City = address.City;
                    dataProvider.Country = address.CountryName;
                    dataProvider.TenantFax = address.FaxNumber;
                    dataProvider.TenantPhone = address.PhoneNumber;
                    dataProvider.State = address.StateEnglishName;
                    dataProvider.ZipCode = address.ZipCode;
                }
            }
            #endregion

            #region Fill Data
            List<StatisticsByAgentReport> tempList = new List<StatisticsByAgentReport>();

            if (iQueryable.Count() > 0)
            {
                foreach (ShipmentDataView shipment in iQueryable)
                {
                    StatisticsByAgentReport record = new StatisticsByAgentReport();

                    record.AgentId = shipment.AgentComputed;
                    record.AgentName = shipment.AgentName;

                    record.TransportMode = shipment.TransportModeName;
                    record.Direction = shipment.DirectionName;
                    record.TransmodeDirection = shipment.DirectionName + shipment.TransportModeName;
                    record.GrossWeight = shipment.GrossWeight;
                    record.Volume = shipment.Volume;
                    record.TEU = shipment.TEU;

                    if (currencyarray[1] == "profit")
                    {
                        record.Payables = shipment.OpenPayablesInProfitCurrency + shipment.AccountedPayablesInProfitCurrency;
                        record.Receivables = shipment.OpenReceivablesInProfitCurrency + shipment.AccountedReceivablesInProfitCurrency;
                        record.Profit = shipment.ProfitInProfitCurrency;

                    }
                    else if (currencyarray[1] == "local")
                    {
                        record.Payables = shipment.OpenPayablesInLocalCurrency + shipment.AccountedPayablesInLocalCurrency;
                        record.Receivables = shipment.OpenReceivablesInLocalCurrency + shipment.AccountedReceivablesInLocalCurrency;
                        record.Profit = shipment.ProfitInLocalCurrency;
                    }

                    tempList.Add(record);
                }
            }
            #endregion

            #region Group
            tempList = tempList.OrderBy(or => or.AgentName).ToList();

            List<StatisticsByAgentGroup> finalResults = (from p in tempList
                                                         group p by new { p.AgentId, p.AgentName } into g
                                                         select new StatisticsByAgentGroup()
                                                         {
                                                             AgentId = g.Key.AgentId,
                                                             AgentName = g.Key.AgentName,
                                                             StatisticsRecordList = g.ToList(),
                                                         }).ToList();

            dataProvider.StatisticsGroupList = finalResults.OrderBy(d => d.AgentName).ToList();

            foreach (StatisticsByAgentGroup item in dataProvider.StatisticsGroupList)
            {
                List<StatisticsByAgentReport> myList = (from a in item.StatisticsRecordList
                                                        group a by new
                                                        {
                                                            a.TransportMode,
                                                            a.Direction,
                                                        } into gr
                                                        orderby gr.Key.TransportMode
                                                        select new StatisticsByAgentReport()
                                                        {
                                                            GrossWeight = gr.Sum(d => d.GrossWeight),
                                                            TEU = gr.Sum(t => t.TEU),
                                                            TransmodeDirection = gr.Key.Direction + gr.Key.TransportMode,
                                                            NumberOfShipments = gr.Count(),
                                                            Payables = gr.Sum(d => d.Payables),
                                                            Profit = gr.Sum(d => d.Profit),
                                                            Receivables = gr.Sum(d => d.Receivables),
                                                            Volume = gr.Sum(d => d.Volume),
                                                        }).ToList();

                item.StatisticsRecordList = myList;

                StatisticsByAgentReport totalRecord = new StatisticsByAgentReport();
                totalRecord.TransmodeDirection = "Total";
                totalRecord.NumberOfShipments = item.StatisticsRecordList.Sum(d => d.NumberOfShipments);
                totalRecord.GrossWeight = item.StatisticsRecordList.Sum(d => d.GrossWeight);
                totalRecord.Volume = item.StatisticsRecordList.Sum(d => d.Volume);
                totalRecord.TEU = item.StatisticsRecordList.Sum(d => d.TEU);
                totalRecord.Receivables = item.StatisticsRecordList.Sum(d => d.Receivables);
                totalRecord.Payables = item.StatisticsRecordList.Sum(d => d.Payables);
                totalRecord.Profit = item.StatisticsRecordList.Sum(d => d.Profit);

                item.StatisticsRecordList.Add(totalRecord);
            }
            #endregion

            return dataProvider;
        }
        #endregion

        #region e-Booking Report
        [WebMethod]
        public byte[] LoadBookingsData(byte[] xmlFilters, int tenant)
        {
            BookingsDataProvider dataprovider = GetBookingsDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(BookingsDataProvider), tenant);
        }

        public BookingsDataProvider GetBookingsDataProvider(byte[] xmlFilters, int tenant)
        {
            BookingsDataProvider totalData = new BookingsDataProvider();
            totalData.BookingRecordList = new List<BookingRecord>();

            PortRepository portRep = new PortRepository(tenant);
            ParticipantRepository ParticipantRep = new ParticipantRepository(tenant);
            TenantRepository tenantRep = new TenantRepository(tenant);
            AirlineStatisticsRepository statisticsRep = new AirlineStatisticsRepository(tenant);

            IQueryable<AirlineStatistics> iQueryable = statisticsRep.GetAirlineStatistics(tenant).Where(d => d.MessageType == "FFR");

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_FromPortId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "MainCarriageFromPortId").FirstOrDefault();
            QueryFilterItem filterItem_FinalDestinationPortId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "MainCarriageFinalDestinationPortId").FirstOrDefault();
            QueryFilterItem filterItem_CustomerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);

            DateTime fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            DateTime toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));

            string fromPortId = null;
            string finalDestinationPortId = null;
            string customerId = null;

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);
            }

            if (filterItem_ToDate != null)
            {
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
            }

            if (filterItem_FromPortId != null)
            {
                if (filterItem_FromPortId.FieldValue != null)
                {
                    fromPortId = filterItem_FromPortId.FieldValue.ToString();
                }
            }

            if (filterItem_FinalDestinationPortId != null)
            {
                if (filterItem_FinalDestinationPortId.FieldValue != null)
                {
                    finalDestinationPortId = filterItem_FinalDestinationPortId.FieldValue.ToString();
                }
            }

            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    customerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }

            #endregion

            #region Base Data Filtered

            iQueryable = iQueryable.Where(d => d.IsCancelled == false);

            if (fromDate != null)
            {
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.EntitiyCreateDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
            }

            if (toDate != null)
            {
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.EntitiyCreateDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
            }

            if (!string.IsNullOrEmpty(fromPortId))
            {
                Port port = portRep.GetSinglePort(tenant, fromPortId);

                if (port != null)
                {
                    iQueryable = iQueryable.Where(d => d.OriginCode == port.Code);
                }
            }

            if (!string.IsNullOrEmpty(finalDestinationPortId))
            {
                Port port = portRep.GetSinglePort(tenant, finalDestinationPortId);

                if (port != null)
                {
                    iQueryable = iQueryable.Where(d => d.DestinationCode == port.Code);
                }
            }

            if (string.IsNullOrEmpty(customerId))
            {
                IQueryable<Participant> myParticipants = ParticipantRep.GetParticipants(tenant);
                List<int> forwarderTennats = new List<int>();
                foreach (Participant item in myParticipants)
                {
                    forwarderTennats.Add(item.ForwarderTenant);
                }

                iQueryable = iQueryable.Where(d => forwarderTennats.Contains(d.SourceTenant));
            }

            else
            {
                int selectedTenantId = Convert.ToInt32(customerId);
                iQueryable = iQueryable.Where(d => d.SourceTenant == selectedTenantId);
            }

            #endregion

            #region Fill Report Data

            totalData.FromDate = fromDate;
            totalData.ToDate = toDate;
            totalData.Logo = WebFreight.Web.DataProviders.General.GetLogo(tenant);

            if (!string.IsNullOrEmpty(customerId))
            {
                int selectedTenant = Convert.ToInt32(customerId);
                Tenant selectedCustomer = tenantRep.GetSingleTenant(selectedTenant);

                if (selectedCustomer != null)
                {
                    totalData.SelectedCustomerName = selectedCustomer.Company;
                    totalData.SelectedCustomerLabel = string.IsNullOrEmpty(selectedCustomer.Company) ? "" : "Customer:";
                }
            }

            if (iQueryable.Count() > 0)
            {
                BookingRecord bookingRecord = null;

                foreach (AirlineStatistics a in iQueryable)
                {
                    bookingRecord = new BookingRecord();

                    bookingRecord.CreateDate = a.CreateDate;
                    bookingRecord.CreatedByUser = a.EntityCreatedByUserName;
                    bookingRecord.BookingNumber = a.EntityReference;
                    bookingRecord.CustomerId = a.SourceTenant.ToString();
                    bookingRecord.CustomerName = a.SourceTenantName;
                    bookingRecord.Prefix = a.AirlinePrefix;
                    bookingRecord.AWBNumber = a.AWBNumber;
                    bookingRecord.Weight = a.GrossWeight;
                    bookingRecord.Status = a.EntityStatus;
                    bookingRecord.Product = a.ProductName;
                    bookingRecord.BookedByUser = a.Sender;
                    bookingRecord.Origin = a.OriginCode;
                    bookingRecord.Destination = a.DestinationCode;
                    bookingRecord.BookedDate = a.LastSentDate;

                    totalData.BookingRecordList.Add(bookingRecord);
                }
            }

            #endregion

            totalData.BookingRecordList = totalData.BookingRecordList.OrderBy(d => d.CustomerName).ToList();

            return totalData;
        }
        #endregion

        #region e-AWBs Report
        [WebMethod]
        public byte[] LoadEAWBsData(byte[] xmlFilters, int tenant)
        {
            EAWBsDataProvider dataprovider = GetAwbsDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(EAWBsDataProvider), tenant);
        }

        private EAWBsDataProvider GetAwbsDataProvider(byte[] xmlFilters, int tenant)
        {
            EAWBsDataProvider totalData = new EAWBsDataProvider();
            totalData.AWBsRecordList = new List<AWBRecord>();

            PortRepository portRep = new PortRepository(tenant);
            ParticipantRepository ParticipantRep = new ParticipantRepository(tenant);
            TenantRepository tenantRep = new TenantRepository(tenant);
            AirlineStatisticsRepository statisticsRep = new AirlineStatisticsRepository(tenant);

            IQueryable<AirlineStatistics> iQueryable = statisticsRep.GetAirlineStatistics(tenant).Where(d => d.MessageType == "FWB");

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_FromPortId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "MainCarriageFromPortId").FirstOrDefault();
            QueryFilterItem filterItem_FinalDestinationPortId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "MainCarriageFinalDestinationPortId").FirstOrDefault();
            QueryFilterItem filterItem_CustomerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();

            string fromPortId = null;
            string finalDestinationPortId = null;
            string customerId = null;

            if (filterItem_FromPortId != null)
            {
                if (filterItem_FromPortId.FieldValue != null)
                {
                    fromPortId = filterItem_FromPortId.FieldValue.ToString();
                }
            }

            if (filterItem_FinalDestinationPortId != null)
            {
                if (filterItem_FinalDestinationPortId.FieldValue != null)
                {
                    finalDestinationPortId = filterItem_FinalDestinationPortId.FieldValue.ToString();
                }
            }

            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    customerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }

            #endregion

            #region Base Data Filtered
            if (filterItem_FromDate != null)
            {
                DateTime fromDate;
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);
                if (fromDate != null)
                {
                    totalData.FromDate = fromDate;
                    iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.EntitiyCreateDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                }
            }

            if (filterItem_ToDate != null)
            {
                DateTime toDate;
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
                if (toDate != null)
                {
                    totalData.ToDate = toDate;
                    iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.EntitiyCreateDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                }
            }

            if (!string.IsNullOrEmpty(fromPortId))
            {
                Port port = portRep.GetSinglePort(tenant, fromPortId);

                if (port != null)
                {
                    iQueryable = iQueryable.Where(d => d.OriginCode == port.Code);
                }
            }

            if (!string.IsNullOrEmpty(finalDestinationPortId))
            {
                Port port = portRep.GetSinglePort(tenant, finalDestinationPortId);

                if (port != null)
                {
                    iQueryable = iQueryable.Where(d => d.DestinationCode == port.Code);
                }
            }

            if (string.IsNullOrEmpty(customerId))
            {
                IQueryable<Participant> myParticipants = ParticipantRep.GetParticipants(tenant);
                List<int> forwarderTennats = new List<int>();
                foreach (Participant item in myParticipants)
                {
                    forwarderTennats.Add(item.ForwarderTenant);
                }

                iQueryable = iQueryable.Where(d => forwarderTennats.Contains(d.SourceTenant));
            }

            else
            {
                int selectedTenantId = Convert.ToInt32(customerId);
                iQueryable = iQueryable.Where(d => d.SourceTenant == selectedTenantId);
            }

            #endregion

            #region Fill Report Data            
            totalData.Logo = WebFreight.Web.DataProviders.General.GetLogo(tenant);

            if (iQueryable.Count() > 0)
            {
                AWBRecord awbRecord = null;

                foreach (AirlineStatistics a in iQueryable)
                {
                    awbRecord = new AWBRecord();

                    awbRecord.CarrierCode = a.AirlineCode;
                    awbRecord.MessageType = a.MessageType;
                    awbRecord.Prefix = a.AirlinePrefix;
                    awbRecord.AWBNumber = a.AWBNumber;
                    awbRecord.HouseNumber = a.HWBNumber;
                    awbRecord.SentDate = a.LastSentDate;
                    awbRecord.Participant = a.SourceTenantName;
                    awbRecord.User = a.Sender;
                    awbRecord.Origin = a.OriginCode;
                    awbRecord.Destination = a.DestinationCode;
                    awbRecord.NumberOfPieces = a.NumberOfPackages;
                    awbRecord.GrossWeight = a.GrossWeight;
                    awbRecord.ChargeableWeight = a.ChargeableWeight;
                    awbRecord.WeightUnitCode = a.GrossWeightUnitCode;
                    awbRecord.Volume = a.Volume;
                    awbRecord.NatureOfGoods = a.DescriptionOfGoods;
                    awbRecord.Shipper = a.ShipperName;
                    awbRecord.Consignee = a.ConsigneeName;
                    awbRecord.Flight1 = a.Flight1;
                    awbRecord.FlightDate1 = a.Flight1Date;
                    awbRecord.Flight2 = a.Flight2;
                    awbRecord.FlightDate2 = a.Flight2Date;
                    awbRecord.Flight3 = a.Flight3;
                    awbRecord.FlightDate3 = a.Flight3Date;

                    totalData.AWBsRecordList.Add(awbRecord);
                }
            }

            #endregion

            totalData.AWBsRecordList = totalData.AWBsRecordList.OrderBy(d => d.Participant).ToList();

            return totalData;
        }
        #endregion

        #region Flight Booking Report
        [WebMethod]
        public byte[] LoadFlightBookingData(byte[] xmlFilters, int tenant)
        {
            FlightBookingDataProvider dataprovider = GetFlightBookingDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(FlightBookingDataProvider), tenant);
        }

        private FlightBookingDataProvider GetFlightBookingDataProvider(byte[] xmlFilters, int tenant)
        {
            FlightBookingDataProvider totalData = new FlightBookingDataProvider();
            totalData.FlightBookingRecordList = new List<FlightBookingRecord>();
            ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);

            ContactRepository contactRepository = new ContactRepository(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            ShipmentPackageRepository shipmentPackageRepository = new ShipmentPackageRepository(tenant);
            IQueryable<ShipmentDataView> iQueryable = shipmentRepository.GetShipmentViewsByTenant(tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(GetAuthenticatedUser(tenant), tenant);

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            #region Report Filters

            QueryFilterItem filterItem_FlightDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FlightDate").FirstOrDefault();
            QueryFilterItem filterItem_FlightNumber = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FlightNumber").FirstOrDefault();
            QueryFilterItem filterItem_CustomAgent = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomAgentId").FirstOrDefault();

            DateTime? flightDate = null;
            string flightNumber = null;
            string customAgentId = null;
            double totalWeight = 0;
            double totalPackagesQuantity = 0;
            double totalContainersQuantity = 0;

            if (filterItem_FlightDate != null)
            {
                if (filterItem_FlightDate.FieldValue != null)
                {
                    flightDate = (DateTime)filterItem_FlightDate.FieldValue;
                }
            }

            if (filterItem_FlightNumber != null)
            {
                if (filterItem_FlightNumber.FieldValue != null)
                {
                    flightNumber = filterItem_FlightNumber.FieldValue.ToString();
                }
            }

            if (filterItem_CustomAgent != null)
            {
                if (filterItem_CustomAgent.FieldValue != null)
                {
                    customAgentId = filterItem_CustomAgent.FieldValue.ToString();
                }
            }

            #endregion

            #region Base Data Filtered

            iQueryable = iQueryable.Where(d => d.TransportModeId == "A");
            iQueryable = iQueryable.Where(d => d.ShipmentLevelCode != "H");

            if (flightDate != null)
            {
                iQueryable = (from d in iQueryable
                              where
                             (d.MainCarriageETD != null && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETD) == System.Data.Entity.DbFunctions.TruncateTime(flightDate))
                             ||
                             (d.MainCarriageATD != null && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATD) == System.Data.Entity.DbFunctions.TruncateTime(flightDate))
                              select d);
            }

            if (!string.IsNullOrEmpty(flightNumber))
            {
                iQueryable = iQueryable.Where(d => (d.MainCarriageCarrierCode + d.MainCarriageCarrierNumber) == flightNumber);
            }

            if (!string.IsNullOrEmpty(customAgentId))
            {
                iQueryable = iQueryable.Where(d => d.CustomAgentImportId == customAgentId || d.CustomAgentExportId == customAgentId);
            }

            #endregion

            #region Fill Report Data

            totalData.Logo = WebFreight.Web.DataProviders.General.GetLogo(tenant);
            totalData.FlightDateFilter = flightDate;

            if (!string.IsNullOrEmpty(flightNumber))
            {
                totalData.FlightNumberFilter = flightNumber;

                if (flightDate != null)
                {
                    totalData.FlightNumberFilter = totalData.FlightNumberFilter + " / ";
                }
            }

            if (loggedContact != null)
            {
                totalData.LoggedUserName = loggedContact.EnglishName;
            }

            ShipmentDataView singleShipment = iQueryable.FirstOrDefault();
            if (singleShipment != null)
            {
                totalData.FlightTime = singleShipment.MainCarriageETD != null ? singleShipment.MainCarriageETD.Value.ToShortTimeString() : (singleShipment.MainCarriageATD != null ? singleShipment.MainCarriageATD.Value.ToShortTimeString() : "");
                totalData.Routing = singleShipment.MainCarriageFromPortCode + "-" + singleShipment.MainCarriageFinalDestinationPortCode;
            }
            string WeightUnit = "";
            if (iQueryable.Count() > 0)
            {
                FlightBookingRecord flightBookingRecord = null;

                foreach (ShipmentDataView a in iQueryable)
                {
                    IQueryable<ShipmentPackage> packages = shipmentPackageRepository.GetShipmentPackagesForShipmentTenant(a.Id, tenant);

                    flightBookingRecord = new FlightBookingRecord();
                    flightBookingRecord.AWBNumber = a.AirlinePrefix + "-" + a.Master;
                    flightBookingRecord.ShipperName = a.ShipperName;
                    flightBookingRecord.Pieces = a.NumberOfPackages;
                    flightBookingRecord.GrossWeight = a.GrossWeightInKG;
                    flightBookingRecord.Volume = a.VolumeInCBM;
                    flightBookingRecord.DescriptionOfGoods = a.DescriptionOfGoods;
                    flightBookingRecord.ShipmentNumber = a.ShipmentNumber;
                    flightBookingRecord.House = a.House;
                    flightBookingRecord.ConsigneeName = a.ConsigneeName;
                    flightBookingRecord.VolumetricWeight = a.VolumetricWeight;
                    flightBookingRecord.VolumetricWeightUnit = a.ChargeableWeightUnitCode;
                    flightBookingRecord.ShipmentCreateDate = a.CreateDateTime;
                    flightBookingRecord.CustomAgentExport = a.CustomAgentExportName;
                    flightBookingRecord.CustomAgentImport = a.CustomAgentImportName;
                    flightBookingRecord.MoveType = a.MoveTypeName;
                    flightBookingRecord.SCI = a.SCI;
                    flightBookingRecord.InvoiceNumber = a.ARInvoices;

                    if (a.ShipmentLevelCode == "C")
                    {
                        flightBookingRecord.HAWBsNumbers = shipmentRepository.GetHouseShipmentsCountForMaster(a.Id, tenant);
                    }

                    if (!string.IsNullOrEmpty(a.ShipperAddressId))
                    {
                        AddressRepository addressRepository = new AddressRepository(myCommonContext);
                        Address address = addressRepository.GetSingleAddress(a.ShipperAddressId, tenant);
                        if (address != null)
                        {
                            flightBookingRecord.ShipperAddress = DataProviders.General.GetAddress(address);
                        }
                    }

                    if (!string.IsNullOrEmpty(a.ConsigneeAddressId))
                    {
                        AddressRepository addressRepository = new AddressRepository(myCommonContext);
                        Address address = addressRepository.GetSingleAddress(a.ConsigneeAddressId, tenant);
                        if (address != null)
                        {
                            flightBookingRecord.ConsigneeAddress = DataProviders.General.GetAddress(address);
                        }
                    }

                    flightBookingRecord.PC = a.FreightPrepaidCollectId;
                    flightBookingRecord.DestinationPortCode = a.MainCarriageFinalDestinationPortCode != null ? a.MainCarriageFinalDestinationPortCode : "";
                    flightBookingRecord.ChargeableWeight = a.ChargeableWeight != null ? a.ChargeableWeight != 0 ? (String.Format("{0:#,0.00}", a.ChargeableWeight)) : "" : "";
                    totalWeight = totalWeight + (a.GrossWeight != null ? a.GrossWeight.Value : 0);

                    totalPackagesQuantity = totalPackagesQuantity + (a.NumberOfPackages != null ? a.NumberOfPackages.Value : 0);

                    if (a.TransportModeId != "A")
                    {
                        totalContainersQuantity = totalContainersQuantity + (a.NumberOfContainers != null ? a.NumberOfContainers.Value : 0);
                    }

                    if (!string.IsNullOrEmpty(WeightUnit))
                    {
                        WeightUnit = a.GrossWeightUnitCode != null ? a.GrossWeightUnitCode : ""; // "KG";
                    }

                    if (packages != null && packages.Count() > 0)
                    {
                        string dim = "";
                        string ref1 = "";
                        string ref2 = "";
                        string ref3 = "";
                        string ref4 = "";
                        string packagesNotes = "";
                        string packagesQuantity = "";

                        foreach (ShipmentPackage package in packages)
                        {
                            if (!string.IsNullOrEmpty(dim))
                            {
                                dim = dim + Environment.NewLine;
                            }

                            dim = dim + package.Quantity + " pc / " + package.Length + "x" + package.Width + "x" + package.Height + " Cm";

                            //Ref 1
                            if (!string.IsNullOrEmpty(package.Reference1))
                            {
                                if (!string.IsNullOrEmpty(ref1))
                                {
                                    ref1 = ref1 + Environment.NewLine;
                                }

                                ref1 = ref1 + package.Reference1;
                            }
                            /////////////////

                            //Ref 2
                            if (!string.IsNullOrEmpty(package.Reference2))
                            {
                                if (!string.IsNullOrEmpty(ref2))
                                {
                                    ref2 = ref2 + Environment.NewLine;
                                }

                                ref2 = ref2 + package.Reference2;
                            }
                            /////////////////

                            //Ref 3
                            if (!string.IsNullOrEmpty(package.Reference3))
                            {
                                if (!string.IsNullOrEmpty(ref3))
                                {
                                    ref3 = ref3 + Environment.NewLine;
                                }

                                ref3 = ref3 + package.Reference3;
                            }
                            /////////////////

                            //ref 4
                            if (!string.IsNullOrEmpty(package.Reference4))
                            {
                                if (!string.IsNullOrEmpty(ref4))
                                {
                                    ref4 = ref4 + Environment.NewLine;
                                }

                                ref4 = ref4 + package.Reference4;
                            }

                            if (!string.IsNullOrEmpty(package.Notes))
                            {
                                if (!string.IsNullOrEmpty(packagesNotes))
                                {
                                    packagesNotes = packagesNotes + Environment.NewLine;
                                }
                                packagesNotes = packagesNotes + package.Notes;
                            }

                            if (package.Quantity != null)
                            {
                                if (!string.IsNullOrEmpty(packagesQuantity))
                                {
                                    packagesQuantity = packagesQuantity + Environment.NewLine;
                                }
                                packagesQuantity = packagesQuantity + package.Quantity.ToString();
                            }
                        }

                        flightBookingRecord.Dimensions = dim;
                        flightBookingRecord.PackagesRef1 = ref1;
                        flightBookingRecord.PackagesRef2 = ref2;
                        flightBookingRecord.PackagesRef3 = ref3;
                        flightBookingRecord.PackagesRef4 = ref4;
                        flightBookingRecord.PackagesQuantity = packagesQuantity;
                        flightBookingRecord.PackagesNotes = packagesNotes;
                    }

                    totalData.FlightBookingRecordList.Add(flightBookingRecord);
                }
            }

            totalData.TotalPieces = totalData.FlightBookingRecordList.Sum(s => s.Pieces);
            totalData.TotalGrossWeight = totalData.FlightBookingRecordList.Sum(s => s.GrossWeight);
            totalData.TotalVolume = totalData.FlightBookingRecordList.Sum(s => s.Volume);
            totalData.TotalAWBs = totalData.FlightBookingRecordList.Count;

            totalData.TotalWeight = totalWeight != 0 ? (String.Format("{0:#,0.00}", totalWeight) + " " + (WeightUnit)) : ""; //KGS 
            if (totalPackagesQuantity != 0)
                totalData.TotalQuantity = totalPackagesQuantity.ToString();// + " Pcs" + Environment.NewLine;

            if (totalContainersQuantity != 0)
                totalData.TotalQuantity += totalContainersQuantity.ToString();// +" Con";

            #endregion

            return totalData;
        }
        #endregion

        #region Shipment Charges Analysis Report
        [WebMethod]
        public byte[] LoadShipmentChargesAnalysisData(byte[] xmlFilters, int tenant)
        {
            ShipmentChargesAnalysisDataProvider dataprovider = GetShipmentChargesAnalysisDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(ShipmentChargesAnalysisDataProvider), tenant);
        }

        private ShipmentChargesAnalysisDataProvider GetShipmentChargesAnalysisDataProvider(byte[] xmlFilters, int tenant)
        {
            ShipmentChargesAnalysisDataProvider totalData = new ShipmentChargesAnalysisDataProvider();
            totalData.ShipmentAnalysisRecordList = new List<ShipmentAnalysisRecord>();

            IShipmentsContext context = ShipmentsContext.GetContext(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(context);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
            IQueryable<ShipmentList> iQueryable = shipmentQuery.GetShipmentListTenant(tenant);

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_CustomerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();
            QueryFilterItem filterItem_DateType = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DateType").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_OperationalType = queryOperations.QueryFilterItems.Where(d => d.FieldName == "OperationalType").FirstOrDefault();
            QueryFilterItem filterItem_AccountingType = queryOperations.QueryFilterItems.Where(d => d.FieldName == "AccountingType").FirstOrDefault();

            QueryFilterItem filterItem_ReceivablesType = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ReceivablesType").FirstOrDefault();
            QueryFilterItem filterItem_PayablesType = queryOperations.QueryFilterItems.Where(d => d.FieldName == "PayablesType").FirstOrDefault();
            QueryFilterItem filterItem_ChargesTypeId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ChargesTypeId").FirstOrDefault();
            QueryFilterItem filterItem_IsProfitCurrecny = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IsProfitCurrecny").FirstOrDefault();
            QueryFilterItem filterItem_CurrencyCode = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CurrencyCode").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);

            DateTime fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            DateTime toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));

            string customerId = null;
            string chargesTypeId = null;
            string dateType = null;
            bool isProfitCurrecny = false;
            string currencyCode = null;
            string operationalType = null;
            string accountingType = null;
            string receivablesType = null;
            string payablesType = null;

            if (filterItem_DateType != null)
            {
                if (filterItem_DateType.FieldValue != null)
                {
                    dateType = filterItem_DateType.FieldValue.ToString();
                }
            }

            if (filterItem_IsProfitCurrecny != null)
            {
                if (filterItem_IsProfitCurrecny.FieldValue != null)
                {
                    isProfitCurrecny = (bool)filterItem_IsProfitCurrecny.FieldValue;
                }
            }

            if (filterItem_CurrencyCode != null)
            {
                if (filterItem_CurrencyCode.FieldValue != null)
                {
                    currencyCode = filterItem_CurrencyCode.FieldValue.ToString();
                }
            }

            if (filterItem_ChargesTypeId != null)
            {
                if (filterItem_ChargesTypeId.FieldValue != null)
                {
                    chargesTypeId = filterItem_ChargesTypeId.FieldValue.ToString();
                }
            }

            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    customerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);
            }

            if (filterItem_ToDate != null)
            {
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
            }

            if (filterItem_OperationalType != null)
            {
                if (filterItem_OperationalType.FieldValue != null)
                {
                    operationalType = filterItem_OperationalType.FieldValue.ToString();
                }
            }

            if (filterItem_AccountingType != null)
            {
                if (filterItem_AccountingType.FieldValue != null)
                {
                    accountingType = filterItem_AccountingType.FieldValue.ToString();
                }
            }

            if (filterItem_ReceivablesType != null)
            {
                if (filterItem_ReceivablesType.FieldValue != null)
                {
                    receivablesType = filterItem_ReceivablesType.FieldValue.ToString();
                }
            }

            if (filterItem_PayablesType != null)
            {
                if (filterItem_PayablesType.FieldValue != null)
                {
                    payablesType = filterItem_PayablesType.FieldValue.ToString();
                }
            }

            #endregion

            #region Shipment Filtered

            if (!string.IsNullOrEmpty(customerId))
            {
                iQueryable = iQueryable.Where(d => d.CustomerId == customerId);
            }

            #region Date Filter

            switch (dateType)
            {
                case "CRT":
                    {
                        if (fromDate != null)
                        {
                            iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                        }

                        if (toDate != null)
                        {
                            iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                        }

                        break;
                    }

                case "ARR":
                    {
                        if (fromDate != null)
                        {
                            iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATA) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                        }

                        if (toDate != null)
                        {
                            iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATA) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                        }

                        break;
                    }

                case "DEP":
                    {
                        if (fromDate != null)
                        {
                            iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATD) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                        }

                        if (toDate != null)
                        {
                            iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATD) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                        }

                        break;
                    }

                case "OPE":
                    {
                        if (fromDate != null)
                        {
                            iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OperationalDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                        }

                        if (toDate != null)
                        {
                            iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OperationalDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                        }

                        break;
                    }
                case "FOPC":
                    {
                        if (fromDate != null)
                        {
                            iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.FirstOperationalCloseDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                        }

                        if (toDate != null)
                        {
                            iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.FirstOperationalCloseDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                        }

                        break;
                    }
            }

            #endregion

            #region Operational Filter

            switch (operationalType)
            {
                case "All":
                    {
                        break;
                    }

                case "Open":
                    {
                        iQueryable = iQueryable.Where(d => !d.IsOperationalClosed);
                        break;
                    }

                case "Close":
                    {
                        iQueryable = iQueryable.Where(d => d.IsOperationalClosed);
                        break;
                    }
            }

            #endregion

            #region Accounting Filter

            switch (accountingType)
            {
                case "All":
                    {
                        break;
                    }

                case "Open":
                    {
                        iQueryable = iQueryable.Where(d => !d.IsAccountingClosed);
                        break;
                    }

                case "Close":
                    {
                        iQueryable = iQueryable.Where(d => d.IsAccountingClosed);
                        break;
                    }
            }

            #endregion

            #endregion

            List<ShipmentsReceivablesPayablesList> myResult = new List<ShipmentsReceivablesPayablesList>();

            IQueryable<ShipmentPayable> allPayables = null;
            IQueryable<ShipmentReceivable> allReceivables = null;

            if (iQueryable.Count() > 0)
            {
                allPayables = context.ShipmentPayables.Where(d => d.Tenant == tenant).Include("ChargesType").Include("ChargesType.ChargesGroup");
                allReceivables = context.ShipmentReceivables.Where(d => d.Tenant == tenant).Include("ChargesType").Include("ChargesType.ChargesGroup");

                myResult = ((from myShipment in iQueryable
                             join myPayable in allPayables on myShipment.Id equals myPayable.ShipmentId into myShipmentPayable
                             from myItem in myShipmentPayable.DefaultIfEmpty()
                             select new ShipmentsReceivablesPayablesList()
                             {
                                 Id = myShipment.Id + "P",
                                 ShipmentId = myShipment.Id,
                                 CustomerId = myShipment.CustomerId,
                                 CustomerName = myShipment.CustomerName,
                                 House = myShipment.House,
                                 Master = myShipment.LongMaster,
                                 CreateDateTime = myShipment.CreateDateTime,
                                 IsAccountingClosed = myShipment.IsAccountingClosed,
                                 IsOperationalClosed = myShipment.IsOperationalClosed,
                                 MainCarriageATA = myShipment.MainCarriageATA,
                                 MainCarriageATD = myShipment.MainCarriageATD,
                                 ShipmentNumber = myShipment.ShipmentNumber,
                                 OperationalDate = myShipment.OperationalDate,
                                 Quantity = myShipment.PackagesQuantity,
                                 GrossWeight = myShipment.GrossWeightInKG,
                                 ChargeableWeight = myShipment.ChargeableWeightInKG,
                                 TEU = myShipment.TEU,
                                 FirstPickupETD = myShipment.FirstPickupETD,
                                 MainCarriageETD = myShipment.MainCarriageETD,
                                 MainCarriagePortCode = myShipment.MainCarriageFromPortCode,
                                 FinalDestinationPortCode = myShipment.MainCarriageFinalDestinationPortCode,
                                 TransportMode = myShipment.TransportModeName,
                                 ValueOfGoods = myShipment.ValueOfGoods,
                                 FlightNumber = myShipment.MainCarriageCarrierCode + myShipment.MainCarriageCarrierNumber,
                                 ExchangeRate = myItem.ProfitCurrencyExchangeRate,
                                 ChargeTypeId = myItem.ChargesTypeId,
                                 ChargeTypeCode = myItem.ChargesType == null ? null : myItem.ChargesType.Code,
                                 ChargeTypeName = myItem.ChargesType == null ? null : myItem.ChargesType.EnglishName,
                                 Payables_OPEN = isProfitCurrecny ? myItem.OpenAmountInProfitCurrency : myItem.OpenAmountInLocalCurrency,
                                 Payables_ACCT = isProfitCurrecny ? myItem.AccountedAmountInProfitCurrency : myItem.AccountedAmountInLocalCurrency,
                                 Receivables_OPEN = 0,
                                 Receivables_ACCT = 0,
                                 ChargeGroupCode = myItem.ChargesType == null ? null : myItem.ChargesType.ChargesGroupCode,
                                 ChargeGroupName = myItem.ChargesType == null ? null : (myItem.ChargesType.ChargesGroup == null ? null : myItem.ChargesType.ChargesGroup.Name),
                             }).ToList()

                            .Union

                            (from myShipment in iQueryable
                             join myReceivable in allReceivables on myShipment.Id equals myReceivable.ShipmentId into myShipmentReceivable
                             from myItem in myShipmentReceivable.DefaultIfEmpty()
                             select new ShipmentsReceivablesPayablesList()
                             {
                                 Id = myShipment.Id + "R",
                                 ShipmentId = myShipment.Id,
                                 CustomerId = myShipment.CustomerId,
                                 CustomerName = myShipment.CustomerName,
                                 House = myShipment.House,
                                 Master = myShipment.LongMaster,
                                 CreateDateTime = myShipment.CreateDateTime,
                                 IsAccountingClosed = myShipment.IsAccountingClosed,
                                 IsOperationalClosed = myShipment.IsOperationalClosed,
                                 MainCarriageATA = myShipment.MainCarriageATA,
                                 MainCarriageATD = myShipment.MainCarriageATD,
                                 ShipmentNumber = myShipment.ShipmentNumber,
                                 OperationalDate = myShipment.OperationalDate,
                                 Quantity = myShipment.PackagesQuantity,
                                 GrossWeight = myShipment.GrossWeightInKG,
                                 ChargeableWeight = myShipment.ChargeableWeightInKG,
                                 TEU = myShipment.TEU,
                                 FirstPickupETD = myShipment.FirstPickupETD,
                                 MainCarriageETD = myShipment.MainCarriageETD,
                                 MainCarriagePortCode = myShipment.MainCarriageFromPortCode,
                                 FinalDestinationPortCode = myShipment.MainCarriageFinalDestinationPortCode,
                                 TransportMode = myShipment.TransportModeName,
                                 ValueOfGoods = myShipment.ValueOfGoods,
                                 FlightNumber = myShipment.MainCarriageCarrierCode + myShipment.MainCarriageCarrierNumber,
                                 ExchangeRate = myItem.ProfitCurrencyExchangeRate,
                                 ChargeTypeId = myItem.ChargesTypeId,
                                 ChargeTypeCode = myItem.ChargesType == null ? null : myItem.ChargesType.Code,
                                 ChargeTypeName = myItem.ChargesType == null ? null : myItem.ChargesType.EnglishName,
                                 Payables_OPEN = 0,
                                 Payables_ACCT = 0,
                                 Receivables_OPEN = myItem.ShipmentReceivableLineStatusCode == "OAMT" || myItem.ShipmentReceivableLineStatusCode == "EMPT" ? (isProfitCurrecny ? myItem.AmountInProfitCurrency : myItem.TotalAmountLocal) : 0,
                                 Receivables_ACCT = myItem.ShipmentReceivableLineStatusCode == "ACCT" || myItem.ShipmentReceivableLineStatusCode == "DRFT" ? (isProfitCurrecny ? myItem.AmountInProfitCurrency : myItem.TotalAmountLocal) : 0,
                                 ChargeGroupCode = myItem.ChargesType == null ? null : myItem.ChargesType.ChargesGroupCode,
                                 ChargeGroupName = myItem.ChargesType == null ? null : (myItem.ChargesType.ChargesGroup == null ? null : myItem.ChargesType.ChargesGroup.Name),
                             }).ToList())

                            .GroupBy(d => new
                            {
                                d.ShipmentId,
                                d.CustomerId,
                                d.CustomerName,
                                d.House,
                                d.Master,
                                d.CreateDateTime,
                                d.IsAccountingClosed,
                                d.IsOperationalClosed,
                                d.MainCarriageATA,
                                d.MainCarriageATD,
                                d.ShipmentNumber,
                                d.ChargeTypeId,
                                d.ChargeTypeCode,
                                d.ChargeTypeName,
                                d.OperationalDate,
                                d.Quantity,
                                d.GrossWeight,
                                d.ChargeableWeight,
                                d.TEU,
                                d.FirstPickupETD,
                                d.MainCarriageETD,
                                d.MainCarriagePortCode,
                                d.FinalDestinationPortCode,
                                d.TransportMode,
                                d.ValueOfGoods,
                                d.FlightNumber,
                                d.ChargeGroupCode,
                                d.ChargeGroupName,
                                d.ExchangeRate,
                            })

                            .Select(s => new ShipmentsReceivablesPayablesList()
                            {
                                Id = s.Key.ShipmentId + s.Key.ChargeTypeId,
                                ShipmentId = s.Key.ShipmentId,
                                CustomerId = s.Key.CustomerId,
                                CustomerName = s.Key.CustomerName,
                                House = s.Key.House,
                                Master = s.Key.Master,
                                CreateDateTime = s.Key.CreateDateTime,
                                IsAccountingClosed = s.Key.IsAccountingClosed,
                                IsOperationalClosed = s.Key.IsOperationalClosed,
                                MainCarriageATA = s.Key.MainCarriageATA,
                                MainCarriageATD = s.Key.MainCarriageATD,
                                ShipmentNumber = s.Key.ShipmentNumber,
                                OperationalDate = s.Key.OperationalDate,
                                Quantity = s.Key.Quantity,
                                GrossWeight = s.Key.GrossWeight,
                                ChargeableWeight = s.Key.ChargeableWeight,
                                TEU = s.Key.TEU,
                                FirstPickupETD = s.Key.FirstPickupETD,
                                MainCarriageETD = s.Key.MainCarriageETD,
                                MainCarriagePortCode = s.Key.MainCarriagePortCode,
                                FinalDestinationPortCode = s.Key.FinalDestinationPortCode,
                                TransportMode = s.Key.TransportMode,
                                ValueOfGoods = s.Key.ValueOfGoods,
                                FlightNumber = s.Key.FlightNumber,
                                ChargeTypeId = s.Key.ChargeTypeId,
                                ChargeTypeCode = s.Key.ChargeTypeCode,
                                ChargeTypeName = s.Key.ChargeTypeName,
                                Payables_OPEN = s.Sum(k => k.Payables_OPEN),
                                Payables_ACCT = s.Sum(k => k.Payables_ACCT),
                                Receivables_OPEN = s.Sum(k => k.Receivables_OPEN),
                                Receivables_ACCT = s.Sum(k => k.Receivables_ACCT),
                                ChargeGroupCode = s.Key.ChargeGroupCode,
                                ChargeGroupName = s.Key.ChargeGroupName,
                                ExchangeRate = s.Key.ExchangeRate,
                            }).ToList();
            }

            if (!string.IsNullOrEmpty(chargesTypeId))
            {
                if (payablesType != "MISS")
                {
                    #region Receivables
                    switch (receivablesType)
                    {
                        case "NOFI":
                            {
                                myResult = myResult.Where(d => d.ChargeTypeId == chargesTypeId).ToList();
                                break;
                            }

                        case "OPEN":
                            {
                                myResult = myResult.Where(d => d.ChargeTypeId == chargesTypeId && d.Receivables_OPEN > 0).ToList();
                                break;
                            }

                        case "ACCT":
                            {
                                myResult = myResult.Where(d => d.ChargeTypeId == chargesTypeId && d.Receivables_ACCT > 0).ToList();
                                break;
                            }

                        case "OPAT":
                            {
                                myResult = myResult.Where(d => d.ChargeTypeId == chargesTypeId && (d.Receivables_OPEN + d.Receivables_ACCT > 0)).ToList();
                                break;
                            }

                        case "MISS":
                            {
                                List<ShipmentsReceivablesPayablesList> myList = new List<ShipmentsReceivablesPayablesList>();
                                List<string> allShipmentsId = (from a in myResult group a by a.ShipmentId into g select g.Key).ToList();
                                foreach (string id in allShipmentsId)
                                {
                                    List<ShipmentsReceivablesPayablesList> shipmentData = myResult.Where(d => d.ShipmentId == id).ToList();
                                    double? allChargeAmopunt = shipmentData.Where(d => d.ChargeTypeId == chargesTypeId).Sum(s => s.Receivables_OPEN + s.Receivables_ACCT);

                                    if (allChargeAmopunt == 0)
                                    {
                                        ShipmentsReceivablesPayablesList myItem = shipmentData.Where(d => d.ShipmentId == id).FirstOrDefault();
                                        if (myItem != null)
                                        {
                                            myList.Add(myItem);
                                        }
                                    }
                                }

                                List<ShipmentPayable> allPayablesList = new List<ShipmentPayable>();
                                if (myList.Count > 0)
                                {
                                    allPayablesList = allPayables.Where(d => allShipmentsId.Contains(d.ShipmentId) && d.ChargesTypeId == chargesTypeId).ToList();
                                }

                                myResult = new List<ShipmentsReceivablesPayablesList>();
                                foreach (ShipmentsReceivablesPayablesList a in myList)
                                {
                                    double? myPayables_OPEN = 0;
                                    double? myPayables_ACCT = 0;

                                    if (isProfitCurrecny)
                                    {
                                        myPayables_OPEN = allPayablesList.Where(d => d.ShipmentId == a.ShipmentId).Sum(s => s.OpenAmountInProfitCurrency);
                                        myPayables_ACCT = allPayablesList.Where(d => d.ShipmentId == a.ShipmentId).Sum(s => s.AccountedAmountInProfitCurrency);
                                    }
                                    else
                                    {
                                        myPayables_OPEN = allPayablesList.Where(d => d.ShipmentId == a.ShipmentId).Sum(s => s.OpenAmountInLocalCurrency);
                                        myPayables_ACCT = allPayablesList.Where(d => d.ShipmentId == a.ShipmentId).Sum(s => s.AccountedAmountInLocalCurrency);
                                    }

                                    myResult.Add(new ShipmentsReceivablesPayablesList()
                                    {
                                        ShipmentNumber = a.ShipmentNumber,
                                        CustomerName = a.CustomerName,
                                        Master = a.Master,
                                        House = a.House,
                                        CreateDateTime = a.CreateDateTime,
                                        MainCarriageATA = a.MainCarriageATA,
                                        MainCarriageATD = a.MainCarriageATD,
                                        OperationalDate = a.OperationalDate,
                                        Quantity = a.Quantity,
                                        GrossWeight = a.GrossWeight,
                                        ChargeableWeight = a.ChargeableWeight,
                                        TEU = a.TEU,
                                        FirstPickupETD = a.FirstPickupETD,
                                        MainCarriageETD = a.MainCarriageETD,
                                        MainCarriagePortCode = a.MainCarriagePortCode,
                                        FinalDestinationPortCode = a.FinalDestinationPortCode,
                                        TransportMode = a.TransportMode,
                                        ValueOfGoods = a.ValueOfGoods,
                                        FlightNumber = a.FlightNumber,
                                        Receivables_OPEN = 0,
                                        Receivables_ACCT = 0,
                                        Payables_OPEN = myPayables_OPEN,
                                        Payables_ACCT = myPayables_ACCT,
                                    });
                                }
                                break;
                            }
                    }
                    #endregion
                }

                if (receivablesType != "MISS")
                {
                    #region Payables Filter
                    switch (payablesType)
                    {
                        case "NOFI":
                            {
                                myResult = myResult.Where(d => d.ChargeTypeId == chargesTypeId).ToList();
                                break;
                            }

                        case "OPEN":
                            {
                                myResult = myResult.Where(d => d.ChargeTypeId == chargesTypeId && d.Payables_OPEN > 0).ToList();
                                break;
                            }

                        case "ACCT":
                            {
                                myResult = myResult.Where(d => d.ChargeTypeId == chargesTypeId && d.Payables_ACCT > 0).ToList();
                                break;
                            }

                        case "OPAT":
                            {
                                myResult = myResult.Where(d => d.ChargeTypeId == chargesTypeId && (d.Payables_OPEN + d.Payables_ACCT > 0)).ToList();
                                break;
                            }

                        case "MISS":
                            {
                                List<ShipmentsReceivablesPayablesList> myList = new List<ShipmentsReceivablesPayablesList>();
                                List<string> allShipmentsId = (from a in myResult group a by a.ShipmentId into g select g.Key).ToList();
                                foreach (string id in allShipmentsId)
                                {
                                    List<ShipmentsReceivablesPayablesList> shipmentData = myResult.Where(d => d.ShipmentId == id).ToList();
                                    double? allChargeAmopunt = shipmentData.Where(d => d.ChargeTypeId == chargesTypeId).Sum(s => s.Payables_OPEN + s.Payables_ACCT);

                                    if (allChargeAmopunt == 0)
                                    {
                                        ShipmentsReceivablesPayablesList myItem = shipmentData.Where(d => d.ShipmentId == id).FirstOrDefault();
                                        if (myItem != null)
                                        {
                                            myList.Add(myItem);
                                        }
                                    }
                                }

                                List<ShipmentReceivable> allReceivablesList = new List<ShipmentReceivable>();
                                if (myList.Count > 0)
                                {
                                    allReceivablesList = allReceivables.Where(d => allShipmentsId.Contains(d.ShipmentId) && d.ChargesTypeId == chargesTypeId).ToList();
                                }

                                myResult = new List<ShipmentsReceivablesPayablesList>();
                                foreach (ShipmentsReceivablesPayablesList a in myList)
                                {
                                    double? myReceivables_OPEN = 0;
                                    double? myReceivables_ACCT = 0;

                                    if (isProfitCurrecny)
                                    {
                                        myReceivables_OPEN = allReceivablesList.Where(d => d.ShipmentId == a.ShipmentId && (d.ShipmentReceivableLineStatusCode == "OAMT" || d.ShipmentReceivableLineStatusCode == "EMPT")).Sum(s => s.AmountInProfitCurrency);
                                        myReceivables_ACCT = allReceivablesList.Where(d => d.ShipmentId == a.ShipmentId && (d.ShipmentReceivableLineStatusCode == "ACCT" || d.ShipmentReceivableLineStatusCode == "DRFT")).Sum(s => s.AmountInProfitCurrency);
                                    }
                                    else
                                    {
                                        myReceivables_OPEN = allReceivablesList.Where(d => d.ShipmentId == a.ShipmentId && (d.ShipmentReceivableLineStatusCode == "OAMT" || d.ShipmentReceivableLineStatusCode == "EMPT")).Sum(s => s.TotalAmountLocal);
                                        myReceivables_ACCT = allReceivablesList.Where(d => d.ShipmentId == a.ShipmentId && (d.ShipmentReceivableLineStatusCode == "ACCT" || d.ShipmentReceivableLineStatusCode == "DRFT")).Sum(s => s.TotalAmountLocal);
                                    }

                                    myResult.Add(new ShipmentsReceivablesPayablesList()
                                    {
                                        ShipmentNumber = a.ShipmentNumber,
                                        CustomerName = a.CustomerName,
                                        Master = a.Master,
                                        House = a.House,
                                        CreateDateTime = a.CreateDateTime,
                                        MainCarriageATA = a.MainCarriageATA,
                                        MainCarriageATD = a.MainCarriageATD,
                                        OperationalDate = a.OperationalDate,
                                        Quantity = a.Quantity,
                                        GrossWeight = a.GrossWeight,
                                        ChargeableWeight = a.ChargeableWeight,
                                        TEU = a.TEU,
                                        FirstPickupETD = a.FirstPickupETD,
                                        MainCarriageETD = a.MainCarriageETD,
                                        MainCarriagePortCode = a.MainCarriagePortCode,
                                        FinalDestinationPortCode = a.FinalDestinationPortCode,
                                        TransportMode = a.TransportMode,
                                        ValueOfGoods = a.ValueOfGoods,
                                        FlightNumber = a.FlightNumber,
                                        Receivables_OPEN = myReceivables_OPEN,
                                        Receivables_ACCT = myReceivables_ACCT,
                                        Payables_OPEN = 0,
                                        Payables_ACCT = 0,
                                    });
                                }
                                break;
                            }
                    }
                    #endregion
                }
            }

            else
            {
                if ((allPayables != null && allPayables.Count() > 0) || (allReceivables != null && allReceivables.Count() > 0))
                {
                    myResult = myResult.Where(d => !string.IsNullOrEmpty(d.ChargeTypeId)).ToList();
                }
            }

            #region Fill Report Data

            totalData.FromDate = fromDate;
            totalData.ToDate = toDate;
            totalData.SelectedCurrency = currencyCode;

            if (myResult != null && myResult.Count > 0)
            {
                ShipmentAnalysisRecord record = null;

                foreach (ShipmentsReceivablesPayablesList a in myResult.OrderBy(d => d.ShipmentNumber))
                {
                    record = new ShipmentAnalysisRecord();

                    record.ShipmentNumber = a.ShipmentNumber;
                    record.CustomerName = a.CustomerName;
                    record.Master = a.Master;
                    record.House = a.House;

                    if (dateType == "CRT")
                    {
                        record.Date = a.CreateDateTime;
                        record.SelectedDateLable = "Create Date";
                    }
                    else if (dateType == "ARR")
                    {
                        record.Date = a.MainCarriageATA == null ? null : a.MainCarriageATA;
                        record.SelectedDateLable = "Actual Arrival Date";
                    }
                    else if (dateType == "DEP")
                    {
                        record.Date = a.MainCarriageATD == null ? null : a.MainCarriageATD;
                        record.SelectedDateLable = "Actual Departure Date";
                    }
                    else if (dateType == "OPE")
                    {
                        record.Date = a.OperationalDate == null ? null : a.OperationalDate;
                        record.SelectedDateLable = "Operational Date";
                    }

                    record.OperationalDate = a.OperationalDate;
                    record.ChargeTypeCode = a.ChargeTypeCode;
                    record.ChargeTypeName = a.ChargeTypeName;
                    record.OpenReceivables = a.Receivables_OPEN;
                    record.AccountedReceivables = a.Receivables_ACCT;
                    record.OpenPayables = a.Payables_OPEN;
                    record.AccountedPayables = a.Payables_ACCT;
                    record.TotalQuantity = a.Quantity;
                    record.TotalGrossWeight = a.GrossWeight;
                    record.TotalChargeableWeight = a.ChargeableWeight;
                    record.TotalTEU = a.TEU;
                    record.DateOfLoading = a.FirstPickupETD != null ? a.FirstPickupETD : a.MainCarriageETD;
                    record.Origin = a.MainCarriagePortCode;
                    record.Destination = a.FinalDestinationPortCode;
                    record.TransportMode = a.TransportMode;
                    record.ValueOfGoods = a.ValueOfGoods;
                    record.FlightNumber = a.FlightNumber;
                    record.ChargeGroupCode = a.ChargeGroupCode;
                    record.ChargeGroupName = a.ChargeGroupName;
                    record.AccountingClosed = a.IsAccountingClosed;
                    record.ExchangeRate = a.ExchangeRate;

                    totalData.ShipmentAnalysisRecordList.Add(record);
                }
            }

            totalData.TotalOpenReceivables = totalData.ShipmentAnalysisRecordList.Sum(s => s.OpenReceivables);
            totalData.TotalAccountedReceivables = totalData.ShipmentAnalysisRecordList.Sum(s => s.AccountedReceivables);
            totalData.TotalOpenPayables = totalData.ShipmentAnalysisRecordList.Sum(s => s.OpenPayables);
            totalData.TotalAccountedPayables = totalData.ShipmentAnalysisRecordList.Sum(s => s.AccountedPayables);

            #endregion

            return totalData;
        }

        #endregion

        #region Participants Users Activities Report
        [WebMethod]
        public byte[] LoadParticipantsUsersActivitiesData(byte[] xmlFilters, int tenant)
        {
            ParticipantsUsersActivitiesDataProvider dataprovider = GetParticipantsUsersActivitiesDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(ParticipantsUsersActivitiesDataProvider), tenant);
        }

        private ParticipantsUsersActivitiesDataProvider GetParticipantsUsersActivitiesDataProvider(byte[] xmlFilters, int tenant)
        {
            ParticipantsUsersActivitiesDataProvider totalData = new ParticipantsUsersActivitiesDataProvider();
            totalData.ActiveParticipantsList = new List<ActiveParticipantRecord>();

            UserRepository userRep = new UserRepository(tenant);
            LogitudeMessagesTransmissionLogRepository messageLogRep = new LogitudeMessagesTransmissionLogRepository(tenant);
            IQueryable<LogitudeMessagesTransmissionLog> iQueryable = messageLogRep.GetLogitudeMessagesTransmissionLogs(tenant);

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_ParticipantId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ParticipantId").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);

            DateTime fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            DateTime toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));

            string participantId = null;

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);
            }

            if (filterItem_ToDate != null)
            {
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
            }

            if (filterItem_ParticipantId != null)
            {
                if (filterItem_ParticipantId.FieldValue != null)
                {
                    participantId = filterItem_ParticipantId.FieldValue.ToString();
                }
            }

            #endregion

            #region Base Data Filtered

            if (fromDate != null)
            {
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
            }

            if (toDate != null)
            {
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
            }

            if (!string.IsNullOrEmpty(participantId))
            {
                iQueryable = iQueryable.Where(d => d.ParticipantId == participantId);
            }
            #endregion

            #region Fill Report Data

            totalData.FromDate = fromDate;
            totalData.ToDate = toDate;
            totalData.Logo = WebFreight.Web.DataProviders.General.GetLogo(tenant);

            if (iQueryable.Count() > 0)
            {
                List<LogitudeMessagesTransmissionLog> allLogs = iQueryable.ToList();

                List<ActiveParticipantRecord> myList = (from a in allLogs
                                                        group a by new { a.UserEmail, a.UserName, a.Participant } into g
                                                        select new ActiveParticipantRecord()
                                                        {
                                                            Id = g.Key.UserEmail,
                                                            UserEmail = g.Key.UserEmail,
                                                            UserFullName = g.Key.UserName,
                                                            ParticipantName = g.Key.Participant,
                                                            TotalTransmissionPerFFR = g.Where(d => d.MessageTypeCode == "FFR").Count(),
                                                            TotalTransmissionPerFHL = g.Where(d => d.MessageTypeCode == "FHL").Count(),
                                                            TotalTransmissionPerFSR = g.Where(d => d.MessageTypeCode == "FSR").Count(),
                                                            TotalTransmissionPerFVR = g.Where(d => d.MessageTypeCode == "FVR").Count(),
                                                            TotalTransmissionPerFWB = g.Where(d => d.MessageTypeCode == "FWB").Count(),
                                                        }).ToList();

                foreach (ActiveParticipantRecord item in myList)
                {
                    ActiveParticipantRecord record = new ActiveParticipantRecord();

                    if (!string.IsNullOrEmpty(item.UserFullName))
                    {
                        string[] names = item.UserFullName.Trim().Split(' ');

                        if (names.Length == 1)
                        {
                            record.UserFirstName = names[0];
                            record.UserLastName = "";
                        }
                        else
                        {
                            record.UserFirstName = names[0];
                            record.UserLastName = names[1];
                        }
                    }

                    record.Id = item.Id;
                    record.UserEmail = item.UserEmail;
                    record.ParticipantName = item.ParticipantName;
                    record.TotalTransmissionPerFFR = item.TotalTransmissionPerFFR;
                    record.TotalTransmissionPerFHL = item.TotalTransmissionPerFHL;
                    record.TotalTransmissionPerFSR = item.TotalTransmissionPerFSR;
                    record.TotalTransmissionPerFVR = item.TotalTransmissionPerFVR;
                    record.TotalTransmissionPerFWB = item.TotalTransmissionPerFWB;

                    totalData.ActiveParticipantsList.Add(record);
                }
            }

            #endregion

            return totalData;
        }

        #endregion

        #region AR Invoice Include Vat and Routing Report

        [WebMethod]
        public byte[] LoadInvoicesVatAndRoutingData(byte[] xmlFilters, int tenant)
        {
            ARInvoiceIncludeVATRoutingsDataProvider dataprovider = LoadInvoicesVatAndRoutingDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(ARInvoiceIncludeVATRoutingsDataProvider), tenant);
        }

        public ARInvoiceIncludeVATRoutingsDataProvider LoadInvoicesVatAndRoutingDataProvider(byte[] xmlFilters, int tenant)
        {
            ARInvoiceIncludeVATRoutingsDataProvider totalData = new ARInvoiceIncludeVATRoutingsDataProvider();
            totalData.ARInvoiceVATRoutingList = new List<ARInvoiceVATRouting>();
            totalData.InvoiceTotalsList = new List<InvoiceVATRoutingTotals>();

            ARInvoiceTotalVATRepository aRInvoiceToatalVatRepository = new ARInvoiceTotalVATRepository(tenant);
            ARInvoiceQuery arInvoiceQuery = new ARInvoiceQuery(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            VatTypeRepository vatTypeRepository = new VatTypeRepository(tenant);

            IQueryable<ARInvoiceList> iQueryable_Invoices = arInvoiceQuery.GetInvoiceListByTenant(tenant);
            iQueryable_Invoices = iQueryable_Invoices.Where(d => !d.IsConstituentInvoice);
            List<string> shipmentIds = iQueryable_Invoices.Select(s => s.MainEntityId).ToList();
            IQueryable<Shipment> iQueryable_Shipments = shipmentRepository.GetShipmentsForUnpaidInvoicesReportWithInnerSelect(tenant);
            List<VatType> tenantVatTypes = vatTypeRepository.GetVatTypes(tenant).ToList();

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_CustomerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();
            QueryFilterItem filterItem_PartnerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "PartnerId").FirstOrDefault();
            QueryFilterItem filterItem_IsByInvoiceDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IsByInvoiceDate").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_IncludeVoidInvoices = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeVoidInvoices").FirstOrDefault();
            QueryFilterItem filterItem_IncludeDraftInvoices = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeDraftInvoices").FirstOrDefault();
            QueryFilterItem filterItem_IsLocalCurrency = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IsLocalCurrency").FirstOrDefault();
            QueryFilterItem filterItem_DirectionCode = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DirectionCode").FirstOrDefault();
            QueryFilterItem filterItem_TransportModeCode = queryOperations.QueryFilterItems.Where(d => d.FieldName == "TransportModeCode").FirstOrDefault();
            QueryFilterItem filterItem_InvoiceStatusCode = queryOperations.QueryFilterItems.Where(d => d.FieldName == "InvoiceStatusCode").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);
            DateTime fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            DateTime toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));

            string customerId = null;
            string partnerId = null;
            bool isByInvoiceDate = true;
            bool includeVoidInvoices = true;
            bool includeDraftInvoices = true;
            bool isLocalCurrency = true;
            string directionCode = null;
            string transportModeCode = null;
            string invoiceStatusCode = null;

            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    customerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }

            if (filterItem_PartnerId != null)
            {
                if (filterItem_PartnerId.FieldValue != null)
                {
                    partnerId = filterItem_PartnerId.FieldValue.ToString();
                }
            }

            if (filterItem_IsByInvoiceDate != null)
            {
                if (filterItem_IsByInvoiceDate.FieldValue != null)
                {
                    isByInvoiceDate = (bool)filterItem_IsByInvoiceDate.FieldValue;
                }
            }

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);
            }

            if (filterItem_ToDate != null)
            {
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
            }

            if (filterItem_IncludeVoidInvoices != null)
            {
                if (filterItem_IncludeVoidInvoices.FieldValue != null)
                {
                    includeVoidInvoices = (bool)filterItem_IncludeVoidInvoices.FieldValue;
                }
            }

            if (filterItem_IncludeDraftInvoices != null)
            {
                if (filterItem_IncludeDraftInvoices.FieldValue != null)
                {
                    includeDraftInvoices = (bool)filterItem_IncludeDraftInvoices.FieldValue;
                }
            }

            if (filterItem_IsLocalCurrency != null)
            {
                if (filterItem_IsLocalCurrency.FieldValue != null)
                {
                    isLocalCurrency = (bool)filterItem_IsLocalCurrency.FieldValue;
                }
            }

            if (filterItem_DirectionCode != null)
            {
                if (filterItem_DirectionCode.FieldValue != null)
                {
                    directionCode = filterItem_DirectionCode.FieldValue.ToString();
                }
            }

            if (filterItem_TransportModeCode != null)
            {
                if (filterItem_TransportModeCode.FieldValue != null)
                {
                    transportModeCode = filterItem_TransportModeCode.FieldValue.ToString();
                }
            }

            if (filterItem_InvoiceStatusCode != null)
            {
                if (filterItem_InvoiceStatusCode.FieldValue != null)
                {
                    invoiceStatusCode = filterItem_InvoiceStatusCode.FieldValue.ToString();
                }
            }

            #endregion

            #region Base Data Filtered

            if (!string.IsNullOrEmpty(customerId))
            {
                iQueryable_Invoices = iQueryable_Invoices.Where(d => d.BillToId == customerId);
            }

            if (!string.IsNullOrEmpty(partnerId))
            {
                iQueryable_Invoices = iQueryable_Invoices.Where(d => d.PartnerId == partnerId);
            }

            if (isByInvoiceDate)
            {
                if (fromDate != null)
                {
                    iQueryable_Invoices = iQueryable_Invoices.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                }

                if (toDate != null)
                {
                    iQueryable_Invoices = iQueryable_Invoices.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                }
            }

            else
            {
                if (fromDate != null)
                {
                    iQueryable_Shipments = iQueryable_Shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                }

                if (toDate != null)
                {
                    iQueryable_Shipments = iQueryable_Shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                }

                List<string> ids = iQueryable_Shipments.Select(s => s.Id).ToList();
                iQueryable_Invoices = iQueryable_Invoices.Where(d => ids.Contains(d.MainEntityId));
            }

            if (!includeDraftInvoices)
            {
                iQueryable_Invoices = iQueryable_Invoices.Where(d => d.StatusCode != "DR");
            }

            if (!includeVoidInvoices)
            {
                iQueryable_Invoices = iQueryable_Invoices.Where(d => d.StatusCode != "VD");
            }

            if (!string.IsNullOrEmpty(directionCode))
            {
                iQueryable_Shipments = iQueryable_Shipments.Where(d => d.DirectionId == directionCode);

                List<string> ids = iQueryable_Shipments.Select(s => s.Id).ToList();
                iQueryable_Invoices = iQueryable_Invoices.Where(d => ids.Contains(d.MainEntityId));
            }

            if (!string.IsNullOrEmpty(transportModeCode))
            {
                iQueryable_Shipments = iQueryable_Shipments.Where(d => d.TransportModeId == transportModeCode);

                List<string> ids = iQueryable_Shipments.Select(s => s.Id).ToList();
                iQueryable_Invoices = iQueryable_Invoices.Where(d => ids.Contains(d.MainEntityId));
            }

            if (!string.IsNullOrEmpty(invoiceStatusCode))
            {
                iQueryable_Invoices = iQueryable_Invoices.Where(d => d.StatusCode == invoiceStatusCode);
            }

            #endregion

            #region Fill Report Data

            totalData.FromDate = fromDate;
            totalData.ToDate = toDate;
            totalData.Logo = WebFreight.Web.DataProviders.General.GetLogo(tenant);

            if (iQueryable_Invoices.Count() > 0)
            {
                List<ARInvoiceTotalVAT> totalVats = aRInvoiceToatalVatRepository.GetInvoiceTotalVATsByTenant(tenant).ToList();
                int counter = 1;

                foreach (ARInvoiceList invoice in iQueryable_Invoices)
                {
                    ARInvoiceVATRouting record = new ARInvoiceVATRouting();

                    Shipment shipment = iQueryable_Shipments.Where(d => d.Id == invoice.MainEntityId).FirstOrDefault();

                    List<ARInvoiceTotalVAT> myTotalVats = totalVats.Where(d => d.ARInvoiceId == invoice.Id).ToList();
                    List<VATClass> myVATS = new List<VATClass>();

                    foreach (ARInvoiceTotalVAT vat in myTotalVats)
                    {
                        VATClass item = new VATClass()
                        {
                            Index = counter,
                            Percentage = vat.VatPercent,
                            VATName = tenantVatTypes.Where(d => d.Id == vat.VatTypeId).FirstOrDefault().EnglishName,
                            VATCode = tenantVatTypes.Where(d => d.Id == vat.VatTypeId).FirstOrDefault().Code,
                            InvoiceAmount = vat.InvoiceCurrencyVATAmount,
                            LocalAmount = vat.LocalVATAmount,
                        };

                        myVATS.Add(item);

                        counter++;
                    }

                    foreach (VATClass item in myVATS)
                    {
                        //1
                        if (string.IsNullOrEmpty(totalData.VAT1Code))
                        {
                            totalData.VAT1Code = item.VATCode;
                            totalData.VAT1Header = item.Percentage + "% " + item.VATName;
                            record.VAT1Amount = isLocalCurrency ? item.LocalAmount : item.InvoiceAmount;
                            continue;
                        }

                        else
                        {
                            if (totalData.VAT1Code == item.VATCode)
                            {
                                totalData.VAT1Code = item.VATCode;
                                totalData.VAT1Header = item.Percentage + "% " + item.VATName;
                                record.VAT1Amount = isLocalCurrency ? item.LocalAmount : item.InvoiceAmount;
                                continue;
                            }
                        }

                        //2
                        if (string.IsNullOrEmpty(totalData.VAT2Code))
                        {
                            totalData.VAT2Code = item.VATCode;
                            totalData.VAT2Header = item.Percentage + "% " + item.VATName;
                            record.VAT2Amount = isLocalCurrency ? item.LocalAmount : item.InvoiceAmount;
                            continue;
                        }

                        else
                        {
                            if (totalData.VAT2Code == item.VATCode)
                            {
                                totalData.VAT2Code = item.VATCode;
                                totalData.VAT2Header = item.Percentage + "% " + item.VATName;
                                record.VAT2Amount = isLocalCurrency ? item.LocalAmount : item.InvoiceAmount;
                                continue;
                            }
                        }

                        //3
                        if (string.IsNullOrEmpty(totalData.VAT3Code))
                        {
                            totalData.VAT3Code = item.VATCode;
                            totalData.VAT3Header = item.Percentage + "% " + item.VATName;
                            record.VAT3Amount = isLocalCurrency ? item.LocalAmount : item.InvoiceAmount;
                            continue;
                        }

                        else
                        {
                            if (totalData.VAT3Code == item.VATCode)
                            {
                                totalData.VAT3Code = item.VATCode;
                                totalData.VAT3Header = item.Percentage + "% " + item.VATName;
                                record.VAT3Amount = isLocalCurrency ? item.LocalAmount : item.InvoiceAmount;
                                continue;
                            }
                        }

                        //4
                        if (string.IsNullOrEmpty(totalData.VAT4Code))
                        {
                            totalData.VAT4Code = item.VATCode;
                            totalData.VAT4Header = item.Percentage + "% " + item.VATName;
                            record.VAT4Amount = isLocalCurrency ? item.LocalAmount : item.InvoiceAmount;
                            continue;
                        }

                        else
                        {
                            if (totalData.VAT4Code == item.VATCode)
                            {
                                totalData.VAT4Code = item.VATCode;
                                totalData.VAT4Header = item.Percentage + "% " + item.VATName;
                                record.VAT4Amount = isLocalCurrency ? item.LocalAmount : item.InvoiceAmount;
                                continue;
                            }
                        }
                    }

                    if (isByInvoiceDate)
                    {
                        record.Date = invoice.InvoiceDate;
                    }
                    else
                    {
                        if (shipment != null)
                        {
                            record.Date = shipment.CreateDateTime;
                        }
                    }

                    record.InvoiceNumber = invoice.InvoiceNumber;
                    record.BillToName = invoice.BillToName;
                    record.PartnerName = invoice.PartnerName;
                    record.OurReference = invoice.MainEntityReference;
                    record.CustomerReference = invoice.CustomerRef;
                    record.BillToCode = invoice.BillToCode;
                    record.InvoiceDueDate = invoice.DueDate;

                    if (shipment != null)
                    {
                        record.Routing = shipment.Routing;
                        record.ProfitInLocalCurrency = shipment.ProfitInLocalCurrency;
                        record.ProfitInProfitCurrency = shipment.ProfitInProfitCurrency;
                        record.Consignee = shipment.ConsigneeName;
                        record.Shipper = shipment.ShipperName;
                        if (!string.IsNullOrEmpty(shipment.CustomerReference1))
                        {
                            record.CustomerReference = shipment.CustomerReference1;
                        }
                    }

                    record.InvoiceStatus = invoice.StatusName;

                    if (isLocalCurrency)
                    {
                        record.SubTotal = invoice.SubTotalInLocalCurrency;
                        record.VAT = myTotalVats.Sum(d => d.LocalVATAmount);
                        record.GrandTotal = invoice.SubTotalInLocalCurrency + myTotalVats.Sum(d => d.LocalVATAmount);
                        record.Currency = invoice.LocalCurrencyCode;
                        record.TaxableAmount = myTotalVats.Where(x => x.VatPercent != 0).Sum(d => d.LocalVatableAmount);
                        record.NonTaxableAmount = myTotalVats.Where(x => x.VatPercent == 0).Sum(d => d.LocalVatableAmount);

                    }
                    else
                    {
                        record.SubTotal = invoice.SubTotalInInvoiceCurrency;
                        record.VAT = myTotalVats.Sum(d => d.InvoiceCurrencyVATAmount);
                        record.GrandTotal = invoice.SubTotalInInvoiceCurrency + myTotalVats.Sum(d => d.InvoiceCurrencyVATAmount);
                        record.Currency = invoice.InvoiceCurrencyCode;
                        record.TaxableAmount = myTotalVats.Where(x => x.VatPercent != 0).Sum(d => d.InvoiceCurrencyVATAmount);
                        record.NonTaxableAmount = myTotalVats.Where(x => x.VatPercent == 0).Sum(d => d.InvoiceCurrencyVATAmount);
                    }



                    totalData.ARInvoiceVATRoutingList.Add(record);
                }

                if (isLocalCurrency)
                {
                    totalData.SubTotal_Sum = totalData.ARInvoiceVATRoutingList.Sum(s => s.SubTotal);
                    totalData.GrandTotal_Sum = totalData.ARInvoiceVATRoutingList.Sum(s => s.GrandTotal);
                    totalData.VAT_Sum = totalData.ARInvoiceVATRoutingList.Sum(s => s.VAT);

                    totalData.TotalVats_1 = totalData.ARInvoiceVATRoutingList.Sum(s => s.VAT1Amount);
                    totalData.TotalVats_2 = totalData.ARInvoiceVATRoutingList.Sum(s => s.VAT2Amount);
                    totalData.TotalVats_3 = totalData.ARInvoiceVATRoutingList.Sum(s => s.VAT3Amount);
                    totalData.TotalVats_4 = totalData.ARInvoiceVATRoutingList.Sum(s => s.VAT4Amount);
                }

                else
                {
                    totalData.InvoiceTotalsList = (from b in totalData.ARInvoiceVATRoutingList
                                                   group b by new { b.Currency } into g
                                                   select new InvoiceVATRoutingTotals()
                                                   {
                                                       Currency = g.Key.Currency,
                                                       SubTotals_Local = g.Sum(b => b.SubTotal),
                                                       GrandTotal_Local = g.Sum(b => b.GrandTotal),
                                                       TotalVats = g.Sum(b => b.VAT).Value,
                                                       VAT1Amount = g.Sum(b => b.VAT1Amount),
                                                       VAT2Amount = g.Sum(b => b.VAT2Amount),
                                                       VAT3Amount = g.Sum(b => b.VAT3Amount),
                                                       VAT4Amount = g.Sum(b => b.VAT4Amount),
                                                   }).ToList();
                }
            }

            #endregion

            return totalData;
        }

        #endregion

        #region OceanInsights
        [WebMethod]
        public byte[] LoadOceanInsightsData(byte[] xmlFilters, int tenant)
        {
            OceanInsightsDataProvider dataprovider = LoadOceanInsightsDataProvider(xmlFilters, tenant);
            XmlSerializer serializer = new XmlSerializer(typeof(OceanInsightsDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, dataprovider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        public OceanInsightsDataProvider LoadOceanInsightsDataProvider(byte[] xmlFilters, int tenant)
        {
            OceanInsightsDataProvider totalData = new OceanInsightsDataProvider();
            //totalData.OceanInsightsRecordList = new List<OceanInsightsRecord>();

            //ContactRepository contactRepository = new ContactRepository(tenant);
            OceanInsightsRequestRepository OceanInsightsRepository = new OceanInsightsRequestRepository(tenant);
            //ShipmentPackageRepository shipmentPackageRepository = new ShipmentPackageRepository(tenant);
            IQueryable<OceanInsightsRequest> iQueryable = OceanInsightsRepository.GetOceanInsightsRequests().Where(a => a.FromPushPage == false);
            //Contact loggedContact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), tenant);

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            #region Report Filters

            QueryFilterItem filterItem_Tenant = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Tenant").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_Detailed = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Detailed").FirstOrDefault();

            //DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            //DateTime myStartDate = todayDate.AddMonths(-1);
            //DateTime fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            //DateTime toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));


            //if (filterItem_FromDate != null)
            //{
            //    DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);
            //}

            //if (filterItem_ToDate != null)
            //{
            //    DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
            //}
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            int Tenant = -1;
            bool Detail = false;

            if (filterItem_FromDate != null)
            {
                if (filterItem_FromDate.FieldValue != null)
                {
                    FromDate = (DateTime)filterItem_FromDate.FieldValue;
                    totalData.FromDate = (DateTime)FromDate;

                }

            }

            if (filterItem_ToDate != null)
            {
                if (filterItem_ToDate.FieldValue != null)
                {
                    ToDate = (DateTime)filterItem_ToDate.FieldValue;
                    totalData.ToDate = (DateTime)ToDate;
                }
            }

            if (filterItem_Tenant != null)
            {
                if (filterItem_Tenant.FieldValue != null)
                {
                    Tenant = (int)filterItem_Tenant.FieldValue;
                    totalData.Tenant = Tenant;
                }
            }

            if (filterItem_Detailed != null)
            {
                if (filterItem_Detailed.FieldValue != null)
                {
                    Detail = (bool)filterItem_Detailed.FieldValue;
                    totalData.Detailed = Detail;
                }
            }

            #endregion

            #region Base Data Filtered

            if (FromDate != null && ToDate != null)
            {
                iQueryable = (from d in iQueryable
                              where (System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= FromDate && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= ToDate)
                              select d);
            }

            if (Tenant != -1)
            {
                iQueryable = iQueryable.Where(d => d.Tenant == Tenant);
            }

            #endregion

            #region Fill Report Data

            totalData.Logo = WebFreight.Web.DataProviders.General.GetLogo(tenant);
            if (totalData.OceanInsightsRecordList == null)
            {
                totalData.OceanInsightsRecordList = new List<OceanInsightsRecord>();
            }
            if (totalData.OceanInsightsRecordGroup == null)
            {
                totalData.OceanInsightsRecordGroup = new List<OceanInsightsRecordGrouped>();
            }
            if (iQueryable.Count() > 0)
            {
                OceanInsightsRecord Record = null;
                if (Tenant == -1 && Detail == false)
                {
                    var temp = iQueryable.GroupBy(a => a.Tenant);
                    foreach (var item in temp)
                    {
                        TenantRepository rep = new TenantRepository(0);
                        var Name = rep.GetSingleByTenant(item.Key);
                        totalData.OceanInsightsRecordList.Add(new OceanInsightsRecord() { key = item.Key, Total = item.Count(), TenantName = Name.Company });
                    }
                }
                else
                {
                    foreach (OceanInsightsRequest a in iQueryable)
                    {
                        Record = new OceanInsightsRecord();
                        Record.BLNumber = a.BLNumber;
                        Record.ContainerNumber = a.ContainerNumber;
                        Record.CreateDate = a.CreateDate;
                        Record.Id = a.OceanInsigntId;
                        Record.SCACCode = a.SCACCode;
                        Record.Tenant = a.Tenant;
                        totalData.OceanInsightsRecordList.Add(Record);
                    }
                }


            }

            totalData.Total = totalData.OceanInsightsRecordList.Count;

            #endregion

            return totalData;
        }
        #endregion

        #region CASS Report
        [WebMethod]
        public byte[] LoadCASSData(byte[] xmlFilters, int tenant)
        {
            CASSDataProvider dataprovider = LoadCASSDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(CASSDataProvider), tenant);
        }

        private CASSDataProvider LoadCASSDataProvider(byte[] xmlFilters, int tenant)
        {
            CASSDataProvider totalData = new CASSDataProvider();
            totalData.ShipmentsData = new List<ShipmentDataRecord>();

            IShipmentsContext myShipmentsContext = ShipmentsContext.GetContext(tenant);
            ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);

            ShipmentRepository shipmentRepository = new ShipmentRepository(myShipmentsContext);
            TenantRepository tenantRepository = new TenantRepository(myCommonContext);
            AddressRepository addressRepository = new AddressRepository(myCommonContext);
            CurrencyRepository currencyRepository = new CurrencyRepository(myCommonContext);
            CardRepository cardRepository = new CardRepository(myCommonContext);
            APPaymentRepository paymentRepository = new APPaymentRepository(tenant);

            IQueryable<ShipmentDataView> allShipments = shipmentRepository.GetShipmentViewsByTenant(tenant);
            allShipments = allShipments.Where(d => d.ShipmentLevelCode != "H");
            allShipments = allShipments.Where(d => d.MAWBOBLDate != null);

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_AirlineId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "AirlineId").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);

            DateTime fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            DateTime toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));

            string airlineId = null;

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);
            }

            if (filterItem_ToDate != null)
            {
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
            }

            if (filterItem_AirlineId != null)
            {
                if (filterItem_AirlineId.FieldValue != null)
                {
                    airlineId = filterItem_AirlineId.FieldValue.ToString();
                }
            }

            #endregion

            #region Tenant Data

            Tenant CurrenctTenant = tenantRepository.GetSingleByTenant(tenant);
            if (CurrenctTenant != null)
            {
                totalData.TenantName = CurrenctTenant.Company;
                totalData.TenantIATACode = CurrenctTenant.IATA;
                totalData.FromDate = fromDate;
                totalData.ToDate = toDate;
                totalData.Logo = WebFreight.Web.DataProviders.General.GetLogo(tenant);

                if (!string.IsNullOrEmpty(CurrenctTenant.AddressId))
                {
                    Address tenantAddress = addressRepository.GetSingleAddress(CurrenctTenant.AddressId, tenant);

                    if (tenantAddress != null)
                    {
                        totalData.TenantAddress = DataProviders.General.GetAddress(tenantAddress);
                    }
                }

                if (!string.IsNullOrEmpty(CurrenctTenant.CurrencyId))
                {
                    Currency tenantCurrency = currencyRepository.GetSingleCurrency(CurrenctTenant.CurrencyId, tenant);

                    if (tenantCurrency != null)
                    {
                        totalData.TenantLocalCurrencyCode = tenantCurrency.Code;
                    }
                }

                if (!string.IsNullOrEmpty(airlineId))
                {
                    Card airline = cardRepository.GetSingleCard(airlineId, tenant);
                    if (airline != null)
                    {
                        totalData.AirlineName = airline.EnglishName;
                    }
                }
            }

            #endregion

            #region Base Data Filtered

            if (fromDate != null)
            {
                allShipments = allShipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.MAWBOBLDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
            }

            if (toDate != null)
            {
                allShipments = allShipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.MAWBOBLDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
            }

            if (!string.IsNullOrEmpty(airlineId))
            {
                allShipments = allShipments.Where(d => d.MainCarriageCarrierId == airlineId);
            }

            #endregion

            #region Fill Report Data
            List<ShipmentDataView> allShipments_ToList = allShipments.ToList();

            if (allShipments_ToList.Count > 0)
            {
                List<string> allShipmentsIds = allShipments_ToList.Select(s => s.Id).ToList();

                List<ShipmentPayable> allPayables = (from d in myShipmentsContext.ShipmentPayables.Include("ChargesType").Include("ChargesType.IATACode")
                                                     where allShipmentsIds.Contains(d.ShipmentId)
                                                     && d.Tenant == tenant
                                                     select d).ToList();

                List<ShipmentCommodity> allCommodities = (from d in myShipmentsContext.ShipmentCommodities
                                                          where allShipmentsIds.Contains(d.ShipmentId)
                                                          && d.Tenant == tenant && d.IsFirstLine
                                                          select d).ToList();

                var allVATsAndPercentages = (from a in myCommonContext.VatTypes
                                             join b in myCommonContext.VatTypePercentages on a.Id equals b.VatTypeId into VatAndPercentages
                                             from myVatAndPercentages in VatAndPercentages.DefaultIfEmpty()
                                             where myVatAndPercentages.Tenant == tenant
                                             && myVatAndPercentages.Percentage != null
                                             && myVatAndPercentages.Percentage != 0
                                             select new
                                             {
                                                 Id = a.Id,
                                                 Name = a.EnglishName,
                                                 FromDate = myVatAndPercentages.FromDate,
                                                 Percentage = myVatAndPercentages.Percentage,
                                             }).ToList();

                double? totalDue = 0;
                double? totalVat = 0;

                foreach (ShipmentDataView a in allShipments_ToList)
                {
                    ShipmentCommodity myShipmentCommodity = allCommodities.Where(d => d.ShipmentId == a.Id).FirstOrDefault();
                    List<ShipmentPayable> myPayables = allPayables.Where(d => d.ShipmentId == a.Id).ToList();

                    if (myPayables.Count > 0)
                    {
                        double? freight = 0;
                        double? commission = 0;
                        double? due = 0;

                        foreach (ShipmentPayable item in myPayables)
                        {
                            if (item.ChargesType.ChargesGroupCode == "FRT")
                            {
                                freight += item.OpenAmountInLocalCurrency == null ? 0 : item.OpenAmountInLocalCurrency;
                                due += item.OpenAmountInLocalCurrency == null ? 0 : item.OpenAmountInLocalCurrency;
                                totalDue += item.OpenAmountInLocalCurrency == null ? 0 : item.OpenAmountInLocalCurrency;
                            }

                            if (item.ChargesType.ChargesGroupCode == "COMM")
                            {
                                commission += item.OpenAmountInLocalCurrency == null ? 0 : item.OpenAmountInLocalCurrency;
                                due += item.OpenAmountInLocalCurrency == null ? 0 : item.OpenAmountInLocalCurrency;
                                totalDue += item.OpenAmountInLocalCurrency == null ? 0 : item.OpenAmountInLocalCurrency;
                            }

                            if (!string.IsNullOrEmpty(item.ChargesType.VatTypeId))
                            {
                                var myVATsAndPercentages = allVATsAndPercentages.Where(d => d.Id == item.ChargesType.VatTypeId).OrderByDescending(o => o.FromDate).FirstOrDefault();
                                if (myVATsAndPercentages != null)
                                {
                                    ShipmentDataRecord shipmentRecord = new ShipmentDataRecord();
                                    shipmentRecord.Id = a.ShipmentNumber;
                                    shipmentRecord.AWBNumber = EntityFieldsHelper.GetLongMasterField(a);
                                    shipmentRecord.ChargeableWeight = a.ChargeableWeight;
                                    shipmentRecord.DestinationCode = a.MainCarriageFinalDestinationPortCode;

                                    if (myShipmentCommodity != null)
                                    {
                                        shipmentRecord.CommodityNumber = myShipmentCommodity.CommodityNumber;
                                    }

                                    shipmentRecord.PayablesFrieghtRate = myPayables.Where(d => d.ChargesType.ChargesGroupCode == "FRT").Sum(s => s.UnitPrice);
                                    shipmentRecord.CrossTabHeader = myVATsAndPercentages.Name + " " + myVATsAndPercentages.Percentage + "%";
                                    shipmentRecord.CrossTabValue = (item.OpenAmountInLocalCurrency == null ? 0 : item.OpenAmountInLocalCurrency) * myVATsAndPercentages.Percentage;
                                    shipmentRecord.CrossTabIndex = 3;
                                    totalVat += shipmentRecord.CrossTabValue == null ? 0 : shipmentRecord.CrossTabValue;

                                    totalData.ShipmentsData.Add(shipmentRecord);
                                }
                            }

                            if (item.ChargesType.ChargesGroupCode != "FRT" && item.ChargesType.ChargesGroupCode != "COMM")
                            {
                                ShipmentDataRecord shipmentRecord = new ShipmentDataRecord();
                                shipmentRecord.Id = a.ShipmentNumber;
                                shipmentRecord.AWBNumber = EntityFieldsHelper.GetLongMasterField(a);
                                shipmentRecord.ChargeableWeight = a.ChargeableWeight;
                                shipmentRecord.DestinationCode = a.MainCarriageFinalDestinationPortCode;

                                if (myShipmentCommodity != null)
                                {
                                    shipmentRecord.CommodityNumber = myShipmentCommodity.CommodityNumber;
                                }

                                shipmentRecord.PayablesFrieghtRate = myPayables.Where(d => d.ChargesType.ChargesGroupCode == "FRT").Sum(s => s.UnitPrice);

                                string header = "";
                                if (item.ChargesType.IATACode != null)
                                {
                                    header = item.ChargesType.IATACode.Code;

                                    if (!string.IsNullOrEmpty(item.ChargesType.DueTypeCode))
                                    {
                                        header = header + item.ChargesType.DueTypeCode.Substring(0, 1);
                                    }
                                }
                                else
                                {
                                    header = item.ChargesType.EnglishName;
                                }

                                shipmentRecord.CrossTabHeader = header;
                                shipmentRecord.CrossTabValue = item.OpenAmountInLocalCurrency == null ? 0 : item.OpenAmountInLocalCurrency;
                                shipmentRecord.CrossTabIndex = 4;

                                due += item.OpenAmountInLocalCurrency == null ? 0 : item.OpenAmountInLocalCurrency;
                                totalDue += item.OpenAmountInLocalCurrency == null ? 0 : item.OpenAmountInLocalCurrency;

                                totalData.ShipmentsData.Add(shipmentRecord);
                            }
                        }

                        ShipmentDataRecord shipmentRecord_Freight = new ShipmentDataRecord();
                        ShipmentDataRecord shipmentRecord_Comm = new ShipmentDataRecord();
                        ShipmentDataRecord shipmentRecord_due = new ShipmentDataRecord();

                        shipmentRecord_Freight.Id = shipmentRecord_Comm.Id = shipmentRecord_due.Id = a.ShipmentNumber;
                        shipmentRecord_Freight.AWBNumber = shipmentRecord_Comm.AWBNumber = shipmentRecord_due.AWBNumber = EntityFieldsHelper.GetLongMasterField(a);
                        shipmentRecord_Freight.ChargeableWeight = shipmentRecord_Comm.ChargeableWeight = shipmentRecord_due.ChargeableWeight = a.ChargeableWeight;
                        shipmentRecord_Freight.DestinationCode = shipmentRecord_Comm.DestinationCode = shipmentRecord_due.DestinationCode = a.MainCarriageFinalDestinationPortCode;
                        shipmentRecord_Freight.PayablesFrieghtRate = shipmentRecord_Comm.PayablesFrieghtRate = shipmentRecord_due.PayablesFrieghtRate = myPayables.Where(d => d.ChargesType.ChargesGroupCode == "FRT").Sum(s => s.UnitPrice);

                        if (myShipmentCommodity != null)
                        {
                            shipmentRecord_Freight.CommodityNumber = shipmentRecord_Comm.CommodityNumber = shipmentRecord_due.CommodityNumber = myShipmentCommodity.CommodityNumber;
                        }

                        shipmentRecord_Freight.CrossTabIndex = 1;
                        shipmentRecord_Freight.CrossTabHeader = "Freight";
                        shipmentRecord_Freight.CrossTabValue = freight;

                        shipmentRecord_Comm.CrossTabIndex = 2;
                        shipmentRecord_Comm.CrossTabHeader = "AGENT Comm";
                        shipmentRecord_Comm.CrossTabValue = commission;

                        shipmentRecord_due.CrossTabIndex = 5;
                        shipmentRecord_due.CrossTabHeader = "Total Due (local)";
                        shipmentRecord_due.CrossTabValue = due;

                        totalData.ShipmentsData.Add(shipmentRecord_Freight);
                        totalData.ShipmentsData.Add(shipmentRecord_Comm);
                        totalData.ShipmentsData.Add(shipmentRecord_due);
                    }
                }

                IQueryable<APPayment> payments = paymentRepository.GetOpenedAPPayments(tenant).Where(d => d.VendorId == airlineId);
                double? payments_sum = 0;
                if (payments.Count() > 0)
                {
                    payments_sum = payments.Sum(s => s.OpenAmount);
                }

                totalData.TotalDue = totalDue;
                totalData.TotalVAT = totalVat;
                totalData.AdvancedPayment = payments_sum;
                totalData.FinalPayableNetSale = totalData.TotalDue + totalData.TotalVAT - totalData.AdvancedPayment;
            }

            #endregion

            totalData.Logo = DataProviders.General.GetLogo(tenant);
            totalData.ShipmentsData = totalData.ShipmentsData.OrderBy(d => d.CrossTabIndex).ToList();
            return totalData;
        }
        #endregion

        #region InventoryReport
        public byte[] LoadInventoryData(byte[] xmlFilters, int tenant)
        {
            InventoryDataProvider dataprovider = LoadInventoryDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(InventoryDataProvider), tenant);

        }

        private InventoryDataProvider LoadInventoryDataProvider(byte[] xmlFilters, int tenant)
        {
            InventoryDataProvider dataProvider = new InventoryDataProvider();
            #region Report Filters
            string customerId = string.Empty;
            string warehouseId = string.Empty;
            string shipperConsigneeId = string.Empty;
            int DaysInWarehouseValue = 0;
            string DaysInWarehouseOperatorFilterValue = string.Empty;

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            //CustomerId
            QueryFilterItem queryFilterItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();
            if (queryFilterItem != null && queryFilterItem.FieldValue != null) customerId = queryFilterItem.FieldValue.ToString();


            //WarehouseId
            queryFilterItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "WarehouseId").FirstOrDefault();
            if (queryFilterItem != null && queryFilterItem.FieldValue != null) warehouseId = queryFilterItem.FieldValue.ToString();

            //ShipperConsigneeId
            queryFilterItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ShipperConsigneeId").FirstOrDefault();
            if (queryFilterItem != null && queryFilterItem.FieldValue != null) shipperConsigneeId = queryFilterItem.FieldValue.ToString();

            //DaysInWarehouseValue
            queryFilterItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DaysInWarehouse").FirstOrDefault();
            if (queryFilterItem != null)
            {
                if (queryFilterItem.FieldValue != null) DaysInWarehouseValue = queryFilterItem.FieldValue != null && !string.IsNullOrEmpty(queryFilterItem.FieldValue.ToString()) ? Int32.Parse(queryFilterItem.FieldValue.ToString()) : 0;
                DaysInWarehouseOperatorFilterValue = queryFilterItem.Operator;
            }

            WarehouseEntryPackageQueryService warehouseEntryPackageQueryService = new WarehouseEntryPackageQueryService(tenant);
            List<WarehouseEntryPackageItem> result = warehouseEntryPackageQueryService.GetWarehouseEntryPackageItemForInventoryReport(new WarehouseEntryPackageArgs() { CustomerId = customerId, WarehouseId = warehouseId, ShipperConsigneesId = shipperConsigneeId, Tenant = tenant, DaysInWarehouseOperatorFilterValue = DaysInWarehouseOperatorFilterValue, DaysInWarehouseValue = DaysInWarehouseValue });
            dataProvider.WarehouseEntryPackageList = result;
            dataProvider.PartnerName = string.IsNullOrEmpty(customerId) ? "All" : "";
            dataProvider.Warehouse = string.IsNullOrEmpty(warehouseId) ? "All" : "";
            dataProvider.ShipperConsignee = string.IsNullOrEmpty(shipperConsigneeId) ? "All" : "";
            dataProvider.TotalVolume = result.Sum(s => s.Volume);

            if (!string.IsNullOrEmpty(customerId))
            {
                Card customer = CardRepository.GetSingleCard(customerId, tenant, true);
                if (customer != null) dataProvider.PartnerName = customer.EnglishName;
            }

            if (!string.IsNullOrEmpty(warehouseId))
            {
                Card warehouseCard = CardRepository.GetSingleCard(warehouseId, tenant, true);
                if (warehouseCard != null) dataProvider.Warehouse = warehouseCard.EnglishName;
            }

            if (!string.IsNullOrEmpty(shipperConsigneeId))
            {
                Card shipperConsigneeCard = CardRepository.GetSingleCard(shipperConsigneeId, tenant, true);
                if (shipperConsigneeCard != null) dataProvider.ShipperConsignee = shipperConsigneeCard.EnglishName;
            }

            List<InventoryDataProvider.InventoryGroup> finalResults = (from a in dataProvider.WarehouseEntryPackageList

                                                                       group a by new { a.WarehouseName, a.WarehouseId, }
                                                       into g
                                                                       select new InventoryDataProvider.InventoryGroup()
                                                                       {
                                                                           Warehouse = g.Key.WarehouseName,
                                                                           WarehouseList = g.ToList(),
                                                                       }).ToList();

            dataProvider.InventoryGroupList = finalResults.OrderBy(d => d.Warehouse).ToList();

            #endregion
            return dataProvider;
        }
        #endregion

        #region Work Per Hours Project Report 
        [WebMethod]
        public byte[] LoadWorkPerDaysProjectData(byte[] xmlFilters, int tenant)
        {
            WorkDaysPerProjectDataProvider dataprovider = GetWorkPerDaysProjectDataProvider(xmlFilters, tenant);
            XmlSerializer serializer = new XmlSerializer(typeof(WorkDaysPerProjectDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, dataprovider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        private WorkDaysPerProjectDataProvider GetWorkPerDaysProjectDataProvider(byte[] xmlFilters, int tenant)
        {
            WorkDaysPerProjectDataProvider result = new WorkDaysPerProjectDataProvider();
            result.SummarizedWorkHoursPerProjectList = new List<WorkDaysPerProjectData>();
            result.DetailedWorkHoursPerProjectList = new List<WorkDaysPerProjectData>();

            ContactRepository contactRepository = new ContactRepository(tenant);
            TMEmployeeTimeRepository employeeTimeRepository = new TMEmployeeTimeRepository(tenant);
            ITimeManagementContext myContext = TimeManagementContext.GetContext(tenant);
            IQueryable<TMProject> allProjects = (from d in myContext.TMProjects where d.Tenant == tenant select d);
            IQueryable<TMEmployeeTime> iQueryable = (from d in myContext.TMEmployeeTimes where d.Tenant == tenant select d);
            CardRepository cardRep = new CardRepository(tenant);
            ContactRepository contactRep = new ContactRepository(tenant);

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_ProjectId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ProjectId").FirstOrDefault();
            QueryFilterItem filterItem_CustomerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();
            QueryFilterItem filterItem_OwnerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "OwnerId").FirstOrDefault();
            QueryFilterItem filterItem_IncludeInnerProject = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeInnerProject").FirstOrDefault();
            QueryFilterItem filterItem_EmployeeUserId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "EmployeeUserId").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_BudgetId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "BudgetId").FirstOrDefault();
            QueryFilterItem filterItem_CategoryId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CategoryId").FirstOrDefault();
            QueryFilterItem filterItem_ExternalProjectNumber = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ExternalProjectNumber").FirstOrDefault();

            DateTime? fromDate = null;
            DateTime? toDate = null;
            string customerId = null;
            string employeeUserId = null;
            string budgetId = null;
            string categoryId = null;
            string projectId = null;
            string ownerId = null;
            string externalProjectNumber = null;
            bool IncludeInnerProject = false;

            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    customerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }

            if (filterItem_FromDate != null)
            {
                if (filterItem_FromDate.FieldValue != null)
                {
                    fromDate = (DateTime)filterItem_FromDate.FieldValue;
                }
            }

            if (filterItem_ToDate != null)
            {
                if (filterItem_ToDate.FieldValue != null)
                {
                    toDate = (DateTime)filterItem_ToDate.FieldValue;
                }
            }

            if (filterItem_EmployeeUserId != null)
            {
                if (filterItem_EmployeeUserId.FieldValue != null)
                {
                    employeeUserId = filterItem_EmployeeUserId.FieldValue.ToString();
                }
            }

            if (filterItem_BudgetId != null)
            {
                if (filterItem_BudgetId.FieldValue != null)
                {
                    budgetId = filterItem_BudgetId.FieldValue.ToString();
                }
            }
            if (filterItem_CategoryId != null)
            {
                if (filterItem_CategoryId.FieldValue != null)
                {
                    categoryId = filterItem_CategoryId.FieldValue.ToString();
                }
            }


            if (filterItem_ExternalProjectNumber != null)
            {
                if (filterItem_ExternalProjectNumber.FieldValue != null)
                {
                    externalProjectNumber = filterItem_ExternalProjectNumber.FieldValue.ToString();
                }
            }

            if (filterItem_ProjectId != null)
            {
                if (filterItem_ProjectId.FieldValue != null)
                {
                    projectId = filterItem_ProjectId.FieldValue.ToString();
                }
            }

            if (filterItem_OwnerId != null)
            {
                if (filterItem_OwnerId.FieldValue != null)
                {
                    ownerId = filterItem_OwnerId.FieldValue.ToString();
                }
            }

            if (filterItem_IncludeInnerProject != null)
            {
                if (filterItem_IncludeInnerProject.FieldValue != null)
                {
                    IncludeInnerProject = Convert.ToBoolean(filterItem_IncludeInnerProject.FieldValue);
                }
            }

            if (fromDate != null && toDate != null)
            {
                iQueryable = (from myTMEmployeeTime in iQueryable
                              join db_Projects in allProjects on myTMEmployeeTime.ProjectId equals db_Projects.Id into joinedData
                              from myProjct in joinedData
                              where myTMEmployeeTime.Tenant == tenant
                              && myProjct.Tenant == tenant
                              && myProjct.IsProrated == false
                              select myTMEmployeeTime);
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
            }

            if (!string.IsNullOrEmpty(projectId))
            {
                iQueryable = iQueryable.Where(d => d.ProjectId == projectId);
                TMProject project = allProjects.Where(d => d.Id == projectId).FirstOrDefault();
                if (project != null)
                {
                    result.ProjectName = project.Name;
                }
            }

            if (IncludeInnerProject == false)
            {
                iQueryable = (from myTMEmployeeTime in iQueryable
                              join db_Projects in allProjects on myTMEmployeeTime.ProjectId equals db_Projects.Id into joinedData
                              from myProjct in joinedData
                              where myTMEmployeeTime.Tenant == tenant
                              && myProjct.Tenant == tenant
                              && myProjct.IsInnerProject == false
                              && myProjct.IsProrated == false
                              select myTMEmployeeTime);
            }

            if (ownerId != null)
            {
                iQueryable = (from myTMEmployeeTime in iQueryable
                              join db_Projects in allProjects on myTMEmployeeTime.ProjectId equals db_Projects.Id into joinedData
                              from myProjct in joinedData
                              where myTMEmployeeTime.Tenant == tenant
                              && myProjct.Tenant == tenant
                              && myProjct.OwnerId == ownerId
                              && myProjct.IsProrated == false
                              select myTMEmployeeTime);
            }

            if (customerId != null)
            {
                var customerCard = cardRep.GetSingleCard(customerId, tenant);
                if (customerCard != null)
                {
                    result.CustomerName = customerCard.EnglishName;
                }

                iQueryable = (from myTMEmployeeTime in iQueryable
                              join db_Projects in allProjects on myTMEmployeeTime.ProjectId equals db_Projects.Id into joinedData
                              from myProjct in joinedData
                              where myTMEmployeeTime.Tenant == tenant
                              && myProjct.Tenant == tenant
                              && myProjct.CustomerId == customerId
                              && myProjct.IsProrated == false
                              select myTMEmployeeTime);
            }

            if (budgetId != null)
            {
                iQueryable = (from myTMEmployeeTime in iQueryable
                              join db_Projects in allProjects on myTMEmployeeTime.ProjectId equals db_Projects.Id into joinedData
                              from myProjct in joinedData
                              where myTMEmployeeTime.Tenant == tenant
                              && myProjct.Tenant == tenant
                              && myProjct.BudgetId == budgetId
                              && myProjct.IsProrated == false
                              select myTMEmployeeTime);
            }

            if (categoryId != null)
            {
                iQueryable = (from myTMEmployeeTime in iQueryable
                              join db_Projects in allProjects on myTMEmployeeTime.ProjectId equals db_Projects.Id into joinedData
                              from myProjct in joinedData
                              where myTMEmployeeTime.Tenant == tenant
                              && myProjct.Tenant == tenant
                              && myProjct.CategoryId == categoryId
                              && myProjct.IsProrated == false
                              select myTMEmployeeTime);
            }


            if (externalProjectNumber != null)
            {
                iQueryable = (from myTMEmployeeTime in iQueryable
                              join db_Projects in allProjects on myTMEmployeeTime.ProjectId equals db_Projects.Id into joinedData
                              from myProjct in joinedData
                              where myTMEmployeeTime.Tenant == tenant
                              && myProjct.Tenant == tenant
                              && myProjct.ExternalProjectNumber == externalProjectNumber
                              && myProjct.IsProrated == false
                              select myTMEmployeeTime);
            }

            result.FromDate = fromDate.Value;
            result.ToDate = toDate.Value;
            result.EmployeeUserId = employeeUserId;
            result.ProjectId = projectId;
            result.CustomerId = customerId;
            result.BudgetId = budgetId;
            result.CategoryId = categoryId;

            WorkDaysPerProjectData timSheetItem_Detailed = null;
            WorkDaysPerProjectData timSheetItem = null;
            double totalWIWorkedDays_Employee = 0;
            double totalWIWorkedDays = 0;

            bool isUsingNewCode = false;

            if (iQueryable.Count() > 0)
            {
                var daysList = (from d in iQueryable
                                group d by new { d.DateOfWork, d.EmployeeUserId, d.ProjectId, d.WINumber, d.Description } into g
                                select new
                                {
                                    DateOfWork = g.Key.DateOfWork,
                                    EmployeeUserId = g.Key.EmployeeUserId,
                                    ProjectId = g.Key.ProjectId,
                                    WINumber = g.Key.WINumber,
                                    Description = g.Key.Description,
                                });


                var daysList_Total = (from d in iQueryable
                                      group d by new { d.ProjectId } into g
                                      select new
                                      {
                                          ProjectId = g.Key.ProjectId,
                                      });

                foreach (var item in daysList_Total)
                {
                    List<TMEmployeeTime> itemGrouplist = iQueryable.Where(d => d.ProjectId == item.ProjectId).ToList();
                    if (itemGrouplist != null && itemGrouplist.Count() > 0)
                    {
                        timSheetItem = new WorkDaysPerProjectData();
                        TMProject project = allProjects.Where(d => d.Id == item.ProjectId).FirstOrDefault();
                        if (project != null)
                        {
                            timSheetItem.ProjectName = project.Name;
                            timSheetItem.ProjectNumber = project.ProjectNumber;
                            var card = cardRep.GetSingleCard(project.CustomerId, tenant);
                            if (card != null)
                            {
                                timSheetItem.CustomerName = card.EnglishName;
                            }
                            timSheetItem.Description = project.Description;


                            if (project.CategoryId != null)
                            {
                                TMProjectCategory category = new TMProjectCategory();
                                TMProjectCategoryRepository repo = new TMProjectCategoryRepository(tenant);
                                category = repo.GetSingle(project.CategoryId, tenant);
                                if (category != null)
                                {
                                    timSheetItem.CategoryId = category.Id;
                                    timSheetItem.CategoryName = category.Name;
                                }

                            }
                        }

                        var wIWorkedDays = Math.Round((itemGrouplist.Sum(a => a.TimeInMinutes)) / 60.0, 2) / 8;
                        totalWIWorkedDays += wIWorkedDays;
                        timSheetItem.TotalWIWorkedDays = DateFormat(wIWorkedDays);
                        timSheetItem.TotalWIWorkedDays_number = wIWorkedDays;
                        result.SummarizedWorkHoursPerProjectList.Add(timSheetItem);

                    }
                }

                List<ProjectsByCategoryGroup> finalResults = (from p in result.SummarizedWorkHoursPerProjectList
                                                              group p by new { p.CategoryId, p.CategoryName } into g
                                                              select new ProjectsByCategoryGroup()
                                                              {
                                                                  CategoryId = g.Key.CategoryId,
                                                                  CategoryName = g.Key.CategoryName,
                                                                  ProjectsRecordList = g.ToList(),
                                                                  Total = DateFormat((Math.Round(g.Sum(s => s.TotalWIWorkedDays_number), 2))),
                                                              }).ToList();

                result.ProjectsByCategoryGroupList = finalResults.OrderBy(d => d.CategoryName).ToList();

                if (!string.IsNullOrEmpty(employeeUserId))
                {
                    iQueryable = iQueryable.Where(d => d.EmployeeUserId == employeeUserId);
                    Contact contact = contactRepository.GetSingleContact(employeeUserId, tenant);
                    if (contact != null)
                    {
                        result.EmployeeName = contact.EnglishName;
                    }
                }

                foreach (var item in daysList)
                {
                    List<TMEmployeeTime> itemGrouplist = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) == System.Data.Entity.DbFunctions.TruncateTime(item.DateOfWork) && d.EmployeeUserId == item.EmployeeUserId && d.ProjectId == item.ProjectId && d.WINumber == item.WINumber && d.Description == item.Description).ToList();

                    if (itemGrouplist != null && itemGrouplist.Count() > 0)
                    {
                        timSheetItem_Detailed = new WorkDaysPerProjectData();

                        TMProject project = allProjects.Where(d => d.Id == item.ProjectId).FirstOrDefault();
                        if (project != null)
                        {
                            if (project.CategoryId != null)
                            {
                                TMProjectCategory category = new TMProjectCategory();
                                TMProjectCategoryRepository repo = new TMProjectCategoryRepository(tenant);
                                category = repo.GetSingle(project.CategoryId, tenant);
                                if (category != null)
                                {
                                    timSheetItem_Detailed.CategoryId = category.Id;
                                    timSheetItem_Detailed.CategoryName = category.Name;
                                }

                            }
                            timSheetItem_Detailed.ExternalProjectNumber = project.ExternalProjectNumber;
                            timSheetItem_Detailed.ProjectName = project.Name;
                            timSheetItem_Detailed.ProjectNumber = project.ProjectNumber;
                            var card = cardRep.GetSingleCard(project.CustomerId, tenant);
                            if (card != null)
                            {
                                timSheetItem_Detailed.CustomerName = card.EnglishName;
                            }

                            var owner = contactRep.GetSingleContact(project.OwnerId, tenant);
                            if (owner != null)
                            {
                                timSheetItem_Detailed.OwnerName = owner.EnglishName;
                            }
                        }

                        string contact_Name = "";
                        if (!string.IsNullOrEmpty(result.EmployeeName))
                        {
                            contact_Name = result.EmployeeName;
                        }

                        else
                        {
                            Contact contact = contactRepository.GetSingleContact(item.EmployeeUserId, tenant);
                            if (contact != null)
                            {
                                contact_Name = contact.EnglishName;
                            }
                        }

                        timSheetItem_Detailed.EmployeeName = contact_Name;
                        var date = item.DateOfWork.Date;
                        if (date != null)
                        {
                            timSheetItem_Detailed.DateOfWork = date;
                        }

                        timSheetItem_Detailed.WINumber = item.WINumber;
                        timSheetItem_Detailed.Description = item.Description;

                        TMEmployeeTime myTMEmployeeTime = null;

                        if (isUsingNewCode)
                        {
                            myTMEmployeeTime = (from d in myContext.TMEmployeeTimes
                                                where d.Tenant == tenant
                                                && d.ProjectId == item.ProjectId
                                                && d.Description == item.Description
                                                && d.EmployeeUserId == item.EmployeeUserId
                                                && d.WINumber == item.WINumber
                                                && System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) == System.Data.Entity.DbFunctions.TruncateTime(item.DateOfWork)
                                                select d).FirstOrDefault();
                        }

                        else
                        {
                            myTMEmployeeTime = employeeTimeRepository.GetSingleByPrjectandEmployeeandWIandDescription(item.ProjectId, item.Description, item.WINumber, item.EmployeeUserId, tenant);
                        }

                        var wIWorkedHours_Employee = Math.Round(myTMEmployeeTime.FullDuration / 60.0, 2);
                        totalWIWorkedDays_Employee += wIWorkedHours_Employee;
                        timSheetItem_Detailed.TotalWIWorkedDays_Employee = DateFormat(wIWorkedHours_Employee);
                        result.DetailedWorkHoursPerProjectList.Add(timSheetItem_Detailed);
                    }
                }
                result.Total_TotalWIWorkedHours = DateFormat((Math.Round(totalWIWorkedDays, 2)));
                result.Total_TotalWIWorkedHours_Employee = DateFormat(Math.Round(totalWIWorkedDays_Employee, 2));
            }
            return result;
        }

        private string DateFormat(double time)
        {
            var result = "";
            var isMinus = false;
            if (time != 0)
            {
                var ts = TimeSpan.FromHours(time);
                var h = 0.0;
                if (ts.TotalHours < 0)
                {
                    isMinus = true;
                    var h_Abs = Math.Abs(ts.TotalHours);
                    h = System.Math.Floor(h_Abs);
                }
                else
                {
                    h = System.Math.Floor(ts.TotalHours);
                }

                if (Math.Abs(ts.TotalHours) < 1)
                {
                    h = 0;
                }
                if (isMinus)
                {
                    h = h * -1;
                }
                var m = (ts.TotalHours - h) * 60;
                if (isMinus)
                {
                    if (m < 0)
                        m = m * -1;
                    if (h < 0)
                        h = h * -1;
                    result = "- " + h + ":" + m.ToString("00");
                }
                else
                {
                    result = h + ":" + m.ToString("00");
                }
            }
            return result;
        }
        public List<DateTime> GetDates(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                             .Select(day => new DateTime(year, month, day)) // Map each day to a date
                             .ToList(); // Load dates into a list
        }
        #endregion

        #region Tasks Without Projects 
        [WebMethod]
        public byte[] LoadTasksWithoutProjectsData(byte[] xmlFilters, int tenant)
        {
            TasksWithoutProjectsDataProvider dataprovider = GetTasksWithoutProjectsDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(TasksWithoutProjectsDataProvider), tenant);
        }

        private TasksWithoutProjectsDataProvider GetTasksWithoutProjectsDataProvider(byte[] xmlFilters, int tenant)
        {
            TasksWithoutProjectsDataProvider result = new TasksWithoutProjectsDataProvider();
            result.TasksWithoutProjectsList = new List<TasksWithoutProjectsData>();
            TMEmployeeTimeRepository employeeTimeRepository = new TMEmployeeTimeRepository(tenant);
            IQueryable<TMEmployeeTime> iQueryable = employeeTimeRepository.GetTasksWithoutProject(tenant);
            ContactRepository contactRepository = new ContactRepository(tenant);

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_EmployeeUserId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "EmployeeUserId").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();

            DateTime? fromDate = null;
            DateTime? toDate = null;
            string employeeUserId = null;

            if (filterItem_FromDate != null)
            {
                if (filterItem_FromDate.FieldValue != null)
                {
                    fromDate = (DateTime)filterItem_FromDate.FieldValue;
                }
            }

            if (filterItem_ToDate != null)
            {
                if (filterItem_ToDate.FieldValue != null)
                {
                    toDate = (DateTime)filterItem_ToDate.FieldValue;
                }
            }

            if (filterItem_EmployeeUserId != null)
            {
                if (filterItem_EmployeeUserId.FieldValue != null)
                {
                    employeeUserId = filterItem_EmployeeUserId.FieldValue.ToString();
                }
            }

            if (fromDate != null && toDate != null)
            {
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
            }

            if (!string.IsNullOrEmpty(employeeUserId))
            {
                iQueryable = iQueryable.Where(d => d.EmployeeUserId == employeeUserId);
                Contact contact = contactRepository.GetSingleContact(employeeUserId, tenant);
                if (contact != null)
                {
                    result.EmployeeUserName = contact.EnglishName;
                }
            }

            result.FromDate = fromDate.Value;
            result.ToDate = toDate.Value;
            result.EmployeeUserId = employeeUserId;
            var dateList = iQueryable.ToList();

            foreach (var item in dateList)
            {
                var taskItem = new TasksWithoutProjectsData();
                string contact_Name = "";
                if (!string.IsNullOrEmpty(result.EmployeeUserId))
                {
                    contact_Name = result.EmployeeUserName;
                }
                else
                {
                    Contact contact = contactRepository.GetSingleContact(item.EmployeeUserId, tenant);
                    if (contact != null)
                    {
                        contact_Name = contact.EnglishName;
                    }
                }

                taskItem.EmployeeName = contact_Name;
                taskItem.DateOfWork = item.DateOfWork;
                taskItem.Description = item.Description;
                taskItem.WINumber = item.WINumber;
                result.TasksWithoutProjectsList.Add(taskItem);
            }
            return result;
        }
        #endregion

        #region AccountingAging
        public byte[] LoadAccountingAgingDataProvider(byte[] xmlFilters, int tenant)
        {
            AccountingAgingDataProvider dataprovider = GetAccountingAgingDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(AccountingAgingDataProvider), tenant);
        }

        private AccountingAgingDataProvider GetAccountingAgingDataProvider(byte[] xmlFilters, int tenant)
        {

            AgingReportDataProviderLoader agingReportLoader = new AgingReportDataProviderLoader(tenant);

            return agingReportLoader.LoadFromXML(xmlFilters);

        }


        public byte[] LoadCustomerStatusDataProvider(byte[] xmlFilters, int tenant)
        {
            CustomerStatusDataProvider dataprovider = GetCustomerStatusDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(CustomerStatusDataProvider), tenant);
        }
        private CustomerStatusDataProvider GetCustomerStatusDataProvider(byte[] xmlFilters, int tenant)
        {
            CustomerStatusDataProviderLoader customerStatusDataProvider = new CustomerStatusDataProviderLoader(tenant);

            return customerStatusDataProvider.LoadFromXML(xmlFilters);

        }
        #endregion

        #region Ledger Transaction report
        public byte[] LoadLedgerTransactionDataProvider(byte[] xmlFilters, ReportFliter reportFliter, int tenant)
        {
            LedgerTransactionsDataProvider dataprovider = GetLedgerTransactionsDataProvider(xmlFilters, tenant);
            byte[] bytearray = new ReportMemoryStreamService().Convert(dataprovider, typeof(LedgerTransactionsDataProvider), tenant);
            ReportManipulationDataService reportManipulationDataService = new ReportManipulationDataService(dataprovider, reportFliter);
            bytearray = reportManipulationDataService.IsDataProviderHaveListWithValues() ? bytearray : null;
            return bytearray;
        }

        public LedgerTransactionsDataProvider GetLedgerTransactionsDataProvider(byte[] xmlFilters, int tenant)
        {
            LedgerTransactionReportLoader transactionReportLoader = new LedgerTransactionReportLoader(tenant);

            return transactionReportLoader.LoadFromXML(xmlFilters);
        }

        T GetQueryFilterItemValue<T>(QueryFilterItem filterItem)
        {
            if (filterItem?.FieldValue != null)
            {
                return (T)filterItem.FieldValue;
            }
            return default(T);
        }
        #endregion

        #region Open Shipments By Customer
        public byte[] LoadOpenShipmentsByCustomerDataProvider(byte[] xmlFilters, ReportFliter reportFliter, int tenant)
        {
            OpenShipmentsByCustomerDataProvider dataprovider = GetOpenShipmentsByCustomerDataProvider(xmlFilters, tenant);
            byte[] bytearray = new ReportMemoryStreamService().Convert(dataprovider, typeof(OpenShipmentsByCustomerDataProvider), tenant);
            ReportManipulationDataService reportManipulationDataService = new ReportManipulationDataService(dataprovider, reportFliter);
            bytearray = reportManipulationDataService.IsDataProviderHaveListWithValues() ? bytearray : null;
            return bytearray;

        }

        private OpenShipmentsByCustomerDataProvider GetOpenShipmentsByCustomerDataProvider(byte[] xmlFilters, int tenant)
        {
            WebServiceHelper servicHelper = new WebServiceHelper(tenant);
            OpenShipmentsByCustomerDataProvider totalData = new OpenShipmentsByCustomerDataProvider();
            totalData.OpenShipmentsRecordList = new List<OpenShipmentsRecord>();

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            AddressRepository addressRepository = new AddressRepository(commonContext);
            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            IQueryable<ShipmentDataView> iQueryable = shipmentRepository.GetShipmentViewsByTenant(tenant);

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_CustomerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();

            string customerId = null;
            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    customerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }

            #endregion

            #region Base Data Filtered

            iQueryable = iQueryable.Where(d => !d.IsOperationalClosed);
            iQueryable = iQueryable.Where(d => !d.IsCancelled);
            iQueryable = iQueryable.Where(d => d.ShipmentLevelCode != "C");

            if (!string.IsNullOrEmpty(customerId))
            {
                iQueryable = iQueryable.Where(d => d.CustomerId == customerId);
            }

            #endregion

            #region Fill Report Data

            totalData.Logo = WebFreight.Web.DataProviders.General.GetLogo(tenant);

            if (!string.IsNullOrEmpty(customerId))
            {
                CardRepository cardRepository = new CardRepository(tenant);
                Card customerCard = cardRepository.GetSingleCard(customerId, tenant);
                if (customerCard != null)
                {
                    totalData.SelectedCustomerName = customerCard.EnglishName;
                }
            }

            List<ShipmentDataView> myData = iQueryable.ToList();

            if (myData.Count > 0)
            {
                OpenShipmentsRecord myRecord = null;

                foreach (ShipmentDataView a in myData)
                {
                    myRecord = new OpenShipmentsRecord();
                    myRecord.TransPortModeCode = a.TransportModeId;
                    myRecord.TransPortModeName = a.TransportModeName;
                    myRecord.DirectionCode = a.DirectionId;
                    myRecord.DirectionName = a.DirectionName;
                    myRecord.ShipmentNumber = a.ShipmentNumber;
                    myRecord.CreateDate = a.CreateDateTime;
                    myRecord.CustomerName = a.CustomerName;
                    myRecord.ShipperName = a.ShipperName;
                    myRecord.Status = a.StatusName;
                    myRecord.LastSharedEvent = a.LastSharedEventName;
                    myRecord.IncotermCode = a.IncotermCode;
                    myRecord.ETD = a.MainCarriageETD;
                    myRecord.ExpectedDate = a.MainCarriageETA;
                    myRecord.AgentName = a.AgentName;
                    myRecord.CustomerReference1 = a.CustomerReference1;
                    myRecord.VolumInCBM = a.VolumeInCBM;
                    myRecord.TEU = a.TEU;
                    myRecord.GrossWeight = a.GrossWeight;
                    myRecord.ChargeableWeight = a.ChargeableWeight;
                    myRecord.DescriptionOfGoods = a.DescriptionOfGoods;
                    myRecord.Notes = a.Notes;
                    myRecord.MasterNumber = a.Master;
                    myRecord.HouseNumber = a.House;
                    myRecord.NumberOfContainers = a.NumberOfContainers;
                    myRecord.ETA = a.MainCarriageETA;
                    myRecord.ATA = a.MainCarriageATA;
                    myRecord.IsCancelled = a.IsCancelled;
                    myRecord.LastSharedEventDate = a.LastSharedEventDate;
                    myRecord.LastSharedEventNote = a.LastSharedEventNotes;
                    myRecord.PortOfLoading = a.MainCarriageFromPortCode;
                    myRecord.PortOfDischarge = a.MainCarriageFinalDestinationPortCode;
                    myRecord.PortOfLoadingName = a.MainCarriageFromPortName;
                    myRecord.PortOfDischargeName = a.MainCarriageFinalDestinationPortName;
                    myRecord.Consignee = a.ConsigneeName;
                    myRecord.BookingNumber = a.BookingConfirmationNumber;
                    myRecord.Transshipment1ETA = a.Transshipment1ETA;
                    myRecord.Transshipment2ETA = a.Transshipment2ETA;
                    myRecord.Transshipment3ETA = a.Transshipment3ETA;
                    myRecord.Transshipment1ETD = a.Transshipment1ETD;
                    myRecord.Transshipment2ETD = a.Transshipment2ETD;
                    myRecord.Transshipment3ETD = a.Transshipment3ETD;
                    myRecord.ShippingLine = a.MainCarriageCarrierName;
                    myRecord.Vessel = a.MainCarriageVesselName;
                    myRecord.Voyage = a.MainCarriageCarrierNumber;
                    myRecord.FullStatus = a.StatusName;
                    myRecord.FistPickupFromAddress = a.FirstPickupFullAddress;
                    myRecord.LastDeliveryToAddress = a.LastDeliveryFullAddress;
                    myRecord.MainCarriageATD = a.MainCarriageATD;
                    myRecord.ShipperReference1 = a.ShipperReference1;
                    myRecord.ShipperReference2 = a.ShipperReference2;
                    if (!string.IsNullOrEmpty(a.StatusLocation))
                    {
                        myRecord.FullStatus = a.StatusName + "(" + a.StatusLocation + ")";
                    }

                    if (!string.IsNullOrEmpty(a.ShipmentTypeId))
                    {
                        myRecord.ShipmentType = a.ShipmentTypeId + " " + a.ShipmentLevelName;
                    }

                    else
                    {
                        myRecord.ShipmentType = a.ShipmentLevelName;
                    }

                    string myRoutingField = null;
                    if (a.DirectionId == "D" && a.TransportModeId == "I")
                    {
                        InlandDomesticArgs args = new InlandDomesticArgs()
                        {
                            InlandDomesticFromTypeCode = a.InlandDomesticFromTypeCode,
                            MainCarriageFromAddressId = a.MainCarriageFromAddressId,
                            MainCarriageFromPortId = a.MainCarriageFromPortId,
                            InlandDomesticFromCity = a.InlandDomesticFromCity,
                            InlandDomesticFromCountryId = a.InlandDomesticFromCountryId,
                            InlandDomesticToTypeCode = a.InlandDomesticToTypeCode,
                            MainCarriageToAddressId = a.MainCarriageToAddressId,
                            InlandDomesticToCity = a.InlandDomesticToCity,
                            InlandDomesticToCountryId = a.InlandDomesticToCountryId,
                            MainCarriageToPortId = a.MainCarriageToPortId,
                            MainCarriageFromPortCountryCode = a.MainCarriageFromPortCountryCode,
                            MainCarriageToPortCountryCode = a.MainCarriageToPortCountryCode
                        };
                        myRoutingField = servicHelper.GetInlandDomesticRouting(args);
                    }

                    else
                    {
                        if (a.ShipmentLevelCode == "H" && a.MasterShipmentDataId == null)
                        {
                            PortPM fromPort = PortQuery.GetSinglePort(tenant, a.FromPortId, false);
                            PortPM toPort = PortQuery.GetSinglePort(tenant, a.ToPortId, false);
                            myRoutingField = fromPort.Code + " , " + toPort.Code;
                        }

                        else
                        {
                            myRoutingField = a.MainCarriageFromPortCode + " , " + a.MainCarriageFinalDestinationPortCode;
                        }
                    }

                    myRecord.Routing = myRoutingField;

                    StringBuilder str = new StringBuilder();
                    StringBuilder str2 = new StringBuilder();
                    List<ShipmentPackage> packages = shipmentsContext.ShipmentPackages.Where(d => d.ShipmentId == a.Id && d.Tenant == tenant).ToList();

                    myRecord.PackagesCount = packages.Count;

                    int insideCount = 0;
                    foreach (ShipmentPackage package in packages)
                    {
                        List<InsideShipmentPackage> insidePackages = shipmentsContext.InsideShipmentPackages.Where(d => d.ShipmentPackageId == package.Id && d.Tenant == package.Tenant).ToList();
                        insideCount += insidePackages.Count;

                        PackageType packagetype = (from pa in commonContext.PackageTypes
                                                   where pa.Id == package.PackageTypeId
                                                   select pa).FirstOrDefault();

                        if (packagetype != null)
                        {
                            if (packagetype.IsContainer && !string.IsNullOrEmpty(package.ContainerNumber))
                            {
                                string containerNo = package.ContainerNumber;
                                str.Append(containerNo);
                                str.Append(',');

                                string type = !string.IsNullOrEmpty(packagetype.PrintAs) ? packagetype.PrintAs : packagetype.Code;

                                str2.Append(containerNo);
                                str2.Append(' ');
                                str2.Append(type);
                                str2.Append(',');
                            }
                        }
                    }

                    myRecord.InsidePackagesCount = insideCount;

                    string str_String = str.ToString();
                    if (!string.IsNullOrEmpty(str_String))
                    {
                        str_String = str_String.TrimEnd(',');
                    }

                    string str2_String = str2.ToString();
                    if (!string.IsNullOrEmpty(str2_String))
                    {
                        str2_String = str2_String.TrimEnd(',');
                    }

                    myRecord.ContainersNumbersArray = str_String;
                    myRecord.ContainersNumbersAndTypesArray = str2_String;

                    totalData.OpenShipmentsRecordList.Add(myRecord);
                }
            }

            #endregion

            return totalData;
        }
        #endregion

        #region Parent Vs Child Tenants
        public byte[] LoadParentVsChildTenantsDataProvider(byte[] xmlFilters, int tenant)
        {
            ParentVsChildTenantsDataProvider dataprovider = GetParentVsChildTenantsDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(ParentVsChildTenantsDataProvider), tenant);
        }

        private ParentVsChildTenantsDataProvider GetParentVsChildTenantsDataProvider(byte[] xmlFilters, int tenant)
        {
            ParentVsChildTenantsDataProvider totalData = new ParentVsChildTenantsDataProvider();
            totalData.ParentTenantRecordList = new List<ParentTenantRecord>();

            TenantManagementRepository repository = new TenantManagementRepository();
            UserRepository userRepository = new UserRepository(tenant);

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_ParentTenantId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ParentTenantId").FirstOrDefault();

            string parentTenantId = null;
            if (filterItem_ParentTenantId != null)
            {
                if (filterItem_ParentTenantId.FieldValue != null)
                {
                    parentTenantId = filterItem_ParentTenantId.FieldValue.ToString();
                }
            }

            #endregion

            IQueryable<TenantManagement> allTenants = repository.GetAllTenants();
            IQueryable<TenantManagement> parentTenants = allTenants.Where(d => d.IsParentTenant);

            List<int> parentTenantIds = parentTenants.Select(s => s.Id).ToList();
            List<User> parentTenantUsers = userRepository.GetUsersFromTenantsList(parentTenantIds);

            IQueryable<TenantManagement> childTenants = allTenants.Where(d => (d.FreeUsers != null && d.FreeUsers != 0) && (d.GlobalTenant != null && d.GlobalTenant.IsActive));
            List<int> childTenantIds = childTenants.Select(s => s.Id).ToList();
            List<User> childTenantUsers = userRepository.GetUsersFromTenantsList(childTenantIds);

            List<TenantManagement> myData = childTenants.ToList();

            if (!string.IsNullOrEmpty(parentTenantId))
            {
                myData = myData.Where(d => d.ParentTenantId.ToString() == parentTenantId).ToList();
            }

            if (myData.Count > 0)
            {
                ParentTenantRecord myRecord = null;

                foreach (TenantManagement a in myData)
                {
                    myRecord = new ParentTenantRecord();
                    List<User> childList = childTenantUsers.Where(d => d.Tenant == a.Id).ToList();

                    myRecord.TenantId = a.Id;
                    myRecord.TenantName = a.Name;
                    myRecord.NumberOfUsers = a.NumberOfUsers;
                    myRecord.FreeUsers = a.FreeUsers;
                    myRecord.ParentTenantId = a.ParentTenantId;

                    if (a.IsMultiPackage)
                    {
                        myRecord.Package = "Multi-package";
                    }
                    else
                    {
                        myRecord.Package = a.PackageName;
                    }

                    if (a.ParentTenantId != null)
                    {
                        List<User> parentList = parentTenantUsers.Where(d => d.Tenant == a.ParentTenantId).ToList();

                        TenantManagement parentTenant = repository.GetSingleTenantManagement(a.ParentTenantId.Value);
                        if (parentTenant != null)
                        {
                            myRecord.ParentTenantName = parentTenant.Name;

                            if (parentTenant.IsMultiPackage)
                            {
                                myRecord.ParentTenantPackage = "Multi-package";
                            }
                            else
                            {
                                myRecord.ParentTenantPackage = parentTenant.PackageName;
                            }
                        }

                        List<string> commonList = parentList.Select(s1 => s1.Contact.Email.ToLower()).Intersect(childList.Select(s2 => s2.Contact.Email.ToLower())).ToList();

                        if (commonList.Count < a.FreeUsers)
                        {
                            myRecord.NumberOfInvalidUsers = a.FreeUsers.Value - commonList.Count;
                        }
                    }

                    totalData.ParentTenantRecordList.Add(myRecord);
                }
            }

            return totalData;
        }
        #endregion

        #region Revenue Expense
        public byte[] LoadRevenueExpenseDataProvider(byte[] xmlFilters, int tenant)
        {
            RevenueExpenseDataProvider dataprovider = GetRevenueExpenseDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(RevenueExpenseDataProvider), tenant);
        }

        private RevenueExpenseDataProvider GetRevenueExpenseDataProvider(byte[] xmlFilters, int tenant)
        {
            RevenueExpenseDataProvider totalData = new DataProviders.RevenueExpenseDataProvider();

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_tODate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CreateDate").FirstOrDefault();
            QueryFilterItem filterItem_level = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Level").FirstOrDefault();
            QueryFilterItem filterItem_card = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CardFilter").FirstOrDefault();
            QueryFilterItem filterItem_fromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ChartOfAccountsIdList = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ChartOfAccountsIdList").FirstOrDefault();
            QueryFilterItem filterItem_ChartOfAccountsTypeCodeList = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ChartOfAccountsTypeCodeList").FirstOrDefault();

            //ChartOfAccountsIdList
            List<string> chartOfAccountsIdList = null;
            if (filterItem_ChartOfAccountsIdList != null)
            {
                if (filterItem_ChartOfAccountsIdList.FieldValue != null)
                {
                    var chartOfAccountsCodeListString = (string)filterItem_ChartOfAccountsIdList.FieldValue;
                    chartOfAccountsIdList = chartOfAccountsCodeListString.Split(',').ToList();
                }
            }

            //ChartOfAccountsTypeCodeList
            List<string> chartOfAccountsTypeCodeList = null;
            if (filterItem_ChartOfAccountsTypeCodeList != null)
            {
                if (filterItem_ChartOfAccountsTypeCodeList.FieldValue != null)
                {
                    var chartOfAccountsTypeCodeListString = (string)filterItem_ChartOfAccountsTypeCodeList.FieldValue;
                    chartOfAccountsTypeCodeList = chartOfAccountsTypeCodeListString.Split(',').ToList();
                }
            }

            //ToDate
            DateTime? toDate = null;
            if (filterItem_tODate != null)
            {
                if (filterItem_tODate.FieldValue != null)
                {
                    toDate = (DateTime)filterItem_tODate.FieldValue;
                }
            }
            //FromDate
            DateTime? fromDate = null;
            if (filterItem_fromDate != null)
            {
                if (filterItem_fromDate.FieldValue != null)
                {
                    fromDate = (DateTime)filterItem_fromDate.FieldValue;
                }
            }
            //Level
            string level = null;
            if (filterItem_level != null)
            {
                if (filterItem_level.FieldValue != null)
                {
                    level = (string)filterItem_level.FieldValue;
                }
            }

            //Level
            string card = null;
            if (filterItem_card != null)
            {
                if (filterItem_card.FieldValue != null)
                {
                    card = (string)filterItem_card.FieldValue;
                }
            }


            #endregion


            var revenueExpenseReportParam = new RevenueExpenseReportParam()
            {
                Tenant = tenant,
                //  MyRevenueExpenseReportLevel = ReportLevel.,
                ToDate = (DateTime)toDate,
                FromDate = (DateTime)fromDate
            };

            switch (level)
            {
                //case "ChartOfAccountType":
                //    {
                //        revenueExpenseReportParam.MyRevenueExpenseReportLevel = ReportLevel.ChartofaccountType;
                //        break;
                //    }
                case "ChartOfAccount":
                    {
                        totalData.Level = " קבוצת מאזן";
                        //revenueExpenseReportParam.MyRevenueExpenseReportLevel = ReportLevel.Chartofaccount;
                        break;
                    }
                case "GLAccount":
                    {
                        totalData.Level = "כרטיס";
                        // revenueExpenseReportParam.MyRevenueExpenseReportLevel = ReportLevel.GLAccount;
                        break;
                    }
            }

            switch (card)
            {
                case "0":
                    {
                        revenueExpenseReportParam.MyCardFilter = CardFilterEnum.DoNotShowCardWithZeroBalance;
                        break;
                    }
                case "1":
                    {
                        revenueExpenseReportParam.MyCardFilter = CardFilterEnum.ShowCardsWithActivity_EvenBalanceItsZero;
                        break;
                    }
                case "2":
                    {
                        revenueExpenseReportParam.MyCardFilter = CardFilterEnum.ShowAllCard;
                        break;
                    }

            }
            totalData.ForDate = toDate;
            totalData.FromDate = fromDate;
            List<RevenueExpenseReportM> result = null;
            List<string> GLAccountParents = new List<string>();

            totalData.ResultList = new List<ResultList>();
            if (chartOfAccountsTypeCodeList != null)
                revenueExpenseReportParam.ChartOfAccountsTypes = chartOfAccountsTypeCodeList;
            if (chartOfAccountsIdList != null)
                revenueExpenseReportParam.ChartOfAccounts = chartOfAccountsIdList;

            if (level == "GLAccount")
            {
                RevenueExpenseReportService servce = new RevenueExpenseReportService(revenueExpenseReportParam, 5);
                revenueExpenseReportParam.MyRevenueExpenseReportLevel = ReportLevel.GLAccount;


                try
                {
                    servce.Execute();

                    result = servce.result;

                }
                finally
                {
                    //servce.Dispose();
                }


                GLAccountQueryService queryService = new GLAccountQueryService(tenant);
                ChartOfAccountQueryService chartQuaryService = new ChartOfAccountQueryService(tenant);




                #region Fill Report Data



                foreach (var item in result)
                {
                    ResultList record = new ResultList()
                    {
                        Id = item.GLAccountId,
                        Name = item.GLAccountNumber + "-" + item.GLAccountName,
                        AccountDisplayNumber = item.GLAccountNumber,
                        AccountName = item.GLAccountName,
                        ParentId = item.ChartOfAccountId,
                        Balance = item.LocalCloseBalancePeriod1,
                    };

                    GLAccountParents.Add(record.ParentId);
                    totalData.ResultList.Add(record);

                }
            }






            revenueExpenseReportParam.MyCardFilter = CardFilterEnum.DoNotShowCardWithZeroBalance;
            RevenueExpenseReportService service = new RevenueExpenseReportService(revenueExpenseReportParam, 5);
            revenueExpenseReportParam.MyRevenueExpenseReportLevel = ReportLevel.Chartofaccount;
            try
            {
                var res = service.Execute();

                var bytes = LogitudeXmlSerializer.SerializeObject<List<RevenueExpenseReportM>>(res);
                var ds = new DataSet();

                ds.ReadXml(new MemoryStream(bytes));

                result = service.result;
            }
            finally
            {
                //service.Dispose();
            }



            List<RevenueExpenseReportM> ChartOfAccount5s = result.Where(d => d.ChartOfAcountName5 != "").ToList();
            if (ChartOfAccount5s.Count > 0)
            {
                foreach (var item in ChartOfAccount5s)
                {
                    ResultList record = new ResultList()
                    {
                        Id = item.ChartOfAcount5,
                        Name = item.ChartOfAcountCode5 + "-" + item.ChartOfAcountName5,
                        AccountDisplayNumber = item.ChartOfAcountCode5,
                        AccountName = item.ChartOfAcountName5,
                        Number = null,
                        ParentId = item.ChartOfAcount4,
                        Balance = item.LocalCloseBalancePeriod1,
                    };



                    var duplicated = totalData.ResultList.Where(d => d.Id == record.Id).FirstOrDefault();

                    if (duplicated == null)
                    {
                        totalData.ResultList.Add(record);
                    }


                }
            }

            List<RevenueExpenseReportM> ChartOfAccount4s = result.Where(d => d.ChartOfAcountName4 != "").ToList();
            if (ChartOfAccount4s.Count > 0)
            {
                foreach (var item in ChartOfAccount4s)
                {
                    ResultList record = new ResultList()
                    {
                        Id = item.ChartOfAcount4,
                        Name = item.ChartOfAcountCode4 + "-" + item.ChartOfAcountName4,
                        AccountDisplayNumber = item.ChartOfAcountCode4,
                        AccountName = item.ChartOfAcountName4,
                        Number = null,
                        ParentId = item.ChartOfAcount3,
                        Balance = item.LocalCloseBalancePeriod1,
                    };

                    if (record.Balance == null)
                    {
                        bool exist = GLAccountParents.Contains(record.Id);
                        if (!exist)
                        {

                            var child = totalData.ResultList.Where(d => d.ParentId == record.Id).FirstOrDefault();
                            if (child != null)
                            {
                                record.Balance = child.Balance;
                            }
                        }
                    }

                    var duplicated = totalData.ResultList.Where(d => d.Id == record.Id).FirstOrDefault();

                    if (duplicated == null)
                    {
                        totalData.ResultList.Add(record);
                    }

                }
            }

            List<RevenueExpenseReportM> ChartOfAccount3s = result.Where(d => d.ChartOfAcountName3 != "").ToList();
            if (ChartOfAccount3s.Count > 0)
            {
                foreach (var item in ChartOfAccount3s)
                {
                    ResultList record = new ResultList()
                    {
                        Id = item.ChartOfAcount3,
                        Name = item.ChartOfAcountCode3 + "-" + item.ChartOfAcountName3,
                        AccountDisplayNumber = item.ChartOfAcountCode3,
                        AccountName = item.ChartOfAcountName3,
                        Number = null,
                        ParentId = item.ChartOfAcount2,
                        Balance = item.LocalCloseBalancePeriod1,
                    };
                    if (record.Balance == null)
                    {
                        bool exist = GLAccountParents.Contains(record.Id);
                        if (!exist)
                        {

                            var child = totalData.ResultList.Where(d => d.ParentId == record.Id).FirstOrDefault();
                            if (child != null)
                            {
                                record.Balance = child.Balance;
                            }
                        }
                    }
                    var duplicated = totalData.ResultList.Where(d => d.Id == record.Id).FirstOrDefault();

                    if (duplicated == null)
                    {
                        totalData.ResultList.Add(record);
                    }


                }
            }

            List<RevenueExpenseReportM> ChartOfAccount2s = result.Where(d => d.ChartOfAcountName2 != "").ToList();
            if (ChartOfAccount2s.Count > 0)
            {
                foreach (var item in ChartOfAccount2s)
                {

                    ResultList record = new ResultList()
                    {
                        Id = item.ChartOfAcount2,
                        Name = item.ChartOfAcountCode2 + "-" + item.ChartOfAcountName2,
                        AccountDisplayNumber = item.ChartOfAcountCode2,
                        AccountName = item.ChartOfAcountName2,
                        Number = null,
                        ParentId = item.ChartOfAcount1,
                        Balance = item.LocalCloseBalancePeriod1,
                    };

                    if (record.Balance == null)
                    {
                        bool exist = GLAccountParents.Contains(record.Id);
                        if (!exist)
                        {

                            var child = totalData.ResultList.Where(d => d.ParentId == record.Id).FirstOrDefault();
                            if (child != null)
                            {
                                record.Balance = child.Balance;
                            }
                        }
                    }
                    var duplicated = totalData.ResultList.Where(d => d.Id == record.Id).FirstOrDefault();

                    if (duplicated == null)
                    {
                        totalData.ResultList.Add(record);
                    }


                }
            }

            List<RevenueExpenseReportM> ChartOfAccount1s = result.Where(d => d.ChartOfAcountName1 != "").ToList();
            if (ChartOfAccount1s.Count > 0)
            {
                foreach (var item in ChartOfAccount1s)
                {
                    ResultList record = new ResultList()
                    {
                        Id = item.ChartOfAcount1,
                        Name = item.ChartOfAcountCode1 + "-" + item.ChartOfAcountName1,
                        AccountDisplayNumber = item.ChartOfAcountCode1,
                        AccountName = item.ChartOfAcountName1,
                        Number = null,
                        ParentId = item.ChartOfAcountType,
                        Balance = item.LocalCloseBalancePeriod1,
                    };
                    if (record.Balance == null)
                    {
                        bool exist = GLAccountParents.Contains(record.Id);
                        if (!exist)
                        {

                            var child = totalData.ResultList.Where(d => d.ParentId == record.Id).FirstOrDefault();
                            if (child != null)
                            {
                                record.Balance = child.Balance;
                            }
                        }
                    }
                    var duplicated = totalData.ResultList.Where(d => d.Id == record.Id).FirstOrDefault();

                    if (duplicated == null)
                    {
                        totalData.ResultList.Add(record);
                    }

                }
            }

            revenueExpenseReportParam.MyRevenueExpenseReportLevel = ReportLevel.ChartofaccountType;
            try
            {
                service.Execute();

                result = service.result;
            }
            finally
            {
                //service.Dispose();
            }


            foreach (var item in result)
            {
                ResultList record = new ResultList()
                {
                    Id = item.ChartOfAcountType,
                    Name = item.ChartOfAcountType == "1" ? "1-הכנסות" : "2-הוצאות",
                    Number = null,
                    ParentId = null,
                    Balance = item.LocalCloseBalancePeriod1,
                };

                totalData.ResultList.Add(record);

            }


            totalData.ResultList.OrderBy(d => d.Name);

            var revenues = result.Where(d => d.ChartOfAcountType == "1").FirstOrDefault();
            decimal? totalRevenues = null;
            if (revenues != null)
            {
                totalRevenues = revenues.LocalCloseBalancePeriod1;
            }
            var expenses = result.Where(d => d.ChartOfAcountType == "2").FirstOrDefault();
            decimal? totalExpenses = null;
            if (expenses != null)
            {
                totalExpenses = expenses.LocalCloseBalancePeriod1;
            }


            totalData.TotalRevenueExpense = (totalRevenues == null ? 0 : totalRevenues) + (totalExpenses == null ? 0 : totalExpenses);


            #endregion

            return totalData;
        }


        #endregion

        #region Trail Balance
        public byte[] LoadTrailBalanceDataProvider(byte[] xmlFilters, int tenant)
        {
            RevenueExpenseDataProvider dataprovider = GetTrailBalanceDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(RevenueExpenseDataProvider), tenant);
        }
        bool showLocals;

        private readonly object _locker = new object();
        private static readonly NLog.Logger Logger = NLog.LogManager.GetLogger("AmitalLogger");
        private static int executionCount = 1;
        private RevenueExpenseDataProvider GetTrailBalanceDataProvider(byte[] xmlFilters, int tenant)
        {
            RevenueExpenseDataProvider totalData = new DataProviders.RevenueExpenseDataProvider();
            List<ChartOfAccountsTypePM> chartOfAccountTypes = GetChartOfAccountTypes(tenant);
            List<ChartOfAccount> ChartOfAccountsList = GetChartOfAccounts(tenant);

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_fromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_level = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Level").FirstOrDefault();
            QueryFilterItem filterItem_toDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_currency = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CurrencyDetailed").FirstOrDefault();
            QueryFilterItem filterItem_Category1 = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Category1").FirstOrDefault();
            QueryFilterItem filterItem_Category2 = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Category2").FirstOrDefault();
            QueryFilterItem filterItem_Category3 = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Category3").FirstOrDefault();
            QueryFilterItem filterItem_Category4 = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Category4").FirstOrDefault();

            QueryFilterItem filterItem_ChartOfAccountId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ChartOfAccountId").FirstOrDefault();
            QueryFilterItem filterItem_DontShowCardsWith0Balance = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DontShowCardsWith0Balance").FirstOrDefault();
            QueryFilterItem filterItem_Category5 = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Category5").FirstOrDefault();
            QueryFilterItem filterItem_DetailedCustomer = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DetailedForCustomers").FirstOrDefault();
            QueryFilterItem filterItem_DetailedVendor = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DetailedForVendors").FirstOrDefault();
            QueryFilterItem filterItem_DetailedJobs = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DetailedForJobs").FirstOrDefault();
            QueryFilterItem filterItem_DetailedFiles = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DetailedForFiles").FirstOrDefault();


            //ChartOfAccountId
            string ChartOfAccountId = null;
            if (filterItem_ChartOfAccountId != null)
            {
                if (filterItem_ChartOfAccountId.FieldValue != null)
                {
                    ChartOfAccountId = (string)filterItem_ChartOfAccountId.FieldValue;
                }
            }
            //UseBalanceFilter
            bool dontShowCardsWith0Balance = false;
            if (filterItem_DontShowCardsWith0Balance != null)
            {
                if (filterItem_DontShowCardsWith0Balance.FieldValue != null)
                {
                    dontShowCardsWith0Balance = (bool)filterItem_DontShowCardsWith0Balance.FieldValue;
                }
            }
            //category1
            string category1 = null;
            if (filterItem_Category1 != null)
            {
                if (filterItem_Category1.FieldValue != null)
                {
                    category1 = (string)filterItem_Category1.FieldValue;
                }
            }
            //category2
            string category2 = null;
            if (filterItem_Category2 != null)
            {
                if (filterItem_Category2.FieldValue != null)
                {
                    category2 = (string)filterItem_Category2.FieldValue;
                }
            }
            //category3
            string category3 = null;
            if (filterItem_Category3 != null)
            {
                if (filterItem_Category3.FieldValue != null)
                {
                    category3 = (string)filterItem_Category3.FieldValue;
                }
            }
            //category1
            string category4 = null;
            if (filterItem_Category4 != null)
            {
                if (filterItem_Category4.FieldValue != null)
                {
                    category4 = (string)filterItem_Category4.FieldValue;
                }
            }
            //category5
            string category5 = null;
            if (filterItem_Category5 != null)
            {
                if (filterItem_Category5.FieldValue != null)
                {
                    category5 = (string)filterItem_Category5.FieldValue;
                }
            }


            //fromDate
            DateTime? fromDate = null;
            if (filterItem_fromDate != null)
            {
                if (filterItem_fromDate.FieldValue != null)
                {
                    fromDate = (DateTime)filterItem_fromDate.FieldValue;
                }
            }

            //currency
            bool currency = false;
            if (filterItem_currency != null)
            {
                if (filterItem_currency.FieldValue != null)
                {
                    currency = (bool)filterItem_currency.FieldValue;
                    if (currency == true)
                    {
                        totalData.CurrencyDetailed = true;

                    }
                }
            }


            //ToDate
            DateTime? toDate = null;
            if (filterItem_toDate != null)
            {
                if (filterItem_toDate.FieldValue != null)
                {
                    toDate = (DateTime)filterItem_toDate.FieldValue;
                }
            }

            //Level
            string level = null;
            if (filterItem_level != null)
            {
                if (filterItem_level.FieldValue != null)
                {
                    level = (string)filterItem_level.FieldValue;
                }
            }

            bool customer = false;
            if (filterItem_DetailedCustomer != null)
            {
                if (filterItem_DetailedCustomer.FieldValue != null)
                {
                    customer = (bool)filterItem_DetailedCustomer.FieldValue;

                }
            }
            bool vendor = false;
            if (filterItem_DetailedVendor != null)
            {
                if (filterItem_DetailedVendor.FieldValue != null)
                {
                    vendor = (bool)filterItem_DetailedVendor.FieldValue;

                }
            }
            bool jobs = false;
            if (filterItem_DetailedJobs != null)
            {
                if (filterItem_DetailedJobs.FieldValue != null)
                {
                    jobs = (bool)filterItem_DetailedJobs.FieldValue;

                }
            }
            bool files = false;
            if (filterItem_DetailedFiles != null)
            {
                if (filterItem_DetailedFiles.FieldValue != null)
                {
                    files = (bool)filterItem_DetailedFiles.FieldValue;

                }
            }
            #endregion


            string category1Name = GetCategory1Name(category1, tenant);
            string category5Name = GetCategory5Name(category5, tenant);
            totalData.CurrencyDetailed = currency;
            ContactPM contact = GetLoggedContact(tenant);
            showLocals = !contact.DontShowLocal;
            totalData.DetailedCustomersAccounts = SetDetailedCustomersAccounts(customer);
            totalData.DetailedVendorsAccounts = SetDetailedVendorsAccounts(vendor);
            totalData.Category = category1Name != null ? category1Name : category5Name;
            totalData.DontShowCardsWith0Balance = dontShowCardsWith0Balance;
            totalData.FromDate = fromDate;
            totalData.ToDate = toDate;

            var trailReportParam = new TrailReportParam()
            {
                Tenant = tenant,
                //    MyRevenueExpenseReportLevel = ReportLevel.,
                ToDate = (DateTime)toDate,
                FromDate = (DateTime)fromDate,
                CurrenciesDetailed = (bool)currency,
                DetailedControlVendors = vendor,
                DetailedControlClients = customer,
                DetailedControlFile = files,
                DetailedControlJob = jobs,
                Category1 = category1,
                Category2 = category2,
                Category3 = category3,
                Category4 = category4,
                Category5 = category5,
                DoNotShowCardWithLocalCloseBalanceEqualZero = dontShowCardsWith0Balance,
                IsRevenueExpenseReport = false,
                //  Skip = true
                Suppress_DoNotShowCardWithoutActivity = false,


            };

            switch (level)
            {
                case "ChartOfAccountType":
                    {
                        totalData.Level = "סוג קבוצת מאזן";
                        //trailReportParam.MyTrailReportLevel = ReportLevel.ChartofaccountType;
                        break;
                    }
                case "ChartOfAccount":
                    {
                        totalData.Level = " קבוצת מאזן";
                        //revenueExpenseReportParam.MyRevenueExpenseReportLevel = ReportLevel.Chartofaccount;
                        break;
                    }
                case "GLAccount":
                    {
                        totalData.Level = "כרטיס";
                        // revenueExpenseReportParam.MyRevenueExpenseReportLevel = ReportLevel.GLAccount;
                        break;
                    }
            }

            List<TrailReportM> result = null;
            List<string> GLAccountParents = new List<string>();
            //  var service = null;// TrailReportFactory.CreateNew(trailReportParam);
            totalData.ResultList = new List<ResultList>();


            IAccountingContext context = AccountingContext.GetContext(tenant);
            ChartOfAccountsTypeQueryService typeQueryService = new ChartOfAccountsTypeQueryService(context);
            GLAccountQueryService queryService = new GLAccountQueryService(tenant);
            ChartOfAccountQueryService chartQuaryService = new ChartOfAccountQueryService(tenant);

            if (level == "GLAccount" || level == "ChartOfAccount" || level == "ChartOfAccountType")
            {

                trailReportParam.DetailedControlVendors = false;
                trailReportParam.DetailedControlClients = false;
                trailReportParam.DetailedControlJob = false;
                trailReportParam.DetailedControlFile = false;
                trailReportParam.CurrenciesDetailed = false;
                trailReportParam.Suppress_DoNotShowCardWithoutActivity = false;
                trailReportParam.DoNotShowCardWithLocalCloseBalanceEqualZero = false;
                trailReportParam.Category1 = null;
                trailReportParam.Category2 = null;
                trailReportParam.Category3 = null;
                trailReportParam.Category4 = null;
                trailReportParam.Category5 = null;
                trailReportParam.MyTrailReportLevel = ReportLevel.ChartofaccountType;
                var typeservice = TrailReportFactory.CreateNew(trailReportParam);
                List<TrailReportM> res1;
                try
                {
                    res1 = typeservice.Execute();
                }
                finally
                {
                    // typeservice.Dispose();
                }









                foreach (var item in res1)
                {
                    ChartOfAccountsTypePM chartType = null;
                    string typeName = null;
                    if (item != null)
                    {
                        if (!string.IsNullOrEmpty(item.ChartOfAcountType))
                        {
                            chartType = chartOfAccountTypes.FirstOrDefault(x => x.Code == item.ChartOfAcountType);
                            if (chartType != null) { typeName = chartType.LocalName; }
                        }
                        ResultList record = new ResultList()
                        {
                            Id = item.ChartOfAcountType,
                            Number = item.ChartOfAcountType,
                            Name = typeName,

                            ParentId = null,
                            LocalCloseBalance = item.LocalCloseBalance != null ? item.LocalCloseBalance : 0,
                            LocalCredit = item.LocalCredit != null ? item.LocalCredit : 0,
                            LocalDebit = item.LocalDebit != null ? item.LocalDebit : 0,
                            LocalOpenBalance = item.LocalOpenBalance != null ? item.LocalOpenBalance : 0,


                            ForeignCloseBalance = item.ForeignCloseBalance != null ? item.ForeignCloseBalance : 0,
                            ForeignCredit = item.ForeignCredit != null ? item.ForeignCredit : 0,
                            ForeignDebit = item.ForeignDebit != null ? item.ForeignDebit : 0,
                            ForeignOpenBalance = item.ForeignOpenBalance != null ? item.ForeignOpenBalance : 0,
                            ChartOfAccountTypeOrder = chartOfAccountTypes.FirstOrDefault(x => x.Code == item.ChartOfAcountType)?.Order,
                        };

                        var duplicated = totalData.ResultList.Where(d => d.Id == record.Id).FirstOrDefault();
                        if (duplicated == null)
                        {
                            if (!string.IsNullOrEmpty(record.Id))
                            {

                                totalData.ResultList.Add(record);
                            }
                        }

                    }
                }
            }



            if (level == "GLAccount" || level == "ChartOfAccount")
            {


                trailReportParam.DetailedControlVendors = false;
                trailReportParam.DetailedControlClients = false;
                trailReportParam.DetailedControlJob = false;
                trailReportParam.DetailedControlFile = false;
                trailReportParam.CurrenciesDetailed = false;
                trailReportParam.Suppress_DoNotShowCardWithoutActivity = false;
                trailReportParam.DoNotShowCardWithLocalCloseBalanceEqualZero = false;
                trailReportParam.Category1 = null;
                trailReportParam.Category2 = null;
                trailReportParam.Category3 = null;
                trailReportParam.Category4 = null;
                trailReportParam.Category5 = null;
                trailReportParam.MyTrailReportLevel = ReportLevel.Chartofaccount;
                var service = TrailReportFactory.CreateNew(trailReportParam);
                List<TrailReportM> res;
                try
                {
                    res = service.Execute();
                }
                finally
                {
                    //service.Dispose();
                }




                List<TrailReportM> ChartOfAccount5s = res.Where(d => !string.IsNullOrEmpty(d != null ? d.ChartOfAcountName5 : null)).ToList();
                if (ChartOfAccount5s.Count > 0)
                {
                    foreach (var item in ChartOfAccount5s)
                    {
                        ResultList record = new ResultList()
                        {
                            Id = item.ChartOfAcount5,
                            Name = item.ChartOfAcountName5,
                            Number = item.ChartOfAcountCode5,
                            ParentId = item.ChartOfAcount4,
                            LocalCloseBalance = item.LocalCloseBalance != null ? item.LocalCloseBalance : 0,
                            LocalCredit = item.LocalCredit != null ? item.LocalCredit : 0,
                            LocalDebit = item.LocalDebit != null ? item.LocalDebit : 0,
                            LocalOpenBalance = item.LocalOpenBalance != null ? item.LocalOpenBalance : 0,


                            ForeignCloseBalance = item.ForeignCloseBalance != null ? item.ForeignCloseBalance : 0,
                            ForeignCredit = item.ForeignCredit != null ? item.ForeignCredit : 0,
                            ForeignDebit = item.ForeignDebit != null ? item.ForeignDebit : 0,
                            ForeignOpenBalance = item.ForeignOpenBalance != null ? item.ForeignOpenBalance : 0,

                            Type = "ChartOfAccount"
                        };

                        ResultList parent = totalData.ResultList.Where(d => d.Id == record.ParentId).FirstOrDefault();
                        if (parent == null)
                        {
                            ChartOfAccountPM chartOfAccount = chartQuaryService.GetSinglePM(record.ParentId, tenant);
                            string name = null;
                            string code = null;
                            if (chartOfAccount != null)
                            {
                                name = chartOfAccount.LocalName;
                                code = chartOfAccount.Code;

                                ResultList parentrecord = new ResultList()
                                {
                                    Id = record.ParentId,
                                    Name = chartOfAccount.LocalName,
                                    Number = chartOfAccount.Code,
                                    ParentId = item.ChartOfAcount3,
                                    LocalCloseBalance = item.LocalCloseBalance != null ? item.LocalCloseBalance : 0,
                                    LocalCredit = item.LocalCredit != null ? item.LocalCredit : 0,
                                    LocalDebit = item.LocalDebit != null ? item.LocalDebit : 0,
                                    LocalOpenBalance = item.LocalOpenBalance != null ? item.LocalOpenBalance : 0,


                                    ForeignCloseBalance = item.ForeignCloseBalance != null ? item.ForeignCloseBalance : 0,
                                    ForeignCredit = item.ForeignCredit != null ? item.ForeignCredit : 0,
                                    ForeignDebit = item.ForeignDebit != null ? item.ForeignDebit : 0,
                                    ForeignOpenBalance = item.ForeignOpenBalance != null ? item.ForeignOpenBalance : 0,

                                    Type = "ChartOfAccount",
                                    Error = true,

                                };
                                totalData.ResultList.Add(parentrecord);
                            }
                        }


                        var duplicated = totalData.ResultList.Where(d => d.Id == record.Id).FirstOrDefault();

                        if (duplicated == null)
                        {
                            if (!string.IsNullOrEmpty(record.Id))
                            {
                                totalData.ResultList.Add(record);
                            }
                        }


                    }
                }
                List<TrailReportM> ChartOfAccount4s = res.Where(d => !string.IsNullOrEmpty(d != null ? d.ChartOfAcountName4 : null)).ToList();
                if (ChartOfAccount4s.Count > 0)
                {
                    foreach (var item in ChartOfAccount4s)
                    {
                        ResultList record = new ResultList()
                        {
                            Id = item.ChartOfAcount4,
                            Name = item.ChartOfAcountName4,
                            Number = item.ChartOfAcountCode4,
                            ParentId = item.ChartOfAcount3,
                            LocalCloseBalance = item.LocalCloseBalance != null ? item.LocalCloseBalance : 0,
                            LocalCredit = item.LocalCredit != null ? item.LocalCredit : 0,
                            LocalDebit = item.LocalDebit != null ? item.LocalDebit : 0,
                            LocalOpenBalance = item.LocalOpenBalance != null ? item.LocalOpenBalance : 0,


                            ForeignCloseBalance = item.ForeignCloseBalance != null ? item.ForeignCloseBalance : 0,
                            ForeignCredit = item.ForeignCredit != null ? item.ForeignCredit : 0,
                            ForeignDebit = item.ForeignDebit != null ? item.ForeignDebit : 0,
                            ForeignOpenBalance = item.ForeignOpenBalance != null ? item.ForeignOpenBalance : 0,

                            Type = "ChartOfAccount"

                        };

                        ResultList parent = totalData.ResultList.Where(d => d.Id == record.ParentId).FirstOrDefault();
                        if (parent == null)
                        {
                            ChartOfAccountPM chartOfAccount = chartQuaryService.GetSinglePM(record.ParentId, tenant);
                            string name = null;
                            string code = null;
                            if (chartOfAccount != null)
                            {
                                name = chartOfAccount.LocalName;
                                code = chartOfAccount.Code;

                                ResultList parentrecord = new ResultList()
                                {
                                    Id = record.ParentId,
                                    Name = chartOfAccount.LocalName,
                                    Number = chartOfAccount.Code,
                                    ParentId = item.ChartOfAcount2,
                                    LocalCloseBalance = item.LocalCloseBalance != null ? item.LocalCloseBalance : 0,
                                    LocalCredit = item.LocalCredit != null ? item.LocalCredit : 0,
                                    LocalDebit = item.LocalDebit != null ? item.LocalDebit : 0,
                                    LocalOpenBalance = item.LocalOpenBalance != null ? item.LocalOpenBalance : 0,


                                    ForeignCloseBalance = item.ForeignCloseBalance != null ? item.ForeignCloseBalance : 0,
                                    ForeignCredit = item.ForeignCredit != null ? item.ForeignCredit : 0,
                                    ForeignDebit = item.ForeignDebit != null ? item.ForeignDebit : 0,
                                    ForeignOpenBalance = item.ForeignOpenBalance != null ? item.ForeignOpenBalance : 0,

                                    Type = "ChartOfAccount",
                                    Error = true,

                                };
                                totalData.ResultList.Add(parentrecord);
                            }
                        }

                        if (record.Balance == null)
                        {
                            bool exist = GLAccountParents.Contains(record.Id);
                            if (!exist)
                            {

                                var child = totalData.ResultList.Where(d => d.ParentId == record.Id).FirstOrDefault();
                                if (child != null)
                                {
                                    record.Balance = child.Balance;
                                }
                            }
                        }

                        var duplicated = totalData.ResultList.Where(d => d.Id == record.Id).FirstOrDefault();

                        if (duplicated == null)
                        {
                            totalData.ResultList.Add(record);
                        }

                    }
                }

                List<TrailReportM> ChartOfAccount3s = res.Where(d => !string.IsNullOrEmpty(d != null ? d.ChartOfAcountName3 : null)).ToList();
                if (ChartOfAccount3s.Count > 0)
                {
                    foreach (var item in ChartOfAccount3s)
                    {
                        ResultList record = new ResultList()
                        {
                            Id = item.ChartOfAcount3,
                            Name = item.ChartOfAcountName3,
                            Number = item.ChartOfAcountCode3,
                            ParentId = item.ChartOfAcount2,
                            LocalCloseBalance = item.LocalCloseBalance != null ? item.LocalCloseBalance : 0,
                            LocalCredit = item.LocalCredit != null ? item.LocalCredit : 0,
                            LocalDebit = item.LocalDebit != null ? item.LocalDebit : 0,
                            LocalOpenBalance = item.LocalOpenBalance != null ? item.LocalOpenBalance : 0,


                            ForeignCloseBalance = item.ForeignCloseBalance != null ? item.ForeignCloseBalance : 0,
                            ForeignCredit = item.ForeignCredit != null ? item.ForeignCredit : 0,
                            ForeignDebit = item.ForeignDebit != null ? item.ForeignDebit : 0,
                            ForeignOpenBalance = item.ForeignOpenBalance != null ? item.ForeignOpenBalance : 0,

                            Type = "ChartOfAccount"
                        };

                        ResultList parent = totalData.ResultList.Where(d => d.Id == record.ParentId).FirstOrDefault();
                        if (parent == null)
                        {
                            ChartOfAccountPM chartOfAccount = chartQuaryService.GetSinglePM(record.ParentId, tenant);
                            string name = null;
                            string code = null;
                            if (chartOfAccount != null)
                            {
                                name = chartOfAccount.LocalName;
                                code = chartOfAccount.Code;

                                ResultList parentrecord = new ResultList()
                                {
                                    Id = record.ParentId,
                                    Name = chartOfAccount.LocalName,
                                    Number = chartOfAccount.Code,
                                    ParentId = item.ChartOfAcount1,
                                    LocalCloseBalance = item.LocalCloseBalance != null ? item.LocalCloseBalance : 0,
                                    LocalCredit = item.LocalCredit != null ? item.LocalCredit : 0,
                                    LocalDebit = item.LocalDebit != null ? item.LocalDebit : 0,
                                    LocalOpenBalance = item.LocalOpenBalance != null ? item.LocalOpenBalance : 0,


                                    ForeignCloseBalance = item.ForeignCloseBalance != null ? item.ForeignCloseBalance : 0,
                                    ForeignCredit = item.ForeignCredit != null ? item.ForeignCredit : 0,
                                    ForeignDebit = item.ForeignDebit != null ? item.ForeignDebit : 0,
                                    ForeignOpenBalance = item.ForeignOpenBalance != null ? item.ForeignOpenBalance : 0,

                                    Error = true,
                                    Type = "ChartOfAccount"

                                };
                                totalData.ResultList.Add(parentrecord);
                            }
                        }

                        if (record.Balance == null)
                        {
                            bool exist = GLAccountParents.Contains(record.Id);
                            if (!exist)
                            {

                                var child = totalData.ResultList.Where(d => d.ParentId == record.Id).FirstOrDefault();
                                if (child != null)
                                {
                                    record.Balance = child.Balance;
                                }
                            }
                        }
                        var duplicated = totalData.ResultList.Where(d => d.Id == record.Id).FirstOrDefault();

                        if (duplicated == null)
                        {
                            totalData.ResultList.Add(record);
                        }


                    }
                }


                List<TrailReportM> ChartOfAccount2s = res.Where(d => !string.IsNullOrEmpty(d != null ? d.ChartOfAcountName2 : null)).ToList();
                if (ChartOfAccount2s.Count > 0)
                {
                    foreach (var item in ChartOfAccount2s)
                    {

                        ResultList record = new ResultList()
                        {
                            Id = item.ChartOfAcount2,
                            Name = item.ChartOfAcountName2,
                            Number = item.ChartOfAcountCode2,
                            ParentId = item.ChartOfAcount1,
                            LocalCloseBalance = item.LocalCloseBalance != null ? item.LocalCloseBalance : 0,
                            LocalCredit = item.LocalCredit != null ? item.LocalCredit : 0,
                            LocalDebit = item.LocalDebit != null ? item.LocalDebit : 0,
                            LocalOpenBalance = item.LocalOpenBalance != null ? item.LocalOpenBalance : 0,


                            ForeignCloseBalance = item.ForeignCloseBalance != null ? item.ForeignCloseBalance : 0,
                            ForeignCredit = item.ForeignCredit != null ? item.ForeignCredit : 0,
                            ForeignDebit = item.ForeignDebit != null ? item.ForeignDebit : 0,
                            ForeignOpenBalance = item.ForeignOpenBalance != null ? item.ForeignOpenBalance : 0,

                            Type = "ChartOfAccount"
                        };
                        ResultList parent = totalData.ResultList.Where(d => d.Id == record.ParentId).FirstOrDefault();
                        if (parent == null)
                        {
                            ChartOfAccountPM chartOfAccount = chartQuaryService.GetSinglePM(record.ParentId, tenant);
                            string name = null;
                            string code = null;
                            if (chartOfAccount != null)
                            {
                                name = chartOfAccount.LocalName;
                                code = chartOfAccount.Code;

                                ResultList parentrecord = new ResultList()
                                {
                                    Id = record.ParentId,
                                    Name = chartOfAccount.LocalName,
                                    Number = chartOfAccount.Code,
                                    ParentId = item.ChartOfAcountType,
                                    LocalCloseBalance = item.LocalCloseBalance != null ? item.LocalCloseBalance : 0,
                                    LocalCredit = item.LocalCredit != null ? item.LocalCredit : 0,
                                    LocalDebit = item.LocalDebit != null ? item.LocalDebit : 0,
                                    LocalOpenBalance = item.LocalOpenBalance != null ? item.LocalOpenBalance : 0,


                                    ForeignCloseBalance = item.ForeignCloseBalance != null ? item.ForeignCloseBalance : 0,
                                    ForeignCredit = item.ForeignCredit != null ? item.ForeignCredit : 0,
                                    ForeignDebit = item.ForeignDebit != null ? item.ForeignDebit : 0,
                                    ForeignOpenBalance = item.ForeignOpenBalance != null ? item.ForeignOpenBalance : 0,
                                    ChartOfAccountTypeOrder = chartOfAccountTypes.FirstOrDefault(x => x.Code == item.ChartOfAcountType)?.Order,
                                    Error = true,

                                    Type = "ChartOfAccount"
                                };
                                totalData.ResultList.Add(parentrecord);
                            }
                        }

                        if (record.Balance == null)
                        {
                            bool exist = GLAccountParents.Contains(record.Id);
                            if (!exist)
                            {

                                var child = totalData.ResultList.Where(d => d.ParentId == record.Id).FirstOrDefault();
                                if (child != null)
                                {
                                    record.Balance = child.Balance;
                                }
                            }
                        }
                        var duplicated = totalData.ResultList.Where(d => d.Id == record.Id).FirstOrDefault();

                        if (duplicated == null)
                        {
                            totalData.ResultList.Add(record);
                        }


                    }
                }


                List<TrailReportM> ChartOfAccount1s = res.Where(d => !string.IsNullOrEmpty(d != null ? d.ChartOfAcountName1 : null)).ToList();
                if (ChartOfAccount1s.Count > 0)
                {
                    foreach (var item in ChartOfAccount1s)
                    {
                        ResultList record = new ResultList()
                        {
                            Id = item.ChartOfAcount1,
                            Name = item.ChartOfAcountName1,
                            Number = item.ChartOfAcountCode1,
                            ParentId = item.ChartOfAcountType,
                            LocalCloseBalance = item.LocalCloseBalance != null ? item.LocalCloseBalance : 0,
                            LocalCredit = item.LocalCredit != null ? item.LocalCredit : 0,
                            LocalDebit = item.LocalDebit != null ? item.LocalDebit : 0,
                            LocalOpenBalance = item.LocalOpenBalance != null ? item.LocalOpenBalance : 0,


                            ForeignCloseBalance = item.ForeignCloseBalance != null ? item.ForeignCloseBalance : 0,
                            ForeignCredit = item.ForeignCredit != null ? item.ForeignCredit : 0,
                            ForeignDebit = item.ForeignDebit != null ? item.ForeignDebit : 0,
                            ForeignOpenBalance = item.ForeignOpenBalance != null ? item.ForeignOpenBalance : 0,
                            ChartOfAccountTypeOrder = chartOfAccountTypes.FirstOrDefault(x => x.Code == item.ChartOfAcountType)?.Order,
                            Type = "ChartOfAccount"
                        };

                        //if (record.ParentId == "6")
                        //{
                        //    record.ParentId = "7";

                        //}

                        ResultList parent = totalData.ResultList.Where(d => d.Id == record.ParentId).FirstOrDefault();
                        if (parent == null)
                        {
                            ChartOfAccountsTypePM chartOfAccountType = chartOfAccountTypes.FirstOrDefault(x => x.Code == item.ChartOfAcountType);

                            if (chartOfAccountType != null)
                            {


                                ResultList parentrecord = new ResultList()
                                {
                                    Id = record.ParentId,
                                    Name = chartOfAccountType.LocalName,
                                    Number = chartOfAccountType.Code,
                                    ParentId = null,
                                    LocalCloseBalance = item.LocalCloseBalance != null ? item.LocalCloseBalance : 0,
                                    LocalCredit = item.LocalCredit != null ? item.LocalCredit : 0,
                                    LocalDebit = item.LocalDebit != null ? item.LocalDebit : 0,
                                    LocalOpenBalance = item.LocalOpenBalance != null ? item.LocalOpenBalance : 0,


                                    ForeignCloseBalance = item.ForeignCloseBalance != null ? item.ForeignCloseBalance : 0,
                                    ForeignCredit = item.ForeignCredit != null ? item.ForeignCredit : 0,
                                    ForeignDebit = item.ForeignDebit != null ? item.ForeignDebit : 0,
                                    ForeignOpenBalance = item.ForeignOpenBalance != null ? item.ForeignOpenBalance : 0,
                                    ChartOfAccountTypeOrder = chartOfAccountType.Order,
                                    Error = true,


                                };
                                totalData.ResultList.Add(parentrecord);
                            }
                        }
                        var duplicated = totalData.ResultList.Where(d => d.Id == record.Id).FirstOrDefault();

                        if (duplicated == null)
                        {
                            if (!string.IsNullOrEmpty(record.Id))
                            {
                                totalData.ResultList.Add(record);
                            }
                        }


                    }
                }





            }






            if (level == "GLAccount")
            {

                List<string> chartOfAccountTypesCodes = GetChartOfAccountsTypesFilterValue(queryOperations);
                List<string> chartOfAccountIds = GetChartoOfAccountsFilterValue(queryOperations);
                trailReportParam.ChartOfAccountsTypeCodeList = chartOfAccountTypesCodes;
                trailReportParam.ChartOfAccountsIdList = chartOfAccountIds;

                trailReportParam.DetailedControlVendors = vendor;
                trailReportParam.DetailedControlClients = customer;
                trailReportParam.DetailedControlFile = files;
                trailReportParam.DetailedControlJob = jobs;
                trailReportParam.CurrenciesDetailed = currency;
                trailReportParam.Category1 = category1;
                trailReportParam.Category2 = category2;
                trailReportParam.Category3 = category3;
                trailReportParam.Category4 = category4;
                trailReportParam.Category5 = category5;
                trailReportParam.MyTrailReportLevel = ReportLevel.GLAccount;
                trailReportParam.Suppress_DoNotShowCardWithoutActivity = false;
                trailReportParam.DoNotShowCardWithLocalCloseBalanceEqualZero = dontShowCardsWith0Balance;

                var servce = TrailReportFactory.CreateNew(trailReportParam);

                List<TrailReportM> list;
                try
                {
                    list = servce.Execute();

                }
                finally
                {
                    // servce.Dispose();

                }



                List<CurrencyPM> currencies = GetCurrenciesByTenant(tenant);


                #region Fill Report Data
                totalData.ForDate = toDate;
                var watch = System.Diagnostics.Stopwatch.StartNew();

                FilterChartOfAccountsAndTypes(totalData, list);
                watch.Start();
                Parallel.ForEach(list, (item) => {


                    if (item != null)
                    {

                        ResultList record = new ResultList()
                        {
                            Id = item.GLAccountId,
                            Name = item.GLAccountName,
                            Number = item.GLAccountNumber,

                            ParentId = item.ChartOfAccountId,
                            LocalCloseBalance = item.LocalCloseBalance != null ? item.LocalCloseBalance : 0,
                            LocalCredit = item.LocalCredit != null ? item.LocalCredit : 0,
                            LocalDebit = item.LocalDebit != null ? item.LocalDebit : 0,
                            LocalOpenBalance = item.LocalOpenBalance != null ? item.LocalOpenBalance : 0,


                            ForeignCloseBalance = item.ForeignCloseBalance != null ? item.ForeignCloseBalance : 0,
                            ForeignCredit = item.ForeignCredit != null ? item.ForeignCredit : 0,
                            ForeignDebit = item.ForeignDebit != null ? item.ForeignDebit : 0,
                            ForeignOpenBalance = item.ForeignOpenBalance != null ? item.ForeignOpenBalance : 0,
                            ChartofAccountCode = item.ChartOfAcountCode1,
                            ChartofAccountLocalName = item.ChartOfAcountName1,
                            ChartofAccountTypeCode = item.ChartOfAcountType,
                            ChartOfAccountTypeOrder = chartOfAccountTypes.FirstOrDefault(x => x.Code == item.ChartOfAcountType)?.Order,
                            ChartofAccountTypeLocalName = item.ChartOfAcountType != null ? chartOfAccountTypes.Where(d => d.Code == item.ChartOfAcountType).FirstOrDefault().LocalName : null,
                            CurrencyCode = item.CurrencyId != null ? currencies.Where(d => d.Id == item.CurrencyId).FirstOrDefault().Code : "Multi",



                        };

                        GLAccountParents.Add(record.ParentId);
                        if (!string.IsNullOrEmpty(record.Id))
                        {
                            lock (_locker)
                            {
                                totalData.ResultList.Add(record);
                                ResultList parent = totalData.ResultList.Where(d => d.Id == record.ParentId).FirstOrDefault();
                                if (parent == null)
                                {
                                    var chartOfAccount = ChartOfAccountsList.FirstOrDefault(x => x.Id == item.ChartOfAccountId);
                                    ResultList parentrecord = CreateNotRetreivedParent(item, tenant, chartOfAccount);
                                    totalData.ResultList.Add(parentrecord);
                                }
                                else
                                {

                                    //RecalculateParentTotals(record, totalData);
                                    //ResultList resultList = totalData.ResultList.Where(d => d.Id == record.ParentId).FirstOrDefault();
                                }
                            }
                        }
                    }
                });

                watch.Stop();
                Logger.Debug("RecalculateParentTotals ForEach:" + watch.ElapsedMilliseconds.ToString());
                watch.Restart();
                watch.Start();
                RecalculateParentTotals(totalData);
                watch.Stop();
                Logger.Debug("RecalculateParentTotals ForEach:" + watch.ElapsedMilliseconds.ToString());


            }



            totalData.TotalLocalCloseBalance = totalData.ResultList.Where(d => d.ParentId == null).Sum(d => d.LocalCloseBalance);
            totalData.TotalLocalOpenBalance = totalData.ResultList.Where(d => d.ParentId == null).Sum(d => d.LocalOpenBalance);
            totalData.TotalLocalCredit = totalData.ResultList.Where(d => d.ParentId == null).Sum(d => d.LocalCredit);
            totalData.TotalLocalDebit = totalData.ResultList.Where(d => d.ParentId == null).Sum(d => d.LocalDebit);

            totalData.TotalForeignCloseBalance = totalData.ResultList.Where(d => d.ParentId == null).Sum(d => d.ForeignCloseBalance);
            totalData.TotalForeignCredit = totalData.ResultList.Where(d => d.ParentId == null).Sum(d => d.ForeignCredit);
            totalData.TotalForeignDebit = totalData.ResultList.Where(d => d.ParentId == null).Sum(d => d.ForeignDebit);
            totalData.TotalForeignOpenBalance = totalData.ResultList.Where(d => d.ParentId == null).Sum(d => d.ForeignOpenBalance);

            //  totalData.ResultList.OrderBy(d => d.Name);
            //var revenues = result.Where(d => d.ChartOfAcountType == "1").FirstOrDefault().LocalCloseBalance;
            //var expenses = result.Where(d => d.ChartOfAcountType == "2").FirstOrDefault().LocalCloseBalance;
            //totalData.TotalRevenueExpense = (revenues == null ? 0 : revenues) - (expenses == null ? 0 : expenses);
            // service.Dispose();

            #endregion







            return totalData;
        }

        private void FilterChartOfAccountsAndTypes(RevenueExpenseDataProvider totalData, List<TrailReportM> trailReportMs)
        {
            // filter chart of account types
            //foreach (var item in totalData.ResultList.Where(x => x.Type == null).ToList())
            //{
            //    if (!trailReportMs.Any(x => x != null && x.ChartOfAcountType == item.Id))
            //    {
            //        totalData.ResultList.Remove(item);
            //    }
            //}
            // filter chart of accounts
            foreach (var item in totalData.ResultList.Where(x => x.Type == "ChartOfAccount").ToList())
            {
                if (!trailReportMs.Any(x => x != null && x.ChartOfAccountId == item.Id) && !item.Error)
                {
                    totalData.ResultList.Remove(item);
                }
            }
            // Update chart of account types total amounts
            foreach (var item in totalData.ResultList.Where(x => x.Type == null).ToList())
            {
                if (totalData.ResultList.Where(x => x.Type == "ChartOfAccount" && x.ParentId == item.Id).Count() > 0)
                {
                    item.LocalCloseBalance = totalData.ResultList.Where(x => x.Type == "ChartOfAccount" && x.ParentId == item.Id).Sum(d => d.LocalCloseBalance);
                    item.LocalOpenBalance = totalData.ResultList.Where(x => x.Type == "ChartOfAccount" && x.ParentId == item.Id).Sum(d => d.LocalOpenBalance);
                    item.LocalCredit = totalData.ResultList.Where(x => x.Type == "ChartOfAccount" && x.ParentId == item.Id).Sum(d => d.LocalCredit);
                    item.LocalDebit = totalData.ResultList.Where(x => x.Type == "ChartOfAccount" && x.ParentId == item.Id).Sum(d => d.LocalDebit);
                    item.ForeignCloseBalance = totalData.ResultList.Where(x => x.Type == "ChartOfAccount" && x.ParentId == item.Id).Sum(d => d.ForeignCloseBalance);
                    item.ForeignCredit = totalData.ResultList.Where(x => x.Type == "ChartOfAccount" && x.ParentId == item.Id).Sum(d => d.ForeignCredit);
                    item.ForeignDebit = totalData.ResultList.Where(x => x.Type == "ChartOfAccount" && x.ParentId == item.Id).Sum(d => d.ForeignDebit);
                    item.ForeignOpenBalance = totalData.ResultList.Where(x => x.Type == "ChartOfAccount" && x.ParentId == item.Id).Sum(d => d.ForeignOpenBalance);
                }
            }
        }

        private static List<string> GetChartoOfAccountsFilterValue(QueryOperations queryOperations)
        {
            List<string> chartOfAccountIds = new List<string>();
            QueryFilterItem filterItem_ChartOfAccountsIdList = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ChartOfAccountsIdList").FirstOrDefault();
            if (filterItem_ChartOfAccountsIdList?.FieldValue != null && filterItem_ChartOfAccountsIdList?.FieldValue != "")
            {
                var chartOfAccountsIdList = (string)filterItem_ChartOfAccountsIdList.FieldValue;
                chartOfAccountIds = chartOfAccountsIdList.Split(',').ToList();
            }

            return chartOfAccountIds;
        }

        private static List<string> GetChartOfAccountsTypesFilterValue(QueryOperations queryOperations)
        {
            List<string> chartOfAccountTypesCodes = new List<string>(); ;
            QueryFilterItem filterItem_ChartOfAccountsTypeCodeList = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ChartOfAccountsTypeCodeList").FirstOrDefault();
            if (filterItem_ChartOfAccountsTypeCodeList?.FieldValue != null && filterItem_ChartOfAccountsTypeCodeList?.FieldValue != "")
            {
                var chartOfAccountsTypeCodeList = (string)filterItem_ChartOfAccountsTypeCodeList.FieldValue;
                chartOfAccountTypesCodes = chartOfAccountsTypeCodeList.Split(',').ToList();
            }

            return chartOfAccountTypesCodes;
        }

        private List<CurrencyPM> GetCurrenciesByTenant(int tenant)
        {
            CurrencyQuery currencyQuery = new CurrencyQuery(tenant);
            List<CurrencyPM> currencies = currencyQuery.GetCurrencyPMsByTenant(tenant).ToList();
            return currencies;
        }
        private List<ChartOfAccountsTypePM> GetChartOfAccountTypes(int tenant)
        {
            ChartOfAccountsTypeQueryService chartOfAccountsTypeQueryService = new ChartOfAccountsTypeQueryService(tenant);
            return chartOfAccountsTypeQueryService.GetAllChartOfAccounts();

        }
        private List<ChartOfAccount> GetChartOfAccounts(int tenant)
        {
            var repo = new ChartOfAccountRepository(tenant);
            return repo.GetAllByTenant(tenant);
        }
        private void RecalculateParentTotals(ResultList record, RevenueExpenseDataProvider totalData)
        {
            List<ResultList> relatedRecords = totalData.ResultList.Where(c => c.ParentId == record.ParentId).ToList();
            Logger.Debug("RecalculateParentTotals" + relatedRecords.Count());

            var parentRecords = totalData.ResultList.Where(d => d.Id == record.ParentId).ToList();
            Logger.Debug("RecalculateParentTotals" + relatedRecords.Count());

            foreach (var parentRecord in parentRecords)
            {
                executionCount++;
                parentRecord.LocalCloseBalance = relatedRecords.Sum(c => c.LocalCloseBalance);
                parentRecord.LocalCredit = relatedRecords.Sum(c => c.LocalCredit);
                parentRecord.LocalDebit = relatedRecords.Sum(c => c.LocalDebit);
                parentRecord.LocalOpenBalance = relatedRecords.Sum(c => c.LocalOpenBalance);

                parentRecord.ForeignCloseBalance = relatedRecords.Sum(c => c.ForeignCloseBalance);
                parentRecord.ForeignCredit = relatedRecords.Sum(c => c.ForeignCredit);
                parentRecord.ForeignDebit = relatedRecords.Sum(c => c.ForeignDebit);
                parentRecord.ForeignOpenBalance = relatedRecords.Sum(c => c.ForeignOpenBalance);
                Logger.Debug($"Code block executed {executionCount} times.");

            }
        }

        private void RecalculateParentTotals(RevenueExpenseDataProvider totalData)
        {
            try
            {
                var groupedRecords = totalData.ResultList.GroupBy(c => c.ParentId);

                foreach (var group in groupedRecords)
                {
                    var parentRecord = totalData.ResultList.FirstOrDefault(d => d.Id == group.Key);

                    if (parentRecord != null)
                    {
                        executionCount++;
                        parentRecord.LocalCloseBalance = group.Sum(c => c.LocalCloseBalance);
                        parentRecord.LocalCredit = group.Sum(c => c.LocalCredit);
                        parentRecord.LocalDebit = group.Sum(c => c.LocalDebit);
                        parentRecord.LocalOpenBalance = group.Sum(c => c.LocalOpenBalance);

                        parentRecord.ForeignCloseBalance = group.Sum(c => c.ForeignCloseBalance ?? 0);
                        parentRecord.ForeignCredit = group.Sum(c => c.ForeignCredit);
                        parentRecord.ForeignDebit = group.Sum(c => c.ForeignDebit);
                        parentRecord.ForeignOpenBalance = group.Sum(c => c.ForeignOpenBalance);

                        Logger.Debug($"Code block executed {executionCount} times."); 
                    }
                }
            }
            catch (OverflowException ex)
            {
                var error = ex.Message;
                // Handle or log the exception
            }
        }
            private ResultList CreateNotRetreivedParent(TrailReportM item, int tenant, ChartOfAccount chartOfAccount)
        {
            if (chartOfAccount != null)
            {
                ResultList parentrecord = new ResultList()
                {
                    Id = item.ChartOfAccountId,
                    Name = chartOfAccount.LocalName,
                    Number = chartOfAccount.Code,
                    ParentId = item.ChartOfAcountType,
                    LocalCloseBalance = item.LocalCloseBalance != null ? item.LocalCloseBalance : 0,
                    LocalCredit = item.LocalCredit != null ? item.LocalCredit : 0,
                    LocalDebit = item.LocalDebit != null ? item.LocalDebit : 0,
                    LocalOpenBalance = item.LocalOpenBalance != null ? item.LocalOpenBalance : 0,


                    ForeignCloseBalance = item.ForeignCloseBalance != null ? item.ForeignCloseBalance : 0,
                    ForeignCredit = item.ForeignCredit != null ? item.ForeignCredit : 0,
                    ForeignDebit = item.ForeignDebit != null ? item.ForeignDebit : 0,
                    ForeignOpenBalance = item.ForeignOpenBalance != null ? item.ForeignOpenBalance : 0,

                    Error = true,


                };
                return parentrecord;
            }
            else return null;

        }
        private string SetDetailedCustomersAccounts(bool customer)
        {
            if (showLocals)
            {
                return customer ? "הצג פירוט" : "ללא פירוט";
            }
            else
            {
                return customer ? "Show" : "Dont show";
            }

        }
        private string SetDetailedVendorsAccounts(bool vendor)
        {
            if (showLocals)
            {
                return vendor ? "הצג פירוט" : "ללא פירוט";
            }
            else
            {
                return vendor ? "Show" : "Dont show";
            }
        }
        private string GetCategory1Name(string category1, int tenant)
        {
            Category1QueryService category1QueryService = new Category1QueryService(tenant);
            Category1PM category = category1QueryService.GetSinglePM(category1, tenant);
            ContactPM contact = GetLoggedContact(tenant);
            bool showLocals = !contact.DontShowLocal;
            if (category != null)
                return showLocals ? category.LocalName : category.EnglishName;
            else return null;

        }
        private string GetCategory5Name(string category5, int tenant)
        {
            Category5QueryService category5QueryService = new Category5QueryService(tenant);
            Category5PM category = category5QueryService.GetSinglePM(category5, tenant);
            ContactPM contact = GetLoggedContact(tenant);
            bool showLocals = !contact.DontShowLocal;
            if (category != null)
                return showLocals ? category.LocalName : category.EnglishName;
            else return null;

        }
        #endregion

        #region Load Shipments Stocks
        public byte[] LoadShipmentsStocksData(byte[] xmlFilters, int tenant)
        {
            ShipmentsStocksDataProviderHelper shipmentsStocksDataProviderHelper = new ShipmentsStocksDataProviderHelper();
            ShipmentsStocksDataProvider dataprovider = shipmentsStocksDataProviderHelper.LoadShipmentsStocksDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(ShipmentsStocksDataProvider), tenant);
        }
        #endregion

        #region Load Users By Tenant 
        public byte[] LoadUsersByTenantData(byte[] xmlFilters, int tenant)
        {
            UsersByTenantDataProviderHelper usersByTenantDataProviderHelper = new UsersByTenantDataProviderHelper();
            UsersByTenantDataProvider dataprovider = usersByTenantDataProviderHelper.LoadUsersByTenantDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(UsersByTenantDataProvider), tenant);
        }
        #endregion

        #region Shipment Details
        public byte[] LoadShipmentDetailsDataProvider(byte[] xmlFilters, ReportFliter reportFliter, int tenant)
        {
            ShipmentDetailsDataProvider dataprovider = GetShipmentDetailsDataProvider(xmlFilters, tenant);
            byte[] bytearray = new ReportMemoryStreamService().Convert(dataprovider, typeof(ShipmentDetailsDataProvider), tenant);
            ReportManipulationDataService reportManipulationDataService = new ReportManipulationDataService(dataprovider, reportFliter);
            bytearray = reportManipulationDataService.IsDataProviderHaveListWithValues() ? bytearray : null;
            return bytearray;
        }

        private ShipmentDetailsDataProvider GetShipmentDetailsDataProvider(byte[] xmlFilters, int tenant)
        {
            WebServiceHelper servicHelper = new WebServiceHelper(tenant);
            ShipmentDetailsDataProvider totalData = new DataProviders.ShipmentDetailsDataProvider();
            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            AddressRepository addressRepository = new AddressRepository(commonContext);
            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant);

            #region Report Filters

            QueryOperations queryOperations = GetQueryOperationsFromXmlFilters(xmlFilters);
            QueryFilterItem filterItem_tODate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();

            //ToDate
            DateTime? ToDate = null;
            if (filterItem_tODate != null)
            {
                if (filterItem_tODate.FieldValue != null)
                {
                    ToDate = (DateTime)filterItem_tODate.FieldValue;
                }
            }

            //FromDate
            DateTime? FromDate = null;
            if (filterItem_FromDate != null)
            {
                if (filterItem_FromDate.FieldValue != null)
                {
                    FromDate = (DateTime)filterItem_FromDate.FieldValue;
                }
            }

            #endregion

            #region Base Data Foltered
            shipments = shipments.Where(d => (d.ShipmentLevelCode == "D" || d.ShipmentLevelCode == "H") && !d.IsCancelled);

            if (FromDate != null)
            {
                shipments = shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= System.Data.Entity.DbFunctions.TruncateTime(FromDate));
            }

            if (ToDate != null)
            {
                shipments = shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= System.Data.Entity.DbFunctions.TruncateTime(ToDate));
            }
            #endregion

            #region Fill Report Data
            List<ShipmentDataView> Shipments = shipments.ToList();
            if (Shipments.Count > 0)
            {
                List<string> valueOfGoodsCurrencyIds = Shipments.GroupBy(d => d.ValueOfGoodsCurrencyId).Select(d => d.FirstOrDefault().ValueOfGoodsCurrencyId).ToList();
                List<string> departmentIdIds = Shipments.GroupBy(d => d.DepartmentId).Select(d => d.FirstOrDefault().DepartmentId).ToList();
                List<string> shipmentdelevriesIds = Shipments.Select(d => d.Id).ToList();
                List<string> FromPartnerCardIds = (from d in shipmentsContext.ShipmentPickUpDeliveries where d.PickUpDeliveryTypeCode == "PICK" && shipmentdelevriesIds.Contains(d.ShipmentId) select d.FromPartnerCardId).ToList();
                List<string> FromAddressCountryIds = (from d in shipmentsContext.ShipmentPickUpDeliveries where d.PickUpDeliveryTypeCode == "PICK" && shipmentdelevriesIds.Contains(d.ShipmentId) select d.FromAddressCountryId).ToList();
                List<string> ToPartnerCardIds = (from d in shipmentsContext.ShipmentPickUpDeliveries where d.PickUpDeliveryTypeCode == "DELV" && shipmentdelevriesIds.Contains(d.ShipmentId) select d.ToPartnerCardId).ToList();

                List<Currency> currencyLists = (from d in commonContext.Currencies where valueOfGoodsCurrencyIds.Contains(d.Id) select d).ToList();
                List<Department> departmentLists = (from d in commonContext.Departments where departmentIdIds.Contains(d.Id) select d).ToList();
                List<ShipmentPickUpDelivery> shipmentPickUpDeliveriesLists = (from d in shipmentsContext.ShipmentPickUpDeliveries where shipmentdelevriesIds.Contains(d.ShipmentId) select d).Include("ToAddressCountry").ToList();
                List<Address> FromPartnerAddressLists = (from a in commonContext.Addresses where a.Tenant == tenant && FromPartnerCardIds.Contains(a.CardId) && a.AddressTypeId.ToUpper() == "M" select a).ToList();
                List<Country> FromAddressCountryLists = (from record in commonContext.Countries where FromAddressCountryIds.Contains(record.Id) && record.Tenant == tenant select record).ToList();
                List<Address> ToPartnerAddressLists = (from a in commonContext.Addresses where a.Tenant == tenant && ToPartnerCardIds.Contains(a.CardId) && a.AddressTypeId.ToUpper() == "M" select a).ToList();
                List<ShipmentPackage> ShipmentPackages = (from d in shipmentsContext.ShipmentPackages where shipmentdelevriesIds.Contains(d.ShipmentId) && d.Reference1 != null && d.Reference2 != null && d.Reference3 != null && d.Reference4 != null select d).ToList();
                List<Card> CardList = commonContext.Cards.Where(d => d.Tenant == tenant).ToList();

                totalData.Shipments = new List<ShipmentDetals>();
                foreach (ShipmentDataView Item in Shipments)
                {
                    bool isInlandDomesticShipment = (Item.DirectionId == "D" && Item.TransportModeId == "I");

                    Currency ValueOfgoodsCurrency = currencyLists.Where(d => d.Id == Item.ValueOfGoodsCurrencyId).FirstOrDefault();
                    Department ShipmentDepartment = departmentLists.Where(d => d.Id == Item.DepartmentId).FirstOrDefault();

                    ShipmentPickUpDelivery myLastPickup = shipmentPickUpDeliveriesLists.Where(d => d.ShipmentId == Item.Id && d.PickUpDeliveryTypeCode == "PICK").OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();
                    ShipmentPickUpDelivery myLastDelivery = shipmentPickUpDeliveriesLists.Where(d => d.ShipmentId == Item.Id && d.PickUpDeliveryTypeCode == "DELV").OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();
                    ShipmentPickUpDelivery myFirstPickup = shipmentPickUpDeliveriesLists.Where(d => d.ShipmentId == Item.Id && d.PickUpDeliveryTypeCode == "PICK").OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();
                    ShipmentPackage firstShipmentPackage = ShipmentPackages.Where(d => d.ShipmentId == Item.Id).FirstOrDefault();

                    ShipmentDetals shipment = new ShipmentDetals();
                    shipment.Openedby = Item.CreatedByUserName;
                    shipment.Direction = Item.DirectionName;
                    shipment.ShipmentId = Item.ShipmentNumber;
                    shipment.ShipmentLevel = Item.ShipmentLevelName;

                    shipment.Shipper = Item.Shipper;
                    shipment.BUShipper = Item.ShipperNotExporterName;
                    shipment.ShipperRef1 = Item.ShipperReference1;
                    shipment.ShipperRef2 = Item.ShipperReference2;
                    shipment.CountOfInvoices = Item.MainHarmonize;
                    shipment.BUImporte = Item.ConsigneeNotImporterName;
                    shipment.Consignee = Item.ConsigneeName;
                    shipment.ConsigneeRef1 = Item.ConsigneeReference1;
                    shipment.ConsigneeRef2 = Item.ConsigneeReference2;
                    shipment.Agent = Item.AgentName;
                    shipment.AgentRef1 = Item.AgentReference1;
                    shipment.AgentRef2 = Item.AgentReference2;
                    shipment.Incoterm = Item.IncotermCode;
                    shipment.FreightPC = Item.FreightPrepaidCollectId;
                    shipment.OtherPC = Item.OtherPrepaidCollectId;
                    shipment.ModeofTransport = Item.TransportModeName;
                    shipment.Type = Item.ShipmentTypeName;
                    shipment.FreightForwarder = Item.FreightForwarderName;
                    shipment.ClearingAgentimport = Item.CustomAgentImportName;
                    shipment.MainCarriageCarrier = Item.MainCarriageCarrierName;
                    shipment.Vessel = Item.MainCarriageVesselName;
                    shipment.CarrierNumber = Item.CarrierNumber;
                    shipment.BookingConf = Item.BookingConfirmationNumber;
                    shipment.ConfirmedBy = Item.BookingConfirmedBy;
                    shipment.Cutoffdate = Item.CutoffDate;
                    shipment.CutoffTime = String.Format("{0:t}", Item.CutoffDate);
                    shipment.ConfirmationNotes = Item.BookingConfirmationNotes;
                    shipment.MAWBMBL = Item.Master;
                    shipment.HAWBHBL = Item.House;
                    shipment.HAWBDate = Item.HAWBDate;
                    shipment.NumberofPackages = Item.NumberOfInsidePackages == 0 ? Item.NumberOfPackages : Item.NumberOfInsidePackages;
                    shipment.GrossWeightKgs = Item.GrossWeightInKG;
                    shipment.Volumem3 = Item.VolumeInCBM;
                    shipment.ChargeableWeightKgs = Item.ChargeableWeightInKG;
                    shipment.TotalPackagesReceived = Item.NumberOfPackages;
                    shipment.CountofTEU = Item.TEU;
                    shipment.DGR = Item.IsDangerous;
                    shipment.DescriptionofGoods = Item.DescriptionOfGoods;
                    shipment.Routing = Item.Routing;
                    shipment.PortofDeparture = Item.MainCarriageFromPortCode;
                    shipment.CountryofDeparture = Item.MainCarriageFromPortCountryName;
                    shipment.ViaCity = Item.Transshipment1FromPortCode;
                    shipment.CountryofDestination = Item.MainCarriageToPortCountryName;
                    shipment.PortofDestination = Item.MainCarriageToPortCode;
                    shipment.CreateDate = Item.CreateDateTime;
                    shipment.PickupFromDate = myLastPickup != null ? myLastPickup.ATD : null;
                    shipment.GroupageDate = Item.CutoffDate;
                    shipment.DateonboardOrigin = Item.MainCarriageATD != null ? Item.MainCarriageATD : Item.MainCarriageETD;
                    shipment.Dateofarrivaltoport = Item.MainCarriageATA != null ? Item.MainCarriageATA : Item.MainCarriageETA;
                    shipment.ImportDeclarationDate = Item.DeclarationDate;
                    shipment.CustomsClearanceDate = Item.CustomsClearanceDate;
                    shipment.DeliveryDate = myLastDelivery != null ? (myLastDelivery.ATA != null ? myLastDelivery.ATA : myLastDelivery.ETA) : null;
                    shipment.ClosedDate = Item.OperationalCloseDate;
                    shipment.IncludeCustoms = Item.IncludesCustoms;
                    shipment.ImportDeclarationNumber = Item.DeclarationNumber;
                    shipment.LocalInspection = Item.SalesmanUserName;
                    shipment.CountofLegalisedDocuments = Item.AMSBL;
                    shipment.ShipperInvoiceValue = Item.ValueOfGoods;
                    shipment.CurrencyofShipperInvoice = ValueOfgoodsCurrency != null ? ValueOfgoodsCurrency.Code : null;

                    shipment.CustomerName = Item.CustomerName;
                    shipment.Notify1Name = Item.Notify1Name;
                    shipment.Notify2Name = Item.Notify2Name;
                    shipment.ConsigneeNotImporterName = Item.ConsigneeNotImporterName;
                    shipment.ShipperNotExporterName = Item.ShipperNotExporterName;

                    if (!string.IsNullOrEmpty(Item.Notify1Id))
                    {
                        Card myCard = CardList.Where(d => d.Id == Item.Notify1Id).FirstOrDefault();
                        if (myCard != null)
                        {
                            shipment.Notify1ReceivablesAccountingCard = myCard.ReceivablesAccountingCard;
                        }
                    }

                    if (!string.IsNullOrEmpty(Item.Notify2Id))
                    {
                        Card myCard = CardList.Where(d => d.Id == Item.Notify2Id).FirstOrDefault();
                        if (myCard != null)
                        {
                            shipment.Notify2ReceivablesAccountingCard = myCard.ReceivablesAccountingCard;
                        }
                    }

                    if (!string.IsNullOrEmpty(Item.ConsigneeId))
                    {
                        Card myCard = CardList.Where(d => d.Id == Item.ConsigneeId).FirstOrDefault();
                        if (myCard != null)
                        {
                            shipment.ConsigneeReceivablesAccountingCard = myCard.ReceivablesAccountingCard;
                        }
                    }

                    if (!string.IsNullOrEmpty(Item.ShipperId))
                    {
                        Card myCard = CardList.Where(d => d.Id == Item.ShipperId).FirstOrDefault();
                        if (myCard != null)
                        {
                            shipment.ShipperReceivablesAccountingCard = myCard.ReceivablesAccountingCard;
                        }
                    }

                    if (Item.ShipmentNumber == "E9069")
                    {
                        var test = "z";
                    }

                    if (Item.DirectionId == "D" && Item.TransportModeId == "I")
                    {
                        InlandDomesticArgs args = new InlandDomesticArgs()
                        {
                            InlandDomesticFromTypeCode = Item.InlandDomesticFromTypeCode,
                            MainCarriageFromAddressId = Item.MainCarriageFromAddressId,
                            MainCarriageFromPortId = Item.MainCarriageFromPortId,
                            InlandDomesticFromCity = Item.InlandDomesticFromCity,
                            InlandDomesticFromCountryId = Item.InlandDomesticFromCountryId,
                            InlandDomesticToTypeCode = Item.InlandDomesticToTypeCode,
                            MainCarriageToAddressId = Item.MainCarriageToAddressId,
                            InlandDomesticToCity = Item.InlandDomesticToCity,
                            InlandDomesticToCountryId = Item.InlandDomesticToCountryId,
                            MainCarriageToPortId = Item.MainCarriageToPortId,
                            MainCarriageFromPortCountryCode = Item.MainCarriageFromPortCountryCode,
                            MainCarriageToPortCountryCode = Item.MainCarriageToPortCountryCode
                        };
                        shipment.FinalCountryofDestination = servicHelper.GetInlandDomesticToCountryName(args);
                    }
                    else
                    {
                        if (myLastDelivery != null)
                        {
                            switch (myLastDelivery.PickUpDeliveryToTypeCode)
                            {
                                case "PART":
                                    {
                                        if (!string.IsNullOrEmpty(myLastDelivery.ToAddressId))
                                        {
                                            Address myPartnerAddress = addressRepository.GetSingleAddress(myLastDelivery.ToAddressId, tenant);
                                            if (myPartnerAddress != null)
                                            {
                                                shipment.FinalCountryofDestination = myPartnerAddress.Country != null ? myPartnerAddress.Country.EnglishName : null;
                                            }
                                        }

                                        break;
                                    }

                                case "PORT":
                                    {
                                        if (!string.IsNullOrEmpty(myLastDelivery.ToPortId))
                                        {
                                            PortPM myPort = PortQuery.GetSinglePort(tenant, myLastDelivery.ToPortId, true);
                                            if (myPort != null)
                                            {
                                                shipment.FinalCountryofDestination = myPort.CountryName;
                                                shipment.FinalPortofDestination = myPort.Code;


                                            }
                                        }

                                        break;
                                    }

                                case "CASL":
                                    {
                                        string myCountry = myLastDelivery.ToAddressCountry != null ? myLastDelivery.ToAddressCountry.EnglishName : null;
                                        if (!string.IsNullOrEmpty(myCountry))
                                        {
                                            shipment.FinalCountryofDestination = myCountry;
                                            //shipment.FinalPortofDestination = myLastDelivery..Code;

                                        }

                                        break;
                                    }
                            }
                        }

                        else if (Item.DirectionId == "I" && !string.IsNullOrEmpty(Item.WarehouseLegWarehouseId))
                        {
                            Card warehouse = CardList.Where(p => p.Id == Item.WarehouseLegWarehouseId).FirstOrDefault();
                            if (warehouse != null)
                            {
                                shipment.FinalCountryofDestination = warehouse.CountryName;
                                //shipment.FinalPortofDestination = warehouse.Code;

                            }
                        }

                        else if (!string.IsNullOrEmpty(Item.OnForwardingToPortId))
                        {
                            PortPM myPort = PortQuery.GetSinglePort(tenant, Item.OnForwardingToPortId, true);
                            if (myPort != null)
                            {
                                shipment.FinalCountryofDestination = myPort.CountryName;
                                shipment.FinalPortofDestination = myPort.Code;
                            }
                        }

                        else if (!string.IsNullOrEmpty(Item.OnCarriageToPortId))
                        {
                            PortPM onCarriageToPort = PortQuery.GetSinglePort(tenant, Item.OnCarriageToPortId, true);
                            if (onCarriageToPort != null)
                            {
                                shipment.FinalCountryofDestination = onCarriageToPort.CountryName;
                                shipment.FinalPortofDestination = onCarriageToPort.Code;
                            }
                        }

                        else
                        {
                            if (!string.IsNullOrEmpty(Item.Transshipment3ToPortId))
                            {
                                PortPM transshipment3ToPort = PortQuery.GetSinglePort(tenant, Item.Transshipment3ToPortId, true);
                                if (transshipment3ToPort != null)
                                {
                                    shipment.FinalCountryofDestination = transshipment3ToPort.CountryName;
                                    shipment.FinalPortofDestination = transshipment3ToPort.Code;

                                }
                            }

                            else if (!string.IsNullOrEmpty(Item.Transshipment2ToPortId))
                            {
                                PortPM transshipment2ToPort = PortQuery.GetSinglePort(tenant, Item.Transshipment2ToPortId, true);
                                if (transshipment2ToPort != null)
                                {
                                    shipment.FinalCountryofDestination = transshipment2ToPort.CountryName;
                                    shipment.FinalPortofDestination = transshipment2ToPort.Code;

                                }
                            }

                            else if (!string.IsNullOrEmpty(Item.Transshipment1ToPortId))
                            {
                                PortPM transshipment1ToPort = PortQuery.GetSinglePort(tenant, Item.Transshipment1ToPortId, true);
                                if (transshipment1ToPort != null)
                                {
                                    shipment.FinalCountryofDestination = transshipment1ToPort.CountryName;
                                    shipment.FinalPortofDestination = transshipment1ToPort.Code;

                                }
                            }

                            else if (!string.IsNullOrEmpty(Item.MainCarriageToPortId))
                            {
                                PortPM mainCarriageToPort = PortQuery.GetSinglePort(tenant, Item.MainCarriageToPortId, true);
                                if (mainCarriageToPort != null)
                                {
                                    shipment.FinalCountryofDestination = mainCarriageToPort.CountryName;
                                    shipment.FinalPortofDestination = mainCarriageToPort.Code;
                                }
                            }
                            else if (!string.IsNullOrEmpty(Item.ToPortId))
                            {
                                PortPM mainCarriageToPort = PortQuery.GetSinglePort(tenant, Item.ToPortId, true);
                                if (mainCarriageToPort != null)
                                {
                                    shipment.FinalCountryofDestination = mainCarriageToPort.CountryName;
                                    shipment.FinalPortofDestination = mainCarriageToPort.Code;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(Item.OnCarriageTransportModeId))
                    {
                        shipment.OnCarriageTransportMode = Item.OnCarriageTransportModeId == "I" ? "Inland" : Item.OnCarriageTransportModeId == "A" ? "Air" : "Ocean";
                    }

                    if (!string.IsNullOrEmpty(Item.OnForwardingTransportModeId))
                    {
                        shipment.OnForwardingTransportMode = Item.OnForwardingTransportModeId == "I" ? "Inland" : Item.OnForwardingTransportModeId == "A" ? "Air" : "Ocean";
                    }

                    if (!string.IsNullOrEmpty(Item.MasterShipmentDataId))
                    {
                        if (!string.IsNullOrEmpty(Item.ShipmentMasterDataStatusId))
                        {
                            string statusName = null;
                            string statusCode = null;

                            EntityStatusHelper.GetHighestStatusId(Item.ShipmentStatusId, Item.ShipmentMasterDataStatusId, Item.Tenant, ref statusName, ref statusCode);
                            shipment.Status = statusName;
                        }
                        else
                        {
                            shipment.Status = Item.ShipmentStatusName;
                        }
                    }
                    else
                    {
                        shipment.Status = Item.ShipmentStatusName;

                    }

                    shipment.Dept = ShipmentDepartment != null ? ShipmentDepartment.EnglishName : null;
                    shipment.Branch = Item.BranchName;
                    shipment.ChargeableWeight = Item.ChargeableWeight;
                    shipment.ChargeableWeightUnitCode = Item.ChargeableWeightUnitCode;
                    shipment.NumberofDeliveries = shipmentPickUpDeliveriesLists.Where(d => d.ShipmentId == Item.Id && d.PickUpDeliveryTypeCode == "DELV").ToList().Count;

                    customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, Item, shipment);

                    if (Item.DirectionId == "I")
                    {
                        if (!string.IsNullOrEmpty(Item.ConsigneeId))
                        {
                            Card consignee = CardList.Where(d => d.Id == Item.ConsigneeId).FirstOrDefault();
                            if (consignee != null)
                            {
                                shipment.ShipperConsigneeExternalID = consignee.ReceivablesAccountingCard;
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Item.ShipperId))
                        {
                            Card shipper = CardList.Where(d => d.Id == Item.ShipperId).FirstOrDefault();
                            if (shipper != null)
                            {
                                shipment.ShipperConsigneeExternalID = shipper.ReceivablesAccountingCard;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(Item.OperationalClosedByUserId))
                    {
                        Contact contact = ContactRepository.GetSingleContact(Item.OperationalClosedByUserId, tenant, true);
                        if (contact != null)
                        {
                            shipment.OperationalClosedby = contact.EnglishName;
                        }
                    }

                    if (firstShipmentPackage != null)
                    {
                        shipment.Reference1 = firstShipmentPackage.Reference1;
                        shipment.Reference2 = firstShipmentPackage.Reference2;
                        shipment.Reference3 = firstShipmentPackage.Reference3;
                        shipment.Reference4 = firstShipmentPackage.Reference4;
                    }

                    if (myLastPickup != null)
                    {
                        shipment.LastPickupArrivalDate = myLastPickup.ATA;

                        switch (myLastPickup.PickUpDeliveryFromTypeCode)
                        {
                            case "PART":
                                {
                                    if (!string.IsNullOrEmpty(myLastPickup.FromPartnerCardId))
                                    {
                                        Address myPartnerAddress = FromPartnerAddressLists.Where(d => d.Id == myLastPickup.FromPartnerCardId).FirstOrDefault();// addressRepository.GetMainAddressByCardId(myLastPickup.FromPartnerCardId, tenant);
                                        if (myPartnerAddress != null)
                                        {
                                            shipment.PickupCity = myPartnerAddress.City;
                                            shipment.PickupCountry = myPartnerAddress.Country == null ? "" : myPartnerAddress.Country.EnglishName;
                                        }
                                    }
                                    break;
                                }

                            case "PORT":
                                {
                                    if (!string.IsNullOrEmpty(myLastPickup.FromPortId))
                                    {
                                        PortPM myPort = PortQuery.GetSinglePort(tenant, myLastPickup.FromPortId, true);
                                        if (myPort != null)
                                        {
                                            shipment.PickupCity = myPort.StateName;
                                            shipment.PickupCountry = myPort.CountryName;
                                        }
                                    }
                                    break;
                                }

                            case "CASL":
                                {
                                    shipment.PickupCity = myLastPickup.FromAddressCity;
                                    Country country = FromAddressCountryLists.Where(d => d.Id == myLastPickup.FromAddressCountryId).FirstOrDefault();
                                    if (country != null)
                                    {
                                        shipment.PickupCountry = country.EnglishName;
                                    }
                                    break;
                                }
                        }
                    }

                    if (myLastDelivery != null)
                    {
                        shipment.LastDeliveryArrivalDate = myLastDelivery.ATA;

                        if (!string.IsNullOrEmpty(myLastDelivery.CarrierId))
                        {
                            Card truckerCard = CardList.Where(p => p.Id == myLastDelivery.CarrierId).First();
                            if (truckerCard != null)
                            {
                                shipment.TruckerName = truckerCard.EnglishName;
                            }
                        }

                        switch (myLastDelivery.PickUpDeliveryToTypeCode)
                        {
                            case "PART":
                                {
                                    if (!string.IsNullOrEmpty(myLastDelivery.ToPartnerCardId))
                                    {
                                        Card myPartner = CardList.Where(p => p.Id == myLastDelivery.ToPartnerCardId).First();
                                        if (myPartner != null)
                                        {
                                            shipment.DeliveryToName = myPartner.EnglishName;
                                            Address myPartnerAddress = ToPartnerAddressLists.Where(d => d.Id == myLastDelivery.ToPartnerCardId).FirstOrDefault();//addressRepository.GetMainAddressByCardId(myLastDelivery.ToPartnerCardId, tenant);
                                            if (myPartnerAddress != null)
                                            {
                                                shipment.DeliveryTocity = myPartnerAddress.City;
                                            }
                                        }
                                    }
                                    break;
                                }

                            case "PORT":
                                {
                                    if (!string.IsNullOrEmpty(myLastDelivery.ToPortId))
                                    {
                                        PortPM myPort = PortQuery.GetSinglePort(tenant, myLastDelivery.ToPortId, true);
                                        if (myPort != null)
                                        {

                                            shipment.DeliveryToName = myPort.EnglishName;
                                            shipment.DeliveryTocity = myPort.StateName;
                                        }
                                    }
                                    break;
                                }

                            case "CASL":
                                {
                                    string myCity = myLastDelivery.ToAddressCity;
                                    if (!string.IsNullOrEmpty(myCity))
                                    {
                                        shipment.DeliveryToName = myCity;
                                        shipment.DeliveryTocity = myCity;
                                    }
                                    break;
                                }
                        }
                    }

                    if (!string.IsNullOrEmpty(Item.CustomerId))
                    {
                        Card customer = CardList.Where(d => d.Id == Item.CustomerId).FirstOrDefault();
                        if (customer != null)
                        {
                            shipment.CustomerExternalID = customer.ReceivablesAccountingCard;
                        }
                    }

                    totalData.Shipments.Add(shipment);
                }
            }

            totalData.FromDate = FromDate;
            totalData.ToDate = ToDate;
            #endregion

            return totalData;
        }

        private static QueryOperations GetQueryOperationsFromXmlFilters(byte[] xmlFilters)
        {
            using (MemoryStream memorystream = new MemoryStream(xmlFilters))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
                return (QueryOperations)serializer.Deserialize(memorystream);
            }
        }


        #endregion

        #region License Management
        public byte[] LoadLicenseManagementDataProvider(byte[] xmlFilters, int tenant)
        {
            LicenseManagementDataProvider dataprovider = GetLicenseManagementDataProvider(xmlFilters, tenant); ;
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(LicenseManagementDataProvider), tenant);
        }

        private LicenseManagementDataProvider GetLicenseManagementDataProvider(byte[] xmlFilters, int tenant)
        {
            LicenseManagementDataProvider totalData = new LicenseManagementDataProvider();
            totalData.LicensedUsers = new List<LicenseManagementDataList>();

            ICommonDataContext myContext = CommonDataContext.GetContext(tenant);
            UserRepository userRepository = new UserRepository(myContext);
            UserLicenseRepository myRepository = new UserLicenseRepository(myContext);
            PackageRepository packageRepository = new PackageRepository(myContext);
            TenantManagementLicenseRepository myTenantRepository = new TenantManagementLicenseRepository(tenant);

            #region Report Filters

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            QueryFilterItem filterItem_UserId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "UserId").FirstOrDefault();

            string userId = null;
            if (filterItem_UserId != null)
            {
                if (filterItem_UserId.FieldValue != null)
                {
                    userId = filterItem_UserId.FieldValue.ToString();
                }
            }

            #endregion

            IQueryable<TenantManagementLicense> tenantManagementLicenses = myTenantRepository.GetTenantManagementLicenses(tenant);
            IQueryable<User> usersList = userRepository.GetUsers(tenant);
            usersList = usersList.Where(d => !d.Contact.InActive);

            if (!string.IsNullOrEmpty(userId))
            {
                usersList = usersList.Where(d => d.Id == userId);
            }

            List<string> Codes = tenantManagementLicenses.Select(s => s.PackageCode).ToList();
            IQueryable<UserLicense> allLicenses = myRepository.GetUserLicenses(tenant);
            IQueryable<UserLicense> usersLicenses = (from d in allLicenses
                                                     where Codes.Contains(d.PackageCode)
                                                     select d);

            List<UserLicenseClass> totalDataList = new List<UserLicenseClass>();
            foreach (TenantManagementLicense license in tenantManagementLicenses.OrderBy(o => o.PackageCode))
            {
                string packageName = "";
                Package package = packageRepository.GetSinglePackage(license.PackageCode);
                if (package != null)
                {
                    packageName = package.Name + " (" + allLicenses.Where(d => d.PackageCode == license.PackageCode).Count() + "/" + license.NumberOfUsers + ")";
                }

                foreach (User user in usersList.OrderBy(o => o.Contact.EnglishName))
                {
                    UserLicenseClass myResultItem = new UserLicenseClass()
                    {
                        Id = user.Id,
                        Tenant = user.Tenant,
                        EnglishName = user.Contact.EnglishName,
                        Email = user.Contact.Email,
                        PackageCode = license.PackageCode,
                        PackageName = packageName,
                    };

                    UserLicense item = usersLicenses.Where(d => d.PackageCode == license.PackageCode && d.UserId == user.Id).FirstOrDefault();
                    if (item != null)
                    {
                        myResultItem.IsChecked = true;
                    }

                    totalDataList.Add(myResultItem);
                }
            }

            var groupedData = from item in totalDataList
                              group item by new { item.EnglishName, item.Email, } into g
                              select new
                              {
                                  UserName = g.Key.EnglishName,
                                  UserEmail = g.Key.Email,
                                  UserPackageItems = g,
                              };

            foreach (var item in groupedData.OrderBy(d => d.UserName))
            {
                foreach (var item11 in item.UserPackageItems)
                {
                    totalData.LicensedUsers.Add(new LicenseManagementDataList()
                    {
                        UserName = item11.EnglishName,
                        UserEmail = item11.Email,
                        PackageCode = item11.PackageCode,
                        PackageName = item11.PackageName,
                        Exists = item11.NumberOfUsers != null ? (item11.PackageUsers + "/" + item11.NumberOfUsers) : (item11.IsChecked ? "1" : "0"),
                    });
                }
            }

            return totalData;
        }
        #endregion

        #region Shipments Events List
        public byte[] LoadShipmentsEventsListDataProvider(byte[] xmlFilters, int tenant)
        {
            ShipmentsEventsListDataProvider dataprovider = GetShipmentsEventsListDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(ShipmentsEventsListDataProvider), tenant);
        }


        private ShipmentsEventsListDataProvider GetShipmentsEventsListDataProvider(byte[] xmlFilters, int tenant)
        {
            ShipmentsEventsListDataProvider shipmentsEventsListDataProvider = new ShipmentsEventsListDataProvider();
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            bool manuallyAddedEventsOnly = false;
            string userId = string.Empty;
            string EventTypeId = string.Empty;
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            QueryFilterItem filterItem_UserId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "UserId").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate" && d.Operator == "GreaterThanOrEqual").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate" && d.Operator == "LessThanOrEqual").FirstOrDefault();
            QueryFilterItem filterItem_ManuallyAddedEventsOnly = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ManuallyAddedEventsOnly").FirstOrDefault();
            QueryFilterItem filterItem_EventTypeId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "EventTypeId").FirstOrDefault();


            if (filterItem_ManuallyAddedEventsOnly != null)
            {
                if (filterItem_ManuallyAddedEventsOnly.FieldValue != null)
                {
                    manuallyAddedEventsOnly = (bool)filterItem_ManuallyAddedEventsOnly.FieldValue;
                }
            }
            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);
            }
            if (filterItem_ToDate != null)
            {
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
            }
            if (filterItem_UserId != null)
            {
                if (filterItem_UserId.FieldValue != null)
                {
                    userId = filterItem_UserId.FieldValue.ToString();
                }
            }

            if (filterItem_EventTypeId != null)
            {
                if (filterItem_EventTypeId.FieldValue != null)
                {
                    EventTypeId = filterItem_EventTypeId.FieldValue.ToString();
                }
            }
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            List<ShipmentList> shipmentlists = shipmentQuery.GetShipmentListsByFromCreateDateAndToCreateDate(fromDate, toDate, tenant).ToList();
            if (shipmentlists.Count > 0)
            {
                List<string> shipmentIds = shipmentlists.Select(d => d.Id).ToList();
                TraceEventQuery traceEventQuery = new TraceEventQuery(tenant);
                ObjectTableQuery objectTableQuery = new ObjectTableQuery(tenant);



                ObjectTablePM table = objectTableQuery.GetObjectTableByName("Shipment", 0);
                IQueryable<TraceEventPM> traceEventPMs = traceEventQuery.GetTraceEventPMsByEntityIdsAndObjectTableId(shipmentIds, table.Id, tenant);
                List<TraceEventPM> traceEvenList = FilterTraceEvenList(manuallyAddedEventsOnly, userId, EventTypeId, traceEventPMs);

                FillShipmentNumberToTraceEventList(shipmentlists, traceEvenList);
                List<ShipmentEventsList> shipmentEventsLists = new List<ShipmentEventsList>();
                GroupTraceEvenListByShipmentNumberAndOrderByEventDate(traceEvenList, shipmentEventsLists);

                shipmentsEventsListDataProvider.ShipmentEventsLists = shipmentEventsLists;
            }
            return shipmentsEventsListDataProvider;
        }

        private void GroupTraceEvenListByShipmentNumberAndOrderByEventDate(List<TraceEventPM> traceEvenList, List<ShipmentEventsList> shipmentEventsLists)
        {
            IEnumerable<IGrouping<string, TraceEventPM>> traceEventgroups = traceEvenList.ToList().GroupBy(q => q.EntityNumber);
            foreach (IGrouping<string, TraceEventPM> traceEventgroup in traceEventgroups)
            {
                foreach (TraceEventPM item in traceEventgroup.OrderBy(d => d.EventDateTime).ToList())
                {
                    //item.EventDateTime1 = item.EventDateTime.ToString("yyyy-MM-dd");
                    //item.EventDateTime2 = item.EventDateTime.ToString("HH: mm:ss");
                    ShipmentEventsList shipmentEventsList = CreateNewShipmentEventsList(item);

                    shipmentEventsLists.Add(shipmentEventsList);
                }
            }
        }

        private static ShipmentEventsList CreateNewShipmentEventsList(TraceEventPM item)
        {
            return new ShipmentEventsList()
            {
                ShipmentNumber = item.EntityNumber,
                EventCode = item.EventTypeCode,
                EventName = item.EventTypeEnglishName,
                EventDate = item.EventDateTime,
                //EventDate1 = item.EventDateTime.ToString("yyyy-MM-dd"),
                //EventDate2 = item.EventDateTime.ToString("HH: mm:ss"),
                LogDate = item.LogDateTime,
                UserName = item.ContactEnglishFirstName,
                Notes = item.Notes,
            };
        }

        private void FillShipmentNumberToTraceEventList(List<ShipmentList> shipmentlists, List<TraceEventPM> traceEvenList)
        {

            foreach (TraceEventPM traceEventPM in traceEvenList)
            {
                var shipment = shipmentlists.Where
                    (d => d.Id == traceEventPM.EntityId).FirstOrDefault();
                if (shipment != null) traceEventPM.EntityNumber = shipment.ShipmentNumber;
            }

        }


        private List<TraceEventPM> FilterTraceEvenList(bool manuallyAddedEventsOnly, string userId, string EventTypeId, IQueryable<TraceEventPM> traceEventPMs)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                traceEventPMs = traceEventPMs.Where(d => d.UserId == userId);
            }
            if (!string.IsNullOrEmpty(EventTypeId))
            {
                traceEventPMs = traceEventPMs.Where(d => d.EventTypeId == EventTypeId);
            }

            if (manuallyAddedEventsOnly)
            {
                traceEventPMs = traceEventPMs.Where(d => d.IsAddedManually);
            }

            List<TraceEventPM> traceEvenList = traceEventPMs.ToList();
            return traceEvenList;
        }
        #endregion


        #region Automation Test Report
        public byte[] LoadAutomationTestReportDataProvider(byte[] xmlFilters, int tenant)
        {
            AutomationTestReportDataProvider dataprovider = GetAutomationTestReportDataProvider(xmlFilters, tenant);
            return new ReportMemoryStreamService().Convert(dataprovider, typeof(AutomationTestReportDataProvider), tenant);
        }


        private AutomationTestReportDataProvider GetAutomationTestReportDataProvider(byte[] xmlFilters, int tenant)
        {
            AutomationTestReportDataProvider automationTestReportDataProvider = new AutomationTestReportDataProvider();

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            QueryFilterItem filterItem_IsException = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IsException").FirstOrDefault();

            if (filterItem_IsException != null)
            {
                if (filterItem_IsException.FieldValue != null)
                {
                    automationTestReportDataProvider.IsException = (bool)filterItem_IsException.FieldValue;
                }
            }


            if (automationTestReportDataProvider.IsException)
            {
                throw new Exception("Exception Test");
            }


            if (!automationTestReportDataProvider.IsException)
            {
                TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenant);
                var tenantManagementPM = tenantManagementQuery.GetSinglePM(tenant);
                if (tenantManagementPM != null)
                {
                    automationTestReportDataProvider.TenantName = tenantManagementPM.Name;
                    automationTestReportDataProvider.PackageName = tenantManagementPM.PackageName;
                    automationTestReportDataProvider.CreateDate = tenantManagementPM.CreateDate;
                    automationTestReportDataProvider.UpdateDate = tenantManagementPM.UpdateDate;
                    automationTestReportDataProvider.Notes = tenantManagementPM.Notes;
                }
            }

            return automationTestReportDataProvider;
        }



        #endregion

        private string GetAuthenticatedUser(int tenant)
        {
            string email = "";
            if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                email = HttpContext.Current.User.Identity.Name;
            }
            else
            {
                email = "system@tenant" + tenant.ToString() + ".com";
            }

            return email;
        }



        private ContactPM GetLoggedContact(int tenant)
        {
            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedContact;
        }
    }

    public class ProductActualDataHelper
    {
        [Key]
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string SalesmanId { get; set; }
        public string SalesmanName { get; set; }
        public DateTime? Date { get; set; }
        public string ProductCode { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }

        public decimal? TEU { get; set; }
        public decimal? Revenue { get; set; }
        public decimal? NumberOfShipments { get; set; }
        public decimal? ChargeableWeight { get; set; }

        public int LocationsCount { get; set; }
    }

    public class AmountsClass
    {
        public string Currency { get; set; }
        public double Amount { get; set; }
    }

    public class CardEntityClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PaymentTerm { get; set; }
        public string CardCode { get; set; }
        public string CardTypeName { get; set; }
        public string Salesman { get; set; }
    }

    public class AgingStatemantDataItem
    {
        public string Id { get; set; }
        public string TypeCode { get; set; }
        public string EntityName { get; set; }
        public string PartnerId { get; set; }
        public string EntityTypeCode { get; set; }
        public DateTime? Date { get; set; }
        public string CardId { get; set; }
        public double? Amount { get; set; }
        public double? Debit { get; set; }
        public double? Credit { get; set; }
        public double? ProfitExchangeRate { get; set; }
    }

    public class GroupedPeriodM
    {
        public string PeriodName { get; set; }
        public decimal GrandTotal { get; set; }
    }

    public class VATClass
    {
        public int Index { get; set; }
        public double? Percentage { get; set; }
        public string VATName { get; set; }
        public string VATCode { get; set; }
        public double? LocalAmount { get; set; }
        public double? InvoiceAmount { get; set; }
    }

    public class UserLicenseClass
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string EnglishName { get; set; }
        public string Email { get; set; }
        public bool IsChecked { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public int? NumberOfUsers { get; set; }
        public int? PackageUsers { get; set; }
    }
}