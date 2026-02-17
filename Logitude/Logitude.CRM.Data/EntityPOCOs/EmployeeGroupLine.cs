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
using Logitude.Infrastructure.Data.EntityPOCOs;
namespace Logitude.CRM.Data.EntityPOCOs
{
   
    public class EmployeeGroupLine
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("EmployeeGroup")]
        [Column("EmployeeGroupId")]
	    public string EmployeeGroupId { get; set; }
	      
        public virtual EmployeeGroup EmployeeGroup { get; set; }
        [ForeignKey("User")]
        [Column("UserId")]
	    public string UserId { get; set; }
	      
        public virtual User User { get; set; }
        [Column("IsDefaultOwner")]
	    public bool IsDefaultOwner { get; set; }
    }
}
	 