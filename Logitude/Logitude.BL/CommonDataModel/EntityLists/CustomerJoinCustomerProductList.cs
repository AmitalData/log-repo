using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CustomerJoinCustomerProductList
    {
        [Key]
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string PrimaryContact { get; set; }
        public string Salesman { get; set; }
        public string CustomerId { get; set; }
        public string ProductTypeCode { get; set; }

        public int? Potential_Shipments { get; set; }
        public int? Actual_Shipments { get; set; }

        public decimal? Potential_TEU{ get; set; }
        public decimal? Actual_TEU { get; set; }

        public decimal? Potential_Revenue { get; set; }
        public decimal? Actual_Revenue { get; set; }

        public decimal? Potential_Weight { get; set; }
        public decimal? Actual_Weight { get; set; }

        public decimal? TotalPotentialData { get; set; }
    }
}