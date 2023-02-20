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

namespace Logitude.Workflow.Data.EntityPOCOs
{
   
    public class Expression
    {
	 string dbms;

        [Key]
        [Column("Code")]
	    public string Code { get; set; }
        [Column("Name")]
	    public string Name { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("Body")]
	    public string Body { get; set; }
        [Column("Description")]
	    public string Description { get; set; }
        [ForeignKey("Category")]
        [Column("CategoryCode")]
	    public string CategoryCode { get; set; }
	      
        public virtual ExpressionCategory Category { get; set; }
        [Column("Title")]
	    public string Title { get; set; }
    }
}
	 