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

           [Column("ID")]
	    public int ID { get; set; }
        [Column("RegularityRequirementID")]
	    public int RegularityRequirementID { get; set; }
        [Column("CountryID")]
	    public string CountryID { get; set; }
     [Key]
        [Column("CB_ID")]
	    public string CB_ID { get; set; }
    }
}
	 