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
   
    public class CB_CountriesExclusion
    {
	 string dbms;

        [Key]
        [Column("ID")]
	    public string ID { get; set; }
        [ForeignKey("RegularityRequirement")]
        [Column("RegularityRequirementID")]
	    public string RegularityRequirementID { get; set; }
	      
        public virtual CB_RegularityRequirement RegularityRequirement { get; set; }
        [ForeignKey("CountryCode")]
        [Column("CountryID")]
	    public string CountryID { get; set; }
	      
        public virtual CustomsCountry CountryCode { get; set; }
    }
}
	 