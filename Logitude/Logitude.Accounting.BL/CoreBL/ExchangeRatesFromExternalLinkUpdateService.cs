using Logitude.BL.InfrastructureModel.APIDataContract;
using Logitude.BL.InfrastructureModel.APIDataContract.Messages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;

namespace Logitude.Accounting.BL.CoreBL
{
   public class ExchangeRatesFromExternalLinkUpdateService
    {
        private readonly int _tenant;
        private const string XmlLinkedNode = "https://boi.org.il/PublicApi/GetExchangeRates?asXML=true";
        
        public ExchangeRatesFromExternalLinkUpdateService(int tenant)
        {
            _tenant = tenant;
        }
        public void UpdateRatesByExternalXml()
        {
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			XmlDocument document = GetExchangeRatesXmlFromExternalLink();
            List<RateUpdate> rates = GetRatesFromXml(document);
            RatesUpdate ratesUpdate = GetRatesUpdate(rates);
            UpdateRatesByService(ratesUpdate);

        }
        private XmlDocument GetExchangeRatesXmlFromExternalLink()
        {
            XmlDocument document = new XmlDocument();
            document.Load(XmlLinkedNode);
            return document;
        }     
        private void UpdateRatesByService(RatesUpdate ratesUpdate)
        {
            RatesUpdateService ratesUpdateService = new RatesUpdateService(ratesUpdate, _tenant);
            ratesUpdateService.ValidateRatesDataMapping();
            ratesUpdateService.UpdateRatesData();
        }
        private List<RateUpdate> GetRatesFromXml(XmlDocument document)
        {
            XmlNode currencyNodes = document.GetElementsByTagName("ExchangeRates")[0];
            List<RateUpdate> rates = new List<RateUpdate>();

            foreach (XmlNode currencyNode in currencyNodes.ChildNodes)
            {
                rates.Add(AddCurrencyRateToRatesList(currencyNode));
            }
            return rates;
        }
        private RateUpdate AddCurrencyRateToRatesList(XmlNode currencyNode)
        {
            string currencyCode = currencyNode["Key"]?.InnerText;
            string rate = currencyNode["CurrentExchangeRate"]?.InnerText;
            string unit = currencyNode["Unit"]?.InnerText;
            string lastUpdateDate = currencyNode["LastUpdate"]?.InnerText;

            RateUpdate rateUpdate = CreateRateUpdateInstance(currencyCode, rate,unit, lastUpdateDate);
            
            return rateUpdate;
        }

        private RatesUpdate GetRatesUpdate(List<RateUpdate> rates)
        {
            RatesUpdate ratesUpdate = new RatesUpdate();
            ratesUpdate.RateUpdateList = rates;
            return ratesUpdate;
        }

        private RateUpdate CreateRateUpdateInstance(string currencyCode, string rate,string unit, string date)
        {
            Currency currency = new Currency()
            {
                Code = currencyCode,
            };

            return new RateUpdate()
            {
                Currency = currency,
                Rate = Double.Parse(rate),
                Unit = Int16.Parse(unit),
                RateDate = DateTime.Parse(date),
            };
        }
       
    }
}
