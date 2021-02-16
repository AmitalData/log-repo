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
   
    public class ExportDeclarationClosingData
    {
	 string dbms;

           [Column("FinalThirdCargoId")]
	    public string FinalThirdCargoId { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [Column("DeclarationId")]
	    public string DeclarationId { get; set; }
        [ForeignKey("FinalCargoType")]
        [Column("FinalCargoTypeCode")]
	    public string FinalCargoTypeCode { get; set; }
	      
        public virtual CargoIdentifireType FinalCargoType { get; set; }
        [Column("FinalManifestNumber")]
	    public string FinalManifestNumber { get; set; }
        [Column("FinalSecondCargoId")]
	    public string FinalSecondCargoId { get; set; }
        [Column("LoadingDateTime")]
	    public DateTime? LoadingDateTime { get; set; }
        [ForeignKey("FinalCustomsShip")]
        [Column("FinalShipCode")]
	    public string FinalShipCode { get; set; }
	      
        public virtual CustomsShip FinalCustomsShip { get; set; }
        [ForeignKey("FinalLoadingSiteType")]
        [Column("FinalLoadingSite")]
	    public string FinalLoadingSite { get; set; }
	      
        public virtual LoadingSiteType FinalLoadingSiteType { get; set; }
    }
}
	 