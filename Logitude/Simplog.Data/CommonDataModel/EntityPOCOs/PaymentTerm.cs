using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class PaymentTerm
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }        
        public bool InActive { get; set; }
        public bool AddedManually { get; set; }
        public int Days { get; set; }
        public bool DisplayInLOV { get; set; }
        public string Description { get; set; }
        public string LocalDescription { get; set; }
        public string SearchFields { get; set; }
        public string ExternalId { get; set; }
        public bool IsManuallySet { get; set; }
        public bool CurrentMonth { get; set; }
        public string FromDateTypeCode { get; set; }
        public string Code { get; set; }

        [ForeignKey("FromDateTypeCode")]
        public virtual PaymentTermDateType FromDateType { get; set; }
        
    }
}