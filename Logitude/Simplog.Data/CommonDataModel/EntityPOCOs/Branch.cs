using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Branch
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string Notes { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
        public string ExternalId { get; set; }
        public string AddressId { get; set; }
        public string Signature { get; set; }

        public string INTTRAId { get; set; }
        public string INTTRAContactId { get; set; }
        public string INTTRAAlias { get; set; }
		public string CounterCode { get; set; }

		public virtual Address Address { get; set; }

        [ForeignKey("INTTRAContactId")]
        public virtual Contact INTTRAContact { get; set; }
    }
}
