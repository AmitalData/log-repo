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

namespace Logitude.DashboardModule.Data.EntityPOCOs
{
   
    public class AnalyticsFactsMetaData
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Name")]
	    public string Name { get; set; }
        [Column("TableName")]
	    public string TableName { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("HashString")]
	    public string HashString { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("ObjectTableName")]
	    public string ObjectTableName { get; set; }
    }
}
	 