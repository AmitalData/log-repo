using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CustomerForwarderByProductList
    {
        [Key]
        public string ProductTypeCode { get; set; }

        public string ForwarderId { get; set; }
        [Key]
        public string CustomerId { get; set; }

        public int Tenant { get; set; }


    }
}
