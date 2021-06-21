using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class ProductItemLine 
    {
        public ProductItemLine()
        {
            this.HTScodeLines = new List<HTSCodeLine>();
        }

        public string Name { get; set; }
        public string Brand { get; set; }
        public int Tenant { get; set; }
        public string CustomerId { get; set; }
        public string SKU { get; set; }
        public string Remarks { get; set; }
        public bool InActive { get; set; }
        public string Description { get; set; }
        public string ShipmentItemHTSCode { get; set; }
        public  List<HTSCodeLine> HTScodeLines { get; set; }
    }


    public class HTSCodeLine {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ItemId { get; set; }
        public string DestinationCountryId { get; set; }
        public string Code { get; set; }
        public bool ApprovedByCustomer { get; set; }
        public bool InActive { get; set; }
        public string CountryEnglishName { get; set; }
    }

}
