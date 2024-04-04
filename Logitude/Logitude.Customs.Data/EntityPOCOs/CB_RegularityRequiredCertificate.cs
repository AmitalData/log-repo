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
   
    public class CB_RegularityRequiredCertificate
    {
	 string dbms;

           [Column("ID")]
	    public int ID { get; set; }
        [Column("RegularityInceptionID")]
	    public int RegularityInceptionID { get; set; }
        [ForeignKey("ConfirmationTypeCode")]
        [Column("ConfirmationTypeID")]
	    public string ConfirmationTypeID { get; set; }
	      
        public virtual ConfirmationType ConfirmationTypeCode { get; set; }
        [Column("Number")]
	    public int? Number { get; set; }
        [Column("TextualCondition")]
	    public string TextualCondition { get; set; }
        [Column("TrNumber")]
	    public int? TrNumber { get; set; }
        [ForeignKey("AuthorityCode")]
        [Column("AuthorityID")]
	    public string AuthorityID { get; set; }
	      
        public virtual Authority AuthorityCode { get; set; }
     [Key]
        [Column("CB_ID")]
	    public string CB_ID { get; set; }
    }
}
	 