using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CustomerSalesmanByProductList
    {
        [Key]
        public string ProductTypeCode { get; set; }

        public string SalesmanUserId { get; set; }
        [Key]
        public string CustomerId { get; set; }

        public int Tenant { get; set; }
    }
}
