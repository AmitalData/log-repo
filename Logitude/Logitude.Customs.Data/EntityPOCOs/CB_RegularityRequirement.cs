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
   
    public class CB_RegularityRequirement
    {
	 string dbms;

        [Key]
        [Column("ID")]
	    public int ID { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime? UpdateDate { get; set; }
        [Column("CountryID")]
	    public string CountryID { get; set; }
        [Column("IsAllCountries")]
	    public bool IsAllCountries { get; set; }
        [ForeignKey("CustomsItem")]
        [Column("CustomsItemID")]
	    public int? CustomsItemID { get; set; }
	      
        public virtual CB_CustomsItem CustomsItem { get; set; }
        [Column("IsAllCustomsItems")]
	    public bool IsAllCustomsItems { get; set; }
        [Column("IsLimitedCountryRegularRequire")]
	    public bool IsLimitedCountryRegularRequire { get; set; }
        [Column("StartDate")]
	    public DateTime StartDate { get; set; }
        [Column("EndDate")]
	    public DateTime EndDate { get; set; }
        [ForeignKey("InceptionCode")]
        [Column("InceptionCodeID")]
	    public string InceptionCodeID { get; set; }
	      
        public virtual InceptionCode InceptionCode { get; set; }
        [ForeignKey("RegularityPublicationCode")]
        [Column("RegularityPublicationCodeID")]
	    public string RegularityPublicationCodeID { get; set; }
	      
        public virtual RegularityPublication RegularityPublicationCode { get; set; }
        [ForeignKey("RegularitySourceCode")]
        [Column("RegularitySourceCodeID")]
	    public string RegularitySourceCodeID { get; set; }
	      
        public virtual RegularitySource RegularitySourceCode { get; set; }
        [ForeignKey("CustomsBookTypeCode")]
        [Column("CustomsBookTypeID")]
	    public string CustomsBookTypeID { get; set; }
	      
        public virtual CustomsBookType CustomsBookTypeCode { get; set; }
    }
}
	 