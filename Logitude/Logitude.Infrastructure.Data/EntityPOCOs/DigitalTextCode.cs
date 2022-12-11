using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.Infrastructure.Data.EntityPOCOs
{

    public class DigitalTextCode
    {
	    string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [ForeignKey("ObjectTable")]
        [Column("ObjectTableId")]
	    public string ObjectTableId { get; set; }
	      
        public virtual ObjectTable ObjectTable { get; set; }
        [Column("Labels")]
	    public string Labels { get; set; }
    }
}
	 