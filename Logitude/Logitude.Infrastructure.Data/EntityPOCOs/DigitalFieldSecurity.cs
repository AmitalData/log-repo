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

namespace Logitude.Infrastructure.Data.EntityPOCOs
{
   
    public class DigitalFieldSecurity
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [ForeignKey("ObjectTable")]
        [Column("ObjectTableId")]
	    public string ObjectTableId { get; set; }
	      
        public virtual ObjectTable ObjectTable { get; set; }
        [Column("DefaultSettings")]
	    public string DefaultSettings { get; set; }
        [ForeignKey("DigitalProfile")]
        [Column("ProfileId")]
	    public string ProfileId { get; set; }
	      
        public virtual DigitalProfile DigitalProfile { get; set; }
        [ForeignKey("ParentObjectTable")]
        [Column("ParentObjectTableId")]
	    public string ParentObjectTableId { get; set; }
	      
        public virtual ObjectTable ParentObjectTable { get; set; }
    }
}
	 