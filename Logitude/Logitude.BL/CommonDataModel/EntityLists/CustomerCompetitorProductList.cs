using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CustomerCompetitorProductList
    {
        [Key]
        public string CustomerId { get; set; }

        [Key]
        public string CompetitorId { get; set; }

        [Key]
        public string ProductTypeCode { get; set; }

        public int Tenant { get; set; }

        public string ProductTypeName { get; set; }

        public string CustomerName { get; set; }

        public string CompetitorName { get; set; }
    }
}