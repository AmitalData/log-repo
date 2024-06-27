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
   
    public class Containerization
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("AgentDeclaration")]
	    public bool AgentDeclaration { get; set; }
        [Column("ContainerizationDate")]
	    public DateTime ContainerizationDate { get; set; }
        [Column("ContainerizationNumber")]
	    public string ContainerizationNumber { get; set; }
        [ForeignKey("ContainerizationStatusCode")]
        [Column("ContainerizationStatus")]
	    public string ContainerizationStatus { get; set; }
	      
        public virtual ContainerizationStatusCode ContainerizationStatusCode { get; set; }
        [ForeignKey("ContainerizationHataraStatus")]
        [Column("HataraStatus")]
	    public string HataraStatus { get; set; }
	      
        public virtual ContainerizationHataraStatus ContainerizationHataraStatus { get; set; }
        [ForeignKey("NDMessageActionCode")]
        [Column("OperationMode")]
	    public string OperationMode { get; set; }
	      
        public virtual NDMessageActionCode NDMessageActionCode { get; set; }
        [Column("ExportFile")]
	    public string ExportFile { get; set; }
        [Column("IsChange")]
	    public bool IsChange { get; set; }
        [Column("IsMultiCustomers")]
	    public string IsMultiCustomers { get; set; }
        [Column("IsMultiExportFiles")]
	    public bool? IsMultiExportFiles { get; set; }
        [ForeignKey("CargoType")]
        [Column("CargoTypeCode")]
	    public string CargoTypeCode { get; set; }
	      
        public virtual CargoIdentifireType CargoType { get; set; }
        [Column("ManifestNumber")]
	    public string ManifestNumber { get; set; }
        [Column("SecondCargoID")]
	    public string SecondCargoID { get; set; }
        [Column("ThirdCargoID")]
	    public string ThirdCargoID { get; set; }
        [ForeignKey("TransportMode")]
        [Column("TransportModeId")]
	    public string TransportModeId { get; set; }
	      
        public virtual TransportMode TransportMode { get; set; }
        [Column("ExistInCustoms")]
	    public string ExistInCustoms { get; set; }
    }
}
	 