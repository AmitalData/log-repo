using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports
{
    public class ShipmentProfitVSQuoteEstimateManager
    {
        private int tenant;
        private string CustomerId = null;
        private string SalesmanUserId = null;
        private bool IsLocalCurrency = false;
        private string ShipmentsTypeCode = null;
        private DateTime? FromDate = null;
        private DateTime? ToDate = null;
        private ICommonDataContext myCommonContext;
        private IShipmentsContext myShipmentsContext;
        public ShipmentProfitVSQuoteEstimateManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;

            myCommonContext = CommonDataContext.GetContext(tenant);
            myShipmentsContext = ShipmentsContext.GetContext(tenant);

            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations myQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);

            QueryFilterItem filterItem_CustomerId = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();
            QueryFilterItem filterItem_SalesmanUserId = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "SalesmanUserId").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_IsLocalCurrency = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IsLocalCurrency").FirstOrDefault();
            QueryFilterItem filterItem_ShipmentsTypeCode = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ShipmentsTypeCode").FirstOrDefault();

            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    CustomerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }

            if (filterItem_SalesmanUserId != null)
            {
                if (filterItem_SalesmanUserId.FieldValue != null)
                {
                    SalesmanUserId = filterItem_SalesmanUserId.FieldValue.ToString();
                }
            }

            if (filterItem_IsLocalCurrency != null)
            {
                if (filterItem_IsLocalCurrency.FieldValue != null)
                {
                    IsLocalCurrency = (bool)filterItem_IsLocalCurrency.FieldValue;
                }
            }

            if (filterItem_ShipmentsTypeCode != null)
            {
                if (filterItem_ShipmentsTypeCode.FieldValue != null)
                {
                    ShipmentsTypeCode = filterItem_ShipmentsTypeCode.FieldValue.ToString();
                }
            }

            if (filterItem_FromDate != null)
            {
                if (filterItem_FromDate.FieldValue != null)
                {
                    DateTime date;
                    bool isValid = DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out date);
                    if (isValid)
                    {
                        this.FromDate = date;
                    }
                }
            }

            if (filterItem_ToDate != null)
            {
                if (filterItem_ToDate.FieldValue != null)
                {
                    DateTime date;
                    bool isValid = DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out date);
                    if (isValid)
                    {
                        this.ToDate = date;
                    }
                }
            }
        }

        public byte[] GetData()
        {
            ShipmentProfitVSQuoteEstimateDataProvider myDataProvider = this.LoadDataProvider();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ShipmentProfitVSQuoteEstimateDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, myDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);

            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private ShipmentProfitVSQuoteEstimateDataProvider LoadDataProvider()
        {
            ShipmentProfitVSQuoteEstimateDataProvider myDataProvider = new ShipmentProfitVSQuoteEstimateDataProvider();
            myDataProvider.Id = tenant;
            myDataProvider.Customer = "All";
            myDataProvider.Salesman = "All";
            myDataProvider.From = this.GetDateString(this.FromDate);
            myDataProvider.To = this.GetDateString(this.ToDate);
            myDataProvider.Shipments = new List<ProfitVSEstimateShipmentItem>();

            if (!string.IsNullOrEmpty(this.CustomerId))
            {
                Card myCard = (from d in myCommonContext.Cards
                               where d.Id == this.CustomerId && d.Tenant == this.tenant
                               select d).FirstOrDefault();

                if (myCard != null)
                {
                    myDataProvider.Customer = myCard.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(this.SalesmanUserId))
            {
                Contact myContact = (from d in myCommonContext.Contacts
                                     where d.Id == this.SalesmanUserId && d.Tenant == this.tenant
                                     select d).FirstOrDefault();

                if(myContact != null)
                {
                    myDataProvider.Salesman = myContact.EnglishName;
                }
            }

            IQueryable<Shipment> iQueryable = (from d in myShipmentsContext.Shipments
                                               where d.Tenant == tenant
                                               && d.CustomerId != null
                                               && (d.ShipmentLevelCode == "D" || d.ShipmentLevelCode == "H")
                                               select d);

            if (this.CustomerId != null)
            {
                iQueryable = iQueryable.Where(d => d.CustomerId == this.CustomerId);
            }

            if (this.SalesmanUserId != null)
            {
                iQueryable = iQueryable.Where(d => d.SalesmanUserId == this.SalesmanUserId);
            }

            if (this.FromDate != null)
            {
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= System.Data.Entity.DbFunctions.TruncateTime(this.FromDate));
            }

            if (this.ToDate != null)
            {
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= System.Data.Entity.DbFunctions.TruncateTime(this.ToDate));
            }

            List<Shipment> allShipments = new List<Shipment>();
            if (this.ShipmentsTypeCode == "C")
            {
                allShipments = iQueryable.Where(d => d.QuoteId != null).ToList();
            }

            else
            {
                allShipments = iQueryable.ToList();
            }

            List<Quote> allQuotes = new List<Quote>();
            IQuotesContext myQuotesContext = QuotesContext.GetContext(tenant);
            List<string> allQuotesIds = allShipments.Select(s => s.QuoteId).ToList();
            allQuotes = (from d in myQuotesContext.Quotes where d.Tenant == tenant && allQuotesIds.Contains(d.Id) select d).ToList();

            List<ShipmentType> allShipmentTypes = (from d in myShipmentsContext.ShipmentTypes select d).ToList();
            List<ShipmentLevel> allShipmentLevels = (from d in myShipmentsContext.ShipmentLevels select d).ToList();

            List<string> allCustomersIds = allShipments.Where(d => d.CustomerId != null).Select(s => s.CustomerId).ToList();
            List<Card> allCustomers = (from d in myCommonContext.Cards where d.Tenant == tenant && allCustomersIds.Contains(d.Id) select d).ToList();

            List<string> allContactsIds = allShipments.Where(d => d.SalesmanUserId != null).Select(s => s.SalesmanUserId).ToList();
            List<Contact> allContacts = (from d in myCommonContext.Contacts where d.Tenant == tenant && allContactsIds.Contains(d.Id) select d).ToList();

            List<string> allShipmentsIds = allShipments.Select(s => s.Id).ToList();
            List<ShipmentMasterData> allMasterDatas = (from d in myShipmentsContext.ShipmentMasterDatas where d.Tenant == tenant && allShipmentsIds.Contains(d.Id) select d).ToList();
            List<string> allFromAddressIds = allMasterDatas.Where(d => d.MainCarriageFromAddressId != null).Select(s => s.MainCarriageFromAddressId).ToList();
            List<string> allToAddressIds = allMasterDatas.Where(d => d.MainCarriageToAddressId != null).Select(s => s.MainCarriageToAddressId).ToList();
            List<string> allAddressIds = allFromAddressIds;
            foreach (string id in allToAddressIds)
            {
                if (!allAddressIds.Contains(id))
                {
                    allAddressIds.Add(id);
                }
            }

            List<Address> allAddresses = (from d in myCommonContext.Addresses where d.Tenant == tenant && allAddressIds.Contains(d.Id) select d).ToList();
            List<PortPM> allPorts = new List<PortPM>();

            foreach (Shipment item in allShipments)
            {
                ProfitVSEstimateShipmentItem myRecord = new ProfitVSEstimateShipmentItem();
                myRecord.CustomerId = item.CustomerId;
                myRecord.SalesmanUserId = item.SalesmanUserId;
                myRecord.ShipmentNumber = item.ShipmentNumber;

                Card myCard = allCustomers.Where(d => d.Id == item.CustomerId).FirstOrDefault();
                if (myCard != null)
                {
                    myRecord.CustomerName = myCard.EnglishName;
                }

                Contact myContact = allContacts.Where(d => d.Id == item.SalesmanUserId).FirstOrDefault();
                if (myContact != null)
                {
                    myRecord.SalesmanUserName = myContact.EnglishName;
                }

                string myShipmentTypeString = "";
                ShipmentType myShipmentType = allShipmentTypes.Where(d => d.Id == item.ShipmentTypeId).FirstOrDefault();
                ShipmentLevel myShipmentLevel = allShipmentLevels.Where(d => d.Code == item.ShipmentLevelCode).FirstOrDefault();
                if (myShipmentType != null)
                {
                    myShipmentTypeString = myShipmentType.Name;
                }

                if (myShipmentLevel != null)
                {
                    myShipmentTypeString = string.IsNullOrEmpty(myShipmentTypeString) ? myShipmentLevel.Name : myShipmentTypeString + " " + myShipmentLevel.Name;
                }

                myRecord.ShipmentType = myShipmentTypeString;

                // Routing
                myRecord.Routing = this.GetRoutingField(item, allMasterDatas, allAddresses, allPorts);

                // ActualProfit
                if (this.IsLocalCurrency)
                {
                    myRecord.ActualProfit = item.ProfitInLocalCurrency;
                }

                else
                {
                    myRecord.ActualProfit = item.ProfitInProfitCurrency == null ? 0 : item.ProfitInProfitCurrency.Value;
                }

                // Selected Currency
                //if (this.ShipmentsTypeCode == "C")
                if (item.QuoteId != null)
                {
                    Quote myQuote = allQuotes.Where(d => d.Id == item.QuoteId).FirstOrDefault();
                    if (myQuote != null)
                    {
                        myRecord.CreateDate = item.CreateDateTime;
                        myRecord.QuoteNumber = myQuote.QuoteNumber;

                        if (this.IsLocalCurrency)
                        {
                            myRecord.EstimatedProfit = item.EstimateProfitInLocalCurrency == null ? 0 : item.EstimateProfitInLocalCurrency.Value;
                        }

                        else
                        {
                            myRecord.EstimatedProfit = item.EstimateProfitInProfitCurrency == null ? 0 : item.EstimateProfitInProfitCurrency.Value;
                        }

                        myRecord.Margin = myRecord.ActualProfit - myRecord.EstimatedProfit;
                    }
                }

                myDataProvider.Shipments.Add(myRecord);
            }

            return myDataProvider;
        }
        private string GetRoutingField(Shipment item, List<ShipmentMasterData> allMasterDatas, List<Address> allAddresses, List<PortPM> allPorts)
        {        
            string myResult = "";

            ShipmentMasterData myMasterData = null;

            if (item.MasterShipmentDataId != null)
            {
                myMasterData = allMasterDatas.Where(d => d.Id == item.MasterShipmentDataId).FirstOrDefault();
            }

            if (item.DirectionId == "D" && item.TransportModeId == "I")
            {
                if (myMasterData != null)
                {
                    if (myMasterData.MainCarriageFromAddressId != null)
                    {
                        Address address = allAddresses.Where(d => d.Id == myMasterData.MainCarriageFromAddressId).FirstOrDefault();
                        myResult = address.City;
                    }

                    if (myMasterData.MainCarriageToAddressId != null)
                    {
                        Address address = allAddresses.Where(d => d.Id == myMasterData.MainCarriageToAddressId).FirstOrDefault();
                        myResult = string.IsNullOrEmpty(myResult) ? address.City : myResult + " > " + address.City;
                    }
                }
            }

            else
            {
                string fromPortId = null;
                string toPortId = null;
                if(item.ShipmentLevelCode == "H" && item.MasterShipmentDataId == null)
                {
                    fromPortId = item.FromPortId;
                    toPortId = item.ToPortId;
                }

                else if (myMasterData != null)
                {
                    fromPortId = myMasterData.MainCarriageFromPortId;
                    toPortId = myMasterData.MainCarriageToPortId;
                }

                if (fromPortId != null)
                {
                    PortPM myPort = allPorts.Where(d => d.Id == fromPortId).FirstOrDefault();
                    if (myPort == null)
                    {
                        myPort = PortQuery.GetSinglePort(tenant, fromPortId, true);
                        if(myPort != null)
                        {
                            allPorts.Add(myPort);
                        }
                    }

                    if(myPort != null)
                    {
                        myResult = myPort.Code;
                    }
                }

                if (toPortId != null)
                {
                    PortPM myPort = allPorts.Where(d => d.Id == toPortId).FirstOrDefault();
                    if (myPort == null)
                    {
                        myPort = PortQuery.GetSinglePort(tenant, toPortId, true);
                        if (myPort != null)
                        {
                            allPorts.Add(myPort);
                        }
                    }

                    if (myPort != null)
                    {
                        myResult = string.IsNullOrEmpty(myResult) ? myPort.Code : myResult + " > " + myPort.Code;                    
                    }
                }
            }

            return myResult;
        }
        private string GetDateString(DateTime? date)
        {
            string myResult = null;

            if (date != null)
            {
                //myResult = date.Value.Day.ToString() + @"/" + date.Value.Month.ToString() + @"/" + date.Value.Year.ToString();
                myResult = String.Format("{0:dd/MM/yyyy}", date);
            }

            return myResult;
        }

    }
}