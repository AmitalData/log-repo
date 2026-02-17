using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.Customs.Data.EntityPOCOs
{
   
    public class VendorCommunication
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
     [Key]
        [ForeignKey("Vendor")]
        [Column("VendorId" ,Order = 1)]
	    public string VendorId { get; set; }
	      
        public virtual CustomsVendor Vendor { get; set; }
     [Key]
        [Column("LineNumber" ,Order = 2)]
	    public int LineNumber { get; set; }
        [ForeignKey("CommunicationType")]
        [Column("CommunicationTypeCode")]
	    public string CommunicationTypeCode { get; set; }
	      
        public virtual CommunicationType CommunicationType { get; set; }
        [Column("CommunicationAddress")]
	    public string CommunicationAddress { get; set; }
    }
}
	 