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
   
    public class GatepassRequest
    {
	 string dbms;

        [Key]
        [Column("MasterCourierId" ,Order = 1)]
	    public string MasterCourierId { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [Column("GatepassNumber" ,Order = 2)]
	    public int GatepassNumber { get; set; }
        [ForeignKey("OriginSiteType")]
        [Column("OriginSiteCode")]
	    public string OriginSiteCode { get; set; }
	      
        public virtual SiteLookup OriginSiteType { get; set; }
        [ForeignKey("UpdateCodeType")]
        [Column("UpdateCode")]
	    public string UpdateCode { get; set; }
	      
        public virtual UpdateCode UpdateCodeType { get; set; }
        [ForeignKey("DesignateSiteType")]
        [Column("DesignateSiteCode")]
	    public string DesignateSiteCode { get; set; }
	      
        public virtual SiteLookup DesignateSiteType { get; set; }
        [ForeignKey("TransferCargoMethodType")]
        [Column("TransportationTypeCode")]
	    public string TransportationTypeCode { get; set; }
	      
        public virtual TransferCargoMethodType TransferCargoMethodType { get; set; }
        [Column("GatepassRequestStatus")]
	    public string GatepassRequestStatus { get; set; }
        [Column("CustomsUpdateDateTime")]
	    public DateTime? CustomsUpdateDateTime { get; set; }
    }
}
	 