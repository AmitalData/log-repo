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
        [ForeignKey("DeclarationStatusType")]
        [Column("HataraStatus")]
	    public string HataraStatus { get; set; }
	      
        public virtual DeclarationStatusType DeclarationStatusType { get; set; }
        [ForeignKey("NDMessageActionCode")]
        [Column("OperationMode")]
	    public string OperationMode { get; set; }
	      
        public virtual NDMessageActionCode NDMessageActionCode { get; set; }
    }
}
	 