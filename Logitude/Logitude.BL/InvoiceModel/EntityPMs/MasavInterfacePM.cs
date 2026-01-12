using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    public class MasavInterfacePM
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string SearchFields { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public double? Amount { get; set; }
        public int TotalPayments { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }

    }
}