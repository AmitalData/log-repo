using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.CRM.BL.DataContracts;
using Logitude.CRM.BL.EntityQueryServices;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.WebServices
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class DashBoardWebService : System.Web.Services.WebService
    {
        #region New Quotes by Salesman

        [WebMethod]
        public byte[] LoadQuotesBySalemsmanData(string code, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            DashBoardDataClass dataprovider = GetQuotesBySalemsmanData(code, ownerId, businessUnitId, fieldCode, tenant, isTopTen);
            XmlSerializer serializer = new XmlSerializer(typeof(DashBoardDataClass));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, dataprovider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        private DashBoardDataClass GetQuotesBySalemsmanData(string code, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            DashBoardDataClass totalData = new DashBoardDataClass();
            totalData.GeneralList = new List<GeneralDataClass>();

            QuoteQuery quoteQuery = new QuoteQuery(tenant);
            List<ChartingDataClass> myResult = quoteQuery.GetQuotesGroupBySalesman(code, ownerId, businessUnitId, fieldCode, tenant, isTopTen);

            if (myResult != null && myResult.Count > 0)
            {
                foreach (ChartingDataClass item in myResult)
                {
                    GeneralDataClass listItem = new GeneralDataClass()
                    {
                        Id = item.Id,
                        DataTypeCode = item.DataTypeCode,
                        StringProperty = item.StringProperty,
                        IntegerProperty = item.IntegerProperty,
                        Day = item.Day,
                        OwnerId = item.OwnerId,
                        BusinessUnitId = item.BusinessUnitId,
                    };

                    totalData.GeneralList.Add(listItem);
                }
            }

            return totalData;
        }

        #endregion

        #region New Opportunities by Salesman

        [WebMethod]
        public byte[] LoadOpportunitiesBySalemsmanData(string code, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            DashBoardDataClass dataprovider = GetOpportunitiesBySalemsmanData(code, ownerId, businessUnitId, fieldCode, tenant, isTopTen);
            XmlSerializer serializer = new XmlSerializer(typeof(DashBoardDataClass));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, dataprovider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        private DashBoardDataClass GetOpportunitiesBySalemsmanData(string code, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            DashBoardDataClass totalData = new DashBoardDataClass();
            totalData.GeneralList = new List<GeneralDataClass>();

            OpportunityQueryService opportunityQuery = new OpportunityQueryService(tenant);
            List<CRMChartingClass> myResult = opportunityQuery.GetOpportunitiesGroupBySalesman(code, ownerId, businessUnitId, fieldCode, tenant, isTopTen);

            if (myResult != null && myResult.Count > 0)
            {
                foreach (CRMChartingClass item in myResult)
                {
                    GeneralDataClass listItem = new GeneralDataClass()
                    {
                        Id = item.Id,
                        DataTypeCode = item.DataTypeCode,
                        StringProperty = item.StringProperty,
                        IntegerProperty = item.IntegerProperty,
                        OwnerId = item.OwnerId,
                    };

                    totalData.GeneralList.Add(listItem);
                }
            }

            return totalData;
        }

        #endregion

        #region New Customers by Salesman

        [WebMethod]
        public byte[] LoadCustomersBySalemsmanData(int days, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            DashBoardDataClass dataprovider = GetCustomersBySalemsmanData(days, ownerId, businessUnitId, fieldCode, tenant, isTopTen);
            XmlSerializer serializer = new XmlSerializer(typeof(DashBoardDataClass));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, dataprovider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        private DashBoardDataClass GetCustomersBySalemsmanData(int days, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            DashBoardDataClass totalData = new DashBoardDataClass();
            totalData.GeneralList = new List<GeneralDataClass>();

            CustomerQuery customerQuery = new CustomerQuery(tenant);
            List<ChartingDataClass> myResult = customerQuery.GetCustomersGroupBySalesman(days, ownerId, businessUnitId, fieldCode, tenant, isTopTen);

            if (myResult != null && myResult.Count > 0)
            {
                foreach (ChartingDataClass item in myResult)
                {
                    GeneralDataClass listItem = new GeneralDataClass()
                    {
                        Id = item.Id,
                        DataTypeCode = item.DataTypeCode,
                        StringProperty = item.StringProperty,
                        IntegerProperty = item.IntegerProperty,
                        OwnerId = item.OwnerId,
                        BusinessUnitId = item.BusinessUnitId,
                    };

                    totalData.GeneralList.Add(listItem);
                }
            }

            return totalData;
        }

        #endregion        

        #region Activity Status Details

        [WebMethod]
        public byte[] LoadActivityStatusDetailsData(byte[] xmlFilters, int tenant)
        {
            DashBoardDataClass dataprovider = GetActivityStatusDetailsData(xmlFilters, tenant);
            XmlSerializer serializer = new XmlSerializer(typeof(DashBoardDataClass));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, dataprovider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        private DashBoardDataClass GetActivityStatusDetailsData(byte[] xmlFilters, int tenant)
        {
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            QueryFilterItem filterItem_LastDays = queryOperations.QueryFilterItems.Where(d => d.FieldName == "LastDays").FirstOrDefault();
            int LastDays = 0;
            if (filterItem_LastDays != null)
            {
                if (filterItem_LastDays.FieldValue != null)
                {
                    Int32.TryParse(filterItem_LastDays.FieldValue.ToString(), out LastDays);
                  //  LastDays = (int)filterItem_LastDays.FieldValue;
                }
            }

            QueryFilterItem filterItem_LastMonths = queryOperations.QueryFilterItems.Where(d => d.FieldName == "LastMonths").FirstOrDefault();
            int LastMonths = 0;
            if (filterItem_LastMonths != null)
            {
                if (filterItem_LastMonths.FieldValue != null)
                {
                    LastMonths =Convert.ToInt32(filterItem_LastMonths.FieldValue);
                }
            }

            QueryFilterItem filterItem_ShowIndex = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ShowIndex").FirstOrDefault();
            int ShowIndex = 0;
            if (filterItem_ShowIndex != null)
            {
                if (filterItem_ShowIndex.FieldValue != null)
                {
                    Int32.TryParse(filterItem_ShowIndex.FieldValue.ToString(), out ShowIndex);

                   // ShowIndex = (int)filterItem_ShowIndex.FieldValue;
                }
            }

            QueryFilterItem filterItem_DirectionId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DirectionId").FirstOrDefault();
            string DirectionId = null;
            if (filterItem_DirectionId != null)
            {
                if (filterItem_DirectionId.FieldValue != null)
                {
                    DirectionId = filterItem_DirectionId.FieldValue.ToString();
                }
            }

            QueryFilterItem filterItem_TransportModeId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "TransportModeId").FirstOrDefault();
            string TransportModeId = null;
            if (filterItem_TransportModeId != null)
            {
                if (filterItem_TransportModeId.FieldValue != null)
                {
                    TransportModeId = filterItem_TransportModeId.FieldValue.ToString();
                }
            }

            DashBoardDataClass totalData = new DashBoardDataClass();
           
               
            totalData.CustomersList = new List<CustomersDataClass>();
            totalData.CountriesList = new List<CountriesDataClass>();
            totalData.DirectionTransportModeList = new List<DirectionTransportModeDataClass>();
            totalData.ShipmentQuantityList = new List<ShipmentQuantityDataClass>();


            DateTime? fromDate = null;
            DateTime? toDate = null;

            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            if (filterItem_FromDate != null && filterItem_FromDate.FieldValue != null) fromDate = DateTime.Parse(filterItem_FromDate.FieldValue.ToString());

            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            if (filterItem_ToDate != null && filterItem_ToDate.FieldValue != null) toDate = DateTime.Parse(filterItem_ToDate.FieldValue.ToString());

            QueryFilterItem filterItem_TimeRange = queryOperations.QueryFilterItems.Where(d => d.FieldName == "TimeRange").FirstOrDefault();
            string TimeRange = null;
            if (filterItem_TimeRange != null)
            {
                if (filterItem_TimeRange.FieldValue != null)
                {
                    TimeRange = filterItem_TimeRange.FieldValue.ToString();
                }
            }


            totalData.FromDate = fromDate;
            totalData.ToDate = toDate;
            totalData.TimeRange = TimeRange;

            if (TimeRange == "Custom")
            {
                totalData.CustomersList = this.GetCustomersDataCustom(tenant, LastMonths, LastDays, ShowIndex, DirectionId, TransportModeId,fromDate,toDate);
                totalData.CountriesList = this.GetCountriesDataCustom(tenant, LastMonths, LastDays, ShowIndex, DirectionId, TransportModeId,fromDate,toDate);
                totalData.DirectionTransportModeList = this.GetDirectionTransportModeDataCustom(tenant, LastMonths, LastDays, ShowIndex,fromDate,toDate);
                totalData.ShipmentQuantityList = this.GetShipmentsData(tenant, LastMonths, -365, ShowIndex, DirectionId, TransportModeId);

            }
            else
            {
                totalData.CustomersList = this.GetCustomersData(tenant, LastMonths, LastDays, ShowIndex, DirectionId, TransportModeId);
                totalData.CountriesList = this.GetCountriesData(tenant, LastMonths, LastDays, ShowIndex, DirectionId, TransportModeId);
                totalData.DirectionTransportModeList = this.GetDirectionTransportModeData(tenant, LastMonths, LastDays, ShowIndex);
                totalData.ShipmentQuantityList = this.GetShipmentsData(tenant, LastMonths, LastDays, ShowIndex, DirectionId, TransportModeId);

            }
            return totalData;
        }


        private List<CustomersDataClass> GetCustomersDataCustom(int tenant, int lastMonths, int lastDays, int measurment, string directionId, string transmodeId,DateTime?fromDate,DateTime?toDate)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            List<DashBoardClass> list = shipmentQuery.GetTop10DashBoardCustom(null, fromDate, toDate, measurment, tenant, 10, false, directionId, transmodeId).ToList();

            List<DashBoardClass> list1 = (from l in list
                                          group l by new
                                          {
                                              l.CustomerName,
                                              l.CustomerID,
                                          }
                          into m
                                          orderby m.Key.CustomerName
                                          select new DashBoardClass()
                                          {
                                              CustomerID = m.Key.CustomerID,
                                              CustomerName = m.Key.CustomerName,
                                              total = m.Sum(d => d.total),
                                              sumGrossWeight = m.Sum(d => d.sumGrossWeight),
                                              sumChargeableWeight = m.Sum(d => d.sumChargeableWeight),
                                              totalProfitInLocalCurrency = m.Sum(d => d.totalProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.totalProfitInProfitCurrency),
                                              ReceivablesInLocalCurrency = m.Sum(d => d.ReceivablesInLocalCurrency),
                                              ReceivablesInProfitCurrency = m.Sum(d => d.ReceivablesInProfitCurrency),
                                          }).ToList<DashBoardClass>();



            List<CustomersDataClass> result = new List<CustomersDataClass>();

            if (list1 != null)
            {
                int i = 0;
                foreach (DashBoardClass item in list1)
                {
                    CustomersDataClass newItem = new CustomersDataClass()
                    {
                        Id = i++,
                        CustomerId = item.CustomerID,
                        CustomerName = item.CustomerName,
                        DirectionId = item.directionID,
                        TransportModeId = item.transportModeID,
                        Total = item.total,
                        SumGrossWeight = item.sumGrossWeight,
                        SumChargeableWeight = item.sumChargeableWeight,
                        ProfitInLocalCurrency = item.totalProfitInLocalCurrency,
                        ProfitInProfitCurrency = item.totalProfitInProfitCurrency,
                        ReceivablesInLocalCurrency = item.ReceivablesInLocalCurrency,
                        ReceivablesInProfitCurrency = item.ReceivablesInProfitCurrency,
                    };

                    result.Add(newItem);
                }
            }

            foreach (CustomersDataClass item in result)
            {
                switch (measurment)
                {
                    case 0:
                        {
                            item.GeneralTotal = item.Total;
                            break;
                        }

                    case 1:
                        {
                            item.GeneralTotal = item.SumChargeableWeight;
                            break;
                        }

                    case 2:
                        {
                            item.GeneralTotal = item.SumGrossWeight;
                            break;
                        }

                    case 3:
                        {
                            item.GeneralTotal = item.ProfitInLocalCurrency;
                            break;
                        }

                    case 4:
                        {
                            item.GeneralTotal = item.ProfitInProfitCurrency;
                            break;
                        }

                    case 5:
                        {
                            item.GeneralTotal = item.ReceivablesInLocalCurrency;
                            break;
                        }

                    case 6:
                        {
                            item.GeneralTotal = item.ReceivablesInProfitCurrency;
                            break;
                        }
                }

                item.GeneralTotal = Math.Round(item.GeneralTotal.Value, 2);
            }

            return result;
        }

        private List<CountriesDataClass> GetCountriesDataCustom(int tenant, int lastMonths, int lastDays, int measurment, string directionId, string transmodeId,DateTime?fromDate, DateTime? toDate)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            List<DashBoardClass> list = shipmentQuery.GetShipmentsByTop10CountriesDashBoardCustom2(null, fromDate, toDate, measurment, tenant, 10, false, null, directionId, transmodeId).ToList();

            List<DashBoardClass> list1 = (from l in list
                                          where l.countryName != "Others"
                                          group l by new
                                          {
                                              l.countryName,
                                              l.countryCode
                                          }
                      into m
                                          orderby m.Key.countryName
                                          select new DashBoardClass()
                                          {
                                              countryCode = m.Key.countryCode,
                                              countryName = m.Key.countryName,
                                              total = m.Sum(d => d.total),
                                              sumGrossWeight = m.Sum(d => d.sumGrossWeight),
                                              sumChargeableWeight = m.Sum(d => d.sumChargeableWeight),
                                              totalLastMonth = m.Sum(d => d.totalLastMonth),
                                              sumChargeableWeightLastMonth = m.Sum(d => d.sumChargeableWeightLastMonth),
                                              sumGrossWeightLastMonth = m.Sum(d => d.sumGrossWeightLastMonth),
                                              totalProfitInLocalCurrency = m.Sum(d => d.totalProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.totalProfitInProfitCurrency),
                                              ReceivablesInLocalCurrency = m.Sum(d => d.ReceivablesInLocalCurrency),
                                              ReceivablesInProfitCurrency = m.Sum(d => d.ReceivablesInProfitCurrency),
                                          }).ToList<DashBoardClass>();

            //OrderByDescending(d => d.YField).Take(Convert.ToInt32(10))

            List<CountriesDataClass> result = new List<CountriesDataClass>();

            if (list1 != null)
            {
                int i = 0;
                foreach (DashBoardClass item in list1)
                {
                    CountriesDataClass newItem = new CountriesDataClass()
                    {
                        Id = i++,
                        CountryCode = item.countryCode,
                        CountryName = item.countryName,
                        Total = item.total,
                        SumGrossWeight = item.sumGrossWeight,
                        SumChargeableWeight = item.sumChargeableWeight,
                        TotalLastMonth = item.totalLastMonth,
                        SumChargeableWeightLastMonth = item.sumChargeableWeightLastMonth,
                        SumGrossWeightLastMonth = item.sumGrossWeightLastMonth,
                        ProfitInLocalCurrency = item.totalProfitInLocalCurrency,
                        ProfitInProfitCurrency = item.totalProfitInProfitCurrency,
                        ReceivablesInLocalCurrency = item.ReceivablesInLocalCurrency,
                        ReceivablesInProfitCurrency = item.ReceivablesInProfitCurrency,
                    };

                    result.Add(newItem);
                }
            }

            foreach (CountriesDataClass item in result)
            {
                switch (measurment)
                {
                    case 0:
                        {
                            item.GeneralTotal = item.Total;
                            break;
                        }

                    case 1:
                        {
                            item.GeneralTotal = item.SumChargeableWeight;
                            break;
                        }

                    case 2:
                        {
                            item.GeneralTotal = item.SumGrossWeight;
                            break;
                        }

                    case 3:
                        {
                            item.GeneralTotal = item.ProfitInLocalCurrency;
                            break;
                        }

                    case 4:
                        {
                            item.GeneralTotal = item.ProfitInProfitCurrency;
                            break;
                        }

                    case 5:
                        {
                            item.GeneralTotal = item.ReceivablesInLocalCurrency;
                            break;
                        }

                    case 6:
                        {
                            item.GeneralTotal = item.ReceivablesInProfitCurrency;
                            break;
                        }
                }

                item.GeneralTotal = Math.Round(item.GeneralTotal.Value, 2);
            }

            return result;
        }

        private List<DirectionTransportModeDataClass> GetDirectionTransportModeDataCustom(int tenant, int lastMonths, int lastDays, int measurment, DateTime? fromDate, DateTime? toDate)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            List<DashBoardClass> list = shipmentQuery.GetShipmentsByDirectionAndTransModeCustom(null, fromDate, toDate, tenant, null).ToList();

            List<DashBoardClass> list1 = (from l in list
                                          group l by new
                                          {
                                              l.transportModeID,
                                              l.directionID,
                                              l.DirectionName,
                                              l.TransportModeName,
                                          }
                                          into m
                                          orderby m.Key.directionID
                                          select new DashBoardClass()
                                          {
                                              directionID = m.Key.directionID,
                                              transportModeID = m.Key.transportModeID,
                                              TransportModeName = m.Key.TransportModeName,
                                              DirectionName = m.Key.DirectionName,
                                              total = m.Sum(d => d.total),
                                              sumChargeableWeight = m.Sum(d => d.sumChargeableWeight),
                                              sumGrossWeight = m.Sum(d => d.sumGrossWeight),
                                              totalProfitInLocalCurrency = m.Sum(d => d.totalProfitInLocalCurrency),
                                              totalProfitInProfitCurrency = m.Sum(d => d.totalProfitInProfitCurrency),
                                              ReceivablesInLocalCurrency = m.Sum(d => d.ReceivablesInLocalCurrency),
                                              ReceivablesInProfitCurrency = m.Sum(d => d.ReceivablesInProfitCurrency),
                                          }).ToList<DashBoardClass>();

            List<DirectionTransportModeDataClass> dataList = new List<DirectionTransportModeDataClass>();

            if (list1 != null)
            {
                int i = 0;
                foreach (DashBoardClass item in list1)
                {
                    DirectionTransportModeDataClass newItem = new DirectionTransportModeDataClass()
                    {
                        Id = i++,
                        Day = item.day,
                        Year = item.year,
                        Month = item.month,
                        DirectionId = item.directionID,
                        TransportModeId = item.transportModeID,
                        TransportModeName = item.TransportModeName,
                        DirectionName = item.DirectionName,
                        Total = item.total,
                        SumGrossWeight = item.sumGrossWeight,
                        SumChargeableWeight = item.sumChargeableWeight,
                        TotalProfitInLocalCurrency = item.totalProfitInLocalCurrency,
                        TotalProfitInProfitCurrency = item.totalProfitInProfitCurrency,
                        ReceivablesInLocalCurrency = item.ReceivablesInLocalCurrency,
                        ReceivablesInProfitCurrency = item.ReceivablesInProfitCurrency,
                        DirectionTransportModeNames = item.DirectionName + "/" + item.TransportModeName,
                    };

                    dataList.Add(newItem);
                }
            }

            foreach (DirectionTransportModeDataClass item in dataList)
            {
                switch (measurment)
                {
                    case 0:
                        {
                            item.GeneralTotal = item.Total;
                            break;
                        }
                    case 1:
                        {
                            item.GeneralTotal = item.SumChargeableWeight;
                            break;
                        }
                    case 2:
                        {
                            item.GeneralTotal = item.SumGrossWeight;
                            break;
                        }
                    case 3:
                        {
                            item.GeneralTotal = item.TotalProfitInLocalCurrency;
                            break;
                        }
                    case 4:
                        {
                            item.GeneralTotal = item.TotalProfitInProfitCurrency;
                            break;
                        }
                    case 5:
                        {
                            item.GeneralTotal = item.ReceivablesInLocalCurrency;
                            break;
                        }
                    case 6:
                        {
                            item.GeneralTotal = item.ReceivablesInProfitCurrency;
                            break;
                        }
                }

                item.GeneralTotal = Math.Round(item.GeneralTotal.Value, 2);
            }

            return dataList;
        }

        private List<ShipmentQuantityDataClass> GetShipmentsDataCustom(int tenant, int lastMonths, int lastDays, int measurment, string directionId, string transmodeId, DateTime? fromDate, DateTime? toDate)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            List<DashBoardClass> list = shipmentQuery.GetShipmentsByCreateOperationalDate(null, fromDate, toDate, tenant, null, directionId, transmodeId);
            List<DashBoardClass> list1 = new List<DashBoardClass>();
           
            

            List<ShipmentQuantityDataClass> result = new List<ShipmentQuantityDataClass>();

            if (list != null)
            {
                int i = 0;
                foreach (DashBoardClass item in list)
                {
                    ShipmentQuantityDataClass newItem = new ShipmentQuantityDataClass()
                    {
                        Id = i++,
                        Day = item.day,
                        Month = item.month,
                        Year = item.year,
                        DirectionId = item.directionID,
                        DirectionName = item.DirectionName,
                        TransportModeId = item.transportModeID,
                        TransportModeName = item.TransportModeName,
                        Total = item.total,
                        SumGrossWeight = item.sumGrossWeight,
                        SumChargeableWeight = item.sumChargeableWeight,
                        TotalProfitInLocalCurrency = item.totalProfitInLocalCurrency,
                        TotalProfitInProfitCurrency = item.totalProfitInProfitCurrency,
                        ReceivablesInLocalCurrency = item.ReceivablesInLocalCurrency,
                        ReceivablesInProfitCurrency = item.ReceivablesInProfitCurrency,
                        XField = item.XField,
                        GeneralTotal = item.GeneralTotal,
                    };

                    result.Add(newItem);
                }
            }

            return result;
        }


        private List<CustomersDataClass> GetCustomersData(int tenant, int lastMonths, int lastDays, int measurment, string directionId, string transmodeId)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            List<DashBoardClass> list = shipmentQuery.GetTop10DashBoard(null,lastMonths, lastDays, measurment, tenant, 10, false,directionId, transmodeId).ToList();

            if (!string.IsNullOrEmpty(directionId))
            {
                list = list.Where(d => d.directionID == directionId).ToList();
            }

            if (!string.IsNullOrEmpty(transmodeId))
            {
                list = list.Where(d => d.transportModeID == transmodeId).ToList();
            }

            List<DashBoardClass>  list1 = (from l in list
                          group l by new 
                          { 
                              l.CustomerName,   
                              l.CustomerID,
                          } 
                          into m
                          orderby m.Key.CustomerName
                          select new DashBoardClass()
                          {
                              CustomerID = m.Key.CustomerID,
                              CustomerName = m.Key.CustomerName,
                              total = m.Sum(d => d.total),
                              sumGrossWeight = m.Sum(d => d.sumGrossWeight),
                              sumChargeableWeight =  m.Sum(d => d.sumChargeableWeight),
                              totalProfitInLocalCurrency = m.Sum(d => d.totalProfitInLocalCurrency),
                              totalProfitInProfitCurrency = m.Sum(d => d.totalProfitInProfitCurrency),
                              ReceivablesInLocalCurrency = m.Sum(d => d.ReceivablesInLocalCurrency),
                              ReceivablesInProfitCurrency = m.Sum(d => d.ReceivablesInProfitCurrency),
                          }).ToList<DashBoardClass>();

            List<CustomersDataClass> result = new List<CustomersDataClass>();

            if (list1 != null)
            {
                int i = 0;
                foreach (DashBoardClass item in list1)
                {
                    CustomersDataClass newItem = new CustomersDataClass()
                    {
                        Id = i++,
                        CustomerId = item.CustomerID,
                        CustomerName = item.CustomerName,
                        DirectionId = item.directionID,
                        TransportModeId = item.transportModeID,
                        Total = item.total,
                        SumGrossWeight = item.sumGrossWeight,
                        SumChargeableWeight = item.sumChargeableWeight,
                        ProfitInLocalCurrency = item.totalProfitInLocalCurrency,
                        ProfitInProfitCurrency = item.totalProfitInProfitCurrency,
                        ReceivablesInLocalCurrency = item.ReceivablesInLocalCurrency,
                        ReceivablesInProfitCurrency = item.ReceivablesInProfitCurrency,
                    };

                    result.Add(newItem);
                }
            }

            foreach (CustomersDataClass item in result)
            {
                switch (measurment)
                {
                    case 0:
                        {
                            item.GeneralTotal = item.Total;
                            break;
                        }

                    case 1:
                        {
                            item.GeneralTotal = item.SumChargeableWeight;
                            break;
                        }

                    case 2:
                        {
                            item.GeneralTotal = item.SumGrossWeight;
                            break;
                        }

                    case 3:
                        {
                            item.GeneralTotal = item.ProfitInLocalCurrency;
                            break;
                        }

                    case 4:
                        {
                            item.GeneralTotal = item.ProfitInProfitCurrency;
                            break;
                        }

                    case 5:
                        {
                            item.GeneralTotal = item.ReceivablesInLocalCurrency;
                            break;
                        }

                    case 6:
                        {
                            item.GeneralTotal = item.ReceivablesInProfitCurrency;
                            break;
                        }
                }

                item.GeneralTotal = Math.Round(item.GeneralTotal.Value, 2);
            }

            return result;
        }

        private List<CountriesDataClass> GetCountriesData(int tenant, int lastMonths, int lastDays, int measurment, string directionId, string transmodeId)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            List<DashBoardClass> list = shipmentQuery.GetShipmentsByTop10CountriesDashBoard(null,lastMonths, lastDays, measurment, tenant, 10, false, null, directionId, transmodeId).ToList();

            List<DashBoardClass> list1 = (from l in list
                      where l.countryName != "Others"
                      group l by new 
                      { 
                          l.countryName, 
                          l.countryCode
                      } 
                      into m
                      orderby m.Key.countryName
                      select new DashBoardClass()
                      {
                          countryCode = m.Key.countryCode,
                          countryName = m.Key.countryName,
                          total = m.Sum(d => d.total),
                          sumGrossWeight = m.Sum(d => d.sumGrossWeight),
                          sumChargeableWeight = m.Sum(d => d.sumChargeableWeight),
                          totalLastMonth = m.Sum(d => d.totalLastMonth),
                          sumChargeableWeightLastMonth = m.Sum(d => d.sumChargeableWeightLastMonth),
                          sumGrossWeightLastMonth = m.Sum(d => d.sumGrossWeightLastMonth),
                          totalProfitInLocalCurrency = m.Sum(d => d.totalProfitInLocalCurrency),
                          totalProfitInProfitCurrency = m.Sum(d => d.totalProfitInProfitCurrency),
                          ReceivablesInLocalCurrency = m.Sum(d => d.ReceivablesInLocalCurrency),
                          ReceivablesInProfitCurrency = m.Sum(d => d.ReceivablesInProfitCurrency),
                      }).ToList<DashBoardClass>();
            
            //OrderByDescending(d => d.YField).Take(Convert.ToInt32(10))

            List<CountriesDataClass> result = new List<CountriesDataClass>();

            if (list1 != null)
            {
                int i = 0;
                foreach (DashBoardClass item in list1)
                {
                    CountriesDataClass newItem = new CountriesDataClass()
                    {
                        Id = i++,
                        CountryCode = item.countryCode,
                        CountryName = item.countryName,
                        Total = item.total,
                        SumGrossWeight = item.sumGrossWeight,
                        SumChargeableWeight = item.sumChargeableWeight,
                        TotalLastMonth = item.totalLastMonth,
                        SumChargeableWeightLastMonth = item.sumChargeableWeightLastMonth,
                        SumGrossWeightLastMonth = item.sumGrossWeightLastMonth,
                        ProfitInLocalCurrency = item.totalProfitInLocalCurrency,
                        ProfitInProfitCurrency = item.totalProfitInProfitCurrency,
                        ReceivablesInLocalCurrency = item.ReceivablesInLocalCurrency,
                        ReceivablesInProfitCurrency = item.ReceivablesInProfitCurrency,
                    };

                    result.Add(newItem);
                }
            }

            foreach (CountriesDataClass item in result)
            {
                switch (measurment)
                {
                    case 0:
                        {
                            item.GeneralTotal = item.Total;
                            break;
                        }

                    case 1:
                        {
                            item.GeneralTotal = item.SumChargeableWeight;
                            break;
                        }

                    case 2:
                        {
                            item.GeneralTotal = item.SumGrossWeight;
                            break;
                        }

                    case 3:
                        {
                            item.GeneralTotal = item.ProfitInLocalCurrency;
                            break;
                        }

                    case 4:
                        {
                            item.GeneralTotal = item.ProfitInProfitCurrency;
                            break;
                        }

                    case 5:
                        {
                            item.GeneralTotal = item.ReceivablesInLocalCurrency;
                            break;
                        }

                    case 6:
                        {
                            item.GeneralTotal = item.ReceivablesInProfitCurrency;
                            break;
                        }
                }

                item.GeneralTotal = Math.Round(item.GeneralTotal.Value, 2);
            }

            return result;
        }

        private List<DirectionTransportModeDataClass> GetDirectionTransportModeData(int tenant, int lastMonths, int lastDays, int measurment)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            List<DashBoardClass> list =  shipmentQuery.GetShipmentsByDirectionAndTransMode(null,lastMonths, lastDays, tenant, null).ToList();

            List<DashBoardClass> list1 = (from l in list
                                          group l by new 
                                          { 
                                              l.transportModeID, 
                                              l.directionID, 
                                              l.DirectionName, 
                                              l.TransportModeName,
                                          } 
                                          into m
                                          orderby m.Key.directionID
                                          select new DashBoardClass()
                                                       {
                                                           directionID = m.Key.directionID,
                                                           transportModeID = m.Key.transportModeID,
                                                           TransportModeName = m.Key.TransportModeName,
                                                           DirectionName = m.Key.DirectionName,
                                                           total = m.Sum(d => d.total),
                                                           sumChargeableWeight = m.Sum(d => d.sumChargeableWeight),
                                                           sumGrossWeight = m.Sum(d => d.sumGrossWeight),
                                                           totalProfitInLocalCurrency = m.Sum(d => d.totalProfitInLocalCurrency),
                                                           totalProfitInProfitCurrency = m.Sum(d => d.totalProfitInProfitCurrency),
                                                           ReceivablesInLocalCurrency = m.Sum(d => d.ReceivablesInLocalCurrency),
                                                           ReceivablesInProfitCurrency = m.Sum(d => d.ReceivablesInProfitCurrency),
                                                       }).ToList<DashBoardClass>();

            List<DirectionTransportModeDataClass> dataList = new List<DirectionTransportModeDataClass>();

            if (list1 != null)
            {
                int i = 0;
                foreach (DashBoardClass item in list1)
                {
                    DirectionTransportModeDataClass newItem = new DirectionTransportModeDataClass()
                    {
                        Id = i ++,
                        Day = item.day,
                        Year = item.year,
                        Month = item.month,
                        DirectionId = item.directionID,
                        TransportModeId = item.transportModeID,
                        TransportModeName = item.TransportModeName,
                        DirectionName = item.DirectionName,
                        Total = item.total,
                        SumGrossWeight = item.sumGrossWeight,
                        SumChargeableWeight = item.sumChargeableWeight,
                        TotalProfitInLocalCurrency = item.totalProfitInLocalCurrency,
                        TotalProfitInProfitCurrency = item.totalProfitInProfitCurrency,
                        ReceivablesInLocalCurrency = item.ReceivablesInLocalCurrency,
                        ReceivablesInProfitCurrency = item.ReceivablesInProfitCurrency,
                        DirectionTransportModeNames = item.DirectionName + "/" + item.TransportModeName,
                    };

                    dataList.Add(newItem);
                }
            }

            foreach (DirectionTransportModeDataClass item in dataList)
            {
                switch (measurment)
                {
                    case 0:
                        {
                            item.GeneralTotal = item.Total;
                            break;
                        }
                    case 1:
                        {
                            item.GeneralTotal = item.SumChargeableWeight;
                            break;
                        }
                    case 2:
                        {
                            item.GeneralTotal = item.SumGrossWeight;
                            break;
                        }
                    case 3:
                        {
                            item.GeneralTotal = item.TotalProfitInLocalCurrency;
                            break;
                        }
                    case 4:
                        {
                            item.GeneralTotal = item.TotalProfitInProfitCurrency;
                            break;
                        }
                    case 5:
                        {
                            item.GeneralTotal = item.ReceivablesInLocalCurrency;
                            break;
                        }
                    case 6:
                        {
                            item.GeneralTotal = item.ReceivablesInProfitCurrency;
                            break;
                        }
                }

                item.GeneralTotal = Math.Round(item.GeneralTotal.Value, 2);
            }

            return dataList;
        }

        private List<ShipmentQuantityDataClass> GetShipmentsData(int tenant, int lastMonths, int lastDays, int measurment, string directionId, string transmodeId)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            List<DashBoardClass> list =  shipmentQuery.GetShipmentsByMonthDashBoard(null,lastMonths, lastDays, tenant, null);
            List<DashBoardClass> list1 = new List<DashBoardClass>();            

            if (!string.IsNullOrEmpty(directionId))
            {
                list = list.Where(d => d.directionID == directionId).ToList();
            }

            if (!string.IsNullOrEmpty(transmodeId))
            {
                list = list.Where(d => d.transportModeID == transmodeId).ToList();
            }

            #region show type filter

            int monthFilterIndex = 0;
            if(lastDays == -7)
            {
                monthFilterIndex = 0;
            }
            else if (lastDays == -30)
            {
                monthFilterIndex = 1;
            }
            else if (lastDays == -90)
            {
                monthFilterIndex = 2;
            }
            else if (lastDays == -365)
            {
                monthFilterIndex = 3;
            }

            #region by weeks

            if (monthFilterIndex == 1 || monthFilterIndex == 2)
            {
                switch (measurment)
                {
                    case 0:
                        {
                            list = FillEmptyDates(monthFilterIndex, list, tenant);

                            list1 = (from l in list
                                          group l by new { l.DateRange, l.StartOfTheWeek.Date } into m
                                          orderby m.Key.Date
                                          select new DashBoardClass()
                                          {
                                              XField = m.Key.DateRange,
                                              Date = m.Key.Date,
                                              GeneralTotal = m.Sum(u => u.total)
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    case 1:
                        {
                            list = FillEmptyDates(monthFilterIndex, list, tenant);
                            list1 = (from l in list
                                          group l by new { l.DateRange, l.StartOfTheWeek.Date } into m
                                          orderby m.Key.Date
                                          select new DashBoardClass()
                                          {

                                              XField = m.Key.DateRange,
                                              Date = m.Key.Date,
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.sumChargeableWeight))
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    case 2:
                        {
                            list = FillEmptyDates(monthFilterIndex, list, tenant);
                            list1 = (from l in list
                                          group l by new { l.DateRange, l.StartOfTheWeek.Date } into m
                                          orderby m.Key.Date
                                          select new DashBoardClass()
                                          {

                                              XField = m.Key.DateRange,
                                              Date = m.Key.Date,
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.sumGrossWeight))
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    case 3:
                        {
                            list = FillEmptyDates(monthFilterIndex, list, tenant);
                            list1 = (from l in list
                                          group l by new { l.DateRange, l.StartOfTheWeek.Date } into m
                                          orderby m.Key.Date
                                          select new DashBoardClass()
                                          {

                                              XField = m.Key.DateRange,
                                              Date = m.Key.Date,
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.totalProfitInLocalCurrency))
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    case 4:
                        {
                            list = FillEmptyDates(monthFilterIndex, list, tenant);
                            list1 = (from l in list
                                          group l by new { l.DateRange, l.StartOfTheWeek.Date } into m
                                          orderby m.Key.Date
                                          select new DashBoardClass()
                                          {

                                              XField = m.Key.DateRange,
                                              Date = m.Key.Date,
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.totalProfitInProfitCurrency))
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    case 5:
                        {
                            list = FillEmptyDates(monthFilterIndex, list, tenant);
                            list1 = (from l in list
                                          group l by new { l.DateRange, l.StartOfTheWeek.Date } into m
                                          orderby m.Key.Date
                                          select new DashBoardClass()
                                          {

                                              XField = m.Key.DateRange,
                                              Date = m.Key.Date,
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.ReceivablesInLocalCurrency))
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    case 6:
                        {
                            list = FillEmptyDates(monthFilterIndex, list, tenant);
                            list1 = (from l in list
                                          group l by new { l.DateRange, l.StartOfTheWeek.Date } into m
                                          orderby m.Key.Date
                                          select new DashBoardClass()
                                          {

                                              XField = m.Key.DateRange,
                                              Date = m.Key.Date,
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.ReceivablesInProfitCurrency))
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    default: break;
                }
            }

            #endregion

            #region by months

            if (monthFilterIndex == 3)
            {
                switch (measurment)
                {
                    case 0:
                        {
                            list1 = (from l in list
                                          group l by new { l.month, l.year } into m
                                          orderby m.Key.year, m.Key.month
                                          select new DashBoardClass()
                                          {

                                              month = m.Key.month,
                                              year = m.Key.year,
                                              XField = m.Key.month.ToString() + "-" + m.Key.year.ToString(),
                                              GeneralTotal = m.Sum(u => u.total)
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    case 1:
                        {
                            list1 = (from l in list
                                          group l by new { l.month, l.year } into m
                                          orderby m.Key.year, m.Key.month
                                          select new DashBoardClass()
                                          {
                                              month = m.Key.month,
                                              year = m.Key.year,
                                              XField = m.Key.month.ToString() + "-" + m.Key.year.ToString(),
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.sumChargeableWeight))
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    case 2:
                        {
                            list1 = (from l in list
                                          group l by new { l.month, l.year } into m
                                          orderby m.Key.year, m.Key.month
                                          select new DashBoardClass()
                                          {
                                              month = m.Key.month,
                                              year = m.Key.year,
                                              XField = m.Key.month.ToString() + "-" + m.Key.year.ToString(),
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.sumGrossWeight))
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    case 3:
                        {

                            list1 = (from l in list
                                          group l by new { l.month, l.year } into m
                                          orderby m.Key.year, m.Key.month
                                          select new DashBoardClass()
                                          {
                                              month = m.Key.month,
                                              year = m.Key.year,
                                              XField = m.Key.month.ToString() + "-" + m.Key.year.ToString(),
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.totalProfitInLocalCurrency))
                                          }).ToList<DashBoardClass>();

                            break;
                        }

                    case 4:
                        {
                            list1 = (from l in list
                                          group l by new { l.month, l.year } into m
                                          orderby m.Key.year, m.Key.month
                                          select new DashBoardClass()
                                          {
                                              month = m.Key.month,
                                              year = m.Key.year,
                                              XField = m.Key.month.ToString() + "-" + m.Key.year.ToString(),
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.totalProfitInProfitCurrency))
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    case 5:
                        {
                            list1 = (from l in list
                                          group l by new { l.month, l.year } into m
                                          orderby m.Key.year, m.Key.month
                                          select new DashBoardClass()
                                          {
                                              month = m.Key.month,
                                              year = m.Key.year,
                                              XField = m.Key.month.ToString() + "-" + m.Key.year.ToString(),
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.ReceivablesInLocalCurrency))
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    case 6:
                        {
                            list1 = (from l in list
                                          group l by new { l.month, l.year } into m
                                          orderby m.Key.year, m.Key.month
                                          select new DashBoardClass()
                                          {
                                              month = m.Key.month,
                                              year = m.Key.year,
                                              XField = m.Key.month.ToString() + "-" + m.Key.year.ToString(),
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.ReceivablesInProfitCurrency))
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    default: break;
                }
            }

            #endregion

            #region by days

            if (monthFilterIndex == 0)
            {
                switch (measurment)
                {
                    case 0:
                        {
                            list1 = (from l in list
                                          group l by new { l.month, l.year, l.day } into m
                                          orderby m.Key.year, m.Key.month, m.Key.day
                                          select new DashBoardClass()
                                          {
                                              day = m.Key.day,
                                              month = m.Key.month,
                                              year = m.Key.year,
                                              XField = m.Key.day.ToString() + "-" + m.Key.month.ToString(),
                                              GeneralTotal = m.Sum(u => u.total),
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    case 1:
                        {
                            list1 = (from l in list
                                          group l by new { l.month, l.year, l.day } into m
                                          orderby m.Key.year, m.Key.month, m.Key.day
                                          select new DashBoardClass()
                                          {
                                              day = m.Key.day,
                                              month = m.Key.month,
                                              year = m.Key.year,
                                              XField = m.Key.day.ToString() + "-" + m.Key.month.ToString(),
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.sumChargeableWeight))
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    case 2:
                        {
                            list1 = (from l in list
                                          group l by new { l.month, l.year, l.day } into m
                                          orderby m.Key.year, m.Key.month, m.Key.day
                                          select new DashBoardClass()
                                          {
                                              day = m.Key.day,
                                              month = m.Key.month,
                                              year = m.Key.year,
                                              XField = m.Key.day.ToString() + "-" + m.Key.month.ToString(),
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.sumGrossWeight))
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    case 3:
                        {

                            list1 = (from l in list
                                          group l by new { l.month, l.year, l.day } into m
                                          orderby m.Key.year, m.Key.month, m.Key.day
                                          select new DashBoardClass()
                                          {
                                              day = m.Key.day,
                                              month = m.Key.month,
                                              year = m.Key.year,
                                              XField = m.Key.day.ToString() + "-" + m.Key.month.ToString(),
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.totalProfitInLocalCurrency))
                                          }).ToList<DashBoardClass>();

                            break;
                        }

                    case 4:
                        {
                            list1 = (from l in list
                                          group l by new { l.month, l.year, l.day } into m
                                          orderby m.Key.year, m.Key.month, m.Key.day
                                          select new DashBoardClass()
                                          {
                                              day = m.Key.day,
                                              month = m.Key.month,
                                              year = m.Key.year,
                                              XField = m.Key.day.ToString() + "-" + m.Key.month.ToString(),
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.totalProfitInProfitCurrency))
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    case 5:
                        {
                            list1 = (from l in list
                                          group l by new { l.month, l.year, l.day } into m
                                          orderby m.Key.year, m.Key.month, m.Key.day
                                          select new DashBoardClass()
                                          {
                                              day = m.Key.day,
                                              month = m.Key.month,
                                              year = m.Key.year,
                                              XField = m.Key.day.ToString() + "-" + m.Key.month.ToString(),
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.ReceivablesInLocalCurrency))
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    case 6:
                        {
                            list1 = (from l in list
                                          group l by new { l.month, l.year, l.day } into m
                                          orderby m.Key.year, m.Key.month, m.Key.day
                                          select new DashBoardClass()
                                          {
                                              day = m.Key.day,
                                              month = m.Key.month,
                                              year = m.Key.year,
                                              XField = m.Key.day.ToString() + "-" + m.Key.month.ToString(),
                                              GeneralTotal = Convert.ToDouble(m.Sum(u => u.ReceivablesInProfitCurrency))
                                          }).ToList<DashBoardClass>();
                            break;
                        }

                    default: break;
                }
            }

            #endregion

            #endregion

            #region
            switch (lastDays)
            {
                case -30:
                case -90:
                    {
                        List<DashBoardClass> newList = new List<DashBoardClass>();
                        newList = (from r in list1
                                   orderby r.Date
                                   select r).ToList<DashBoardClass>();

                        list1.Clear();
                        list1 = newList;
                        break;
                    }

                case -7:
                    {
                        if (list1.Count < 7)
                        {
                            list1 = this.GetFilledListByMonths(list1, -6, tenant);
                        }

                        List<DashBoardClass> newList = new List<DashBoardClass>();
                        newList = (from r in list1
                                   orderby r.year, r.month, r.day
                                   select r).ToList<DashBoardClass>();

                        list1.Clear();
                        list1 = newList;
                        break;

                    }

                case -365:
                    {
                        if (list1.Count < 12)
                        {
                            list1 = this.GetFilledListByMonths(list1, -11, tenant);
                        }

                        List<DashBoardClass> newList = new List<DashBoardClass>();
                        newList = (from r in list1
                                   orderby r.year, r.month
                                   select r).ToList<DashBoardClass>();

                        list1.Clear();
                        list1 = newList;
                        break;
                    }
            }
            #endregion                

            List<ShipmentQuantityDataClass> result = new List<ShipmentQuantityDataClass>();

            if (list1 != null)
            {
                int i = 0;
                foreach (DashBoardClass item in list1)
                {
                    ShipmentQuantityDataClass newItem = new ShipmentQuantityDataClass()
                    {
                        Id = i++,
                        Day = item.day,
                        Month = item.month,
                        Year = item.year,
                        DirectionId = item.directionID,
                        DirectionName = item.DirectionName,
                        TransportModeId = item.transportModeID,
                        TransportModeName = item.TransportModeName,
                        Total = item.total,
                        SumGrossWeight = item.sumGrossWeight,
                        SumChargeableWeight = item.sumChargeableWeight,
                        TotalProfitInLocalCurrency = item.totalProfitInLocalCurrency,
                        TotalProfitInProfitCurrency = item.totalProfitInProfitCurrency,
                        ReceivablesInLocalCurrency = item.ReceivablesInLocalCurrency,
                        ReceivablesInProfitCurrency = item.ReceivablesInProfitCurrency,
                        XField = item.XField,
                        GeneralTotal = item.GeneralTotal,
                    };

                    result.Add(newItem);
                }
            }

            return result;
        }

        public List<DashBoardClass> FillEmptyDates(int monthFilterIndex, List<DashBoardClass> dataList, int tenant)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime startDate = todayDate.Date.AddMonths(-1);

            if (monthFilterIndex == 2)
            {
                startDate = todayDate.Date.AddMonths(-3);
            }

            while (startDate <= todayDate.Date)
            {
                var monthShipments = from s in dataList
                                     where s.month == startDate.Month && s.year == startDate.Year && s.day == startDate.Day
                                     select s;

                if (monthShipments.Count() == 0)
                {
                    DashBoardClass newEntry = new DashBoardClass()
                    {
                        day = startDate.Day,
                        month = startDate.Month,
                        year = startDate.Year,
                        total = 0,
                        sumChargeableWeight = 0,
                        sumGrossWeight = 0,
                    };

                    newEntry.Date = new DateTime(newEntry.year, newEntry.month, newEntry.day);

                    int day = Convert.ToInt32(newEntry.Date.DayOfWeek);
                    DateTime startOfWeek = newEntry.Date.AddDays((-1 * day));
                    DateTime endOfWeek = newEntry.Date.AddDays((6 - day));

                    newEntry.StartOfTheWeek = startOfWeek;
                    newEntry.EndOfTheWeek = endOfWeek;
                    newEntry.DateRange = startOfWeek.Day + "/" + startOfWeek.Month;

                    dataList.Add(newEntry);
                }

                startDate = startDate.AddDays(1);
            }

            return dataList;
        }

        private List<DashBoardClass> GetFilledListByMonths(List<DashBoardClass> list, int days, int tenant)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime startDate = todayDate.Date.AddDays(days);

            if (days == -1)
            {
                while (startDate <= todayDate.Date)
                {

                    var monthShipments = from s in list
                                         where s.month == startDate.Month && s.year == startDate.Year && s.day == startDate.Day
                                         select s;
                    if (monthShipments.Count() == 0)
                    {
                        DashBoardClass newEntry = new DashBoardClass()
                        {
                            day = startDate.Day,
                            year = startDate.Year,
                            month = startDate.Month,
                            XField = startDate.Day.ToString() + "-" + startDate.Month.ToString() + "-" + startDate.Year,
                            YField = 0
                        };

                        list.Add(newEntry);
                    }

                    startDate = startDate.AddDays(1);
                }
            }

            else if (days == -11)
            {
                startDate = todayDate.Date.AddMonths(days);
                while (startDate <= todayDate.Date)
                {
                    var monthShipments = from s in list
                                         where s.month == startDate.Month && s.year == startDate.Year
                                         select s;
                    if (monthShipments.Count() == 0)
                    {
                        DashBoardClass newEntry = new DashBoardClass()
                        {
                            day = startDate.Day,
                            year = startDate.Year,
                            month = startDate.Month,
                            XField = startDate.Month.ToString() + "-" + startDate.Year,
                            YField = 0
                        };

                        list.Add(newEntry);
                    }

                    startDate = startDate.AddMonths(1);
                }
            }

            else
            {

                while (startDate <= todayDate.Date)
                {
                    var monthShipments = from s in list
                                         where s.month == startDate.Month && s.year == startDate.Year && s.day == startDate.Day
                                         select s;

                    if (monthShipments.Count() == 0)
                    {
                        DashBoardClass newEntry = new DashBoardClass()
                        {
                            day = startDate.Day,
                            year = startDate.Year,
                            month = startDate.Month,
                            XField = startDate.Day.ToString() + "-" + startDate.Month,
                            YField = 0
                        };

                        list.Add(newEntry);
                    }

                    startDate = startDate.AddDays(1);
                }
            }
            return list;
        }
        #endregion
    }
}
