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
        [ForeignKey("DeclarationEntity")]
        [Column("DeclarationId")]
	    public string DeclarationId { get; set; }
	      
        public virtual Declaration DeclarationEntity { get; set; }
        [Column("ExportFileNo")]
	    public string ExportFileNo { get; set; }
        [Column("StorageStatus")]
	    public string StorageStatus { get; set; }
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
        [ForeignKey("CustomsCargoStatus")]
        [Column("CustomsStatus")]
	    public string CustomsStatus { get; set; }
	      
        public virtual CargoStatus CustomsCargoStatus { get; set; }
        [Column("ExporterID")]
	    public string ExporterID { get; set; }
        [ForeignKey("CustomsShipCode")]
        [Column("ShipCode")]
	    public string ShipCode { get; set; }
	      
        public virtual CustomsShip CustomsShipCode { get; set; }
        [Column("FirstCargoID")]
	    public string FirstCargoID { get; set; }
        [Column("SecondCargoID")]
	    public string SecondCargoID { get; set; }
        [Column("ThirdCargoID")]
	    public string ThirdCargoID { get; set; }
        [Column("StorErrorXML")]
	    public string StorErrorXML { get; set; }
        [Column("StorageNo")]
	    public string StorageNo { get; set; }
        [Column("ExportDealIdentification")]
	    public string ExportDealIdentification { get; set; }
        [Column("PackageQuantity")]
	    public decimal? PackageQuantity { get; set; }
        [Column("GrossMassMeasure")]
	    public decimal? GrossMassMeasure { get; set; }
        [Column("MarksNumbers")]
	    public string MarksNumbers { get; set; }
        [ForeignKey("UnloadingSiteType")]
        [Column("ExportLoadingPortcode")]
	    public string ExportLoadingPortcode { get; set; }
	      
        public virtual UnloadingSiteType UnloadingSiteType { get; set; }
        [ForeignKey("DeliverySiteType")]
        [Column("StorageSiteCode")]
	    public string StorageSiteCode { get; set; }
	      
        public virtual DeliverySiteType DeliverySiteType { get; set; }
        [ForeignKey("InternationalSite")]
        [Column("ExportUnloadingPortCode")]
	    public string ExportUnloadingPortCode { get; set; }
	      
        public virtual InternationalSite InternationalSite { get; set; }
        [ForeignKey("InternationalSiteS")]
        [Column("FinalDestinationPortCode")]
	    public string FinalDestinationPortCode { get; set; }
	      
        public virtual InternationalSite InternationalSiteS { get; set; }
        [Column("IsDangerousGoods")]
	    public decimal? IsDangerousGoods { get; set; }
        [ForeignKey("ExportLogisticPermitAction")]
        [Column("ActionCode")]
	    public string ActionCode { get; set; }
	      
        public virtual ExportLogisticPermitAction ExportLogisticPermitAction { get; set; }
        [Column("ContainerTypeWCO")]
	    public string ContainerTypeWCO { get; set; }
    }
}
	 