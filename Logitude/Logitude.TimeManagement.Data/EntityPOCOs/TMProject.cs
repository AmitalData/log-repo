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

namespace Logitude.TimeManagement.Data.EntityPOCOs
{
   
    public class TMProject
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("Name")]
	    public string Name { get; set; }
        [Column("Description")]
	    public string Description { get; set; }
        [ForeignKey("Customer")]
        [Column("CustomerId")]
	    public string CustomerId { get; set; }
	      
        public virtual Card Customer { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("Owner")]
        [Column("OwnerId")]
	    public string OwnerId { get; set; }
	      
        public virtual User Owner { get; set; }
        [Column("ProjectNumber")]
	    public string ProjectNumber { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("IsInnerProject")]
	    public bool IsInnerProject { get; set; }
        [Column("Inactive")]
	    public bool Inactive { get; set; }
        [ForeignKey("TMBudget")]
        [Column("BudgetId")]
	    public string BudgetId { get; set; }
	      
        public virtual TMBudget TMBudget { get; set; }
        [ForeignKey("TMProjectCategory")]
        [Column("CategoryId")]
	    public string CategoryId { get; set; }
	      
        public virtual TMProjectCategory TMProjectCategory { get; set; }
        [Column("IsProrated")]
	    public bool IsProrated { get; set; }
        [Column("ExternalProjectNumber")]
	    public string ExternalProjectNumber { get; set; }
        [Column("ExcludeFromProrating")]
	    public bool ExcludeFromProrating { get; set; }
    }
}
	 