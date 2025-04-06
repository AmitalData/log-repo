using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
   
    public class AdditionalCurrencyRate
    {
        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
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
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("Name")]
	    public string Name { get; set; }
        [Column("RateCoefficient")]
	    public double? RateCoefficient { get; set; }
    }
}
	 