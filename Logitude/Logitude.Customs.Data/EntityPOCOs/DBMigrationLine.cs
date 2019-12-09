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
   
    public class DBMigrationLine
    {
	 string dbms;

        [Key]
        [ForeignKey("MyDBMigration")]
        [Column("DBMigrationId")]
	    public string DBMigrationId { get; set; }
	      
        public virtual DBMigration MyDBMigration { get; set; }
     [Key]
        [Column("CounterKey")]
	    public int CounterKey { get; set; }
        [Column("SqlScript")]
	    public string SqlScript { get; set; }
        [Column("ApprovedRemarks")]
	    public string ApprovedRemarks { get; set; }
    }
}
	 