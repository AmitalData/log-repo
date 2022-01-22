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
   
    public class TariffSetting
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("DefaultPriceSteps")]
	    public string DefaultPriceSteps { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("DefaultWarningPercentage")]
	    public double? DefaultWarningPercentage { get; set; }
        [Column("AirDefaultStepsId")]
	    public string AirDefaultStepsId { get; set; }
        [Column("LCLDefaultStepsId")]
	    public string LCLDefaultStepsId { get; set; }
        [Column("ContainerDefaults")]
	    public string ContainerDefaults { get; set; }
        [ForeignKey("DefaultCurrency")]
        [Column("DefaultCurrencyId")]
	    public string DefaultCurrencyId { get; set; }
	      
        public virtual Currency DefaultCurrency { get; set; }
        [ForeignKey("UnitOfMeasurement")]
        [Column("UnitOfMeasurementCode")]
	    public string UnitOfMeasurementCode { get; set; }
	      
        public virtual WeightUnit UnitOfMeasurement { get; set; }
    }
}
	 