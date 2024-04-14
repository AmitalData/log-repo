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
   
    public class ClientItem
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("ItemDescription")]
	    public string ItemDescription { get; set; }
        [Column("ClassificationCode")]
	    public string ClassificationCode { get; set; }
        [Column("ItemCode")]
	    public string ItemCode { get; set; }
        [ForeignKey("OriginCountry")]
        [Column("OriginCountryCode")]
	    public string OriginCountryCode { get; set; }
	      
        public virtual CustomsCountry OriginCountry { get; set; }
     [Key]
        [Column("ClientCode")]
	    public string ClientCode { get; set; }
     [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("ItemKey")]
	    public string ItemKey { get; set; }
    }
}
	 