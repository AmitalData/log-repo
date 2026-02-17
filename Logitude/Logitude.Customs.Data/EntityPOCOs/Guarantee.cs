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
   
    public class Guarantee
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("Tapag")]
        [Column("TapagID")]
	    public string TapagID { get; set; }
	      
        public virtual Tapag Tapag { get; set; }
        [ForeignKey("RequestStatus")]
        [Column("GuaranteeRequestStatusCode")]
	    public string GuaranteeRequestStatusCode { get; set; }
	      
        public virtual RequestStatus RequestStatus { get; set; }
        [Column("GuaranteeRequestNumber")]
	    public string GuaranteeRequestNumber { get; set; }
        [Column("NumeralRequest")]
	    public string NumeralRequest { get; set; }
        [Column("MsgID")]
	    public string MsgID { get; set; }
        [ForeignKey("ClientActivityType")]
        [Column("ClientActivityCode")]
	    public string ClientActivityCode { get; set; }
	      
        public virtual GuaranteeCustomerActivity ClientActivityType { get; set; }
        [ForeignKey("EntityTypeLookup")]
        [Column("CustomEntityTypeCode")]
	    public string CustomEntityTypeCode { get; set; }
	      
        public virtual EntityTypeLookup EntityTypeLookup { get; set; }
        [Column("CustomEntityNumber")]
	    public string CustomEntityNumber { get; set; }
        [Column("RequestValidityDate")]
	    public DateTime? RequestValidityDate { get; set; }
        [Column("GuaranteeValidityDate")]
	    public DateTime? GuaranteeValidityDate { get; set; }
        [Column("BrandNumber")]
	    public string BrandNumber { get; set; }
        [Column("LawyerNumber")]
	    public string LawyerNumber { get; set; }
        [Column("BirthDate")]
	    public DateTime? BirthDate { get; set; }
        [Column("VehicleChassisNumber")]
	    public string VehicleChassisNumber { get; set; }
        [Column("EngineNumber")]
	    public string EngineNumber { get; set; }
        [Column("UpdateDate")]
	    public DateTime? UpdateDate { get; set; }
        [Column("GuaranteeExternalNumber")]
	    public string GuaranteeExternalNumber { get; set; }
    }
}
	 