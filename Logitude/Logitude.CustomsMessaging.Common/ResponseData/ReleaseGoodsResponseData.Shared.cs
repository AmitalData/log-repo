using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class ReleaseGoodsResponseData : ResponseDataBase
    {
        public string DeclarationNumber { get; set; }
        public string FileNumber { get; set; }
        public string governmentProcedureType { get; set; }
        public string releaseDate { get; set; }
        public string dealValueNIS { get; set; }
        public string CifValueNis { get; set; }
        public string CurrencyTypeCode { get; set; }
        public string ExchangeRate { get; set; }
        public string TaxationDate { get; set; }
        public string importerExpoterExternalID { get; set; }
        public string cargoIdentifierType { get; set; }
        public string cargoIdentifierKey1 { get; set; }
        public string cargoIdentifierKey2 { get; set; }
        public string loadingPort { get; set; }
        public string unloadingSiteNumber { get; set; }
        public string storageSiteNumber { get; set; }
        public string cargoDescription { get; set; }
        public string packageType { get; set; }
        public string packageQuantity { get; set; }
        public string packagesWeight { get; set; }

        public List<GoodsItems> GoodsItemsList { get; set; }
    }

    public class GoodsItems
    {
        public string SupplierInvoice { get; set; }
        public string GoodsItemPath { get; set; }
        public string CustomItemID { get; set; }
    }
}
