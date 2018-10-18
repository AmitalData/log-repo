using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class CustomerPotentialActualDataProvider : BaseDataProvider
    {
        public DateTime TodayDate { get; set;}
        public List<CustomersData> Customers { get; set; }
    }

    public class CustomersData
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public int LocationsCount { get; set; }
        public string Salesman { get; set; }
        public decimal? AD_POT { get; set; }
        public decimal? AR_POT { get; set; }
        public decimal? AE_POT { get; set; }
        public decimal? AI_POT { get; set; }
        public decimal? ID_POT { get; set; }
        public decimal? IR_POT { get; set; }
        public decimal? IE_POT { get; set; }
        public decimal? II_POT { get; set; }
        public decimal? OD_POT { get; set; }
        public decimal? OR_POT { get; set; }
        public decimal? OE_POT { get; set; }
        public decimal? OI_POT { get; set; }
        public decimal? CI_POT { get; set; }
        public decimal? DL_POT { get; set; }
        public decimal? IN_POT { get; set; }
        public decimal? AD_ACT { get; set; }
        public decimal? AR_ACT { get; set; }
        public decimal? AE_ACT { get; set; }
        public decimal? AI_ACT { get; set; }
        public decimal? ID_ACT { get; set; }
        public decimal? IR_ACT { get; set; }
        public decimal? IE_ACT { get; set; }
        public decimal? II_ACT { get; set; }
        public decimal? OD_ACT { get; set; }
        public decimal? OR_ACT { get; set; }
        public decimal? OE_ACT { get; set; }
        public decimal? OI_ACT { get; set; }
        public decimal? CI_ACT { get; set; }
        public decimal? DL_ACT { get; set; }
        public decimal? IN_ACT { get; set; }
    }
}