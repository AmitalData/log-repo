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
        [ForeignKey("Seller")]
        [Column("SellerId")]
	    public string SellerId { get; set; }
	      
        public virtual Card Seller { get; set; }
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
        [Column("TariffNumber")]
	    public string TariffNumber { get; set; }
        [ForeignKey("Surcharge1I")]
        [Column("Surcharge1Id")]
	    public string Surcharge1Id { get; set; }
	      
        public virtual ChargesType Surcharge1I { get; set; }
        [ForeignKey("Surcharge2I")]
        [Column("Surcharge2Id")]
	    public string Surcharge2Id { get; set; }
	      
        public virtual ChargesType Surcharge2I { get; set; }
        [ForeignKey("Surcharge3I")]
        [Column("Surcharge3Id")]
	    public string Surcharge3Id { get; set; }
	      
        public virtual ChargesType Surcharge3I { get; set; }
        [ForeignKey("Surcharge4I")]
        [Column("Surcharge4Id")]
	    public string Surcharge4Id { get; set; }
	      
        public virtual ChargesType Surcharge4I { get; set; }
        [ForeignKey("Surcharge5I")]
        [Column("Surcharge5Id")]
	    public string Surcharge5Id { get; set; }
	      
        public virtual ChargesType Surcharge5I { get; set; }
        [ForeignKey("Surcharge6I")]
        [Column("Surcharge6Id")]
	    public string Surcharge6Id { get; set; }
	      
        public virtual ChargesType Surcharge6I { get; set; }
        [ForeignKey("Surcharge7I")]
        [Column("Surcharge7Id")]
	    public string Surcharge7Id { get; set; }
	      
        public virtual ChargesType Surcharge7I { get; set; }
        [ForeignKey("Surcharge8I")]
        [Column("Surcharge8Id")]
	    public string Surcharge8Id { get; set; }
	      
        public virtual ChargesType Surcharge8I { get; set; }
        [ForeignKey("Surcharge9I")]
        [Column("Surcharge9Id")]
	    public string Surcharge9Id { get; set; }
	      
        public virtual ChargesType Surcharge9I { get; set; }
        [ForeignKey("Surcharge10I")]
        [Column("Surcharge10Id")]
	    public string Surcharge10Id { get; set; }
	      
        public virtual ChargesType Surcharge10I { get; set; }
        [ForeignKey("Surcharge1U")]
        [Column("Surcharge1UOM")]
	    public string Surcharge1UOM { get; set; }
	      
        public virtual Measurement Surcharge1U { get; set; }
        [ForeignKey("Surcharge2U")]
        [Column("Surcharge2UOM")]
	    public string Surcharge2UOM { get; set; }
	      
        public virtual Measurement Surcharge2U { get; set; }
        [ForeignKey("Surcharge3U")]
        [Column("Surcharge3UOM")]
	    public string Surcharge3UOM { get; set; }
	      
        public virtual Measurement Surcharge3U { get; set; }
        [ForeignKey("Surcharge4U")]
        [Column("Surcharge4UOM")]
	    public string Surcharge4UOM { get; set; }
	      
        public virtual Measurement Surcharge4U { get; set; }
        [ForeignKey("Surcharge5U")]
        [Column("Surcharge5UOM")]
	    public string Surcharge5UOM { get; set; }
	      
        public virtual Measurement Surcharge5U { get; set; }
        [ForeignKey("Surcharge6U")]
        [Column("Surcharge6UOM")]
	    public string Surcharge6UOM { get; set; }
	      
        public virtual Measurement Surcharge6U { get; set; }
        [ForeignKey("Surcharge7U")]
        [Column("Surcharge7UOM")]
	    public string Surcharge7UOM { get; set; }
	      
        public virtual Measurement Surcharge7U { get; set; }
        [ForeignKey("Surcharge8U")]
        [Column("Surcharge8UOM")]
	    public string Surcharge8UOM { get; set; }
	      
        public virtual Measurement Surcharge8U { get; set; }
        [ForeignKey("Surcharge9U")]
        [Column("Surcharge9UOM")]
	    public string Surcharge9UOM { get; set; }
	      
        public virtual Measurement Surcharge9U { get; set; }
        [ForeignKey("Surcharge10U")]
        [Column("Surcharge10UOM")]
	    public string Surcharge10UOM { get; set; }
	      
        public virtual Measurement Surcharge10U { get; set; }
    }
}
	 