using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.DataContracts
{
	public class SupplieInvoiceItemsForSIIRequest
	{
		public string InvoiceNumber { get; set; }
		public int LineNumber { get; set; }
		public int CounterKey { get; set; }
        public string ItemCode { get; set; }
		public string ItemDescription { get; set; }
		public string ClassificationCode { get; set; }
		public string TradeAgreementCode { get; set; }
		public string InvoiceQuantityType { get; set; }
		public string InvoiceQuantity { get; set; }
		public string ItemPrice { get; set; }
		public string ItemPriceCurrencyCode { get; set; }
		public string OriginCountryCode { get; set; }
		public string InvoiceQuantityTypeName { get; set; }
		public string TradeAgreementName { get; set; }
		public string OriginCountryName { get; set; }
		public string ReqConfirmationTypeCode { get; set; }
		public string RequestRequiredStatus { get; set; }

    }
}
