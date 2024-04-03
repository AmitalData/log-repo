using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using WebFreight.Web.Services;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports
{
    public class StatisticsByCustomerManager
    {
        private int tenant;

        private DateTime? FromDate = null;
        private DateTime? ToDate = null;
        private string Currency = null;
        private string CustomerId = null;
        private bool IncludeOperationalClosed = false;
        private bool IsByCreateDate = true;
        private string Direction = null;
        private string TransportMode = null;
        private string ShipmentLevel = null;

        public StatisticsByCustomerManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;

            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations myQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);

            QueryFilterItem filterItem_IsByCreateDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IsByCreateDate").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_CustomerId = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();
            QueryFilterItem filterItem_IncludeOperationalClosed = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeOperationalClosed").FirstOrDefault();
            QueryFilterItem filterItem_Currency = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "Currency").FirstOrDefault();
            QueryFilterItem filterItem_Direction = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "Direction").FirstOrDefault();
            QueryFilterItem filterItem_TransportMode = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "TransportMode").FirstOrDefault();
            QueryFilterItem filterItem_ShipmentLevel = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ShipmentLevel").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);
            DateTime fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            DateTime toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));
            
            if (filterItem_IsByCreateDate != null)
            {
                if (filterItem_IsByCreateDate.FieldValue != null)
                {
                    IsByCreateDate = (bool)filterItem_IsByCreateDate.FieldValue;
                }
            }

            if (filterItem_FromDate != null)
            {
                DateTime date;
                bool isValid = DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out date);
                if (isValid)
                {
                    this.FromDate = date;
                }
            }

            if (filterItem_ToDate != null)
            {
                DateTime date;
                bool isValid = DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out date);
                if (isValid)
                {
                    this.ToDate = date;
                }
            }

            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    CustomerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }

            if (filterItem_Currency != null)
            {
                if (filterItem_Currency.FieldValue != null)
                {
                    Currency = filterItem_Currency.FieldValue.ToString();
                }
            }

            if (filterItem_IncludeOperationalClosed != null)
            {
                if (filterItem_IncludeOperationalClosed.FieldValue != null)
                {
                    IncludeOperationalClosed = (bool)filterItem_IncludeOperationalClosed.FieldValue;
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
        }

        public byte[] GetData()
        {
            StatisticsByClientDataProvider myDataProvider = new StatisticsByClientDataProvider();
            myDataProvider = this.LoadDataProvider();
            return new ReportMemoryStreamService().Convert(myDataProvider, typeof(StatisticsByClientDataProvider), tenant);
        }

        private StatisticsByClientDataProvider LoadDataProvider()
        {
            StatisticsByClientDataProvider myDataProvider = new StatisticsByClientDataProvider();
            myDataProvider.StatisticsList_NoGroup = new List<StatisticsByClientReport>();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            CardRepository cardRepository = new CardRepository(tenant);
            ContactRepository contactRepository = new ContactRepository(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant);

            #region filter data
            shipments = shipments.Where(d => !d.IsCancelled);

            if (IsByCreateDate)
            {
                if (FromDate != null)
                {
                    shipments = shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                }

                if (ToDate != null)
                {
                    shipments = shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= System.Data.Entity.DbFunctions.TruncateTime(ToDate));
                }
            }

            else
            {
                if (FromDate != null)
                {
                    shipments = shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OperationalDate) >= System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                }

                if (ToDate != null)
                {
                    shipments = shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OperationalDate) <= System.Data.Entity.DbFunctions.TruncateTime(ToDate));
                }
            }

            if (!string.IsNullOrEmpty(CustomerId))
            {
                shipments = shipments.Where(d => (d.ShipmentLevelCode == "C" && d.AgentId == CustomerId) || (d.ShipmentLevelCode != "C" && d.CustomerId == CustomerId));

                Card customer = CardRepository.GetSingleCard(CustomerId, tenant, true);
                if (customer != null)
                {
                    myDataProvider.Customer = customer.EnglishName;
                }
            }
            else
            {
                myDataProvider.Customer = "All";
            }

            if (!IncludeOperationalClosed)
            {
                shipments = shipments.Where(d => !d.IsOperationalClosed);
            }

            if (!string.IsNullOrEmpty(Direction))
            {
                shipments = shipments.Where(d => d.DirectionId == Direction);
            }

            if (!string.IsNullOrEmpty(TransportMode))
            {
                shipments = shipments.Where(d => d.TransportModeId == TransportMode);
            }

            if (!string.IsNullOrEmpty(ShipmentLevel))
            {
                shipments = shipments.Where(d => d.ShipmentLevelCode == ShipmentLevel);
            }

            string[] currencyarray = Currency.Split(',');
            myDataProvider.Currency = currencyarray[0];

            #endregion

            #region General data
            myDataProvider.Name = @"Statistics By Customers";
            myDataProvider.FromPeriod = FromDate;
            myDataProvider.ToPeriod = ToDate;

            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);

            if (currentTenant != null)
            {
                myDataProvider.TenantName = currentTenant.Company;
                myDataProvider.Signature = currentTenant.Signature;
                myDataProvider.Logo = DataProviders.General.GetLogo(currentTenant.Id);

                AddressQuery addressQuery = new AddressQuery(tenant);
                AddressPM address = addressQuery.GetSingleAddressPM(currentTenant.AddressId, currentTenant.Id, false);

                if (address != null)
                {
                    myDataProvider.Address1 = address.Address1;
                    myDataProvider.Address2 = address.Address2;
                    myDataProvider.City = address.City;
                    myDataProvider.Country = address.CountryName;
                    myDataProvider.TenantFax = address.FaxNumber;
                    myDataProvider.TenantPhone = address.PhoneNumber;
                    myDataProvider.State = address.StateEnglishName;
                    myDataProvider.ZipCode = address.ZipCode;
                }
            }
            #endregion

            #region fill report data
            string currencytype = currencyarray[1];

            List<StatisticsByClientReport> tempList = new List<StatisticsByClientReport>();

            if (shipments.Count() > 0)
            {
                foreach (ShipmentDataView shipment in shipments)
                {
                    StatisticsByClientReport record = new StatisticsByClientReport();
                    if (shipment.ShipmentLevelCode == "C")
                    {
                        record.CustomerId = shipment.AgentId;
                        record.CustomerName = shipment.AgentName;
                    }

                    else
                    {
                        record.CustomerId = shipment.CustomerId;
                        record.CustomerName = shipment.CustomerName;
                    }

                    Card myCustomer = cardRepository.GetSingleCard(record.CustomerId, tenant);
                    if (myCustomer != null)
                    {
                        if(!string.IsNullOrEmpty(myCustomer.SalesmanUserId))
                        {
                            Contact salesman = contactRepository.GetSingleContact(myCustomer.SalesmanUserId, tenant);
                            record.CustomerSalesman = salesman?.EnglishName;
                        }

                        if (myCustomer.Customer != null)
                        {
                            customFieldResolver.SetDataProviderCustomFieldsValues("Customer", tenant, myCustomer.Customer, record);
                        }
                    }

                    record.ChargeableWeightInKg = shipment.ChargeableWeightInKG;
                    record.TransportMode = shipment.TransportModeName;
                    record.Direction = shipment.DirectionName;
                    record.TransmodeDirection = shipment.DirectionName + shipment.TransportModeName;
                    record.GrossWeight = shipment.GrossWeight;
                    record.Volume = shipment.Volume;
                    record.TEU = shipment.TEU;
                    
                    if (currencytype == "profit")
                    {
                        record.Payables = shipment.OpenPayablesInProfitCurrency + shipment.AccountedPayablesInProfitCurrency;
                        record.Receivables = shipment.OpenReceivablesInProfitCurrency + shipment.AccountedReceivablesInProfitCurrency;
                        record.Profit = shipment.ProfitInProfitCurrency;
                    }

                    else if (currencytype == "local")
                    {
                        record.Payables = shipment.OpenPayablesInLocalCurrency + shipment.AccountedPayablesInLocalCurrency;
                        record.Receivables = shipment.OpenReceivablesInLocalCurrency + shipment.AccountedReceivablesInLocalCurrency;
                        record.Profit = shipment.ProfitInLocalCurrency;
                    }

                    tempList.Add(record);
                }
                myDataProvider.StatisticsList_NoGroup = tempList;
            }
            #endregion

            #region Group
            tempList = tempList.OrderBy(or => or.CustomerName).ToList();

            List<StatisticsByClientGroup> finalResults = (from p in tempList
                                                          group p by new { p.CustomerId, p.CustomerName } into g
                                                          select new StatisticsByClientGroup()
                                                          {
                                                              CustomerId = g.Key.CustomerId,
                                                              CustomerName = g.Key.CustomerName,
                                                              StatisticsRecordList = g.ToList(),
                                                          }).ToList();

            myDataProvider.StatisticsGroupList = finalResults.OrderBy(d => d.CustomerName).ToList();

            foreach (StatisticsByClientGroup item in myDataProvider.StatisticsGroupList)
            {
                List<StatisticsByClientReport> myList = (from a in item.StatisticsRecordList
                                                         group a by new
                                                         {
                                                             a.TransportMode,
                                                             a.Direction,
                                                         } into gr
                                                         orderby gr.Key.TransportMode
                                                         select new StatisticsByClientReport()
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

                StatisticsByClientReport totalRecord = new StatisticsByClientReport();
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

            return myDataProvider;
        }
    }
}