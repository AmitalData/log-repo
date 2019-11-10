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
   
    public class DecDangersContact
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [ForeignKey("Declaration")]
        [Column("DeclarationId" ,Order = 1)]
	    public string DeclarationId { get; set; }
	      
        public virtual Declaration Declaration { get; set; }
        [Column("CompanyName")]
	    public string CompanyName { get; set; }
        [Column("CompanyCommNumber")]
	    public string CompanyCommNumber { get; set; }
        [ForeignKey("CommunicationType")]
        [Column("CompanyCommTypeCode")]
	    public string CompanyCommTypeCode { get; set; }
	      
        public virtual CommunicationType CommunicationType { get; set; }
        [Column("ContactName")]
	    public string ContactName { get; set; }
        [Column("ContactCommNumber")]
	    public string ContactCommNumber { get; set; }
        [ForeignKey("ContactCommunicationType")]
        [Column("ContactCommTypeCode")]
	    public string ContactCommTypeCode { get; set; }
	      
        public virtual CommunicationType ContactCommunicationType { get; set; }
        [Column("ContactId")]
	    public string ContactId { get; set; }
    }
}
	 