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
   
    public class DBMigration
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("ExecuteDate")]
	    public DateTime ExecuteDate { get; set; }
        [Column("MajorVersion")]
	    public decimal MajorVersion { get; set; }
        [Column("MinorVersion")]
	    public int MinorVersion { get; set; }
        [Column("Remarks")]
	    public string Remarks { get; set; }
        [Column("IsClose")]
	    public bool IsClose { get; set; }
    }
}
	 