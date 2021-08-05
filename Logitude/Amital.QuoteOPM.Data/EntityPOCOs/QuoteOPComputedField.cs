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

namespace Amital.QuoteOPM.Data.EntityPOCOs
{
   
    public class QuoteOPComputedField
    {
	 string dbms;

        [Key]
        [ForeignKey("QuoteOP")]
        [Column("Id")]
	    public string Id { get; set; }
	      
        public virtual QuoteOP QuoteOP { get; set; }
        [Column("ConnectedToShipment")]
	    public bool ConnectedToShipment { get; set; }
        [Column("ConnectedToTicket")]
	    public bool ConnectedToTicket { get; set; }
        [Column("ToLocation")]
	    public string ToLocation { get; set; }
        [Column("FromLocation")]
	    public string FromLocation { get; set; }
        [Column("DeliveryTo")]
	    public string DeliveryTo { get; set; }
        [Column("PickupFrom")]
	    public string PickupFrom { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("EstimatedPayablesInSales")]
	    public double? EstimatedPayablesInSales { get; set; }
        [Column("EstimatedPayablesInLocal")]
	    public double? EstimatedPayablesInLocal { get; set; }
        [Column("EstimatedReceivablesInLocal")]
	    public double? EstimatedReceivablesInLocal { get; set; }
        [Column("EstimatedReceivablesInSales")]
	    public double? EstimatedReceivablesInSales { get; set; }
        [Column("AutomaticLastUpdateDate")]
	    public DateTime? AutomaticLastUpdateDate { get; set; }
    }
}
	 