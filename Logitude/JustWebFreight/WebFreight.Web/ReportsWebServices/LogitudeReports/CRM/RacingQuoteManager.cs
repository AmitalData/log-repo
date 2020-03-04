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
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.Helpers;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports
{
    public class RacingQuoteManager
    {
        private int tenant;
        private bool IsLocalCurrency = false;
        private DateTime? FromDate = null;
        private DateTime? ToDate = null;
        private ICommonDataContext myCommonContext;
        private IQuotesContext myQuoteContext;
        public RacingQuoteManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;

            myCommonContext = CommonDataContext.GetContext(tenant);
            myQuoteContext = QuotesContext.GetContext(tenant);

            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations myQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);

            QueryFilterItem filterItem_FromDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
           

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
            RacingQuoteDataProvider myDataProvider = this.LoadDataProvider();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(RacingQuoteDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, myDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);

            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private RacingQuoteDataProvider LoadDataProvider()
        {
            RacingQuoteDataProvider myDataProvider = new RacingQuoteDataProvider();
            myDataProvider.Id = tenant;
            myDataProvider.From = this.GetDateString(this.FromDate);
            myDataProvider.To = this.GetDateString(this.ToDate);
            myDataProvider.Quotes = new List<RacingQuoteItem>();

      

            IQueryable<Quote> iQueryable =(from d in myQuoteContext.Quotes.Include("Stage").Include("Rating").Include("FromPort").Include("ToPort") where d.Tenant == tenant select d);
           
            if (this.FromDate != null)
            {
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OpenDate) >= System.Data.Entity.DbFunctions.TruncateTime(this.FromDate));
            }

            if (this.ToDate != null)
            {
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OpenDate) <= System.Data.Entity.DbFunctions.TruncateTime(this.ToDate));
            }

            List<Quote> allQuotes = new List<Quote>();
            allQuotes = iQueryable.ToList();
            

            List<string> allContactsIds = allQuotes.Where(d => d.SalesmanUserId != null).Select(s => s.SalesmanUserId).ToList();
            allContactsIds = allContactsIds.Concat(allQuotes.Where(d => d.CreatedByUserId != null).Select(s => s.CreatedByUserId)).ToList();

            List<Contact> allContacts = (from d in myCommonContext.Contacts where d.Tenant == tenant && allContactsIds.Contains(d.Id) select d).ToList();

            List<string> allQuotesIds = allQuotes.Select(s => s.Id).ToList();
            List<QuoteClosingReason> quoteClosingReasons = (from d in myQuoteContext.QuoteClosingReasons  select d).ToList();
           // List<QuoteCustomerType> quotecustomerTypes = (from d in myQuoteContext.QuoteCustomerTypes select d).ToList();
            List<QuoteType> quoteTypes = (from d in myQuoteContext.QuoteTypes select d).ToList();
            List<string> FromAddressIds = allQuotes.Where(p => p.TransportModeId == "I" && p.DirectionId == "D").Select(p => p.FromPartnerAddressId).ToList();
            List<string> ToAddressIds = allQuotes.Where(p => p.TransportModeId == "I" && p.DirectionId == "D").Select(p => p.ToPartnerAddressId).ToList();
            List<Address> allFromAddress = (from d in myCommonContext.Addresses where d.Tenant == tenant && FromAddressIds.Contains(d.Id) select d).ToList();
            List<Address> allToAddress = (from d in myCommonContext.Addresses where d.Tenant == tenant && ToAddressIds.Contains(d.Id) select d).ToList();

            List<PortPM> allPorts = new List<PortPM>();

            foreach (Quote item in allQuotes)
            {
                RacingQuoteItem myRecord = new RacingQuoteItem();
                if (!string.IsNullOrEmpty(item.QuoteClosingReasonCode))
                {
                    QuoteClosingReason quoteClosingReason = quoteClosingReasons.Where(d => d.Code == item.QuoteClosingReasonCode).FirstOrDefault();
                    if (quoteClosingReason != null) {
                        myRecord.ClosingReason = quoteClosingReason.Name;

}
                }

                //if (!string.IsNullOrEmpty(item.QuoteCustomerTypeCode))
                //{
                //    QuoteCustomerType quoteCustomerType = quotecustomerTypes.Where(d => d.Code == item.QuoteCustomerTypeCode).FirstOrDefault();
                //    if (quoteCustomerType != null)
                //    {
                //   //     myRecord.CustomerType = quoteCustomerType.Name;

                //    }
                //}


                if (!string.IsNullOrEmpty(item.QuoteTypeCode))
                {
                    QuoteType quoteType = quoteTypes.Where(d => d.Code == item.QuoteTypeCode).FirstOrDefault();
                    if (quoteType != null)
                    {
                        myRecord.Type = quoteType.Name;

                    }
                }

                myRecord.AcceptedDate = item.AcceptedDate;
                myRecord.Customer = item.CustomerName;
                myRecord.DeclinedDate = item.DeclinedDate;
                myRecord.FinishDate = item.ExpirationDate;
                myRecord.From = item.FromPort == null ? "" : item.FromPort.EnglishName;
                myRecord.To = item.ToPort == null ? "" : item.ToPort.EnglishName;

                string varTransportMode = String.Empty;
                switch (item.TransportModeId)
                {
                    case "A": { varTransportMode = "Air"; break; }
                    case "O": { varTransportMode = "Ocean"; break; }
                    case "I": { varTransportMode = "Inland"; break; }
                }
                if (!string.IsNullOrEmpty(item.DirectionId))
                {
                    myRecord.Direction = item.DirectionId == "E" ? "Export" : item.DirectionId == "I" ? "Import" : "Domestic";
                }
                myRecord.TransportMode = varTransportMode;
                myRecord.LastActivityDate = item.LastActivityDate;
                myRecord.OpenDate = item.OpenDate;
                myRecord.QuoteNotes = item.Notes;
                myRecord.QuoteNumber = item.QuoteNumber;
                myRecord.QuoteSubject = item.Subject;
                myRecord.SentDate = item.SentDate;
                myRecord.Stage = item.Stage.Name;
                myRecord.StartDate = item.StartDate;
       
                if (item.TransportModeId == "I" && item.DirectionId == "D")
                {
                    if (!string.IsNullOrEmpty(item.FromPartnerAddressId))
                    {
                        Address address = allFromAddress.Where(p => p.Id == item.FromPartnerAddressId).FirstOrDefault();
                        if (address != null)
                        {
                            Country fromCountry = CountryRepository.GetSingleCountry(address.CountryId, tenant, true);
                            if (fromCountry != null)
                            {
                                myRecord.FromPortCountry = fromCountry.EnglishName;
                                myRecord.From = address.City;
                            }
                        }
                    }


                    if (!string.IsNullOrEmpty(item.ToPartnerAddressId))
                    {
                        Address address = allToAddress.Where(p => p.Id == item.ToPartnerAddressId).FirstOrDefault();
                        if (address != null)
                        {
                            Country fromCountry = CountryRepository.GetSingleCountry(address.CountryId, tenant, true);
                            if (fromCountry != null)
                            {
                                myRecord.ToPortCountry = fromCountry.EnglishName;
                                myRecord.To = address.City;

                            }
                        }
                    }
                }
                    if (item.FromPort != null)
                {
                    Country fromCountry = CountryRepository.GetSingleCountry(item.FromPort.CountryId, tenant, true);
                    if (fromCountry != null)
                    {
                        myRecord.FromPortCountry = fromCountry.EnglishName;
                    }
                }

                if (item.ToPort != null)
                {
                    Country toCountry = CountryRepository.GetSingleCountry(item.ToPort.CountryId, tenant, true);
                    if (toCountry != null)
                    {
                        myRecord.ToPortCountry = toCountry.EnglishName;
                    }
                }


                Contact salesContact = allContacts.Where(d => d.Id == item.SalesmanUserId).FirstOrDefault();
                if (salesContact != null)
                {
                    myRecord.Salesman = salesContact.EnglishName;
                }

                Contact notifyContact = allContacts.Where(d => d.Id == item.NotifyId).FirstOrDefault();
                if (notifyContact != null)
                {
                    myRecord.Notify = notifyContact.EnglishName;
                }


                Contact userContact = allContacts.Where(d => d.Id == item.CreatedByUserId).FirstOrDefault();
                if (userContact != null)
                {
                    myRecord.OpenedBy = userContact.EnglishName;
                }

                if (!string.IsNullOrEmpty(item.NotifyId))
                {
                    Card notify = CardRepository.GetSingleCard(item.NotifyId, tenant, true);
                    if (notify != null)
                    {
                        myRecord.Notify = notify.EnglishName;
                    }

                }
                CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                customFieldResolver.SetDataProviderCustomFieldsValues("Quote", tenant, item, myRecord);



                myDataProvider.Quotes.Add(myRecord);
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