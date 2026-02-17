using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class CustomerAdditionalServicesDataProvider : BaseDataProvider
    {
        public string BusinessUnitName { get; set; }
        public string SalesmanUserName { get; set; }

        public List<AdditionalServicesDataList> AdditionalServices { get; set; }
    }

    public class AdditionalServicesDataList
    {
        public string CustomerName { get; set; }
        public string PrimaryContact { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string Salesman { get; set; }

        public string ServiceName { get; set; }
        public string Potential_InUse { get; set; }

        public int? NumberOfShipments { get; set; }
        public string NumberOfShipmentsLabel { get; set; }
    }
}