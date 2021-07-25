using Logitude.BL.InfrastructureModel.APIDataContract;
using Logitude.BL.InfrastructureModel.APIDataContract.Messages;
using Simplog.Data.CommonDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Logitude.Accounting.BL.CoreBL
{
   public class ExchangeRatesFromExternalLinkUpdateService
    {
        int tenant;
        private const string XmlLinkedNode = "https://forex.boi.org.il/currency.xml";
        List<string> TenantCurrencies;
        public ExchangeRatesFromExternalLinkUpdateService(int Tenant)
        {
            tenant = Tenant;

        }
        public void UpdateRatesByExternalXML()
        {
            XmlDocument document = GetExchangRatesXmlFromExternalLink();
            List<RateUpdate> rates = GetRatesFromXML(document);
            RatesUpdate ratesUpdate = GetRatesUpdate(rates);
            UpdateRatesByService(ratesUpdate);

        }
        private XmlDocument GetExchangRatesXmlFromExternalLink()
        {
            XmlDocument document = new XmlDocument();
            document.Load(XmlLinkedNode);
            return document;
        }     
        private void UpdateRatesByService(RatesUpdate ratesUpdate)
        {
            RatesUpdateService ratesUpdateService = new RatesUpdateService(ratesUpdate, tenant);
            ratesUpdateService.ValidateRatesDataMapping();
            ratesUpdateService.UpdateRatesData();
        }
        private List<RateUpdate> GetRatesFromXML(XmlDocument document)
        {
            XmlNodeList currencyNodes = document.GetElementsByTagName("CURRENCY");
            XmlNodeList lastUpdateDateNode = document.GetElementsByTagName("LAST_UPDATE");
            List<RateUpdate> rates = new List<RateUpdate>();
            TenantCurrencies = GetTenantCurrenciesCodes();
            foreach (XmlNode currencyNode in currencyNodes)
            {
                rates = AddCurrencyRateToRatesList(currencyNode, rates, lastUpdateDateNode);
             
            }
            return rates;
        }
        private List<RateUpdate> AddCurrencyRateToRatesList(XmlNode currencyNode, List<RateUpdate> rates, XmlNodeList lastUpdateDateNode)
        {
            var currencyCode = currencyNode.SelectNodes("CURRENCYCODE")[0].InnerText;
            var rate = currencyNode.SelectNodes("RATE")[0].InnerText;
            if (TenantCurrencies.Contains(currencyCode))
            {
                RateUpdate rateUpdate = CreateRateUpdateInstance(currencyCode, rate, lastUpdateDateNode);
                rates.Add(rateUpdate);
            }
            return rates;
        }

        private RatesUpdate GetRatesUpdate(List<RateUpdate> rates)
        {
            RatesUpdate ratesUpdate = new RatesUpdate();
            ratesUpdate.RateUpdateList = rates;
            return ratesUpdate;
        }
        private List<string> GetTenantCurrenciesCodes()
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
            return (from a in objectContext.Currencies where a.Tenant ==tenant select a.Code).ToList();
        }
        private RateUpdate CreateRateUpdateInstance(string currencyCode, string rate, XmlNodeList dateNode)
        {
            Logitude.BL.InfrastructureModel.APIDataContract.Currency currency = new Logitude.BL.InfrastructureModel.APIDataContract.Currency()
            {
                Code = currencyCode,
            };

            return new RateUpdate()
            {
                Currency = currency,
                Rate = Double.Parse(rate),
                RateDate = DateTime.Parse(dateNode[0].InnerText),
            };
        }
       
    }
}
