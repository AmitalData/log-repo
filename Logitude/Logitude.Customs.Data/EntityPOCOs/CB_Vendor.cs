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
   
    public class CB_Vendor
    {
	 string dbms;

        [Key]
        [Column("ID")]
	    public int ID { get; set; }
        [Column("Title")]
	    public string Title { get; set; }
        [Column("State")]
	    public int State { get; set; }
        [Column("EnglishCountryName")]
	    public string EnglishCountryName { get; set; }
        [Column("VendorSingleStringAddress")]
	    public string VendorSingleStringAddress { get; set; }
    }
}
	 