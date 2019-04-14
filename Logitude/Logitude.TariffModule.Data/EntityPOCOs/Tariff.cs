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

namespace Logitude.TariffModule.Data.EntityPOCOs
{
   
    public class Tariff
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("StartDate")]
	    public DateTime? StartDate { get; set; }
        [Column("ExpirationDate")]
	    public DateTime? ExpirationDate { get; set; }
        [Column("Name")]
	    public string Name { get; set; }
        [Column("InActive")]
	    public bool InActive { get; set; }
        [Column("Description")]
	    public string Description { get; set; }
        [Column("SellerId")]
	    public string SellerId { get; set; }
        [Column("CurrencyId")]
	    public string CurrencyId { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [Column("LastExpirationDate")]
	    public DateTime? LastExpirationDate { get; set; }
        [Column("PriceSteps")]
	    public string PriceSteps { get; set; }
        [Column("TypeCode")]
	    public string TypeCode { get; set; }
        [Column("LastStartDate")]
	    public DateTime? LastStartDate { get; set; }
        [Column("LastVersion")]
	    public int LastVersion { get; set; }
        [Column("ContractNumber")]
	    public int? ContractNumber { get; set; }
    }
}
	 