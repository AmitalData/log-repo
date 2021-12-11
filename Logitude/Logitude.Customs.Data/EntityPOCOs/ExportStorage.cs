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
   
    public class ExportStorage
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("DeclarationId")]
	    public string DeclarationId { get; set; }
        [Column("ExportFileNo")]
	    public string ExportFileNo { get; set; }
        [Column("StorageNo")]
	    public int StorageNo { get; set; }
        [ForeignKey("CustomsStorageStatus")]
        [Column("StorageStatus")]
	    public string StorageStatus { get; set; }
	      
        public virtual StorageStatus CustomsStorageStatus { get; set; }
        [ForeignKey("CargoIdentifireType")]
        [Column("CargoTypeCode")]
	    public string CargoTypeCode { get; set; }
	      
        public virtual CargoIdentifireType CargoIdentifireType { get; set; }
        [Column("OpenDate")]
	    public DateTime OpenDate { get; set; }
        [ForeignKey("CargoTypeEntity")]
        [Column("CargoType")]
	    public string CargoType { get; set; }
	      
        public virtual CargoType CargoTypeEntity { get; set; }
        [Column("CustomsStatus")]
	    public string CustomsStatus { get; set; }
        [Column("ExporterID")]
	    public string ExporterID { get; set; }
        [ForeignKey("CustomsShip")]
        [Column("ShipCode")]
	    public string ShipCode { get; set; }
	      
        public virtual CustomsShip CustomsShip { get; set; }
        [Column("Manifest")]
	    public string Manifest { get; set; }
        [Column("SecondCargoID")]
	    public string SecondCargoID { get; set; }
        [Column("ThirdCargoID")]
	    public string ThirdCargoID { get; set; }
        [Column("ExportDealIdentification")]
	    public decimal ExportDealIdentification { get; set; }
    }
}
	 