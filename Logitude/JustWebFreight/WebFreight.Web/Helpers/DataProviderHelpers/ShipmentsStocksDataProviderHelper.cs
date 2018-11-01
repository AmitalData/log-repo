using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.Helpers.DataProviderHelpers
{
    public class ShipmentsStocksDataProviderHelper
    { 

        public ShipmentsStocksDataProvider LoadShipmentsStocksDataProvider(byte[] xmlFilters, int tenant)
        {
            ShipmentsStocksDataProvider dataProvider = new ShipmentsStocksDataProvider();
            dataProvider.ResultList = new List<ShipmentsStocksResult>();

            bool includeShipmentsDetails = false;// true;
            DateTime? fromDate = null; // DateTime.Now.AddYears(-1);
            DateTime? toDate = null;// DateTime.Now;


            #region Report Filters
            if (xmlFilters != null)
            {
                MemoryStream memorystream = new MemoryStream(xmlFilters);
                XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
                QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);


    

                //From Date
                QueryFilterItem queryFilterItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
                if (queryFilterItem != null && queryFilterItem.FieldValue != null) fromDate = DateTime.Parse(queryFilterItem.FieldValue.ToString());

                //To Date
                queryFilterItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
                if (queryFilterItem != null && queryFilterItem.FieldValue != null) toDate = DateTime.Parse(queryFilterItem.FieldValue.ToString());

                //IncludeShipmentsDetails
                queryFilterItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeShipmentsDetails").FirstOrDefault();
                if (queryFilterItem != null && queryFilterItem.FieldValue != null) includeShipmentsDetails = queryFilterItem.FieldValue.ToString().ToLower() == "true" ? true : false;
            }

            CustomerTenantAccessCardQuery customerTenantAccessCardQuery = new CustomerTenantAccessCardQuery(tenant);

            List<string> cardIds = customerTenantAccessCardQuery.GetCustomerIdsByTenant(tenant);
            List<CardList> cardLists = null;
            if (cardIds.Count > 0)
            {
                CardQuery cardQuery = new CardQuery(tenant);
                cardLists = cardQuery.GetCardListsByCardIds(cardIds, tenant);
            }

            if (cardLists != null)
            {
                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                cardIds = cardLists.GroupBy(d => d.Id).Select(d => d.First().Id).ToList();
                List<ShipmentList> shipmentLists = shipmentQuery.GetShipmentListsByCustomerIdsAndDates(cardIds, fromDate, toDate, tenant);
                foreach (CardList card in cardLists)
                {
                    dataProvider.ResultList.Add(new ShipmentsStocksResult()
                    {
                        Id = card.Id,
                        CustomerName = card.EnglishName,
                        LastShipmentDate = shipmentLists.Where(d => d.CustomerId == card.Id).OrderByDescending(d => d.CreateDateTime).FirstOrDefault().CreateDateTime,
                        TotalShipments = shipmentLists.Where(d => d.CustomerId == card.Id).Count(),
                    });



                    if (includeShipmentsDetails)
                    {
                        foreach (ShipmentList shipment in shipmentLists.Where(d => d.CustomerId == card.Id))
                        {
                            dataProvider.ResultList.Add(new ShipmentsStocksResult()
                            {
                                ShipmentNumber = shipment.ShipmentNumber,
                                CreateDate = shipment.CreateDateTime,
                                Status = shipment.StatusName,
                                ParentId = card.Id,
                            });
                        }
                    }

                }

                dataProvider.TotlaShipment = shipmentLists.Count();
            }

    
            #endregion
            return dataProvider;
        }









    }
}