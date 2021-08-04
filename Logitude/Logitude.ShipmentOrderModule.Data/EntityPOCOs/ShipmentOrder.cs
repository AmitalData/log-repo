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

namespace Logitude.ShipmentOrderModule.Data.EntityPOCOs
{
   
    public class ShipmentOrder
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("OrderNumber")]
	    public string OrderNumber { get; set; }
        [ForeignKey("TransportMode")]
        [Column("TransportModeId")]
	    public string TransportModeId { get; set; }
	      
        public virtual TransportMode TransportMode { get; set; }
        [ForeignKey("ConsigneeCard")]
        [Column("ConsigneeId")]
	    public string ConsigneeId { get; set; }
	      
        public virtual Card ConsigneeCard { get; set; }
        [ForeignKey("ShipperCard")]
        [Column("ShipperId")]
	    public string ShipperId { get; set; }
	      
        public virtual Card ShipperCard { get; set; }
        [ForeignKey("AgentCard")]
        [Column("AgentId")]
	    public string AgentId { get; set; }
	      
        public virtual Card AgentCard { get; set; }
        [ForeignKey("Incoterm")]
        [Column("IncotermId")]
	    public string IncotermId { get; set; }
	      
        public virtual Incoterm Incoterm { get; set; }
        [ForeignKey("AccountManagerUser")]
        [Column("AccountManagerId")]
	    public string AccountManagerId { get; set; }
	      
        public virtual User AccountManagerUser { get; set; }
        [Column("PONumber")]
	    public string PONumber { get; set; }
        [Column("DescriptionofGoods")]
	    public string DescriptionofGoods { get; set; }
        [ForeignKey("ShipmentType")]
        [Column("ShipmentTypeId")]
	    public string ShipmentTypeId { get; set; }
	      
        public virtual ShipmentType ShipmentType { get; set; }
        [Column("House")]
	    public string House { get; set; }
        [ForeignKey("Vessel")]
        [Column("VesselId")]
	    public string VesselId { get; set; }
	      
        public virtual Vessel Vessel { get; set; }
        [ForeignKey("CustomsAgentCard")]
        [Column("CustomsAgentId")]
	    public string CustomsAgentId { get; set; }
	      
        public virtual Card CustomsAgentCard { get; set; }
        [ForeignKey("SpecialServicesType")]
        [Column("SpecialServicesTypeId")]
	    public string SpecialServicesTypeId { get; set; }
	      
        public virtual SpecialServicesType SpecialServicesType { get; set; }
        [Column("CustomerRefrences")]
	    public string CustomerRefrences { get; set; }
    }
}
	 