using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.DataContracts
{
    public class RequestPayment
    {
        [Key]
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string Master { get; set; }
        public string Hawb { get; set; }
        public string DeclarationNumber { get; set; }
        public string ShipmentValueInNIS { get; set; }
        public string SenderDetails { get; set; }
        public string GoodsDescritpion { get; set; }
        public string Quantity { get; set; }
        public string Weight { get; set; }
        public string TotalChargesInNIS { get; set; }
       


        public List<ServiceType> ServiceTypes { get; set; }
     
       
    }

    public class ServiceType
    {
        public string ServiceTypeCode { get; set; }
        public string LocalName { get; set; }
        public string AmountInNIS { get; set; } 
    } 
}