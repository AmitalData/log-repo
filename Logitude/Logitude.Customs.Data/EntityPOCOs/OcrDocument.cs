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
   
    public class OcrDocument
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("Process")]
	    public string Process { get; set; }
        [Column("JsonUrl")]
	    public string JsonUrl { get; set; }
        [Column("Score")]
	    public decimal? Score { get; set; }
        [Column("JsonTif")]
	    public string JsonTif { get; set; }
        [Column("ErrorMsg")]
	    public string ErrorMsg { get; set; }
        [ForeignKey("OcrStatus")]
        [Column("StatusCode")]
	    public string StatusCode { get; set; }
	      
        public virtual OcrStatus OcrStatus { get; set; }
        [Column("OcrId")]
	    public string OcrId { get; set; }
        [ForeignKey("DocumentsFiling")]
        [Column("DocId")]
	    public string DocId { get; set; }
	      
        public virtual DocumentsFiling DocumentsFiling { get; set; }
        [Column("Reference")]
	    public string Reference { get; set; }
        [Column("JsonData")]
	    public string JsonData { get; set; }
        [Column("NotConnect")]
	    public bool? NotConnect { get; set; }
    }
}
	 